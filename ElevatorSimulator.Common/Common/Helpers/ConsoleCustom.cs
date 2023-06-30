using System;

namespace ElevatorSimulator.Common.Interfaces
{
    public class ConsoleCustom : IConsole
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
