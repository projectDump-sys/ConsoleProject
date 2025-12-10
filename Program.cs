using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace PanForGold
{
    class PanForGold
    {
        static void Main()
        {
            bool debugmode = false;
            bool aquavat = false;
            double m = 0;
            double g = 0;
            double ig = 0;
            double tbdmult = 1;
            ConsoleKeyInfo k;
            Console.CursorVisible = false;
        enter:
            Console.WriteLine($"Pan For Gold!\n\nPress Enter to pan for gold\nPress P to purify gold\nPress S to sell gold\nPress B to open shop\n\nImpure Gold: {ig}g\nPure Gold: {g}g\nMoney: ${m}");
        skip:
            if (Console.KeyAvailable)
            {
                k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.Enter:
                        ig = Math.Round(ig + StartPanning(tbdmult, debugmode), 3);
                        goto enter;

                    case ConsoleKey.S:
                        m = Math.Round(m + SellGold(k, ref g, ref ig), 2);
                        goto enter;

                    case ConsoleKey.P:
                        g = Math.Round(PurifyGold(ref ig, aquavat), 3);
                        goto enter;

                    case ConsoleKey.Oem3:
                        if (debugmode == false)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Debug mode has been enabled!\n");
                            Console.ResetColor();
                            debugmode = true;
                        }
                        else if (debugmode == true)
                        {
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Debug mode has been disabled\n");
                            Console.ResetColor();
                            debugmode = false;
                        }
                        goto enter;

                    default:
                        goto skip;
                }
            }
            else
            {
                goto skip;
            }
        }

        public static double StartPanning(double tbdmult, bool debugmode)
        {
            Random r = new Random();
            double w = Math.Round(((0.05 + (r.NextDouble() * (0.35 - 0.05))) * tbdmult), 3);
            if (debugmode != true)
            {
                double c = 0;
                do
                {
                    Console.Clear();
                    Console.WriteLine($"Panning {c}% complete.\n");
                    if (c == 95)
                    {
                        Console.WriteLine("(;.,");
                    }
                    else if (c == 90)
                    {
                        Console.WriteLine("(;__");
                    }
                    else if (c == 5)
                    {
                        Console.WriteLine("u/~~");
                    }
                    else if (c == 0)
                    {
                        Console.WriteLine("(~~~");
                    }
                    else if (c % 2 == 1)
                    {
                        Console.WriteLine("~~u/");
                    }
                    else if (c % 2 == 0)
                    {
                        Console.WriteLine(@"\u~~");
                    }
                    Thread.Sleep(250);
                    c += 5;
                } while (c < 100);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine($"Panning Complete!, you obtained {w}g of gold!\n");
            Console.ResetColor();
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            return w;
        }

        public static double SellGold(ConsoleKeyInfo k, ref double g, ref double ig)
        {
            double m = 0;
            const double s = 138.87;
            const double impm = 0.125;
            double gm = Math.Round((g * s), 2);
            double im = Math.Round((ig * s * impm), 2);
            double tm = Math.Round((gm + im), 2);
            if (g != 0 && ig != 0)
            {
                Console.Clear();
                Console.WriteLine($"Press I to sell {ig}g of impure gold for ${im}\nPress P to sell {g}g of pure gold for ${gm}\nPress B to sell both for ${tm}\nPress X to exit.");
            }
            else if (g != 0 && ig == 0)
            {
                Console.Clear();
                Console.WriteLine($"Press P to sell {g}g of pure gold for ${gm}\nPress X to exit.");
            }
            else if (ig != 0 && g == 0)
            {
                Console.Clear();
                Console.WriteLine($"Press I to sell {ig}g of impure gold for ${im}\nPress X to exit.");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You don't have any gold to sell!\n");
                Console.ResetColor();
                return m;
            }
        retry:
            k = Console.ReadKey(true);
            switch (k.Key)
            {
                case ConsoleKey.I:
                    if (ig != 0)
                    {
                        m = im;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {ig}g of impure gold for ${m}!\n");
                        Console.ResetColor();
                        ig = 0;
                        return m;
                    }
                    else
                    {
                        goto retry;
                    }

                case ConsoleKey.P:
                    if (g != 0)
                    {
                        m = gm;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {g}g of pure gold for ${m}!\n");
                        Console.ResetColor();
                        g = 0;
                        return m;
                    }
                    else
                    {
                        goto retry;
                    }

                case ConsoleKey.B:
                    if (g != 0 && ig != 0)
                    {
                        m = tm;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {g}g of gold and {ig}g of impure gold for ${m}!\n");
                        Console.ResetColor();
                        g = 0;
                        ig = 0;
                        return m;
                    }
                    else
                    {
                        goto retry;
                    }

                case ConsoleKey.X:
                    m = 0;
                    Console.Clear();
                    Console.WriteLine("You successfully exited the sell menu.\n");
                    return m;

                default:
                    goto retry;
            }
        }

        public static double PurifyGold(ref double ig, bool aquavat)
        {
            if (aquavat == true)
            {
                if (ig != 0)
                {
                    double g = Math.Round((ig * 0.208333333), 3);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You purified {ig}g of impure gold into {g}g of pure gold!\n");
                    Console.ResetColor();
                    ig = 0;
                    return g;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You don't have any gold to purify!\n");
                    double g = 0;
                    return g;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You need a vat of aqua regia to purify gold.\n");
                double g = 0;
                return g;
            }
        }
    }
}