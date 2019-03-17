using Moq;
using System;
using System.Threading.Tasks;
using UserManager.Implementation.Constant;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;
using Xunit;

namespace UserManagerTest
{
    public class UserWriterServiceTest
    {
        public class TestContext
        {
            public TestContext()
            {
                UnitOfWork = new Mock<IUnitOfWork>();
                NewUserRepository = new Mock<IGenericWriterRepository<INewUser>>();
                UpdateUserRepository = new Mock<IGenericWriterRepository< IUpdateUser, int>>();
                Sut = new UserManager.Implementation.UserWriterService(
                    UnitOfWork.Object,
                    NewUserRepository.Object,
                    UpdateUserRepository.Object
                );
            }

            public Mock<IUnitOfWork> UnitOfWork { get; set; }
            public Mock<IGenericWriterRepository<INewUser>> NewUserRepository { get; set; }
            public Mock<IGenericWriterRepository<IUpdateUser, int>> UpdateUserRepository { get; set; }
            public UserManager.Implementation.UserWriterService Sut { get; set; } 
        }

        public class Delete
        {
            [Theory]
            [InlineData(1, 2)]
            [InlineData(1, 3)]
            public async Task Should_Call_Repository(int userId, int userConnectedId)
            {
                // arrange
                var context = new TestContext();

                // act
                await context.Sut.Delete(userId, userConnectedId);

                // assert
                context.UpdateUserRepository.Verify(_ => _.Delete(userId));
            }

            [Theory]
            [InlineData(1, 1)]
            [InlineData(2, 2)]
            public async Task Should_Raise_Exception(int userId, int userConnectedId)
            {
                // arrange
                var context = new TestContext();

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Delete(userId, userConnectedId));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.DeleteUserConnected);
            }
        }

        public class SaveNewUser
        {
            [Fact]
            public async Task Should_Call_Repository_Save_Method()
            {
                // arrange
                var context = new TestContext();
                var user = new TestNewUser
                {
                    LastName = "LastName",
                    FirstName = "FirstName",
                    BirthDate = DateTime.UtcNow,
                    Email = "Email",
                    Password = "Password"
                };

                // act
                await context.Sut.Save(user);

                // assert
                context.NewUserRepository.Verify(_ => _.Save(user));
            }

            [Fact]
            public async Task Should_Raise_Exception_If_FirstName_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestNewUser
                {
                    LastName = "LastName",
                    FirstName = null,
                    BirthDate = DateTime.UtcNow,
                    Email = "Email",
                    Password = "Password"
                };

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.FirstNameRequired);
            }

            [Fact]
            public async Task Should_Raise_Exception_If_LastName_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestNewUser
                {
                    LastName = null,
                    FirstName = "FirstName",
                    BirthDate = DateTime.UtcNow,
                    Email = "Email",
                    Password = "Password"
                };

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.LastNameRequired);
            }

            [Fact]
            public async Task Should_Raise_Exception_If_Birthdate_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestNewUser
                {
                    LastName = "LastName",
                    FirstName = "FirstName",
                    BirthDate = null,
                    Email = "Email",
                    Password = "Password"
                };

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.BirthDateRequired);
            }

            [Fact]
            public async Task Should_Raise_Exception_If_Password_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestNewUser
                {
                    LastName = "LastName",
                    FirstName = "FirstName",
                    BirthDate = DateTime.UtcNow,
                    Email = "Email",
                    Password = null
                };

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.PasswordRequired);
            }

            [Fact]
            public async Task Should_Raise_Exception_If_Email_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestNewUser
                {
                    LastName = "LastName",
                    FirstName = "FirstName",
                    BirthDate = DateTime.UtcNow,
                    Email = null,
                    Password = "Password"
                };

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.LoginRequired);
            }

            class TestNewUser : INewUser
            {
                public int? Id { get; set; }
                public DateTime? BirthDate { get; set; }
                public string Password { get; set; }
                public string Email { get; set; }
                public string LastName { get; set; }
                public string FirstName { get; set; }
                public bool IsActive { get; set; }
                public IUser ParentUser { get; set; }
            }
        }

        public class SaveUpdateUser
        {
            [Fact]
            public async Task Should_Call_Repository_Save_Method()
            {
                // arrange
                var context = new TestContext();
                var user = new TestUpdateUser
                {
                    LastName = "LastName",
                    FirstName = "FirstName",
                    BirthDate = DateTime.UtcNow
                };

                // act
                await context.Sut.Save(user);

                // assert
                context.UpdateUserRepository.Verify(_ => _.Save(user));
            }

            [Fact]
            public async Task Should_Raise_Exception_If_FirstName_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestUpdateUser();

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.FirstNameRequired);
            }

            [Fact]
            public async Task Should_Raise_Exception_If_LastName_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestUpdateUser();

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.LastNameRequired);
            }

            [Fact]
            public async Task Should_Raise_Exception_If_Birthdate_Is_Empty()
            {
                // arrange
                var context = new TestContext();
                var user = new TestUpdateUser();

                // act
                var exception = await Assert.ThrowsAsync<ArrayException>(async () => await context.Sut.Save(user));

                // assert
                Assert.Contains(exception.Errors, (error) => error.Code == CodeError.BirthDateRequired);
            }

            class TestUpdateUser : IUpdateUser
            {
                public int? Id { get; set; }
                public DateTime? BirthDate { get; set; }
                public string LastName { get; set; }
                public string FirstName { get; set; }
                public bool IsActive { get; set; }
            }
        }
    }
}
