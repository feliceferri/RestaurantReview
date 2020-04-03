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
using Microsoft.AspNetCore.Hosting;

namespace Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        public readonly ApplicationDbContext _ApplicationDbContext;
        private readonly UserManager<Shared.DBModels.ApplicationUser> _userManager;
        private readonly SignInManager<Shared.DBModels.ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public ReviewsController(ApplicationDbContext ApplicationDbContext, UserManager<Shared.DBModels.ApplicationUser> userManager,
                                SignInManager<Shared.DBModels.ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _ApplicationDbContext = ApplicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
         
        }

        [HttpPost]
        [Route("New")]
        public async Task<ActionResult<Shared.DBModels.Review>> New(Shared.DBModels.Review model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (model.Id == null)
                {
                    model.Id = Guid.NewGuid();
                }
                if (model.Created == null)
                {
                    model.Created = DateTime.Now;
                }


                _ApplicationDbContext.Reviews.Add(model);

                var result = _ApplicationDbContext.SaveChanges();

                if (result > 0)
                {
                    return Ok(model);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(New));
                throw;
            }
        }
    }
}