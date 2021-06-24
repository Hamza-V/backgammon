using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

// Student Number : 1306170130
// Student Name: HAMZEH ALNEHLAWI
namespace main
{
    internal class Program
    {
        static int tableWidth = 73;

        public static class IntUtil
        {
            private static Random random;

            private static void Init()
            {
                if (random == null) random = new Random();
            }

            public static int Random(int min, int max)
            {
                Init();
                return random.Next(min, max);
            }
        }

        public static void Main(string[] args)
        {

            
            //Generating random Dice number
            Random rnd = new Random();
            int Dice1 = rnd.Next(1, 7);
            int Dice2 = rnd.Next(1, 7);

            bool X_Turn = false;
            bool Y_Turn = false;

            char Turn = '0';
            Turn = checkFirst(ref Turn, ref Dice1, ref Dice2);

            using (StreamWriter log = new StreamWriter("DiceHistory_PlayLog.txt"))
            {
                log.WriteLine(Dice1);
                log.WriteLine(Dice2);
                log.Close();

            }



            int Brokenf_X = 0;
            int Brokenf_Y = 0;

            //initiation of players arrays
            int[] xPlayer = new int[24];
            // int[] yPlayer = new int[25];

            // assigning zeros to array
            for (int i = 0; i < xPlayer.Length; i++)
            {
                xPlayer[i] = 0;
                // yPlayer[i] = 0;
            }

            //start position for Player X
            xPlayer[23] = 5;
            xPlayer[12] = 2;
            xPlayer[4] = 3;
            xPlayer[6] = 5;

            //Printing xPlayer 
            for (int i = 0; i <= xPlayer.Length - 1; i++)
            {
                Console.WriteLine("x[" + (i + 1) + "]= " + xPlayer[i]);
            }

            Console.WriteLine();
            Console.WriteLine();


            //Defining yPlayer (Reverse xPlayer)
            int[] yPlayer = Enumerable.Reverse(xPlayer).ToArray();

            for (int i = 0; i <= 23; i++)
            {
                // Loop through List with for
                Console.WriteLine("y[" + (i + 1) + "]= " + yPlayer[i]);
            }

           
            
            Console.WriteLine();
            /*for (int i = 0; i <= 10; i++)
            {
                Console.Write("Dice1 =  " + Dice1 + " , Dice2 = " + Dice2 + " , Turn : " + Turn);
                PlayX(ref Turn, ref xPlayer, ref yPlayer, ref Dice1, ref Dice2);

               // Print2Console(ref up,  ref down,  Brokenf_X,  Brokenf_Y,   Dice1,   Dice2,ref result,ref  Turn,ref xPlayer,ref  yPlayer);

                ThrowDice(ref Dice1,ref Dice2);
         
                Console.WriteLine();

            }
            */
          
            string[] result = Stones(xPlayer, yPlayer); //z.Select(x=>x.ToString()).ToArray();
            var up = result.Skip(0).Take(12).ToArray(); //an array for the upper side of the table
            var down = result.Skip(12).Take(24).ToArray();
            down = Enumerable.Reverse(down).ToArray(); //an array for the lower side of the table.
            
            Clearfile();
            bool EmptyX = false;
            bool EmptyY = false;

            var z = ArrTotal(ref xPlayer, ref yPlayer);

            //First turn
            Console.WriteLine("First Turn to: "+Turn);
            //Play(ref Turn, ref xPlayer, ref yPlayer, ref Dice1, ref Dice2);
            Print2Console( ref z, ref up,ref  down, Brokenf_X,  Brokenf_Y,  0, 0 , ref  result,ref Turn,ref xPlayer,ref yPlayer);
            //Print2File(   ref up,  ref down, Brokenf_X,  Brokenf_Y,  0, 0 ,ref Turn);

            Console.WriteLine("Dice1 : " + Dice1+ " , Dice2 = : " + Dice2);

            char winner = '0';
            char SameDice = '0';
            
            ThrowDice(ref Dice1, ref Dice2,ref SameDice);

            while (winner == '0')
            {


                using (StreamWriter log = new StreamWriter("DiceHistory_PlayLog.txt", true))
                {
                    
                    log.Write(Turn + $" {Dice1}" + $" {Dice2}");
                    if(SameDice == 'S'){                    log.WriteLine();
                         log.Write(Turn + $" {Dice1}" + $" {Dice2} Double move !");}
                    log.WriteLine();





                    Console.WriteLine();
                    Console.WriteLine("Turn of : " + Turn);
                    Console.WriteLine();

                    Console.WriteLine("Dice1 : " + Dice1 + " , Dice2 = : " + Dice2);
                    Console.WriteLine();

                    /*if (i == 0)
                    {
                        ThrowDice(ref Dice1,ref Dice2);
                        
                    }*/
                    if (SameDice == 'S')
                    {
                        for (int s = 1; s <= 2; s++)
                        {
                            Play(ref Turn, ref xPlayer, ref yPlayer, ref Dice1, ref Dice2, ref Brokenf_X, ref Brokenf_Y,
                                ref EmptyX, ref EmptyY, ref winner);

                            Print2Console(ref z, ref up, ref down, Brokenf_X, Brokenf_Y, Dice1, Dice2, ref result,
                                ref Turn,
                                ref xPlayer, ref yPlayer);

                            Print2File(ref up, ref down, Brokenf_X, Brokenf_Y, Dice1, Dice2, ref Turn);
                        }
                    }
                    else if (SameDice != 'S')
                    {
                        Play(ref Turn, ref xPlayer, ref yPlayer, ref Dice1, ref Dice2, ref Brokenf_X, ref Brokenf_Y,
                            ref EmptyX, ref EmptyY, ref winner);

                        Print2Console(ref z, ref up, ref down, Brokenf_X, Brokenf_Y, Dice1, Dice2, ref result,
                            ref Turn,
                            ref xPlayer, ref yPlayer);

                        Print2File(ref up, ref down, Brokenf_X, Brokenf_Y, Dice1, Dice2, ref Turn);
                    }
                    SameDice = '0';
                    ThrowDice(ref Dice1, ref Dice2,ref SameDice);
                    Turn = ToggleTurn(ref Turn);
                    if (winner == 'X')
                    {
                        Console.WriteLine("The winner of the game is X");
                        log.Close();
                        break;
                    }
                    else if (winner == 'Y')
                    {
                        Console.WriteLine("The winner of the game is Y");
                        log.Close();
                        break;
                    }

                }
            }

            for (int i = 0; i <= xPlayer.Length - 1; i++)
            {
                Console.WriteLine("x[" + (i + 1) + "]= " + xPlayer[i]);
            }

            Console.WriteLine();
            Console.WriteLine();

            for (int i = 0; i <= 23; i++)
            {
                // Loop through List with for
                Console.WriteLine("y[" + (i + 1) + "]= " + yPlayer[i]);
            }
            


            Console.WriteLine();



            

            




////////////////////////////////////
///
            




        }



