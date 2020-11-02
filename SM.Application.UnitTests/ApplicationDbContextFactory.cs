using Microsoft.EntityFrameworkCore;
using Moq;
using SM.Application.Common.Interfaces;
using SM.Domain.Entities;
using SM.Infrastructure.Persistence;
using System;

namespace SM.Application.UnitTests
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var currentUserServiceMock = new Mock<ICurrentUserService>();
            currentUserServiceMock.Setup(m => m.UserId)
                .Returns("00000000-0000-0000-0000-000000000000");

            var context = new ApplicationDbContext(options, currentUserServiceMock.Object);

            context.Database.EnsureCreated();

            SeedSampleData(context);

            return context;
        }

        public static void SeedSampleData(ApplicationDbContext context)
        {
            context.Groups.AddRange(
                new Group { Id = 1, Name = "First" },
                new Group { Id = 2, Name = "Second" }
            );

            context.Students.AddRange(
                new Student { Id = 1, FirstName = "Mike", MiddleName = "William", PublicId = "Mike First" },
                new Student { Id = 2, FirstName = "Jane", MiddleName = "Grace", PublicId = "Jane Second" }
            );

            context.StudentGroup.AddRange(
                new StudentGroup { GroupId = 1, StudentId = 1},
                new StudentGroup { GroupId = 1, StudentId = 2 },
                new StudentGroup { GroupId = 2, StudentId = 1 }
            );

            context.SaveChanges();
        }

        public static void Destroy(ApplicationDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
