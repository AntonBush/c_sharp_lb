/*

Задание:
1. Создать класс Point - точка на плоскости с вещественными 
координатами x, y. Создать конструктор, ToString() и свойства для 
доступа к координатам точки.
2. Создайте метод, который генерирует набор (LIst<Point>) случайно 
расположенных точек в квадрате [0,1]x[0,1].
3. Используя интерфейс IComparer, выведите все точки, упорядочивая их 
следующими способами:
- по удалению от начала координат (сначала выводится ближайшая 
к началу координат, порядок равноудалённых точек не важен);
- по удалению от оси абсцисс (сначала выводится ближайшая к оси 
абсцисс, порядок равноудалённых точек не важен);
- по удалению от оси ординат (сначала выводится ближайшая к оси 
ординат, порядок равноудалённых точек не важен);
- по удалению от диагонали первой и третьей четвертей 
(прямая y=x, порядок равноудалённых точек не важен)

*/

Console.WriteLine("App: Random points");

var random = new Random();
var random_points = randomizePoints(random);
printPoints(random_points, "Generated:");

random_points.Sort(new lb11.CoordinatesOriginComparer());
printPoints(random_points, "Sorted by distance from {0, 0}:");

random_points.Sort(new lb11.AbscissaAxisComparer());
printPoints(random_points, "Sorted by distance from x=0:");

random_points.Sort(new lb11.OrdinateAxisComparer());
printPoints(random_points, "Sorted by distance from y=0:");

random_points.Sort(new lb11.MainDiagonalComparer());
printPoints(random_points, "Sorted by distance from y=x:");

Console.WriteLine("App: closed");

void printPoints(List<lb11.Point> points, string message)
{
    Console.WriteLine(message);
    Console.WriteLine(string.Join(", ", random_points));
}

List<lb11.Point> randomizePoints(Random random)
{
    var n_points = random.NextInt64(5, 8);
    var points = new List<lb11.Point>();
    for (int i = 0; i < n_points; ++i)
    {
        points.Add(randomizePoint(random));
    }
    return points;
}

lb11.Point randomizePoint(Random random)
{
    return new lb11.Point(random.NextDouble(), random.NextDouble());
}

namespace lb11
{
    class Point
    {
        public double x { get { return _x; } }
        public double y { get { return _y; } }

        public Point(double x, double y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return $"{{{_x.ToString("0.00")}, {_y.ToString("0.00")}}}";
        }

        double _x;
        double _y;
    }

    class CoordinatesOriginComparer : IComparer<Point>
    {
        int IComparer<Point>.Compare(Point? lhs, Point? rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentNullException("Attempt to compare null");
            }

            return (lhs.x * lhs.x + lhs.y * lhs.y).CompareTo(rhs.x * rhs.x + rhs.y * rhs.y);
        }
    }

    class AbscissaAxisComparer : IComparer<Point>
    {
        int IComparer<Point>.Compare(Point? lhs, Point? rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentNullException("Attempt to compare null");
            }

            return Math.Abs(lhs.x).CompareTo(Math.Abs(rhs.x));
        }
    }

    class OrdinateAxisComparer : IComparer<Point>
    {
        int IComparer<Point>.Compare(Point? lhs, Point? rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentNullException("Attempt to compare null");
            }

            return Math.Abs(lhs.y).CompareTo(Math.Abs(rhs.y));
        }
    }

    class MainDiagonalComparer : IComparer<Point>
    {
        int IComparer<Point>.Compare(Point? lhs, Point? rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentNullException("Attempt to compare null");
            }

            return Math.Abs(lhs.x - lhs.y).CompareTo(Math.Abs(rhs.x - rhs.y));
        }
    }
}
