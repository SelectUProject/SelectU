using NUnit.Framework;
using SelectU.Contracts;
using SelectU.Contracts.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SelectU.Core.Extensions;
using SelectU.Core;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Infrastructure;
using SelectU.Core.Services;

namespace SelectU.Tests
{
    [TestFixture]
    public class UserTests
    {
        private IUnitOfWork _unitOfWork;
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWork = unitOfWorkMock.Object;

            var userServiceMock = new Mock<IUserService>();
            _userService = userServiceMock.Object;
        }

        [Test]
        public async Task Test_RegisterUserAsync_ValidInput_Success()
        {
            // Arrange
            var registerDTO = new UserRegisterDTO
            {
                Email = "test@example.com",
                Password = "Test@123",
            };

            var userStore = new Mock<IUserStore<User>>();
            var roleStore = new Mock<IRoleStore<IdentityRole>>();

            var userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStore.Object, null, null, null, null);
            var emailClientMock = new Mock<IEmailClient>();

            userManagerMock.Setup(x => x.FindByNameAsync(registerDTO.Email)).ReturnsAsync((User)null);
            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDTO.Password)).ReturnsAsync(IdentityResult.Success);
            roleManagerMock.Setup(x => x.RoleExistsAsync(UserRoles.User)).ReturnsAsync(true);
            emailClientMock.Setup(x => x.SendRegistrationEmailAsync(registerDTO)).Returns(Task.CompletedTask);

            var userService = new UserService(userManagerMock.Object, roleManagerMock.Object, emailClientMock.Object, _unitOfWork, null);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await userService.RegisterUserAsync(registerDTO));
        }
    }
}