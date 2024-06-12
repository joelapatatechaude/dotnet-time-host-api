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
        var localTime = DateTime.Now;
        var timeEntry = new TimeEntry
        {
            DateTime = localTime,
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

    // Catch-all route for api/time to list allowed methods with clickable links
    [HttpGet]
    public ContentResult Get()
    {
        var responseHtml = @"
        <html>
            <body>
                <h2>Allowed methods are:</h2>
                <ul>
                    <li><a href='/api/time/time'>GET /api/time/time</a></li>
                    <li><a href='/api/time/history'>GET /api/time/history</a></li>
                </ul>
            </body>
        </html>";

        return new ContentResult
        {
            ContentType = "text/html",
            StatusCode = 200,
            Content = responseHtml
        };
    }
}

