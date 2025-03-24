using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionUsage_HW
{
   
    class TaskCountLimitException : Exception
    {
        public TaskCountLimitException(int taskCountLimit) : base($"Превышено максимальное количество задач: {taskCountLimit}")
        {
        }
    }

    class TaskLengthLimitException : Exception
    {
        public TaskLengthLimitException(int taskLength, int taskLengthLimit) : base($"Длина задачи ‘{taskLength}’ превышает максимально допустимое значение {taskLengthLimit}")
        {
        }
    }

    class DuplicateTaskException : Exception
    {
        public DuplicateTaskException(string task) : base($"Задача ‘{task}’ уже существует")
        {
        }
    }
}
