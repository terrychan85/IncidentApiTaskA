IncidentApiTaskA_HostAddress: http://localhost:5148

---

A. Successful Creation
Request:
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json

{
  "title": "Server Down",
  "description": "Main server is not responding.",
  "severity": "High"
}

Response:
201 Created
Content-Type: application/json
{
  "id": 1,
  "message": "Incident created successfully."
}

---

B. Duplicate Incident (within 24 hours)
Request:
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{
  "title": "Server Down",
  "description": "Main server is not responding.",
  "severity": "High"
}

Response:
409 Conflict
Content-Type: application/json
{
  "error": "Duplicate incident detected within 24 hours."
}

---

C. Invalid Severity
Request:
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{
  "title": "Network Issue",
  "description": "WiFi is unstable.",
  "severity": "Critical"
}

Response:
400 Bad Request
Content-Type: application/json
{
  "error": "Severity must be one of: Low, Medium, High."
}

---

D. Missing Required Field
Request:
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{
  "title": "",
  "description": "No title provided.",
  "severity": "Low"
}

Response:
400 Bad Request
Content-Type: application/json
{
  "error": "Title, Description, and Severity are required."
}

---

E. Invalid JSON
Request:
POST {IncidentApiTaskA_HostAddress}/api/incidents
Content-Type: application/json
{ not valid json }

Response:
400 Bad Request
Content-Type: application/problem+json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "traceId": "..."
} 
