using System;
using SafeCity.Domain.Entity;

namespace SafeCity.Repository
{
    public interface IDispatchRepository
    {
        Task AddAsync(Dispatch dispatch);
        Task<List<Dispatch>> GetByIncidentIdAsync(int incidentId);
    }
}