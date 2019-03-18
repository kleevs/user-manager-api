using Entity;
using Moq;
using UserManager.Model;
using Xunit;
using EntityUnitTest.Tools;
using System;
using UserManager.Spi;

namespace EntityUnitTest
{
    public class UserRepositoryTest
    {
        public class TestContext
        {
            public TestContext()
            {
                DbContext = new Mock<IDbContext>();
                Sut = new Entity.Repository.UserRepository(
                    DbContext.Object
                );
            }

            public Mock<IDbContext> DbContext { get; set; }
            public Entity.Repository.UserRepository Sut { get; set; }
        }

        public class Delete
        {
            [Theory]
            [InlineData(7)]
            [InlineData(1)]
            [InlineData(9)]
            public void Should_Delete_From_DBContext_By_Id(int idToDelete)
            {
                // arrange
                var userDbSet = new FakeDbSet<User>();
                var context = new TestContext();
                userDbSet.Add(new User { Id = idToDelete });
                context.DbContext.Setup(_ => _.User).Returns(userDbSet);

                // act
                context.Sut.Delete(idToDelete);

                // assert
                userDbSet.Mock.Verify(_ => _.Remove(It.Is<User>(user => user.Id == idToDelete)));
            }
        }

        public class SaveNewUser
        {
            [Theory]
            [InlineData("lastname", "firstname", "email", "password", true)]
            [InlineData("lastname", "firstname", "email", "password", false)]
            [InlineData("Dupont", "Marc", "test@email.com", "1234", true)]
            public void Should_Save_Into_DBContext(
                string lastName,
                string firstName,
                string email,
                string password,
                bool isActive
            )
            {
                // arrange
                var userDbSet = new FakeDbSet<User>();
                var context = new TestContext();
                var user = new TestNewUser
                {
                    Password = password,
                    Email = email,
                    LastName = lastName,
                    FirstName = firstName,
                    IsActive = isActive
                };
                userDbSet.Mock.Setup(_ => _.Add(It.IsAny<User>())).Callback<User>(u => u.Id = 1);
                context.DbContext.Setup(_ => _.User).Returns(userDbSet);

                // act
                context.Sut.Save(user);

                // assert
                userDbSet.Mock.Verify(_ => _.Add(It.Is<User>(u =>
                    u.LastName == lastName &&
                    u.FirstName == firstName &&
                    u.Email == email &&
                    u.Password == password &&
                    u.IsActive == isActive
                )));
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
            [Theory]
            [InlineData(1, "lastname", "firstname", true)]
            [InlineData(2, "lastname", "firstname", false)]
            [InlineData(3, "Dupont", "Marc", true)]
            public void Should_Save_Into_DBContext_By_Id(
                int idToUpdate,
                string lastName,
                string firstName,
                bool isActive
            )
            {
                // arrange
                var userDbSet = new FakeDbSet<User>();
                var context = new TestContext();
                userDbSet.Add(new User { Id = idToUpdate });
                var user = new TestUpdateUser
                {
                    Id = idToUpdate,
                    LastName = lastName,
                    FirstName = firstName,
                    IsActive = isActive
                };
                context.DbContext.Setup(_ => _.User).Returns(userDbSet);

                // act
                context.Sut.Save(user);

                // assert
                Assert.Collection(userDbSet, 
                    (u) => {
                        Assert.Equal(lastName, u.LastName);
                        Assert.Equal(firstName, u.FirstName);
                        Assert.Equal(isActive, u.IsActive);
                    });
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
