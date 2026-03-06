using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/health")]
    public sealed class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Check()
        {
            var data = new
            {
                status = "Healthy",
                timestampUtc = DateTime.UtcNow
            };

            return Ok(ApiResponse<object>.Success("Service is healthy.", data));
        }

        [HttpGet("ui")]
        [Produces("text/html")]
        public ContentResult Ui()
        {
            const string html = """
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width,initial-scale=1">
  <title>CMS API Health Check</title>
  <style>
    :root { color-scheme: light; }
    body {
      margin: 0;
      font-family: "Segoe UI", Arial, sans-serif;
      background: linear-gradient(135deg, #f6fbff, #edf7ef);
      color: #1f2937;
      min-height: 100vh;
      display: grid;
      place-items: center;
      padding: 16px;
    }
    .card {
      width: min(680px, 100%);
      background: #fff;
      border: 1px solid #dbe5f0;
      border-radius: 14px;
      box-shadow: 0 12px 28px rgba(0,0,0,.08);
      padding: 24px;
    }
    h1 { margin: 0 0 8px; font-size: 1.35rem; }
    p { margin: 0 0 14px; color: #4b5563; }
    .status {
      border-radius: 10px;
      padding: 10px 12px;
      font-weight: 700;
      display: inline-block;
      margin-bottom: 12px;
    }
    .ok { background: #e8f8ec; color: #166534; border: 1px solid #bbf7d0; }
    .bad { background: #fef2f2; color: #b91c1c; border: 1px solid #fecaca; }
    .pending { background: #eff6ff; color: #1d4ed8; border: 1px solid #bfdbfe; }
    pre {
      margin: 10px 0 0;
      background: #0f172a;
      color: #e2e8f0;
      border-radius: 10px;
      padding: 14px;
      overflow: auto;
      font-size: .87rem;
      line-height: 1.45;
    }
    button {
      border: 0;
      border-radius: 10px;
      background: #0b5ed7;
      color: #fff;
      padding: 10px 14px;
      font-weight: 600;
      cursor: pointer;
      margin-right: 8px;
    }
    button:hover { background: #0a53be; }
    .meta { margin-top: 10px; font-size: .9rem; color: #6b7280; }
  </style>
</head>
<body>
  <main class="card">
    <h1>Complaint Management API Health Check</h1>
    <p>Use this page to quickly verify that <code>/api/v1/health</code> is responding.</p>
    <div id="status" class="status pending">Checking...</div>
    <div>
      <button id="recheck" type="button">Check Again</button>
    </div>
    <pre id="payload">Waiting for response...</pre>
    <div id="meta" class="meta"></div>
  </main>

  <script>
    const statusEl = document.getElementById('status');
    const payloadEl = document.getElementById('payload');
    const metaEl = document.getElementById('meta');
    const button = document.getElementById('recheck');

    function updateStatus(kind, text) {
      statusEl.className = 'status ' + kind;
      statusEl.textContent = text;
    }

    async function checkHealth() {
      updateStatus('pending', 'Checking...');
      payloadEl.textContent = 'Loading...';
      metaEl.textContent = '';
      const started = performance.now();

      try {
        const response = await fetch('/api/v1/health', { cache: 'no-store' });
        const elapsedMs = Math.round(performance.now() - started);
        const text = await response.text();

        let data = text;
        try {
          data = JSON.parse(text);
          payloadEl.textContent = JSON.stringify(data, null, 2);
        } catch {
          payloadEl.textContent = text;
        }

        if (response.ok) {
          updateStatus('ok', 'API is healthy');
        } else {
          updateStatus('bad', 'API returned error status');
        }

        metaEl.textContent = 'HTTP ' + response.status + ' in ' + elapsedMs + ' ms';
      } catch (error) {
        updateStatus('bad', 'Failed to reach API');
        payloadEl.textContent = String(error);
        metaEl.textContent = 'Request failed';
      }
    }

    button.addEventListener('click', checkHealth);
    checkHealth();
  </script>
</body>
</html>
""";

            return Content(html, "text/html");
        }
    }
}
