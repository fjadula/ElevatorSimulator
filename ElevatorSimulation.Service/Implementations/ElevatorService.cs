using ElevatorSimulator.DTOs;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System.Security.Cryptography.X509Certificates;

namespace ElevatorSimulator.Service.Implementations
{

  public interface IElevatorService
  {
    void ShowElevatorStatus(List<Elevator> elevators);
    Task<Elevator?> GetClosestElevator(List<Elevator> elevators, int destinationFloor);
    Task CallElevator(List<Elevator> elevators, List<Floor> floors);

   // int AddPassengers(int numPeopeleGettingOn, Elevator elevator);
   // int RemovePassengers(int numPeopeleGettingOff, Elevator elevator);

  }

  public class ElevatorService : IElevatorService
  {
    public void ShowElevatorStatus(List<Elevator> elevators)
    {
      Console.WriteLine("Elevator Status:");
      foreach (var elevator in elevators)
      {
        Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor} - Status: {elevator.Status} , People Count: {elevator.PeopleCount}");
      }
    }

    public async  Task<Elevator?> GetClosestElevator(List<Elevator> elevators, int destinationFloor)
    {
      Elevator? closestElevator = null;
      int minDistance = int.MaxValue;

      foreach (var elevator in elevators)
      {
        if (elevator.Status == ElevatorStatus.Operational)
        {
          int distance = Math.Abs(elevator.CurrentFloor - destinationFloor);
          if (distance < minDistance)
          {
            closestElevator = elevator;
            minDistance = distance;
          }
        }
      }

      return  await Task.FromResult(closestElevator);
    }

    public async Task CallElevator(List<Elevator> elevators,List<Floor> floors)
    {
      Console.WriteLine("Enter the destination floor number:");
      int destinationFloor = Convert.ToInt32(Console.ReadLine());

      Elevator? selectedElevator = await GetClosestElevator(elevators, destinationFloor);
   
      if (selectedElevator != null)
      {
        Console.WriteLine("How many people are getting on?");
        int numPeopleGettingOn = Convert.ToInt32(Console.ReadLine());

        //if (numPeopleGettingOn > selectedElevator.WeightLimit - selectedElevator.PeopleCount)
        //{
        //  Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people.");
        //  return;
        //}

        Console.WriteLine("How many people are waiting on the destination floor?");
        int numOfPeopleWaiting = Convert.ToInt32(Console.ReadLine());

        Floor selectedFloor = floors.FirstOrDefault(floor => floor.FloorNumber == destinationFloor);
        selectedElevator.DestinationFloor = destinationFloor;
        selectedElevator.PeopleCount += numPeopleGettingOn;
      //  AddPassengers(numPeopleGettingOn, selectedElevator,numOfPeopleWaiting, selectedFloor);

        Console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");
        selectedElevator.MoveElevator(); // Simulate elevator movement

        Console.WriteLine("How many people are getting off?");
        int peopleGettingOff = Convert.ToInt32(Console.ReadLine());

        peopleGettingOff = Math.Min(peopleGettingOff, selectedElevator.PeopleCount);

        selectedElevator.PeopleCount -= peopleGettingOff;

        Console.WriteLine($"Elevator {selectedElevator.Id}, {peopleGettingOff} people got off.");

        if (numOfPeopleWaiting > 0)
        {
          int maxnumPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount, Math.Min(numOfPeopleWaiting, 4));
          Console.WriteLine($"How many people from the waiting group are getting on? (Up to {maxnumPeopleGettingOnFromWaiting})");

          int numPeopleGettingOnFromWaiting = Convert.ToInt32(Console.ReadLine());
          int maxReached = selectedElevator.PeopleCount + numPeopleGettingOnFromWaiting;

          if (maxReached > selectedElevator.WeightLimit)
          {
            Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people from the waiting group.");
            return;
          }
          selectedElevator.PeopleCount += numPeopleGettingOnFromWaiting;
          Console.WriteLine($"Elevator {selectedElevator.Id} accommodated {numPeopleGettingOnFromWaiting} people from the waiting group.");
        }

        if (peopleGettingOff > 0 && selectedElevator.PeopleCount == 0)
        {
          Console.WriteLine("No more people in the elevator to get off.");
        }
      }
      else
      {
        Console.WriteLine("No operational elevators available. Please try again later.");
      }
    }



    public void SetElevatorStatus(List<Elevator> elevators)
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
    private static void AddPassengers(int numOfPeopleGettingOn, Elevator selectedElevator,int numOfnumOfPeopleWaiting,Floor selectedFloor)
    {
      int peopleOnElevator = 0;
      if (numOfPeopleGettingOn > selectedElevator.WeightLimit - selectedElevator.PeopleCount)
        {
          Console.WriteLine("Weight limit exceeded. Cannot accommodate that many people.");
          return;
        }

      if (numOfnumOfPeopleWaiting>numOfnumOfPeopleWaiting)
      {
        Console.WriteLine($"There are only {numOfnumOfPeopleWaiting} people waiting.");
        return;
      }

      else
      {
        peopleOnElevator=selectedElevator.PeopleCount + numOfPeopleGettingOn;
        selectedFloor.WaitingPassengers = numOfnumOfPeopleWaiting - numOfPeopleGettingOn;
      }

      return;
    }
    private int RemovePassengers(int numOfPeopleGettingOff, Elevator elevator)
    {
      int peopleRemainingOnElevator = elevator.PeopleCount - numOfPeopleGettingOff;
      int numOfPeopleWaitingOntheFloor = 1;
      return peopleRemainingOnElevator;
    }
  }

 
}
