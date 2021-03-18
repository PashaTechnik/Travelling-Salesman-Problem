using System;
using System.Drawing;
using System.IO;

namespace Salesman_Problem
{
    class CCities
    {
        //массив коррдинат городов
        public Point[] Coordinate;
        
        public CCities(int N, int maxValue) 
        {
            Random random = new Random();

            Coordinate = new Point[N];
            
            int minBorder = (int)(maxValue * 0.05);
            int maxBorder = (int)(maxValue * 0.95);

            for (int i = 0; i < N; i++)
            {
                Coordinate[i] = new Point(random.Next(minBorder, maxBorder),
                    random.Next(minBorder, maxBorder));
            }
        }

        public CCities(int N, string Path)
        {
            int i = 0;
            Coordinate = new Point[N];
            using (StreamReader sr = new StreamReader(Path, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] point = line.Split(" ");
                    Coordinate[i].X = (int)double.Parse(point[0]);
                    Coordinate[i].Y = (int)double.Parse(point[1]);
                    i++;
                }
            }
        }
        


    }
}