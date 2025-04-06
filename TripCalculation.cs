using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Asio_Home_Assignment
{
    internal class TripCalculation : ITripCalculation
    {
        private const int MaxWeight = 3;
        public Tuple<int, List<Trip>> CalculateTrips(List<Bag> bagWeights)
        {
            bagWeights.Sort((x, y) => x.Weight.CompareTo(y.Weight));
            List<Trip> trips = new List<Trip>();

            while (bagWeights.Count > 0)
            {
                Trip trip = new Trip();  // Bags to be taken in this trip
                trip.Bags = new List<Bag>();
                double currentWeight = 0;

                //Greedily pick bags to add to the current trip until the weight exceeds 3 kg
                for (int i = bagWeights.Count - 1; i >= 0; i--)
                {
                    if (bagWeights[i].Weight + currentWeight <= MaxWeight)
                    {
                        trip.Bags.Add(new Bag() { Weight = bagWeights[i].Weight });
                        currentWeight += bagWeights[i].Weight;
                        bagWeights.RemoveAt(i);
                    }
                }
                trips.Add(trip);
            }
            return Tuple.Create(trips.Count, trips);
        }
    }
}




