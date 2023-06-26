using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
        Console.WriteLine("Elevator simulator Menu");
        Console.WriteLine("1. Show status of all the elevators");
        Console.WriteLine("2. Call an elevator");
        Console.WriteLine("3. Set status of an elevator");
        Console.WriteLine("4. Exit Program");

        Console.WriteLine("Enter your choice (1-4):");
        choice = Convert.ToInt32(Console.ReadLine());
        Elevator selectedElevator;
        switch (choice)
        {
          case 1:
            foreach (var elevator in elevators)
            {
              elevator.ShowStatus();
              Console.WriteLine();
            }
            break;
          case 2:
            selectedElevator = elevators.FirstOrDefault(e => e.Status == ElevatorStatus.Operational);
            if (selectedElevator != null)
            {
              Console.WriteLine("Enter the destination floor number:");
              int destinationFloor = Convert.ToInt32(Console.ReadLine());

              Console.WriteLine("How many people are getting on?");
              int peopleGettingOn = Convert.ToInt32(Console.ReadLine());

              Console.WriteLine("How many people are waiting on the destination floor?");
              int peopleWaiting = Convert.ToInt32(Console.ReadLine());

              // Check if the number of people getting on exceeds the weight limit
              if (peopleGettingOn > selectedElevator.WeightLimit)
              {
                Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people.");
                break;
              }

              // Update elevator properties accordingly
              selectedElevator.CurrentFloor = destinationFloor;
              selectedElevator.PeopleCount += peopleGettingOn;
              // Update other elevator properties as needed

              Console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");

              // Prompt for the number of people getting off
              Console.WriteLine("How many people are getting off?");
              int peopleGettingOff = Convert.ToInt32(Console.ReadLine());

              // Ensure the number of people getting off doesn't exceed the current PeopleCount
              peopleGettingOff = Math.Min(peopleGettingOff, selectedElevator.PeopleCount);

              // Update elevator properties based on the number of people getting off
              selectedElevator.PeopleCount -= peopleGettingOff;
              // Update other elevator properties as needed

              Console.WriteLine($"Elevator {selectedElevator.Id} reached the destination floor. {peopleGettingOff} people got off.");

              if (selectedElevator.PeopleCount == 0)
              {
                Console.WriteLine($"Elevator {selectedElevator.Id} is now empty.");
              }

              if (peopleWaiting > 0)
              {
                // Calculate the maximum number of people from the waiting group who can get on without exceeding the weight limit
                int maxPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount, peopleWaiting);

                // Prompt for the number of people from the waiting group getting on
                Console.WriteLine($"How many people from the waiting group are getting on? (Up to {maxPeopleGettingOnFromWaiting})");
                int peopleGettingOnFromWaiting = Convert.ToInt32(Console.ReadLine());

                // Check if the number of people getting on exceeds the available spots
                if (peopleGettingOnFromWaiting > maxPeopleGettingOnFromWaiting)
                {
                  Console.WriteLine($"Weight Limit Exceeded.Only up to {maxPeopleGettingOnFromWaiting} people from the waiting group can get on.");
                  break;
                }

                // Check if the number of people getting on from the waiting group exceeds the weight limit
                if (peopleGettingOnFromWaiting + selectedElevator.PeopleCount > selectedElevator.WeightLimit)
                {
                  Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people.");
                  break;
                }

                // Update elevator properties based on the number of people getting on from the waiting group
                selectedElevator.PeopleCount += peopleGettingOnFromWaiting;
                // Update other elevator properties as needed

                Console.WriteLine($"Elevator {selectedElevator.Id} accommodated {peopleGettingOnFromWaiting} people from the waiting group.");
              }
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
  }
}
