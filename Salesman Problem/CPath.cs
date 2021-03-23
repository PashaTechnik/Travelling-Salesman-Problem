using System;

namespace Salesman_Problem
{
    class CPath
    {

        double[,] distance;

        public int[] Path;

        public CPath(CCities map)
        {

            distance = new double[map.Coordinate.Length, map.Coordinate.Length];

            for (int j = 0; j < map.Coordinate.Length; j++)
            {
                distance[j, j] = 0;

                for (int i = 0; i < map.Coordinate.Length; i++)
                {
                    double value = Math.Sqrt(Math.Pow(map.Coordinate[i].X - map.Coordinate[j].X, 2) +
                                             Math.Pow(map.Coordinate[i].Y - map.Coordinate[j].Y, 2));
                    distance[i, j] = distance[j, i] = value;
                }
            }

            //создаем начальный путь
            //массив на 1 больше кол-ва городов, а первый и последний индексы равны 0 - это сделано для того чтобы "замкнуть" путь
            Path = new int[map.Coordinate.Length + 1];
            for (int i = 0; i < map.Coordinate.Length; i++)
            {
                Path[i] = i;
            }
            Path[map.Coordinate.Length] = 0;
        }

        //метод, реулизующий алгоритм поиска оптимального пути
        public void FindBestPath()
        {
            Random random = new Random();

            int iteration = -1;
            double proba;
            double alpha = 0.999;
            double temperature = 400.0;
            double epsilon = 0.001;
            double delta;

            while (temperature > epsilon)
            {
                iteration++;
                int p1 = 0, p2 = 0;
                while (p1 == p2)
                {
                    p1 = random.Next(1, Path.Length - 1);
                    p2 = random.Next(1, Path.Length - 1);
                }

                double sum1 = distance[Path[p1 - 1], Path[p1]] + distance[Path[p1], Path[p1 + 1]] +
                              distance[Path[p2 - 1], Path[p2]] + distance[Path[p2], Path[p2 + 1]];
                double sum2 = distance[Path[p1 - 1], Path[p2]] + distance[Path[p2], Path[p1 + 1]] +
                              distance[Path[p2 - 1], Path[p1]] + distance[Path[p1], Path[p2 + 1]];

                if (sum2 < sum1)
                {
                    int temp = Path[p1];
                    Path[p1] = Path[p2];
                    Path[p2] = temp;
                }
                else
                {
                    delta = sum2 - sum1;
                    if (ProbabilityCalc(Math.Exp(-delta / temperature)))
                    {
                        int temp = Path[p1];
                        Path[p1] = Path[p2];
                        Path[p2] = temp;
                    }
                }

                temperature *= alpha;

                if (iteration % 400 == 0)
                    Console.WriteLine(PathLength());

            }





            //     for (int fails = 0, F = Path.Length * Path.Length; fails < F; )
            //     {
            //         int p1 = 0, p2 = 0;
            //         while (p1 == p2)
            //         {
            //             p1 = random.Next(1, Path.Length - 1);
            //             p2 = random.Next(1, Path.Length - 1);
            //         }
            //         
            //         //проверка расстояний
            //         double sum1 = distance[Path[p1 - 1], Path[p1]] + distance[Path[p1], Path[p1 + 1]] +
            //                       distance[Path[p2 - 1], Path[p2]] + distance[Path[p2], Path[p2 + 1]];
            //         double sum2 = distance[Path[p1 - 1], Path[p2]] + distance[Path[p2], Path[p1 + 1]] +
            //                       distance[Path[p2 - 1], Path[p1]] + distance[Path[p1], Path[p2 + 1]];
            //
            //         if (sum2 < sum1)
            //         {
            //             int temp = Path[p1];
            //             Path[p1] = Path[p2];
            //             Path[p2] = temp;
            //         }
            //         else
            //         {
            //             fails++;
            //         }
            //     }
            // 
        }

        //возвращает длину пути
        public double PathLength()
        {
            double pathSum = 0;
            for (int i = 0; i < Path.Length - 1; i++)
            {
                pathSum += distance[Path[i], Path[i + 1]];
            }
            return pathSum;
        }

        public bool ProbabilityCalc(double prob)
        {
            Random random = new Random();
            int p = random.Next(1, 100);
            if (p < prob * 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}