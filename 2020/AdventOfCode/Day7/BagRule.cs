using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day7
{
    public record BagRule(string Container, List<(int Count, string Name)> Contents)
    {
        private static readonly Regex ContainerRegex =
            new Regex(@"^(?<container>\w+ \w+) bags contain( (?<quantity>\d+) (?<containee>\w+ \w+) bags?[,\.])+$",
                RegexOptions.Compiled);

        private static readonly Regex EmptyRegex =
            new Regex(@"^(?<container>\w+ \w+) bags contain no other bags\.$", RegexOptions.Compiled);
        
        public static BagRule Parse(string line)
        {
            var match = ContainerRegex.Match(line);
            if (!match.Success)
            {
                var emptyMatch = EmptyRegex.Match(line);
                var emptyContainer = emptyMatch.Groups["container"].Value;
                return new BagRule(emptyContainer, new List<(int Count, string Name)>());
            }
            var container = match.Groups["container"].Value;
            var containees = match.Groups["containee"].Captures.Select(x => x.Value).ToArray();
            var quantities = match.Groups["quantity"].Captures.Select(x => x.Value).ToArray();
            var contents = containees.Zip(quantities, (color, number) => (int.Parse(number), color)).ToList();
            return new BagRule(container, contents);
        }
    }
}