/*

Задание:
Нужно реализовать http веб-сервер на языке C#, который будет выдавать html
страницу. Варианты:
!* Четные по списку - Реализовать сервер на Pool Thread
Нечетные по списку - Реализовать сервер на Thread per Request

*/

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

int max_n_threads = Environment.ProcessorCount * 4;
ThreadPool.SetMaxThreads(max_n_threads, max_n_threads);
ThreadPool.SetMinThreads(2, 2);

var server = new Server();
Console.WriteLine("Запуск сервера...");
server.start();

class Server
{
    public Server()
    {
        _port = 1313;
    }
    public void start()
    {
        _listener = new TcpListener(IPAddress.Any, _port);

        try
        {
            _listener.Start();
            Console.WriteLine("Сервер запущен.");
        }
        catch (Exception)
        {
            throw;
        }
        while (true)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(obj => new Worker((TcpClient?)obj)), _listener.AcceptTcpClient());
        }
    }

    int _port { get; }
    TcpListener? _listener;

    ~Server()
    {
        _listener?.Stop();
    }
}

class Worker
{
    public Worker(TcpClient? client)
    {
        if (client == null)
        {
            Console.WriteLine("Client == null");
            return;
        }

        // получаем входящее подключение
        Console.WriteLine("Подключен клиент. Выполнение запроса...");

        using var _client = client;
        // получаем сетевой поток для чтения и записи
        using var stream = client.GetStream();

        if (client.ReceiveBufferSize == 0)
        {
            _error(stream);
            return;
        }

        var bytes = new byte[client.ReceiveBufferSize];
        stream.Read(bytes, 0, client.ReceiveBufferSize);
        var msg = Encoding.ASCII.GetString(bytes); //the message incoming
        // Console.WriteLine(msg);
        var request_str = msg
                            .Trim()
                            .Split("\n")
                            .FirstOrDefault((string?)null);

        if (request_str == null)
        {
            _error(stream);
            return;
        }

        var request_split = request_str.Split();

        if (request_split.Length < 2)
        {
            _error(stream);
            return;
        }

        if (request_split[1] == "/favicon.ico")
        {
            _icon(stream);
            return;
        }

        try
        {
            var query_str = string.Join("", request_split[1].SkipWhile(ch => ch == '/' || ch == '?'));
            var query = HttpUtility.ParseQueryString(query_str);
            var input = query.Get("input");
            if (input != null)
            {
                _algorithm(string.Join("", input.Split()
                                            .Select(s => s.Trim())
                                            .Where(s => s != "")), stream);
            }
            else
            {
                _index(stream);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _error(stream);
            return;
        }
    }

    void _index(Stream stream)
    {
        // сообщение для отправки клиенту
        string body = File.ReadAllText(".\\dz3\\index.html");
        string headers = $"HTTP/1.1 200 OK\nContent-Type: text/html; charset=utf-8\nContent-Length: {Encoding.UTF8.GetByteCount(body)}\n\n";
        string response = headers + body;
        // преобразуем сообщение в массив байтов
        byte[] data = Encoding.UTF8.GetBytes(response);

        // отправка сообщения
        stream.Write(data, 0, data.Length);
        stream.Flush();
        // Console.WriteLine("Отправлено сообщение: {0}", response);
    }

    void _algorithm(string input, Stream stream)
    {
        var json = new JsonData();
        try
        {
            json.result = dz1.utility.Utility.algorithm(input);
        }
        catch (Exception e)
        {
            json.result = e.Message;
        }
        var body = JsonSerializer.Serialize(json);
        var headers = $"HTTP/1.1 200 OK\nContent-Type: text/html; charset=utf-8\nContent-Length: {Encoding.UTF8.GetByteCount(body)}\n\n";
        var response = headers + body;

        // преобразуем сообщение в массив байтов
        byte[] data = Encoding.UTF8.GetBytes(response);
        // отправка сообщения
        stream.Write(data, 0, data.Length);
        stream.Flush();
        // Console.WriteLine("Отправлено сообщение: {0}", response);
    }

    void _icon(Stream stream)
    {
        var headers = $"HTTP/1.1 200 OK\n\n";
        var response = headers;

        // преобразуем сообщение в массив байтов
        byte[] data = Encoding.UTF8.GetBytes(response);
        // отправка сообщения
        stream.Write(data, 0, data.Length);
        stream.Flush();
        // Console.WriteLine("Отправлено сообщение: {0}", response);
    }

    void _error(Stream stream)
    {
        var headers = $"HTTP/1.1 400 Bad Request\n\n";
        var response = headers;

        // преобразуем сообщение в массив байтов
        byte[] data = Encoding.UTF8.GetBytes(response);
        // отправка сообщения
        stream.Write(data, 0, data.Length);
        stream.Flush();
        // Console.WriteLine("Отправлено сообщение: {0}", response);
    }
}

class JsonData
{
    public string result { get; set; } = "";
}
