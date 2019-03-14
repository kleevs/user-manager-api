using Moq;
using System.Threading.Tasks;
using UserManager.Implementation.Constant;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;
using Xunit;

namespace UserManagerTest
{
    public class UserManagerTest
    {
        public class TestContext
        {
            public TestContext()
            {
                UnitOfWork = new Mock<IUnitOfWork>();
                UserRepository = new Mock<IGenericReaderRepository< IFilter, IUserData>>();
                NewUserRepository = new Mock<IGenericWriterRepository<INewUser>>();
                UpdateUserRepository = new Mock<IGenericWriterRepository< IUpdateUser, int>>();
                Hasher = new Mock<IHasher>();
                Sut = new UserManager.Implementation.UserManager(
                    UnitOfWork.Object,
                    UserRepository.Object,
                    NewUserRepository.Object,
                    UpdateUserRepository.Object,
                    Hasher.Object
                );
            }

            public Mock<IUnitOfWork> UnitOfWork { get; set; }
            public Mock<IGenericReaderRepository<IFilter, IUserData>> UserRepository { get; set; }
            public Mock<IGenericWriterRepository<INewUser>> NewUserRepository { get; set; }
            public Mock<IGenericWriterRepository<IUpdateUser, int>> UpdateUserRepository { get; set; }
            public Mock<IHasher> Hasher { get; set; }
            public UserManager.Implementation.UserManager Sut { get; set; } 
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
    }
}
