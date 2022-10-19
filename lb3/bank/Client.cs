using System;
using System.Collections.Generic;

namespace lb3.bank
{
    public struct PersonName
    {
        public readonly string first;
        public readonly string second;
        public readonly string patronymic;

        public PersonName(string first, string second, string patronymic)
        {
            this.first = first;
            this.second = second;
            this.patronymic = patronymic;
        }
    }

    public class Client
    {
        public uint id;
        public readonly PersonName name;
        public readonly uint age;
        public readonly string workplace;
        public List<bank.Account> accounts;

        public Client(uint id, PersonName name, uint age, string workplace)
        {
            this.id = id;
            this.name = name;
            this.age = age;
            this.workplace = workplace;
            accounts = new List<bank.Account>();
        }
    }
}