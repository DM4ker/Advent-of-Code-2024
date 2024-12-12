using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    public static void Main()
    {
        Txt_Input txt_Input = new Txt_Input();
        Order_Analyzer analyzer = new Order_Analyzer(txt_Input.Rules, txt_Input.Updates);
        analyzer.Ayz_Update();
        Console.WriteLine(analyzer.Mid_Sum);
        analyzer.Rec_Inc_Update();
        Console.WriteLine(analyzer.Mid_Sum_Incorrect);
    }
}

public class Txt_Input
{
    private List<(int, int)> rules;
    private List<List<int>> updates;

    public List<(int, int)> Rules
    {
        get
        {
            if (rules == null)
            {
                Parse_Input();
            }
            return rules;
        }
    }

    public List<List<int>> Updates
    {
        get
        {
            if (updates == null)
            {
                Parse_Input();
            }
            return updates;
        }
    }

    private void Parse_Input()
    {
        string desktop_Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string file_Path = Path.Combine(desktop_Path, "InputAOC5.txt");
        string[] lines = File.ReadAllLines(file_Path);

        rules = new List<(int, int)>();
        updates = new List<List<int>>();
        bool is_Rules_Section = true;

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                is_Rules_Section = false;
                continue;
            }

            if (is_Rules_Section)
            {
                string[] parts = line.Split('|');
                rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
            }
            else
            {
                List<int> update = line.Split(',').Select(int.Parse).ToList();
                updates.Add(update);
            }
        }
    }
}

public class Order_Analyzer
{
    private List<(int, int)> Rules;
    private List<List<int>> Updates;
    private int Mid_Sum_Value;
    private int Mid_Sum_Incorrect_Value;

    public int Mid_Sum
    {
        get { return Mid_Sum_Value; }
        set { Mid_Sum_Value = value; }
    }

    public int Mid_Sum_Incorrect
    {
        get { return Mid_Sum_Incorrect_Value; }
        set { Mid_Sum_Incorrect_Value = value; }
    }

    private List<List<int>> Inc_Update;

    public Order_Analyzer(List<(int, int)> rules, List<List<int>> updates)
    {
        Rules = rules;
        Updates = updates;
        Inc_Update = new List<List<int>>();
    }

    public void Ayz_Update()
    {
        foreach (var update in Updates)
        {
            if (Correct_Ord(update))
            {
                Mid_Sum += Mid_Page(update);
            }
            else
            {
                Inc_Update.Add(update);
            }
        }
    }

    public void Rec_Inc_Update()
    {
        foreach (var update in Inc_Update)
        {
            List<int> reordered_Update = Rec_Update(update);
            Mid_Sum_Incorrect += Mid_Page(reordered_Update);
        }
    }

    private bool Correct_Ord(List<int> update)
    {
        var positions = update.Select((page, index) => (page, index)).ToDictionary(x => x.page, x => x.index);

        foreach (var rule in Rules)
        {
            if (positions.ContainsKey(rule.Item1) && positions.ContainsKey(rule.Item2))
            {
                if (positions[rule.Item1] > positions[rule.Item2])
                {
                    return false;
                }
            }
        }
        return true;
    }

    private List<int> Rec_Update(List<int> update)
    {
        var graph = new Dictionary<int, List<int>>();
        foreach (var rule in Rules)
        {
            if (!graph.ContainsKey(rule.Item1)) graph[rule.Item1] = new List<int>();
            graph[rule.Item1].Add(rule.Item2);
        }

        var sorted = TLog_Sort(update, graph);
        return sorted;
    }

    private List<int> TLog_Sort(List<int> update, Dictionary<int, List<int>> graph)
    {
        var in_Degree = update.ToDictionary(x => x, x => 0);
        foreach (var key in graph.Keys)
        {
            if (!update.Contains(key)) continue;
            foreach (var neighbor in graph[key])
            {
                if (!update.Contains(neighbor)) continue;
                in_Degree[neighbor]++;
            }
        }

        var queue = new Queue<int>(update.Where(x => in_Degree[x] == 0));
        var sorted = new List<int>();

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            sorted.Add(current);

            if (!graph.ContainsKey(current)) continue;
            foreach (var neighbor in graph[current])
            {
                if (!update.Contains(neighbor)) continue;
                in_Degree[neighbor]--;
                if (in_Degree[neighbor] == 0)
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return sorted;
    }

    private int Mid_Page(List<int> update)
    {
        int middle_Index = update.Count / 2;
        return update[middle_Index];
    }
}
