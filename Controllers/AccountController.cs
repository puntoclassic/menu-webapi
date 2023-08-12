using MenuBackend.Models.Auth;
using MenuBackend.Models.Data;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MenuBackend.Models.InputModel;
using MenuBackend.Models.ResponseModel;
using MenuBackend.Models.Options;
using MenuBackend.Services;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MenuBackend.Models.EmailModel;
namespace MenuBackend.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class AccountController : BaseController
{
    private readonly FrontendOptions frontendOptions;
    private readonly EmailService emailService;
    private readonly TokenService tokenService;
    public AccountController(
          UserManager<ApplicationUser> _userManager,
       ApplicationDbContext _dbContext,
    IOptions<FrontendOptions> _frontendOptions,
    TokenService _tokenService,
    EmailService _emailService) : base(_userManager, _dbContext)
    {

        this.frontendOptions = _frontendOptions.Value;
        this.emailService = _emailService;
        this.tokenService = _tokenService;
    }

    [HttpGet]
    public ActionResult<bool> GetEmailIsBusy([FromQuery] string Email)
    {
        return dbContext.Users.Any(w => w.Email == Email);
    }
    [HttpGet]
    [Authorize]
    public async Task<GetAccountStatusResponse> GetAccountStatus()
    {
        var user = await GetUserFromRequest();
        if (user != null)
        {
            var userClaims = await userManager!.GetClaimsAsync(user);
            return new GetAccountStatusResponse
            {
                Status = "Success",
                Logged = true,
                User = new LoggedUser
                {
                    Email = user.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Verified = user.EmailConfirmed,
                    Role = userClaims.First(w => w.Type == ClaimTypes.Role).Value ?? "user"
                }
            };
        }
        return new GetAccountStatusResponse
        {
            Status = "FetchFailed"
        };
    }
    [HttpPost]
    public async Task<PostGenericResponse> PostActivateAccountByToken([FromBody] PostAccountActivateByTokenRequest inputModel)
    {
        var user = await userManager!.FindByEmailAsync(inputModel.Email!);
        if (user != null)
        {
            inputModel.Token = inputModel.Token!.Replace(" ", "+");
            var result = await userManager.ConfirmEmailAsync(user, inputModel.Token!);
            if (result.Succeeded)
            {
                return new PostGenericResponse
                {
                    Status = "Account activated"
                };
            }
            else
            {
                var errorMessage = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    errorMessage.Append($"{item.Code}:{item.Description}");
                }
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Account activation failed",
                    ErrorMessage = errorMessage.ToString()
                };
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Account activation failed",
            };
        }
    }
    [HttpPost]
    public async Task<PostGenericResponse> PostRequireNewToken([FromBody] PostRequireNewTokenRequest inputModel)
    {
        var user = await userManager!.FindByEmailAsync(inputModel.Email!);
        if (user != null)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            //Generate page'url to activate account
            var url = $"{frontendOptions.BaseUrl}{frontendOptions.ActivateAccountByTokenUrl}?token={token}&email={inputModel.Email!}";
            this.emailService.SendAccountCreatedEmail(new Models.EmailModel.AccountCreatedModel
            {
                Email = user.Email,
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Url = url
            });
            return new PostGenericResponse
            {
                Status = "Token generated"
            };
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Token generation failed",
                ErrorMessage = "Invalid user"
            };
        }
    }
    [HttpPost]
    public async Task<PostGenericResponse> PostCreateAccount([FromBody] PostCreateAccountRequest inputModel)
    {
        //Prepare user model
        var user = new ApplicationUser
        {
            Firstname = inputModel.FirstName!,
            Lastname = inputModel.LastName!,
            Email = inputModel.Email!,
            EmailConfirmed = false,
            UserName = inputModel.Email!
        };
        //Create user
        var result = await userManager!.CreateAsync(user, inputModel.Password!);
        if (result.Succeeded)
        {
            //Assign user role
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "user"));
            //Generate token to activate account
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            //Generate page'url to activate account
            var url = $"{frontendOptions.BaseUrl}{frontendOptions.ActivateAccountByTokenUrl}?token={token}&email={inputModel.Email!}";
            //Send email
            this.emailService.SendAccountCreatedEmail(new Models.EmailModel.AccountCreatedModel
            {
                Email = inputModel.Email,
                FirstName = inputModel.FirstName,
                LastName = inputModel.LastName,
                Url = url
            });
            return new PostGenericResponse
            {
                Status = "Account created"
            };
        }
        else
        {
            var errorMessage = new StringBuilder();
            foreach (var item in result.Errors)
            {
                errorMessage.Append($"{item.Code}:{item.Description}");
            }
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Account creation failed",
                ErrorMessage = errorMessage.ToString()
            };
        }
    }
    [HttpPost]
    public async Task<PostLoginResponse> PostLogin([FromBody] PostLoginRequest inputModel)
    {
        var user = await userManager!.FindByEmailAsync(inputModel.Email!);
        if (user != null)
        {
            bool validatePassword = await userManager.CheckPasswordAsync(user, inputModel.Password!);
            if (validatePassword)
            {
                var userClaims = await userManager.GetClaimsAsync(user);
                var accessToken = tokenService.CreateToken(user, userClaims);
                HttpContext.Response.Cookies.Append("token", accessToken, new CookieOptions
                {
                    Expires = DateTimeOffset.FromUnixTimeSeconds(10 * 86400),
                    MaxAge = TimeSpan.FromDays(10),
                    SameSite = SameSiteMode.Lax,
                    HttpOnly = true
                });
                return new PostLoginResponse
                {
                    Status = "Ok",
                    User = new LoggedUser
                    {
                        Verified = user.EmailConfirmed,
                        Email = user.Email,
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Role = userClaims.First(w => w.Type == ClaimTypes.Role).Value ?? "user"
                    }
                };
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                return new PostLoginResponse
                {
                    Status = "LoginFailed",
                };
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new PostLoginResponse
            {
                Status = "LoginFailed",
                User = null
            };
        }
    }
    [HttpGet]
    [Authorize]
    public ActionResult GetLogout()
    {
        HttpContext.Response.Cookies.Delete("token");
        return Ok();
    }
    [HttpPost]
    public async Task<PostGenericResponse> PostResetPassword([FromBody] PostResetPassword inputModel)
    {
        var user = await userManager!.FindByEmailAsync(inputModel.Email!);
        if (user != null)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var url = $"{frontendOptions.BaseUrl}{frontendOptions.ResetPasswordByTokenUrl}?token={token}&email={inputModel.Email!}";
            this.emailService.SendPasswordResetEmail(new PasswordResetModel
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = inputModel.Email!,
                Url = url
            });
        }
        return new PostGenericResponse
        {
            Status = "success"
        };
    }
    [HttpPost]
    public async Task<PostGenericResponse> PostChangePasswordByToken([FromBody] PostChangePasswordByTokenRequest inputModel)
    {
        var user = await userManager!.FindByEmailAsync(inputModel.Email!);
        if (user != null)
        {
            inputModel.Token = inputModel.Token!.Replace(" ", "+");
            var result = await userManager.ResetPasswordAsync(user,
            inputModel.Token!,
            inputModel.Password!);
            if (result.Succeeded)
            {
                return new PostGenericResponse
                {
                    Status = "success"
                };
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Account not founded or token invalid"
                };
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Account not founded or token invalid"
            };
        }
    }
    [HttpPost]
    [Authorize]
    public async Task<PostGenericResponse> PostUpdatePersonalInfo([FromBody] PostUpdatePersonalInfoRequest inputModel)
    {
        var user = await GetUserFromRequest();
        if (user != null)
        {
            try
            {
                var applicationUser = dbContext.Users.First(w => w.Email == user.Email);
                applicationUser.Firstname = inputModel.FirstName;
                applicationUser.Lastname = inputModel.LastName;
                dbContext.Update(applicationUser);
                await dbContext.SaveChangesAsync();
                return new PostGenericResponse
                {
                    Status = "Success"
                };
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Error"
                };
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Error"
            };
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<PostGenericResponse> PostUpdatePassword([FromBody] PostUpdatePasswordRequest inputModel)
    {
        var user = await GetUserFromRequest();
        if (user != null)
        {
            var result = await userManager!.ChangePasswordAsync(user, inputModel.CurrentPassword, inputModel.Password);
            if (result.Succeeded)
            {
                return new PostGenericResponse
                {
                    Status = "Success"
                };

            }
            else
            {
                StringBuilder errorBuilder = new StringBuilder();
                foreach (var err in result.Errors)
                {
                    errorBuilder.Append($"{err.Code}:{err.Description}");
                }
                HttpContext.Response.StatusCode = 403;
                return new PostGenericResponse
                {
                    Status = "Error",
                    ErrorMessage = errorBuilder.ToString()
                };
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Error"
            };
        }
    }
}
