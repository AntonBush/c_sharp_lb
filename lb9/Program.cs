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
// var temp = new shop.ProductDatabase();
// temp.add(new shop.Product("Anton", 200));
// Console.WriteLine(temp);
// Console.WriteLine(shop.ProductDatabase.from("{\"Q205\":{\"title\":\"Anton\",\"price\":200}}"));
// Console.WriteLine(shop.ProductDatabase.from("{\"Q205\":{\"title\":\"Anton\"}}"));
var temp = new shop.Order(new shop.Customer("Anton", "Moscow", 0.2));
temp.add(new shop.OrderLine(10, new shop.Product("Kolbasa", 200)));
temp.add(new shop.OrderLine(20, new shop.Product("AK-47", 35000)));
Console.WriteLine(temp);
Console.WriteLine(shop.Order.from(temp.ToString()));
Console.WriteLine(temp.ToString() == shop.Order.from(temp.ToString())?.ToString());
Console.WriteLine(shop.Order.from(
"{ \"customer\":{ \"name\":\"Anton\",\"address\":\"Moscow\",\"discount\":0.2},\"discount\":140400.0,\"cost\":561600.0,\"lines\":[{ \"count\":10,\"product\":{ \"title\":\"Kolbasa\",\"price\":200} },{ \"count\":20,\"product\":{ \"title\":\"AK-47\",\"price\":35000} }]}"
));
