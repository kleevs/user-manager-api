using Moq;
using System.Collections.Generic;
using System.Linq;
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
                UserRepository = new Mock<IGenericReaderRepository<IFilter, IUserEmailable>>();
                Hasher = new Mock<IHasher>();
                Sut = new UserManager.Implementation.IdentityManager(
                    UserRepository.Object,
                    Hasher.Object
                );
            }
            
            public Mock<IGenericReaderRepository<IFilter, IUserEmailable>> UserRepository { get; set; }
            public Mock<IHasher> Hasher { get; set; }
            public UserManager.Implementation.IdentityManager Sut { get; set; }
        }

        public class Login
        {
            [Fact]
            public void Should_Raise_Exception()
            {
                // arrange
                var context = new TestContext();
                context.UserRepository.Setup(_ => _.List(It.IsAny<IFilter>())).Returns(Enumerable.Empty<IUserEmailable>());

                // act
                var exception = Assert.Throws<LoginException>(() => context.Sut.Login("login", "password"));

                // assert
                Assert.Equal(CodeError.LoginFailed, exception.Code);
            }

            [Fact]
            public void Should_Call_Repository_With_Filter()
            {
                // arrange
                var context = new TestContext();
                context.UserRepository.Setup(_ => _.List(It.IsAny<IFilter>())).Returns(new List<IUserEmailable>
                {
                    new TestUserEmailable { Id = 1, Email = "Email" }
                });

                // act
                var user = context.Sut.Login("login", "password");

                // assert
                context.UserRepository.Verify(_ => _.List(It.Is<IFilter>(filter => filter.Email == "login")));
            }

            [Fact]
            public void Should_Return_Repository_Item()
            {
                // arrange
                var context = new TestContext();
                context.UserRepository.Setup(_ => _.List(It.IsAny<IFilter>())).Returns(new List<IUserEmailable>
                {
                    new TestUserEmailable { Id = 1, Email = "Email" }
                });

                // act
                var user = context.Sut.Login("login", "password");

                // assert
                Assert.Equal(1, user.Id);
                Assert.Equal("Email", user.Email);
            }

            class TestUserEmailable : IUserEmailable
            {
                public int? Id { get; set; }
                public string Email { get; set; }
            }
        }
    }
}
