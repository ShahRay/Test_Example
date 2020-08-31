using DeepEqual.Syntax;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Test_Example.Back.Data.DapperHelpers;
using Test_Example.Back.Data.Queries;
using Test_Example.Back.Data.Repositories;
using Test_Example.Back.Domain;

namespace Test_Example.Back.Data.Test
{
    [TestFixture]
    internal class RepositoryTests
    {
        private Mock<ICommandText> _commandTextMock;
        private Mock<DbConnection> _dbConnectionMock;
        private Mock<IDapperHelper<Request>> _dapperHelperMock;
        private Mock<ILogger<Repository>> _logger;
        private Repository _repository;

        [SetUp]
        public void Setup()
        {
            _commandTextMock = new Mock<ICommandText>();

            _dbConnectionMock = new Mock<DbConnection>();
            _dbConnectionMock.SetupGet(connection => connection.ConnectionString).Returns("TestConnectionString");

            _dapperHelperMock = new Mock<IDapperHelper<Request>>();

            _logger = new Mock<ILogger<Repository>>();

            _repository = new Repository(_commandTextMock.Object, _dbConnectionMock.Object, _dapperHelperMock.Object, _logger.Object);
        }

        [Test]
        public void GetById_GiveValidData_ReturnValidData()
        {
            // Arrange.
            var testRequest = new Request() { Id = new Guid(), ClientId = new Guid(), DepartmentAddress = "Железнодорожная пр., 932, Тверь, Доминика", Amount = 221.86, Currency = "USD", ClientIp = "56.21.94.123", RequestStatus = "payment", DateTime = DateTime.Now };
            _dapperHelperMock
                .Setup(dapperHelper => dapperHelper.GetByIdAsync(It.IsAny<DbConnection>(), It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(testRequest));

            // Act.
            Request request = _repository.GetByIdAsync(It.IsAny<Guid>()).Result;

            // Assert.
            request.ShouldDeepEqual(testRequest);
        }

        [Test]
        public void GetByParams_GiveValidData_ReturnValidData()
        {
            // Arrange.
            IEnumerable<Request> testRequests = new List<Request>
            {
                new Request() { Id = new Guid(), ClientId = new Guid(), DepartmentAddress = "Железнодорожная пр., 932, Тверь, Доминика", Amount = 221.86, Currency = "USD", ClientIp = "56.21.94.123", RequestStatus = "payment", DateTime = DateTime.Now },
                new Request() { Id = new Guid(), ClientId = new Guid(), DepartmentAddress = "Железнодорожная пр., 932, Тверь, Доминика", Amount = 421.86, Currency = "UAH", ClientIp = "56.21.94.123", RequestStatus = "payment", DateTime = DateTime.Now.AddDays(30) }
            };
            _dapperHelperMock
                .Setup(dapperHelper => dapperHelper.GetByParamsAsync(It.IsAny<DbConnection>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(testRequests));

            // Act.
            IEnumerable<Request> requests = _repository.GetByParamsAsync(It.IsAny<Guid>(), It.IsAny<string>()).Result;

            // Assert.
            requests.ShouldDeepEqual(testRequests);
        }

        [Test]
        public void Verify_Add()
        {
            // Arrange.
            _dapperHelperMock
                .Setup(dapperHelper => dapperHelper.AddAsync(It.IsAny<DbConnection>(), It.IsAny<Request>(), It.IsAny<string>()))
                .Verifiable();

            // Act.
            _repository.AddAsync(It.IsAny<Request>()).Wait();

            // Assert.
            _dapperHelperMock.Verify();
        }
    }
}