        //Throw the dice, and play
        public static void Play(ref char Turn, ref int[] x, ref int[] y, ref int Dice1, ref int Dice2,ref int BrokenX,ref int BrokenY,ref bool EmptyX, ref bool EmptyY,ref char winner)
        {
            string[] letter = {"A1","B1","C1","D1","E1","F1","G1","H1","I1","J1","K1","L1","L5","K5","J5","I5","H5","G5","F5","E5","D5","C5","B5","A5" };
            if (Turn == 'X')
            {
                    if (x[18] + x[19] + x[20] + x[21] + x[22] + x[23] == 15)
                    {
                        EmptyX = true;
                    }

                    if (EmptyX == true)
                    {
                        Console.WriteLine("Emptying  X .....");
                        if (x[24 - Dice1] > 0)
                        {
                            x[24 - Dice1] -= 1;
                        }

                        if (x[24 - Dice2] > 0)
                        {
                            x[24 - Dice2] -= 1;
                        }

                        if (x[18] + x[19] + x[20] + x[21] + x[22] + x[23] == 0)
                        {
                            Console.WriteLine("X WON @@@@@@@@@@@@@@@@@@@@@@@@");
                            winner = 'X';
                        }
                    }
                    else
                    {
                        // loop for the Dice1
                        for (int i = 0; i <= 17; i++)
                        {
                            string ask = "";
                            
                            
                            if (BrokenX < 1)
                                    {
                                        if (x[i] > 0 && x[i + Dice1] != 5 && y[i + Dice1] == 0)
                                        {
                                            Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                            ask = Console.ReadLine();
                                            if (ask == "yes")
                                            {
                                                x[i] = x[i] - 1;
                                                x[i + Dice1] += 1;
                                                break;
                                            }
                                            else if (ask == "no")
                                            {
                                                continue;
                                            }

                                        }
                                        else if (x[i] > 0 && x[i + Dice1] != 5 && y[i + Dice1] == 1)
                                        {
                                            Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                            ask = Console.ReadLine();
                                            if (ask == "yes")
                                            {
                                                x[i] = x[i] - 1;
                                                y[i + Dice1] = 0;
                                                x[i + Dice1] += 1;
                                                BrokenY += 1;
                                                Console.WriteLine("BINGOOOOOOOOOOOOO for x = " + BrokenY);
                                                break;
                                            }
                                            else if (ask == "no")
                                            {
                                                continue;
                                            }
                                        }
                                    }
                            else if (BrokenX >= 1)
                                    {

                                        if (x[Dice1 - 1] != 5 && y[i + Dice1] == 0)
                                        {
                                            Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                            ask = Console.ReadLine();
                                            if (ask == "yes")
                                            {
                                                BrokenX -= 1;
                                                x[Dice1 - 1] += 1;
                                                Console.WriteLine(
                                                    "1 RESTORED FOR X !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice1 ");
                                                break;
                                            }
                                            else if (ask == "no")
                                            {
                                                continue;
                                            }
                                        }
                                        else if (y[i + Dice1] == 1)
                                        {
                                            Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                            ask = Console.ReadLine();
                                            if (ask == "yes")
                                            {
                                                BrokenX -= 1;
                                                y[i + Dice1] = 0;
                                                x[Dice1 - 1] += 1;
                                                BrokenY += 1;
                                                Console.WriteLine(
                                                    "1 RESTORED FOR X !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice1 and killed one");
                                                break;
                                            }
                                            else if (ask == "no")
                                            {
                                                continue;
                                            }
                                            
                                        }



                                    }
                                    
          

                        }


                        //loop for the Dice2
                        for (int i = 0; i <= 17; i++)
                        {
                            string ask = "";

                            if (BrokenX < 1)
                            {
                                if (x[i] > 0 && x[i + Dice2] != 5 && y[i + Dice2] == 0)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        x[i] = x[i] - 1;
                                        x[i + Dice2] += 1;

                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }
                                else if (x[i] > 0 && x[i + Dice2] != 5 && y[i + Dice2] == 1)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        x[i] = x[i] - 1;
                                        y[i + Dice2] = 0;
                                        x[i + Dice2] += 1;
                                        BrokenY += 1;
                                        Console.WriteLine("BINGOOOOOOOOOOOOO for x = " + BrokenY);
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }
                            }
                            else if (BrokenX >= 1)
                            {
                                if (x[Dice2 - 1] != 5 && y[i + Dice2] == 0)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {

                                        BrokenX -= 1;
                                        x[Dice2 - 1] += 1;
                                        Console.WriteLine(
                                            "1 RESTORED FOR X !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice2 ");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                }

                                if (y[i + Dice2] == 1)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        BrokenX -= 1;
                                        y[i + Dice2] = 0;
                                        x[Dice2 - 1] += 1;
                                        BrokenY += 1;
                                        Console.WriteLine(
                                            "1 RESTORED FOR X !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice2 and killed one");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }
                            }
                        }
                    }



                }

            else if (Turn == 'Y')
            {
                string ask = "";
                    if (y[0] + y[1] + y[2] + y[3] + y[4] + y[5] == 15)
                    {
                        EmptyY = true;
                    }

                    if (EmptyY == true)
                    {
                        Console.WriteLine("Emptying  Y .....");
                        if (y[Dice1 - 1] > 0)
                        {
                            y[Dice1 - 1] -= 1;
                        }

                        if (y[Dice2 - 1] > 0)
                        {
                            y[Dice2 - 1] -= 1;
                        }

                        if (y[0] + y[1] + y[2] + y[3] + y[4] + y[5] == 0)
                        {
                            Console.WriteLine("Y WON @@@@@@@@@@@@@@@@@@@@@@@@");
                            winner = 'Y';
                        }
                    }
                    else
                    {
                        // loop for the Dice1 in for Y
                        for (int i = 23; i >= 6; i--)
                        {
                            if (BrokenY < 1)
                            {
                                if (y[i] > 0 && y[i - Dice1] != 5 && x[i - Dice1] == 0)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        y[i] = y[i] - 1;
                                        y[i - Dice1] += 1;
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }

                                }
                                else if (y[i] > 0 && y[i - Dice1] != 5 && x[i - Dice1] == 1)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        y[i] = y[i] - 1;
                                        x[i - Dice1] = 0;
                                        y[i - Dice1] += 1;
                                        BrokenX += 1;
                                        Console.WriteLine("BINGOOOOOOOOOOOOO for y in Dice1");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                }
                            }
                            else if (BrokenY >= 1)
                            {

                                if (y[Dice1 - 1] != 5 && x[23 - Dice1 + 1] == 0)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        BrokenY -= 1;
                                        y[23 - Dice1 + 1] += 1;
                                        Console.WriteLine(
                                            "1 RESTORED FOR Y !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice1 ");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }
                                else if (x[23 - Dice1 + 1] == 1)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        BrokenY -= 1;
                                        x[23 - Dice1 + 1] = 0;
                                        y[23 - Dice1 + 1] += 1;
                                        BrokenX += 1;
                                        Console.WriteLine(
                                            "1 RESTORED FOR Y !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice1 and Killed one !!");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }



                            }

                        }

                        // loop for the Dice2 in for Y
                        for (int i = 23; i >= 6; i--)
                        {
                            if (BrokenY < 1)
                            {
                                if (y[i] > 0 && y[i - Dice2] != 5 && x[i - Dice2] == 0)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        y[i] = y[i] - 1;
                                        y[i - Dice2] += 1;
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }

                                }
                                else if (y[i] > 0 && y[i - Dice2] != 5 && x[i - Dice2] == 1)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        y[i] = y[i] - 1;
                                        x[i - Dice2] = 0;
                                        y[i - Dice2] += 1;
                                        BrokenX += 1;
                                        Console.WriteLine("BINGOOOOOOOOOOOOO for y from Dice2");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }
                            }
                            else if (BrokenY >= 1)
                            {

                                if (y[Dice2 - 1] != 5 && x[24 - Dice2 + 1] == 0)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        BrokenY -= 1;
                                        y[23 - Dice2 + 1] += 1;
                                        Console.WriteLine(
                                            "1 RESTORED FOR Y !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice2 ");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                    
                                }
                                else if (x[23 - Dice2 + 1] == 1)
                                {
                                    Console.WriteLine("Do you want to move the stone  "+letter[i] + " ?");
                                    ask = Console.ReadLine();
                                    if (ask == "yes")
                                    {
                                        BrokenY -= 1;
                                        x[23 - Dice2 + 1] = 0;
                                        y[23 - Dice2 + 1] += 1;
                                        BrokenX += 1;
                                        Console.WriteLine(
                                            "1 RESTORED FOR Y !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! From Dice2 and killed one !! lol ");
                                        break;
                                    }
                                    else if (ask == "no")
                                    {
                                        continue;
                                    }
                                }



                            }


                        }
                    }


                }
            
            Console.WriteLine("Broken Flakes for Y = "+ BrokenY);
            Console.WriteLine("Broken Flakes for X = "+ BrokenX);

            Console.WriteLine();

        }






        public static char ToggleTurn(ref char Turn)
        {
            char temp = 'T';
            if (Turn == 'X')
                temp =  'Y';
            else if(Turn == 'Y')
                temp = 'X';
            return temp;
        }
        
        
        
        
        //throw dice
        public static void ThrowDice(ref int d1, ref int d2,ref char SameDice)
        {
            
                d1 = IntUtil.Random(1,7);
                d2 = IntUtil.Random(1,7);
                if(d1==d2)
                {
                    Console.WriteLine("Dice1 and Dice2 are equal !! Double move !");
                    SameDice = 'S';
                    //ThrowDice(ref d1,ref d2);
                }
                
            
        }

        
        
        //Pring table to console
        public static void Print2Console(ref int [] z,ref string[] up,  ref string[] down, int Brokenf_X, int Brokenf_Y, int Dice1, int Dice2 , ref string[] result,ref char Turn,ref int[] xPlayer,ref int[] yPlayer)
        {
            
            
            //Sum two arrays
            z = ArrTotal(ref xPlayer, ref yPlayer);
            for (int j = 0; j <12 ; j++)
            {
                Console.Write(z[j]);

            }
            Console.WriteLine();
            for (int k = 23; k >= 12; k--)
            {
                Console.Write(z[k]);
            }

            Console.WriteLine();
            result = Stones(xPlayer, yPlayer); //z.Select(x=>x.ToString()).ToArray();
             up = result.Skip(0).Take(12).ToArray(); //an array for the upper side of the table
             down = result.Skip(12).Take(24).ToArray();
             down = Enumerable.Reverse(down).ToArray(); //an array for the lower side of the table.
            //  Console.Clear();
            PrintRow("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L");
            Console.WriteLine();
            PrintLine();
            PrintRow(up);
            PrintLine();
            PrintRow("", "", "", "", "", "", "", "", "", "", "", "");
            PrintLine();
            PrintRow("", "", "", "", $"{Brokenf_X}", $"{Dice1}", $"{Dice2}", $"{Brokenf_Y}", $"{Turn}", "", "", "");
            PrintLine();
            PrintRow("", "", "", "", "", "", "", "", "", "", "", "");
            PrintLine();
            PrintRow(down);
            PrintLine();
            
        }
        
        
        public static void Print2File(  ref string[] up,  ref string[] down, int Brokenf_X, int Brokenf_Y, int Dice1, int Dice2,ref char Turn)
        {
            Clearfile();
            using (StreamWriter writer = new StreamWriter("Table.dat", true))
            {
                writer.WriteLine();
                writer.Close();
            }
            PrintRow_file("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L");
            
            PrintLine_file();
            PrintRow_file(up);
            PrintLine_file();
            PrintRow_file("", "", "", "", "", "", "", "", "", "", "", "");
            PrintLine_file();
            PrintRow_file("", "", "", "", $"{Brokenf_X}", $"{Dice1}", $"{Dice2}", $"{Brokenf_Y}", $"{Turn}", "", "", "");
            PrintLine_file();
            PrintRow_file("", "", "", "", "", "", "", "", "", "", "", "");
            PrintLine_file();
            PrintRow_file(down);
            PrintLine_file();
            /*result = Stones(xPlayer, yPlayer); //z.Select(x=>x.ToString()).ToArray();
            up = result.Skip(0).Take(12).ToArray(); //an array for the upper side of the table
            down = result.Skip(12).Take(24).ToArray();
            down = Enumerable.Reverse(down).ToArray(); //an array for the lower side of the table.*/
            
        }
        
        
        //Check who is gonna start 
        public static char checkFirst(ref char Turn,ref int d1,ref int d2)
        {
            if (d1 > d2)
            {
                Turn = 'X';
                Console.WriteLine("Player X Turn ..");
            }
            else if (d2 > d1)
            {
                Turn = 'Y';
                Console.WriteLine("Player Y Turn ..");
            }
            else
            {
                d1 = IntUtil.Random(1,7);
                d2 = IntUtil.Random(1,7);
                Console.WriteLine("Trying again");
                checkFirst(ref Turn, ref d1, ref d2);
            }

            return Turn;
        }



        


            public static string[] Stones( int[] x,int []y)
        {
            string[] res = new string[x.Length];
            //string[] xx = new string[x.Length];
            
            for (int i = 0; i <= 23; i++)
            {
                for (int j = 23; j >= 0; j--)
                {
                    if (x[i] > 0)
                    {
                        res[i] = x[i].ToString();
                        res[i] = res[i] + "X";
                    }
                    if (y[j] > 0)
                    {
                        res[j] = y[j].ToString();
                        res[j] = res[j] + "Y";
                    }
                    else
                    {
                        res[i] = "";
                    }
                }


            }
            return res;

        }
        
        
        
        public static int[] ArrTotal(ref int[] a, ref int[] b)
        {
            int[] result = new int[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == 0)
                    result[i] = b[i];
                else if (b[i] == 0)
                    result[i] = a[i];
            }
            return result;
        }


        public static int y_move(int a)
          {
            a= Math.Abs(a - 24);
            return a;
        }
        
   

        public static void PrintStoneX(string [] player)
        {
            
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i] != "0")
                {

                    player[i] = player[i] + "X";
                }
                
            }

        }
        static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        public static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }



        static void Clearfile()
        {
            using (StreamWriter writer = new StreamWriter("Table.dat"))
            {
                writer.Close();
            }
            
        }
        static void PrintLine_file()
        {
            using (StreamWriter writer = new StreamWriter("Table.dat", true))
            {

                
                writer.WriteLine(new string('-', tableWidth));
                writer.Close();
            }
        }

        static void PrintRow_file(params string[] columns)
        {
            using (StreamWriter writer = new StreamWriter("Table.dat", true))
            {
                int width = (tableWidth - columns.Length) / columns.Length;
                string row = "|";

                foreach (string column in columns)
                {
                    row += AlignCentre(column, width) + "|";
                }

                writer.WriteLine(row);
                writer.Close();
            }
        }




        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}