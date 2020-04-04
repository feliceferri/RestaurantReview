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
    public class RestaurantsController : ControllerBase
    {

        public readonly ApplicationDbContext _ApplicationDbContext;
        private readonly UserManager<Shared.DBModels.ApplicationUser> _userManager;
        private readonly SignInManager<Shared.DBModels.ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IWebHostEnvironment _env;

        public RestaurantsController(ApplicationDbContext ApplicationDbContext, UserManager<Shared.DBModels.ApplicationUser> userManager,
                                SignInManager<Shared.DBModels.ApplicationUser> signInManager, ILogger<LoginModel> logger,
                                 IWebHostEnvironment env)
        {
            _ApplicationDbContext = ApplicationDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _env = env;
        }


        [HttpGet]
        [Route("ByUserId/{UserId}")]
        public async Task<ActionResult<List<Restaurant>>> ByUserId(string UserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<Restaurant> res;

                List<string> Roles = await _ApplicationDbContext.UserRoles.Where(x => x.UserId == UserId).Select(x => x.Role.Name).ToListAsync();
                if (Roles.Contains("Owner"))
                {
                    res = await _ApplicationDbContext.Restaurants.Where(x => x.CreatedById == UserId).ToListAsync();
                }
                else
                {
                    //Admin && Users
                    res = await _ApplicationDbContext.Restaurants.ToListAsync();
                }

                return Ok(res);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(ByUserId));
                throw;
            }
        }

        [HttpGet]
        [Route("ByRestaurantId/{RestaurantId}/IncludeReviews")]
        public async Task<ActionResult<List<Restaurant>>> ByRestaurantId(Guid RestaurantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                
                var res = await _ApplicationDbContext.Restaurants.Where(x => x.Id == RestaurantId)
                                               .Include(x=> x.Reviews)
                                               .FirstOrDefaultAsync();
               
                if(res != null)
                {
                    res.Rating = await (from p in _ApplicationDbContext.Reviews
                                        where p.RestaurantId == RestaurantId
                                        select p).AverageAsync(x => x.Rating);
                }

                return Ok(res);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(ByUserId));
                throw;
            }
        }

        [HttpPost]
        [Route("New")]
        public async Task<ActionResult<Restaurant>> New(Shared.DTOModels.NewRestaurant model)
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

                if (model.ImageBase64 != null)
                {
                    var filePath = _env.WebRootPath + @"\Images\Restaurants\" + model.Id.ToString() + model.ImageFileExtensionIncludingDot;

                    byte[] bytes = Convert.FromBase64String(model.ImageBase64);

                    System.IO.File.WriteAllBytes(filePath, bytes); //If file exists its overwritten

                    model.ImageNameWithExtension = model.Id.ToString() + model.ImageFileExtensionIncludingDot;
                }

                model.Created = DateTime.Now;


                _ApplicationDbContext.Restaurants.Add(model);

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
                _logger.LogError(ex, nameof(ByUserId));
                throw;
            }
        }
    }
}