﻿using InterviewHelper.Core.Exceptions;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.DataAccess.Repositories;

namespace InterviewHelper.Services.Services;

public class CommentService : ICommentService
{
    private readonly CommentRepository _commentRepository;
    private readonly IQuestionsService _questionService;

    public CommentService(CommentRepository commentRepository, IQuestionsService questionService)
    {
        _commentRepository = commentRepository;
        _questionService = questionService;
    }

    public List<Comment> GetAllQuestionComments(int questionId)
    {
        if (!_questionService.CheckIfQuestionExists(questionId))
        {
            throw new QuestionNotFoundException();
        }

        var questionComments = _commentRepository.GetAllQuestionComments(questionId);
        return questionComments;
    }

    public Comment AddComment(CommentRequest newComment)
    {
        var commentToAdd = new Comment
        {
            CommentContent = newComment.CommentContent,
            UserId = newComment.UserId,
            QuestionId = newComment.QuestionId,
            CreationDate = newComment.CreationDate
        };
        return _commentRepository.AddComment(commentToAdd);
    }

    public void EditComment(Comment comment)
    {
        _commentRepository.EditCommentContent(comment);
    }

    public void DeleteComment(int commentId)
    {
        _commentRepository.DeleteComment(commentId);
    }

    public User GetCommentOwnerById(int commentId)
    {
        var commentOwner = _commentRepository.GetCommentOwnerById(commentId);
        if (commentOwner == null)
        {
            throw new UserNotFoundException();
        }

        return commentOwner;
    }

    public bool CommentBelongsToUser(string email, int commentId)
    {
        try
        {
            var commentOwner = _commentRepository.GetCommentOwnerById(commentId);

            return commentOwner.Email == email;
        }
        catch (UserNotFoundException)
        {
            return false;
        }
    }
}