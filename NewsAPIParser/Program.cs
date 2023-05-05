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

        // передаём результат запроса в виде строки в новую строковую переменную 
        string responce = request.Responce!;

        // парсим строку в json file с помощью Newtonsoft.Json
        JObject jsonResult = JObject.Parse(responce!);

        // получаем массив статей JToken
        JToken? articles = jsonResult["articles"];

        // пустой лист статей для помещения туда информации из JToken
        List<Article> articleListFromAPI = new List<Article>();

        // По каждой статье создаём экземпляр класса Article и помещаем его в список статей articleListFromAPI
        foreach (var item in articles!)
        {
            // создание экз класса и наполение его данными статьи, добавление ArticleId
            Article article = new Article()
            {
                // ArticleId = null,
                Source = (string?)item["source"]?["name"],
                Author = (string?)item["author"] ?? "Unknown",
                Title = (string?)item["title"] ?? "Unknown",
                Description = (string?)item["description"] ?? "Unknown",
                Url = (string?)item["url"] ?? "Unknown",
                UrlToImage = (string?)item["urlToImage"] ?? "Unknown",
                PublishedAt = (DateTime?)item["publishedAt"],
                Content = (string?)item["content"] ?? "Unknown"
            };

            // добавляем статью в лист articleListFromAPI
            articleListFromAPI.Add(article);

            // для наглядности выводим с консоль некие результаты созданных статей 
            Console.WriteLine("\t***");
            Console.WriteLine("Source: " + article.Source);
            Console.WriteLine("Author: " + article.Author);
            Console.WriteLine("Title: " + article.Title);
        }

        // на этот момент у нас есть список articleListFromAPI<Article> со статьями полученными из API

        // для проверки дубликатов при записи в базу создаём спикок заголовков статей
        // в дальнейшем будем сравнивать по этим заголовками заголовки полученных статей, для избежания дублирования статей в базе
        using (ApplicationContext db = new ApplicationContext())
        {
            // получаем текущий список статей из базы данных
            DbSet<Article>? listArticlesInDb = db.Articles;
            // создаём список всех заголовков статей из базы данных
            List<string> titlesArticlesFromDb = new List<string>();

            // проверяем лист на наличие содержимого
            // если спикок не пуст
            if (listArticlesInDb != null)
            {
                // начинаем просматривать все статьи в базе
                foreach (var article in listArticlesInDb)
                {
                    // добавляем title в список для дальнейшего стравнения при добавлении статьи
                    titlesArticlesFromDb.Add(article.Title!);
                }
            }


            // проверка и запись полученных статей в БД 
            foreach (var article in articleListFromAPI)
            {
                // получаем обрезанную версию заголовка полученной статьи из API 
                string shortTitle = article.Title!.Substring(0, 10);

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
                    db.Articles.Add(article);
            }

            // сохраняем внесейнные данные в самой базе
            db.SaveChanges();

            Console.WriteLine("Данные успешно добавлены в базу!");
            Console.WriteLine($"Текущее количество статей в базе данных: {listArticlesInDb!.Count()}");
        }
    }
}