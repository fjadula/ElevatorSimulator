using ElevatorSimulator.Service;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulator
{
    public static class ServiceExtensions
  {
    public static IServiceCollection AddElevatorService(this IServiceCollection services)
    {
      services.AddScoped<ElevatorService>();
      services.AddScoped<FloorService>();
      return services;
    }
  }
}