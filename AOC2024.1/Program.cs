using System.Text.RegularExpressions;

var input = (await File.ReadAllLinesAsync("input.txt")).ToList();

var leftNumbers = new List<int>();
var rightNumbers = new List<int>();

var regex = new Regex(@"(?<digit1>\d+)\s+(?<digit2>\d+)", RegexOptions.Compiled);

foreach (var line in input)
{
    var match = regex.Match(line);
    leftNumbers.Add(int.Parse(match.Groups["digit1"].Value));
    rightNumbers.Add(int.Parse(match.Groups["digit2"].Value));
}

Part1(leftNumbers.ToList(), rightNumbers.ToList());
Console.WriteLine();
Part2(leftNumbers.ToList(), rightNumbers.ToList());

void Part1(List<int> leftNumbers, List<int> rightNumbers)
{
    leftNumbers = leftNumbers.OrderBy(x => x).ToList();
    rightNumbers = rightNumbers.OrderBy(x => x).ToList();

    var total = 0;
    for (var i = 0; i < leftNumbers.Count; i++)
    {
        var left = leftNumbers[i];
        var right = rightNumbers[i];
        var biggest = Math.Max(left, right);
        var smallest = Math.Min(left, right);
        var diff = biggest - smallest;
        total += diff;
    }

    Console.WriteLine("Part 1 total: {0}", total);
}

void Part2(List<int> leftNumbers, List<int> rightNumbers)
{
    var total = 0;
    foreach (var leftNumber in leftNumbers)
    {
        var occurrences = rightNumbers.Count(x => x == leftNumber);
        var multiplied = leftNumber * occurrences;
        total += multiplied;
    }

    Console.WriteLine("Part 2 total: {0}", total);
}