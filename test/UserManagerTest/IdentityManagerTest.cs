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
                UserRepository = new Mock<IAccountRepository>();
                Sut = new UserManager.Implementation.IdentityManager(
                    UserRepository.Object
                );
            }
            
            public Mock<IAccountRepository> UserRepository { get; set; }
            public UserManager.Implementation.IdentityManager Sut { get; set; }
        }

        public class Login
        {
            [Fact]
            public void Should_Raise_Exception()
            {
                // arrange
                var context = new TestContext();
                context.UserRepository.Setup(_ => _.Accounts).Returns(Enumerable.Empty<IUserFull>().AsQueryable());

                // act
                var exception = Assert.Throws<LoginException>(() => context.Sut.Login("login", "password"));

                // assert
                Assert.Equal(CodeError.LoginFailed, exception.Code);
            }

            [Theory]
            [InlineData("Email_1", "Password_1")]
            [InlineData("Email_1", "Password_2")]
            [InlineData("Email_3", "Password_2")]
            public void Should_Return_Value(string login, string password)
            {
                // arrange
                var context = new TestContext();
                var query = new List<IUserLoginFilterable> 
                { 
                    new TestUserEmailable { Email = "Email_1", Password = "Password_1" },
                    new TestUserEmailable { Email = "Email_1", Password = "Password_2" },
                    new TestUserEmailable { Email = "Email_3", Password = "Password_2" }
                }.AsQueryable();
                context.UserRepository.Setup(_ => _.Accounts).Returns(query);

                // act
                var user = context.Sut.Login(login, password);

                // assert
                Assert.Equal(query.FirstOrDefault(_ => _.Email == login && _.Password == password), user);
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
