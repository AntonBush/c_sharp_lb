/*

Задание: 
Требуется разработать программу, ведущую учёт заказов в магазине.
Классы: - (Покупатель) с тремя атрибутами: имя (string), адрес (string), скидка (double)
- (Товар). Поля, соответствующие названию (string) и цене (decimal)
- (База данных товаров), хранящий ассоциативный массив («словарь») с информацией о 
товарах
- OrderLine с полями количество (int) и продукт (Product)
- Order с полями номер заказа (int), клиент (Customer), скидка (decimal), общая стоимость 
(decimal) и строки заказа (List<OrderLIne>).
Реализовать следующую логику основной программы:
1. Создаётся и заполняется база данных товаров (ассоциативный массив).
2. В консоли вводятся данные по конкретному покупателю, создаётся 
соответствующий объект.
3. Создаётся заказ для введённого ранее покупателя. Устанавливается 
скидка на заказ в соответствии со скидкой покупателя.
4. В цикле формируются необходимое количество строк заказа: вводятся 
коды товаров и количества их единиц.
5. Полная информация о заказе сохраняется в файле с заданным именем.
Создать методы, которые осуществляют сериализацию/десериализацию 
объекта типа База данных товаров. Формат выбрать самостоятельно.

*/

using shop = lb9.shop;

Console.WriteLine("App: Magashop");

if (args.Length < 1)
{
    throw new Exception("First parameter must be path to DB.");
}

var db = shop.ProductDatabase.from(File.ReadAllText(args[0]));
if (db == null)
{
    throw new Exception("DB is corrupted.");
}

Console.WriteLine("Loaded DB:");
Console.WriteLine(db);

var customer = registerCustomer();
var order = makeOrder(customer, db);

var path = readLine("Enter path:");
Console.WriteLine($"Entered: {path}");

File.WriteAllText(path, order.ToString());
Console.WriteLine("App: closed");

shop.Customer registerCustomer()
{
    var customer = new shop.Customer(readLine("Enter customer name:").Trim()
                                    , readLine("Enter customer address:").Trim()
                                    , double.Parse(readLine("Enter customer discount:"))
                                    );
    Console.WriteLine($"Entered: {customer}");
    return customer;
}

shop.Order makeOrder(shop.Customer customer, shop.ProductDatabase db)
{
    var order = new shop.Order(customer);

    var input = readLine("Enter product key or empty line:").Trim();
    while (input != "")
    {
        try
        {
            var product = db.at(input);
            var count = int.Parse(readLine("Enter number of products:"));
            order.add(new shop.OrderLine(count, product));
            input = readLine("Enter product key or empty line:").Trim();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    Console.WriteLine($"Entered: {order}");
    return order;
}

string readLine(string? message)
{
    Console.WriteLine(message);
    var str = Console.ReadLine();
    if (str == null)
    {
        throw new Exception("Nothing was read");
    }
    return str;
}
