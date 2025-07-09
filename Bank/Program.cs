using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bank;
using static Bank.Constants;



class BankSystem
{
   
    static List<Client> clients = new List<Client>();
    static string filePath = _filePath;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        LoadClients();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Банкова Система ---");
            Console.Write("Въведи ID: ");
            string id = Console.ReadLine();
            Console.Write("Въведи парола: ");
            string pass = Console.ReadLine();

            Client client = clients.FirstOrDefault(c => c.ID == id && c.Password == pass);

            if (client != null)
            {
                ShowClientMenu(client);
            }
            else
            {
                Console.WriteLine("Невалидни данни. Натисни клавиш за нов опит.");
                Console.ReadKey();
            }
        }
    }

    static void LoadClients()
    {
        if (!File.Exists(filePath)) return;
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            string ID = parts[0];
            string FirstName = parts[1];
            string LastName = parts[2];
            int Age = int.Parse(parts[3]);
            string IBAN = parts[4];
            decimal Balance = decimal.Parse(parts[5]);
            string Password = parts[6];
            Client c = new Client(ID, FirstName, LastName, Age, IBAN, Balance, Password);
            clients.Add(c);
        }
    }

    static void SaveClients()
    {
        var lines = clients.Select(c => $"{c.ID},{c.FirstName},{c.LastName},{c.Age},{c.IBAN},{c.Balance},{c.Password}");
        File.WriteAllLines(filePath, lines);
    }

    static void ShowClientMenu(Client client)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Здравей, {client.FirstName} {client.LastName}!");
            Console.WriteLine($"Баланс: {client.Balance} лв");
            Console.WriteLine("1. Депозит");
            Console.WriteLine("2. Теглене");
            Console.WriteLine("3. Прехвърляне на пари");
            Console.WriteLine("4. Смяна на парола");
            Console.WriteLine("5. Изход от акаунта");
            Console.Write("Избери опция: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Deposit(client); break;
                case "2": Withdraw(client); break;
                case "3": Transfer(client); break;
                case "4": ChangePassword(client); break;
                case "5": SaveClients(); return;
                default: Console.WriteLine("Невалиден избор."); break;
            }

            Console.WriteLine("Натисни клавиш за продължение...");
            Console.ReadKey();
        }
    }

    static void Deposit(Client client)
    {
        Console.Write("Сума за депозит: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            client.Balance += amount;
            Console.WriteLine($"Успешен депозит: {amount} лв");
        }
        else Console.WriteLine("Невалидна сума.");
    }

    static void Withdraw(Client client)
    {
        Console.Write("Сума за теглене: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0 && amount <= client.Balance)
        {
            client.Balance -= amount;
            Console.WriteLine($"Успешно теглене: {amount} лв");
        }
        else Console.WriteLine("Недостатъчна наличност или невалидна сума.");
    }

    static void Transfer(Client client)
    {
        Console.Write("IBAN на получателя: ");
        string iban = Console.ReadLine();
        Console.Write("Сума за прехвърляне: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0 && amount <= client.Balance)
        {
            Client recipient = clients.FirstOrDefault(c => c.IBAN == iban);
            if (recipient != null)
            {
                client.Balance -= amount;
                recipient.Balance += amount;
                Console.WriteLine($"Успешен трансфер към {recipient.FirstName} {recipient.LastName}: {amount} лв");
            }
            else Console.WriteLine("Невалиден IBAN.");
        }
        else Console.WriteLine("Невалидна сума или недостатъчна наличност.");
    }

    static void ChangePassword(Client client)
    {
        Console.Write("Нова парола: ");
        string newPass = Console.ReadLine();
        client.Password = newPass;
        Console.WriteLine("Паролата е сменена успешно.");
    }
    
}