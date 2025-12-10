using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;

namespace PanForGold
{
    public enum Pans { Rusty = 0, Basic, Large, Steel, Silver, Gold, Diamond }
    public class Data
    {
        public static readonly Dictionary<Pans, double> PanPrice = new Dictionary<Pans, double>
        {
            { Pans.Rusty, 0 }, { Pans.Basic, 99.99 }, { Pans.Large, 249.99 }, { Pans.Steel, 449.99 }, { Pans.Silver, 749.99 }, { Pans.Gold, 1499.99 }, { Pans.Diamond, 2499.99 }
        };
        public readonly Dictionary<Pans, string> PanName = new Dictionary<Pans, string>
        {
            { Pans.Rusty, "Rusty" }, { Pans.Basic, "Basic" }, { Pans.Large, "Large" }, { Pans.Steel, "Steel" }, { Pans.Silver, "Silver" }, { Pans.Gold, "Gold" }, { Pans.Diamond, "Diamond" }
        };
        public static Dictionary<Pans, bool> PanPurchased = new Dictionary<Pans, bool>
        {
            { Pans.Rusty, true }, { Pans.Basic, false }, { Pans.Large, false }, { Pans.Steel, false }, { Pans.Silver, false }, { Pans.Gold, false }, { Pans.Diamond, false }
        };

    }

