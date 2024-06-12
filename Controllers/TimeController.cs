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
        var timeEntry = new TimeEntry
        {
            DateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time")),
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
