using ElevatorSimulator.Models.BO;
using System;
using static ElevatorSimulator.Common.Constants;

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
            Console.WriteLine(Input.RequestFloorNumber);
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
                        Console.WriteLine(Input.PeopleAddingtoFloor);
                        int numofPeopleWaiting = Convert.ToInt32(Console.ReadLine());
                        AddWaitingPassengersToFloor(numofPeopleWaiting, selectedFloor);
                        break;

                    case 2:
                        Console.WriteLine(Input.PeopleRemoveFromFloor);
                        int numofWaitingPeopleToRemove = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(RemoveWaitingPassengersFromFloor(numofWaitingPeopleToRemove, selectedFloor));

                        break;

                    default:
                        Console.WriteLine(Messages.Error);
                        break;
                }
            }
            else
            {
                Console.WriteLine(Messages.InvalidFloor);
            }
        }
        public static void AddWaitingPassengersToFloor(int numofPeopleWaiting, Floor selectedFloor)
        {

            selectedFloor.WaitingPassengers = selectedFloor.WaitingPassengers + numofPeopleWaiting;
            Console.WriteLine(Messages.ReturnNoPeopleOntheFloor,selectedFloor.WaitingPassengers,selectedFloor.FloorNumber);
        }
        public static string RemoveWaitingPassengersFromFloor(int numofPeopleToRemove, Floor selectedFloor)
        {
            string message;
            if (numofPeopleToRemove > selectedFloor.WaitingPassengers)
            {
                numofPeopleToRemove = selectedFloor.WaitingPassengers;
            }
            selectedFloor.WaitingPassengers = selectedFloor.WaitingPassengers - numofPeopleToRemove;
            message= string.Format(Messages.ReturnNoPeopleOntheFloor,selectedFloor.WaitingPassengers,selectedFloor.FloorNumber);
         return message;
        }

        public static void ShowFloorStatus(List<Floor> floors)
        {
            Console.WriteLine(Messages.FloorStatusInfo);
            foreach (var floor in floors)
            {
                Console.WriteLine(Messages.ReturnNoPeopleOntheFloor , floor.WaitingPassengers,floor.FloorNumber );
            }
        }

        public static void ValidateDestinationFloor(List<Floor> floors, int destinationFloor)
        {
            if (!floors.Any(floor => floor.FloorNumber == destinationFloor))
            {
                Console.WriteLine(Messages.InvalidFloor);
                return;
            }
        }
        #endregion
    }


}



