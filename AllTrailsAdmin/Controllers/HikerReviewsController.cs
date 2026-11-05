
using AllTrailsAdmin;
using AllTrailsAdmin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "hiker")]
public class HikerReviewsController : Controller
{
    private readonly AllTrailsAdminContext _context;

    // The logged in user
    private string? _userId;

    // Constructor
    public HikerReviewsController(AllTrailsAdminContext context)
    {
        _context = context;
    }

    // User is not available when contructor is called.
    // OnActionExecuting() is called after constructor is called, but before action method is called.
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        _userId = User.GetUserId();
    }

    // GET: HIKERREVIEWS
    public async Task<IActionResult> Index()    
    {
        // Get reviews for the logged in user (My Reviews)
        var reviews = await _context.HikerReview
            .Where(m => m.CreatedBy == _userId)
            .Include(m => m.Trail)
            .ToListAsync();

        return View(reviews);
    }

    // GET: HIKERREVIEWS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hikerreview = await _context.HikerReview
            .Where(m => m.CreatedBy == _userId)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (hikerreview == null)
        {
            return NotFound();
        }

        return View(hikerreview);
    }

    // GET: HIKERREVIEWS/Create
    public IActionResult Create(int trailId)
    {
        ViewBag.TrailId = trailId;

        return View();
    }

    // POST: HIKERREVIEWS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,TrailId,Description,Rating,IsPublished")] HikerReview hikerReview)
    {
        // Initialize some fields
        hikerReview.CreatedBy = _userId;
        hikerReview.CreatedDate = DateTime.Now;

        if (ModelState.IsValid)
        {
            _context.Add(hikerReview);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(hikerReview);
    }

    // GET: HIKERREVIEWS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hikerreview = await _context.HikerReview.FindAsync(id);
        if (hikerreview == null)
        {
            return NotFound();
        }
        return View(hikerreview);
    }

    // POST: HIKERREVIEWS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,TrailId,Description,Rating,IsPublished,CreatedBy,CreatedDate")] HikerReview hikerreview)
    {
        if (id != hikerreview.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(hikerreview);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HikerReviewExists(hikerreview.Id))
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
        return View(hikerreview);
    }

    // GET: HIKERREVIEWS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hikerreview = await _context.HikerReview
            .FirstOrDefaultAsync(m => m.Id == id);
        if (hikerreview == null)
        {
            return NotFound();
        }

        return View(hikerreview);
    }

    // POST: HIKERREVIEWS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var hikerreview = await _context.HikerReview.FindAsync(id);
        if (hikerreview != null)
        {
            _context.HikerReview.Remove(hikerreview);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HikerReviewExists(int? id)
    {
        return _context.HikerReview.Any(e => e.Id == id);
    }
}
