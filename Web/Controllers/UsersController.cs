using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shared.DBModels;
using Web.Data;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersController(ApplicationDbContext context, IWebHostEnvironment env,
                                UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = _context.Users.Include(x=> x.UserRoles).ThenInclude(x=> x.Role);
            return View(await users.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<ActionResult<ApplicationUser>> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser res = new ApplicationUser();

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            
            return View(res);
        }


        // GET: Restaurants/Create
        public IActionResult Create()
        {
            return View(new ApplicationUser_CreateUser());
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Web.ViewModels.ApplicationUser_CreateUser model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = model.User.Email;
                    user.Email = model.User.Email;
                    user.Name = model.User.Name;
                    user.CreatedDate = DateTime.Now;
                    

                    IdentityResult result = _userManager.CreateAsync(user, model.Password).Result;

                    if (result.Succeeded)
                    {
                        _userManager.AddToRoleAsync(user, model.RoleName).Wait();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Error", ex.Message);
                }
            }
            
            return View();
        }
        public async Task<ActionResult<ApplicationUserWithSingleRole>> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser user = await _context.Users.Where(x => x.Id == id)
                                   .Include(x => x.UserRoles).ThenInclude(x => x.Role).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            ViewData["Role"] = new SelectList(_context.Roles, "Name", "Name");

            ApplicationUserWithSingleRole userWithRole = new ApplicationUserWithSingleRole();
            userWithRole.User = user;
            userWithRole.RoleName = user.UserRoles.FirstOrDefault()?.Role?.Name;

            return View(userWithRole);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUserWithSingleRole model)
        {
            if (id != model.User.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(model);

                    var user = await _context.Users.Where(x => x.Id == model.User.Id).FirstOrDefaultAsync();
                    user.Name = model.User.Name;
                    user.Email = model.User.Email;
                    user.CreatedDate = model.User.CreatedDate;

                    if((await _userManager.IsInRoleAsync(user,model.RoleName)) == false)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.User.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(model);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Users
                .Include(x=> x.UserRoles).ThenInclude(x=> x.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var restaurant = await _context.Users.FindAsync(id);
            _context.Users.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }

}