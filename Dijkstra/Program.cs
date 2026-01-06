using Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace keyInfluencers
{
    class Program
    {
        static void Main()
        {
            // Define the CSV paths relative to the running folder
            string unweightedCsv = Path.Combine(AppContext.BaseDirectory, "unweighted.csv");
            string weightedCsv = Path.Combine(AppContext.BaseDirectory, "weighted.csv");

      
 

            // Display menu

            Console.WriteLine("=== KeyInfluencer ===\n");
            Console.WriteLine("1. Use Unweighted CSV");
            Console.WriteLine("2. Use Weighted CSV");
            Console.Write("Select option 1 or 2: ");

            string choice = Console.ReadLine();

            try
            {
                if (choice == "1")
                {
                    // Load unweighted graph
                    GraphLoader.Path = unweightedCsv;
                    GraphLoader.LoadUnweighted();

                    var scores = InfluenceLogic.CalculateUnweighted();
                    InfluenceLogic.PrintBestWorst(scores);
                    InfluenceLogic.PrintAllScores(scores);
                }
                else if (choice == "2")
                {
                    // Load weighted graph
                    GraphLoader.Path = weightedCsv;
                    GraphLoader.LoadWeighted();

                    var scores = InfluenceLogic.CalculateWeighted();
                    InfluenceLogic.PrintBestWorst(scores);
                    InfluenceLogic.PrintAllScores(scores);
                }
                else
                {
                    Console.WriteLine("Invalid option.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}


