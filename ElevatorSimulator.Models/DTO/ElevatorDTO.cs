using ElevatorSimulator.Models.Enums;

namespace ElevatorSimulator.DTOs
{
  public class ElevatorDTO
  {
    public int Id { get; set; }
    public int CurrentFloor { get; set; }
    public int DestinationFloor { get; set; }
    public Direction Direction { get; set; }
    public int PeopleCount { get; set; }
    public TimeSpan ExpectedTimeOfArrival { get; set; }
    public ElevatorStatus Status { get; set; }
    public int WeightLimit { get; set; }
  }
}
