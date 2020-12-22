namespace AdventOfCode.Day20
{
    public static class MatrixOperations
    {
        public static void RotateClockwise<T>(T[][] input, T[][] output)
        {
            var height = input.Length;
            var width = input[0].Length;
            for (var i = 0; i < height / 2; i++)
            {
                for (var j = i; j < width - i - 1; j++)
                {
                    var tmp = input[i][j];
                    output[i][j] = input[height - 1 - j][i];
                    output[height - 1 - j][i] = input[height - 1 - i][width - 1 - j];
                    output[height - 1 - i][width - 1 - j] = input[j][width - 1 - i];
                    output[j][width - 1 - i] = tmp;
                }
            }
        }
        
        public static void RotateCounterClockwise<T>(T[][] input, T[][] output)
        {
            var height = input.Length;
            var width = input[0].Length;
            for (var i = 0; i < height / 2; i++)
            {
                for (var j = i; j < width - i - 1; j++)
                {
                    var tmp = input[i][j];
                    output[i][j] = input[j][width - 1 - i];
                    output[j][width - 1 - i] = input[height - 1 - i][width - 1 - j];
                    output[height - 1 - i][width - 1 - j] = input[height - 1 - j][i];
                    output[height - 1 - j][i] = tmp;
                }
            }
        }
        
        public static void FlipVertically<T>(T[][] input, T[][] output)
        {
            var height = input.Length;
            var width = input[0].Length;
            for (var i = 0; i < height / 2; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var tmp = input[height - 1 - i][j];
                    output[height - 1 - i][j] = input[i][j];
                    output[i][j] = tmp;
                }
            }
        }
        
        public static void FlipHorizontally<T>(T[][] input, T[][] output)
        {
            var height = input.Length;
            var width = input[0].Length;
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width / 2; j++)
                {
                    var tmp = input[i][width - 1 - j];
                    output[i][width - 1 - j] = input[i][j];
                    output[i][j] = tmp;
                }
            }
        }
    }
}