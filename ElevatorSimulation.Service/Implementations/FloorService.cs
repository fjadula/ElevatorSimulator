using ElevatorSimulator.Models.BO;
using System.Runtime.InteropServices;

namespace ElevatorSimulator.Service.Implementations
{

  public interface IFloorService
  {
    void AddWaitingPassengers(int numofPeopleWaiting);
  }
  public class FloorService
  {

    public void ManageWaitingPassengersOnFloor(List<Floor> floors)
    {
      Console.WriteLine("Enter the floor number:");
      int floorNum = Convert.ToInt32(Console.ReadLine());
      if (floorNum >= 0 && floorNum <= floors.Count)
      {
        Floor selectedFloor = floors[floorNum];

        Console.WriteLine("What would like to do?:");
        Console.WriteLine("1. Add waiting people");
        Console.WriteLine("2. Remove waiting people");
        int actionChoice = Convert.ToInt32(Console.ReadLine());

        switch (actionChoice)
        {
          case 1:
            Console.WriteLine("How many people do you want to add to the floor?");
            int numofPeopleWaiting = Convert.ToInt32(Console.ReadLine());
            AddWaitingPassengersToFloor(numofPeopleWaiting, selectedFloor);
            break;

          case 2:
            Console.WriteLine("How many people do you want to remove from the floor?");
            int numofWaitingPeopleToRemove = Convert.ToInt32(Console.ReadLine());
            RemoveWaitingPassengersFromFloor(numofWaitingPeopleToRemove,selectedFloor);
            break;

          default:
            Console.WriteLine("Invalid action choice. Please try again.");
            break;
        }
      }
      else
      {
        Console.WriteLine("Invalid floor number. Please try again.");
      }
    }
    public static void AddWaitingPassengersToFloor(int numofPeopleWaiting,Floor selectedFloor)
    {

      selectedFloor.WaitingPassengers = selectedFloor.WaitingPassengers + numofPeopleWaiting;
      Console.WriteLine($"There are now {selectedFloor.WaitingPassengers} people waiting on floor {selectedFloor.FloorNumber}");
    }
    public static void RemoveWaitingPassengersFromFloor(int numofPeopleToRemove, Floor selectedFloor)
    {

      selectedFloor.WaitingPassengers = selectedFloor.WaitingPassengers - Math.Min(selectedFloor.WaitingPassengers, numofPeopleToRemove);
      Console.WriteLine($"There are now {selectedFloor.WaitingPassengers} people waiting on floor {selectedFloor.FloorNumber}");

    }
  }
  
}



