using AutoMapper;
using Azure.Cosmos;
using Cosmos.Toggles.Application.Service;
using Cosmos.Toggles.Domain.DataTransferObject.Validators;
using Cosmos.Toggles.Domain.Entities.Repositories;
using Cosmos.Toggles.Domain.Service;
using Cosmos.Toggles.Infra.Cosmos.Db;
using FluentAssertions;
using FluentValidation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Cosmos.Toggles.Test.Application.Service
{
    public class EnvironmentAppServiceFixture
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<CosmosClient> _cosmosClientMock;
        private readonly Mock<CosmosContainer> _containerMock;

        private readonly Mock<IEnvironmentRepository> _environmentRepositoryMock;
        private readonly EnvironmentValidator _environmentValidator;

        public EnvironmentAppServiceFixture()
        {
            _mapper = new Mock<IMapper>();
            _cosmosClientMock = new Mock<CosmosClient>();
            _containerMock = new Mock<CosmosContainer>();
            _environmentRepositoryMock = new Mock<IEnvironmentRepository>();
            _environmentValidator = new EnvironmentValidator { };

            SetupCosmosClient();
        }

        private void SetupCosmosClient()
        {
            _cosmosClientMock.Setup(x => x.GetContainer(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(_containerMock.Object);
        }

        private Domain.Entities.Environment GetEntityEnvironment()
        {
            return new Domain.Entities.Environment
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Dev",
                Project = new Domain.Entities.Project
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Sample Project",
                    Description = "Sample project description"
                }
            };
        }

        private Domain.DataTransferObject.Environment GetDtoEnvironment()
        {
            var entity = GetEntityEnvironment();

            return new Domain.DataTransferObject.Environment
            {
                Id = entity.Id,
                Name = entity.Name,
                Project = new Domain.DataTransferObject.Project
                {
                    Id = entity.Project.Id,
                    Name = entity.Project.Name,
                    Description = entity.Project.Description
                }
            };
        }

        private CosmosToggleDataContext GetCosmosToggleDataContext(IEnvironmentRepository environmentRepository)
        {
            var cosmosToggleDataContext = new CosmosToggleDataContext(_cosmosClientMock.Object);
            cosmosToggleDataContext.EnvironmentRepository = _environmentRepositoryMock.Object;

            return cosmosToggleDataContext;
        }

        [Fact]
        public void CreateAsync_Shold_Be_Success()
        {
            _environmentRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Domain.Entities.Environment>(), It.IsAny<PartitionKey>()))
                .Returns(Task.FromResult<object>(null));

            var environmentAppService = new EnvironmentAppService(_mapper.Object, _environmentValidator, GetCosmosToggleDataContext(_environmentRepositoryMock.Object),
                new NotificationContext { });

            Func<Task> action = async () => { await environmentAppService.CreateAsync(GetDtoEnvironment()); };
            action.Should().NotThrowAsync();
        }

        [Fact]
        public void CreateAsync_EnvironmentProject_Null_Shold_Be_ValidationException()
        {
            var environmentAppService = new EnvironmentAppService(_mapper.Object, _environmentValidator,
                GetCosmosToggleDataContext(_environmentRepositoryMock.Object), new NotificationContext { });

            var environment = GetDtoEnvironment();
            environment.Project = null;

            Func<Task> action = async () => { await environmentAppService.CreateAsync(environment); };
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void CreateAsync_EnvironmentName_Null_Shold_Be_ValidationException()
        {
            var environmentAppService = new EnvironmentAppService(_mapper.Object, _environmentValidator,
                GetCosmosToggleDataContext(_environmentRepositoryMock.Object), new NotificationContext { });

            var environment = GetDtoEnvironment();
            environment.Name = null;

            Func<Task> action = async () => { await environmentAppService.CreateAsync(environment); };
            action.Should().ThrowExactly<ValidationException>();
        }

        [Fact]
        public void GetByProjectAsync_Shold_Be_Success()
        {
            _environmentRepositoryMock.Setup(x => x.GetByProjectAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Domain.Entities.Environment>>(new List<Domain.Entities.Environment>() { GetEntityEnvironment() }));

            var environmentAppService = new EnvironmentAppService(_mapper.Object, _environmentValidator,
                GetCosmosToggleDataContext(_environmentRepositoryMock.Object), new NotificationContext { });

            Func<Task> action = async () => { await environmentAppService.GetByProjectAsync("123456"); };
            action.Should().NotThrowAsync();
        }

        [Fact]
        public void GetByProjectAsync_Shold_Be_NullAndHasNotFoundNotification()
        {
            _environmentRepositoryMock.Setup(x => x.GetByProjectAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<Domain.Entities.Environment>>(null));

            var notificationContext = new NotificationContext { };

            var environmentAppService = new EnvironmentAppService(_mapper.Object, _environmentValidator,
                GetCosmosToggleDataContext(_environmentRepositoryMock.Object), notificationContext);

            Func<Task> action = async () => { await environmentAppService.GetByProjectAsync("123456"); };
            action.Should().NotThrowAsync();

            notificationContext.HasNotifications.Should().Be(true);
            notificationContext.Notifications.Where(x => x.Code == HttpStatusCode.NotFound);
        }
    }
}
