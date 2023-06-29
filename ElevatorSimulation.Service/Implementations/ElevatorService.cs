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
    private const int elevatorTimeinSec = 1;
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
      Floor selectedFloor = floors.FirstOrDefault(floor => floor.FloorNumber == destinationFloor);
      Floor currentFloor = floors.FirstOrDefault(floor => floor.FloorNumber == selectedElevator.CurrentFloor);
      string elevatormMessage;
      string floorMessage;
      int numOfPeopleWaiting = 0;
      int peopleOnElevator = 0;

      if (selectedElevator != null)
      {
        Console.WriteLine("How many people are getting on?");
        int numPeopleGettingOn = Convert.ToInt32(Console.ReadLine());

        numPeopleGettingOn = Math.Min(selectedFloor.WaitingPassengers, numPeopleGettingOn);
        floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleGettingOn, currentFloor);
        Console.WriteLine(floorMessage);
        var addPassengerResult = AddPassengers(numPeopleGettingOn, selectedElevator);
        peopleOnElevator = addPassengerResult.Item1;
        elevatormMessage = addPassengerResult.Item2;
        if (!String.IsNullOrEmpty(elevatormMessage))
        {
          Console.WriteLine(elevatormMessage);
          return;
        }

       
        selectedElevator.DestinationFloor = destinationFloor;

        Console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");
        MoveElevator(selectedElevator, elevatorTimeinSec); // Simulate elevator movement

        Console.WriteLine("How many people are getting off?");
        int peopleGettingOff = Convert.ToInt32(Console.ReadLine());
        if (peopleGettingOff > 0 && selectedElevator.PeopleCount == 0)
        {
          Console.WriteLine(" There are no people in the elevator to get off.");
        }
        else
        {
          peopleOnElevator = RemovePassengers(peopleGettingOff, selectedElevator);
        }

        Console.WriteLine($"Elevator {selectedElevator.Id}, {peopleGettingOff} people got off. {peopleOnElevator} remaining.");

        if (selectedFloor != null && selectedFloor.WaitingPassengers > 0)
        {
          numOfPeopleWaiting = selectedFloor.WaitingPassengers;
          Console.WriteLine($"There are  {selectedFloor.WaitingPassengers} people waiting on floor {selectedFloor.FloorNumber}");

          int maxNumPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount,numOfPeopleWaiting);
          Console.WriteLine($"How many people from the waiting group are getting on? (Up to {maxNumPeopleGettingOnFromWaiting})");

          int numPeopleGettingOnFromWaiting = Convert.ToInt32(Console.ReadLine());
     
          
          if (numPeopleGettingOnFromWaiting > maxNumPeopleGettingOnFromWaiting)
          {
            Console.WriteLine($"Those are more people than are waiting so only {maxNumPeopleGettingOnFromWaiting} will get on.");
            numPeopleGettingOnFromWaiting = maxNumPeopleGettingOnFromWaiting;
          }
            addPassengerResult = AddPassengers(numPeopleGettingOnFromWaiting, selectedElevator);
            floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleGettingOnFromWaiting, selectedFloor);
            peopleOnElevator = addPassengerResult.Item1;
            elevatormMessage = addPassengerResult.Item2;
       

          if (!String.IsNullOrEmpty(elevatormMessage))
          {
            Console.WriteLine(elevatormMessage);
            return;
          }
          Console.WriteLine($"Elevator {selectedElevator.Id} accommodated {numPeopleGettingOnFromWaiting} people from the waiting group.{floorMessage}");
        }

      }
      else
      {
        Console.WriteLine("No operational elevators available. Please try again later.");
      }
    }

    public  void SetElevatorStatus(List<Elevator> elevators)
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

    private static void MoveElevator(Elevator selectedElevator,int elevatorTimeinSec)
    {

      Console.WriteLine($"Elevator {selectedElevator.Id} is moving from floor {selectedElevator.CurrentFloor} to floor {selectedElevator.DestinationFloor}...");
      int distance = Math.Abs(selectedElevator.CurrentFloor - selectedElevator.DestinationFloor);
      int travelTimeInSeconds = distance * elevatorTimeinSec;
      int initialETA = travelTimeInSeconds;
      selectedElevator.Direction = selectedElevator.CurrentFloor < selectedElevator.DestinationFloor ? Direction.Up : Direction.Down;


      Console.WriteLine($"Elevator {selectedElevator.Id} - Current Floor: {selectedElevator.CurrentFloor}, Direction: {selectedElevator.Direction}, People Count: {selectedElevator.PeopleCount}, Status: {selectedElevator.Status}, ETA: {initialETA} seconds");
      System.Threading.Thread.Sleep(elevatorTimeinSec * 1000);
      for (int i = 0; i < distance; i++)
      {
        selectedElevator.CurrentFloor += selectedElevator.Direction == Direction.Up ? 1 : -1;
        if (selectedElevator.CurrentFloor == selectedElevator.DestinationFloor)
        {
          selectedElevator.Direction = Direction.Stationary;
        }
        UpdateElevatorStatus(selectedElevator, elevatorTimeinSec);
        if (selectedElevator.CurrentFloor != selectedElevator.DestinationFloor)
        {
          System.Threading.Thread.Sleep(elevatorTimeinSec * 1000);
        }
      }

      Console.WriteLine($"Elevator {selectedElevator.Id} has reached the destination floor: {selectedElevator.DestinationFloor}.");
    }
    private static void UpdateElevatorStatus(Elevator elevator,int elevatorTimeinSec)
    {
      Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor}, Direction: {elevator.Direction}, People Count: {elevator.PeopleCount}, Status: {elevator.Status}, ETA: {CalculateETA(elevator, elevatorTimeinSec)} seconds");
    }

   
    #region Private Methods
    private static Tuple<int,string> AddPassengers(int numOfPeopleGettingOn, Elevator selectedElevator)
    {
      string weightLimitedExceeded;
      int peopleOnElevator = 0;
      weightLimitedExceeded = Elevator.WeightLimitManagement(numOfPeopleGettingOn, selectedElevator);
      Tuple <int, string> returnedValues = Tuple.Create(peopleOnElevator, weightLimitedExceeded);
    

      if (String.IsNullOrEmpty(weightLimitedExceeded)) 
      {
        peopleOnElevator = selectedElevator.PeopleCount += numOfPeopleGettingOn;
        return returnedValues;
      }

      return returnedValues;

    }
    private int RemovePassengers(int numOfPeopleGettingOff, Elevator selectedElevator)
    {
      int peopleRemainingOnElevator = selectedElevator.PeopleCount - Math.Min(selectedElevator.PeopleCount, numOfPeopleGettingOff);
      selectedElevator.PeopleCount = peopleRemainingOnElevator;
      return peopleRemainingOnElevator;
    }

    private static int CalculateETA(Elevator elevator, int elevatorTimeinSec)
    {
      int distance = Math.Abs(elevator.CurrentFloor - elevator.DestinationFloor);
      int travelTimeInSeconds = distance * elevatorTimeinSec;
      return travelTimeInSeconds;

    }

    #endregion
  }


}
