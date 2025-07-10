using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Вкарваме си класовете и константите от други файлове (например пътя до файла)
using Bank;
using static Bank.Constants;

class BankSystem
{
    static List<Client> clients = new List<Client>();

    // Пътят до файла, в който пазим клиентите – идва от Constants
    static string filePath = _filePath;

    static void Main()
    {
        // Това е за да показва кирилица в конзолата
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        LoadClients();

        // Безкраен цикъл – програмата не спира, а чака нови логвания
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Банкова Система ---");

            Console.Write("Въведи ID: ");
            string id = Console.ReadLine();
            Console.Write("Въведи парола: ");
            string pass = Console.ReadLine();

            // Търсим потребител с точно това ID и парола
            Client client = clients.FirstOrDefault(c => c.ID == id && c.Password == pass);

            // Ако намерим потребителя – влизаме в менюто му
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

    // Зареждаме клиентите от текстовия файл
    static void LoadClients()
    {
        if (!File.Exists(filePath)) return; 

        var lines = File.ReadAllLines(filePath); 

        foreach (var line in lines)
        {
            var parts = line.Split(',');

            // Пълним си обект Client с информацията от файла
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

    // Записваме промените обратно във файла – извиква се при logout
    static void SaveClients()
    {
        var lines = clients.Select(c => $"{c.ID},{c.FirstName},{c.LastName},{c.Age},{c.IBAN},{c.Balance},{c.Password}");
        File.WriteAllLines(filePath, lines);
    }

    // Менюто на клиента след логин – тук са всички опции
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

            // Разглеждаме избора
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

    // Депозит – добавя пари към акаунта
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

    // Теглене на пари – ако има достатъчно наличност
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

    // Прехвърляне на пари към друг клиент по IBAN
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

    // Смяна на паролата – просто я презаписваме
    static void ChangePassword(Client client)
    {
        Console.Write("Нова парола: ");
        string newPass = Console.ReadLine();
        client.Password = newPass;
        Console.WriteLine("Паролата е сменена успешно.");
    }
}
