using Microsoft.EntityFrameworkCore;
using NewsAPIParser;
using Newtonsoft.Json.Linq;
using System;

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
            // создание экз класса и наполение его данными статьи
            Article article = new Article()
            {
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
            int addedArticles = 0;
            int matchesFound = 0;

            foreach (var item in articleListFromAPI)
            {
                // проверяем заголовок каждой статьи из списка на наличие такой же в базе по index полю
                var existArticle = db.Articles.FirstOrDefault(a => a.Title == item.Title);
                if (existArticle == null)
                {
                    db.Articles.Add(item);
                    addedArticles++;
                }
                else matchesFound++;
            }

            // сохраняем внесейнные данные в самой базе
            db.SaveChanges();

            Console.WriteLine($"Added to DB {addedArticles} articles.");
            Console.WriteLine($"Had find matches in DB for {matchesFound} articles.");
            Console.WriteLine($"Current quantity of articles in DB: {db.Articles!.Count()}");
        }
    }
}