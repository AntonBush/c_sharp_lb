namespace lb3.bank;

public class Account
{
    public uint id { get; }
    public List<string> history
    {
        get { return new List<string>(_history); }
    }
    public uint balance { get; private set; }

    public Account(uint id)
    {
        this.id = id;
        this.balance = 0;
        _history = new List<string>();
    }

    public void deposite(uint money)
    {
        balance += money;
        _addToHistory("Deposite: " + money.ToString() + ", balance: " + balance.ToString());
    }

    public bool withdraw(uint money)
    {
        if (balance < money)
        {
            return false;
        }

        balance -= money;
        _addToHistory("Withdraw: " + money.ToString() + ", remains: " + balance.ToString());
        return true;
    }


    readonly List<string> _history;

    void _addToHistory(string message)
    {
        _history.Add(message + " | " + DateTime.Now.ToString());
    }
}
