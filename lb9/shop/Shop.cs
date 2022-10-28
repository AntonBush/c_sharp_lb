using System.Text.Json;

namespace lb9.shop;

public class Customer
{
    public class JsonDeserializable
    {
        public string? name { get; set; }
        public string? address { get; set; }
        public Nullable<double> discount { get; set; }
    }

    public string name { get; }
    public string address { get; }
    public double discount { get; }

    public static Customer? from(JsonDeserializable? jd)
    {
        if (jd == null
            || jd.name == null
            || jd.address == null
            || !jd.discount.HasValue
            )
        {
            return null;
        }
        return new Customer(jd.name, jd.address, jd.discount.Value);
    }
    public static Customer? from(string s)
    {
        return from(JsonSerializer.Deserialize<JsonDeserializable>(s));
    }

    public Customer(string name, string address, double discount)
    {
        this.name = name;
        this.address = address;
        this.discount = discount;
    }

    override public string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class Product
{
    public class JsonDeserializable
    {
        public string? title { get; set; }
        public Nullable<decimal> price { get; set; }
    }

    public string title { get; }
    public decimal price { get; }

    public static Product? from(JsonDeserializable? jd)
    {
        if (jd == null
            || jd.title == null
            || !jd.price.HasValue
            )
        {
            return null;
        }
        return new Product(jd.title, jd.price.Value);
    }

    public static Product? from(string s)
    {
        return from(JsonSerializer.Deserialize<JsonDeserializable>(s));
    }

    public Product(string title, decimal price)
    {
        this.title = title;
        this.price = price;
    }

    override public string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class ProductDatabase
{
    public static ProductDatabase? from(string s)
    {
        var jd = JsonSerializer.Deserialize<Dictionary<string, Product.JsonDeserializable?>>(s);
        if (jd == null)
        {
            return null;
        }
        try
        {
            var data = jd.Select((key_value) =>
            {
                var (str, prod_jd) = key_value;
                if (str == null)
                {
                    throw new Exception();
                }
                var product = Product.from(prod_jd);
                if (product == null)
                {
                    throw new Exception();
                }
                return new KeyValuePair<string, Product>(str, product);
            }).ToDictionary((x) => x.Key, (x) => x.Value);
            return new ProductDatabase(data);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public ProductDatabase()
    {
        _data = new Dictionary<string, Product>();
    }

    public void add(Product product)
    {
        if (_data.Count == 26 * 10 * 10 * 10)
        {
            throw new Exception("Database is full.");
        }
        string key;
        do
        {
            var letter = (char)('A' + _random.Next(26));
            var number = _random.Next(1000).ToString("D3");
            key = $"{letter}{number}";
        } while (_data.ContainsKey(key));
        _data.Add(key, product);
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(_data);
    }

    static Random _random = new Random();
    Dictionary<string, Product> _data;

    ProductDatabase(Dictionary<string, Product> data)
    {
        _data = data;
    }
}

public class OrderLine
{
    public class JsonDeserializable
    {
        public Nullable<int> count { get; set; }
        public Product.JsonDeserializable? product { get; set; }
    }

    public int count { get; }
    public Product product { get; }

    public static OrderLine? from(JsonDeserializable? jd)
    {
        if (jd == null
            || !jd.count.HasValue
            )
        {
            return null;
        }
        var product = Product.from(jd.product);
        if (product == null)
        {
            return null;
        }

        return new OrderLine(jd.count.Value, product);
    }

    public static OrderLine? from(string s)
    {
        return from(JsonSerializer.Deserialize<JsonDeserializable>(s));
    }

    public OrderLine(int count, Product product)
    {
        this.count = count;
        this.product = product;
    }

    override public string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

public class Order
{
    public class JsonDeserializable
    {
        public Nullable<int> number { get; set; }
        public Customer.JsonDeserializable? customer { get; set; }
        public List<OrderLine.JsonDeserializable?>? lines { get; set; }
    }

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

    public static Order? from(JsonDeserializable? jd)
    {
        if (jd == null
            || !jd.number.HasValue
            || jd.customer == null
            || jd.lines == null
            )
        {
            return null;
        }
        var number = jd.number.Value;
        var customer = Customer.from(jd.customer);
        if (customer == null)
        {
            return null;
        }
        try
        {
            var lines = jd.lines.Select((ord_line_jd) =>
            {
                if (ord_line_jd == null)
                {
                    throw new Exception();
                }
                var order_line = OrderLine.from(ord_line_jd);
                if (order_line == null)
                {
                    throw new Exception();
                }
                return order_line;
            }).ToList();
            return new Order(number, customer, lines);
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static Order? from(string s)
    {
        return from(JsonSerializer.Deserialize<JsonDeserializable>(s));
    }

    public Order(Customer customer)
    {
        number = _number_counter++;
        this.customer = customer;
        _lines = new List<OrderLine>();
    }

    public void add(OrderLine line)
    {
        _lines.Add(line);
    }

    override public string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    static int _number_counter = 0;
    List<OrderLine> _lines;

    Order(int number, Customer customer, List<OrderLine> lines)
    {
        this.number = number;
        this.customer = customer;
        _lines = lines;
    }

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
