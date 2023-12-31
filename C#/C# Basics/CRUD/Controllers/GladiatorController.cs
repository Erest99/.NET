﻿using System;
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
                if(DateTime.Now > gladiator.restingTill) duelModel.gladiatorsSelectList.Add(new SelectListItem { Text = gladiator.name, Value = gladiator.id.ToString()});
            }
            return _context.GladiatorModel != null ?
                        View("ShowFightView", duelModel) :
                        Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
        }

        // POST: Gladiator/ProcessBattle
        public async Task<IActionResult> ProcessBattle(DuelViewModel duelViewModel)
        {
            GladiatorModel gladiator1 = await _context.GladiatorModel.Where(g => g.id == Convert.ToInt64(duelViewModel.firstFighterID)).FirstAsync();
            GladiatorModel gladiator2 = await _context.GladiatorModel.Where(g => g.id == Convert.ToInt64(duelViewModel.secondFighterID)).FirstAsync();
            GladiatorModel looser, winner;
            double attack1, attack2, losthp1, losthp2, turn;
            double health1, health2, speed1, speed2;
            speed1 = gladiator1.speed;
            speed2 = gladiator2.speed; 

            if (gladiator1.hasShield) { attack2 = Convert.ToDouble(gladiator2.attack) - Convert.ToDouble(gladiator1.defence) * 1.25; speed1 = (3/5) * speed1; }
            else attack2 = Convert.ToDouble(gladiator2.attack) - Convert.ToDouble(gladiator1.defence) * 0.75;
            if (gladiator2.hasShield) { attack1 = Convert.ToDouble(gladiator1.attack) - Convert.ToDouble(gladiator2.defence) * 1.25; speed2 = (3/5) * speed2; }
            else attack1 = Convert.ToDouble(gladiator1.attack) - Convert.ToDouble(gladiator2.defence) * 0.75;
            if (attack1 < 1)attack1 = 1;
            if (attack2 < 1)attack2 = 1;
            gladiator1.speed = Math.Max(Convert.ToInt32(Math.Ceiling(speed1)),1);
            gladiator2.speed = Math.Max(Convert.ToInt32(Math.Ceiling(speed2)),1);
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

        // GET: Gladiator/ShowRestingPage
        public async Task<IActionResult> ShowRestingPage()
        {
            List<GladiatorModel> gladiators = await _context.GladiatorModel.ToListAsync();
            var restModel = new RestViewModel();
            restModel.gladiatorsSelectList = new List<SelectListItem>();
            restModel.healthList = new List<(int, int, int)>();
            foreach (var gladiator in gladiators)
            {
                if (DateTime.Now > gladiator.restingTill)
                {
                    restModel.gladiatorsSelectList.Add(new SelectListItem { Text = gladiator.name, Value = gladiator.id.ToString() });
                    restModel.healthList.Add((gladiator.id, gladiator.maxhealth, gladiator.health));
                }
            }
            return _context.GladiatorModel != null ?
                        View("ShowRestingPage", restModel) :
                        Problem("Entity set 'ApplicationDbContext.GladiatorModel'  is null.");
        }

        // POST: Gladiator/Rest
        public async Task<IActionResult> Rest(RestViewModel restModel)
        {
            GladiatorModel gladiator = await _context.GladiatorModel.Where(g => g.id == Convert.ToInt64(restModel.id)).FirstAsync();
            gladiator.health = gladiator.maxhealth;
            gladiator.restingTill = DateTime.Now.AddMinutes(5);
            await _context.SaveChangesAsync();
            return _context.GladiatorModel != null ?
                View("Index", await _context.GladiatorModel.ToListAsync()) :
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
            GladiatorInit(gladiatorModel);
            if (ModelState.IsValid)
            {
                var skillresult = GladiatorCheck(gladiatorModel);
                if (skillresult.Item1)
                {
                    _context.Add(gladiatorModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                TempData["AlertMessage"] = "Not enough skill points, need "+skillresult.Item2.ToString() + " got " + skillresult.Item3.ToString();
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
        public async Task<IActionResult> Edit(int id, [Bind("id,name,weapon,attack,speed,defence,health,maxhealth,hasShield,level,xp,xptolevel,lastrested")] GladiatorModel gladiatorModel)
        {
            if (id != gladiatorModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var skillresult = GladiatorCheck(gladiatorModel);
                if (skillresult.Item1)
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
                TempData["AlertMessage"] = "Not enough skill points, need " + skillresult.Item2.ToString() + " got " + skillresult.Item3.ToString();
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
            gladiator.id = _context.GladiatorModel.Count()+1;
            gladiator.health = gladiator.maxhealth;
            gladiator.xp = 0;
            gladiator.xptolevel = 1000;
            gladiator.level = 1;
            gladiator.restingTill = DateTime.MinValue;
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

        private (bool,int,int) GladiatorCheck(GladiatorModel gladiator)
        {
            double allowedskillpoints = 9 + gladiator.level;
            double neededskillpoints = Convert.ToDouble(gladiator.health) / 3 + gladiator.attack + gladiator.defence + gladiator.speed;
            if (allowedskillpoints >= neededskillpoints) return (true, Convert.ToInt32(neededskillpoints), Convert.ToInt32(allowedskillpoints));
            else return (false, Convert.ToInt32(neededskillpoints), Convert.ToInt32(allowedskillpoints));
        }
    }
}
