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
    private static ElevatorService elevatorService;

    static void Main(string[] args)
    {
      var serviceProvider = new ServiceCollection()
          .AddSingleton<ElevatorService>()
          .BuildServiceProvider();

      elevatorService = serviceProvider.GetService<ElevatorService>();

      List<Elevator> elevators = new List<Elevator>();
      elevators.Add(new Elevator());
      elevators.Add(new Elevator());
      elevators.Add(new Elevator());

      int choice = 0;

      do
      {
        Console.WriteLine("Elevator Simulator Menu");
        Console.WriteLine("1. Show Status of Elevator");
        Console.WriteLine("2. Call Elevator");
        Console.WriteLine("3. Set Status of Elevator");
        Console.WriteLine("4. Exit Program");

        Console.WriteLine("Enter your choice (1-4):");
        choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
          case 1:
            elevatorService.UpdateElevatorStatus(elevators);
            break;
          case 2:
            CallElevator(elevators);
            break;
          case 3:
            SetElevatorStatus(elevators);
            break;
          case 4:
            Console.WriteLine("Exiting the program...");
            break;
          default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
        }

        Console.WriteLine();
      } while (choice != 4);
    }

    static void CallElevator(List<Elevator> elevators)
    {
      Console.WriteLine("Enter the destination floor number:");
      int destinationFloor = Convert.ToInt32(Console.ReadLine());

      Elevator selectedElevator = elevatorService.GetClosestElevator(elevators, destinationFloor);
      if (selectedElevator != null)
      {
        Console.WriteLine("How many people are getting on?");
        int peopleGettingOn = Convert.ToInt32(Console.ReadLine());

        if (peopleGettingOn > selectedElevator.WeightLimit - selectedElevator.PeopleCount)
        {
          Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people.");
          return;
        }

        Console.WriteLine("How many people are waiting on the destination floor?");
        int peopleWaiting = Convert.ToInt32(Console.ReadLine());

        selectedElevator.DestinationFloor = destinationFloor;
        selectedElevator.PeopleCount += peopleGettingOn;

        Console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");
        selectedElevator.MoveElevator(); // Simulate elevator movement

        Console.WriteLine("How many people are getting off?");
        int peopleGettingOff = Convert.ToInt32(Console.ReadLine());

        peopleGettingOff = Math.Min(peopleGettingOff, selectedElevator.PeopleCount);

        selectedElevator.PeopleCount -= peopleGettingOff;

        Console.WriteLine($"Elevator {selectedElevator.Id}, {peopleGettingOff} people got off.");

        if (peopleWaiting > 0)
        {
          int maxPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount, Math.Min(peopleWaiting, 4));
          Console.WriteLine($"How many people from the waiting group are getting on? (Up to {maxPeopleGettingOnFromWaiting})");

          int peopleGettingOnFromWaiting = Convert.ToInt32(Console.ReadLine());
          int maxReached = selectedElevator.PeopleCount + peopleGettingOnFromWaiting;

          if (maxReached > selectedElevator.WeightLimit)
          {
            Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people from the waiting group.");
            return;
          }
          selectedElevator.PeopleCount += peopleGettingOnFromWaiting;
          Console.WriteLine($"Elevator {selectedElevator.Id} accommodated {peopleGettingOnFromWaiting} people from the waiting group.");
        }
      }
      else
      {
        Console.WriteLine("No operational elevators available. Please try again later.");
      }
    }


    static void SetElevatorStatus(List<Elevator> elevators)
    {
      Console.WriteLine("Enter the elevator number:");
      int elevatorNum = Convert.ToInt32(Console.ReadLine());
      if (elevatorNum > 0 && elevatorNum <= elevators.Count)
      {
        Elevator selectedElevator = elevators[elevatorNum - 1];

        Console.WriteLine("Enter the new status of the elevator:");
        Console.WriteLine("1. Operational");
        Console.WriteLine("2. OutOfOrder");
        int statusChoice = Convert.ToInt32(Console.ReadLine());

        switch (statusChoice)
        {
          case 1:
            selectedElevator.Status = ElevatorStatus.Operational;
            Console.WriteLine($"Elevator {elevatorNum} status updated to Operational successfully!");
            break;
          case 2:
            selectedElevator.Status = ElevatorStatus.OutOfOrder;
            Console.WriteLine($"Elevator {elevatorNum} status updated to OutOfOrder successfully!");
            break;
          default:
            Console.WriteLine("Invalid status choice. Please try again.");
            break;
        }
      }
      else
      {
        Console.WriteLine("Invalid elevator number. Please try again.");
      }
    }
  }
}