using System;
using System.Drawing;

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

            //создаем более узкие границы, чем сам pictureBox, чтобы города не лежали с самого краю
            //так просто визуально приятнее выглядит
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
            
            
        }
        


    }
}