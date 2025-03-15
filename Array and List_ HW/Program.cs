
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Transactions;
using System.Xml.Linq;

namespace Array_and_List__HW
{
    internal class Program
    {
        // Общие данные для работы приложения
        static readonly List<string> transactions = new();
        static string userName = "";

        // Тексты для сообщений
        static readonly string helpText = @"
Добро пожаловать в бота для учета финансов!
Доступные команды:
• /start – Запускает работу бота. После ввода этой команды вас попросят ввести имя.
• /help – Выводит справку по командам.
• /info – Показывает информацию о версии программы и дате её создания.
• /echo – Повторяет введённый текст (например, /echo Привет – бот ответит 'Привет').
• /addtask – Добавляет задачу.
• /showtasks – Показывает задачи.
• /removetask – Удаляет задачу.
• /exit – Завершает работу бота.";

        static readonly string infoText = "Версия программы: V0.3. Дата создания: 16.03.2025.";
        static readonly string exitText = "Вы закрыли приложение.";
        static readonly string echoText = "Вы не ввели текст для echo.";
        static readonly string addTaskText = "Пожалуйста, введите описание задачи:";
        static readonly string showTasksText = "Ваши задачи:";
        static readonly string removeTaskText = "Какую задачу вы хотите удалить? Наберите цифру задачи:";


        public static void Main(string[] args)
        {
            DisplayWelcomeMessage();

            string input = Console.ReadLine() ?? "";
            while (input != "/exit")
            {
                switch (input)
                {
                    case "/start":
                        ProcessStart();
                        break;
                    case "/help":
                        ProcessHelp();
                        break;
                    case "/info":
                        ProcessInfo();
                        break;
                    case string s when s.StartsWith("/echo"):
                        ProcessEcho(input);
                        break;
                    case "/addtask":
                        AddTask();
                        break;
                    case "/showtasks":
                        ShowTasks();
                        break;
                    case "/removetask":
                        RemoveTask();
                        break;
                    default:
                        Console.WriteLine("Вы ввели неправильную команду!");
                        Console.WriteLine(helpText);
                        break;
                }

                input = Console.ReadLine() ?? "";
            }

            ProcessExit();
        }

        static void DisplayWelcomeMessage()
        {
            Console.WriteLine("Добро пожаловать! Тут вы можете управлять своими задачами!");
            Console.WriteLine("Вводите команду, чтобы продолжить!");
            Console.WriteLine("/start, /help, /info, /echo, /addtask, /showtasks, /removetask, /exit");
        }

        static void ProcessStart()
        {
            Console.WriteLine("Введите свое имя:");
            userName = Console.ReadLine() ?? "";
            Console.WriteLine($"Добро пожаловать, {userName}!");
        }

        static void ProcessHelp()
        {
            Console.WriteLine($"Уважаемый {userName}, вот список команд: {helpText}");
        }

        static void ProcessInfo()
        {
            Console.WriteLine($"Уважаемый {userName}, {infoText}");
        }

        static void ProcessEcho(string input)
        {
            if (userName.Length <= 0)
            {
                Console.WriteLine("Вы еще не запустили бот.");
                return;
            }
            if (input.Length > 6)
            {
                string echoTrim = input.Substring(6);
                Console.WriteLine(echoTrim);
            }
            else
            {
                Console.WriteLine($"Уважаемый {userName}, {echoText}");
                Console.WriteLine(helpText);
            }
        }

        static void AddTask()
        {
            Console.WriteLine($"Уважаемый {userName}, {addTaskText}");
            string newTransaction = Console.ReadLine() ?? "";
            transactions.Add(newTransaction);
            Console.WriteLine($"Ваша задача \"{newTransaction}\" добавлена.");
        }

        static void ShowTasks()
        {
            Console.WriteLine($"Уважаемый {userName}, {showTasksText}");
            if (transactions.Count > 0)
            {
                int transactionCounter = 1;
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"{transactionCounter}. {transaction}");
                    transactionCounter++;
                }

            }
            else
            {
                Console.WriteLine("Вы еще не добавили задачи.");
            }
        }

        static void RemoveTask()
        {
            Console.WriteLine($"Уважаемый {userName}, {removeTaskText}");
            if (transactions.Count == 0)
            {
                Console.WriteLine("Список задач пуст. Удалять нечего.");
                return;
            }

            int index;
            while (true)
            {
                Console.WriteLine("Введите номер задачи (начиная с 1):");

                int transactionCounter = 1;
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"{transactionCounter}. {transaction}");
                    transactionCounter++;
                }

                string transactionNumber = Console.ReadLine() ?? "";
                if (!int.TryParse(transactionNumber, out index))
                {
                    Console.WriteLine("Некорректный ввод. Введите число.");
                    continue;
                }

                if (index <= 0 || index > transactions.Count)
                {
                    Console.WriteLine("Такой транзакции в списке нет. Повторите ввод.");
                    continue;
                }

                index--;
                string removedTransaction = transactions[index];
                transactions.RemoveAt(index);
                Console.WriteLine($"Задача {index + 1}. {removedTransaction} удалена.");

                break;
            }

            #region Преобразуем индекс для 0-based нумерации
            // Преобразуем индекс для 0-based нумерации
            //index--;


            //LinkedListNode<string>? node = transactions.First;
            //int currentIndex = 0;
            //while (node != null && currentIndex < index)
            //{
            //    node = node.Next;
            //    currentIndex++;
            //}

            //if (node != null)
            //{
            //    transactions.Remove(node);
            //    Console.WriteLine("Транзакция удалена.");
            //}
            #endregion
        }

        static void ProcessExit()
        {
            Console.WriteLine($"Уважаемый {userName}, {exitText}");
        }

        #region PrintTransacrtions
        //static void PrintTransactions(ref int transactionCounter)
        //{
        //    foreach (var transaction in transactions)
        //    {
        //        Console.WriteLine($"{transactionCounter}. {transaction}");
        //        transactionCounter++;

        //    }
        //}
        #endregion
    }
}
