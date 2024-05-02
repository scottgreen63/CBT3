using CBT3_Domain.Entities;
using CBT3_Domain.Interfaces;

namespace CBT_Infrastructure.Common;



public static class Mappers
{
    public static List<T> LoadCollection<T>(this SqlDataReader reader) where T : new()
    {
        List<T> collection = new List<T>();

        while (reader.Read())
        {
            T entity = CreateEntityFromReader<T>(reader);
            collection.Add(entity);
        }

        return collection;
    }

    private static T CreateEntityFromReader<T>(this SqlDataReader reader) where T : new()
    {
        T entity = new T();

        // Assuming you have a method or logic to populate entity properties from SqlDataReader
        PopulateEntityFromReader(reader, entity);

        return entity;
    }

    private static void PopulateEntityFromReader<T>(this SqlDataReader reader, T entity)
    {
        // Implement logic to map SqlDataReader columns to entity properties
        // This can be done manually or using an Object-Relational Mapper (ORM)
        // For simplicity, assuming properties have the same name as SqlDataReader columns

        for (int i = 0; i < reader.FieldCount; i++)
        {
            string columnName = reader.GetName(i);
            object columnValue = reader.GetValue(i);

            // Assuming entity properties have the same name as SqlDataReader columns
            PropertyInfo property = typeof(T).GetProperty(columnName);
            if (property != null && columnValue != DBNull.Value)
            {
                property.SetValue(entity, columnValue, null);
            }
        }
    }

    public static T GetValue<T>(this IDataReader reader, string columnName)
    {
        ////Id = (int)reader["Id"],
        ////Value1 = (int)reader["Value1"],
        ////Value2 = reader["Value2"].ToString()


        object value = reader[columnName]; // read column value

        return value == DBNull.Value ? default(T) : (T)value;


    }


    public static Course MapToCourse(SqlDataReader reader)
    {
        CourseID courseID = new(GetValue<string>(reader, FieldNames.fCourseId));
        Course course = new Course(courseID);

        course.CourseName = reader[FieldNames.fCourseName].ToString();
        course.ExpiryMonths = GetValue<int>(reader, FieldNames.fExpiryMonths);
        course.CourseType = GetValue<string>(reader, FieldNames.fCourseType);
        course.CreatedBy = "SYSTEM";
        course.CreatedDate = DateTime.UtcNow;
        return course;
    }

    public static Lesson MapToLesson(SqlDataReader reader)
    {
        LessonID lessonID = new(GetValue<string>(reader, FieldNames.fLessonId));
        Lesson lesson = new Lesson(lessonID);

        lesson.CourseID = new(GetValue<string>(reader, FieldNames.fCourseId));
        lesson.LessonName = GetValue<string>(reader, FieldNames.fLessonName);
        lesson.LessonOrder = GetValue<int>(reader, FieldNames.fLessonOrder);
        lesson.LessonPages = new List<LessonPage>();
        //lesson.LessonQuiz = new LessonQuiz();
        lesson.CreatedBy = "SYSTEM";
        lesson.CreatedDate = DateTime.UtcNow;
        return lesson;
    }
    public static LessonPage MapToLessonPage(SqlDataReader reader)
    {
        LessonPageID lessonPageID = new(GetValue<string>(reader, FieldNames.fLessonPageId));
        LessonPage lessonpage = new LessonPage(lessonPageID);
        lessonpage.LessonID = new(GetValue<string>(reader, FieldNames.fLessonId));
        lessonpage.LessonPageType = PageType.FromValue(GetValue<string>(reader, FieldNames.fPageTypeId));
        lessonpage.LessonPageSubType = GetValue<string>(reader, FieldNames.fPageSubTypeId);
        lessonpage.PageOrder = GetValue<int>(reader, FieldNames.fPageOrder);
        lessonpage.PageText = GetValue<string>(reader, FieldNames.fPageText);
        lessonpage.VideoURL = GetValue<string>(reader, FieldNames.fVideoURL);// URL.Create(GetValue<string>(reader, FieldNames.fVideoURL)).Value;
        lessonpage.AudioURL = GetValue<string>(reader, FieldNames.fAudioURL);// URL.Create(GetValue<string>(reader, FieldNames.fAudioURL)).Value;
        lessonpage.ImageURL = GetValue<string>(reader, FieldNames.fImageURL);// URL.Create(GetValue<string>(reader, FieldNames.fImageURL)).Value;

        return lessonpage;
    }

