using Moq;
using System.Collections.Generic;
using System.Linq;
using UserManager;
using UserManager.Model;
using UserManager.Spi;
using Xunit;

namespace UserManagerTest
{
    public class UserReaderServiceTest
    {
        public class TestContext
        {
            public TestContext()
            {
                UserRepository = new Mock<IGenericReaderRepository<IUserFilterable>>();
                UserFilterManager = new Mock<IFilterManager<IFilter, IUserFilterable>>();
                Sut = new UserManager.Implementation.UserReaderService(
                    UserRepository.Object,
                    UserFilterManager.Object
                );
            }
            
            public Mock<IGenericReaderRepository<IUserFilterable>> UserRepository { get; set; }
            public Mock<IFilterManager<IFilter, IUserFilterable>> UserFilterManager { get; set; }
            public UserManager.Implementation.UserReaderService Sut { get; set; } 
        }

        public class List
        {
            [Fact]
            public void Should_Return_Value_From_Repository()
            {
                // arrange
                var context = new TestContext();
                var expected = new List<IUserFilterable>().AsQueryable();
                context.UserRepository.Setup(_ => _.List()).Returns(expected);

                // act
                var result = context.Sut.List();

                // assert
                context.UserRepository.Verify(_ => _.List());
                Assert.Equal(expected, result);
            }

            class TestFilter : IFilter
            {
                public int? Id { get; set; }
                public string Email { get; set; }
                public string Password { get; set; }
                public bool? IsActive { get; set; }
            }
        }
    }
}
