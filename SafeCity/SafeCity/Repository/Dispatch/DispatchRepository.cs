using System;
using SafeCity.Domain.Data;
using SafeCity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
namespace SafeCity.Repository;

public class DispatchRepository : IDispatchRepository
{
    private readonly SafeCityDbContext _context;
    public DispatchRepository(SafeCityDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Dispatch dispatch)
    {
        await _context.Dispatches.AddAsync(dispatch);
        await _context.SaveChangesAsync();
    }
    public async Task<bool> DispatchExistsForIncidentAsync(int incidentId)
    {
        return await _context.Dispatches
            .AnyAsync(d => d.IncidentID == incidentId);
    }
}

