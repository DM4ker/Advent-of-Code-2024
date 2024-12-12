using System;
using System.Collections.Generic;
using System.Numerics;

class Program
{
    static void Main()
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "InputAOC11.txt");
        

        
        string[] inputValues = File.ReadAllText(filePath).Split(' ');

        
        Dictionary<BigInteger, BigInteger> input = new Dictionary<BigInteger, BigInteger>();

       
        foreach (var value in inputValues)
        {
            
            if (BigInteger.TryParse(value, out BigInteger parsedValue))
            {
                if (!input.ContainsKey(parsedValue))
                {
                    input[parsedValue] = 0; 
                }
                input[parsedValue] += 1;
            }
        }


        BigInteger blinksPart1 = 25;
        BigInteger blinksPart2 = 75;

        
        BigInteger totalStonesPart1 = ProcessBlinks(input, blinksPart1);
        Console.WriteLine(totalStonesPart1);

        
        BigInteger totalStonesPart2 = ProcessBlinks(input, blinksPart2);
        Console.WriteLine(totalStonesPart2);
    }

    static BigInteger ProcessBlinks(Dictionary<BigInteger, BigInteger> input, BigInteger blinks)
    {
        for (BigInteger blink = 0; blink < blinks; blink++)
        {
            Dictionary<BigInteger, BigInteger> STZählen = new Dictionary<BigInteger, BigInteger>();

            foreach (var entry in input)
            {
                BigInteger stone = entry.Key;
                BigInteger count = entry.Value;

                bool CheckEvenStr = stone.ToString().Length % 2 == 0;

                switch (stone)
                {
                    case BigInteger n when n == 0:
                        if (!STZählen.ContainsKey(1))
                            STZählen[1] = 0;
                        STZählen[1] += count;
                        break;

                    case var _ when CheckEvenStr:
                        string stoneStr = stone.ToString();
                        BigInteger middle = stoneStr.Length / 2;
                        BigInteger left = BigInteger.Parse(stoneStr.Substring(0, (int)middle));
                        BigInteger right = BigInteger.Parse(stoneStr.Substring((int)middle));

                        if (!STZählen.ContainsKey(left))
                            STZählen[left] = 0;
                        if (!STZählen.ContainsKey(right))
                            STZählen[right] = 0;

                        STZählen[left] += count;
                        STZählen[right] += count;
                        break;

                    default:
                        BigInteger newStone = stone * 2024;

                        if (!STZählen.ContainsKey(newStone))
                            STZählen[newStone] = 0;
                        STZählen[newStone] += count;
                        break;
                }
            }

            input = STZählen;
        }

        BigInteger totalStones = 0;
        foreach (var count in input.Values)
        {
            totalStones += count;
        }

        return totalStones;
    }
}
