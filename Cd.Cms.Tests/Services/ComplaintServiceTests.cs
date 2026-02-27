using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Complaints;
using Cd.Cms.Application.Services;
using Cd.Cms.Tests.Helpers;
using Moq;

namespace Cd.Cms.Tests.Services
{
    public class ComplaintServiceTests
    {
        private readonly Mock<IComplaintRepository> _repoMock;
        private readonly ComplaintService _svc;

        public ComplaintServiceTests()
        {
            _repoMock = new Mock<IComplaintRepository>();
            _svc = new ComplaintService(_repoMock.Object);
        }

        // ── GetById ───────────────────────────────────────────────────────────

        [Fact]
        public async Task GetByIdAsync_ReturnsComplaint_WhenFound()
        {
            var expected = MockHelper.SampleComplaint(id: 5);
            _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(expected);

            var result = await _svc.GetByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal(5, result!.Id);
            Assert.Equal("CMP-202600001", result.ComplaintNumber);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ComplaintDto?)null);

            var result = await _svc.GetByIdAsync(999);

            Assert.Null(result);
        }

        // ── Create ────────────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_Success_WhenRequestIsValid()
        {
            var req = MockHelper.SampleCreateRequest();
            var expected = MockHelper.SampleComplaint();
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<CreateComplaintRequest>(), 1)).ReturnsAsync(expected);

            var result = await _svc.CreateAsync(req, actorUserId: 1);

            Assert.NotNull(result);
            Assert.Equal("CMP-202600001", result.ComplaintNumber);
            _repoMock.Verify(r => r.CreateAsync(It.IsAny<CreateComplaintRequest>(), 1), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenSubjectIsEmpty()
        {
            var req = MockHelper.SampleCreateRequest();
            req.Subject = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
            _repoMock.Verify(r => r.CreateAsync(It.IsAny<CreateComplaintRequest>(), It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenDescriptionIsEmpty()
        {
            var req = MockHelper.SampleCreateRequest();
            req.Description = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenPriorityIsEmpty()
        {
            var req = MockHelper.SampleCreateRequest();
            req.Priority = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
        }

        // ── Search ────────────────────────────────────────────────────────────

        [Fact]
        public async Task SearchAsync_ReturnsPagedResult()
        {
            var expected = MockHelper.SamplePagedComplaints(count: 3);
            _repoMock.Setup(r => r.SearchAsync(It.IsAny<ComplaintSearchRequest>())).ReturnsAsync(expected);

            var result = await _svc.SearchAsync(new ComplaintSearchRequest());

            Assert.NotNull(result);
            Assert.Equal(3, result.Items.Length);
            Assert.Equal(3, result.TotalCount);
        }

        // ── Assign ────────────────────────────────────────────────────────────

        [Fact]
        public async Task AssignAsync_Success_WhenRequestIsValid()
        {
            _repoMock.Setup(r => r.AssignAsync(1, It.IsAny<AssignComplaintRequest>(), 1)).Returns(Task.CompletedTask);

            await _svc.AssignAsync(1, new AssignComplaintRequest { AssignedToUserId = 2 }, 1);

            _repoMock.Verify(r => r.AssignAsync(1, It.IsAny<AssignComplaintRequest>(), 1), Times.Once);
        }

        [Fact]
        public async Task AssignAsync_ThrowsArgumentException_WhenUserIdIsZero()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _svc.AssignAsync(1, new AssignComplaintRequest { AssignedToUserId = 0 }, 1));
        }

        // ── Escalate ──────────────────────────────────────────────────────────

        [Fact]
        public async Task EscalateAsync_ThrowsArgumentException_WhenReasonIsEmpty()
        {
            var req = new EscalateComplaintRequest { Reason = "", EscalatedToUserId = 2 };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.EscalateAsync(1, req, 1));
        }

        [Fact]
        public async Task EscalateAsync_ThrowsArgumentException_WhenEscalatedToUserIdIsZero()
        {
            var req = new EscalateComplaintRequest { Reason = "SLA breach", EscalatedToUserId = 0 };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.EscalateAsync(1, req, 1));
        }

        // ── Resolve ───────────────────────────────────────────────────────────

        [Fact]
        public async Task ResolveAsync_Success_WhenRequestIsValid()
        {
            _repoMock.Setup(r => r.ResolveAsync(1, It.IsAny<ResolveComplaintRequest>(), 1)).Returns(Task.CompletedTask);

            await _svc.ResolveAsync(1, new ResolveComplaintRequest { ResolutionSummary = "Fixed the issue." }, 1);

            _repoMock.Verify(r => r.ResolveAsync(1, It.IsAny<ResolveComplaintRequest>(), 1), Times.Once);
        }

        [Fact]
        public async Task ResolveAsync_ThrowsArgumentException_WhenSummaryIsEmpty()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _svc.ResolveAsync(1, new ResolveComplaintRequest { ResolutionSummary = "" }, 1));
        }

        // ── Close & Delete ────────────────────────────────────────────────────

        [Fact]
        public async Task CloseAsync_CallsRepository()
        {
            _repoMock.Setup(r => r.CloseAsync(1, 1)).Returns(Task.CompletedTask);
            await _svc.CloseAsync(1, 1);
            _repoMock.Verify(r => r.CloseAsync(1, 1), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_CallsRepository()
        {
            _repoMock.Setup(r => r.DeleteAsync(1, 1)).Returns(Task.CompletedTask);
            await _svc.DeleteAsync(1, 1);
            _repoMock.Verify(r => r.DeleteAsync(1, 1), Times.Once);
        }
    }
}
