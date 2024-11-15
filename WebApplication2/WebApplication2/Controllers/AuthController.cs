using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

//using JwtAuthMvcExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class AuthController : Controller
    {
        private static List<User> users = new List<User>();       
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_userManager = userManager;
            //_signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Index()
        { 
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User model)
        {
            if (users.Any(u => u.Username == model.Username))
            {
                return BadRequest("Username is already taken.");
            }

            var newUser = new User { Username = model.Username, Password = model.Password };
            users.Add(newUser);

            return Ok("User registered successfully.");
        }

        [HttpPost]
        public async Task<IActionResult> Login(User model)
        {
            var user = users.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Username);
        //        if (user != null)
        //        {
        //            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        //            if (result.Succeeded)
        //            {
        //                var token = GenerateJwtToken(user);
        //                return Ok(new { Token = token });
        //            }
        //        }
        //        ModelState.AddModelError("", "Invalid login attempt.");
        //    }
        //    return BadRequest(ModelState);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
