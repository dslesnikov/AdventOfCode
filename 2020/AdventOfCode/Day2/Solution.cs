using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Day2
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private PasswordLine[] _passwords;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 2;
        
        public async Task InitializeAsync()
        {
            _passwords = await _reader.ReadAsync(PasswordLine.Parse);
        }

        public string PartOne()
        {
            var result = _passwords.Count(line =>
            {
                var characterCount = line.Password.Count(c => c == line.Restriction);
                return characterCount >= line.Range.Lower && characterCount <= line.Range.Upper;
            });
            return result.ToString();
        }

        public string PartTwo()
        {
            var result = _passwords.Count(line => (line.Password[line.Range.Lower] == line.Restriction) ^
                                                  (line.Password[line.Range.Upper] == line.Restriction));
            return result.ToString();
        }
    }
}