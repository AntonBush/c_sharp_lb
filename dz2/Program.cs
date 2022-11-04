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
        _port = 8080;
    }
    public void start()
    {
        _listener = new TcpListener(IPAddress.Any, _port);

        try
        {
            _listener.Start();
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

        // получаем сетевой поток для чтения и записи
        using (var stream = client.GetStream())
        {

            // сообщение для отправки клиенту
            string body = File.ReadAllText(".\\dz2\\index.html");
            string headers = $"HTTP/1.1 200 OK\nContent-Type: text/html; charset=utf-8\nContent-Length: {Encoding.UTF8.GetByteCount(body)}\n\n";
            string response = headers + body;
            // преобразуем сообщение в массив байтов
            byte[] data = Encoding.UTF8.GetBytes(response);

            // отправка сообщения
            stream.Write(data, 0, data.Length);
            stream.Flush();
            Console.WriteLine("Отправлено сообщение: {0}", response);
        }

        // закрываем подключение
        client.Close();
    }
}
