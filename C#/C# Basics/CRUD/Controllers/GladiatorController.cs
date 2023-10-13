using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Models;
using Microsoft.AspNetCore.Authorization;

namespace CRUD.Controllers
{
    public class GladiatorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GladiatorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gladiator
        public async Task<IActionResult> Index()
        {
            return _context.GladiatorModel != null ?
                        View(await _context.GladiatorModel.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
        }

        // GET: Gladiator/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return _context.GladiatorModel != null ?
                        View("ShowSearchForm") :
                        Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
        }

        // POST: Gladiator/ShowSearchResult
        public async Task<IActionResult> ShowSearchResult(String SearchPhrase)
        {
            return _context.GladiatorModel != null ?
                        View("Index", await _context.GladiatorModel.Where(g => g.name.Contains(SearchPhrase)).ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
        }

        // GET: Gladiator/ShowFightView
        public async Task<IActionResult> ShowFightView()
        {
            List<GladiatorModel> gladiators = await _context.GladiatorModel.ToListAsync();
            var duelModel = new DuelViewModel();
            duelModel.gladiatorsSelectList = new List<SelectListItem>();
            foreach (var gladiator in gladiators)
            {
                duelModel.gladiatorsSelectList.Add(new SelectListItem { Text = gladiator.name, Value = gladiator.id.ToString()});
            }
            return _context.GladiatorModel != null ?
                        View("ShowFightView", duelModel) :
                        Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
        }

        // POST: Gladiator/ProcessBattle
        public async Task<IActionResult> ProcessBattle(DuelViewModel duelViewModel)
        {
            //TODO fix passing value and parsing it to GladiatorModel var
            GladiatorModel gladiator1 = await _context.GladiatorModel.Where(g => g.id == Convert.ToInt64(duelViewModel.firstFighterID)).FirstAsync();
            GladiatorModel gladiator2 = await _context.GladiatorModel.Where(g => g.id == Convert.ToInt64(duelViewModel.secondFighterID)).FirstAsync();
            int attack1, attack2, hp1, hp2, turn;

            if (gladiator1.hasShield) attack2 = gladiator2.attack - 1;
            else attack2 = gladiator2.attack;
            if (gladiator2.hasShield) attack1 = gladiator1.attack - 1;
            else attack1 = gladiator1.attack;
            if (attack1 < 1)attack1 = 1;
            if (attack2 < 1)attack2 = 1;
            hp1 = gladiator1.health;
            hp2 = gladiator2.health;
            Random rnd = new Random();
            turn = rnd.Next(0, 2);
            while (hp1 > 0 && hp2 > 0)
            {
                if (turn % 2 == 0) hp2 -= attack1;
                else hp1 -= attack2;
            }
            if(hp1 > 0)
            {
            return _context.GladiatorModel != null ?
                View("Winner",gladiator1) :
                Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
            }
            else
            {
            return _context.GladiatorModel != null ?
                View("Winner", gladiator2) :
                Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
            }

        }

        // GET: Gladiator/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.GladiatorModel == null)
            {
                return NotFound();
            }

            var gladiatorModel = await _context.GladiatorModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (gladiatorModel == null)
            {
                return NotFound();
            }

            return View(gladiatorModel);
        }

        // GET: Gladiator/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gladiator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,weapon,attack,health,hasShield")] GladiatorModel gladiatorModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gladiatorModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gladiatorModel);
        }

        // GET: Gladiator/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.GladiatorModel == null)
            {
                return NotFound();
            }

            var gladiatorModel = await _context.GladiatorModel.FindAsync(id);
            if (gladiatorModel == null)
            {
                return NotFound();
            }
            return View(gladiatorModel);
        }

        // POST: Gladiator/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,weapon,attack,health,hasShield")] GladiatorModel gladiatorModel)
        {
            if (id != gladiatorModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gladiatorModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GladiatorModelExists(gladiatorModel.id))
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
            return View(gladiatorModel);
        }

        // GET: Gladiator/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.GladiatorModel == null)
            {
                return NotFound();
            }

            var gladiatorModel = await _context.GladiatorModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (gladiatorModel == null)
            {
                return NotFound();
            }

            return View(gladiatorModel);
        }

        // POST: Gladiator/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.GladiatorModel == null)
            {
                return Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
            }
            var gladiatorModel = await _context.GladiatorModel.FindAsync(id);
            if (gladiatorModel != null)
            {
                _context.GladiatorModel.Remove(gladiatorModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GladiatorModelExists(int id)
        {
          return (_context.GladiatorModel?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
