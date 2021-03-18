using System;
using System.Collections.Generic;
using System.Linq;

namespace Salesman_Problem
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            CCities Berlin = new CCities(52, "/Users/admin/Desktop/Travelling Salesman Problem/berlin52.txt");
            CCities Eil76 = new CCities(76, "/Users/admin/Desktop/Travelling Salesman Problem/eil76.txt");
            // int i = 0;
            // foreach (var point in Berlin.Coordinate)
            // {
            //     Console.Write(i + " ");
            //     Console.WriteLine(point.X + " " + point.Y);
            //     i++;
            // }
            CPath path = new CPath(Berlin);
            CPath path2 = new CPath(Eil76);
            //path.FindBestPath();

            foreach (var city in path.Path)
            {
                Console.Write(city);
                Console.Write("->");
            }

            Console.WriteLine();
            while (path2.PathLength() > 604)
            {
                path2.FindBestPath();
                //Console.WriteLine(path.PathLength());
            }

            Console.WriteLine(path2.PathLength());

        }
    }

}