using Cd.Cms.Application.Contracts.Repositories;
using Cd.Cms.Application.DTOs.Users;
using Cd.Cms.Application.Services;
using Cd.Cms.Tests.Helpers;
using Moq;

namespace Cd.Cms.Tests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly UserService _svc;

        public UserServiceTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _svc = new UserService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser_WhenFound()
        {
            var expected = MockHelper.SampleUser(id: 1);
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expected);

            var result = await _svc.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((UserDto?)null);
            var result = await _svc.GetByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_Success_WhenRequestIsValid()
        {
            var req = new CreateUserRequest { Name = "John", Email = "john@cms.com", Username = "john", Password = "SecureP@ss1", Role = "Agent" };
            var expected = MockHelper.SampleUser();
            _repoMock.Setup(r => r.CreateAsync(It.IsAny<CreateUserRequest>(), 1)).ReturnsAsync(expected);

            var result = await _svc.CreateAsync(req, 1);

            Assert.NotNull(result);
            _repoMock.Verify(r => r.CreateAsync(It.IsAny<CreateUserRequest>(), 1), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenNameIsEmpty()
        {
            var req = new CreateUserRequest { Name = "", Email = "john@cms.com", Username = "john", Password = "SecureP@ss1", Role = "Agent" };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
        }

        [Fact]
        public async Task CreateAsync_ThrowsArgumentException_WhenPasswordTooShort()
        {
            var req = new CreateUserRequest { Name = "John", Email = "john@cms.com", Username = "john", Password = "short", Role = "Agent" };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.CreateAsync(req, 1));
        }

        [Fact]
        public async Task ChangePasswordAsync_ThrowsArgumentException_WhenPasswordsMismatch()
        {
            var req = new ChangePasswordRequest { CurrentPassword = "old", NewPassword = "NewP@ss123", ConfirmPassword = "Different" };
            await Assert.ThrowsAsync<ArgumentException>(() => _svc.ChangePasswordAsync(1, req, 1));
        }

        [Fact]
        public async Task GetAgentsAsync_ReturnsAgentList()
        {
            var agents = new List<UserDto> { MockHelper.SampleUser(1, "Agent"), MockHelper.SampleUser(2, "Agent") };
            _repoMock.Setup(r => r.GetAgentsAsync()).ReturnsAsync(agents);

            var result = await _svc.GetAgentsAsync();

            Assert.Equal(2, result.Count);
        }
    }
}
