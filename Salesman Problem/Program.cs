using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Salesman_Problem
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            
            SolveSalesmanProblem("eil76",76,800);
            
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

        }

        static void SolveSalesmanProblem(string Path, int N, int length)
        {
            
            CCities Berlin = new CCities(N, $"/Users/admin/Desktop/Travelling Salesman Problem/{Path}.txt");
            CPath path = new CPath(Berlin);
            
            foreach (var city in path.Path)
            {
                Console.Write(city);
                Console.Write(" ");
            }

            Console.WriteLine();
            while (path.PathLength() > length)
            {
                path.FindBestPath();
                //Console.WriteLine(path.PathLength());
            }

            foreach (var city in path.Path)
            {
                Console.Write(city);
                Console.Write(" ");
            }
            Console.WriteLine(path.PathLength());
        }
    }
}