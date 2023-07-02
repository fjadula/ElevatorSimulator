using ElevatorSimulator.Common.Interfaces;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Service;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace ElevatorSimulator.ServiceTests
{
    public class FloorServiceShould
  {
    [Fact]
    public void AddWaitingPassengersToFloor_ValidInput_Success()
    {
      var floor = new Floor { FloorNumber = 1, WaitingPassengers = 0 };
      int numPeopleWaiting = 5;

      FloorService.AddWaitingPassengersToFloor(numPeopleWaiting, floor);

      Assert.Equal(numPeopleWaiting, floor.WaitingPassengers);
    }

    [Fact]
    public void RemoveWaitingPassengersFromFloor_ValidInput_Success()
    {
      var floor = new Floor { FloorNumber = 1, WaitingPassengers = 10 };
      int numPeopleToRemove = 5;

      string message = FloorService.RemoveWaitingPassengersFromFloor(numPeopleToRemove, floor);

      Assert.Equal(5, floor.WaitingPassengers);
      Assert.Equal("There are now 5 people waiting on floor 1", message);
    }

    [Fact]
    public void RemoveWaitingPassengersFromFloor_MoreThanAvailable_RemovesAllPassengers()
    {
      var floor = new Floor { FloorNumber = 1, WaitingPassengers = 5 };
      int numPeopleToRemove = 10;

      string message = FloorService.RemoveWaitingPassengersFromFloor(numPeopleToRemove, floor);

      Assert.Equal(0, floor.WaitingPassengers);
      Assert.Equal("There are now 0 people waiting on floor 1", message);
    }

    [Fact]
    public void ShowFloorStatus_ValidData_PrintsFloorStatus()
    {
      var floors = new List<Floor>
            {
                new Floor { FloorNumber = 0, WaitingPassengers = 3 },
                new Floor { FloorNumber = 1, WaitingPassengers = 5 },
                new Floor { FloorNumber = 2, WaitingPassengers = 0 }
            };

      using (var consoleOutput = new StringWriter())
      {
        Console.SetOut(consoleOutput);

        FloorService.ShowFloorStatus(floors);

        string output = consoleOutput.ToString();

        Assert.Contains("There are now 3 people waiting on floor 0", output);
        Assert.Contains("There are now 5 people waiting on floor 1", output);
        Assert.Contains("There are now 0 people waiting on floor 2", output);
      }
    }
    [Fact]
    public void ValidateDestinationFloor_InvalidFloorNumber_PrintsErrorMessage()
    {
      var floors = new List<Floor>
    {
        new Floor { FloorNumber = 0 },
        new Floor { FloorNumber = 1 },
        new Floor { FloorNumber = 2 }
    };

      int destinationFloor = 3;

      using (var consoleOutput = new StringWriter())
      {
        Console.SetOut(consoleOutput);

        FloorService.ValidateDestinationFloor(floors, destinationFloor);

        string output = consoleOutput.ToString();

        Assert.Contains("Invalid floor number. Please try again.", output);
      }
    }

    [Fact]
    public void ValidateDestinationFloor_ValidFloorNumber_DoesNotPrintErrorMessage()
    {

      var floors = new List<Floor>
            {
                new Floor { FloorNumber = 0 },
                new Floor { FloorNumber = 1 },
                new Floor { FloorNumber = 2 }
            };

      int destinationFloor = 1;

      using (var consoleOutput = new StringWriter())
      {
        string output = consoleOutput.ToString();

        FloorService.ValidateDestinationFloor(floors, destinationFloor);

        Assert.DoesNotContain("Invalid floor number.", output);
      }
    }

  }


}

