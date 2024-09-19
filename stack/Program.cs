using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Cryptography;
using System.Linq;


namespace stack
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //task2 
            Stack s1 = new Stack();
            s1.Push("H");
            s1.Push("e");
            s1.Push("l");
            s1.Push("l");
            s1.Push("o");
            Console.WriteLine("After reversing \n");
            PrintStack(s1);

            Stack rev = new Stack(s1.ToArray());
            Console.WriteLine("\nPrinting revers\n");
            PrintStack(rev);



            ///task4:
            //Console.WriteLine("enter num from 1 to ;
            Stack ss1 = new Stack();
            ss1.Push(2);
            ss1.Push(10);
            ss1.Push(7);

            Console.WriteLine("After push:");
            PrintStack(ss1);

            int maxNum = ss1.Max();
            Console.WriteLine("The max value is: " + maxNum);










            //    foreach (Stack s in ss1)
            //    {
            //        if (s >= 9)
            //        {
            //            Console.WriteLine("this number is the max in stack");
            //        }
            //        else
            //        {
            //            Console.WriteLine("this is not a max num")
            //        }
            //    }



            //}
            //////Queue:
            Queue Q1 = new Queue();

            // Enqueue operation (add items to the queue)
            Q1.Enqueue(1);
            Q1.Enqueue(2);
            Q1.Enqueue(3);
            Q1.Enqueue(4);
            Q1.Enqueue(5);
            Q1.Enqueue(12.5);
            Q1.Enqueue(true);

            Console.WriteLine("After enqueuing items:\n");
            PrintQueue(Q1);


            //*************************************************
            static void PrintStack(Stack stack)
            {
                foreach (var V in stack)
                { Console.WriteLine(V); }
            }


            static void PrintQueue(Queue Q1)
            {
                foreach (var item in Q1)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}