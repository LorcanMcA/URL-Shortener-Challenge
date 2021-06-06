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
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.ShortLink.Where(j => j.entered.Contains(SearchPhrase)).ToListAsync());
        }


        // POST: ShortLinks/ShowSearchResults.
        public async Task<IActionResult> createTempLink(string passedUrl)
        {
            List<ShortLink> links = _context.ShortLink.Where(j => j.expired < DateTime.Now).ToList();
            List<string> shortendedLinks = new List<string>();
            foreach (ShortLink lnk in links)
            {
                shortendedLinks.Add(lnk.returned);
            }

            ShortLink newLink = new ShortLink(shortendedLinks, passedUrl, HttpContext);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //SignInManager<IdentityUser> man = new Microsoft.AspNetCore.Identity.SignInManager<IdentityUser>();


            _context.Add(newLink);
            await _context.SaveChangesAsync();

            return View(newLink);
        }

        // Get: /l/{Link} 	https://localhost:44345/l/ZhIKcCb28K-
        public async Task<IActionResult> Open(string passedLink)
        {
            foreach (ShortLink link in _context.ShortLink)
            {
                if(link != null)
                {
                    if (passedLink == link.returned.Substring(link.returned.Length - 11)){
                        return Redirect(link.entered);
                    }
                }
                
            }
            return View("Index");
            //_context.ShortLink.Where(j => j.returned.Substring(j.returned.Length - 11) == shortlink).First().returned.ToString();
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

        // POST: ShortLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,entered,returned,created")] ShortLink shortLink)
        {
            if (ModelState.IsValid)
            {
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
