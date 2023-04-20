using NewsAPIParser;
using Newtonsoft.Json.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        var key = "d172fecfb2a84ede9339e513fb4fea35";
        var country = "us";

        var url = $"https://newsapi.org/v2/top-headlines?country={country}&apiKey={key}";

        var request = new GetRequest(url);
        request.Run();

        //передаём результат в новую переменную
        string responce = request.Responce;

        //парсим строку в json file с помощью Newtonsoft.Json
        JObject jsonResult = JObject.Parse(responce);

        //получаем массив статей JToken
        JToken? articles = jsonResult["articles"];

        List<Article> articleList = new List<Article>();
 
        //По каждой статье создаём экземпляр класса Article и помещаем его в базу
        foreach (var item in articles)
        {
            //создание экз класса и наполение его данными статьи, добавление ArticleId
            Article article = new Article()
            {
                ArticleId = Guid.NewGuid().ToString("N"),
                Source = (string?)item["source"]?["name"],
                Author = (string?)item["author"] ?? "Unknown",
                Title = (string?)item["title"] ?? "Unknown",
                Description = (string?)item["description"] ?? "Unknown",
                Url = (string?)item["url"] ?? "Unknown",
                UrlToImage = (string?)item["urlToImage"] ?? "Unknown",
                PublishedAt = (DateTime?)item["publishedAt"],
                Content = (string?)item["content"] ?? "Unknown"
            };

            articleList.Add(article);

            Console.WriteLine("\t***");
            Console.WriteLine("GUID: " + article.ArticleId);
            Console.WriteLine("Source: " + article.Source);
            Console.WriteLine("Author: " + article.Author);
            Console.WriteLine("Title: " + article.Title);
        }

        using (ApplicationContext db = new ApplicationContext())
        {
            
            foreach (var item in articleList)
            {
                db.Articles.Add(item);
            }

            db.SaveChanges();
            Console.WriteLine("Данные успешно добавлены в базу!");
        }
    }
}