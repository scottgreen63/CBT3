namespace CBT3_UnitTests.Application.Services;
using System.Threading.Tasks;

using CBT_Infrastructure.Repositories;
using CBT_Infrastructure.Services;

using CBT3_Application.Interfaces;
using CBT3_Application.Services;

using CBT3_Domain.Common;
using CBT3_Domain.Entities;
using CBT3_Domain.Errors;
using CBT3_Domain.ValueObjects;
using CBT3_Shared;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.Logging;

using Moq;

using Xunit;

public class RegistrationServiceTests
{
    CancellationTokenSource cts = new();
    CancellationToken ct;
    
    [Fact]
    public async Task SubmitFirstName_ValidFirstName_ReturnsSuccessResult()
    {
        // Arrange

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);

        var registrationService = new RegistrationService(trainingDataService);
        cts.Cancel();
        ct = cts.Token;
        
        
        var firstName = "Scott";

        // Act
        var result = await registrationService.SubmitFirstNameAsync(firstName,ct).ConfigureAwait(false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(firstName, result.Value);
    }
    [Fact]
    public async Task SubmitLastName_ValidLastLast_ReturnsSuccessResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var lastName = "Green";

        // Act
        var result = await registrationService.SubmitLastNameAsync(lastName,ct).ConfigureAwait(false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(lastName, result.Value);
    }

    [Fact]
    public async Task SubmitFirstName_InvalidFirstName_ReturnsErrorResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var firstName = "123";

        // Act
        var result = await registrationService.SubmitFirstNameAsync(firstName,ct).ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.FirstNameError.ContainsSpecialCharactersOrNumbers, result.Error);
    }
    [Fact]
    public async Task SubmitLastName_InvalidLastName_ReturnsErrorResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var lastName = "!23";

        // Act
        var result = await registrationService.SubmitLastNameAsync(lastName,ct).ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.LastNameError.ContainsSpecialCharactersOrNumbers, result.Error);
    }

    public async Task SubmitFirstUPID_InvalidUPID_ReturnsErrorResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var firstUpid = "123";

        // Act
        var result = await registrationService.SubmitFirstUPIDAsync(firstUpid,ct).ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        //Assert.Equal("Invalid first UPID", result.Error);
    }


    [Fact]
    public async Task SubmitFirstUPID_ValidUPID_ReturnsSuccessResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var _firstUpid = "1234567";

        // Act
        var result = await registrationService.SubmitFirstUPIDAsync(_firstUpid,ct).ConfigureAwait(false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_firstUpid, result.Value);
    }

    [Fact]
    public async Task SubmitSecondUPID_InvalidUPID_ReturnsErrorResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var _firstUpid = "1234567";
        var secondUpid = "123";

        // Act
        var result = await registrationService.SubmitSecondUPIDAsync(secondUpid, UPID.Create(_firstUpid).Value,ct).ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.UPIDError.RequiredLength, result.Error);
    }
    [Fact]
    public async Task SubmitSecondUPID_ValidUPID_ReturnsSuccessResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var _firstUpid = "1234567";
        var secondUpid = "1234567";

        // Act
        var result = await registrationService.SubmitSecondUPIDAsync(secondUpid, UPID.Create(_firstUpid).Value,ct).ConfigureAwait(false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_firstUpid, result.Value);
    }

    [Fact]
    public async Task SubmitFirstYearOfBirth_ValidYearOfBirth_ReturnsSuccessResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var firstYearOfBirth = "1965";

        // Act
        var result = await registrationService.SubmitFirstYearOfBirthAsync(firstYearOfBirth,ct).ConfigureAwait(false);

        // Assert
        Assert.True(result.IsSuccess);

    }
    [Fact]
    public async Task SubmitFirstYearOfBirth_InvalidYearOfBirth_ReturnsErrorResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var firstYearOfBirth = "1899";


        // Act
        var result = await registrationService.SubmitFirstYearOfBirthAsync(firstYearOfBirth,ct).ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.YearOfBirthError.OutOfRange, result.Error);
    }
    [Fact]
    public async Task SubmitSecondYearOfBirth_ValidYearOfBirth_ReturnsSuccessResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var firstYearOfBirth = "1965";
        var secondYearOfBirth = "1965";

        // Act
        var result = await registrationService.SubmitSecondYearOfBirthAsync(firstYearOfBirth, YearOfBirth.Create(secondYearOfBirth).Value,ct).ConfigureAwait(false);

        // Assert
        Assert.True(result.IsSuccess);

    }
    [Fact]
    public async Task SubmitSecondYearOfBirth_InvalidYearOfBirth_ReturnsErrorResult()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        var firstYearOfBirth = "1965";
        var secondYearOfBirth = "1962";


        // Act
        var result = await registrationService.SubmitSecondYearOfBirthAsync(firstYearOfBirth, YearOfBirth.Create(secondYearOfBirth).Value,ct).ConfigureAwait(false);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DomainErrors.YearOfBirthError.MismatchYearOfBirth, result.Error);
    }
    
    [Fact]
    public void GetTrainee_AllStepsCompleted_ReturnsTraineeObject()
    {
        // Arrange
        ct = cts.Token;
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();
        var loggerFactory = LoggerFactory.Create(builder => { });
        var loggerMock = new Mock<ILogger<TrainingDataService>>();

        ILogger<TrainingRepository> repoLogger = loggerFactory.CreateLogger<TrainingRepository>();

        var userDetailsMock = new Mock<UserDetails>();
        var repo = new TrainingRepository(repoLogger, userDetailsMock.Object, configuration);
        var configurationMock = new Mock<IConfiguration>();
        var trainingDataService = new TrainingDataService(loggerMock.Object, userDetailsMock.Object, configuration, repo);
        var registrationService = new RegistrationService(trainingDataService);
        TraineeID traineeId = new(Guid.NewGuid().ToString());

        Result<FirstName> firstname = registrationService.SubmitFirstNameAsync("Scott",ct).Result;
        Result<LastName> lastname = registrationService.SubmitLastNameAsync("Green",ct).Result;
        Result<UPID> upid1 = registrationService.SubmitFirstUPIDAsync("1234567",ct).Result;
        Result<UPID> upid2 = registrationService.SubmitSecondUPIDAsync("1234567", UPID.Create("1234567").Value,ct).Result;
        Result<YearOfBirth> yob1 = registrationService.SubmitFirstYearOfBirthAsync("1990",ct).Result;
        Result<YearOfBirth> yob2 = registrationService.SubmitSecondYearOfBirthAsync("1990", YearOfBirth.Create("1990").Value,ct).Result;

        Trainee trainee = new(traineeId);
        trainee.FirstName = firstname.Value;
        trainee.LastName = lastname.Value;
        trainee.UPID = upid2.Value;
        trainee.YearOfBirth = yob2.Value;
        trainee.CreatedDate = DateTime.Now;
        Result<Trainee> addedTrainee = registrationService.AddTraineeAsync(trainee, ct).Result;

        // Act
        var traineeget = registrationService.GetTraineeByIdAsync((TraineeID)addedTrainee.Value.Id).Result;
       
        // Assert
        Assert.NotNull(traineeget);
        Assert.Equal("Scott", traineeget.Value.FirstName);
        Assert.Equal("Green", traineeget.Value.LastName);
        Assert.Equal("1234567", traineeget.Value.UPID);
        Assert.Equal("1990", traineeget.Value.YearOfBirth);
        Assert.Equal<TraineeID>((TraineeID)addedTrainee.Value.Id,(TraineeID) traineeget.Value.Id);
    }
}

