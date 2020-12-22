using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode
{
    public class SolutionFactory : ISolutionFactory
    {
        private readonly Dictionary<int, IDaySolution> _solutions;
        private readonly int _day;

        public SolutionFactory(IEnumerable<IDaySolution> solutions, Func<int> daySelector)
        {
            _solutions = solutions.ToDictionary(s => s.DayNumber);
            _day = daySelector();
        }

        public IDaySolution Create()
        {
            return _solutions[_day];
        }
    }
}