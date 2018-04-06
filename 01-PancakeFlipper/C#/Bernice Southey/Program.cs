using System.IO;
using System.Linq;

namespace Problem1
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = File.ReadAllLines(@"..\..\..\..\..\Input\A-small-practice.in");
            var output = new string[100];
            int count = int.Parse(input[0]);
            for (int i = 0; i < count; i++)
            {
                var line = input[i + 1];

                var pancakes = line.Split(' ')[0].ToCharArray();
                var flipper = int.Parse(line.Split(' ')[1]);

                var result = FlipAll(pancakes, flipper);
                output[i] = string.Format($"CASE #{i + 1}: {(result == -1 ? "IMPOSSIBLE" : result.ToString())}");
            }
            File.WriteAllLines(@"..\..\..\..\..\Output\A-small-practice.out1", output);
        }

        private static int FlipAll(char[] pancakes, int flipper, int count = 0)
        {
            if (pancakes.Length == flipper)
            {
                if (pancakes.All(p => p == '+')) return count;
                if (pancakes.All(p => p == '-')) return count + 1;
                return -1;
            }

            if (pancakes[0] == '+') return FlipAll(pancakes.Skip(1).ToArray(), flipper, count);
            Flip(pancakes, flipper);
            return FlipAll(pancakes, flipper, count + 1);
        }

        private static void Flip(char[] pancakes, int flipper)
        {
            for (int i = 0; i < flipper; i++)
            pancakes[i] = pancakes[i] == '+' ? '-' : '+';
        }
    }
}
