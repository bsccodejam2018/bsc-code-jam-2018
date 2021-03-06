﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _3___coin_jam
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StreamReader reader = File.OpenText("test.in"))
            {
                using (StreamWriter writer = File.CreateText("test.out"))
                {
                    reader.ReadLine();
                    string input = reader.ReadLine();
                    string [] inputs = input.Split(' ');
                    //the number of digits
                    int n = int.Parse(inputs[0]);
                    //the number of coins
                    int j = int.Parse(inputs[1]);
                    Calculate(n, j, writer);
                }
            }
        }

        public static void Calculate(int N, int J,TextWriter writer)
        {
            writer.WriteLine("Case #1:");
            for (int i = 0; i < J; i++)
            {
                //coins always start and end with a 1
                //generate half of the number and double up
                //this is an attempt at reducing the complexity of the problem
                //seems very hacky since none of the numbers generated by this method have base representations
                //between 2 and 10 that are prime numbers - so no need to check for prime
                string half = "1" + ConvertToBinaryWithPadding(i, N / 2 -2 ) + "1";
                string num = half + half;
                writer.Write(num);
                //get the nontrivial divisors of that jamcoin's interpretation in each base from 2 to 10
                for (int baseDigit = 2; baseDigit <= 10; baseDigit++)
                {
                    //take the base and raise to the power of half of the length, then add one
                    //again very hacky, but this number will be a nontrivial divisor of the number produced
                    //using the above method
                    writer.Write(" " + (power((ulong)baseDigit, N/2) + 1));
                }
                writer.WriteLine();
            }

        }

        public static string ConvertToBinaryWithPadding(int i, int length)
        {
            //convert i to base 2 number
            string binary = Convert.ToString(i, 2);
            //pad with 0s
            string padding = new string('0', length - binary.Length);
            return padding + binary;
        }

        private static ulong power(ulong x, int y)
        //this will let us do efficient power computations in fewer recursive calls
        //http://www.oakton.edu/user/2/somplski/C240/HTML/hk/hk1.htm - see Pow3
        {
            //if raising to the power of zero return 1
            if (y == 0)
                return 1;
            //get the number raised to 'half' the input power
            ulong halfPow = power(x, y / 2);
            //if the input power is an even number
            if (y % 2 == 0)
            {
                return halfPow * halfPow;
            }
            return (halfPow * halfPow) * x;
        }
    }
}
