using Cd.Cms.Application.Contracts.Services;
using Cd.Cms.Application.DTOs.Reports;
using Cd.Cms.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cd.Cms.Api.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _svc;
        public ReportsController(IReportService svc) => _svc = svc;

        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var result = await _svc.GetDashboardAsync(GetActorUserId(), GetActorRole());
            return Ok(ApiResponse<object>.Success("Dashboard loaded.", result));
        }

        [HttpGet("complaints-summary")]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> ComplaintSummary([FromQuery] ReportFilterRequest req)
        {
            var result = await _svc.GetComplaintSummaryAsync(req ?? new());
            return Ok(ApiResponse<object>.Success("Summary loaded.", result));
        }

        private long GetActorUserId() => long.Parse(User.FindFirst("uid")?.Value ?? "0");
        private string GetActorRole() => User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }
}
