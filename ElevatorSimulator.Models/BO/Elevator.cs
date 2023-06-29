using System;
using System.Collections.Generic;
using ElevatorSimulator.Models.Enums;

namespace ElevatorSimulator.Models.BO
{

  public class Elevator
  {
 
    public int Id { get; }
    public int CurrentFloor { get; set; }
    public int DestinationFloor { get; set; }
    public Direction Direction { get; set; }
    public int PeopleCount { get; set; }
    public TimeSpan ExpectedTimeOfArrival { get; set; }
    public ElevatorStatus Status { get; set; }
    public int WeightLimit { get; set; }

    public Elevator()
    {
      Id = 1;
      CurrentFloor = 0;
      DestinationFloor = 0;
      PeopleCount = 0;
      Status = ElevatorStatus.Operational;
      Direction = Direction.Stationary;
      WeightLimit = 10; // Default weight limit of 10 people
    }

    //public void MoveElevator()
    //{
 
    //  Console.WriteLine($"Elevator {Id} is moving from floor {CurrentFloor} to floor {DestinationFloor}...");
    //  int distance = Math.Abs(CurrentFloor - DestinationFloor);
    //  int travelTimeInSeconds = distance * elevatorTimeinSec;
    //  int initialETA = travelTimeInSeconds;
    //  Direction = CurrentFloor < DestinationFloor ? Direction.Up : Direction.Down;
      

    //  Console.WriteLine($"Elevator {Id} - Current Floor: {CurrentFloor}, Direction: {Direction}, People Count: {PeopleCount}, Status: {Status}, ETA: {initialETA} seconds");
    //  System.Threading.Thread.Sleep(elevatorTimeinSec * 1000);
    //  for (int i = 0; i < distance; i++)
    //  {
    //    CurrentFloor += Direction == Direction.Up ? 1 : -1;
    //    if (CurrentFloor == DestinationFloor)
    //    {
    //      Direction = Direction.Stationary;
    //    }
    //    UpdateElevatorStatus(this);
    //    if (CurrentFloor != DestinationFloor)
    //    {
    //      System.Threading.Thread.Sleep(elevatorTimeinSec * 1000);
    //    }
    //  }

    //  Console.WriteLine($"Elevator {Id} has reached the destination floor: {DestinationFloor}.");
    //}
    //private static void UpdateElevatorStatus(Elevator elevator)
    //{
    //  Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor}, Direction: {elevator.Direction}, People Count: {elevator.PeopleCount}, Status: {elevator.Status}, ETA: {CalculateETA(elevator)} seconds");
    //}

    //private static int CalculateETA(Elevator elevator)
    //{
    //  int distance = Math.Abs(elevator.CurrentFloor - elevator.DestinationFloor);
    //  int travelTimeInSeconds = distance * elevatorTimeinSec;
    //  return travelTimeInSeconds;

    //}

    public static string WeightLimitManagement(int numOfPeopleGettingOn, Elevator selectedElevator)
    {
      string message =string.Empty;
      if (numOfPeopleGettingOn > selectedElevator.WeightLimit - selectedElevator.PeopleCount)
      {
        message = "Weight limit exceeded. Cannot accommodate that many people."; ;
        return message;
      }

      return message;

    }
  }
}
