using esok.api.Application.Common.Enum;
using esok.api.Data;
using esok.api.DTO;
using esok.api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net;

namespace esok.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailService;
        private readonly IAuthenticationService _authenticationService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountController(
            ILogger<AccountController> logger,
            IEmailService emailSender,
            IAuthenticationService authAuthenticationService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _emailService = emailSender;
            _authenticationService = authAuthenticationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] ApplicationUserDTO applicationUser)
        {
            var user = new ApplicationUser
            {
                UserName = applicationUser.Email,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                NameSurname = applicationUser.NameSurname
            };

            var roleExists = await _roleManager
                    .RoleExistsAsync(Role.Manager.ToString());

            if (!roleExists)
                return NotFound("Role");

            var result = await _userManager
                .CreateAsync(user, applicationUser.Password);

            if (result.Succeeded)
            {
                await _userManager
                    .AddToRoleAsync(user, Role.Manager.ToString());

                var token = await _userManager
                    .GenerateEmailConfirmationTokenAsync(user);

                _emailService
                    .SendRegisterConfirmEmail(user.Email, token);

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ApplicationUserConfirmDTO applicationUserConfirm)
        {
            var user = await _userManager
                .FindByEmailAsync(applicationUserConfirm.Email);

            if (user == null)
                return NotFound();

            var result = await _userManager
                .ConfirmEmailAsync(user, applicationUserConfirm.Token);

            if (result.Succeeded)
            {
                await CreateGroup(user.Id);
                return Ok();
            }
                
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] ApplicationUserSignInDTO applicationUserSignIn)
        {
            var user = await _userManager
                .FindByEmailAsync(applicationUserSignIn.Email);

            if (user == null)
                return NotFound();

            if (user.Active == false)
                return Forbid();

            var result = await _signInManager
                .PasswordSignInAsync(user, applicationUserSignIn.Password, false, false);

            if (result.Succeeded)
            {
                var token = await _authenticationService
                    .CreateToken(user);

                return Ok(token);
            }

            return NotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            if (user == null)
                return NotFound();

            var token = await _userManager
                .GeneratePasswordResetTokenAsync(user);

            _emailService
                .SendResetPasswordEmail(user.Email, token);

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            var user = await _userManager
                .FindByEmailAsync(resetPassword.Email);

            if (user == null)
                return NotFound();

            var result = await _userManager
                .ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);

            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword( [FromBody] PasswordDTO password)
        {
            var user = await _authenticationService
                .GetUser();

            var result = await _userManager
                .ChangePasswordAsync(user, password.CurrentPassword, password.NewPassword);

            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] ApplicationUserDTO applicationUser)
        {
            var userExist = await _userManager
                .FindByEmailAsync(applicationUser.Email);

            if (userExist != null)
                return StatusCode((int)HttpStatusCode.Conflict);

            var user = new ApplicationUser
            {
                UserName = applicationUser.Email,
                Email = applicationUser.Email,
                PhoneNumber = applicationUser.PhoneNumber,
                NameSurname = applicationUser.NameSurname
            };

            var roleExists = await _roleManager
                .RoleExistsAsync(Role.Employee.ToString());

            if (!roleExists)
                return NotFound("Role");

            var result = await _userManager
                .CreateAsync(user, Guid.NewGuid().ToString());

            if (result.Succeeded)
            {
                await _userManager
                    .AddToRoleAsync(user, Role.Employee.ToString());

                var token = await _userManager
                    .GenerateEmailConfirmationTokenAsync(user);

                _emailService
                    .SendRegisterConfirmEmailToEmployee(user.Email, token);

                var manager = await _authenticationService
                    .GetUser();

                var managerGroup = _applicationDbContext
                    .Group
                    .Where(x => x.ManagerId == manager.Id && x.Active == true)
                    .FirstOrDefault();

                var newGroupId = 0;
                if (managerGroup == null)
                {
                    newGroupId = await CreateGroup(manager.Id);
                }

                var groupId = managerGroup != null ? managerGroup.Id : newGroupId;

                var emplpoyee = await _userManager
                    .FindByEmailAsync(user.Email);

                await _applicationDbContext
                    .UserGroup
                    .AddAsync(new UserGroup()
                    {
                        GroupId = groupId,
                        EmployeeId = emplpoyee.Id,
                        CreateDate = DateTime.UtcNow,
                        CreatedByUserId = manager.Id,
                        Active = true
                    });

                await _applicationDbContext
                    .SaveChangesAsync();

                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ConfirmEmployee")]
        public async Task<IActionResult> ConfirmEmployee([FromBody] ConfirmEmployeeDTO confirmEmployee)
        {
            var user = await _userManager
                .FindByEmailAsync(confirmEmployee.Email);

            if (user == null)
                return NotFound();

            var result = await _userManager
                .ConfirmEmailAsync(user, confirmEmployee.Token);

            if (result.Succeeded)
            {
                var token = await _userManager
                    .GeneratePasswordResetTokenAsync(user);

                var password = await _userManager
                    .ResetPasswordAsync(user, token, confirmEmployee.Password);

                if (password.Succeeded)
                    return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            var user = await _authenticationService
                .GetUser();

            var emplyeeIds =
                (from gr in _applicationDbContext.Group.AsQueryable()
                 join ugr in _applicationDbContext.UserGroup.AsQueryable() on gr.Id equals ugr.GroupId
                 where gr.ManagerId== user.Id
                 select new UserGroup
                 {
                     EmployeeId = ugr.EmployeeId
                 }.EmployeeId)
                 .ToList();

            var employees = _applicationDbContext
                .Users
                .Where(x => emplyeeIds.Contains(x.Id) && x.Active == true)
                .Select(y => new EmployeeDTO
                {
                    Id = y.Id,
                    NameSurname = y.NameSurname,
                    Email= y.Email,
                    PhoneNumber= y.PhoneNumber,
                    Confirmed = y.EmailConfirmed
                }).ToList();

            return Ok(employees);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int employeeId) 
        {
            var user = await _authenticationService
                .GetUser();

            var isManagerEmployee =
                (from gr in _applicationDbContext.Group.AsQueryable()
                join ugr in _applicationDbContext.UserGroup.AsQueryable() on gr.Id equals ugr.GroupId
                where gr.ManagerId == user.Id && ugr.EmployeeId == employeeId
                select ugr)
                .Any();

            if (!isManagerEmployee)
                return Forbid();

            var employee = _applicationDbContext
                .Users
                .Where(x => x.Id == employeeId && x.Active == true)
                .First();

            employee.Active= false;

            _applicationDbContext.Update(employee);
            _applicationDbContext.SaveChanges();

            return Ok();
        }

        private async Task<int> CreateGroup(int managerId)
        {
            var group = await _applicationDbContext
                        .Group
                        .AddAsync(new Group()
                        {
                            ManagerId = managerId,
                            CreateDate = DateTime.UtcNow,
                            CreatedByUserId = managerId,
                            Active = true
                        });

            await _applicationDbContext
                .SaveChangesAsync();

            return group.Entity.Id;
        }
    }
}
