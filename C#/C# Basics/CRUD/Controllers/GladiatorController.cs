﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD.Data;
using CRUD.Models;

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
                        View("Index",await _context.GladiatorModel.Where(g => g.name.Contains(SearchPhrase)).ToListAsync()) :
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gladiator/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
