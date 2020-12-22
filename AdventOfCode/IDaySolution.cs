using System.Threading.Tasks;

namespace AdventOfCode
{
    public interface IDaySolution
    {
        int DayNumber { get; }
        
        Task InitializeAsync();
        
        string PartOne();

        string PartTwo();
    }
}