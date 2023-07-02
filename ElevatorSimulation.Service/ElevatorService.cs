using ElevatorSimulator.Common.Interfaces;
using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
using static ElevatorSimulator.Common.Constants;

namespace ElevatorSimulator.Service
{

    public interface IElevatorService
    {
        void ShowElevatorStatus(List<Elevator> elevators);
        Task<Elevator?> GetClosestElevator(List<Elevator> elevators, int destinationFloor);
        Task CallElevator(List<Elevator> elevators, List<Floor> floors);
    }



    public class ElevatorService : IElevatorService
    {
        private const int elevatorTimeinSec = 1;//Time from one floor to the next assuming one second
        private readonly IConsole _console;

        public ElevatorService(IConsole console)
        {
            _console = console;
        }
        #region Public Methods
        public void ShowElevatorStatus(List<Elevator> elevators)
        {
            Console.WriteLine(Messages.ElevatorStatusInfo);
            foreach (var elevator in elevators)
            {
                Console.WriteLine(Messages.StationaryElevator,elevator.Id,elevator.CurrentFloor, elevator.Status , elevator.PeopleCount, elevator.Direction);
            }
        }

        public async Task<Elevator?> GetClosestElevator(List<Elevator> elevators, int destinationFloor)
        {
            Elevator? closestElevator = null;
            int minDistance = int.MaxValue;
            int minPeopleCount = int.MaxValue;

            foreach (var elevator in elevators)
            {
                if (elevator.Status == ElevatorStatus.Operational)
                {
                    int distance = Math.Abs(elevator.CurrentFloor - destinationFloor);
                    int peopleCount = elevator.PeopleCount;

                    if (distance < minDistance || distance == minDistance && peopleCount < minPeopleCount)
                    {
                        closestElevator = elevator;
                        minDistance = distance;
                        minPeopleCount = peopleCount;
                    }
                }
            }

            return await Task.FromResult(closestElevator);
        }

        public async Task CallElevator(List<Elevator> elevators, List<Floor> floors)
        {
            int destinationFloor = GetUserInput(Input.DestinationFloor);
            string message = string.Empty;

            FloorService.ValidateDestinationFloor(floors, destinationFloor);

            Elevator? selectedElevator = await GetClosestElevator(elevators, destinationFloor);

            if (selectedElevator != null)
            {
                int numPeopleGettingOn = GetUserInput(Input.PeopleGettingOn);
                Floor currentFloor = floors.FirstOrDefault(floor => floor.FloorNumber == selectedElevator.CurrentFloor);
                int numPeopleToGetOn = Math.Min(currentFloor.WaitingPassengers, numPeopleGettingOn);

                var addPassengerResult = AddPassengers(numPeopleToGetOn, selectedElevator);
                int peopleOnElevator = addPassengerResult.Item1;
                string elevatormMessage = addPassengerResult.Item2;

                if (!string.IsNullOrEmpty(elevatormMessage))
                {
                    _console.WriteLine(elevatormMessage);
                    return;
                }
                string floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleToGetOn, currentFloor);
                _console.WriteLine(floorMessage);

                selectedElevator.DestinationFloor = destinationFloor;
                Console.WriteLine(Messages.ElevatorCalled, selectedElevator.Id);

                MoveElevator(selectedElevator, elevatorTimeinSec);

                int peopleGettingOff = GetUserInput(Input.PeopleGettingOff);
                if (peopleGettingOff > 0 && selectedElevator.PeopleCount == 0)
                {
                    _console.WriteLine(Messages.PeopletoGetOff);
                }
                else
                {
                    peopleOnElevator = RemovePassengers(peopleGettingOff, selectedElevator);
        }
        message = string.Format(Messages.PeopleOffPeopleRemaining, selectedElevator.Id, peopleGettingOff, peopleOnElevator);

        _console.WriteLine(message);

                Floor selectedFloor = floors.FirstOrDefault(floor => floor.FloorNumber == destinationFloor);
                if (selectedFloor != null && selectedFloor.WaitingPassengers > 0)
                {
                    int numPeopleWaiting = selectedFloor.WaitingPassengers;
                    
                    message = string.Format(Messages.ReturnNoPeopleOntheFloor, numPeopleWaiting, selectedFloor.FloorNumber);
                    _console.WriteLine(message);
          message = Messages.StatusUpdated;
          _console.WriteLine(message);

          int maxNumPeopleGettingOnFromWaiting = Math.Min(selectedElevator.WeightLimit - selectedElevator.PeopleCount, numPeopleWaiting);
                    message = string.Format(Input.PeopleGettingOn + Input.UpTo, maxNumPeopleGettingOnFromWaiting);
                    int numPeopleGettingOnFromWaiting = GetUserInput(message);

                    //if (numPeopleGettingOnFromWaiting > maxNumPeopleGettingOnFromWaiting)
                    //{
                    //     message = string.Format(Messages.MoreGetOnThanWaiting,maxNumPeopleGettingOnFromWaiting);
                    //    _console.WriteLine(message);
                    //    numPeopleGettingOnFromWaiting = maxNumPeopleGettingOnFromWaiting;
                    //}

                    addPassengerResult = AddPassengers(numPeopleGettingOn, selectedElevator);

                    peopleOnElevator = addPassengerResult.Item1;
                    elevatormMessage = addPassengerResult.Item2;

                    if (!string.IsNullOrEmpty(elevatormMessage))
                    {
                        _console.WriteLine(elevatormMessage);
                        return;
                    }
                    floorMessage = FloorService.RemoveWaitingPassengersFromFloor(numPeopleGettingOnFromWaiting, selectedFloor);
                    message = string.Format(Messages.HowManyGotOn, selectedElevator.Id, numPeopleGettingOnFromWaiting,selectedFloor.FloorNumber,Environment.NewLine+ floorMessage);
                    _console.WriteLine(message);
                }
            }
            else
            {
                _console.WriteLine(Messages.NoOperationalElevators);
            }
        }

