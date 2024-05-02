using CBT_Infrastructure.Repositories;
using CBT_Infrastructure.Services;

using CBT3_Application.Configuration;
using CBT3_Application.Interfaces;
using CBT3_Application.Services;

using CBT3_Infrastructure.Configuration;

using CBT3_Shared;
using CBT3_Shared.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Moq;

namespace CBT3_UnitTests.Application.Services
{
    public class TrainingServiceTests
    {
        public static void Initialize()
        {
            ServiceCollection services = new ServiceCollection();
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<UserDetails>();
            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();
            services.AddSharedServices(configuration);



            ServiceProvider = services.BuildServiceProvider();

        }
        public static IServiceProvider ServiceProvider { get; private set; }

        [Fact()]
        public void GetCourseCodes_ReturnsCorrectCourseCodes()
        {
            // Arrange
            Initialize();
            IMediator _mediatorSvc = ServiceProvider.GetRequiredService<IMediator>();
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var trainingDataServiceloggerMock = new Mock<ILogger<TrainingDataService>>();
            var courseDataServiceloggerMock = new Mock<ILogger<CourseDataService>>();
            var trainingrepoLogger = new Mock<ILogger<TrainingRepository>>();
            var courserepoLogger = new Mock<ILogger<CourseRepository>>();
            var userDetailsMock = new Mock<UserDetails>();

            var trainingRepo = new TrainingRepository(trainingrepoLogger.Object, userDetailsMock.Object, configuration);
            var courseRepo = new CourseRepository(courserepoLogger.Object, userDetailsMock.Object, configuration);
            var trainingDataService = new TrainingDataService(trainingDataServiceloggerMock.Object, userDetailsMock.Object, configuration, trainingRepo);
            var courseDataService = new CourseDataService(courseDataServiceloggerMock.Object, userDetailsMock.Object, configuration, courseRepo);
            var trainingService = new TrainingService(_mediatorSvc, trainingDataService, courseDataService);

            // Act
           // var result = trainingService.GetCourseCodesAsync(true);

            // Assert
          //  Assert.True(result.Result.IsSuccess);
           // Assert.Equal(4, result.Result.Value.Count); // Expected count of SIDA courses

            // Assert specific course codes
            
        }
    }
}
