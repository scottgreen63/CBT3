﻿namespace CBT3_Application.Messaging;


    public class SubmitAnswerCommand : BaseCommandBundle, IRequest<Result<Question>>
    {
        public SubmitAnswerCommand(Answer answer)
        {
            Answer = answer;
        }
        public  Answer Answer { get; init; }

    }

public class SubmitAnswersCommand : BaseCommandBundle, IRequest<Result<Question>>
{
    public SubmitAnswersCommand(List<Answer> answers)
    {
        Answers = answers;
    }
    public List<Answer> Answers { get; init; }

}