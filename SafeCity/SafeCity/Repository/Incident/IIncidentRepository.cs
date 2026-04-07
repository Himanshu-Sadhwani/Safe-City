using System;
using SafeCity.Domain.Entity;

namespace SafeCity.Repository
{
    public interface IIncidentRepository
    {
        Task<Incident?> GetByIdAsync(int incidentId);
        Task UpdateAsync(Incident incident);
    }
}