using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "inputAOC12.txt");
        string[] lines = File.ReadAllLines(filePath);
        char[][] grid = new char[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            grid[i] = lines[i].ToCharArray();
        }

        
        int part1Result = Part1(grid);
        Console.WriteLine(part1Result);

       
        int part2Result = Part2(grid);
        Console.WriteLine(part2Result);
    }
    
    static int Part1(char[][] grid)
    {
        int cost = 0;
        bool[][] visited = new bool[grid.Length][];
        for (int i = 0; i < grid.Length; i++)
        {
            visited[i] = new bool[grid[0].Length];
        }

        for (int i = 0; i < grid.Length; i++)
        {
            for (int j = 0; j < grid[0].Length; j++)
            {
                if (visited[i][j])
                {
                    continue;
                }

               
                int area = 0;
                int perimeter = 0; 
                Queue<int[]> q = new Queue<int[]>();
                q.Enqueue(new int[] { i, j });

                while (q.Count > 0)
                {
                    int[] cell = q.Dequeue();
                    int r = cell[0];
                    int c = cell[1];

                    if (r < 0 || r >= grid.Length || c < 0 || c >= grid[0].Length ||
                        (visited[r][c] && grid[r][c] != grid[i][j]) || grid[r][c] != grid[i][j])
                    {
                        perimeter++;
                        continue;
                    }

                    if (visited[r][c])
                    {
                        continue;
                    }

                    visited[r][c] = true;
                    area++;

                    q.Enqueue(new int[] { r + 1, c });
                    q.Enqueue(new int[] { r - 1, c });
                    q.Enqueue(new int[] { r, c + 1 });
                    q.Enqueue(new int[] { r, c - 1 });
                }

                cost += area * perimeter;
            }
        }
        return cost;
    }

    static int Part2(char[][] grid)
    {
        
        char[][] newGrid = new char[grid.Length + 2][];
        for (int i = 0; i < newGrid.Length; i++)
        {
            newGrid[i] = new char[grid[0].Length + 2];
        }

        for (int i = 0; i < newGrid.Length; i++)
        {
            for (int j = 0; j < newGrid[0].Length; j++)
            {
                if (i == 0 || i == newGrid.Length - 1 || j == 0 || j == newGrid[0].Length - 1)
                {
                    newGrid[i][j] = '.';
                }
                else
                {
                    newGrid[i][j] = grid[i - 1][j - 1];
                }
            }
        }

        int cost = 0;
        bool[][] filled = new bool[newGrid.Length][];
        for (int i = 0; i < newGrid.Length; i++)
        {
            filled[i] = new bool[newGrid[0].Length];
        }

        for (int i = 1; i < newGrid.Length - 1; i++)
        {
            for (int j = 1; j < newGrid[0].Length - 1; j++)
            {
                if (filled[i][j])
                {
                    continue;
                }

                int area = 0;
                HashSet<int>[][] normals = new HashSet<int>[newGrid.Length][];
                for (int a = 0; a < newGrid.Length; a++)
                {
                    normals[a] = new HashSet<int>[newGrid[0].Length];
                    for (int b = 0; b < newGrid[0].Length; b++)
                    {
                        normals[a][b] = new HashSet<int>();
                    }
                }

                bool[][] visited = new bool[newGrid.Length][];
                for (int i2 = 0; i2 < newGrid.Length; i2++)
                {
                    visited[i2] = new bool[newGrid[0].Length];
                }

                Queue<int[]> q = new Queue<int[]>();
                q.Enqueue(new int[] { i, j });

                while (q.Count > 0)
                {
                    int[] cell = q.Dequeue();
                    int r = cell[0];
                    int c = cell[1];

                    if (visited[r][c])
                    {
                        continue;
                    }

                    visited[r][c] = true;

                    if (newGrid[r][c] == newGrid[i][j])
                    {
                        filled[r][c] = true;
                    }

                    area++;

                    if (r > 0)
                    {
                        if (newGrid[r - 1][c] != newGrid[i][j])
                        {
                            normals[r - 1][c].Add(1); // up
                        }
                        else
                        {
                            q.Enqueue(new int[] { r - 1, c });
                        }
                    }

                    if (r < newGrid.Length - 1)
                    {
                        if (newGrid[r + 1][c] != newGrid[i][j])
                        {
                            normals[r + 1][c].Add(3); // down
                        }
                        else
                        {
                            q.Enqueue(new int[] { r + 1, c });
                        }
                    }

                    if (c < newGrid[0].Length - 1)
                    {
                        if (newGrid[r][c + 1] != newGrid[i][j])
                        {
                            normals[r][c + 1].Add(2); // right
                        }
                        else
                        {
                            q.Enqueue(new int[] { r, c + 1 });
                        }
                    }

                    if (c > 0)
                    {
                        if (newGrid[r][c - 1] != newGrid[i][j])
                        {
                            normals[r][c - 1].Add(4); // left
                        }
                        else
                        {
                            q.Enqueue(new int[] { r, c - 1 });
                        }
                    }
                }

                int numSides = 0;
                for (int a = 0; a < newGrid.Length; a++)
                {
                    for (int b = 0; b < newGrid[0].Length; b++)
                    {
                        if (normals[a][b].Count == 0)
                        {
                            continue;
                        }

                        HashSet<int> org = new HashSet<int>(normals[a][b]);
                        foreach (int g in org)
                        {
                            q = new Queue<int[]>();
                            q.Enqueue(new int[] { a, b });

                            while (q.Count > 0)
                            {
                                int[] cell = q.Dequeue();
                                int r = cell[0];
                                int c = cell[1];

                                if (!normals[r][c].Contains(g))
                                {
                                    continue;
                                }

                                normals[r][c].Remove(g);

                                if (r > 0 && normals[r - 1][c].Contains(g))
                                {
                                    q.Enqueue(new int[] { r - 1, c });
                                }

                                if (r < newGrid.Length - 1 && normals[r + 1][c].Contains(g))
                                {
                                    q.Enqueue(new int[] { r + 1, c });
                                }

                                if (c < newGrid[0].Length - 1 && normals[r][c + 1].Contains(g))
                                {
                                    q.Enqueue(new int[] { r, c + 1 });
                                }

                                if (c > 0 && normals[r][c - 1].Contains(g))
                                {
                                    q.Enqueue(new int[] { r, c - 1 });
                                }
                            }

                            numSides++;
                        }
                    }
                }

                cost += area * numSides;
            }
        }

        return cost;
    }
}
