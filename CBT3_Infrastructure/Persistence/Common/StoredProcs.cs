
namespace CBT3_Infrastructure.Persistence.Common
{
    public static class StoredProcs
    {


        private static readonly Lazy<string> _cn_spSelect = new Lazy<string>(() => "sp_GetCourse");
        public static string cn_spSelect => _cn_spSelect.Value;

        private static readonly Lazy<string> _cn_spList = new Lazy<string>(() => "sp_ListCourses");
        public static string cn_spList => _cn_spList.Value;

        private static readonly Lazy<string> _cn_spGetCourseLessons = new Lazy<string>(() => "sp_GetCourseLessons");
        public static string cn_spGetCourseLessons => _cn_spGetCourseLessons.Value;

        private static readonly Lazy<string> _cn_spGetLessonPages = new Lazy<string>(() => "sp_GetLessonPages");
        public static string cn_spGetLessonPages => _cn_spGetLessonPages.Value;

        private static readonly Lazy<string> _cn_spGetLessonQuiz = new Lazy<string>(() => "sp_GetLessonQuiz");
        public static string cn_spGetLessonQuiz => _cn_spGetLessonQuiz.Value;

        private static readonly Lazy<string> _cn_sp_GetLessonQuizQuestionPools = new Lazy<string>(() => "sp_GetLessonQuizQuestionPools");
        public static string cn_sp_GetLessonQuizQuestionPools => _cn_sp_GetLessonQuizQuestionPools.Value;

        private static readonly Lazy<string> _cn_sp_GetQuestionPoolQustions = new Lazy<string>(() => "sp_GetQuestionPoolQustions");
        public static string cn_sp_GetQuestionPoolQuestions => _cn_sp_GetQuestionPoolQustions.Value;

        private static readonly Lazy<string> _cn_sp_GetQuestionAnswers = new Lazy<string>(() => "sp_GetQuestionAnswers");
        public static string cn_sp_GetQuestionAnswers => _cn_sp_GetQuestionAnswers.Value;

        
        private static readonly Lazy<string> _cn_spAddTrainee = new Lazy<string>(() => "sp_InsertTrainee");
        public static string cn_spAddTrainee => _cn_spAddTrainee.Value;

        private static readonly Lazy<string> _cn_spGetTrainee = new Lazy<string>(() => "sp_GetTrainee");
        public static string cn_spGetTrainee => _cn_spGetTrainee.Value;

        private static readonly Lazy<string> _cn_spModifyTrainee = new Lazy<string>(() => "sp_ModifyTrainee");
        public static string cn_spModifyTrainee => _cn_spModifyTrainee.Value;


        private static readonly Lazy<string> _cn_spInsertTrainingLogEntry = new Lazy<string>(() => "sp_InsertTrainingLogEntry");
        public static string cn_spInsertTrainingLogEntry => _cn_spInsertTrainingLogEntry.Value;

        private static readonly Lazy<string> _cn_spDeleteTrainingLogEntries = new Lazy<string>(() => "sp_DeleteTrainingLogEntries");
        public static string cn_spDeleteTrainingLogEntries => _cn_spDeleteTrainingLogEntries.Value;

        private static readonly Lazy<string> _cn_spAddAuditLogEntry = new Lazy<string>(() => "sp_AddAuditLogEntry");
        public static string cn_spAddAuditLogEntry => _cn_spAddAuditLogEntry.Value;

        private static readonly Lazy<string> _cn_spCompleteCourse = new Lazy<string>(() => "sp_CompleteCourse");
        public static string cn_spCompleteCourse => _cn_spCompleteCourse.Value;


        private static readonly Lazy<string> _cn_spCheckForPreviousTrainingOnDate = new Lazy<string>(() => "sp_CheckForPreviousTrainingOnDate");
        public static string cn_spCheckForPreviousTrainingOnDate => _cn_spCheckForPreviousTrainingOnDate.Value;
    }
}
