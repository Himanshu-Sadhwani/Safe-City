using System;
using SafeCity.Domain.Entity;

namespace SafeCity.Repository
{
    public interface IDispatchRepository
    {
        Task AddAsync(Dispatch dispatch);
        Task<List<Dispatch>> GetByIncidentIdAsync(int incidentId);
        Task<Dispatch> GetByIdAsync(int dispatchId);
        Task UpdateAsync(int id,Dispatch dispatch);
    }
}