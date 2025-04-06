using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asio_Home_Assignment
{
    public interface ITripCalculation
    {
        Tuple<int, List<Trip>> CalculateTrips(List<Bag> bagWeights);
    }
}
