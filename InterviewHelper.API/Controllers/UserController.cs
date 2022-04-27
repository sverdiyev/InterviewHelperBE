﻿using System.Net;
using InterviewHelper.Core.Models;
using InterviewHelper.Core.ServiceContracts;
using InterviewHelper.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace InterviewHelper.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly ILogger<UserController> _logger;
        
        public UserController(ILogger<UserController> logger,
            UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        
        // add user
        [HttpPost]
        public IActionResult AddUser(UserDTO newUser)
        {
            try
            {
                return Ok(_userService.AddUser(newUser));
            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        // edit user
        [HttpPut]
        public IActionResult EditUser(User user)
        {
            try
            {
                _userService.EditUser(user);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
    }

   
}
