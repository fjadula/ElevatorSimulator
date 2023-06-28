using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using ElevatorSimulator.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


namespace ElevatorSimulator
{
  public class Program
  {
    //private static ElevatorService elevatorService;
    private const int numberOfElevators = 3;
    private const int numbOfFloors = 10;

    static void Main(string[] args)
    {
      var serviceProvider = new ServiceCollection()
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
        Console.WriteLine("Elevator Simulator Menu");
        Console.WriteLine("1. Show Status of all Elevators");
        Console.WriteLine("2. Manage Waiting Passengers");
        Console.WriteLine("3. Call an Elevator");
        Console.WriteLine("4. Set Status of an Elevator");
        Console.WriteLine("5. Exit Program");

        Console.WriteLine("Enter your choice (1-5):");
        choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
          case 1:
            elevatorService.ShowElevatorStatus(elevators);
            break;
          case 2:
            floorService.ManageWaitingPassengersOnFloor(floors);
            break;
          case 3:
            elevatorService.CallElevator(elevators,floors);
            break;
          case 4:
            elevatorService.SetElevatorStatus(elevators);
            break;
          case 5:
            Console.WriteLine("Exiting the program...");
            break;
          default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
        }

        Console.WriteLine();
      } while (choice != 5);
    }
  }
}