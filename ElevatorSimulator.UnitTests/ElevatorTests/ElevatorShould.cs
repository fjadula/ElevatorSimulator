using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
using Xunit;

namespace ElevatorSimulator
{
  public class ElevatorShould
  {
    [Fact]
    public void BeWithinWeightLimit_ReturnsEmptyMessage()
    {
      var elevator = new Elevator
      {
        WeightLimit = 10,
        PeopleCount = 5
      };
      int numOfPeopleGettingOn = 3;

      string message = Elevator.WeightLimitManagement(numOfPeopleGettingOn, elevator);

      Assert.Equal(string.Empty, message);
    }

    [Fact]
    public void ExceedsWeightLimit_ReturnsErrorMessage()
    {
      var elevator = new Elevator
      {
        WeightLimit = 10,
        PeopleCount = 7
      };
      int numOfPeopleGettingOn = 5;

      string message = Elevator.WeightLimitManagement(numOfPeopleGettingOn, elevator);

      Assert.Equal("Weight limit exceeded. Cannot accommodate that many people.", message);
    }
  }
}