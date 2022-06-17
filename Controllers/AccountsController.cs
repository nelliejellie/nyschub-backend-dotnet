using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.DTO;
using nyschub.Entities;
using nyschub.Repositories;
using nyschub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public UserManager<Corper> _userManager { get; set; }
        private readonly AppDbContext _database;
        private readonly EmailService _email;
        private readonly TokenRepository _tokenRepository;

        public AccountsController(UserManager<Corper> userManager, AppDbContext database, EmailService email, TokenRepository tokenRepository)
        {
            _userManager = userManager;
            _database = database;
            _email = email;
            _tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            // check if registerdto is valid
            if (ModelState.IsValid)
            {
                // check if user already exists
                var userExist = await _userManager.FindByEmailAsync(registerDto.Email);
                var userWithRegNumber = await _database.Corpers.FirstOrDefaultAsync(corper => corper.NyscRegNumber == registerDto.NyscRegNumber);
                Console.WriteLine($"this is {userWithRegNumber}");

                if(userExist != null)
                {
                    return BadRequest("A user with this email already exists");
                }
                if (userWithRegNumber != null)
                {
                    return BadRequest("A user with this reg number already exists");
                }
            }
            // add the new user
            var newUser = new Corper()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                NyscRegNumber = registerDto.NyscRegNumber,
                Status = 1,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                EmailConfirmed = true
            };

            var isCreated = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest(
                    isCreated.Errors.Select(error => error.Description).ToList()
                    ); ;
            }

            var emailModel = new EmailModel()
            {
                Receipient = registerDto.Email,
                Title = "WELCOME TO NYSCHUB",
                Body = $"Dear {registerDto.FirstName} welcome to nyschub. your account has been created"
            };

            await _email.SendMail(emailModel);

            return Ok(new CorperDto { 
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName,
                NyscRegNumber = registerDto.NyscRegNumber
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userExist = await _userManager.FindByEmailAsync(loginDto.Email);

                    if (userExist == null)
                    {
                        return BadRequest("this user does not exist");
                    }

                    var isCorrect = await _userManager.CheckPasswordAsync(userExist, loginDto.Password);

                    if (isCorrect)
                    {
                        return Ok(new LoginResponseDto
                        {
                            Success = true
                        });
                    }
                }
                catch (Exception ex)
                {

                    return BadRequest($"server not responding {ex.Message}");
                }


            }
            
            return BadRequest();
        }

        [HttpPost]
        [Route("PasswordReset")]
        public async Task<IActionResult> SendPasswordResetLink(PasswordResetTokenDto passwordResetTokenDto)
        {
            var user = _userManager.FindByEmailAsync(passwordResetTokenDto.Email).Result;

            // check if user exists
            if (user == null)
            {
                return BadRequest("no user with that email exists");
            }

            // generate app token
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;


            // generate random token
            Random generator = new Random();
            string tokenSentToEmail = generator.Next(0, 1000000).ToString("D6");

            PasswordResetModel recoveryData = new PasswordResetModel
            {
                EmailToken = tokenSentToEmail,
                TokenGenerated = token
            };

            await _tokenRepository.Add(recoveryData);

            var emailModel = new EmailModel()
            {
                Receipient = passwordResetTokenDto.Email,
                Title = "NYSCHUB:  A LINK TO RESET YOUR PASSWORD",
                Body = $"Dear {user.FirstName} this is your reset token {tokenSentToEmail}"
            };

            await _email.SendMail(emailModel);

            return Ok(new { Success = true, Message = "an email has been sent successfully" });
        }

        [HttpPost]
        [Route("PasswordReset/Confirm")]
        public async Task<IActionResult> PasswordResetConfirm(ResetConfirmDto resetConfirmDto)
        {
            var tokenModel = await _tokenRepository.GetToken(resetConfirmDto.Token);
            var user = await _userManager.FindByNameAsync(resetConfirmDto.Username);

            if(tokenModel == null)
            {
                return BadRequest(new { Success = false, Error = "wrong token inputed" });
            }
            else
            {
                try
                {
                    await _userManager.ResetPasswordAsync(user, tokenModel.TokenGenerated, resetConfirmDto.Password);
                    return Ok("your password has been changed successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }
        }
    }
}
