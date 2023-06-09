﻿using DBConnection.Models.Classes;
using DBConnection.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using NewsAPIParser;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static void Main(string[] args)
    {
        using (ApiDataConnectionContext apiDataConnection = new ApiDataConnectionContext())
        {
            #region настройки приложения
            // Ключ для разработки приложения для https://newsapi.org
            // API key: "d172fecfb2a84ede9339e513fb4fea35";
            // Основная часть ссылки портала https://newsapi.org
            // url: https://newsapi.org/v2/top-headlines?
            // Часть со страной добавляемая к основной ссылке 
            // part: country={country}&

            // https://newsapi.org/v2/top-headlines?country={country}&apiKey={key}
            // пример рабочей ссылки
            // https://newsapi.org/v2/top-headlines?country=us&apiKey=d172fecfb2a84ede9339e513fb4fea35

            #endregion

            // получаем данные подключения из базы
            var apiData = apiDataConnection.ApiData.SingleOrDefault();

            // собираем строку подключения через метод класса ApiData
            var url = apiData.GetUrl(apiDataConnection);

            var request = new GetRequest(url);
            request.Run();

            // передаём результат запроса в виде строки в новую строковую переменную 
            string responce = request.Responce!;

            // парсим строку в json file с помощью Newtonsoft.Json
            JObject jsonResult = JObject.Parse(responce!);

            // получаем массив статей JToken
            JToken? articles = jsonResult["articles"];

            // пустой лист статей для помещения туда информации из JToken
            List<Article> articleListFromAPI = new();

            // По каждой статье создаём экземпляр класса Article и помещаем его в список статей articleListFromAPI
            foreach (var item in articles!)
            {
                // создание экз класса и наполение его данными статьи
                Article article = new()
                {
                    Source = (string?)item["source"]?["name"],
                    Author = (string?)item["author"] ?? "",
                    Title = (string?)item["title"] ?? "",
                    Description = (string?)item["description"] ?? "",
                    Url = (string?)item["url"] ?? "",
                    UrlToImage = (string?)item["urlToImage"] ?? "",
                    PublishedAt = (DateTime?)item["publishedAt"],
                    Content = (string?)item["content"] ?? ""
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

            // для избежания дублирования статей в базе, перед добавлением статьи проверяем на совпадения
            using (NewsContext db = new())
            {
                int addedArticles = 0;
                int matchesFound = 0;

                foreach (var item in articleListFromAPI)
                {
                    // проверяем заголовок каждой статьи из списка на наличие такой же в базе по index полю
                    var existArticle = db.articles.FirstOrDefault(a => a.Title == item.Title);
                    if (existArticle == null)
                    {
                        db.articles.Add(item);
                        addedArticles++;
                    }
                    else matchesFound++;
                }

                // сохраняем внесейнные данные в самой базе
                db.SaveChanges();

                Console.WriteLine($"Added to DB {addedArticles} articles.");
                Console.WriteLine($"Had find matches in DB for {matchesFound} articles.");
                Console.WriteLine($"Current quantity of articles in DB: {db.articles!.Count()}");
            }
        }

    }
}