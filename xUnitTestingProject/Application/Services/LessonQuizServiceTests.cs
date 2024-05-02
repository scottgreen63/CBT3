using CBT3_Domain.Enums;
using CBT3_Application.Interfaces;
using CBT3_Application.Services;

using CBT3_Domain.Entities;

using Moq;
using CBT3_Domain.Common;

namespace CBT3_UnitTests.Application.Services
{
    public class LessonQuizServiceTests
    {
        [Fact]
        public void LessonQuiz_IsQuizComplete_WithLessonLoaded()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var eventAggregatorMock = new Mock<IMessenger>();
            var service = new LessonQuizService(mediatorMock.Object, eventAggregatorMock.Object);
            TraineeID id = new("1234567");
            var trainee = new Trainee(id);
            CourseID courseId = new("123");
            LessonQuizID qid = new("qiz");
            var lessonQuiz = new LessonQuiz(qid);
            QuestionID qid1 = new("Q1");
            QuestionID qid2 = new("Q2");
            QuestionID qid3 = new("Q3");
            QuestionID qid4 = new("Q4"); 
            Question q1 = new Question(qid1);
            Question q2 = new Question(qid2);
            Question q3 = new Question(qid3);
            Question q4 = new Question(qid4);

            q1.QuestionType = QuestionType.TrueFalse;
            q2.QuestionType = QuestionType.TrueFalse;
            q3.QuestionType = QuestionType.TrueFalse;
            q4.QuestionType = QuestionType.TrueFalse;

            AnswerID anid1 = new("AN1");
            AnswerID anid2 = new("AN2");
            AnswerID anid3 = new("AN3");
            AnswerID anid4 = new("AN4");


            Answer an1 = new(anid1);
            Answer an2 = new(anid2);
            Answer an3 = new(anid3);
            Answer an4 = new(anid4);
            an1.IsCorrect = true;
            an2.IsCorrect = false;
            an3.IsCorrect = true;
            an4.IsCorrect = false;

            q1.Answers.Add(an1);
            q2.Answers.Add(an2);
            q3.Answers.Add(an3);
            q4.Answers.Add(an4);
            QuestionPoolID qpid1 = new("QP1");
            QuestionPool questionpool1 = new QuestionPool(qpid1);
            questionpool1.Questions.Add(q1);
            questionpool1.Questions.Add(q2);


            QuestionPoolID qpid2 = new("QP2");
            QuestionPool questionpool2 = new QuestionPool(qpid2);
            questionpool2.Questions.Add(q3);
            questionpool2.Questions.Add(q4);

            lessonQuiz.QuestionPools.Add(questionpool1);
            lessonQuiz.QuestionPools.Add(questionpool2);

            // Act
            service.InitializeQuiz(trainee, courseId, lessonQuiz);

            // Assert
            //Assert.Equal(trainee, service._trainee);
            //Assert.Equal(courseId, service._courseId);
            //Assert.Equal(lessonQuiz, service._lessonQuiz);
            var result = service.IsQuizComplete();
            Assert.Equal(result, false);

        }

        // Add more tests for other methods and scenarios...

        [Fact]
        public void LessonQuiz_IsQuizComplete_WithoutLessonLoaded()
        {
            // Arrange
            var mediatorMock = new Mock<IMediator>();
            var eventAggregatorMock = new Mock<IMessenger>();
            var service = new LessonQuizService(mediatorMock.Object, eventAggregatorMock.Object);

            // Act
            var result = service.IsQuizComplete();

            // Assert
            Assert.True(result);
        }

        //[Fact]
        //public void IsQuizFail_Returns_True_When_IsQuizFail_Property_Is_True()
        //{
        //    // Arrange
        //    var mediatorMock = new Mock<IMediator>();
        //    var eventAggregatorMock = new Mock<IEventAggregator>();
        //    var service = new LessonQuizService(mediatorMock.Object, eventAggregatorMock.Object);
        //    service._isQuizFail = true;

        //    // Act
        //    var result = service.IsQuizFail();

        //    // Assert
        //    Assert.True(result);
        //}

        // Add more tests for other scenarios...
    }
}
