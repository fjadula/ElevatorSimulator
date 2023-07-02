using System;
using ElevatorSimulator.Models.Enums;
using static ElevatorSimulator.Common.Constants;

namespace ElevatorSimulator.Models.BO
{

  public class Elevator
  {

    private static int nextId = 1;
    public int Id { get; set; }
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


    public static string WeightLimitManagement(int numOfPeopleGettingOn, Elevator selectedElevator)
    {
      string message = string.Empty;
      if (numOfPeopleGettingOn > selectedElevator.WeightLimit - selectedElevator.PeopleCount)
      {
        message = Messages.WeightExceed;
        return message;
      }

      return message;

    }
  }
}
