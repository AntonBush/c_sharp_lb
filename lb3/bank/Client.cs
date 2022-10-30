namespace lb3.bank;

public struct PersonName
{
    public string first { get; }
    public string second { get; }
    public string patronymic { get; }

    public PersonName(string first, string second, string patronymic)
    {
        this.first = first;
        this.second = second;
        this.patronymic = patronymic;
    }

    public override string ToString()
    {
        return $"{second} {first} {patronymic}";
    }
}

public class Client
{
    public uint id { get; }
    public PersonName name { get; }
    public uint age { get; }
    public string workplace { get; }
    public List<Account> accounts { get; }

    public Client(uint id, PersonName name, uint age, string workplace)
    {
        this.id = id;
        this.name = name;
        this.age = age;
        this.workplace = workplace;
        accounts = new List<Account>();
    }
}
