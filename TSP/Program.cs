using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace TSP
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //SolveSalesmanProblem("gr96",96);
            //SolveSalesmanProblem("berlin52",52);
            //SolveSalesmanProblem("pr76", 76);
            //SolveSalesmanProblem("eil76", 76);
            SolveSalesmanProblem("eil101", 101);
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        public static void SolveSalesmanProblem(string ProblemName, int N)
        {
            string Path = "/Users/admin/Desktop/Travelling Salesman Problem/" + ProblemName + ".txt";
            var stops = Enumerable.Range(1, N)
                .Select(i => new Stop(new City(i,Path)))
                .NearestNeighbors()
                .ToList();
            foreach (var stop in stops)
            {
                Console.WriteLine(stop.City.CityName + " " + stop.City.X + " " + stop.City.Y);
            }
            //create next pointers between them
            stops.Connect(true);

            //wrap in a tour object
            Tour startingTour = new Tour(stops);

            //the actual algorithm
            while (true)
            {
                Console.WriteLine(startingTour);
                var newTour = startingTour.GenerateMutations()
                    .MinBy(tour => tour.Cost());
                if (newTour.Cost() < startingTour.Cost()) startingTour = newTour;
                else break;
            }
        }
        
        
        


        private class City
        {
            private static Random rand = new Random();
            List<string[]> citiesList = new List<string[]>();
            public static int Pointer = 0;




            public City(int cityName, string Path)
            {
                using (StreamReader sr = new StreamReader(Path, System.Text.Encoding.Default))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] point = line.Split(" ");
                        citiesList.Add(point);
                    }
                    citiesList.ToArray();
                }
                
                
                X = Double.Parse(citiesList[Pointer][0]);
                Y =  Double.Parse(citiesList[Pointer][1]);
                CityName = cityName;
                Pointer++;
            }


            public double X { get; set; }

            public double Y { get; set; }

            public int CityName { get; private set; }
        }


        private class Stop
        {
            public Stop(City city)
            {
                City = city;
            }


            public Stop Next { get; set; }

            public City City { get; set; }


            public Stop Clone()
            {
                return new Stop(City);
            }


            public static double Distance(Stop first, Stop other)
            {
                return Math.Sqrt(
                    Math.Pow(first.City.X - other.City.X, 2) +
                    Math.Pow(first.City.Y - other.City.Y, 2));
            }


            //list of nodes, including this one, that we can get to
            public IEnumerable<Stop> CanGetTo()
            {
                var current = this;
                while (true)
                {
                    yield return current;
                    current = current.Next;
                    if (current == this) break;
                }
            }


            public override bool Equals(object obj)
            {
                return City == ((Stop)obj).City;
            }


            public override int GetHashCode()
            {
                return City.GetHashCode();
            }


            public override string ToString()
            {
                return City.CityName.ToString();
            }
        }


        private class Tour
        {
            public Tour(IEnumerable<Stop> stops)
            {
                Anchor = stops.First();
            }


            //the set of tours we can make with 2-opt out of this one
            public IEnumerable<Tour> GenerateMutations()
            {
                for (Stop stop = Anchor; stop.Next != Anchor; stop = stop.Next)
                {
                    //skip the next one, since you can't swap with that
                    Stop current = stop.Next.Next;
                    while (current != Anchor)
                    {
                        yield return CloneWithSwap(stop.City, current.City);
                        current = current.Next;
                    }
                }
            }


            public Stop Anchor { get; set; }


            public Tour CloneWithSwap(City firstCity, City secondCity)
            {
                Stop firstFrom = null, secondFrom = null;
                var stops = UnconnectedClones();
                stops.Connect(true);

                foreach (Stop stop in stops)
                {
                    if (stop.City == firstCity) firstFrom = stop;

                    if (stop.City == secondCity) secondFrom = stop;
                }

                //the swap part
                var firstTo = firstFrom.Next;
                var secondTo = secondFrom.Next;

                //reverse all of the links between the swaps
                firstTo.CanGetTo()
                       .TakeWhile(stop => stop != secondTo)
                       .Reverse()
                       .Connect(false);

                firstTo.Next = secondTo;
                firstFrom.Next = secondFrom;

                var tour = new Tour(stops);
                return tour;
            }


            public IList<Stop> UnconnectedClones()
            {
                return Cycle().Select(stop => stop.Clone()).ToList();
            }


            public double Cost()
            {
                return Cycle().Aggregate(
                    0.0,
                    (sum, stop) =>
                    sum + Stop.Distance(stop, stop.Next));
            }


            private IEnumerable<Stop> Cycle()
            {
                return Anchor.CanGetTo();
            }


            public override string ToString()
            {

                var test = Cycle().Select(stop => stop.ToString()).ToArray();


                for (int i = 0; i < test.Length; i++)
                {
                    test[i] = (int.Parse(test[i]) - 1).ToString();
                }
                string path = String.Join(
                    " ",
                    test);
                

                return String.Format("Cost: {0}, Path:{1}", Cost(), path);
            }

        }


        //take an ordered list of nodes and set their next properties
        private static void Connect(this IEnumerable<Stop> stops, bool loop)
        {
            Stop prev = null, first = null;
            foreach (var stop in stops)
            {
                if (first == null) first = stop;
                if (prev != null) prev.Next = stop;
                prev = stop;
            }

            if (loop)
            {
                prev.Next = first;
            }
        }


        //T with the smallest func(T)
        private static T MinBy<T, TComparable>(
            this IEnumerable<T> xs,
            Func<T, TComparable> func)
            where TComparable : IComparable<TComparable>
        {
            return xs.DefaultIfEmpty().Aggregate(
                (maxSoFar, elem) =>
                func(elem).CompareTo(func(maxSoFar)) > 0 ? maxSoFar : elem);
        }


        //return an ordered nearest neighbor set
        private static IEnumerable<Stop> NearestNeighbors(this IEnumerable<Stop> stops)
        {
            var stopsLeft = stops.ToList();
            for (var stop = stopsLeft.First();
                 stop != null;
                 stop = stopsLeft.MinBy(s => Stop.Distance(stop, s)))
            {
                stopsLeft.Remove(stop);
                yield return stop;
            }
        }
    }
}