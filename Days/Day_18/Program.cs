class Program
{
    public const int grid = 71;

    static void Main()
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InputAOC18.txt");
        string[] input = File.ReadAllLines(filePath);

        Part1(input);
        Part2(input);
    }

    static void Part1(string[] input)
    {
        var errPosition = input
            .Take(1024)
            .Select(line => line.Split(',').Select(int.Parse).ToArray())
            .Select(coords => (X: coords[0], Y: coords[1]))
            .ToHashSet();

        var directions = GetDirections();  

        var result = FindPath(errPosition, directions);
        Console.WriteLine(result.HasValue ? result.Value.ToString() : "File contains no Path!");
    }

    static void Part2(string[] input)
    {
        var errPosition = new HashSet<(int x, int y)>();
        var directions = GetDirections(); 

        foreach (var line in input)
        {
            var coords = line.Split(',').Select(int.Parse).ToArray();
            var cordPos = (X: coords[0], Y: coords[1]);
            errPosition.Add(cordPos);

            var result = FindPath(errPosition, directions);
            if (!result.HasValue)
            {
                Console.WriteLine($"{cordPos.X},{cordPos.Y}");
                return;
            }
        }

        Console.WriteLine("File contains no Path!");
    }

    static (int dx, int dy)[] GetDirections()
    {
        return new (int dx, int dy)[]
        {
            (0, 1), (1, 0), (0, -1), (-1, 0)
        };
    }

    static int? FindPath(HashSet<(int x, int y)> errPosition, (int dx, int dy)[] directions)
    {
        var que = new Queue<(int x, int y, int steps)>();
        var check = new HashSet<(int x, int y)>();

        que.Enqueue((0, 0, 0));
        check.Add((0, 0));

        while (que.Count > 0)
        {
            var (currentX, currentY, steps) = que.Dequeue();

            if (currentX == grid - 1 && currentY == grid - 1)
            {
                return steps;
            }

            foreach (var (dx, dy) in directions)
            {
                int newX = currentX + dx;
                int newY = currentY + dy;

                if (newX >= 0 && newX < grid &&
                    newY >= 0 && newY < grid &&
                    !errPosition.Contains((newX, newY)) &&
                    !check.Contains((newX, newY)))
                {
                    que.Enqueue((newX, newY, steps + 1));
                    check.Add((newX, newY));
                }
            }
        }
        
        return null;
    }
}
