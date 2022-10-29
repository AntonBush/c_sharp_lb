namespace lb3;

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
    static uint account_id_counter = 0;

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
        Console.WriteLine("  list");
        Console.WriteLine("  new");
        Console.WriteLine("  remove <client-id>");
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

    static bool resolveClientNewCommand()
    {
        string first_name = "";
        string second_name = "";
        string patronymic = "";
        uint age = 0;
        string workplace = "";

        utility.Console.StringConverter<string?> string_converter = (out string? t, string? src) =>
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
        utility.Console.Validator<string?> string_validator = (s) => { return s != ""; };

        utility.Console.StringConverter<uint?> uint_converter = (out uint? t, string? src) =>
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
        utility.Console.Validator<uint?> age_validator = (a) => { return (a == null) || (14 <= a); };

        if (!utility.Console.readNonNull<string>(ref first_name
                                                , "Enter first name or 'abort':"
                                                , string_converter
                                                , string_validator
                                                ))
        { return false; }
        if (!utility.Console.readNonNull<string>(ref second_name
                                                , "Enter second name or 'abort':"
                                                , string_converter
                                                , string_validator
                                                ))
        { return false; }
        if (!utility.Console.readNonNull<string>(ref patronymic
                                                , "Enter patronymic name or 'abort':"
                                                , string_converter
                                                , string_validator
                                                ))
        { return false; }
        {
            uint? temp = utility.Console.readValid<uint?>("Enter age (>= 14) or 'abort':"
                                                         , uint_converter
                                                         , age_validator
                                                         );
            if (temp == null)
            { return false; }

            age = (uint)temp;
        }
        if (!utility.Console.readNonNull<string>(ref workplace
                                                , "Enter workplace or 'abort':"
                                                , string_converter
                                                , string_validator
                                                ))
        { return false; }
        clients.Add(new bank.Client(client_id_counter++
                                   , new bank.PersonName(first_name, second_name, patronymic)
                                   , age
                                   , workplace
                                   ));
        return true;
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
            if (!resolveClientNewCommand())
            {
                Console.WriteLine("New client will not be added");
            }
            else
            {
                Console.WriteLine("Client was added");
            }
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

    static void printAccountHelp()
    {
        Console.WriteLine("Available account commands:");
        Console.WriteLine("  open <client-id>");
        Console.WriteLine("  close <account-id>");
        Console.WriteLine("  deposit <account-id> <money>");
        Console.WriteLine("  withdraw  <account-id> <money>");
        Console.WriteLine("  balance <account-id>");
        Console.WriteLine("  history <account-id>");
    }

    static Tuple<bank.Client, bank.Account>? findClientAccount(Predicate<bank.Account> condition)
    {
        foreach (bank.Client maybe_client in clients)
        {
            bank.Account? maybe_account = maybe_client.accounts.Find(condition);
            if (maybe_account != null)
            {
                return new Tuple<bank.Client, bank.Account>(maybe_client, maybe_account);
            }
        }

        return null;
    }
    enum ChangeBalanceCommand
    {
        withdraw,
        deposite
    };
    static void resolveAccountCommand(Queue<string> command_words)
    {
        string? command_word;

        if (doIf(printAccountHelp, !command_words.TryDequeue(out command_word)))
        { return; }

        if (command_word == "open")
        {
            if (doIf(printAccountHelp, !command_words.TryDequeue(out command_word)))
            { return; }

            int client_id;
            if (!int.TryParse(command_word, out client_id))
            {
                Console.WriteLine("Client id must be integer!");
                return;
            }

            bank.Client? client = clients.Find(client => client.id == client_id);

            if (client == null)
            {
                Console.WriteLine($"Client with id {client_id} is not found!");
                return;
            }

            client.accounts.Add(new bank.Account(account_id_counter));
            Console.WriteLine($"Client {client_id} opened account {account_id_counter++}");

            return;
        }

        var read_client_account = () =>
        {
            if (doIf(printAccountHelp, !command_words.TryDequeue(out command_word)))
            { return null; }

            uint account_id;
            if (!uint.TryParse(command_word, out account_id))
            {
                Console.WriteLine("Account id must be integer!");
                return null;
            }

            var client_account = findClientAccount(a => a.id == account_id);
            if (client_account == null)
            {
                Console.WriteLine($"Account with id {account_id} is not found!");
                return null;
            }

            return client_account;
        };

        Tuple<bank.Client, bank.Account>? client_account;
        if (command_word == "close")
        {
            client_account = read_client_account();
            if (client_account == null) { return; }

            client_account.Item1.accounts.Remove(client_account.Item2);
            Console.WriteLine($"Account {client_account.Item2.id} closed");
        }
        else if (command_word == "deposit")
        {
            client_account = read_client_account();
            if (client_account == null) { return; }

            if (doIf(printAccountHelp, !command_words.TryDequeue(out command_word)))
            { return; }

            uint money;
            if (!uint.TryParse(command_word, out money))
            {
                Console.WriteLine("Money must be positive integer!");
                return;
            }

            client_account.Item2.deposite(money);
            Console.WriteLine($"Account {client_account.Item2.id} deposited by {money}");
        }
        else if (command_word == "withdraw")
        {
            client_account = read_client_account();
            if (client_account == null) { return; }

            if (doIf(printAccountHelp, !command_words.TryDequeue(out command_word)))
            { return; }

            uint money;
            if (!uint.TryParse(command_word, out money))
            {
                Console.WriteLine("Money must be positive integer!");
                return;
            }

            if (!client_account.Item2.withdraw(money))
            {
                Console.WriteLine($"Not enough money on account");
            }
            else
            {
                Console.WriteLine($"Account {client_account.Item2.id} withdrawed by {money}");
            }
        }
        else if (command_word == "balance")
        {
            client_account = read_client_account();
            if (client_account == null) { return; }

            Console.WriteLine($"Account {client_account.Item2.id} balance: {client_account.Item2.balance}");
        }
        else if (command_word == "history")
        {
            client_account = read_client_account();
            if (client_account == null) { return; }

            Console.WriteLine($"Account {client_account.Item2.id} history:");
            foreach (string history_item in client_account.Item2.history)
            {
                Console.WriteLine($"  {history_item}");
            }
        }
        else
        {
            printAccountHelp();
        }
    }

    static bool resolveCommand(string command)
    {
        if (command == "quit")
        { return false; }

        if (command == "")
        { return true; }

        var command_words = new Queue<string>(command.Trim().Split(' '));
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
