using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using NewsAPIParser;
using Newtonsoft.Json.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        var key = "d172fecfb2a84ede9339e513fb4fea35";
        var country = "us";

        var url = $"https://newsapi.org/v2/top-headlines?country={country}&apiKey={key}";
        //https://newsapi.org/v2/top-headlines?country=us&apiKey=d172fecfb2a84ede9339e513fb4fea35

        var request = new GetRequest(url);
        request.Run();

        //передаём результат в новую переменную
        string responce = request.Responce;

        //парсим строку в json file с помощью Newtonsoft.Json
        JObject jsonResult = JObject.Parse(responce);

        //получаем массив статей JToken
        JToken? articles = jsonResult["articles"];

        List<Article> articleListFromAPI = new List<Article>();

        //По каждой статье создаём экземпляр класса Article и помещаем его в базу
        foreach (var item in articles)
        {
            //создание экз класса и наполение его данными статьи, добавление ArticleId
            Article article = new Article()
            {
                //ArticleId = null,
                Source = (string?)item["source"]?["name"],
                Author = (string?)item["author"] ?? "Unknown",
                Title = (string?)item["title"] ?? "Unknown",
                Description = (string?)item["description"] ?? "Unknown",
                Url = (string?)item["url"] ?? "Unknown",
                UrlToImage = (string?)item["urlToImage"] ?? "Unknown",
                PublishedAt = (DateTime?)item["publishedAt"],
                Content = (string?)item["content"] ?? "Unknown"
            };


            articleListFromAPI.Add(article);

            Console.WriteLine("\t***");
            Console.WriteLine("Id: " + article.Id);
            Console.WriteLine("Source: " + article.Source);
            Console.WriteLine("Author: " + article.Author);
            Console.WriteLine("Title: " + article.Title);
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            // получаем текущий список статей из базы данных
            DbSet<Article> listArticlesInDb = db.Articles;
            // создаём список всех заголовков статей из базы данных
            List<string> titlesArticlesFromDb = new List<string>();

            // начинаем просматривать все статьи в базе
            foreach (var article in listArticlesInDb)
            {
                //добавляем title в список для дальнейшего стравнения при добавлении статьи
                titlesArticlesFromDb.Add(article.Title);
            }

            // запись полученных статей в БД
            foreach (var article in articleListFromAPI)
            {
                // получаем обрезанную версию заголовка полученной статьи из API 
                string shortTitle = article.Title.Substring(0, 10);

                // вводим переменную для определения совпадений при поиске, если count>0 записывать статью в базу не нужно
                int count = 0;

                // пробегаемся по списку заголовков из БД и ищем совпадения, не начинается ли заголовок из базы
                // на нашу обрезанную версию заголовка полученной статьи
                for (int i = 0; i < titlesArticlesFromDb.Count; i++)
                {
                    // если короткий заголовок находится в базе, плюсуем счетчик
                    if (titlesArticlesFromDb[i].StartsWith($"{shortTitle}"))
                        count++;
                }

                // в случае если совпадений нет помещаем статью в базу
                if (count == 0)
                {
                    db.Articles.Add(article);
                }

            }

            // сохраняем внесейнные данные в самой базе
            db.SaveChanges();

            Console.WriteLine("Данные успешно добавлены в базу!");
            Console.WriteLine($"Текущее количество статей в базе данных: {listArticlesInDb.Count()}");
        }
    }
}