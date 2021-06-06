using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using URL_Shortner_Challenge.Data;
using URL_Shortner_Challenge.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace URL_Shortner_Challenge.Controllers
{
    public class ShortLinksController : Controller
    {
        private readonly ApplicationDbContext _context;


        public ShortLinksController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: ShortLinks
        // Shows the links created by the currently loggend in user.
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShortLink.ToListAsync());
        }


        // GET: ShortLinks/ShowSearchForm.
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }


        // POST: ShortLinks/ShowSearchResults.
        // Shows index filtered by search phrase and current user.
        // Function only visible to logged in users.
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //shortLinks where the entered link matches the search AND the user IDs match.
            return View("Index", await _context.ShortLink.Where(j => j.entered.Contains(SearchPhrase) && j.UserID == userId).ToListAsync());
        }


        //Used to create short term and permanent links.
        private ShortLink CreateLink(bool permanent, string passedUrl)
        {
            //Uses LINQ and lamda's to extract the shortened links from all the currently loaded links.
            List<ShortLink> viableLinks = _context.ShortLink.Where(j => j.expired > DateTime.Now).ToList();
            List<string> shortenedLinks = (from lnk in viableLinks select lnk.returned).ToList();

            ShortLink newLink;

            if (permanent) // Passes the user ID to a diffrent constructor that removes the expiration date.
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                newLink = new ShortLink(shortenedLinks, passedUrl, HttpContext, userId);
            }
            else // With no user ID the links expiration date is set (currently 1 year)
            {
                newLink = new ShortLink(shortenedLinks, passedUrl, HttpContext);
            }

            return newLink;
        }


        // Get: /l/{Link} 	{host}/l/ZhIKcCb28K-
        //OPen the link passed through the /l/{passedLink} endpoint in startup.
        public async Task<IActionResult> Open(string passedLink)
        {
            foreach (ShortLink link in _context.ShortLink)
            {
                if(link != null)
                {
                    //Gets the last few characters from the saved short link.
                    //This means the links should work even if the host changes.
                    if (passedLink == link.returned.Substring(link.returned.Length - 11)){
                        return Redirect(link.entered);
                    }
                }
                
            }
            return View("Index"); //ToDo: return to error page.
        }

        // GET: ShortLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shortLink = await _context.ShortLink
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shortLink == null)
            {
                return NotFound();
            }

            return View(shortLink);
        }

        // GET: ShortLinks/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // Called from the home index page when a user is not logged in.
        public async Task<IActionResult> CreateTemp(string passedUrl)
        {
            ShortLink newLink = CreateLink(false, passedUrl);
            _context.Add(newLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: ShortLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("entered")] ShortLink shortLink)
        {
            if (ModelState.IsValid)
            {
                shortLink = CreateLink(true, shortLink.entered);
                _context.Add(shortLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shortLink);
        }

        // GET: ShortLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shortLink = await _context.ShortLink.FindAsync(id);
            if (shortLink == null)
            {
                return NotFound();
            }
            return View(shortLink);
        }

        // POST: ShortLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,entered,returned,created")] ShortLink shortLink)
        {
            if (id != shortLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shortLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShortLinkExists(shortLink.Id))
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
            return View(shortLink);
        }

        // GET: ShortLinks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shortLink = await _context.ShortLink
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shortLink == null)
            {
                return NotFound();
            }

            return View(shortLink);
        }

        // POST: ShortLinks/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shortLink = await _context.ShortLink.FindAsync(id);
            _context.ShortLink.Remove(shortLink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShortLinkExists(int id)
        {
            return _context.ShortLink.Any(e => e.Id == id);
        }
    }
}
