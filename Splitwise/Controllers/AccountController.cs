using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Angular_Pad.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Splitwise.Model;

namespace Angular_Pad.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager; 
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly IConfiguration configuration; 

        public AccountController( UserManager<IdentityUser> userManager,  SignInManager<IdentityUser> signInManager , IConfiguration config)
        {
            _userManager = userManager; 
            _signInManager = signInManager;
            configuration = config; 
        }
         

        [HttpPost("login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]  Login LoginModel)
        { 
            var user= await _userManager.FindByNameAsync(LoginModel.UserName);
            if (user == null) { 
                //return Ok("User Not Registered!");
                return NotFound("User Not Registered!");
            }
             

            if (!await _userManager.CheckPasswordAsync(user, LoginModel.Password))
            {
                //return Ok("Invalid Credentials!!");
                return BadRequest("Invalid Credentials!!");
            }
             
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub ,configuration["JwtConfig:Subject"]?? "" ),
                new Claim(JwtRegisteredClaimNames.Jti ,Guid.NewGuid().ToString() ),
                new Claim("UserName" , user.UserName.ToString() ?? ""),
                new Claim("Email" , user.Email.ToString()?? "" )

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtConfig:Key"] ));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["JwtConfig:Issuer"],
                configuration["JwtConfig:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signIn
                );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            

            var result = await _signInManager.PasswordSignInAsync(
                    user.UserName, LoginModel.Password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded) {
                return Ok(new { Token = tokenValue, User = user});
            }
            //return Ok("Login Failed ! Please try again");
            
            return BadRequest("Login Failed ! Please try again");
        }
        [HttpPost("register")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] Register registerModel)
        {
            var userExist = await _userManager.FindByEmailAsync(registerModel.Email);
            var usernameExist = await _userManager.FindByNameAsync(registerModel.UserName);
            if (userExist != null)
            {
                return BadRequest("User with this email already exists.");
            }

            if (usernameExist != null)
            {
                return BadRequest("Username is already taken. Please choose another.");
            }
            if (userExist == null)
            {
                IdentityUser user = new IdentityUser()
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email
                };
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded) { 
                    return Ok(user);
                }
                else
                {
                    //return Ok("Register Failed ! Please try again");
                    return BadRequest("Register Failed ! Please try again");
                }
            }
            //return Ok("User Already Exist");
            return BadRequest("User Already Exist");
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Json("User Logout");
        }

        [HttpGet("getallusers")]
        public async Task<List<string>> getallusers()
        {

            var items = await _userManager.Users.Select(e => e.UserName ).ToListAsync();
            return items;
        }


    }
}
