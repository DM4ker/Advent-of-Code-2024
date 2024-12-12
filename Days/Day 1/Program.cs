using System;
using System.Collections.Generic;
using System.IO;

namespace AOCNr1
{
    class Program
    {
        public static void Main()
        {
            TxtInput txtInput = new TxtInput();

            Split split = new Split(txtInput.input);

            Calculate calc = new Calculate(split.input_A, split.input_B);


            
            calc.Part1();

            Console.WriteLine(calc.output);

            calc.Part2();
            Console.WriteLine(calc.output);
        }
    }

    public class TxtInput
    {
        private long[] Input;

        public long[] input
        {
            get
            {
                if (Input == null)
                {
                    Input = InputToArr();
                }
                return Input;
            }
            set
            {
                Input = value;
            }
        }

        private long[] InputToArr()
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "InputAOC1.txt");

            List<long> numbers = new List<long>();

            string[] lines = File.ReadAllLines(filePath);


            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string part in parts)
                {
                    if (long.TryParse(part, out long number))
                    {
                        numbers.Add(number);
                    }
                    else
                    {
                        Console.WriteLine($"Cannot Parse part: {part}");
                    }
                }
            }

            return numbers.ToArray();
        }
    }

    public class Split
    {
        private long[] Input_A;
        private long[] Input_B;

        public long[] input_A
        {
            get { return Input_A; }
            set { Input_A = value; }
        }

        public long[] input_B
        {
            get { return Input_B; }
            set { Input_B = value; }
        }

        public Split(long[] input)
        {
            int length = input.Length;

            Input_A = new long[length / 2 + length % 2];
            Input_B = new long[length / 2];

            int indexA = 0;
            int indexB = 0;

            for (int i = 0; i < length; i++)
            {
                if (i % 2 == 0)
                {
                    Input_A[indexA++] = input[i]; 
                }
                else
                {
                    Input_B[indexB++] = input[i]; 
                }
            }

        }
    }

    public class Calculate
    {
        public Calculate(long[] inputA, long[] inputB)
        {
            Input_A = inputA;
            Input_B = inputB;
            Compare = new long[Math.Min(inputA.Length, inputB.Length)];
        }
        
        private long[] Input_A;
        private long[] Input_B;
        private long[] Compare;
        private long OutputA;

        public long output
        {
            get { return OutputA; }
            set { OutputA = value; }
        }

        public long[] compare
        {
            get { return Compare; }
            set { Compare = value; }
        }


        public void Part1() 
        {

            for (int i = 0; i < Input_A.Length - 1; i++)
            {
                for (int j = 0; j < Input_A.Length - i - 1; j++)
                {
                    if (Input_A[j] > Input_A[j + 1])
                    {
                       
                        long temp = Input_A[j];
                        Input_A[j] = Input_A[j + 1];
                        Input_A[j + 1] = temp;
                    }
                }
            }

            
            for (int i = 0; i < Input_B.Length - 1; i++)
            {
                for (int j = 0; j < Input_B.Length - i - 1; j++)
                {
                    if (Input_B[j] > Input_B[j + 1])
                    {
                        
                        long temp = Input_B[j];
                        Input_B[j] = Input_B[j + 1];
                        Input_B[j + 1] = temp;
                    }
                }
            }

            for (int i = 0; i < Compare.Length; i++)
            {
                if (Input_A[i] >= Input_B[i])
                {
                    Compare[i] = Input_A[i] - Input_B[i];
                }
                else
                {
                    Compare[i] = Input_B[i] - Input_A[i];
                }
            }

            foreach (var value in compare)
            {
                output += value; 
            }


        }

        public void Part2()
        {
            long score = 0;

           
            foreach (var numberA in Input_A)
            {
                long countInB = 0;

                
                foreach (var numberB in Input_B)
                {
                    if (numberA == numberB)
                    {
                        countInB++;
                    }
                }

                
                score += numberA * countInB;
            }

            
            output = score;
        }



    }
}
