using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Service.Implementations
{

    public interface IElevatorService
    {
      void UpdateElevatorStatus(List<Elevator> elevators);
      Elevator GetClosestElevator(List<Elevator> elevators, int destinationFloor);
    }

    public class ElevatorService : IElevatorService
    {
      public void UpdateElevatorStatus(List<Elevator> elevators)
      {
        Console.WriteLine("Elevator Status:");
        foreach (var elevator in elevators)
        {
          Console.WriteLine($"Elevator {elevator.Id} - Current Floor: {elevator.CurrentFloor} - Status: {elevator.Status} , People Count: {elevator.PeopleCount}");
        }
      }

      public Elevator GetClosestElevator(List<Elevator> elevators, int destinationFloor)
      {
        Elevator closestElevator = null;
        int minDistance = int.MaxValue;

        foreach (var elevator in elevators)
        {
          if (elevator.Status == ElevatorStatus.Operational)
          {
            int distance = Math.Abs(elevator.CurrentFloor - destinationFloor);
            if (distance < minDistance)
            {
              closestElevator = elevator;
              minDistance = distance;
            }
          }
        }

        return closestElevator;
      }


    //  public  void UpdateElevatorStatus(List<Elevator> elevators)
    //{
    //  foreach (var elevator in elevators)
    //  {
    //    elevator.UpdateElevatorStatus();
    //  }
    //}
  }
}
