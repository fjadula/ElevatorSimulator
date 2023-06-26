using ElevatorSimulator.Models.Enums;
using System;

namespace ElevatorSimulator.Models.BO
{
  public class Elevator
  {
    private static int nextId = 1;

    public int Id { get; }
    public int CurrentFloor { get; set; }
    public Direction Direction { get; set; }
    public int PeopleCount { get; set; }
    public TimeSpan ExpectedTimeOfArrival { get; set; }
    public ElevatorStatus Status { get; set; }
    public int WeightLimit { get; set; }

    public Elevator()
    {
      Id = nextId++;
      CurrentFloor = 1;
      Status = ElevatorStatus.Operational;
      Direction = Direction.Stationary;
      WeightLimit = 10; // Default weight limit of 10 people
    }

    public void ShowStatus()
    {
      Console.WriteLine("Elevator Status:");
      Console.WriteLine($"ID: {Id}");
      Console.WriteLine($"Current Floor: {CurrentFloor}");
      Console.WriteLine($"Direction: {Direction}");
      Console.WriteLine($"People Count: {PeopleCount}");
      Console.WriteLine($"Expected Time of Arrival: {ExpectedTimeOfArrival}");
      Console.WriteLine($"Status: {Status}");
      Console.WriteLine($"Weight Limit: {WeightLimit} people");
    }
  }
}
