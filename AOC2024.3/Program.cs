using System.Text.RegularExpressions;

var input = (await File.ReadAllLinesAsync("input.txt")).ToList();

var regex = new Regex(@"mul\((?<digit1>\d{1,3}),(?<digit2>\d{1,3})\)", RegexOptions.Compiled);

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var total = 0;
    foreach (var line in input)
    {
        total += GetMuls(line);
    }

    Console.WriteLine("Part 1 total: {0}", total);
}

void Part2()
{
    var total = 0;

    var joinedInput = $"do(){string.Join(string.Empty, input)}don't()";
    var doDontRegex = new Regex(@"(?:do\(\)(.+?)don\'t\(\))+", RegexOptions.Compiled);

    var matches = doDontRegex.Matches(joinedInput);
    foreach (Match match in matches)
    {
        total += GetMuls(match.Value);
    }

    Console.WriteLine("Part 2 total: {0}", total);
}

int GetMuls(string input)
{
    var muls = 0;
    var matches = regex.Matches(input);
    foreach (Match match in matches)
    {
        var digit1 = int.Parse(match.Groups["digit1"].Value);
        var digit2 = int.Parse(match.Groups["digit2"].Value);
        var mul = digit1 * digit2;
        muls += mul;
    }

    return muls;
}