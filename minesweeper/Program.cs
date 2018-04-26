using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace minesweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Generate generate = new Generate();
            Tile[,] map = generate.Map();
            while (true)
            {
                generate.Draw(map);
                Console.WriteLine("type coordinates you want to check, try must be separated by comma x,y");
                Console.WriteLine("eg 0, 3");
                string input = Console.ReadLine();
                Regex.Replace(input, @"\s+", "");
                Char sep = ',';
                string[] coords = input.Split(sep);
                int[] coordsInt = new int[2];
                if(coords.Length == 2 && int.TryParse(coords[0], out coordsInt[0]) && int.TryParse(coords[1], out coordsInt[1]) && coordsInt[0] < 15 && coordsInt[1] < 15)
                {
                    if(map[coordsInt[1], coordsInt[0]].revealed == false)
                    {
                        map[coordsInt[1], coordsInt[0]].revealed = true;
                    }
                    else
                    {
                        Console.WriteLine("You already checked this field");
                        Console.WriteLine("press any key to continue...");
                        Console.ReadKey();
                    }
                    if (map[coordsInt[1], coordsInt[0]].mine == true)
                    {
                        generate.Draw(map, true);
                        Console.WriteLine("You lost");
                        Console.WriteLine("press any key to exit...");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }

                }
                else if (int.TryParse(coords[0], out coordsInt[0]) && int.TryParse(coords[1], out coordsInt[1]))
                {
                    Console.WriteLine("X and Y range is 0-15...");
                    Console.WriteLine("press any key to continue...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("It seems u typed numbers in wrong format...");
                    Console.WriteLine("press any key to continue...");
                    Console.ReadKey();
                }

                /*win condition*/
                int revealedTiles = 0;
                foreach(Tile item in map)
                {
                    if(item.revealed == true)
                    {
                        revealedTiles++;
                    }
                }
                if (revealedTiles == 10)
                {
                    generate.Draw(map);
                    Console.WriteLine("You won");
                    Console.WriteLine("press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }     
        }
    }

    struct Tile
    {
        public bool revealed, mine;
    }
    class Generate
    {
        public Tile[,] Map()
        {
            Tile[,] map = new Tile[16, 16];
            Random rand = new Random();
            List<int[]> results = new List<int[]>();

            results.Add(new int[] {rand.Next(0, 15), rand.Next(0, 15)});
            while (results.Count() < 10)
            {
                int curY = rand.Next(0, 15);
                int curX = rand.Next(0, 15);
                int[] coords = {curY, curX};

                List<int[]> check = new List<int[]>();

                for (int g = 0; g < results.Count(); g++)
                {
                    if (!Equals(results[g],coords))
                    {
                        check.Add(coords);
                    }
                }
                if(check.Count() == results.Count())
                {
                    results.Add(coords);
                }
                
            }
            for (int g = 0; g < results.Count(); g++)
            {
                map[results[g][0], results[g][1]].mine = true;             
            }
            return map;
        }
        public void Draw(Tile[,] map, bool end = false)
        {
            string singleLine = "";
            Console.WriteLine("+-----------------");
            for (int i = 0; i < 16; i++)
            {
                singleLine += "| ";
                for (int g = 0; g < 16; g++)
                {
                    if (map[i, g].revealed == true || end == true)
                    {
                        if (map[i, g].mine == true)
                        {
                            singleLine += "#";
                        }
                        else
                        {
                            int minesAround = 0;

                            if (i > 0 && map[i - 1, g].mine == true)
                            {
                                minesAround++;
                            }
                            if (i > 0 && g > 0 && map[i - 1, g - 1].mine == true)
                            {
                                minesAround++;
                            }
                            if (i > 0 && g < 15 && map[i - 1, g + 1].mine == true)
                            {
                                minesAround++;
                            }
                            if (i < 15 && g > 0 && map[i + 1, g - 1].mine == true)
                            {
                                minesAround++;
                            }
                            if (g > 0 && map[i, g - 1].mine == true)
                            {
                                minesAround++;
                            }
                            if (i < 15 && map[i + 1, g].mine == true)
                            {
                                minesAround++;
                            }
                            if (g < 15 && map[i, g + 1].mine == true)
                            {
                                minesAround++;
                            }      
                            if (i < 15 && g < 15 && map[i + 1, g + 1].mine == true)
                            {
                                minesAround++;
                            }
                            if(minesAround == 0)
                            {
                                singleLine += "-";
                            }
                            else
                            {
                                singleLine += minesAround;
                            }
                            
                        }
                    }
                    else
                    {
                        singleLine += "/";
                    }
                }
                Console.WriteLine(singleLine);
                singleLine = "";
            }
        }
    }
}
