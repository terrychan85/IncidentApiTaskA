using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncidentApiTaskA.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentsController : ControllerBase
    {
        private static readonly List<Incident> _incidents = new List<Incident>();
        private static int _nextId = 1;
        private static readonly string[] AllowedSeverities = new[] { "Low", "Medium", "High" };

        [HttpPost]
        public IActionResult CreateIncident([FromBody] IncidentCreateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Description) || string.IsNullOrWhiteSpace(request.Severity))
            {
                return BadRequest(new { error = "Title, Description, and Severity are required." });
            }

            if (!AllowedSeverities.Contains(request.Severity, StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Severity must be one of: Low, Medium, High." });
            }

            var now = DateTime.UtcNow;
            var duplicate = _incidents.Any(i =>
                i.Title.Equals(request.Title, StringComparison.OrdinalIgnoreCase) &&
                i.Description.Equals(request.Description, StringComparison.OrdinalIgnoreCase) &&
                (now - i.DateReported).TotalHours <= 24
            );
            if (duplicate)
            {
                return Conflict(new { error = "Duplicate incident detected within 24 hours." });
            }

            var incident = new Incident
            {
                Id = _nextId++,
                Title = request.Title,
                Description = request.Description,
                Severity = request.Severity,
                DateReported = now
            };
            _incidents.Add(incident);
            return CreatedAtAction(nameof(GetIncident), new { id = incident.Id }, new { id = incident.Id, message = "Incident created successfully." });
        }

        [HttpGet("{id}")]
        public IActionResult GetIncident(int id)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            if (incident == null) return NotFound();
            return Ok(incident);
        }
    }

    public class IncidentCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
    }

    public class Incident
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public DateTime DateReported { get; set; }
    }
} 