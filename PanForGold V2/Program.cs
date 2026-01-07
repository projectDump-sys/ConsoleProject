using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Net.Http.Headers;
using System.Reflection;
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

        public enum Places
        {
            [Description("River")] River = 0,
            [Description("Home")] Home,
            [Description("Goldburg")] City,
            [Description("Goldburg Toy Store")] ToyStore,
            [Description("Gold Depot")] HardwareStore,
            [Description("Goldburg Custom Glass")] Glassblowers
        }

        public static string ReadDescription(Places val)
        {
            FieldInfo fi = val.GetType().GetField(val.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : val.ToString();
        }
    }
    
    public class PanForGold
    {
        static void Main()
        {
            Data data = new Data();
            bool debugmode = false;
            int money = 0;
            int location = 2;
            var pureGold = 0.0;
            var impureGold = 0.0;
            var tbdmult = 1;
            ConsoleKeyInfo keyinfo;
            Console.CursorVisible = false;
        enter:
            Console.WriteLine($"Pan For Gold!\n\nPress Enter to pan for gold\nPress W to open the workshop\nPress S to sell gold\nPress B to open shop\n\nImpure Gold: {impureGold.ToString("F3")}g\nPure Gold: {pureGold.ToString("F3")}g\nMoney: ¢{money}");
        skip:
            if (Console.KeyAvailable)
            {
                keyinfo = Console.ReadKey(true);
                switch (keyinfo.Key)
                {
                    case ConsoleKey.Enter:
                        impureGold = Math.Round(impureGold + StartPanning(tbdmult, debugmode), 3);
                        goto enter;

                    case ConsoleKey.S:
                        money += SellGold(keyinfo, ref pureGold, ref impureGold);
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

                    case ConsoleKey.M:
                        Navigate(keyinfo, ref location);
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
            Random random = new Random();
            double weight = Math.Round(((0.05 + (random.NextDouble() * (0.35 - 0.05))) * tbdmult), 3);
            if (debugmode != true)
            {
                int percent = 0;
                Console.Clear();
                do
                {
                    Console.SetCursorPosition(0, 2);
                    string a = percent switch
                    {
                        95 => "(;.,",
                        90 => "(;__",
                        5 => "u/~~",
                        0 => "(~~~",
                        _ => (percent % 2 == 1) ? "~~u/" : @"\u~~"
                    };
                    Console.Write(a);
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Panning {percent}% complete.");
                    Thread.Sleep(250);
                    percent += 5;
                } while (percent < 100);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine($"Panning Complete!, you obtained {weight}g of gold!\n");
            Console.ResetColor();
            while (Console.KeyAvailable)
                Console.ReadKey(true);
            return weight;
        }

        public static int SellGold(ConsoleKeyInfo keyinfo, ref double pureGold, ref double impureGold)
        {
            int money = 0;
            const int SELLMULT = 13887;
            const double IMPUREMULT = 0.125;
            int pureGoldMoney = Convert.ToInt32((pureGold * SELLMULT));
            int impureGoldMoney = Convert.ToInt32((impureGold * SELLMULT * IMPUREMULT));
            int totalMoney = Convert.ToInt32((pureGoldMoney + impureGoldMoney));
            if (pureGold != 0 && impureGold != 0)
            {
                Console.Clear();
                Console.WriteLine($"Press I to sell {impureGold}g of impure gold for ¢{impureGoldMoney}\nPress P to sell {pureGold}g of pure gold for ¢{pureGoldMoney}\nPress B to sell both for ¢{totalMoney}\nPress X to exit.");
            }
            else if (pureGold != 0 && impureGold == 0)
            {
                Console.Clear();
                Console.WriteLine($"Press P to sell {pureGold}g of pure gold for ¢{pureGoldMoney}\nPress X to exit.");
            }
            else if (impureGold != 0 && pureGold == 0)
            {
                Console.Clear();
                Console.WriteLine($"Press I to sell {impureGold}g of impure gold for ¢{impureGoldMoney}\nPress X to exit.");
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You don't have any gold to sell!\n");
                Console.ResetColor();
                return money;
            }
        retry:
            keyinfo = Console.ReadKey(true);
            switch (keyinfo.Key)
            {
                case ConsoleKey.I:
                    if (impureGold != 0)
                    {
                        money = Convert.ToInt32(impureGoldMoney);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {impureGold}g of impure gold for ¢{money}!\n");
                        Console.ResetColor();
                        impureGold = 0;
                        return money;
                    }
                    else
                    {
                        goto retry;
                    }

                case ConsoleKey.P:
                    if (pureGold != 0)
                    {
                        money = Convert.ToInt32(pureGoldMoney);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {pureGold}g of pure gold for ¢{money}!\n");
                        Console.ResetColor();
                        pureGold = 0;
                        return money;
                    }
                    else
                    {
                        goto retry;
                    }

                case ConsoleKey.B:
                    if (pureGold != 0 && impureGold != 0)
                    {
                        money = Convert.ToInt32(totalMoney);
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You sold {pureGold}g of gold and {impureGold}g of impure gold for ¢{money}!\n");
                        Console.ResetColor();
                        pureGold = 0;
                        impureGold = 0;
                        return money;
                    }
                    else
                    {
                        goto retry;
                    }

                case ConsoleKey.X:
                    money = 0;
                    Console.Clear();
                    Console.WriteLine("You successfully exited the sell menu.\n");
                    return money;

                default:
                    goto retry;
            }
        }

        public static void Navigate(ConsoleKeyInfo keyinfo, ref int location)
        {
            int i = 1;
            int b = 0;
            Console.Clear();
            Console.WriteLine($"Where would you like to go?\n");
            foreach (Data.Places place in Enum.GetValues(typeof(Data.Places)))
            {
                if (location == 0)
                {
                    if (i > 2)
                    {
                        break;
                    }
                }
                else if (location == 1)
                {
                    if (i > 3)
                    {
                        break;
                    }
                }
                else if (location == 2)
                {
                    b = 1;
                    if (i == 1)
                    {
                        i++;
                    }
                    else if (i > 6)
                    {
                        break;
                    }
                }
                else if (location >= 3)
                {
                    Console.WriteLine($"1. Goldburg\n2. {Data.ReadDescription((Data.Places)location)} (You are here!)");
                    goto reboot;
                }
                Console.Write($"{i - b}. {Data.ReadDescription((Data.Places)(i - 1))} ");
                if (location == (i - 1))
                {
                    Console.Write("(You are here!)");
                }
                Console.Write("\n");
                i++;
            }
        reboot:
            keyinfo = Console.ReadKey(true);
            if (!int.TryParse(keyinfo.KeyChar.ToString(), out int n))
            {
                goto reboot;
            }
            else if (location == 0)
            {
                if (n <= 2 && n != 0)
                {
                    location = (n - 1);
                    Console.Clear();
                    Console.WriteLine($"You went to {Data.ReadDescription((Data.Places)location)}!\n");
                }
                else if (location == n-1)
                {
                    Console.Clear();
                    Console.WriteLine("You are already there!\n");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You can't go there!\n");
                }
            }
            else if (location == 1)
            {
                if (n <= 3 && n != 0)
                {
                    location = (n - 1);
                    Console.Clear();
                    Console.WriteLine($"You went to {Data.ReadDescription((Data.Places)location)}!\n");
                }
                else if (location == n -1)
                {
                    Console.Clear();
                    Console.WriteLine("You are already there\n");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You can't go there!\n");

                }
            }
            else if (location == 2)
            {
                if (n < 6 && n != 0)
                {
                    location = n;
                    Console.Clear();
                    Console.WriteLine($"You went to {Data.ReadDescription((Data.Places)location)}!\n");
                }
                else if (location == n)
                {
                    Console.Clear();
                    Console.WriteLine("You are already there!\n");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You can't go there!\n");
                }
            }
            else if (location >= 3)
            {
                if (n == 1)
                {
                    location = 2;
                    Console.Clear();
                    Console.WriteLine($"You went to {Data.ReadDescription((Data.Places)location)}!\n");
                }
                else if (n == 2)
                {
                    Console.Clear();
                    Console.WriteLine("You are already there!\n");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You can't go there!\n");
                }
            }
        }

        /*public static double Workshop(ref double impureGold, bool aqua, bool debugmode)
        {
            double pureGold;
            if (aqua == true)
            {
                if (impureGold != 0)
                {
                    pureGold =+ Math.Round(impureGold, 3);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You purified {impureGold}g of impure gold into {pureGold}g of pure gold!\n");
                    Console.ResetColor();
                    impureGold = 0;
                    return pureGold;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You don't have any gold to purify!\n");
                    pureGold = 0;
                    return pureGold;
                }
            }
            else 
            {
                if (debugmode == true && impureGold != 0)
                {
                    pureGold =+ Math.Round(impureGold, 3);
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You purified {impureGold}g of impure gold into {pureGold}g of pure gold!\n");
                    Console.ResetColor();
                    impureGold = 0;
                    return pureGold;
                }
                else if (debugmode == false)
                {
                    Console.Clear();
                    Console.WriteLine("This feature is not implemented yet.\n");
                    pureGold = 0;
                    return pureGold;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You don't have enough gold to purify!\n");
                    pureGold = 0;
                    return pureGold;
                }
            }
        }

        public static void Shop(ConsoleKeyInfo keyinfo, Data data, ref int money)
        {
            Console.Clear();
        reset:
            int i = 0;
            Console.WriteLine("Welcome to the shop!\nPress a number key to select one of the options or Press X to exit\n");
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
            keyinfo = Console.ReadKey(true);
            if (keyinfo.Key == ConsoleKey.X)
            {
                Console.Clear();
                Console.WriteLine("You exited the shop.\n");
                return;
            }
            else if (!int.TryParse(keyinfo.KeyChar.ToString(), out int n))
            {
                goto retry;
            }
            else
            {
                try
                {
                    if (!data.shopItems[n - 1].Owned == true && money >= data.shopItems[n - 1].Price)
                    {
                        data.shopItems[n - 1].Owned = true;
                        money -= data.shopItems[n - 1].Price;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"You purchased {data.shopItems[n - 1].Name} for ¢{data.shopItems[n - 1].Price}!\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"You do not have enough money! You need ¢{data.shopItems[n - 1].Price} to buy a {data.shopItems[n - 1].Name}.\n");
                        Console.ResetColor();
                        goto reset;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("That item does not exist!\n");
                    Console.ResetColor();
                    goto reset;
                }
            }
        }*/
    }
}