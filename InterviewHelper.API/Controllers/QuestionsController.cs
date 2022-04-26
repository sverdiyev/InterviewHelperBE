using System.Net;
using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionsServices _questionsServices;

        public QuestionsController(IQuestionRepository questionRepository,
            ILogger<QuestionsController> logger,
            IQuestionsServices questionsServices)
        {
            _questionRepository = questionRepository;
            _logger = logger;
            _questionsServices = questionsServices;
        }

        [HttpGet]
        public IActionResult GetQuestions(string? search)
        {
            return Ok(_questionsServices.GetQuestions(search));
        }

        [HttpPost]
        public IActionResult PostAddQuestion(RequestQuestion newQuestion)
        {
            try
            {
                return Ok(_questionsServices.AddQuestion(newQuestion));
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuestion(RequestQuestion updatedQuestion)
        {
            try
            {
                await _questionsServices.UpdateQuestion(updatedQuestion);
                return NoContent();
            }
            catch (QuestionNotFoundException ex)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}