using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DATA.DTO;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Createuser(UserDTO user, string pass)
        {
            IdentityUser temp = new IdentityUser();
            if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(user.UserName))
            {
                return BadRequest("Important values are missing");
            }
            temp.UserName = user.UserName;
            temp.Email = user.Email;
            temp.PhoneNumber = user.PhoneNumber;
            var response = await _userManager.CreateAsync(temp, pass);
            if (response.Succeeded)
            {
                return Ok(user);
            }
            return BadRequest(response.Errors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] string id) 
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Retourne 404 si l'utilisateur n'est pas trouvé
            }

            return Ok(user); // Retourne 200 OK avec les détails de l'utilisateur
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangePasswordAsync([FromRoute]string id, string currentPassword, string newPassword) 
        {
            IdentityUser temp = await _userManager.FindByIdAsync(id);
            if (temp == null) {
                return BadRequest("Problem avec le ID user not Found");
            }
            var result = await _userManager.ChangePasswordAsync(temp, currentPassword, newPassword);
            if (result.Succeeded) { Ok("Pass changed with success"); }
            return BadRequest(result.Errors);

        }
    }
}
