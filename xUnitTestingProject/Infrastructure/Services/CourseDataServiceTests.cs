using CBT3_Domain.Errors;
using CBT3_Shared;
using CBT_Infrastructure.Repositories;
using CBT_Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using CBT3_Domain.Entities;
using Castle.Core.Configuration;

namespace CBT3_UnitTests.Application.Services
{
    public class CourseDataServiceTests
    {
        [Fact]
        public void GetCourses_Valid()
        {
            // Arrange

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            //var configurationMock = new Mock<IConfiguration>();
            var _dataserviceloggerMock = new Mock<ILogger<CourseDataService>>();
            var _repologgerMock = new Mock<ILogger<CourseRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            var repo = new CourseRepository(_repologgerMock.Object, userDetailsMock.Object, configuration);
            
            var service = new CourseDataService(_dataserviceloggerMock.Object, userDetailsMock.Object, configuration, repo);

            // Act

            var result = service.GetCoursesAsync().Result;

            // Assert
            Assert.True(result.Value.Count > 0);
            Assert.True(result.IsSuccess);


        }
        [Fact]
        public void GetCourses_InValid()
        {
            // Arrange

            IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .Build();
            var loggerFactory = LoggerFactory.Create(builder => { });
            var dataserviceLoggerMock = new Mock<ILogger<CourseDataService>>();
            var repoLoggerMock = new Mock<ILogger<CourseRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            //var configurationMock = new Mock<IConfiguration>();


            var repo = new CourseRepository(repoLoggerMock.Object, userDetailsMock.Object, configuration);
            var service = new CourseDataService(dataserviceLoggerMock.Object, userDetailsMock.Object, configuration, repo);

            // Act

            var result = service.GetCoursesAsync().Result;

            // Assert
            Assert.True(result.Value.Count > 0);
            Assert.True(result.IsSuccess);


        }
        // Add more tests for other methods and scenarios...

        [Fact]
        public void GetCourse_Valid()
        {
            // Arrange

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var dataserviceLoggerMock = new Mock<ILogger<CourseDataService>>();
            var repoLoggerMock = new Mock<ILogger<CourseRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            var repo = new CourseRepository(repoLoggerMock.Object, userDetailsMock.Object, configuration);
            var service = new CourseDataService(dataserviceLoggerMock.Object, userDetailsMock.Object, configuration, repo);

            // Act
            CourseID courseId = new("BLUE_C2120");
            var result = service.GetCourseByIdAsync(courseId).Result;

            // Assert
            //Assert.True(result.Value is not null);
            Assert.True(result.IsSuccess);

        }
        [Fact]
        public void GetCourse_InValid()
        {
            // Arrange

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var dataserviceLoggerMock = new Mock<ILogger<CourseDataService>>();
            var repoLoggerMock = new Mock<ILogger<CourseRepository>>();
            //var configurationMock = new Mock<IConfiguration>();
            var userDetailsMock = new Mock<UserDetails>();
            var repo = new CourseRepository(repoLoggerMock.Object, userDetailsMock.Object, configuration);
            var service = new CourseDataService(dataserviceLoggerMock.Object, userDetailsMock.Object, configuration, repo);

            // Act
            CourseID courseId = null; ;
            var result = service.GetCourseByIdAsync(courseId).Result;

            // Assert
            Assert.Equal(DomainErrors.CourseError.NullOrEmpty, result.Error);
            Assert.True(result.IsFailure);


        }
    }
}
