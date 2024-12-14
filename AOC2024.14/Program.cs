using System.Text.RegularExpressions;

internal class Program
{
    private static Regex _regex = new(@"p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)", RegexOptions.Compiled);
    private static int _fieldWidth = 101;
    private static int _fieldHeight = 103;

    private static async Task Main(string[] args)
    {
        var input = await File.ReadAllLinesAsync("input.txt");

        var robots = input
            .Select(x => _regex.Match(x))
            .Select(x => new Robot
            {
                PX = int.Parse(x.Groups["px"].Value),
                PY = int.Parse(x.Groups["py"].Value),
                VX = int.Parse(x.Groups["vx"].Value),
                VY = int.Parse(x.Groups["vy"].Value),
            }).ToList();

        Part1(robots.ToList());
        Console.WriteLine();
        Part2(robots.ToList());
    }


    static void Part1(List<Robot> robots)
    {
        foreach (var robot in robots)
        {
            robot.Move(100);
        }

        var safetyFactor = GetSafetyFactor(robots);

        foreach (var robot in robots)
        {
            robot.Reset();
        }

        Console.WriteLine($"Part 1 safety factor: {safetyFactor}");
    }

    static long GetSafetyFactor(List<Robot> robots)
    {
        var robotsTopLeft = robots.Where(x => x.CurX < _fieldWidth / 2 && x.CurY < _fieldHeight / 2).ToList();
        var robotsTopRight = robots.Where(x => x.CurX > _fieldWidth / 2 && x.CurY < _fieldHeight / 2).ToList();
        var robotsBottomLeft = robots.Where(x => x.CurX < _fieldWidth / 2 && x.CurY > _fieldHeight / 2).ToList();
        var robotsBottomRight = robots.Where(x => x.CurX > _fieldWidth / 2 && x.CurY > _fieldHeight / 2).ToList();
        return robotsTopLeft.Count * robotsTopRight.Count * robotsBottomLeft.Count * robotsBottomRight.Count;
    }

    static void Part2(List<Robot> robots)
    {
        for (var i = 1; ; i++)
        {
            foreach (var robot in robots)
            {
                robot.Move(1);
            }

            if (robots.GroupBy(x => new { x.CurX, x.CurY }).All(x => x.Count() == 1))
            {
                Console.WriteLine($"Part 2 seconds: {i}");
                Console.WriteLine();

                for (var y = 0; y < _fieldHeight; y++)
                {
                    for (var x = 0; x < _fieldWidth; x++)
                    {
                        var count = robots.Count(r => r.CurX == x && r.CurY == y);
                        Console.Write(count > 0 ? count.ToString() : '.');
                    }

                    Console.WriteLine();
                }

                break;
            }
        }
    }

    private class Robot
    {
        public int PX { get; set; }
        public int PY { get; set; }
        public int VX { get; set; }
        public int VY { get; set; }
        public int? CurX { get; set; }
        public int? CurY { get; set; }

        public void Move(int times)
        {
            if (!CurX.HasValue)
            {
                CurX = PX;
                CurY = PY;
            }

            CurX += times * VX;
            CurY += times * VY;

            CurX %= _fieldWidth;
            CurY %= _fieldHeight;

            if (CurX < 0)
            {
                CurX += _fieldWidth;
            }

            if (CurY < 0)
            {
                CurY += _fieldHeight;
            }
        }

        public void Reset()
        {
            CurX = PX;
            CurY = PY;
        }

        public override string ToString()
        {
            return $"{CurX},{CurY}";
        }
    }
}