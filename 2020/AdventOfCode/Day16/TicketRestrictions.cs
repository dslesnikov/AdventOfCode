using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Day16
{
    public class TicketRestrictions
    {
        private readonly Dictionary<string, Range[]> _fields = new();

        public void AddRestriction(string field, Range[] ranges)
        {
            _fields[field] = ranges;
        }

        public string[] GetFieldNames()
        {
            return _fields.Keys.ToArray();
        }

        public int ValidateTicket(int[] values)
        {
            return values
                .FirstOrDefault(value => _fields.Values
                    .All(ranges => ranges
                        .All(range => range.Start.Value > value || range.End.Value < value)));
        }

        public string[] GuessField(int value)
        {
            return _fields.Keys
                .Where(key => _fields[key].Any(range => range.Start.Value <= value && range.End.Value >= value))
                .ToArray();
        }
    }
}