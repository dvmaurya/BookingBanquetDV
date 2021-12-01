using CommonServices.Model.City;
using System.Collections.Generic;

namespace BookingServices.Repository.Interface
{
    public interface IStateRepository
    {
        bool AddOrUpdateState(List<State> state);
    }
}