    static class Program
    {
        static void Main()
        {
            double Muns = 0;
            double ImpGold = 0;
            double PureGold = 0;
            double PanWeight;
            double SellMult = 138.87;
            double SellGain = 0;
            int PanLvl = 0;
            double PanMult = 1;
            int cent = 0;
            int pressed = 0;
            ConsoleKeyInfo k;
            Random rand = new Random();
            Console.CursorVisible = false;
            while (true)
            {
                string muny = Muns.ToString("F2");
                Pans usingpan = (Pans)PanLvl;
                Console.WriteLine($"Pan For Gold!\n\nControls:\nEnter: Pan for Gold for 1 Hour\nS: Sell Gold\nB: Open the Pan Shop\nP: Purify Gold\n\nImpure Gold: {ImpGold}g\nPure Gold: {PureGold}g\n\nYou have ${muny}\n\nUsing: {usingpan} Pan ({PanMult}x weight multiplier)");
                PanWeight = (0.05 + (rand.NextDouble() * (0.35 - 0.05))) * PanMult;
                PanWeight = Math.Round(PanWeight, 2);
                k = Console.ReadKey(true);
                switch (k.Key)
                {
                    case ConsoleKey.Enter:
                    ImpGold += PanWeight;
                    ImpGold = Math.Round(ImpGold, 2);
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Panning {cent}% Complete.\n");
                        if (cent == 95)
                        {
                            Console.WriteLine("(;.,");
                        }
                        else if (cent == 90)
                        {
                            Console.WriteLine("(;__");
                        }
                        else if (cent == 5)
                        {
                            Console.WriteLine("u/~~");
                        }
                        else if (cent == 0)
                        {
                            Console.WriteLine("(~~~");
                        }
                        else if (cent % 2 == 1)
                        {
                            Console.WriteLine("~~u/");
                        }
                        else if (cent % 2 == 0)
                        {
                            Console.WriteLine(@"\u~~");
                        }
                        Thread.Sleep(250);
                        cent += 5;
                    } while (cent < 100);
                    cent = 0;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Clear();
                    Console.WriteLine($"Panning for gold 100% complete! You got {PanWeight}g\n");
                    Console.ResetColor();
                    pressed--;
                    break;

                    case ConsoleKey.S:
                        Console.Clear();
                        Console.WriteLine($"Press I to sell impure gold ({ImpGold}g, ${Math.Round((SellMult * 0.2), 2)}/g), P to sell pure gold ({PureGold}g ${SellMult}/g), or B to sell both.");
                        k = Console.ReadKey();
                        switch (k.Key)
                        {
                            case ConsoleKey.I:
                                SellGain = Math.Round(ImpGold * (SellMult * 0.2), 2);
                                Muns += SellGain;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Clear();
                                Console.WriteLine($"You sold {ImpGold}g of impure gold for ${SellGain}!\n");
                                Console.ResetColor();
                                ImpGold = 0;
                                break;

                            case ConsoleKey.P:
                                SellGain = Math.Round((PureGold * SellMult), 2);
                                Muns += SellGain;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Clear();
                                Console.WriteLine($"You sold {PureGold}g of pure gold for ${SellGain}!\n");
                                Console.ResetColor();
                                PureGold = 0;
                                break;

                            case ConsoleKey.B:
                                SellGain = Math.Round(ImpGold * (SellMult * 0.2), 2) + Math.Round((PureGold * SellMult), 2);
                                Muns += SellGain;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Clear();
                                Console.WriteLine($"You sold {ImpGold}g of impure gold and {PureGold}g of pure gold for ${SellGain}!\n");
                                Console.ResetColor();
                                PureGold = 0;
                                ImpGold = 0;
                                break;
                        }
                        break;

                    case ConsoleKey.P:
                        Console.Clear();
                        Console.WriteLine($"Press Enter to purify, press X to exit, you can only purify 10g of gold at a time (You have {ImpGold}g of impure gold)");
                        k = Console.ReadKey();
                        switch (k.Key)
                        {
                            case ConsoleKey.Enter:
                                double purifying = 0;
                                double output = 0;
                                if (ImpGold == 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine("You do not have enough gold to purify.\n");
                                    break;
                                }
                                else if (ImpGold < 10)
                                {
                                    purifying = ImpGold; 
                                    ImpGold = 0;
                                }    
                                else if (ImpGold >= 10)
                                {
                                    purifying = 10;
                                    ImpGold = Math.Round(ImpGold - 10, 2);
                                }
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine($"Purifying gold {cent}% complete.\n");
                                    switch (cent)
                                    {
                                        case var _ when cent % 5 == 4:
                                            Console.Write("----o");
                                            break;
                                        case var _ when cent % 5 == 3:
                                            Console.Write("---o-");
                                            break;
                                        case var _ when cent % 5 == 2:
                                            Console.Write("--o--");
                                            break;

                                        case var _ when cent % 5 == 1:
                                            Console.Write("-o---");
                                            break;

                                        case var _ when cent % 5 == 0:
                                            Console.Write("o----");
                                            break;
                                    }
                                    Thread.Sleep(50);
                                    cent += 1;
                                } while (cent < 100);
                                output = (0.2 + (rand.NextDouble() * (0.35 - 0.2))) * purifying;
                                output = Math.Round(output, 2);
                                PureGold += output;
                                Console.Clear();
                                Console.WriteLine($"You purified {purifying}g of gold and got {output}g of gold!\n");
                                purifying = 0;
                                output = 0;
                                break;

                            case ConsoleKey.X:
                                break;
                        }
                    break;

                    case ConsoleKey.B:
                    Console.Clear();
                    Console.WriteLine($"Welcome to the Shop!\n\nOptions:\nU: Upgrade Pan\nS: Buy a Sluice (not available)\n\nPress X to exit the shop.");
                    k = Console.ReadKey(true);
                        if (k.Key == ConsoleKey.U)
                        {
                        retry:
                            int i = 1;
                            Console.Clear();
                            Console.WriteLine("Pan Options:\n");
                            foreach (Pans pan in Enum.GetValues(typeof(Pans)))
                            {
                                double panprice;
                                Data.PanPrice.TryGetValue(pan, out panprice);
                                string price = panprice.ToString("F2");
                                Console.Write($"{i}. {pan} pan: ${price} ");
                                if (Data.PanPurchased[pan] == true)
                                {
                                    Console.Write("(Owned)");
                                }
                                Console.Write("\n");
                                i++;
                            }
                            Console.WriteLine("\nPress X to exit the shop.");
                            k = Console.ReadKey(true);
                            if (char.IsDigit(k.KeyChar))
                            {
                                string s; int n;
                                s = k.KeyChar.ToString();
                                if (!int.TryParse(s, out n))
                                {
                                    goto retry;
                                }
                                if (n <= 7)
                                {
                                    int val = n - 1;
                                    string dis = Enum.GetName(typeof(Pans), val)!;
                                    double panprice = Data.PanPrice[(Pans)val];
                                    if (Data.PanPurchased[(Pans)val] == true)
                                    {
                                        if (PanLvl == val)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("You already have that pan selected.\n");
                                        }
                                        else
                                        {
                                            PanLvl = val;
                                            PanMult = 1 + (PanLvl * 0.5);
                                            Console.Clear();
                                            Console.WriteLine($"{dis} pan has been selected.\n");
                                        }
                                    }
                                    else
                                    {
                                        if (Muns >= panprice)
                                        {
                                            PanLvl = val;
                                            PanMult = 1 + (PanLvl * 0.5);
                                            Muns -= panprice;
                                            Data.PanPurchased[(Pans)val] = true;
                                            Console.Clear();
                                            Console.WriteLine($"You successfully bought the {dis} pan for ${panprice}!\n");
                                        }
                                        else
                                        {
                                            double funds = panprice - Muns;
                                            funds = Math.Round(funds, 2);
                                            Console.Clear();
                                            Console.WriteLine($"Insufficient funds to purchase the {dis} pan. (at least ${funds} extra needed)\n");
                                        }
                                    }
                                }
                                else
                                {
                                    goto retry;
                                }
                            }
                            else if (k.Key == ConsoleKey.X)
                            {
                                Console.Clear();
                                continue;
                            }
                            else
                            {
                                goto retry;
                            }
                        }
                        else
                        {
                            Console.Clear();
                            continue;
                        }
                    break;

                    default:
                        Console.Clear();
                    break;
                } 
            } 
        }
    }
}