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
            GladiatorModel looser, winner;
            double attack1, attack2, losthp1, losthp2, turn;
            double health1, health2;

            if (gladiator1.hasShield) attack2 = Convert.ToDouble(gladiator2.attack) - Convert.ToDouble(gladiator1.defence)*1.25;
            else attack2 = Convert.ToDouble(gladiator2.attack) - Convert.ToDouble(gladiator1.defence) * 0.75;
            if (gladiator2.hasShield) attack1 = Convert.ToDouble(gladiator1.attack) - Convert.ToDouble(gladiator2.defence)*1.25;
            else attack1 = Convert.ToDouble(gladiator1.attack) - Convert.ToDouble(gladiator2.defence) * 0.75;
            if (attack1 < 1)attack1 = 1;
            if (attack2 < 1)attack2 = 1;
            Random rnd = new Random();
            double speedCoef = gladiator1.speed / (gladiator1.speed + gladiator2.speed);
            double luck = rnd.NextDouble();
            if (luck < speedCoef) turn = 0;
            else turn = 1;
            losthp1 = 0;
            losthp2 = 0;
            health1 = gladiator1.health;
            health2 = gladiator2.health;
            while (health1 > 0 && health2 > 0)
            {
                if (turn % 2 == 0) {health2 -= attack1; losthp2 += attack1; }
                else {health1 -= attack2; losthp1 += attack2; }
                turn++;
            }
            gladiator1.health = Convert.ToInt32(Math.Ceiling(health1));
            gladiator2.health = Convert.ToInt32(Math.Ceiling(health2));
            if (gladiator1.health > 0) { looser = gladiator2; winner = gladiator1; }
            else {looser = gladiator1; winner = gladiator2; }
            winner = GladiatorGiveXP(winner, looser);
            _context.GladiatorModel.Remove(looser);
            _context.Update(winner);
            await _context.SaveChangesAsync();
            return _context.GladiatorModel != null ?
                View("Winner", winner) :
                Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
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
        public async Task<IActionResult> Create([Bind("id,name,weapon,attack,speed,defence,health,maxhealth,hasShield,level,xp,xptolevel,lastrested")] GladiatorModel gladiatorModel)
        {
            if (ModelState.IsValid)
            {
                GladiatorInit(gladiatorModel);
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
        public async Task<IActionResult> Edit(int id, [Bind("id,name,weapon,attack,speed,defence,health,maxhealth,hasShield")] GladiatorModel gladiatorModel)
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

        private GladiatorModel GladiatorInit(GladiatorModel gladiator)
        {
            gladiator.maxhealth = gladiator.health;
            gladiator.xp = 0;
            gladiator.xptolevel = 1000;
            gladiator.level = 1;
            gladiator.lastrested = DateTime.MinValue;
            return gladiator;
        }

        private GladiatorModel GladiatorGiveXP(GladiatorModel winner, GladiatorModel loser)
        {
            double xpratio = loser.level / winner.level;
            double xptoreceive = 500 * xpratio;
            double xptolevel;
            winner.xp += Convert.ToInt32(Math.Ceiling(xptoreceive));
            while(winner.xp >= winner.xptolevel)
            {
                winner.xp -= winner.xptolevel;
                xptolevel = winner.xptolevel * 1.25;
                winner.xptolevel = Convert.ToInt32(Math.Ceiling(xptolevel));
                winner.level += 1;
            }
            return winner;
        }
    }
}
