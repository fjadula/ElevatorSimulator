using ElevatorSimulator.Models.BO;
using ElevatorSimulator.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Service.Implementations
{
  public class ElevatorService
  {
    public static Elevator GetClosestElevator(List<Elevator> elevators, int destinationFloor)
    {
      Elevator closestElevator = null;
      int minDistance = int.MaxValue;

      foreach (var elevator in elevators)
      {
        if (elevator.Status == ElevatorStatus.Operational && elevator.Direction == Direction.Stationary)
        {
          int distance = Math.Abs(elevator.CurrentFloor - destinationFloor);

          if (distance < minDistance)
          {
            minDistance = distance;
            closestElevator = elevator;
          }
        }
      }

      return closestElevator;
    }



    public static void UpdateElevatorStatus(List<Elevator> elevators)
    {
      foreach (var elevator in elevators)
      {
        elevator.UpdateElevatorStatus();
      }
    }
  }
}
