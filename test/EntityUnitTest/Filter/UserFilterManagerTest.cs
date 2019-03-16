using Entity;
using EntityUnitTest.Tools;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using UserManager.Model;
using Xunit;

namespace EntityUnitTest
{
    public class UserFilterManagerTest
    {
        public class TestContext
        {
            public TestContext()
            {
                Sut = new Entity.Filter.UserFilterManager();
            }
            
            public Entity.Filter.UserFilterManager Sut { get; set; }
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
                var userDBSet = new FakeDbSet<User>();
                var dbContext = new Mock<IDbContext>();
                dbContext.Setup(_ => _.User).Returns(userDBSet);
                userDBSet.AddRange(new List<User>
                {
                    new User { Id = 1, Email = "Email", Password = "Password1", IsActive = true },
                    new User { Id = 2, Email = "Email", Password = "Password2", IsActive = false  },
                    new User { Id = 3, Email = "Email3", Password = "Password2", IsActive = false  },
                    new User { Id = 4, Email = "Email4", Password = "Password4", IsActive = true  },
                    new User { Id = 5, Email = "Email5", Password = "Password5", IsActive = false  },
                    new User { Id = 6, Email = "Email6", Password = "Password6", IsActive = true  }
                });

                // act
                var result = context.Sut.Apply(filter, userDBSet.AsQueryable(), dbContext.Object).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<User>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            [Theory]
            [InlineData("Email", 1, 2)]
            [InlineData("Email3", 3)]
            [InlineData("notfound")]
            public void Should_Filter_On_Email(string email, params int[] resultIds)
            {
                // arrange
                var context = new TestContext();
                var filter = new TestFilter { Email = email };
                var userDBSet = new FakeDbSet<User>();
                var dbContext = new Mock<IDbContext>();
                dbContext.Setup(_ => _.User).Returns(userDBSet);
                userDBSet.AddRange(new List<User>
                {
                    new User { Id = 1, Email = "Email", Password = "Password1", IsActive = true },
                    new User { Id = 2, Email = "Email", Password = "Password2", IsActive = false  },
                    new User { Id = 3, Email = "Email3", Password = "Password2", IsActive = false  },
                    new User { Id = 4, Email = "Email4", Password = "Password4", IsActive = true  },
                    new User { Id = 5, Email = "Email5", Password = "Password5", IsActive = false  },
                    new User { Id = 6, Email = "Email6", Password = "Password6", IsActive = true  }
                });

                // act
                var result = context.Sut.Apply(filter, userDBSet.AsQueryable(), dbContext.Object).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<User>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            [Theory]
            [InlineData("Password1", 1)]
            [InlineData("Password2", 2, 3)]
            [InlineData("Password")]
            public void Should_Filter_On_Password(string password, params int[] resultIds)
            {
                // arrange
                var context = new TestContext();
                var filter = new TestFilter { Password = password };
                var userDBSet = new FakeDbSet<User>();
                var dbContext = new Mock<IDbContext>();
                dbContext.Setup(_ => _.User).Returns(userDBSet);
                userDBSet.AddRange(new List<User>
                {
                    new User { Id = 1, Email = "Email", Password = "Password1", IsActive = true },
                    new User { Id = 2, Email = "Email", Password = "Password2", IsActive = false  },
                    new User { Id = 3, Email = "Email3", Password = "Password2", IsActive = false  },
                    new User { Id = 4, Email = "Email4", Password = "Password4", IsActive = true  },
                    new User { Id = 5, Email = "Email5", Password = "Password5", IsActive = false  },
                    new User { Id = 6, Email = "Email6", Password = "Password6", IsActive = true  }
                });

                // act
                var result = context.Sut.Apply(filter, userDBSet.AsQueryable(), dbContext.Object).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<User>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            [Theory]
            [InlineData(true, 1, 4, 6)]
            [InlineData(false, 2, 3, 5)]
            public void Should_Filter_On_IsActive(bool isActive, params int[] resultIds)
            {
                // arrange
                var context = new TestContext();
                var filter = new TestFilter { IsActive = isActive };
                var userDBSet = new FakeDbSet<User>();
                var dbContext = new Mock<IDbContext>();
                dbContext.Setup(_ => _.User).Returns(userDBSet);
                userDBSet.AddRange(new List<User>
                {
                    new User { Id = 1, Email = "Email", Password = "Password1", IsActive = true },
                    new User { Id = 2, Email = "Email", Password = "Password2", IsActive = false  },
                    new User { Id = 3, Email = "Email3", Password = "Password2", IsActive = false  },
                    new User { Id = 4, Email = "Email4", Password = "Password4", IsActive = true  },
                    new User { Id = 5, Email = "Email5", Password = "Password5", IsActive = false  },
                    new User { Id = 6, Email = "Email6", Password = "Password6", IsActive = true  }
                });

                // act
                var result = context.Sut.Apply(filter, userDBSet.AsQueryable(), dbContext.Object).ToList();

                // assert
                Assert.Collection(result, resultIds.Select<int, Action<User>>(uid => (user) => Assert.Equal(uid, user.Id)).ToArray());
            }

            class TestFilter : IFilter
            {
                public int? Id  { get; set; }
                public string Email  { get; set; }
                public string Password  { get; set; }
                public bool? IsActive  { get; set; }
            }
        }
    }
}
