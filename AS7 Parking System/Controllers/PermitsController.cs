using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AS7_Parking_System.Models;
using Microsoft.OData.Edm;

namespace AS7_Parking_System.Controllers
{
    public class PermitsController : Controller
    {
        private readonly ParkingDataBaseContext _context;

        public PermitsController(ParkingDataBaseContext context)
        {
            _context = context;
        }

        // GET: Permits
        public async Task<IActionResult> Index()
        {
            var parkingDataBaseContext = _context.Permit.Include(p => p.Vehicle);
            Fee();
            return View(await parkingDataBaseContext.ToListAsync());
        }

        public IActionResult Fee()
        {
            var permits = _context.Permit.Include(p => p.Vehicle).ToList();

            foreach (var permit in permits)
            {
                if (DateTime.Now < permit.PermitEndDate)
                {
                    permit.Valid = true;
                    permit.Fee = 0;
                    permit.Premium = 0;

                }
                else if (DateTime.Now > permit.PermitEndDate && DateTime.Now > permit.PermitEndDate.AddDays(10))
                {
                    permit.Fee = 200;
                    permit.Premium = 20;
                    permit.Valid = false;
                }
                else
                {
                    permit.Fee = 200;
                    permit.Premium = 0;
                    permit.Valid = false;
                }

            }

            return View();
        }

        // GET: Permits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await _context.Permit
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(m => m.PermitId == id);
            if (permit == null)
            {
                return NotFound();
            }

            return View(permit);
        }

        // GET: Permits/Create
        public IActionResult Create()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id");
            return View();
        }

        // POST: Permits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PermitId,VehicleId,PermitStartDate,PermitEndDate,Valid,Fee,Premium")] Permit permit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(permit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", permit.VehicleId);
            return View(permit);
        }

        // GET: Permits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await _context.Permit.FindAsync(id);
            if (permit == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", permit.VehicleId);
            return View(permit);
        }

        // POST: Permits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PermitId,VehicleId,PermitStartDate,PermitEndDate,Valid,Fee,Premium")] Permit permit)
        {
            if (id != permit.PermitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(permit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PermitExists(permit.PermitId))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", permit.VehicleId);
            return View(permit);
        }

        // GET: Permits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var permit = await _context.Permit
                .Include(p => p.Vehicle)
                .FirstOrDefaultAsync(m => m.PermitId == id);
            if (permit == null)
            {
                return NotFound();
            }

            return View(permit);
        }

        // POST: Permits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var permit = await _context.Permit.FindAsync(id);
            _context.Permit.Remove(permit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PermitExists(int id)
        {
            return _context.Permit.Any(e => e.PermitId == id);
        }
    }
}
