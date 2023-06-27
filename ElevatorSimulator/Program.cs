using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ElevatorSimulator
{
  class Program
  {
    static void Main(string[] args)
    {
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
        Elevator selectedElevator;
        switch (choice)
        {
          case 1:
            UpdateElevatorStatus(elevators);
            break;
          case 2:
            Console.WriteLine("Enter the destination floor number:");
            int destinationFloor = Convert.ToInt32(Console.ReadLine());

            selectedElevator = GetClosestElevator(elevators, destinationFloor);
            if (selectedElevator != null)
            {
              Console.WriteLine("How many people are getting on?");
              int peopleGettingOn = Convert.ToInt32(Console.ReadLine());
              // Check if the number of people getting on exceeds the weight limit
              if (peopleGettingOn > selectedElevator.WeightLimit - selectedElevator.PeopleCount)
              {
                Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people.");
                break;
              }

              Console.WriteLine("How many people are waiting on the destination floor?");
              int peopleWaiting = Convert.ToInt32(Console.ReadLine());

              // Update elevator properties accordingly
              selectedElevator.DestinationFloor = destinationFloor;
              selectedElevator.PeopleCount += peopleGettingOn;
              // Update other elevator properties as needed

              Console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");
              MoveElevator(selectedElevator); // Simulate elevator movement

              // Prompt for the number of people getting off
              Console.WriteLine("How many people are getting off?");
              int peopleGettingOff = Convert.ToInt32(Console.ReadLine());

              // Ensure the number of people getting off doesn't exceed the current PeopleCount
              peopleGettingOff = Math.Min(peopleGettingOff, selectedElevator.PeopleCount);

              // Update elevator properties based on the number of people getting off
              selectedElevator.PeopleCount -= peopleGettingOff;
              // Update other elevator properties as needed

              Console.WriteLine($"Elevator {selectedElevator.Id}, {peopleGettingOff} people got off.");

              // Calculate the maximum number of people from the waiting group that can get on
              int maxPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount, Math.Min(peopleWaiting, 4));
              Console.WriteLine($"How many people from the waiting group are getting on? (Up to {maxPeopleGettingOnFromWaiting})");

              int peopleGettingOnFromWaiting = Convert.ToInt32(Console.ReadLine());
              // peopleGettingOnFromWaiting = Math.Min(peopleGettingOnFromWaiting, maxPeopleGettingOnFromWaiting);
              int maxReached = selectedElevator.PeopleCount + peopleGettingOnFromWaiting;

              // Check if the number of people getting on from the waiting group exceeds the weight limit
              if (maxReached > selectedElevator.WeightLimit)
              {
                Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people from the waiting group.");
                break;
              }

              // Update elevator properties based on the number of people getting on from the waiting group
              // Update other elevator properties as needed
              selectedElevator.PeopleCount += peopleGettingOnFromWaiting;
              Console.WriteLine($"Elevator {selectedElevator.Id} accommodated {peopleGettingOnFromWaiting} people from the waiting group.");
            }
            else
            {
              Console.WriteLine("No operational elevators available. Please try again later.");
            }
            break;

          case 3:
            Console.WriteLine("Enter the elevator number:");
            int elevatorNum = Convert.ToInt32(Console.ReadLine());
            if (elevatorNum > 0 && elevatorNum <= elevators.Count)
            {
              selectedElevator = elevators[elevatorNum - 1];

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


    // Calculate the closest elevator to the destination floor among the operational and stationary elevators
    private static Elevator GetClosestElevator(List<Elevator> elevators, int destinationFloor)
    {
      Elevator closestElevator = null;
      int minDistance = int.MaxValue;

      foreach (var elevator in elevators)
      {
        if (elevator.Status == ElevatorStatus.Operational && elevator.Direction == Direction.Stationary)
        {
          int distance = Math.Abs(elevator.CurrentFloor - destinationFloor);

          if (distance < minDistance)
          {
            minDistance = distance;
            closestElevator = elevator;
          }
        }
      }

      return closestElevator;
    }

    // Simulate the movement of the elevator
    private static void MoveElevator(Elevator elevator)
    {
      Console.WriteLine($"Elevator {elevator.Id} is moving from floor {elevator.CurrentFloor} to floor {elevator.DestinationFloor}...");
      int distance = Math.Abs(elevator.CurrentFloor - elevator.DestinationFloor);
      int travelTimeInSeconds = distance * 2; // Assuming 2 seconds for each floor
      elevator.Direction = elevator.CurrentFloor < elevator.DestinationFloor ? Direction.Up : Direction.Down;
      int initialETA = travelTimeInSeconds;

      if (elevator.Direction == Direction.Up)
      {
        initialETA -= (elevator.CurrentFloor - 1) * 2;
      }
      else
      {
        initialETA -= (distance - (elevator.CurrentFloor - 1)) * 2;
      }

      Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor}, Direction: {elevator.Direction}, People Count: {elevator.PeopleCount}, Status: {elevator.Status}, ETA: {initialETA} seconds");

      for (int i = 0; i < distance; i++)
      {
        elevator.CurrentFloor += elevator.Direction == Direction.Up ? 1 : -1;
        if (elevator.CurrentFloor == elevator.DestinationFloor)
        {
          elevator.Direction = Direction.Stationary;

        }
        UpdateElevatorStatus(elevator);
        Thread.Sleep(2000);
      
      }

  
      Console.WriteLine($"Elevator {elevator.Id} has reached the destination floor: {elevator.DestinationFloor} .");
  
    }

    // Update the status of all elevators and display the current status
    private static void UpdateElevatorStatus(List<Elevator> elevators)
    {
      foreach (var elevator in elevators)
      {
        UpdateElevatorStatus(elevator);
      }
    }

    // Update the status of a single elevator and display the current status
    private static void UpdateElevatorStatus(Elevator elevator)
    {
      Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor}, Direction: {elevator.Direction}, People Count: {elevator.PeopleCount}, Status: {elevator.Status}, ETA: {CalculateETA(elevator)} seconds");

    }

    private static int CalculateETA(Elevator elevator)
    {
      int distance = Math.Abs(elevator.CurrentFloor - elevator.DestinationFloor);
      int travelTimeInSeconds = distance  * 2; // Assuming 2 seconds for each floor
      return travelTimeInSeconds;
    }
  }

}
