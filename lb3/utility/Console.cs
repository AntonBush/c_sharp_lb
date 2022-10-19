namespace lb3.utility
{
    class Console
    {
        public delegate bool StringConverter<T>(out T t, string? src);

        public static StringConverter<int> string_to_int_converter = (out int i, string? src) =>
        {
            return int.TryParse(src, out i);
        };

        public static bool read<T>(out T t, StringConverter<T> convert)
        {
            return convert(out t, System.Console.ReadLine()?.Trim());
        }

        public delegate bool Validator<T>(T t);

        public static T readValid<T>(string message, StringConverter<T> converter, Validator<T> validate)
        {
            T t;

            do
            {
                System.Console.WriteLine(message);
            } while ((!read<T>(out t, converter)) || (!validate(t)));

            return t;
        }

        public static bool readNonNull<T>(ref T t
                          , string message
                          , utility.Console.StringConverter<T?> sc
                          , utility.Console.Validator<T?> v
                          )
        {
            T? temp = utility.Console.readValid<T?>(message, sc, v);
            if (temp == null)
            { return false; }

            t = temp;
            return true;
        }

    }
}