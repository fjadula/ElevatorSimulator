using ElevatorSimulator;
using ElevatorSimulator.Common.Interfaces;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
  private const int numberOfElevators = 3;
  private const int numbOfFloors = 10;

  static void Main(string[] args)
  {
    var serviceProvider = new ServiceCollection()
      .  AddSingleton<IConsole, ConsoleCustom>()
        .AddElevatorService()
        .BuildServiceProvider();

    var elevatorService = serviceProvider.GetService<ElevatorService>();
    var floorService = serviceProvider.GetService<FloorService>();
    var _console = serviceProvider.GetService<IConsole>();

    List<Elevator> elevators = new List<Elevator>();
    for (int i = 0; i < numberOfElevators; i++)
    {
      elevators.Add(new Elevator());
    }

    List<Floor> floors = new List<Floor>();
    for (int i = 0; i < numbOfFloors; i++)
    {
      Floor floor = new Floor();
      floor.FloorNumber = i;
      floors.Add(floor);
    }

    int choice = 0;

    do
    {
      choice = ShowMenuAndGetChoice(_console);

      switch (choice)
      {
        case 1:
          elevatorService.ShowElevatorStatus(elevators);
          break;
        case 2:
          floorService.ManageWaitingPassengersOnFloor(floors);
          break;
        case 3:
          FloorService.ShowFloorStatus(floors);
          break;
        case 4:
          elevatorService.CallElevator(elevators, floors);
          break;
        case 5:
          elevatorService.SetElevatorStatus(elevators);
          break;
        case 6:
          _console.WriteLine("Exiting the program...");
          break;
        default:
          _console.WriteLine("Invalid choice. Please try again.");
          break;
      }

      Console.WriteLine();
    } while (choice != 6);
  }

  private static int ShowMenuAndGetChoice(IConsole _console)
  {
    _console.WriteLine("Elevator Simulator Menu");
    _console.WriteLine("1. Show Status of all Elevators");
    _console.WriteLine("2. Manage Waiting Passengers");
    _console.WriteLine("3. Show all floors Status");
    _console.WriteLine("4. Call an Elevator");
    _console.WriteLine("5. Set Status of an Elevator");
    _console.WriteLine("6. Exit Program");

    _console.WriteLine("Enter your choice (1-6):");

    if (int.TryParse(Console.ReadLine(), out int choice))
    {
      return choice;
    }

    return -1; // Invalid choice
  }
}
