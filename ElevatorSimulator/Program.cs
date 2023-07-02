using ElevatorSimulator;
using ElevatorSimulator.Common.Interfaces;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Service;
using Microsoft.Extensions.DependencyInjection;
using static ElevatorSimulator.Common.Constants;

public class Program
{
  private const int numberOfElevators = 3;
  private const int numbOfFloors = 10;

  static void Main(string[] args)
  {
    var serviceProvider = new ServiceCollection()
      .AddSingleton<IConsole, ConsoleCustom>()
        .AddElevatorService()
        .BuildServiceProvider();

    var elevatorService = serviceProvider.GetService<ElevatorService>();
    var floorService = serviceProvider.GetService<FloorService>();

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
      choice = ShowMenuAndGetChoice();

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
          Console.WriteLine(Messages.Exit);
          break;
        default:
          Console.WriteLine(Messages.Error);
          break;
      }

      Console.WriteLine();
    } while (choice != 6);
  }

  private static int ShowMenuAndGetChoice()
  {
    Console.WriteLine("Elevator Simulator Menu");
    Console.WriteLine("1. Show Status of all Elevators");
    Console.WriteLine("2. Manage Waiting Passengers");
    Console.WriteLine("3. Show all floors Status");
    Console.WriteLine("4. Call an Elevator");
    Console.WriteLine("5. Set Status of an Elevator");
    Console.WriteLine("6. "+Messages.Exit);
  
    Console.WriteLine(Messages.MainMenuChoice);

    if (int.TryParse(Console.ReadLine(), out int choice))
    {
      return choice;
    }

    return -1;
  }
}
