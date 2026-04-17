using System;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;
using SafeCity.DTOs.Dispatch;

namespace SafeCity.Repository
{
    public interface IDispatchRepository
    {
        Task AddAsync(Dispatch dispatch);
        Task<List<Dispatch>> GetByIncidentIdAsync(int incidentId);
        Task<Dispatch> GetByIdAsync(int dispatchId);
        Task UpdateAsync(int id,Dispatch dispatch);
        Task<List<GetResponseDto>> ViewDispatch(int? incidentId, bool IsAdmin, int? resourceId, int? dispatcherId,  DispatchStatusOption? status, DateTime? date,string? sortOrder );

    }
}