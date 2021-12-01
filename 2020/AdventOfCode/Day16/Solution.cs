using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Day16
{
    public class Solution : IDaySolution
    {
        private readonly IInputReader _reader;
        private TicketRestrictions _restrictions;
        private int[] _myTicket;
        private int[][] _nearbyTickets;

        public Solution(IInputReader reader)
        {
            _reader = reader;
        }

        public int DayNumber => 16;
        
        public async Task InitializeAsync()
        {
            var lines = await _reader.ReadLinesAsync();
            _restrictions = new TicketRestrictions();
            var index = 0;
            while (!string.IsNullOrWhiteSpace(lines[index]))
            {
                var restrictionMatch = Regex.Match(lines[index], @"(?<field>[\w ]+):( (?<range>\d+-\d+)( or)?)+");
                var field = restrictionMatch.Groups["field"].Value;
                var ranges = restrictionMatch.Groups["range"].Captures
                    .Select(capture => capture.Value.Split('-'))
                    .Select(range => new Range(int.Parse(range[0]), int.Parse(range[1])))
                    .ToArray();
                _restrictions.AddRestriction(field, ranges);
                index++;
            }
            index += 2;
            _myTicket = lines[index].Split(',').Select(int.Parse).ToArray();
            index += 3;
            _nearbyTickets = lines[index..].Select(line => line.Split(',').Select(int.Parse).ToArray()).ToArray();
        }

        public string PartOne()
        {
            var result = _nearbyTickets.Sum(ticket => _restrictions.ValidateTicket(ticket));
            return result.ToString();
        }

        public string PartTwo()
        {
            var allFields = _restrictions.GetFieldNames();
            var fields = _myTicket.Select((item, index) =>
                {
                    var guessedFields = _nearbyTickets.Select(t => _restrictions.GuessField(t[index]))
                        .Where(fieldGuesses => fieldGuesses.Length > 0)
                        .Aggregate(new HashSet<string>(allFields), (acc, i) =>
                        {
                            acc.IntersectWith(i);
                            return acc;
                        });
                    return (Number: item, PossibleFields: guessedFields);
                })
                .OrderBy(x => x.PossibleFields.Count)
                .ToArray();
            for (var i = 0; i < fields.Length; i++)
            {
                for (var j = i + 1; j < fields.Length; j++)
                {
                    fields[j].PossibleFields.ExceptWith(fields[i].PossibleFields);
                }
            }
            var ticket = fields.Select(f => (f.Number, FieldName: f.PossibleFields.Single())).ToArray();
            var departureFields = ticket.Where(t => t.FieldName.StartsWith("departure"));
            var result = departureFields.Aggregate(1ul, (acc, item) => acc * (ulong)item.Number);
            return result.ToString();
        }
    }
}