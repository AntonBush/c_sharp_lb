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

Console.WriteLine("App: Magashop");

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

        override public string ToString()
        {
            return $"name: {name}; address: {address}; discount: {discount}";
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

        override public string ToString()
        {
            return $"title: {title}; price: {price}";
        }
    }

    class ProductDatabase
    {
        public Dictionary<string, Product> data;
    }

    class OrderLine
    {
        public int count { get; }
        public Product product { get; }

        public OrderLine(int count, Product product)
        {
            this.count = count;
            this.product = product;
        }

        override public string ToString()
        {
            return $"count: {count}; product: [{product}]";
        }
    }

    class Order
    {
        public int number { get; }
        public Customer customer { get; }
        public decimal discount
        {
            get { return (_calculateTotalCost() * (decimal)customer.discount); }
        }
        public decimal cost
        {
            get { return (_calculateTotalCost() * (decimal)(1 - customer.discount)); }
        }
        public List<OrderLine> lines { get { return new List<OrderLine>(_lines); } }

        public Order(Customer customer)
        {
            number = _number_counter++;
            this.customer = customer;
            _lines = new List<OrderLine>();
        }

        public void addLine(OrderLine line)
        {
            _lines.Add(line);
        }

        override public string ToString()
        {
            string temp = $"number: [{number}]\n";
            temp += $"customer: [{customer}]\n";
            temp += $"discount: [{discount}]\n";
            temp += $"cost: [{cost}]\n";
            temp += $"lines: [{_lines.Count}]\n";
            foreach (var line in _lines)
            {
                temp += $"  {line}\n";
            }
            return temp;
        }

        static int _number_counter = 0;
        List<OrderLine> _lines;

        decimal _calculateTotalCost()
        {
            decimal cost = 0;
            foreach (var line in _lines)
            {
                cost += line.count * line.product.price;
            }
            return cost;
        }
    }
}
