using System.Numerics;

class Program
{
    static void Main(string[] args)
    {
        
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "AOCD19.txt");

       
        var lines = File.ReadAllLines(filePath);

        
        var input = new HashSet<string>(lines[0].Split(", "));

        
        var struc = new List<string>();
        for (int i = 2; i < lines.Length; i++) 
        {
            struc.Add(lines[i]);
        }

        
        int part1Result = Part1(input, struc);
        Console.WriteLine(part1Result);

        
        BigInteger part2Result = Part2(input, struc);
        Console.WriteLine(part2Result);
    }

    static int Part1(HashSet<string> input, List<string> struc)
    {
        var mem = new Dictionary<string, bool>();
        int Count = 0;

        foreach (var part in struc)
        {
            if (Check(part, input, mem))
            {
                Count++;
            }
        }

        return Count;
    }

    static BigInteger Part2(HashSet<string> input, List<string> struc)
    {
        var mem = new Dictionary<string, BigInteger>();
        BigInteger ways = 0;

        foreach (var part in struc)
        {
            ways += Count(part, input, mem);
        }

        return ways;
    }

    static bool Check(string struc, HashSet<string> input, Dictionary<string, bool> mem)
    {

        if (struc == "") return true; 
        if (mem.ContainsKey(struc)) return mem[struc];



        foreach (var pattern in input)
        {
            if (struc.StartsWith(pattern))
            {
                var remainingstruc = struc.Substring(pattern.Length);
                if (Check(remainingstruc, input, mem))
                {
                    mem[struc] = true;
                    return true;
                }
            }
        }

        mem[struc] = false;
        return false;
    }

    static BigInteger Count(string struc, HashSet<string> input, Dictionary<string, BigInteger> mem)
    {


        if (struc == "") return 1;
        if (mem.ContainsKey(struc)) return mem[struc];

        BigInteger ways = 0;

        foreach (var pattern in input)
        {
            if (struc.StartsWith(pattern))
            {
                var remainingstruc = struc.Substring(pattern.Length);
                ways += Count(remainingstruc, input, mem);
            }
        }

        mem[struc] = ways;
        return ways;
    }
}
