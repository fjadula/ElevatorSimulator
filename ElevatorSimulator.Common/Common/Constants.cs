using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ElevatorSimulator.Common
{
  public class Constants
  {
 
    public static class Formatting
    {
      public const string BetweenFormat = "{0} (between {1} and {2}) ";
      public const string ElevatorMovingFormat = "Elevator moving with {0} people. Current floor: {1}";
      public const string GreaterThanEqualsFormat = "{0} (>= {1}) ";
      public const string OptionsFormat = "{0}) {1} - {2}";
    }
    public static class Input
    {
      public const string DestinationFloor = "Enter the destination floor number:";
      public const string SelectElevatorNumber= "Enter the elevator number:";
      public const string PeopleGettingOff = "How many people are getting off?";
      public const string PeopleGettingOn = "How many people are getting on?";
      public const string UpTo = "(Up to {0})?";
      public const string RequestFloorNumber = "Enter the floor number(Ground is 0):";
      public const string PeopleAddingtoFloor = "How many people do you want to add to the floor?";
      public const string PeopleRemoveFromFloor ="How many people do you want to remove from the floor?";
    }

    public static class Messages
    {
      public const string Error = "Invalid choice. Please try again.";
      public const string InvalidFloor=  "Invalid floor number. Please try again.";
      public const string MainMenu = "Elevator Simulator Menu";
      public const string ElevatorStatusInfo = "Elevator Status:";
      public const string FloorStatusInfo = "Floor Status:";
      public const string Exit = "Exiting program";
      public const string InputPrompt = "Selection: ";
      public const string MainMenuChoice = "Enter your choice (1-6):";
      public const string PeopletoGetOff = "There are no people in the elevator to get off.";
      public const string PeopleToGetOn = "There are no people on the floor to get on.";
      public const string MoreGetOnThanWaiting = "Those are more people than are waiting, so only {0} will get on.";
      public const string HowManyGotOn = "Elevator {0} accommodated {1} people from the waiting group on floor {2}.";
      public const string ElevatorCalled = "Elevator {0} called successfully!";
      public const string NoOperationalElevators = "No operational elevators available. Please try again later.";
      public const string StatusUpdated = "Elevator status updated successfully!";
      public const string StationaryElevator = "Elevator {0} - Current Floor: {1} - Status: {2} , People Count: {3}, Direction: {4}";
      public const string InitialMovingElevator = "Elevator {0} is moving from floor {1} to floor {2}...";
      public const string ArrivedElevator = "Elevator {0} has reached the destination floor: {1}.";
      public const string MovingElevator = "Elevator {0} - Current Floor: {1}, Direction: {2}, People Count: {3}, Status: {4}, ETA: {5} seconds";
      public const string PeopleOffPeopleRemaining = "Elevator {0}, {1} people got off. {2} remaining.";
      public const string SelectElevatorStatus = "Enter the new status of the elevator:";
      public const string WeightExceed = "Weight limit exceeded. Cannot accommodate that many people.";
      public const string ReturnNoPeopleOntheFloor = "There are now {0} people waiting on floor {1}";
    }
  }

}

