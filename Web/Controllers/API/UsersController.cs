using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Web.Areas.Identity.Pages.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.DTOModels;
using Web.Data;
using Shared.DBModels;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Web.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly ApplicationDbContext _ApplicationDbContext;
        private readonly UserManager<Shared.DBModels.ApplicationUser> _userManager;
        private readonly SignInManager<Shared.DBModels.ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public UsersController(ApplicationDbContext ApplicationDbContext, UserManager<Shared.DBModels.ApplicationUser> userManager,
                                SignInManager<Shared.DBModels.ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _ApplicationDbContext = ApplicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


      
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<LoggedUser>> Register(Shared.DTOModels.RegisteringUser model)
        {
            var User = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                EmailConfirmed = true,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(User, model.Password);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            if (result.Succeeded)
            {
                foreach (string role in model.Roles)
                {
                    await _userManager.AddToRoleAsync(User, role);
                }
                
                return Ok(new LoggedUser() { ID = User.Id, Name = User.Name,
                               Roles = await _userManager.GetRolesAsync(User)
                }); ; ;
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoggedUser>> Login(Shared.DTOModels.LoginUser model)
        {
         
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);

                    var newUser = new LoggedUser() {
                        ID = user.Id,
                        Name = user.Name
                    };

                    newUser.Roles = (await _userManager.GetRolesAsync(user)); 

                    if(user.Blocked.HasValue )
                    {
                        ModelState.AddModelError("Blocked", "User account has been blocked.");
                        return BadRequest(ModelState);
                    }

                    return Ok(newUser);
                }

                if (result.RequiresTwoFactor)
                {
                    ModelState.AddModelError("RequiresTwoFactor", "Requires Two Factor Authentication");
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                     ModelState.AddModelError("LockedOut", "User account locked out.");
                }
                else
                {
                    var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError("NonRegisteredUser", "Non Registered User");
                    }
                    //else if (await _signInManager.UserManager.IsEmailConfirmedAsync(user) == false)
                    //{
                    //    ModelState.AddModelError("NeedsToConfirmEmail", "Needs to confirm Email");
                    //}
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                   
                }
            }
                    
           return BadRequest(ModelState);

        }

       
        
        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult<Shared.DTOModels.LoggedUser>> Update(Shared.DTOModels.LoggedUser model, string UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _ApplicationDbContext.Users.Where(s => s.Id == model.ID).FirstOrDefault();

            if (existingUser != null)
            {

                existingUser.UpdatedDate = DateTime.Now;
                existingUser.UpdatedBy = UserId;
                existingUser.Name = model.Name;
                existingUser.Blocked = model.Blocked;
                existingUser.BlockedBy = model.BlockedBy;
                                
                _ApplicationDbContext.SaveChanges();
            }
            else
            {
                return NotFound();
            }


            return Ok(model);
        }


   
    }
}