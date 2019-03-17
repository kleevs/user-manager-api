using Moq;
using System.Collections.Generic;
using System.Linq;
using UserManager;
using UserManager.Implementation.Constant;
using UserManager.Implementation.Exception;
using UserManager.Model;
using UserManager.Spi;
using Xunit;

namespace UserManagerTest
{
    public class IdentityManagerTest
    {
        public class TestContext
        {
            public TestContext()
            {
                UserRepository = new Mock<IGenericReaderRepository<IUserLoginFilterable>>();
                UserFilterManager = new Mock<IFilterManager<ILoginFilter, IUserLoginFilterable>>();
                Sut = new UserManager.Implementation.IdentityManager(
                    UserRepository.Object,
                    UserFilterManager.Object
                );
            }
            
            public Mock<IGenericReaderRepository<IUserLoginFilterable>> UserRepository { get; set; }
            public Mock<IFilterManager<ILoginFilter, IUserLoginFilterable>> UserFilterManager { get; set; }
            public UserManager.Implementation.IdentityManager Sut { get; set; }
        }

        public class Login
        {
            [Fact]
            public void Should_Raise_Exception()
            {
                // arrange
                var context = new TestContext();
                context.UserRepository.Setup(_ => _.List()).Returns(Enumerable.Empty<IUserLoginFilterable>().AsQueryable());

                // act
                var exception = Assert.Throws<LoginException>(() => context.Sut.Login("login", "password"));

                // assert
                Assert.Equal(CodeError.LoginFailed, exception.Code);
            }

            [Theory]
            [InlineData("Email_1", "Password_1")]
            [InlineData("Email_1", "Password_2")]
            [InlineData("Email_3", "Password_2")]
            public void Should_Return_LoginFilterManager_Value(string login, string password)
            {
                // arrange
                var context = new TestContext();
                var query = new List<IUserLoginFilterable> { new TestUserEmailable() }.AsQueryable();
                context.UserFilterManager.Setup(_ => _.Apply(It.IsAny<ILoginFilter>(), It.IsAny<IQueryable<IUserLoginFilterable>>()))
                    .Returns(query);
                context.UserRepository.Setup(_ => _.List()).Returns(query);

                // act
                var user = context.Sut.Login(login, password);

                // assert
                Assert.Equal(query.FirstOrDefault(), user);
            }

            class TestUserEmailable : IUserLoginFilterable
            {
                public int? Id { get; set; }
                public string Email { get; set; }
                public string Password { get; set; }
            }
        }
    }
}
