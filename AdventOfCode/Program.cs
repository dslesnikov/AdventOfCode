using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode
{
    public static class Program
    {
        private const int Day = 22;
        
        public static async Task Main(string[] args)
        {
            var container = new ServiceCollection();
            container
                .AddScoped<ISolutionFactory, SolutionFactory>()
                .AddScoped<IInputReader, InputReader>();
            AddAllSolutions(container);
            container.AddScoped<Func<int>>(_ => () => Day);
            var provider = container.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var scopeProvider = scope.ServiceProvider;
            var factory = scopeProvider.GetRequiredService<ISolutionFactory>();
            var solution = factory.Create();
            await solution.InitializeAsync();
            var partOne = solution.PartOne();
            var partTwo = solution.PartTwo();
            Console.WriteLine($"Part 1: {partOne}");
            Console.WriteLine($"Part 2: {partTwo}");
        }

        private static void AddAllSolutions(IServiceCollection serviceCollection)
        {
            var solutionTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IDaySolution)) && !t.IsInterface);
            var descriptors = solutionTypes
                .Select(type => new ServiceDescriptor(typeof(IDaySolution), type, ServiceLifetime.Scoped));
            foreach (var descriptor in descriptors)
            {
                serviceCollection.Add(descriptor);
            }
        }
    }
}