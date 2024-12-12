using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "inputAOC2.txt");
        string[] lines = File.ReadAllLines(filePath);

        int part1Result = Part1(lines);
        Console.WriteLine(part1Result);

        int part2Result = Part2(lines);
        Console.WriteLine(part2Result);
    }

    static int Part1(string[] lines)
    {
        int Count = 0;

        foreach (string line in lines)
        {
            var levels = Array.ConvertAll(line.Split(' '), int.Parse);
            if (Safe_Report(levels))
            {
                Count++;
            }
        }

        return Count;
    }

    static int Part2(string[] lines)
    {
        int Count = 0;

        foreach (string line in lines)
        {
            var levels = Array.ConvertAll(line.Split(' '), int.Parse);
            if (Safe_Report(levels))
            {
                Count++;
            }
            else
            {
                
                if (P2_check_if_mby_safe(levels))
                {
                    Count++;
                }
            }
        }

        return Count;
    }

    static bool Safe_Report(int[] levels)
    {
        bool Inc = true;
        bool Dec = true;

        for (int i = 1; i < levels.Length; i++)
        {
            int diff = Math.Abs(levels[i] - levels[i - 1]);

            if (diff < 1 || diff > 3)
                return false;

            if (levels[i] < levels[i - 1]) Inc = false;
            if (levels[i] > levels[i - 1]) Dec = false;
        }

        
        return Inc || Dec;
    }

    static bool P2_check_if_mby_safe(int[] levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            
            var mod_lvl = new int[levels.Length - 1];
            Array.Copy(levels, 0, mod_lvl, 0, i);
            Array.Copy(levels, i + 1, mod_lvl, i, levels.Length - i - 1);

            if (Safe_Report(mod_lvl))
            {
                return true; 
            }
        }

        return false; 
    }
}
