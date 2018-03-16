using System;
using System.IO;

public class OversizedPancakeFlipper {

	public static void Main() {
		string name = "A-small-practice";
		string path = "C:/bsc/circles/codejam/oversized pancake flipper/";

        //Create a reader and writer
		TextReader reader = new StreamReader(path + name + ".in");
		TextWriter writer = new StreamWriter(path + name + ".out");
		
		int testCases = int.Parse(reader.ReadLine());
		for (int testCase = 1; testCase <= testCases; testCase++) {
			
            //split out the row of pancakes and the flipper size
			string[] line = reader.ReadLine().Split(' ');
			char[] pancakes = line[0].ToCharArray();
			int flipper = int.Parse(line[1]);

            //use a var to hold the flips required
			var flips_required = 0;
            for (var pancake = 0; pancake < pancakes.Length; pancake++)
            {
                //if we encounter an upside down pancake
                if (pancakes[pancake] == '-')
                {
                    //if we need to flip, but there aren't enough pancakes left = impossible
                    if (pancake + flipper > pancakes.Length)
                    {
                        flips_required = -1;
                        break;
                    }
                    //flip from this unside down point forward
                    for (var j = 0; j < flipper; j++)
                    {
                        if (pancakes[pancake + j] == '-')
                        {
                            pancakes[pancake + j] = '+';
                        }
                        else
                        {
                            pancakes[pancake + j] = '-';
                        }
                    }

                    flips_required++;
                }
            }

            //write out the results if not IMPOSSIBLE
            if (flips_required >= 0)
            {
                writer.WriteLine($"Case #{testCase}: {flips_required}");
            }
            else
            {
                writer.WriteLine($"Case #{testCase}: IMPOSSIBLE");
            }

			writer.Flush();
		}

		writer.Close();
		reader.Close();
	}
}