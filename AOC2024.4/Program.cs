using System.Text;
using System.Text.RegularExpressions;

var input = (await File.ReadAllLinesAsync("input.txt")).ToList();
var rowCount = input.Count;
var colCount = input[0].Length;

var regex = new Regex("XMAS", RegexOptions.Compiled);

Part1();
Console.WriteLine();
Part2();

void Part1()
{
    var total = 0;

    var directions = Enum.GetValues<Directions>();
    foreach (var direction in directions)
    {
        var occurrences = 0;
        switch (direction)
        {
            case Directions.LeftToRight:
                foreach (var line in input)
                {
                    occurrences += FindXmasOccurrences(line);
                }
                break;
            case Directions.RightToLeft:
                foreach (var line in input.Select(x => new string(x.Reverse().ToArray())))
                {
                    occurrences += FindXmasOccurrences(line);
                }
                break;
            case Directions.TopToBottom:
                for (var i = 0; i < input[0].Length; i++)
                {
                    var line = new string(input.Select(x => x[i]).ToArray());
                    occurrences += FindXmasOccurrences(line);
                }
                break;
            case Directions.BottomToTop:
                for (var i = 0; i < input[0].Length; i++)
                {
                    var line = new string(input.Select(x => x[i]).Reverse().ToArray());
                    occurrences += FindXmasOccurrences(line);
                }
                break;
            case Directions.TopLeftToBottomRight:
                occurrences = FindTopLeftToBottomRightOccurrences(input);
                break;
            case Directions.TopRightToBottomLeft:
                var reversedInput = input.Select(x => new string(x.Reverse().ToArray())).ToList();
                occurrences = FindTopLeftToBottomRightOccurrences(reversedInput);
                break;
            case Directions.BottomLeftToTopRight:
                occurrences = FindBottomLeftToTopRightOccurrences(input);
                break;
            case Directions.BottomRightToTopLeft:
                reversedInput = input.Select(x => new string(x.Reverse().ToArray())).ToList();
                occurrences = FindBottomLeftToTopRightOccurrences(reversedInput);
                break;
        }

        Console.WriteLine("Found {0} occurrences when looking from {1}.", occurrences, direction);

        total += occurrences;
    }

    Console.WriteLine("Part 1 total: {0}", total);
}

void Part2()
{
    var total = 0;

    for (var i = 0; i < colCount; i++)
    {
        for (var j = 0; j < rowCount; j++)
        {
            var character = input[j][i];
            if (character != 'A')
            {
                continue;
            }

            try
            {
                var characterTopLeft = input[j - 1][i - 1];
                var characterTopRight = input[j - 1][i + 1];
                var characterBottomLeft = input[j + 1][i - 1];
                var characterBottomRight = input[j + 1][i + 1];

                if ((characterTopLeft == 'M' && characterBottomRight == 'S') || (characterTopLeft == 'S' && characterBottomRight == 'M'))
                {
                    if ((characterBottomLeft == 'M' && characterTopRight == 'S') || (characterBottomLeft == 'S' && characterTopRight == 'M'))
                    {
                        total++;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // Don't care 😇
            }
            catch (ArgumentOutOfRangeException)
            {
                // Don't care 😇
            }
        }
    }

    Console.WriteLine("Part 2 total: {0}", total);
}

int FindXmasOccurrences(string input)
{
    return regex.Matches(input).Count;
}

int FindTopLeftToBottomRightOccurrences(List<string> list)
{
    var occurrences = 0;
    var diagonals = new List<string>();

    // Get diagonals starting from the first row
    for (var col = 0; col < colCount; col++)
    {
        var diagonal = new StringBuilder();
        for (int row = 0, j = col; row < rowCount && j < colCount; row++, j++)
        {
            diagonal.Append(list[row][j]);
        }
        diagonals.Add(diagonal.ToString());
    }

    // Get diagonals starting from the first column (excluding the first element)
    for (var row = 1; row < rowCount; row++)
    {
        var diagonal = new StringBuilder();
        for (int col = 0, j = row; col < colCount && j < rowCount; col++, j++)
        {
            diagonal.Append(list[j][col]);
        }
        diagonals.Add(diagonal.ToString());
    }

    foreach (var line in diagonals)
    {
        occurrences += FindXmasOccurrences(line);
    }

    return occurrences;
}

int FindBottomLeftToTopRightOccurrences(List<string> list)
{
    var occurrences = 0;
    var diagonals = new List<string>();

    // Get diagonals starting from the last row
    for (var col = 0; col < colCount; col++)
    {
        var diagonal = new StringBuilder();
        for (int row = rowCount - 1, j = col; row >= 0 && j < colCount; row--, j++)
        {
            diagonal.Append(list[row][j]);
        }
        diagonals.Add(diagonal.ToString());
    }

    // Get diagonals starting from the first column (excluding the last element)
    for (var row = rowCount - 2; row >= 0; row--)
    {
        var diagonal = new StringBuilder();
        for (int col = 0, j = row; col < colCount && j >= 0; col++, j--)
        {
            diagonal.Append(list[j][col]);
        }
        diagonals.Add(diagonal.ToString());
    }

    foreach (var line in diagonals)
    {
        occurrences += FindXmasOccurrences(line);
    }

    return occurrences;
}

enum Directions
{
    LeftToRight,
    RightToLeft,
    TopToBottom,
    BottomToTop,
    TopLeftToBottomRight,
    TopRightToBottomLeft,
    BottomLeftToTopRight,
    BottomRightToTopLeft,
}