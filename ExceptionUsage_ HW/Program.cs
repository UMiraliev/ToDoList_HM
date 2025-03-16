
using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace ExceptionUsage_HW
{
    internal class Program
    {
        // Общие данные для работы приложения
        static readonly List<string> tasks = new();
        static string userName = "";

        // Тексты для сообщений
        static readonly string helpText = @"
Добро пожаловать в бота для управление задачами!
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
        static int taskCountLimit = 0;
        static int taskLengthLimit = 0;

        public static void Main(string[] args)
        {
            try
            {
                DisplayWelcomeMessage();

                bool isTaskCountLimitValid = false;
                while (!isTaskCountLimitValid)
                {
                    Console.WriteLine("Введите максимально допустимое количество задач:");
                    string input = Console.ReadLine() ?? "";
                    try
                    {
                        taskCountLimit = ParseAndValidateInt(input, 1, 100);
                        isTaskCountLimitValid = true;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }

                bool isTaskLengthLimitValid = false;
                while (!isTaskLengthLimitValid)
                {
                    Console.WriteLine("Введите максимально допустимую длину задачи:");
                    string input = Console.ReadLine() ?? "";
                    try
                    {
                        taskLengthLimit = ParseAndValidateInt(input, 1, 100);
                        isTaskLengthLimitValid = true;
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                string command = Console.ReadLine() ?? "";

                while (command != "/exit")
                {
                    try
                    {
                        switch (command)
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
                                ProcessEcho(command);
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
                    }
                    catch (TaskCountLimitException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (TaskLengthLimitException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (DuplicateTaskException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Произошла непредвиденная ошибка:");
                        Console.WriteLine($"Тип: {ex.GetType()}");
                        Console.WriteLine($"Сообщение: {ex.Message}");
                        Console.WriteLine($"StackTrace: {ex.StackTrace}");
                        if (ex.InnerException != null)
                        {
                            Console.WriteLine($"InnerException: {ex.InnerException}");
                        }
                    }
                    command = Console.ReadLine() ?? "";
                }

                ProcessExit();
            }
            catch(TaskCountLimitException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch(TaskLengthLimitException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (DuplicateTaskException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Произошла непредвиденная ошибка:");
                Console.WriteLine($"Тип: {ex.GetType()}");
                Console.WriteLine($"Сообщение: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"InnerException: {ex.InnerException}");
                }
            }
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
            ValidateString(userName);
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
            string newTask = Console.ReadLine() ?? "";

            if (tasks.Count >= taskCountLimit)
            {
                throw new TaskCountLimitException(taskCountLimit);
            }
            if (newTask.Length > taskLengthLimit)
            { 
                throw new TaskLengthLimitException(newTask.Length, taskLengthLimit);
            }
            if (tasks.Contains(newTask))
            {
                throw new DuplicateTaskException(newTask);
            }
            tasks.Add(newTask);
            Console.WriteLine($"Ваша задача \"{newTask}\" добавлена.");
        }

        static void ShowTasks()
        {
            Console.WriteLine($"Уважаемый {userName}, {showTasksText}");
            if (tasks.Count > 0)
            {
                int taskCounter = 1;
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{taskCounter}. {task}");
                    taskCounter++;
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
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст. Удалять нечего.");
                return;
            }

            int index;
            while (true)
            {
                Console.WriteLine("Введите номер задачи (начиная с 1):");

                int taskCounter = 1;
                foreach (var task in tasks)
                {
                    Console.WriteLine($"{taskCounter}. {task}");
                    taskCounter++;
                }

                string transactionNumber = Console.ReadLine() ?? "";
                if (!int.TryParse(transactionNumber, out index))
                {
                    Console.WriteLine("Некорректный ввод. Введите число.");
                    continue;
                }

                if (index <= 0 || index > tasks.Count)
                {
                    Console.WriteLine("Такой транзакции в списке нет. Повторите ввод.");
                    continue;
                }

                index--;
                string removedTask = tasks[index];
                tasks.RemoveAt(index);
                Console.WriteLine($"Задача {index + 1}. {removedTask} удалена.");

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

        #region Использование метода для ввывода сообщения
        //static void LogException(Exception ex)
        //{
        //    Console.WriteLine("Произошла непредвиденная ошибка:");
        //    Console.WriteLine($"Тип: {ex.GetType()}");
        //    Console.WriteLine($"Сообщение: {ex.Message}");
        //    Console.WriteLine($"StackTrace: {ex.StackTrace}");
        //    if (ex.InnerException != null)
        //    {
        //        Console.WriteLine($"InnerException: {ex.InnerException}");
        //    }
        //}
        #endregion

        static int ParseAndValidateInt(string? input, int minValue, int maxValue)
        {
            if (!int.TryParse(input, out int result))
            {
                throw new ArgumentException("Введено не число.");
            }
            if (result < minValue || result > maxValue)
            {
                throw new ArgumentException($"Число должно быть в диапазоне от {minValue} до {maxValue}.");
            }
            return result;
        }

        static void ValidateString(string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentException("Строка не может быть пустой или состоять только из пробелов.");
            }
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

    class TaskCountLimitException : Exception
    {
        public TaskCountLimitException() : base()
        {
        }
        public TaskCountLimitException(int taskCountLimit) : base()
        {
            Console.WriteLine($"Превышено максимальное количество задач: {taskCountLimit}");
        }
    }

    class TaskLengthLimitException : Exception
    { 
    public TaskLengthLimitException() : base()
        {

        }
        public TaskLengthLimitException(int taskLength, int taskLengthLimit) : base()
        {
            Console.WriteLine($"Длина задачи ‘{taskLength}’ превышает максимально допустимое значение {taskLengthLimit}");
        }
    }

    class DuplicateTaskException : Exception
    {
        public DuplicateTaskException() : base()
        {
        }
        public DuplicateTaskException(string task) : base()
        {
            Console.WriteLine($"Задача ‘{task}’ уже существует");
        }
    }

    }
