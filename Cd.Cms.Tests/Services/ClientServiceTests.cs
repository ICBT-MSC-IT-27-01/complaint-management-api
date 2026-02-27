using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Clients;
using Cd.Cms.Application.Services;
using Moq;

namespace Cd.Cms.Tests.Services
{
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _repoMock;
        private readonly ClientService _svc;

        public ClientServiceTests()
        {
            _repoMock = new Mock<IClientRepository>();
            _svc = new ClientService(_repoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenCompanyNameIsEmpty()
        {
            var req = new CreateClientRequest { CompanyName = "", PrimaryEmail = "client@co.com" };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenEmailIsEmpty()
        {
            var req = new CreateClientRequest { CompanyName = "Acme Corp", PrimaryEmail = "" };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((ClientDto?)null);
            var result = await _svc.GetByIdAsync(999);
            Assert.Null(result);
        }
    }
}
