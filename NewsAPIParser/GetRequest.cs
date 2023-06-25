using System.Net;

namespace NewsAPIParser
{
    public class GetRequest
    {
        readonly string? _address;
        
        public string? Responce { get; set; }

        // конструктор получает адрес
        public GetRequest(string? address)
        {
            _address = address;
        }

        // метод для запуска реквеста
        public void Run()
        {
            HttpWebRequest _request;

            // создание запроса
            _request = (HttpWebRequest)WebRequest.Create(_address);

            // указание метода запроса
            _request.Method = "GET";

            // обходим защиту от анонимных запросов на портале newsAPI
            _request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 OPR/97.0.0.0");//указание заголовка User agent, обход защиты от no-name запросов

            try
            {
                // создаём объект ответа web-сервера
                HttpWebResponse response = (HttpWebResponse)_request.GetResponse();

                // создайм поток для чтения ответа от сервера
                var stream = response.GetResponseStream();

                // запись ответа от сервера в свойство Responce
                if (stream != null)
                {
                    Responce = new StreamReader(stream).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
