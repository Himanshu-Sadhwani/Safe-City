using System;
using SafeCity.Domain.Entity;
using SafeCity.DTOs.Incidents;

namespace SafeCity.Repository
{
    public interface IIncidentRepository
    {
        public Task<Incident?> GetByIdAsync(int incidentId);
        public Task UpdateAsync(Incident incident);
        public Task SubmitIncident(IncidentCreateRequest request);
    }
}