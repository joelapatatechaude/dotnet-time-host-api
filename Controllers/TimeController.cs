using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TimeApi.Data;
using TimeApi.Models;

[Route("api/[controller]")]
[ApiController]
public class TimeController : ControllerBase
{
    private readonly TimeContext _context;

    public TimeController(TimeContext context)
    {
        _context = context;
    }

    [HttpGet("time")]
    public async Task<IActionResult> GetTime()
    {
        DateTime sydneyTime;
        try
        {
            sydneyTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Australia/Sydney"));
        }
        catch (TimeZoneNotFoundException)
        {
            // Handle the case where the time zone ID is not found
            return StatusCode(500, "The time zone ID 'Australia/Sydney' was not found on the local computer.");
        }
        catch (InvalidTimeZoneException)
        {
            // Handle the case where the time zone data is corrupt
            return StatusCode(500, "The time zone data is invalid or corrupt.");
        }

        var timeEntry = new TimeEntry
        {
            DateTime = sydneyTime,
            HostName = Dns.GetHostName()
        };

        _context.TimeEntries.Add(timeEntry);
        await _context.SaveChangesAsync();

        return Ok(timeEntry);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory()
    {
        var history = await _context.TimeEntries
            .OrderByDescending(te => te.Id)
            .Take(100)
            .ToListAsync();

        return Ok(history);
    }
}
