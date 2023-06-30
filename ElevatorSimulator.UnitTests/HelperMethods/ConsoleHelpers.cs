using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.HelperMethods
{
  public class ConsoleHelpers
  {
    public static string CaptureConsoleOutput(Action action)
    {
      var consoleOutputWriter = new StringWriter();
      var standardOutput = Console.Out;

      Console.SetOut(consoleOutputWriter);

      action.Invoke();

      Console.SetOut(standardOutput);

      return consoleOutputWriter.ToString();
    }

    public class ConsoleInput : IDisposable
    {
      private readonly Queue<string> userInputBackup;
      private readonly TextReader originalInput;

      public ConsoleInput(Queue<string> userInput)
      {
        userInputBackup = new Queue<string>(userInput);
        originalInput = Console.In;

        Console.SetIn(new StringReaderWrapper(() => userInputBackup.Dequeue()));
      }

      public void Dispose()
      {
        Console.SetIn(originalInput);
      }
    }

    // Helper class for simulating string input
    public class StringReaderWrapper : TextReader
    {
      private readonly Func<string> readLineFunc;

      public StringReaderWrapper(Func<string> readLineFunc)
      {
        this.readLineFunc = readLineFunc;
      }

      public override string ReadLine()
      {
        return readLineFunc();
      }
    }
  }
}
