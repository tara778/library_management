using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using library_management.Data;
using library_management.Models;

namespace library_management.Controllers
{
    public class LoanController : Controller
    {
        private readonly NeondbContext _context;

        public LoanController(NeondbContext context)
        {
            _context = context;
        }

        // GET: Loan
        public async Task<IActionResult> Index()
        {
            var neondbContext = _context.Loans.Include(l => l.Book).Include(l => l.Member);
            return View(await neondbContext.ToListAsync());
        }

        // GET: Loan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // GET: Loan/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FirstName");
            return View();
        }

        // POST: Loan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,MemberId,LoanDate,DueDate,ReturnDate,FineAmount")] Loan loan)
        {
            Console.WriteLine("Create action hit");
            Console.WriteLine($"Book Data: BookId={loan.BookId}, MemberId={loan.MemberId}, LoanDate={loan.LoanDate}, DueDate={loan.DueDate}, ReturnDate={loan.ReturnDate}, FineAmount={loan.FineAmount}");

            if (ModelState.IsValid)
            {
                try
                {
                    loan.LoanDate = DateTime.SpecifyKind(loan.LoanDate, DateTimeKind.Utc);
                    loan.DueDate = DateTime.SpecifyKind(loan.DueDate, DateTimeKind.Utc);
                    if (loan.ReturnDate.HasValue)
                    {
                        loan.ReturnDate = DateTime.SpecifyKind(loan.ReturnDate.Value, DateTimeKind.Utc);
                    }

                    _context.Add(loan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving loan: {ex.Message}");
                    ModelState.AddModelError("", $"Error saving loan: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Model state is invalid");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["MemberId"] = new SelectList(
                _context.Members.Select(m => new { m.Id, FullName = m.FirstName + " " + m.LastName }),
                "Id", "FullName", loan.MemberId);

            return View(loan);
        }

        // GET: Loan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "Email", loan.MemberId);
            return View(loan);
        }

        // POST: Loan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,MemberId,LoanDate,DueDate,ReturnDate,FineAmount")] Loan loan)
        {
            if (id != loan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure all DateTime values are in UTC
                    loan.LoanDate = DateTime.SpecifyKind(loan.LoanDate, DateTimeKind.Utc);
                    loan.DueDate = DateTime.SpecifyKind(loan.DueDate, DateTimeKind.Utc);
                    if (loan.ReturnDate.HasValue)
                    {
                        loan.ReturnDate = DateTime.SpecifyKind(loan.ReturnDate.Value, DateTimeKind.Utc);
                    }

                    _context.Update(loan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LoanExists(loan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // Repopulate dropdowns in case of validation errors
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", loan.BookId);
            ViewData["MemberId"] = new SelectList(_context.Members, "Id", "FirstName", loan.MemberId);
            return View(loan);
        }

        // GET: Loan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loan = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        // POST: Loan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan != null)
            {
                _context.Loans.Remove(loan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LoanExists(int id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}