       public void SetElevatorStatus(List<Elevator> elevators)
{
    Console.WriteLine(Input.SelectElevatorNumber);
    int elevatorNum;
    bool isValidElevatorNum = int.TryParse(Console.ReadLine(), out elevatorNum);

    if (isValidElevatorNum && elevatorNum > 0 && elevatorNum <= elevators.Count)
    {
        Elevator selectedElevator = elevators[elevatorNum - 1];

        Console.WriteLine(Messages.SelectElevatorStatus);
        Console.WriteLine("1. " + ElevatorStatus.Operational);
        Console.WriteLine("2. " + ElevatorStatus.OutOfOrder);

        int statusChoice;
        bool isValidStatusChoice = int.TryParse(Console.ReadLine(), out statusChoice);

        if (isValidStatusChoice)
        {
            switch (statusChoice)
            {
                case 1:
                    selectedElevator.Status = ElevatorStatus.Operational;
                    Console.WriteLine(Messages.StatusUpdated);
                    break;
                case 2:
                    selectedElevator.Status = ElevatorStatus.OutOfOrder;
                    Console.WriteLine(Messages.StatusUpdated);
                    break;
                default:
                    Console.WriteLine(Messages.Error);
                    break;
            }
        }
        else
        {
            Console.WriteLine(Messages.Error);
        }
    }
    else
    {
        Console.WriteLine(Messages.Error);
    }
}

        #endregion Public Methods

        #region Private Methods
        private static void MoveElevator(Elevator selectedElevator, int elevatorTimeinSec)
        {

            Console.WriteLine(Messages.InitialMovingElevator,selectedElevator.Id, selectedElevator.CurrentFloor,selectedElevator.DestinationFloor);
            int distance = Math.Abs(selectedElevator.CurrentFloor - selectedElevator.DestinationFloor);
            int travelTimeInSeconds = distance * elevatorTimeinSec;
            int initialETA = travelTimeInSeconds;
            selectedElevator.Direction = selectedElevator.CurrentFloor < selectedElevator.DestinationFloor ? Direction.Up : Direction.Down;


            Console.WriteLine(Messages.MovingElevator,selectedElevator.Id,selectedElevator.CurrentFloor,selectedElevator.Direction,selectedElevator.PeopleCount,selectedElevator.Status, initialETA);
            Thread.Sleep(elevatorTimeinSec * 1000);
            for (int i = 0; i < distance; i++)
            {
                selectedElevator.CurrentFloor += selectedElevator.Direction == Direction.Up ? 1 : -1;
                if (selectedElevator.CurrentFloor == selectedElevator.DestinationFloor)
                {
                    selectedElevator.Direction = Direction.Stationary;
                }
                UpdateElevatorStatus(selectedElevator, elevatorTimeinSec);
                if (selectedElevator.CurrentFloor != selectedElevator.DestinationFloor)
                {
                    Sleep(elevatorTimeinSec);
                }
            }

            Console.WriteLine(Messages.ArrivedElevator,selectedElevator.Id,selectedElevator.DestinationFloor);
        }
        private static void UpdateElevatorStatus(Elevator elevator, int elevatorTimeinSec)
        {
            Console.WriteLine(Messages.MovingElevator ,elevator.Id,elevator.CurrentFloor, elevator.Direction,elevator.PeopleCount,elevator.Status,CalculateETA(elevator, elevatorTimeinSec));
        }

        private static Tuple<int, string> AddPassengers(int numOfPeopleGettingOn, Elevator selectedElevator)
        {
            string weightLimitedExceeded;
            int peopleOnElevator = 0;
            weightLimitedExceeded = Elevator.WeightLimitManagement(numOfPeopleGettingOn, selectedElevator);
            Tuple<int, string> returnedValues = Tuple.Create(peopleOnElevator, weightLimitedExceeded);


            if (string.IsNullOrEmpty(weightLimitedExceeded))
            {
                peopleOnElevator = selectedElevator.PeopleCount += numOfPeopleGettingOn;
                return returnedValues;
            }

            return returnedValues;

        }
        private int RemovePassengers(int numOfPeopleGettingOff, Elevator selectedElevator)
        {
            int peopleRemainingOnElevator = selectedElevator.PeopleCount - Math.Min(selectedElevator.PeopleCount, numOfPeopleGettingOff);
            selectedElevator.PeopleCount = peopleRemainingOnElevator;
            return peopleRemainingOnElevator;
        }

        private static int CalculateETA(Elevator elevator, int elevatorTimeinSec)
        {
            int distance = Math.Abs(elevator.CurrentFloor - elevator.DestinationFloor);
            int travelTimeInSeconds = distance * elevatorTimeinSec;
            return travelTimeInSeconds;

        }

        private static void Sleep(int elevatorTimeinSec)
        {
            Thread.Sleep(elevatorTimeinSec * 1000);
        }

        private int GetUserInput(string message)
        {
            _console.WriteLine(message);
            return Convert.ToInt32(_console.ReadLine());
        }

        #endregion
    }


}
