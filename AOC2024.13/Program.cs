using System.Text.RegularExpressions;

internal class Program
{
    private static Regex _buttonRegex = new(@"Button .: X\+(?<X>\d+), Y\+(?<Y>\d+)", RegexOptions.Compiled);
    private static Regex _prizeRegex = new(@"Prize: X=(?<X>\d+), Y=(?<Y>\d+)", RegexOptions.Compiled);

    private static async Task Main(string[] args)
    {
        var input = await File.ReadAllLinesAsync("input.txt");

        var machines = input
            .Chunk(4)
            .Select(x =>
                x.Take(3).ToList()
            )
            .Select(x => new Machine(x))
            .ToList();

        Part1(machines);
        Console.WriteLine();
        Part2(machines);
    }


    static void Part1(List<Machine> machines)
    {
        var total = 0L;
        foreach (var machine in machines)
        {
            total += machine.CalculateCosts();
        }

        Console.WriteLine($"Part 1 price: {total}");
    }

    static void Part2(List<Machine> machines)
    {
        var total = 0L;
        foreach (var machine in machines)
        {
            total += machine.CalculateCosts(10000000000000);
        }

        Console.WriteLine($"Part 2 price: {total}");
    }

    class Machine
    {
        public XY ButtonA { get; set; }
        public XY ButtonB { get; set; }
        public XY Prize { get; set; }
        public long ButtonACosts => 3;
        public long ButtonBCosts => 1;

        public Machine(IList<string> input)
        {
            ButtonA = ParseButton(input[0]);
            ButtonB = ParseButton(input[1]);
            Prize = ParsePrize(input[2]);
        }

        public long CalculateCosts(long? addToXY = null)
        {
            if (addToXY.HasValue)
            {
                Prize.X += addToXY.Value;
                Prize.Y += addToXY.Value;
            }

            var a = (decimal)(Prize.X * ButtonB.Y - Prize.Y * ButtonB.X) / (ButtonA.X * ButtonB.Y - ButtonA.Y * ButtonB.X);
            var b = (decimal)(ButtonA.X * Prize.Y - ButtonA.Y * Prize.X) / (ButtonA.X * ButtonB.Y - ButtonA.Y * ButtonB.X);
            if (a.Scale == 0 && b.Scale == 0)
            {
                Console.WriteLine(this);
                return (long)a * ButtonACosts + (long)b * ButtonBCosts;
            }

            return 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Prize.X}, {Prize.Y}";
        }

        private static XY ParseButton(string buttonText)
        {
            var match = _buttonRegex.Match(buttonText);
            return new XY
            {
                X = long.Parse(match.Groups["X"].Value),
                Y = long.Parse(match.Groups["Y"].Value),
            };
        }

        private static XY ParsePrize(string prizeText)
        {
            var match = _prizeRegex.Match(prizeText);
            return new XY
            {
                X = long.Parse(match.Groups["X"].Value),
                Y = long.Parse(match.Groups["Y"].Value),
            };
        }
    }

    class XY
    {
        public long X { get; set; }
        public long Y { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}