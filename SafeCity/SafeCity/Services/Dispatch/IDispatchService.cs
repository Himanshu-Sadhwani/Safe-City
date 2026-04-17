using System;
using SafeCity.Domain.Enum;
using SafeCity.DTOs;
using SafeCity.DTOs.Dispatch;
namespace SafeCity.Services.Dispatch;

public interface IDispatchService
{
    Task<DispatchResponseDto> AssignUnitAsync(int DispatcherID,DispatchRequestDto request);
    Task UpdateDispatchStatusAsync(int id,DispatchUpdateByStatusRequestDto request);
    Task<List<GetResponseDto>> ViewDispatch(int? incidentId,bool IsAdmin, int? resourceId, int? dispatcherId, DispatchStatusOption? status, DateTime? date,string? sortOrder);
}
