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

namespace lb9
{
    class Customer
    {
        public string name { get; }
        public string address { get; }
        public double discount { get; }

        Customer(string name, string address, double discount)
        {
            this.name = name;
            this.address = address;
            this.discount = discount;
        }
    }

    class Product
    {
        public string title { get; }
        public decimal price { get; }

        public Product(string title, decimal price)
        {
            this.title = title;
            this.price = price;
        }
    }

    class ProductDatabase
    {
        public Dictionary<?, Product>;
    }

    class OrderLine
    {
        public int count { get; }
        public Product product { get; }

        public Product(int count, Product product)
        {
            this.count = count;
            this.product = product;
        }
    }

    class Order
    {
        public int number { get; }
        public Customer customer { get; }
        public decimal discount { get; }
        public decimal cost { get; }
        public List<OrderLine> lines { get; }
    }
}
