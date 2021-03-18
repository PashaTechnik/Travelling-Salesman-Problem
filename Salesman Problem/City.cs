using System;
using System.IO;

namespace Salesman_Problem
{
    public class City
    {
        private static Random rand = new Random();


        public City(int cityName)
        {
            X = rand.NextDouble() * 100;
            Y = rand.NextDouble() * 100;
            CityName = cityName;
        }


        public double X { get; private set; }

        public double Y { get; private set; }

        public int CityName { get; private set; }
    }
}