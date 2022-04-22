using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace InterviewHelper.Services.Services;

public class QuestionsServices : IQuestionsServices
{
    private readonly string _connectionString;

    public QuestionsServices(IOptions<DBConfiguration> config)
    {
        _connectionString = config.Value.ConnectionString;
    }

    public async Task<Question> AddQuestion(RequestQuestion newQuestion)
    {
        Question addedQuestion = new Question()
        {
            Complexity = newQuestion.Complexity,
            Note = newQuestion.Note,
            Tags = newQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList(),
            EasyToGoogle = newQuestion.EasyToGoogle,
            QuestionContent = newQuestion.QuestionContent,
        };

        using (var context = new InterviewHelperContext())
        {
            context.Questions.Add(addedQuestion);
            await context.SaveChangesAsync();

            return addedQuestion;
        }
    }

    public List<Question> GetQuestions(string? rawSearchParam)
    {
        using (var context = new InterviewHelperContext())
        {
            if (rawSearchParam == null)
            {
                return context.Questions.Include("Tags").ToList();
            }

            var searchParam = rawSearchParam.ToLower().Trim();

            var result = new List<Question>();
            result.AddRange(context.Questions
                .Where(q => q.Note.ToLower().Contains(searchParam) ||
                            q.QuestionContent.ToLower().Contains(searchParam) ||
                            (q.EasyToGoogle && searchParam == "easy to google") ||
                            q.Complexity.ToLower().Contains(searchParam) ||
                            q.Tags.Any(t => t.TagName.ToLower().Contains(searchParam)))
                .Include("Tags")
                .ToList());

            return result;
        }
    }
    
    public async void UpdateQuestion(RequestQuestion updatedQuestion)
    {
        using (var context = new InterviewHelperContext())
        {
            try
            {
                var existingQuestion = context.Questions.FirstOrDefault(q => q.Id == updatedQuestion.Id);
                if (existingQuestion != null)
                {
                    existingQuestion.Complexity = updatedQuestion.Complexity;
                    existingQuestion.Note = updatedQuestion.Note;
                    existingQuestion.Vote = updatedQuestion.Vote;
                    existingQuestion.EasyToGoogle = updatedQuestion.EasyToGoogle;
                    existingQuestion.QuestionContent = updatedQuestion.QuestionContent;
                    existingQuestion.Tags.Clear();
                    existingQuestion.Tags = updatedQuestion.Tags.Select(tag => new Tag() {TagName = tag}).ToList();
                }
                else throw new Exception("Not Found");
                
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}