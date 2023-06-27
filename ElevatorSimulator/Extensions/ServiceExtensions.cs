using ElevatorSimulator.Service.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulator
{
  public static class ServiceExtensions
  {
    public static IServiceCollection AddElevatorService(this IServiceCollection services)
    {
      services.AddScoped<ElevatorService, ElevatorService>();
      return services;
    }
  }
}