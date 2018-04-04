using System;
using System.IO;
using System.Collections.Generic;

public class Ratatouille
{

	public static void Main(string[] args)
	{	
        using (StreamReader reader = File.OpenText("B-large-practice.in"))
        {
            using (StreamWriter writer = File.CreateText("B-large-practice.out"))
            {
                var T = int.Parse(reader.ReadLine());
                for (var iteration = 1; iteration <= T; iteration++)
                {
                    var input = reader.ReadLine().Split(' ');
                    //sizes of packets that need to be filled
                    var packetSizes = reader.ReadLine().Split(' ');
                    //number of ingredients
                    var N = int.Parse(input[0]);
                    //number of packages
                    var P = int.Parse(input[1]);

                    var ranges = new Queue<RangeGenerator>[N];

                    //for each of the ingredients
                    for (var i = 0; i < N; i++)
                    {
                        //create a container to whole the range of ingredient options
                        var ingredientQueue = new List<RangeGenerator>();
                        //get the packet size
                        var packet = int.Parse(packetSizes[i]);
                        //get all of the available ingredients to use
                        var availableMass = reader.ReadLine().Split(' ');

                        //for each package
                        for (var j = 0; j < P; j++)
                        {
                            //see how much we have to work with
                            var mass = int.Parse(availableMass[j]);
                            //return the possible range of packets we can pack with the 10% delta
                            var range = RangeGenerator.For(packet, mass);
                            //if we have a valid range, add to the ingredient queue
                            if (range != null)
                            {
                                ingredientQueue.Add(range);
                            }
                        }

                        //sort the ingredient queue from smallest to largest
                        ingredientQueue.Sort();
                        //add the ingredient queue to the range container in a queue we will use later
                        ranges[i] = new Queue<RangeGenerator>(ingredientQueue);
                    }

                    writer.Write("Case #{0}: {1}\n", iteration, Solve(ranges));
                }
            }
        }
	}

    //solver class - does the heavy lifting
	private static int Solve(Queue<RangeGenerator>[] ranges)
	{
        //number of packs we can produce
		var packedKits = 0;
		//use the packet at the start of the list
        //if you can't; remove the item from the queue
		while (true)
		{
            //container variables
            var smallestMax = 99999999999;
			var largestMin = 0;

			foreach (var queue in ranges)
			{
                //if we can't pack any kits, return 0
				if (queue.Count == 0)
				{
					return packedKits;
				}

                //get the first range in the queue
				var range = queue.Peek();

                //if the smallest number in the range is bigger than the largest min, update variable
				if (range.Min > largestMin)
				{
					largestMin = range.Min;
				}

                //if the largest number in the range is smaller than the smallest max, update variable
				if (range.Max < smallestMax)
				{
					smallestMax = range.Max;
				}
			}

            //if the largest min is smaller than or equal to the smallest max
			if (largestMin <= smallestMax)
			{
				//build the kit!!
				packedKits++;
                //clean up the queue
				foreach (var queue in ranges)
				{
					//Removes and returns the object at the beginning of the queue
					queue.Dequeue();
				}
			}
			else
			{
				// remove options where the largest min is larger
                //than the max in the range
				foreach (var queue in ranges)
				{
					var range = queue.Peek();

					if (range.Max < largestMin)
					{
						queue.Dequeue();
					}
				}
			}
		}
	}

    //this is the range class that will hold the potential ranges
    //the IComparable will make comparisions easier later on
    //https://msdn.microsoft.com/en-us/library/system.icomparable(v=vs.110).aspx
	private class RangeGenerator : IComparable<RangeGenerator>
	{
		public int Min { get; private set; }
		public int Max { get; private set; }

        //return type
		private RangeGenerator(int min, int max)
		{
			Min = min;
			Max = max;
		}

		public static RangeGenerator For(int targetMass, int availableMass)
		{
            //generate the range
            //10% variance
			//this pieces was more of a headache than anticipated
			int max = (int)Math.Floor((availableMass * 100.0) / (targetMass * 90.0));
			int min = (int)Math.Floor((availableMass * 100.0) / (targetMass * 110.0));
			//if we have enough, boost the min
			if (availableMass * 100.0 > min * (targetMass * 110.0)) min++;

			if (max < min) return null;
			else return new RangeGenerator(min, max);
		}

        //we need to include this to use IComparable<Range>
		//https://stackoverflow.com/questions/4188013/how-to-implement-icomparable-interface
		public int CompareTo(RangeGenerator other)
		{
			if (Min < other.Min || Min == other.Min && Max < other.Max)	return -1;
			else if (Min == other.Min && Max == other.Max) return 0;
			else return 1;
		}
	}
}