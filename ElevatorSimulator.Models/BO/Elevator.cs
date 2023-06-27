using System;
using System.Collections.Generic;
using ElevatorSimulator.Models.Enums;

namespace ElevatorSimulator.Models.BO
{
  public class Elevator
  {
    private static int nextId = 1;

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
      Id = nextId++;
      CurrentFloor = 0;
      DestinationFloor = 0;
      PeopleCount = 0;
      Status = ElevatorStatus.Operational;
      Direction = Direction.Stationary;
      WeightLimit = 10; // Default weight limit of 10 people
    }



    public void MoveElevator()
    {
      Console.WriteLine($"Elevator {Id} is moving from floor {CurrentFloor} to floor {DestinationFloor}...");
      int distance = Math.Abs(CurrentFloor - DestinationFloor);
      int travelTimeInSeconds = distance * 2; // Assuming 2 seconds for each floor
      Direction = CurrentFloor < DestinationFloor ? Direction.Up : Direction.Down;
      int initialETA = travelTimeInSeconds;

      if (Direction == Direction.Up)
      {
        initialETA -= (CurrentFloor - 1) * 2;
      }
      else
      {
        initialETA -= (distance - (CurrentFloor - 1)) * 2;
      }

      Console.WriteLine($"Elevator {Id} - Current Floor: {CurrentFloor}, Direction: {Direction}, People Count: {PeopleCount}, Status: {Status}, ETA: {initialETA} seconds");

      for (int i = 0; i < distance; i++)
      {
        CurrentFloor += Direction == Direction.Up ? 1 : -1;
        if (CurrentFloor == DestinationFloor)
        {
          Direction = Direction.Stationary;
        }
        UpdateElevatorStatus(this);
        System.Threading.Thread.Sleep(2000);
      }

      Console.WriteLine($"Elevator {Id} has reached the destination floor: {DestinationFloor}.");
    }

    public static void UpdateElevatorStatus(List<Elevator> elevators)
    {
      foreach (var elevator in elevators)
      {
        UpdateElevatorStatus(elevator);
      }
    }

    public void UpdateElevatorStatus()
    {
      Console.WriteLine($"Elevator {Id} - Current Floor: {CurrentFloor}, Direction: {Direction}, People Count: {PeopleCount}, Status: {Status}, ETA: {CalculateETA()} seconds");
    }

    public static void UpdateElevatorStatus(Elevator elevator)
    {
     // Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor}, Direction: {elevator.Direction}, People Count: {elevator.PeopleCount}, Status: {elevator.Status}, ETA: {CalculateETA(elevator)} seconds");
    }

    public int CalculateETA()
    {
      int distance = Math.Abs(CurrentFloor - DestinationFloor);
      int travelTimeInSeconds = distance * 2; // Assuming 2 seconds for each floor
      return travelTimeInSeconds;
    }
  }
}
