using System;
using System.IO;

namespace lb3
{
    /*

    Задание:
    Создать консольное приложение. Программа представляет собой
    автоматизированную систему учета банковских сведений.
    На каждого клиента банка хранятся следующие сведения:
    − Ф.И.О.;
    − Возраст;
    − Место работы;
    − Номера счетов.
    На каждом счете хранится информация о текущем балансе и история
    прихода, расхода. Для каждого клиента может быть создано неограниченное
    количество счетов. С каждым счетом можно производить следующие
    действия: открытие, закрытие, вклад денег, снятие денег, просмотр баланса,
    просмотр истории.
    Вся информация должна хранится в массивах. Рекомендуется объекты
    клиента и счета реализовать в виде классов. Баланс счета организовать в виде
    свойства только для чтения.

    */

    delegate void Procedure();

    class Program
    {
        static List<bank.Client> clients = new List<bank.Client>();
        static List<bank.Account> accounts = new List<bank.Account>();

        static uint client_id_counter = 0;

        static bool doIf(Procedure do_work, bool condition)
        {
            if (condition)
            {
                do_work();
            }

            return condition;
        }

        static void printHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  client list");
            Console.WriteLine("  client new");
            Console.WriteLine("  client remove <client-id>");
            Console.WriteLine("  account open <client-id>");
            Console.WriteLine("  account close <account-id>");
            Console.WriteLine("  account deposit <account-id> <money>");
            Console.WriteLine("  account withdraw  <account-id> <money>");
            Console.WriteLine("  account balance <account-id>");
            Console.WriteLine("  account history <account-id>");
            Console.WriteLine("  quit");
        }
        static void printClientHelp()
        {
            Console.WriteLine("Available client commands:");
            Console.WriteLine("list");
            Console.WriteLine("new");
            Console.WriteLine("remove <client-id>");
        }

        static void resolveClientListCommand(Queue<string> command_words)
        {
            Console.WriteLine($"Clients: {clients.Count}");
            foreach (bank.Client client in clients)
            {
                Console.WriteLine($"id: {client.id}");
                Console.WriteLine($"  first name:  {client.name.first}");
                Console.WriteLine($"  second_name: {client.name.second}");
                Console.WriteLine($"  patronymic:  {client.name.patronymic}");
                Console.WriteLine($"  age:         {client.age}");
                Console.WriteLine($"  workplace:   {client.workplace}");
                Console.WriteLine($"  accounts:    {client.accounts.Count}");
                foreach (bank.Account account in client.accounts)
                {
                    Console.WriteLine($"    id:      {account.id}");
                    Console.WriteLine($"    balance: {account.balance}");
                }
            }
        }

        delegate bool StringConverter<T>(out T t, string? src);

        static StringConverter<int> string_to_int_converter = (out int i, string? src) =>
        {
            return int.TryParse(src, out i);
        };

        static bool read<T>(out T t, StringConverter<T> convert)
        {
            return convert(out t, Console.ReadLine()?.Trim());
        }

        delegate bool Validator<T>(T t);

        static T readValid<T>(string message, StringConverter<T> converter, Validator<T> validate)
        {
            T t;

            do
            {
                Console.WriteLine(message);
            } while ((!read<T>(out t, converter)) || (!validate(t)));

            return t;
        }

        static bool isValidMember<T>(T? t)
        {
            if (t == null)
            {
                Console.WriteLine("New client will not be added");
                return false;
            }

            return true;
        }
        static void resolveClientNewCommand()
        {
            string? first_name;
            string? second_name;
            string? patronymic;
            uint? age;
            string? workplace;

            StringConverter<string?> string_converter = (out string? t, string? src) =>
            {
                if (src == null)
                {
                    t = null;
                    return false;
                }

                if (src == "abort")
                {
                    t = null;
                    return true;
                }

                t = src;
                return true;
            };
            Validator<string?> string_validator = (s) => { return s != ""; };

            StringConverter<uint?> uint_converter = (out uint? t, string? src) =>
            {
                if (src == null)
                {
                    t = null;
                    return false;
                }

                if (src == "abort")
                {
                    t = null;
                    return true;
                }

                uint ui;
                if (!uint.TryParse(src, out ui))
                {
                    t = null;
                    return false;
                }

                t = ui;
                return true;
            };
            Validator<uint?> age_validator = (a) => { return (a == null) || (14 <= a); };

            first_name = readValid<string?>("Enter first name or 'abort':"
                                           , string_converter
                                           , string_validator
                                           );
            if (!isValidMember(first_name))
            { return; }
            second_name = readValid<string?>("Enter second name or 'abort':"
                                           , string_converter
                                           , string_validator
                                           );
            if (!isValidMember(second_name))
            { return; }
            patronymic = readValid<string?>("Enter patronymic or 'abort':"
                                           , string_converter
                                           , string_validator
                                           );
            if (!isValidMember(patronymic))
            { return; }

            age = readValid<uint?>("Enter age (>= 14) or 'abort':"
                                 , uint_converter
                                 , age_validator
                                 );
            if (!isValidMember(age))
            { return; }

            workplace = readValid<string?>("Enter workplace or 'abort':"
                                           , string_converter
                                           , string_validator
                                           );
            if (!isValidMember(workplace))
            { return; }

            clients.Add(new bank.Client(client_id_counter++
                                       , new bank.PersonName(first_name, second_name, patronymic)
                                       , (uint)age
                                       , workplace
                                       ));
            Console.WriteLine("Client was added");
        }

        static void resolveClientRemoveCommand(Queue<string> command_words)
        {
            string? command_word;

            if (doIf(printClientHelp, !command_words.TryDequeue(out command_word)))
            { return; }

            int client_id;
            if (!int.TryParse(command_word, out client_id))
            {
                Console.WriteLine("Client id must be positive integer or zero!");
                return;
            }

            bank.Client? client_to_remove = clients.Find(client => client.id == client_id);

            if (client_to_remove == null)
            {
                Console.WriteLine($"Client with id {client_id} is not found!");
                return;
            }

            clients.Remove(client_to_remove);
        }

        static void resolveClientCommand(Queue<string> command_words)
        {
            string? command_word;

            if (doIf(printClientHelp, !command_words.TryDequeue(out command_word)))
            { return; }

            if (command_word == "list")
            {
                resolveClientListCommand(command_words);
            }
            else if (command_word == "new")
            {
                resolveClientNewCommand();
            }
            else if (command_word == "remove")
            {
                resolveClientRemoveCommand(command_words);
            }
            else
            {
                printClientHelp();
            }
        }

        static void resolveAccountCommand(Queue<string> command_words)
        {
        }

        static bool resolveCommand(string command)
        {
            if (command == "quit")
            { return false; }

            if (command == "")
            { return true; }

            var command_words = new Queue<string>(command.Split(' '));
            string? command_word;

            if (doIf(printHelp, !command_words.TryDequeue(out command_word)))
            { return true; }

            if (command_word == "client")
            {
                resolveClientCommand(command_words);
            }
            else if (command_word == "account")
            {
                resolveAccountCommand(command_words);
            }
            else
            {
                printHelp();
            }

            return true;
        }
        static void Main()
        {
            Console.WriteLine("Welcome to MMM!");
            printHelp();

            string? input;
            do
            {
                input = Console.ReadLine();
            }
            while (resolveCommand(input == null ? "" : input));
        }
    }
}
