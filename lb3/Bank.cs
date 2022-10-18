namespace Bank
{
    struct PersonName
    {
        public readonly string first_name;
        public readonly string second_name;
        public readonly string patronymic;
    }

    class Client
    {
        public readonly PersonName name;
        public readonly uint age;
        public readonly string workplace;
        List<Account> _accounts;

        public Client(PersonName name, uint age, string workplace)
        {
            this.name = name;
            this.age = age;
            workplace = workplace;
        }

        public addAccount(Account account)
        {
            _accounts.Add(account);
        }
    }

    class Account
    {
        public List<string> history
        {
            get { return new List<string>(_history); }
        }
        public uint balance { get; private set; }
        List<string> _history;
        bool _is_opened;
        bool _is_closed;

        public Account()
        {
            _is_opened = false;
            _is_closed = false;
        }

        public void open()
        {
            if (_is_opened)
            {
                return;
            }

            _is_opened = true;
            _addToHistory("Account opened");
        }

        public void close()
        {
            if (!_is_opened || _is_closed)
            {
                return;
            }

            _is_closed = true;
            _addToHistory("Account closed");
        }

        public void deposite(uint money)
        {
            if (!_is_opened || _is_closed)
            { return; }

            balance += money;
            _addToHistory("Deposite: " + money.ToString() + ", balance: " + balance.ToString());
        }

        public void withdraw(uint money)
        {
            if (!_is_opened || _is_closed)
            { return; }

            if (money <= balance)
            {
                balance -= money;
            }
            _addToHistory("Withdraw: " + money.ToString() + ", remains: " + balance.ToString());
        }

        void _addToHistory(string message)
        {
            history.Add(message + "| " + DateTime.Now.ToString());
        }
    }
}