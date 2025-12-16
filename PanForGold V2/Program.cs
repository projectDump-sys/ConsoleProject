using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Transactions;

namespace PanForGold_V2
{
    public class Data
    {
        public struct ShopItems
        {
            public string Name;
            public int Price;
            public bool Owned;

            public ShopItems(string name, int price, bool owned)
            {
                Name = name;
                Price = price;
                Owned = owned;
            }
        }

        public ShopItems[] shopItems = new ShopItems[]
        {
            new ShopItems("Propane Torch", 3999, false),
            new ShopItems("Propane Refill", 2499, false),
            new ShopItems("Graphite Crucible (1kg)", 4999, false),
            new ShopItems("Metal Bucket", 3999, false),
            new ShopItems("Paint Bucket", 1999, false),
            new ShopItems("Insulating Material", 5999, false)
        };
    }
    
    public class PanForGold
    {
        static void Main()
        {
            Data data = new Data();
            bool debugmode = false;
            bool aqua = false;
            int m = 0;
            var g = 0.0;
            var ig = 0.0;
            var tbdmult = 1;
            ConsoleKeyInfo k;
            Console.CursorVisible = false;
        enter:
            Console.WriteLine($"Pan For Gold!\n\nPress Enter to pan for gold\nPress W to open the workshop\nPress S to sell gold\nPress B to open shop\n\nImpure Gold: {ig}g\nPure Gold: {g}g\nMoney: ¢{m}");
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
                        m += SellGold(k, ref g, ref ig);
                        goto enter;

                    case ConsoleKey.W:
                        g += Workshop(ref ig, aqua, debugmode);
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

                    case ConsoleKey.B:
                        Shop(k, data, ref m);
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
                int c = 0;
                Console.Clear();
                do
                {
                    Console.SetCursorPosition(0, 2);
                    string a = c switch
                    {
                        95 => "(;.,",
                        90 => "(;__",
                        5 => "u/~~",
                        0 => "(~~~",
                        _ => (c % 2 == 1) ? "~~u/" : @"\u~~"
                    };
                    Console.Write(a);
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Panning {c}% complete.");
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

        public static int SellGold(ConsoleKeyInfo k, ref double g, ref double ig)
        {
            int m = 0;
            const int s = 13887;
            const double impm = 0.125;
            int gm = Convert.ToInt32((g * s));
            int im = Convert.ToInt32((ig * s * impm));
            int tm = Convert.ToInt32((gm + im));
            if (g != 0 && ig != 0)
            {
                Console.Clear();
                Console.WriteLine($"Press I to sell {ig}g of impure gold for ¢{im}\nPress P to sell {g}g of pure gold for ¢{gm}\nPress B to sell both for ¢{tm}\nPress X to exit.");
            }
            else if (g != 0 && ig == 0)
            {
                Console.Clear();
                Console.WriteLine($"Press P to sell {g}g of pure gold for ¢{gm}\nPress X to exit.");
            }
            else if (ig != 0 && g == 0)
            {
                Console.Clear();
                Console.WriteLine($"Press I to sell {ig}g of impure gold for ¢{im}\nPress X to exit.");
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
                        m = Convert.ToInt32(im);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {ig}g of impure gold for ¢{m}!\n");
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
                        m = Convert.ToInt32(gm);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {g}g of pure gold for ¢{m}!\n");
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
                        m = Convert.ToInt32(tm);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {g}g of gold and {ig}g of impure gold for ¢{m}!\n");
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

        public static double Workshop(ref double ig, bool aqua, bool debugmode)
        {
            double m = 0.208333333333;
            if (aqua == true)
            {
                if (ig != 0)
                {
                    double g =+ Math.Round((ig * m), 3);
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
                if (debugmode == true)
                {
                    double g =+ Math.Round((ig * m), 3);
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
                    Console.WriteLine("You need a container to put aqua regia in.\n");
                    double g = 0;
                    return g;
                }
            }
        }

        public static void Shop(ConsoleKeyInfo k, Data data, ref int m)
        {
            int i = 0;
            Console.Clear();
            Console.WriteLine("Welcome to the shop!\nPress a number key to select one of the options\n");
            foreach (var v in data.shopItems)
            {
                Console.Write($"{i+1}. {data.shopItems[i].Name,-25} ¢{data.shopItems[i].Price} ");
                if (data.shopItems[i].Owned == true)
                {
                    Console.Write("(Owned)");
                }
                Console.Write("\n");
                i++;
            }
        retry:
            k = Console.ReadKey(true);
            if (!int.TryParse(k.KeyChar.ToString(), out int n))
            {
                goto retry;
            }
            else
            {
                if (!data.shopItems[n - 1].Owned == true && m >= data.shopItems[n - 1].Price)
                {
                    data.shopItems[n - 1].Owned = true;
                    m -= data.shopItems[n - 1].Price;
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You purchased {data.shopItems[n-1].Name} for ¢{data.shopItems[n-1].Price}!\n");
                    Console.ResetColor();
                }
                else
                {

                }
            }
        }
    }
}