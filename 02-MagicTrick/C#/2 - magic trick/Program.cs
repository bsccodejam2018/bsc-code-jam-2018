using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicTrick
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var reader = new StreamReader("A-small-practice.in"))
            {
                using (var writer = new StreamWriter("A-small-practice.out"))
                {
                    var T = int.Parse(reader.ReadLine());
                    //for each case we are evaluating
                    for (int t = 0; t < T; t++)
                    {
                        Console.WriteLine("Case #{0}", t + 1);
                        //answer 1 is the row the person said the card was in
                        var answer_1 = int.Parse(reader.ReadLine());
                        var candidates_1 = new List<string>();
                        //for each row of cards
                        for (int i = 0; i < 4; i++)
                        {
                            var line = reader.ReadLine();
                            //when we get the the row that was stated as the answer
                            if (i + 1 == answer_1)
                            {
                                //get the candidates from the row
                                candidates_1 = line.Split(' ').ToList();
                            }
                        }
                        //answer 2 is the row the person said the card was in after the shuffle
                        var answer_2 = int.Parse(reader.ReadLine());
                        var answer = "";
                        var counter = 0;
                         //for each row of cards
                        for (int i = 0; i < 4; i++)
                        {
                            var line = reader.ReadLine();
                            //when we get the the row that was stated as the answer
                            if (i+1 == answer_2)
                            {
                                //get the candidates from the row
                                var candidates_2 = line.Split(' ').ToList();
                                
                                //if a card in the second set is present in the first set add to the counter
                                //and get the answer
                                foreach (var card in candidates_2)
                                {
                                    if (candidates_1.Contains(card))
                                    {
                                        counter++;
                                        answer = card;
                                    }
                                }
                            }
                        }
                        if (counter == 0)
                        {
                            writer.WriteLine("Case #{0}: {1}", t+1, "Volunteer cheated!");
                        }
                        else if (counter == 1)
                        {
                            writer.WriteLine("Case #{0}: {1}", t+1, answer);
                        } else if (counter > 1)
                        {
                            writer.WriteLine("Case #{0}: {1}", t+1, "Bad magician!");
                        }
                    }
                }
            }
            Console.WriteLine("Done.");
        }
    }
}
