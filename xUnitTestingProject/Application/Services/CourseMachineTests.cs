using CBT3_Application.Interfaces;
using CBT3_Application.Services;

using CBT3_Domain.Entities;
using CBT3_Domain.Errors;

using Moq;

namespace CBT3_UnitTests.Application.Services;

public class CourseMachineTests
{
    [Fact()]
    public void CourseMachineTest()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var eventAggregatorMock = new Mock<IMessenger>();

        
        var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);

        // Act
        //var result = service.IsPaused;

        // Assert
        //Assert.True(result);
        //Assert.Fail("This test needs an implementation");
    }

    [Fact()]
    public void InitializeMachine_Valid()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var eventAggregatorMock = new Mock<IMessenger>();
        var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);
        TraineeID traineeID = new("1234567");
        CourseID courseID = new("Course1234");
        LessonID lessonID = new("lesson1");
        // Act
        Trainee trainee = new(traineeID);
        Course course = new(courseID);
        Lesson lesson = new(lessonID);
        course.Lessons.Add(lesson);
        var result = service.InitializeMachine(trainee, course);



        Assert.True(result.IsSuccess);
       // Assert.True(result.IsSuccess);
        
       

    }
    [Fact()]
    public void InitializeMachineNullTrainee_InValid()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var eventAggregatorMock = new Mock<IMessenger>();
        var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);

        Trainee trainee = null;//  new("1234567");
        CourseID courseID = new("Course1234");
        LessonID lessonID = new("lesson1");
        Course course = new(courseID);
        Lesson lesson = new(lessonID);
        // Act

        var result=   service.InitializeMachine(trainee, course);

        // Assert
        Assert.Equal(DomainErrors.TraineeError.NullOrEmpty, result.Error);
        Assert.True(result.IsFailure);



    }
    [Fact()]
    public void InitializeMachineNullCourse_InValid()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var eventAggregatorMock = new Mock<IMessenger>();
        var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);
        TraineeID traineeID = new("1234567");
        Trainee trainee = new(traineeID);
        Course course = null;//  new("Course1234");
        //Lesson lesson = new("lesson1");
        
        // Act
        var result = service.InitializeMachine(trainee, course);

        // Assert
        Assert.Equal(DomainErrors.CourseError.NullOrEmpty, result.Error);
        Assert.True(result.IsFailure);



    }
    [Fact()]
    public void StartTest_Valid()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var eventAggregatorMock = new Mock<IMessenger>();
        var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);
        TraineeID traineeID = new("1234567");
        CourseID courseID = new("Course1234");
        LessonID lessonID = new("lesson1");
        Trainee trainee = new(traineeID);
        Course course = new(courseID);
        Lesson lesson = new(lessonID);
        course.Lessons.Add(lesson);
        
        service.InitializeMachine(trainee, course);

        // Act
        var result = service.Start();

        // Assert
        Assert.True(result.IsSuccess);
    }
    [Fact()]
    public void StartTest_InValid()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var eventAggregatorMock = new Mock<IMessenger>();
        var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);
        TraineeID traineeID = new("1234567");
        CourseID courseID = new("Course1234");
        Trainee trainee = new(traineeID);
        Course course = new(courseID);

        //NO Lessons!

        service.InitializeMachine(trainee, course);

        // Act
        var result = service.Start();

        // Assert
        Assert.Equal(DomainErrors.MachineError.CurrentStateNullOrEmpty, result.Error);
        Assert.True(result.IsFailure);
    }
    //[Fact()]
    //public void StateChangeTest()
    //{
    //    // Arrange
    //    var mediatorMock = new Mock<IMediator>();
    //    var eventAggregatorMock = new Mock<IEventAggregator>();
    //    var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);

    //    // Act
    //    //var result = service.IsPaused;

    //    // Assert
    //    Assert.Fail("This test needs an implementation");
    //}

    //[Fact()]
    //public void MachinePauseTest()
    //{
    //    // Arrange
    //    var mediatorMock = new Mock<IMediator>();
    //    var eventAggregatorMock = new Mock<IEventAggregator>();
    //    var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);

    //    // Act
    //    //var result = service.IsPaused;

    //    // Assert
    //    Assert.Fail("This test needs an implementation");
    //}

    //[Fact()]
    //public void MachineResumeTest()
    //{
    //    // Arrange
    //    var mediatorMock = new Mock<IMediator>();
    //    var eventAggregatorMock = new Mock<IEventAggregator>();
    //    var service = new CourseMachine(mediatorMock.Object, eventAggregatorMock.Object);

    //    // Act
    //    //var result = service.IsPaused;

    //    // Assert
    //    Assert.Fail("This test needs an implementation");
    //}
}