    public static LessonQuiz MapToLessonQuiz(SqlDataReader reader)
    {
        LessonQuizID lessonquizID = new(GetValue<string>(reader, FieldNames.fLessonQuizId));
        LessonQuiz lessonquiz = new LessonQuiz(lessonquizID);
        lessonquiz.LessonID = new(GetValue<string>(reader, FieldNames.fLessonId));
        lessonquiz.AttemptsAllowed = GetValue<int>(reader, FieldNames.fAttemptsAllowed);
        lessonquiz.CreatedBy = "SYSTEM";
        lessonquiz.CreatedDate = DateTime.UtcNow;
        return lessonquiz;
    }
    public static QuestionPool MapToQuestionPool(SqlDataReader reader)
    {
        QuestionPoolID questionpoolID = new(GetValue<string>(reader, FieldNames.fQuestionPoolId));
        QuestionPool questionpool = new QuestionPool(questionpoolID);
        questionpool.JumpBackStart = GetValue<string>(reader, FieldNames.fJumpBackStart);
        questionpool.JumpBackEnd = GetValue<string>(reader, FieldNames.fJumpBackEnd);
        questionpool.JumpBackFilename = GetValue<string>(reader, FieldNames.fJumpBackFileName);

        return questionpool;
    }
    public static Question MapToQuestion(SqlDataReader reader)
    {
        QuestionID questionID = new(GetValue<string>(reader, FieldNames.fQuestionId));
        Question question = new Question(questionID);
        question.QuestionText = GetValue<string>(reader, FieldNames.fQuestionText);
        question.QuestionImage = GetValue<string>(reader, FieldNames.fQuestionImage);
        question.QuestionType = QuestionType.FromValue(GetValue<string>(reader, FieldNames.fQuestionType));
        question.QuestionPoolID = new(GetValue<string>(reader, FieldNames.fQuestionPoolId));
        return question;
    }

    public static Answer MapToAnswer(SqlDataReader reader)
    {
        AnswerID answerID = new(GetValue<string>(reader, FieldNames.fAnswerId));
        Answer answer = new Answer(answerID);
        answer.QuestionID = new(GetValue<string>(reader, FieldNames.fQuestionId)); ;
        answer.AnswerText = GetValue<string>(reader, FieldNames.fAnswerText);
        answer.AnswerImage = GetValue<string>(reader, FieldNames.fAnswerImage);
        answer.IsCorrect = GetValue<bool>(reader, FieldNames.fIsCorrectAnswer);
        return answer;
    }
    public static Trainee MapToTrainee(SqlDataReader reader)
    {
        TraineeID traineeID = new TraineeID(GetValue<string>(reader, FieldNames.fTraineeId));
        Trainee trainee = new Trainee(traineeID);

        trainee.FirstName = FirstName.Create(GetValue<string>(reader, FieldNames.fFirstName)).Value;
        trainee.LastName = LastName.Create(GetValue<string>(reader, FieldNames.fLastName)).Value;
        trainee.UPID = UPID.Create(GetValue<string>(reader, FieldNames.fUPID)).Value;
        trainee.YearOfBirth = YearOfBirth.Create(GetValue<string>(reader, FieldNames.fYearOfBirth)).Value;
        trainee.CreatedBy = GetValue<string>(reader, FieldNames.fCreatedBy);
        trainee.CreatedDate = GetValue<DateTime>(reader, FieldNames.fCreatedDate);

        return trainee;
    }

   
    
}
