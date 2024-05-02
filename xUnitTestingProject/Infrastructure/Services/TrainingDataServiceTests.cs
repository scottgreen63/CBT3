using CBT3_Application.Configuration;
using CBT3_Application.Interfaces;
using CBT3_Application.Messaging.Commands;
using CBT3_Domain.Common;
using CBT3_Domain.Entities;
using CBT3_Domain.Errors;
using CBT3_Domain.ValueObjects;
using CBT3_Infrastructure.Configuration;
using CBT3_Shared;
using CBT3_Shared.Configuration;
using CBT_Infrastructure.Repositories;
using CBT_Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CBT3_UnitTests.Application.Services
{
    public static class ServiceProviderContainer
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


    }
    public class TrainingDataServiceTests
    {
        IConfigurationRoot _configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

        CancellationToken _ct = new();

        [Fact]
        public void AddTrainee_Valid()
        {
            // Arrange
            ServiceProviderContainer.Initialize();
            IMediator _mediatorSvc = ServiceProviderContainer.ServiceProvider.GetRequiredService<IMediator>();
            var loggerFactory = LoggerFactory.Create(builder => { });
            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var userDetailsMock = new Mock<UserDetails>();
            ILogger<TrainingRepository> repoTrainingLogger = loggerFactory.CreateLogger<TrainingRepository>();
            var trainingrepo = new TrainingRepository(repoTrainingLogger, userDetailsMock.Object, _configuration);
            var configurationMock = new Mock<IConfiguration>();
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            TraineeID traineeID = new(Guid.NewGuid().ToString());
            Trainee trainee = new Trainee(traineeID);
            ///First Name Using CQSR / Mediator
            SubmitFirstNameCommand submitfirstname = new();
            submitfirstname.FirstName = "Scott";
            Result<FirstName> submitFirstnameResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitfirstname, _ct).ConfigureAwait(false)).Result;

            SubmitLastNameCommand submitLastname = new();
            submitLastname.LastName = "Green";
            Result<LastName> submitLastnameResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitLastname, _ct).ConfigureAwait(false)).Result;


            SubmitFirstUPIDCommand submitFirstUPID = new();
            submitFirstUPID.FirstUPID = "1234567";
            Result<UPID> submitFirstUPIDResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitFirstUPID, _ct).ConfigureAwait(false)).Result;

            SubmitSecondUPIDCommand submitSecondUPID = new();
            submitSecondUPID.FirstUPID = submitFirstUPIDResult.Value;
            submitSecondUPID.SecondUPID = "1234567";
            Result<UPID> submitSecondUPIDResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitSecondUPID, _ct).ConfigureAwait(false)).Result;

            SubmitFirstYearOfBirthCommand submitFirstYearOfBirth = new();
            submitFirstYearOfBirth.FirstYearOfBirth = "1963";
            Result<YearOfBirth> submitFirstYearOfBirthResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitFirstYearOfBirth, _ct).ConfigureAwait(false)).Result;

            SubmitSecondYearOfBirthCommand submitsecondYearOfBirthCmd = new();
            submitsecondYearOfBirthCmd.FirstYearOfBirth = submitFirstYearOfBirthResult.Value;
            submitsecondYearOfBirthCmd.SecondYearOfBirth = "1963";
            Result<YearOfBirth> second_yearofbirthResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitsecondYearOfBirthCmd, _ct).ConfigureAwait(false)).Result;



            trainee.FirstName = submitFirstnameResult.Value;
            trainee.LastName = submitLastnameResult.Value;
            trainee.UPID = submitSecondUPIDResult.Value;
            trainee.YearOfBirth = second_yearofbirthResult.Value;
            trainee.CreatedDate = DateTime.Now;





            // Act



            var result = trainingservice.AddTraineeAsync(trainee, _ct).Result;

            // Assert
            //We're ignoring the ID because what is needed to create a new Trainee
            //may not match if that Trainee already existed
            Assert.NotNull(trainee);
            Assert.IsType<Trainee>(trainee);
            Assert.Equal(trainee.FirstName, result.Value.FirstName);
            Assert.Equal(trainee.LastName, result.Value.LastName);
            Assert.Equal(trainee.UPID, result.Value.UPID);
            Assert.Equal(trainee.YearOfBirth, result.Value.YearOfBirth);
            Assert.True(result.IsSuccess);


        }
        [Fact]
        public void AddTrainee_InValid()
        {
            // Arrange

            ServiceProviderContainer.Initialize();
            IMediator _mediatorSvc = ServiceProviderContainer.ServiceProvider.GetRequiredService<IMediator>();
            var loggerFactory = LoggerFactory.Create(builder => { });
            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var userDetailsMock = new Mock<UserDetails>();
            ILogger<TrainingRepository> repoTrainingLogger = loggerFactory.CreateLogger<TrainingRepository>();
            var trainingrepo = new TrainingRepository(repoTrainingLogger, userDetailsMock.Object, _configuration);
            var configurationMock = new Mock<IConfiguration>();
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            TraineeID traineeID = new(Guid.NewGuid().ToString());
            Trainee trainee = new Trainee(traineeID);
            ///First Name Using CQSR / Mediator
            SubmitFirstNameCommand submitfirstname = new();
            submitfirstname.FirstName = "Scott";
            Result<FirstName> submitFirstnameResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitfirstname, _ct)).Result;

            SubmitLastNameCommand submitLastname = new();
            submitLastname.LastName = "Green";
            Result<LastName> submitLastnameResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitLastname, _ct)).Result;


            SubmitFirstUPIDCommand submitFirstUPID = new();
            submitFirstUPID.FirstUPID = "1234567";
            Result<UPID> submitFirstUPIDResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitFirstUPID, _ct)).Result;

            SubmitSecondUPIDCommand submitSecondUPID = new();
            submitSecondUPID.FirstUPID = submitFirstUPIDResult.Value;
            submitSecondUPID.SecondUPID = "1234567";
            Result<UPID> submitSecondUPIDResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitSecondUPID, _ct)).Result;

            SubmitFirstYearOfBirthCommand submitFirstYearOfBirth = new();
            submitFirstYearOfBirth.FirstYearOfBirth = "1963";
            Result<YearOfBirth> submitFirstYearOfBirthResult = Task.Run(async () => await _mediatorSvc.SendAsync(submitFirstYearOfBirth, _ct)).Result;

            SubmitSecondYearOfBirthCommand submitsecondYearOfBirthCmd = new();
            submitsecondYearOfBirthCmd.FirstYearOfBirth = submitFirstYearOfBirthResult.Value;
            submitsecondYearOfBirthCmd.SecondYearOfBirth = "1963";
            Result<YearOfBirth> second_yearofbirthResult = Task.Run(() => _mediatorSvc.SendAsync(submitsecondYearOfBirthCmd, _ct)).Result;



            trainee.FirstName = null; // submitFirstnameResult.Value;
            trainee.LastName = submitLastnameResult.Value;
            trainee.UPID = submitSecondUPIDResult.Value;
            trainee.YearOfBirth = second_yearofbirthResult.Value;
            trainee.CreatedDate = DateTime.Now;

            // Act

            var result = trainingservice.AddTraineeAsync(trainee, _ct).Result;

            // Assert
            Assert.Equal(DomainErrors.TraineeError.NullOrEmpty, result.Error);
            Assert.True(result.IsFailure);


        }
        // Add more tests for other methods and scenarios...

        [Fact]
        public void AddTrainingLogEntry_InValid()
        {
            // Arrange

            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var repoTrainingLogger = new Mock<ILogger<TrainingRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            var configurationMock = new Mock<IConfiguration>();

            var trainingrepo = new TrainingRepository(repoTrainingLogger.Object, userDetailsMock.Object, _configuration);
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            // Act
            TrainingLogEntryID tlid = new("newid");
            TrainingLogEntry tle = new(tlid);

            var result = trainingservice.AddTrainingLogEntryAsync(tle, _ct).Result;

            // Assert
            Assert.Equal(DomainErrors.TrainingLogEntryError.AddTrainingLogEntry, result.Error);
            Assert.True(result.IsFailure);

        }
        [Fact]
        public void AddTrainingLogEntry_Valid()
        {
            // Arrange

            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var repoTrainingLogger = new Mock<ILogger<TrainingRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            var configurationMock = new Mock<IConfiguration>();

            var trainingrepo = new TrainingRepository(repoTrainingLogger.Object, userDetailsMock.Object, _configuration);
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            // Act
            TrainingLogEntryID tleid = new("tleid");
            TrainingLogEntry tle = new(tleid);

            tle.AnswerId = new AnswerID("ANSWR1");
            tle.CourseId = new CourseID("COURSE1");
            tle.CreatedDate = DateTime.Now;
            tle.IsCorrect = false;
            tle.LessonId = new("LESSON1");
            tle.LessonQuizId = new LessonQuizID("LESSONQUIZ1");
            tle.QuestionId = new QuestionID("QUESTION1");
            tle.QuestionPoolId = new("QUESTIONPOOLID1");
            tle.TraineeId = new("TRAINEE1");
            tle.RecordedAt = DateTime.Now;



            var result = trainingservice.AddTrainingLogEntryAsync(tle, _ct).Result;

            // Assert

            Assert.True(result.IsSuccess);


        }


        [Fact]
        public void DeleteTrainingLogEntries_Valid()
        {
            // Arrange

            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var repoTrainingLogger = new Mock<ILogger<TrainingRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            var configurationMock = new Mock<IConfiguration>();

            var trainingrepo = new TrainingRepository(repoTrainingLogger.Object, userDetailsMock.Object, _configuration);
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            CourseID courseId = new("BLUE_C2120");
            TraineeID traineeId = new("2b9a0379-9d2e-4f72-944a-ef50c7f46ca9"); // THIS COMBO IS A ONE TIME USE TEST
            LessonQuizID lessonquizid = new("BLUE_C2120_L1 _QZ1");

            // Act
            var result = trainingservice.DeleteTrainingLogEntriesAsync(traineeId, courseId, lessonquizid, _ct).Result;

            // Assert
            Assert.True(result.IsSuccess);

        }
        [Fact]
        public void DeleteTrainingLogEntries_InValid()
        {
            // Arrange

            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var repoTrainingLogger = new Mock<ILogger<TrainingRepository>>();
            var userDetailsMock = new Mock<UserDetails>();
            var configurationMock = new Mock<IConfiguration>();

            var trainingrepo = new TrainingRepository(repoTrainingLogger.Object, userDetailsMock.Object, _configuration);
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            CourseID courseId = null;
            TraineeID traineeid = null;
            LessonQuizID lessonquizid = null;

            // Act


            var result = trainingservice.DeleteTrainingLogEntriesAsync(traineeid, courseId, lessonquizid, _ct).Result;

            // Assert
            Assert.Equal(DomainErrors.TrainingLogEntryError.NullOrEmptyParam, result.Error);
            Assert.True(result.IsFailure);


        }

        [Fact]
        public void CompleteCourse_Valid()
        {
            // Arrange

            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var repoTrainingLogger = new Mock<ILogger<TrainingRepository>>();
            var configurationMock = new Mock<IConfiguration>();
            var userDetailsMock = new Mock<UserDetails>();

            var trainingrepo = new TrainingRepository(repoTrainingLogger.Object, userDetailsMock.Object, _configuration);
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);

            CourseID courseId = new("BLUE_C2120");
            TraineeID traineeId = new("6db88752-2650-4f6d-9c4b-d12ddb9c037a");  //This needs be verified before running tests

            //Trainee trainee = Task.Run(() => trainingservice.GetTraineeAsync(traineeId)).Result.Value;
            //Course course = Task.Run(() => courseservice.GetCourseByIdAsync(courseId)).Result.Value;

            bool passcourse = false;


            // Act

            var result = trainingservice.CompleteCourseAsync(traineeId, courseId, passcourse, _ct).Result;

            // Assert
            Assert.True(result.Value == true);
            Assert.True(result.IsSuccess);

        }
        [Fact]
        public void CompleteCourse_InValid()
        {
            // Arrange

            var trainingloggerMock = new Mock<ILogger<TrainingDataService>>();
            var repoTrainingLogger = new Mock<ILogger<TrainingRepository>>();
            var configurationMock = new Mock<IConfiguration>();
            var userDetailsMock = new Mock<UserDetails>();

            var trainingrepo = new TrainingRepository(repoTrainingLogger.Object, userDetailsMock.Object, _configuration);
            var trainingservice = new TrainingDataService(trainingloggerMock.Object, userDetailsMock.Object, configurationMock.Object, trainingrepo);


            CourseID courseId = null; // NullOrEmptyParam;
            TraineeID traineeId = new("6db88752-2650-4f6d-9c4b-d12ddb9c037a");  //This needs be verified before running tests

            bool passcourse = false;

            // Act

            var result = trainingservice.CompleteCourseAsync(traineeId, courseId, passcourse, _ct).Result;

            // Assert
            Assert.Equal(DomainErrors.TrainingLogEntryError.NullOrEmptyParam, result.Error);
            Assert.True(result.IsFailure);


        }


        [Fact]
        public void CheckID_InValid()
        {
            // Arrange
            CourseID courseId;
            string invalidIdValue = string.Empty;
            Exception result = new();
            // Act
            try
            {
                courseId = new(string.Empty);
            }
            catch (Exception ex)
            {
                result = ex;
            }

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ArgumentException>(result);
            Assert.Equal("ID value cannot be empty. (Parameter 'value')", result.Message);
            // Check if the exception message is as expected
        }

        [Fact]
        public void CheckID_Valid()
        {
            // Arrange


            CourseID courseId;
            string validIdValue = "TEST";
            Exception result = new();
            // Act
            courseId = new CourseID(validIdValue);

            // Assert
            Assert.NotNull(courseId);
            Assert.Equal(validIdValue, courseId.Value);
        }
    }
}
