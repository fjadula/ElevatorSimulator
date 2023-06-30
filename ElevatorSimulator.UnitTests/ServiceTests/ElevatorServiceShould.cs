using ElevatorSimulator.Common.Interfaces;
using ElevatorSimulator.HelperMethods;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using ElevatorSimulator.Service;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static ElevatorSimulator.HelperMethods.ConsoleHelpers;

namespace ElevatorSimulator.ServiceTests
{
  public class ElevatorServiceShould
  {

    [Fact]
    public async Task ReturnClosestElevator_WhenMultipleElevatorsAreAvailable()
    {
      var elevators = new List<Elevator>
    {
        new Elevator { Id = 1, CurrentFloor = 3, Status = ElevatorStatus.Operational, PeopleCount = 2 },
        new Elevator { Id = 2, CurrentFloor = 5, Status = ElevatorStatus.Operational, PeopleCount = 0 },
        new Elevator { Id = 3, CurrentFloor = 2, Status = ElevatorStatus.Operational, PeopleCount = 3 },
    };

      var destinationFloor = 4;
      var expectedElevatorId = 2;

      var consoleMock = new Mock<IConsole>();
      var elevatorService = new ElevatorService(consoleMock.Object);

      var closestElevator = await elevatorService.GetClosestElevator(elevators, destinationFloor);

      Assert.NotNull(closestElevator);
      Assert.Equal(expectedElevatorId, closestElevator.Id);
    }

    [Fact]
    public void DisplayElevatorStatus()
    {
      var elevators = new List<Elevator>
            {
                new Elevator { Id = 1, CurrentFloor = 3, Status = ElevatorStatus.Operational, PeopleCount = 2 },
                new Elevator { Id = 2, CurrentFloor = 5, Status = ElevatorStatus.Operational, PeopleCount = 0 },
                new Elevator { Id = 3, CurrentFloor = 2, Status = ElevatorStatus.Operational, PeopleCount = 4 },
            };

      var expectedOutput = "Elevator Status:\r\n" +
                           "Elevator 1 - Current Floor: 3 - Status: Operational , People Count: 2\r\n" +
                           "Elevator 2 - Current Floor: 5 - Status: Operational , People Count: 0\r\n" +
                           "Elevator 3 - Current Floor: 2 - Status: Operational , People Count: 4\r\n";

      var consoleMock = new Mock<IConsole>();
      var elevatorService = new ElevatorService(consoleMock.Object);


      var consoleOutput = CaptureConsoleOutput(() =>
      {
        elevatorService.ShowElevatorStatus(elevators);
      });

      Assert.Equal(expectedOutput, consoleOutput, ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task CallElevator_ValidInput_Success()
    {

      var elevators = new List<Elevator>
        {
            new Elevator { Id = 1, CurrentFloor = 3, Status = ElevatorStatus.Operational },
            new Elevator { Id = 2, CurrentFloor = 5, Status = ElevatorStatus.Operational },
            new Elevator { Id = 3, CurrentFloor = 2, Status = ElevatorStatus.Operational },
        };

      var floor1 = new Floor { FloorNumber = 1 };
      var floor2 = new Floor { FloorNumber = 2 };
      var floors = new List<Floor> { floor1, floor2 };

      var consoleMock = new Mock<IConsole>();
      var elevatorService = new ElevatorService(consoleMock.Object);

      var consoleInputQueue = new Queue<string>(new[] { "2", "1", "2", "1", "1" });

  
      string output = CaptureConsoleOutput(async () =>
      {
        using (var consoleInput = new ConsoleInput(consoleInputQueue))
        {
          await elevatorService.CallElevator(elevators, floors);
        }
      });

      
      Assert.Contains("Elevator 3 is moving from floor 2 to floor 0...", output);
      Assert.Contains("Elevator 3 has reached the destination floor: 0.", output);

      consoleMock.Verify(c => c.WriteLine(It.IsAny<string>()), Times.AtLeastOnce);
      consoleMock.Verify(c => c.ReadLine(), Times.AtLeastOnce);
    }
  

  [Fact]
    public void SetElevatorStatus_ShouldUpdateElevatorStatus_WhenValidInputProvided()
    {

      var consoleMock = new Mock<IConsole>();
      var elevatorService = new ElevatorService(consoleMock.Object);

      var elevators = new List<Elevator>
            {
                new Elevator { Id = 1, Status = ElevatorStatus.Operational },
                new Elevator { Id = 2, Status = ElevatorStatus.Operational },
                new Elevator { Id = 3, Status = ElevatorStatus.Operational }
            };

      consoleMock.SetupSequence(c => c.ReadLine())
          .Returns("2") // Elevator number
          .Returns("2"); // Status choice


      elevatorService.SetElevatorStatus(elevators);

      // Assert
      Assert.Equal(ElevatorStatus.OutOfOrder, elevators[1].Status);
      consoleMock.Verify(c => c.WriteLine("Elevator 2 status updated to OutOfOrder successfully!"), Times.Once);
    }

    [Fact]
    public void SetElevatorStatus_ShouldNotUpdateElevatorStatus_WhenInvalidElevatorNumberProvided()
    {
      // Arrange
      var consoleMock = new Mock<IConsole>();
      var elevatorService = new ElevatorService(consoleMock.Object);

      var elevators = new List<Elevator>
            {
                new Elevator { Id = 1, Status = ElevatorStatus.Operational },
                new Elevator { Id = 2, Status = ElevatorStatus.Operational },
                new Elevator { Id = 3, Status = ElevatorStatus.Operational }
            };

      consoleMock.SetupSequence(c => c.ReadLine())
          .Returns("4") // Invalid elevator number
          .Returns("2"); // Status choice


      elevatorService.SetElevatorStatus(elevators);


      Assert.Equal(ElevatorStatus.Operational, elevators[1].Status);
      consoleMock.Verify(c => c.WriteLine("Invalid elevator number. Please try again."), Times.Once);
    }

  }


}


