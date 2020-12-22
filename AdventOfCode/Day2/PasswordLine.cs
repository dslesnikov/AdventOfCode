namespace AdventOfCode.Day2
{
    public record PasswordLine(string Password, char Restriction, (int Lower, int Upper) Range)
    {
        public static PasswordLine Parse(string line)
        {
            var split = line.Split(':');
            var password = split[1];
            var restriction = split[0];
            var restrictionSplit = restriction.Split(' ');
            var character = restrictionSplit[1][0];
            var bounds = restrictionSplit[0].Split('-');
            var lowerBound = int.Parse(bounds[0]);
            var upperBound = int.Parse(bounds[1]);
            return new PasswordLine(password, character, (lowerBound, upperBound));
        }
    }
}