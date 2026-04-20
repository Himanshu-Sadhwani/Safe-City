using System;
using SafeCity.DTOs;
namespace SafeCity.Services.Dispatch;

public interface IDispatchService
{
    Task<DispatchResponseDto> AssignUnitAsync(DispatchRequestDto request);
    Task UpdateDispatchStatusAsync(int id,DispatchUpdateByStatusRequestDto request);
}
