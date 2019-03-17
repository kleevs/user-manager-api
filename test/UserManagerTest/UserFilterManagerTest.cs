using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManager.Model;
using UserManager.Spi;
using Xunit;

namespace UserManagerTest
{
    public class UserFilterManagerTest
    {
        public class TestContext
        {
            public TestContext()
            {
                Hasher = new Mock<IHasher>();
                Sut = new UserManager.Implementation.UserFilterManager(Hasher.Object);
            }

            public Mock<IHasher> Hasher { get; set; }
            public UserManager.Implementation.UserFilterManager Sut { get; set; }
        }

        public class Apply
        {
            [Theory]
            [InlineData(1, 1)]
            [InlineData(2, 2)]
            [InlineData(5, 5)]
            [InlineData(9)]
            public void Should_Filter_On_Id(int id, params int[] resultIds)
            {
                // arrange
                var context = new TestContext();
                var filter = new TestFilter { Id = id };
                var query = new List<IUserFilterable>
                {
                    new TestUser { Id = 1, Email = "Email", IsActive = true },
                    new TestUser { Id = 2, Email = "Email", IsActive = false  },
                    new TestUser { Id = 3, Email = "Email3", IsActive = false  },
                    new TestUser { Id = 4, Email = "Email4", IsActive = true  },
                    new TestUser { Id = 5, Email = "Email5", IsActive = false  },
                    new TestUser { Id = 6, Email = "Email6", IsActive = true  }
                }.AsQueryable();

                // act
                var result = context.Sut.Apply(filter, query).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<IUserFilterable>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            [Theory]
            [InlineData("Email", 1, 2, 3, 4, 5, 6)]
            [InlineData("Email3", 3)]
            [InlineData("notfound")]
            public void Should_Filter_On_Email(string email, params int[] resultIds)
            {
                // arrange
                var context = new TestContext();
                var filter = new TestFilter { Email = email };
                var query = new List<IUserFilterable>
                {
                    new TestUser { Id = 1, Email = "Email", IsActive = true },
                    new TestUser { Id = 2, Email = "Email", IsActive = false  },
                    new TestUser { Id = 3, Email = "Email3", IsActive = false  },
                    new TestUser { Id = 4, Email = "Email4", IsActive = true  },
                    new TestUser { Id = 5, Email = "Email5", IsActive = false  },
                    new TestUser { Id = 6, Email = "Email6", IsActive = true  }
                }.AsQueryable();

                // act
                var result = context.Sut.Apply(filter, query).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<IUserFilterable>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            [Theory]
            [InlineData(true, 1, 4, 6)]
            [InlineData(false, 2, 3, 5)]
            public void Should_Filter_On_IsActive(bool isActive, params int[] resultIds)
            {
                // arrange
                var context = new TestContext();
                var filter = new TestFilter { IsActive = isActive };
                var query = new List<IUserFilterable>
                {
                    new TestUser { Id = 1, Email = "Email", IsActive = true },
                    new TestUser { Id = 2, Email = "Email", IsActive = false  },
                    new TestUser { Id = 3, Email = "Email3", IsActive = false  },
                    new TestUser { Id = 4, Email = "Email4", IsActive = true  },
                    new TestUser { Id = 5, Email = "Email5", IsActive = false  },
                    new TestUser { Id = 6, Email = "Email6", IsActive = true  }
                }.AsQueryable();

                // act
                var result = context.Sut.Apply(filter, query).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<IUserFilterable>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            class TestFilter : IFilter
            {
                public int? Id  { get; set; }
                public string Email  { get; set; }
                public bool? IsActive  { get; set; }
            }

            class TestUser : IUserFilterable
            {
                public int? Id { get; set; }
                public string Email { get; set; }
                public bool IsActive { get; set; }
                public DateTime? BirthDate  { get; set; }
                public string LastName  { get; set; }
                public string FirstName  { get; set; }
                public IUser ParentUser  { get; set; }
            }
        }
    }
}
