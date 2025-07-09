using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    internal class Client
    {
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public string Password { get; set; }

        public Client(string ID, string FirstName, string LastName, int Age, string IBAN, decimal Balance, string Password)
        { 
            this.ID = ID;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;
            this.IBAN = IBAN;
            this.Balance = Balance;
            this.Password = Password;
        }


    }
}
