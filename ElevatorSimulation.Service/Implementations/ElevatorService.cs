using ElevatorSimulator.DTOs;
using ElevatorSimulator.Common.Interfaces;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
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
    private const int elevatorTimeinSec = 1;//Time form one floor to the next assuming one second
    private readonly IConsole _console;

    public ElevatorService(IConsole console)
    {
      _console = console;
    }
    #region Public Methods
    public void ShowElevatorStatus(List<Elevator> elevators)
    {
      Console.WriteLine("Elevator Status:");
      foreach (var elevator in elevators)
      {
        Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor} - Status: {elevator.Status} , People Count: {elevator.PeopleCount}");
      }
    }

    public async Task<Elevator?> GetClosestElevator(List<Elevator> elevators, int destinationFloor)
    {
      Elevator? closestElevator = null;
      int minDistance = int.MaxValue;
      int minPeopleCount = int.MaxValue;

      foreach (var elevator in elevators)
      {
        if (elevator.Status == ElevatorStatus.Operational)
        {
          int distance = Math.Abs(elevator.CurrentFloor - destinationFloor);
          int peopleCount = elevator.PeopleCount;

          if (distance < minDistance || (distance == minDistance && peopleCount < minPeopleCount))
          {
            closestElevator = elevator;
            minDistance = distance;
            minPeopleCount = peopleCount;
          }
        }
      }

      return await Task.FromResult(closestElevator);
    }

    //public async Task CallElevator(List<Elevator> elevators,List<Floor> floors)
    //{
    //  _console.WriteLine("Enter the destination floor number:");
    //  int destinationFloor = Convert.ToInt32(_console.ReadLine());

    //  Elevator? selectedElevator = await GetClosestElevator(elevators, destinationFloor);
    //  Floor selectedFloor = floors.FirstOrDefault(floor => floor.FloorNumber == destinationFloor);
    //  Floor currentFloor = floors.FirstOrDefault(floor => floor.FloorNumber == selectedElevator.CurrentFloor);
    //  string elevatormMessage;
    //  string floorMessage;
    //  int numOfPeopleWaiting = 0;
    //  int peopleOnElevator = 0;

    //  if (selectedElevator != null)
    //  {
    //    _console.WriteLine("How many people are getting on?");
    //    int numPeopleGettingOn = Convert.ToInt32(_console.ReadLine());

    //    numPeopleGettingOn = Math.Min(currentFloor.WaitingPassengers, numPeopleGettingOn);
    //    floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleGettingOn, currentFloor);
    //    _console.WriteLine(floorMessage);
    //    var addPassengerResult = AddPassengers(numPeopleGettingOn, selectedElevator);
    //    peopleOnElevator = addPassengerResult.Item1;
    //    elevatormMessage = addPassengerResult.Item2;
    //    if (!String.IsNullOrEmpty(elevatormMessage))
    //    {
    //      _console.WriteLine(elevatormMessage);
    //      return;
    //    }


    //    selectedElevator.DestinationFloor = destinationFloor;

    //    _console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");
    //    MoveElevator(selectedElevator, elevatorTimeinSec); // Simulate elevator movement

    //    _console.WriteLine("How many people are getting off?");
    //    int peopleGettingOff = Convert.ToInt32(_console.ReadLine());
    //    if (peopleGettingOff > 0 && selectedElevator.PeopleCount == 0)
    //    {
    //      _console.WriteLine(" There are no people in the elevator to get off.");
    //    }
    //    else
    //    {
    //      peopleOnElevator = RemovePassengers(peopleGettingOff, selectedElevator);
    //    }

    //    _console.WriteLine($"Elevator {selectedElevator.Id}, {peopleGettingOff} people got off. {peopleOnElevator} remaining.");

    //    if (selectedFloor != null && selectedFloor.WaitingPassengers > 0)
    //    {
    //      numOfPeopleWaiting = selectedFloor.WaitingPassengers;
    //      _console.WriteLine($"There are  {selectedFloor.WaitingPassengers} people waiting on floor {selectedFloor.FloorNumber}");

    //      int maxNumPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount,numOfPeopleWaiting);
    //      _console.WriteLine($"How many people from the waiting group are getting on? (Up to {maxNumPeopleGettingOnFromWaiting})");

    //      int numPeopleGettingOnFromWaiting = Convert.ToInt32(_console.ReadLine());


    //      if (numPeopleGettingOnFromWaiting > maxNumPeopleGettingOnFromWaiting)
    //      {
    //        _console.WriteLine($"Those are more people than are waiting so only {maxNumPeopleGettingOnFromWaiting} will get on.");
    //        numPeopleGettingOnFromWaiting = maxNumPeopleGettingOnFromWaiting;
    //      }
    //        addPassengerResult = AddPassengers(numPeopleGettingOnFromWaiting, selectedElevator);
    //        floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleGettingOnFromWaiting, selectedFloor);
    //        peopleOnElevator = addPassengerResult.Item1;
    //        elevatormMessage = addPassengerResult.Item2;


    //      if (!String.IsNullOrEmpty(elevatormMessage))
    //      {
    //        _console.WriteLine(elevatormMessage);
    //        return;
    //      }
    //      _console.WriteLine($"Elevator {selectedElevator.Id} accommodated {numPeopleGettingOnFromWaiting} people from the waiting group.{floorMessage}");
    //    }

    //  }
    //  else
    //  {
    //    _console.WriteLine("No operational elevators available. Please try again later.");
    //  }
    //}
    public async Task CallElevator(List<Elevator> elevators, List<Floor> floors)
    {
      int destinationFloor = GetUserInput("Enter the destination floor number:");

   
      FloorService.ValidateDestinationFloor(floors, destinationFloor);

      Elevator? selectedElevator = await GetClosestElevator(elevators, destinationFloor);

      if (selectedElevator != null)
      {
        int numPeopleGettingOn = GetUserInput("How many people are getting on?");
        Floor currentFloor = floors.FirstOrDefault(floor => floor.FloorNumber == selectedElevator.CurrentFloor);
        int numPeopleToGetOn = Math.Min(currentFloor.WaitingPassengers, numPeopleGettingOn);

      

        var addPassengerResult = AddPassengers(numPeopleToGetOn, selectedElevator);
        int peopleOnElevator = addPassengerResult.Item1;
        string elevatormMessage = addPassengerResult.Item2;

        if (!String.IsNullOrEmpty(elevatormMessage))
        {
          _console.WriteLine(elevatormMessage);
          return;
        }
        string floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleToGetOn, currentFloor);
        _console.WriteLine(floorMessage);

        selectedElevator.DestinationFloor = destinationFloor;
        _console.WriteLine($"Elevator {selectedElevator.Id} called successfully!");

        MoveElevator(selectedElevator, elevatorTimeinSec);

        int peopleGettingOff = GetUserInput("How many people are getting off?");
        if (peopleGettingOff > 0 && selectedElevator.PeopleCount == 0)
        {
          _console.WriteLine("There are no people in the elevator to get off.");
        }
        else
        {
          peopleOnElevator = RemovePassengers(peopleGettingOff, selectedElevator);
        }

        _console.WriteLine($"Elevator {selectedElevator.Id}, {peopleGettingOff} people got off. {peopleOnElevator} remaining.");

        Floor selectedFloor = floors.FirstOrDefault(floor => floor.FloorNumber == destinationFloor);
        if (selectedFloor != null && selectedFloor.WaitingPassengers > 0)
        {
          int numPeopleWaiting = selectedFloor.WaitingPassengers;
          _console.WriteLine($"There are {numPeopleWaiting} people waiting on floor {selectedFloor.FloorNumber}");

          int maxNumPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount, numPeopleWaiting);
          int numPeopleGettingOnFromWaiting = GetUserInput($"How many people from the waiting group are getting on? (Up to {maxNumPeopleGettingOnFromWaiting})");

          if (numPeopleGettingOnFromWaiting > maxNumPeopleGettingOnFromWaiting)
          {
            _console.WriteLine($"Those are more people than are waiting, so only {maxNumPeopleGettingOnFromWaiting} will get on.");
            numPeopleGettingOnFromWaiting = maxNumPeopleGettingOnFromWaiting;
          }

          addPassengerResult = AddPassengers(maxNumPeopleGettingOnFromWaiting, selectedElevator);
          
          peopleOnElevator = addPassengerResult.Item1;
          elevatormMessage = addPassengerResult.Item2;

          if (!String.IsNullOrEmpty(elevatormMessage))
          {
            _console.WriteLine(elevatormMessage);
            return;
          }
          floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleGettingOnFromWaiting, selectedFloor);

          _console.WriteLine($"Elevator {selectedElevator.Id} accommodated {numPeopleGettingOnFromWaiting} people from the waiting group.{floorMessage}");
        }
      }
      else
      {
        _console.WriteLine("No operational elevators available. Please try again later.");
      }
    }




    public void SetElevatorStatus(List<Elevator> elevators)
    {
      _console.WriteLine("Enter the elevator number:");
      int elevatorNum;
      bool isValidElevatorNum = int.TryParse(_console.ReadLine(), out elevatorNum);

      if (isValidElevatorNum && elevatorNum > 0 && elevatorNum <= elevators.Count)
      {
        Elevator selectedElevator = elevators[elevatorNum - 1];

        _console.WriteLine("Enter the new status of the elevator:");
        _console.WriteLine("1. Operational");
        _console.WriteLine("2. OutOfOrder");

        int statusChoice;
        bool isValidStatusChoice = int.TryParse(_console.ReadLine(), out statusChoice);

        if (isValidStatusChoice)
        {
          switch (statusChoice)
          {
            case 1:
              selectedElevator.Status = ElevatorStatus.Operational;
              _console.WriteLine($"Elevator {elevatorNum} status updated to Operational successfully!");
              break;
            case 2:
              selectedElevator.Status = ElevatorStatus.OutOfOrder;
              _console.WriteLine($"Elevator {elevatorNum} status updated to OutOfOrder successfully!");
              break;
            default:
              _console.WriteLine("Invalid status choice. Please try again.");
              break;
          }
        }
        else
        {
          _console.WriteLine("Invalid status choice. Please try again.");
        }
      }
      else
      {
        _console.WriteLine("Invalid elevator number. Please try again.");
      }
    }

    #endregion Public Methods


    #region Private Methods
    private static void MoveElevator(Elevator selectedElevator, int elevatorTimeinSec)
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
          Sleep(elevatorTimeinSec);
        }
      }

      Console.WriteLine($"Elevator {selectedElevator.Id} has reached the destination floor: {selectedElevator.DestinationFloor}.");
    }
    private static void UpdateElevatorStatus(Elevator elevator, int elevatorTimeinSec)
    {
      Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor}, Direction: {elevator.Direction}, People Count: {elevator.PeopleCount}, Status: {elevator.Status}, ETA: {CalculateETA(elevator, elevatorTimeinSec)} seconds");
    }

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

    private static void Sleep(int elevatorTimeinSec)
    {
      Thread.Sleep(elevatorTimeinSec * 1000);
    }

    private int GetUserInput(string message)
    {
      _console.WriteLine(message);
      return Convert.ToInt32(_console.ReadLine());
    }

    #endregion
  }


}
