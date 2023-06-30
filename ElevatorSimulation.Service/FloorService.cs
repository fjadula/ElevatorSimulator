using ElevatorSimulator.Models.BO;
using System;

namespace ElevatorSimulator.Service
{
    #region interfaces
    public interface IFloorService
    {
        //interfaces
    }
    #endregion interfaces
    public class FloorService
    {

        #region Public Methds
        public void ManageWaitingPassengersOnFloor(List<Floor> floors)
        {
            Console.WriteLine("Enter the floor number(Ground is 0):");
            int floorNum = Convert.ToInt32(Console.ReadLine());
            if (floorNum >= 0 && floorNum < floors.Count)
            {
                Floor selectedFloor;

                selectedFloor = floors[floorNum];

                Console.WriteLine("What would like to do?:");
                Console.WriteLine("1. Add waiting people");
                Console.WriteLine("2. Remove waiting people");
                int actionChoice = Convert.ToInt32(Console.ReadLine());
                ;

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
                        Console.WriteLine(RemoveWaitingPassengersFromFloor(numofWaitingPeopleToRemove, selectedFloor));

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
        public static void AddWaitingPassengersToFloor(int numofPeopleWaiting, Floor selectedFloor)
        {

            selectedFloor.WaitingPassengers = selectedFloor.WaitingPassengers + numofPeopleWaiting;
            Console.WriteLine($"There are now {selectedFloor.WaitingPassengers} people waiting on floor {selectedFloor.FloorNumber}");
        }
        public static string RemoveWaitingPassengersFromFloor(int numofPeopleToRemove, Floor selectedFloor)
        {
            string message;
            if (numofPeopleToRemove > selectedFloor.WaitingPassengers)
            {
                numofPeopleToRemove = selectedFloor.WaitingPassengers;
            }
            selectedFloor.WaitingPassengers = selectedFloor.WaitingPassengers - numofPeopleToRemove;
            message = $"There are now {selectedFloor.WaitingPassengers} people waiting on floor {selectedFloor.FloorNumber}";
            return message;
        }

        public static void ShowFloorStatus(List<Floor> floors)
        {
            Console.WriteLine("Floor Status:");
            foreach (var floor in floors)
            {
                Console.WriteLine($"Floor number {floor.FloorNumber}, People Waiting Count: {floor.WaitingPassengers}");
            }
        }

        public static void ValidateDestinationFloor(List<Floor> floors, int destinationFloor)
        {
            if (!floors.Any(floor => floor.FloorNumber == destinationFloor))
            {
                Console.WriteLine("Invalid floor number.");
                return;
            }
        }
        #endregion
    }


}



