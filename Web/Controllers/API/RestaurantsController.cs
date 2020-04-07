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
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers.API
{
    [AllowAnonymous]
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


        private class RestaurantWithAverageRating
        {
            public Restaurant Restaurant { get; set; }
            public Double? Rating { get; set; }
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
                List<RestaurantWithAverageRating> preRes;
                List<Restaurant> res = null;

                List<string> Roles = await _ApplicationDbContext.UserRoles.Where(x => x.UserId == UserId).Select(x => x.Role.Name).ToListAsync();
                if (Roles.Contains("Owner"))
                {
                    //res = await _ApplicationDbContext.Restaurants.Where(x => x.CreatedById == UserId).ToListAsync();
                    preRes = await (from p in _ApplicationDbContext.Restaurants
                                      orderby p.Reviews.Average(x => x.Rating) descending
                                      select new RestaurantWithAverageRating() { Restaurant = p, Rating = p.Reviews.Average(x => x.Rating) }).ToListAsync();

                }
                else
                {
                    //Admin && Users
                    //res = await _ApplicationDbContext.Restaurants.ToListAsync();
                    preRes = await (from p in _ApplicationDbContext.Restaurants
                                       orderby p.Reviews.Average(x => x.Rating) descending
                                      select new RestaurantWithAverageRating() { Restaurant = p, Rating = p.Reviews.Average(x => x.Rating) }).Take(50).ToListAsync();
                }

                if(preRes != null)
                {
                    res = new List<Restaurant>();
                    foreach (var item in preRes)
                    {
                        item.Restaurant.Rating =  item.Rating.GetValueOrDefault();
                        res.Add(item.Restaurant);
                    }
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
        [Route("ByUserId/{UserId}/FilterByRating/{Rating}")]
        public async Task<ActionResult<List<Restaurant>>> ByUserId(string UserId, int Rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                List<RestaurantWithAverageRating> preRes;
                List<Restaurant> res = null;

                List<string> Roles = await _ApplicationDbContext.UserRoles.Where(x => x.UserId == UserId).Select(x => x.Role.Name).ToListAsync();
                if (Roles.Contains("Owner"))
                {
                    //res = await _ApplicationDbContext.Restaurants.Where(x => x.CreatedById == UserId).ToListAsync();
                    preRes = await (from p in _ApplicationDbContext.Restaurants
                                    where p.Reviews.Average(x => x.Rating) >= Rating && p.Reviews.Average(x => x.Rating) < (Rating + 0.99)
                                    orderby p.Reviews.Average(x => x.Rating) descending
                                    select new RestaurantWithAverageRating() { Restaurant = p, Rating = p.Reviews.Average(x => x.Rating) }).ToListAsync();

                }
                else
                {
                    //Admin && Users
                    //res = await _ApplicationDbContext.Restaurants.ToListAsync();
                    preRes = await (from p in _ApplicationDbContext.Restaurants
                                    where p.Reviews.Average(x => x.Rating) >= Rating && p.Reviews.Average(x => x.Rating) < (Rating + 0.99)
                                    orderby p.Reviews.Average(x => x.Rating) descending
                                    select new RestaurantWithAverageRating() { Restaurant = p, Rating = p.Reviews.Average(x => x.Rating) }).Take(50).ToListAsync();
                }

                if (preRes != null)
                {
                    res = new List<Restaurant>();
                    foreach (var item in preRes)
                    {
                        item.Restaurant.Rating = item.Rating.GetValueOrDefault();
                        res.Add(item.Restaurant);
                    }
                }

                return Ok(res);
            }
            catch (Exception ex)
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

            List<Review> Reviews = new List<Review>();

            try
            {
                
                var res = await _ApplicationDbContext.Restaurants.Where(x => x.Id == RestaurantId)
                                                    .FirstOrDefaultAsync();
               
                if(res != null)
                {
                    var HighestReview = await (from p in _ApplicationDbContext.Reviews
                                         where p.RestaurantId == RestaurantId
                                         orderby p.Rating descending, p.Created descending
                                               select p).Take(1).FirstOrDefaultAsync();

                    if (HighestReview != null)
                    {
                        HighestReview.TypeOfReview = "Best:";
                        Reviews.Add(HighestReview);
                    }
                        
                    var LowestReview = await (from p in _ApplicationDbContext.Reviews
                                               where p.RestaurantId == RestaurantId
                                               orderby p.Rating ascending, p.Created descending
                                              select p).Take(1).FirstOrDefaultAsync();

                    if (LowestReview != null && HighestReview.Id != LowestReview.Id)
                    {
                        LowestReview.TypeOfReview = "Worst:";
                        Reviews.Add(LowestReview);
                    }

                    var LastReview = await (from p in _ApplicationDbContext.Reviews
                                              where p.RestaurantId == RestaurantId
                                              orderby  p.Created descending
                                              select p).Take(1).FirstOrDefaultAsync();

                    if (LastReview != null && LastReview.Id != HighestReview.Id && LastReview.Id != LowestReview.Id)
                    {
                        LastReview.TypeOfReview = "Last:";
                        Reviews.Add(LastReview);
                    }

                    if (Reviews != null && Reviews.Count > 0)  //If there are no reviews it will throw an error
                    {
                        res.Rating = await (from p in _ApplicationDbContext.Reviews
                                            where p.RestaurantId == RestaurantId
                                            select p)?.AverageAsync(x => x.Rating);
                    }

                    res.Reviews = Reviews;
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
        [Route("ForOwners/ByRestaurantId/{RestaurantId}/IncludeReviewsPendingToReply")]
        public async Task<ActionResult<List<Restaurant>>> ForOwnersByRestaurantId_IncludeReviewsPendingToReply(Guid RestaurantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            try
            {

                var res = await _ApplicationDbContext.Restaurants.Where(x => x.Id == RestaurantId)
                                                    .FirstOrDefaultAsync();

                if (res != null)
                {
                    res.Reviews = await (from p in _ApplicationDbContext.Reviews
                                         where p.RestaurantId == RestaurantId && p.ReplyByTheOwner == null
                                         orderby p.Created descending
                                         select p).ToListAsync();

                    if (res.Reviews != null && res.Reviews.Count > 0)  //If there are no reviews it will throw an error
                    {
                        res.Rating = await (from p in _ApplicationDbContext.Reviews
                                            where p.RestaurantId == RestaurantId
                                            select p).AverageAsync(x => x.Rating);
                    }

                    if(res.Reviews != null)
                    {
                        foreach(var item in res.Reviews)
                        {
                            item.TypeOfReview = "Pending: ";
                        }
                    }
                    
                }

                return Ok(res);
            }
            catch (Exception ex)
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