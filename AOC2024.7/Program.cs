internal class Program
{
    private static List<Equation> _equations = null!;
    private static bool _allowConcat;

    private static async Task Main(string[] args)
    {
        var input = (await File.ReadAllLinesAsync("input.txt")).ToList();

        _equations = input.Select(x => new Equation(x)).ToList();

        Part1();
        Console.WriteLine();
        Part2();
    }

    private static void Part1()
    {
        Console.WriteLine("Part 1 total: {0}", _equations.Where(x => x.Resolves).Sum(x => x.TestValue));
    }

    private static void Part2()
    {
        _allowConcat = true;

        _equations.ForEach(x => x.Recalculate());

        Console.WriteLine("Part 2 total: {0}", _equations.Where(x => x.Resolves).Sum(x => x.TestValue));
    }

    private class Equation
    {
        private readonly string _line;

        public Equation(string line)
        {
            _line = line;
            var colonIndex = _line.IndexOf(':');
            TestValue = long.Parse(_line[..colonIndex]);
            var remaining = _line[(colonIndex + 2)..];
            Numbers = remaining.Split(' ').Select(long.Parse).ToList();
            Recalculate();
        }

        public long TestValue { get; }

        public List<long> Numbers { get; }

        public bool Resolves { get; private set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{_line} | {(Resolves ? "Resolves" : "Doesn't resolve")}";
        }

        public void Recalculate()
        {
            Resolves = DoesItResolve();
        }

        private bool DoesItResolve()
        {
            var numOperators = Numbers.Count - 1;
            return DoesItResolve(Numbers, TestValue, new char[numOperators], 0, _allowConcat);
        }

        private static bool DoesItResolve(List<long> testValues, long result, char[] operators, long index, bool allowConcatenate)
        {
            while (true)
            {
                if (index == operators.Length)
                {
                    return DoCalculation(testValues, operators) == result;
                }

                operators[index] = '+';
                if (DoesItResolve(testValues, result, operators, index + 1, allowConcatenate))
                {
                    return true;
                }

                operators[index] = '*';
                if (DoesItResolve(testValues, result, operators, index + 1, allowConcatenate))
                {
                    return true;
                }

                if (!allowConcatenate)
                {
                    return false;
                }

                operators[index] = '|';
                index += 1;
            }
        }

        private static long DoCalculation(List<long> testValues, char[] operators)
        {
            var result = testValues[0];
            for (var i = 0; i < operators.Length; i++)
            {
                result = operators[i] switch
                {
                    '+' => result + testValues[i + 1],
                    '*' => result * testValues[i + 1],
                    '|' => long.Parse(result + testValues[i + 1].ToString()),
                    _ => throw new InvalidOperationException("Impossibruh!"),
                };
            }
            return result;
        }
    }
}