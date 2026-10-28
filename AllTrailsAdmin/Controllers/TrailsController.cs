using AllTrailsAdmin.Models;
using AllTrailsAdmin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class TrailsController : Controller
{
    private readonly AllTrailsAdminContext _context;
    private readonly AzureStorageService _blobService;

    public TrailsController(AllTrailsAdminContext context, AzureStorageService blobService)
    {
        _context = context;
        _blobService = blobService;
    }

    // GET: TRAILS
    public async Task<IActionResult> Index()    
    {
        // Retrieve all trails from the database
        List<Trail> trails = await _context.Trail
            .Include(m => m.Category)
            .ToListAsync();

        return View(trails);
    }

    // GET: TRAILS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        // Check if the id parameter is null from URL
        if (id == null)
        {
            return NotFound(); // return a 404 Not Found response if the id is null
        }

        // Retrieve the trail with the specified id from the database
        var trail = await _context.Trail
            .Include(m => m.Category)
            .FirstOrDefaultAsync(m => m.Id == id);

        // Check if the trail exists in the database
        if (trail == null)
        {
            return NotFound();
        }

        return View(trail);
    }

    // GET: TRAILS/Create
    [Authorize(Roles = "administrator")]
    public IActionResult Create()
    {
        // create a select list for the categories
        var categories = _context.Category.ToList();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Title");

        return View();
    }

    /*
     *  TO-DO: Handle image being required
     */


    // POST: TRAILS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> Create([Bind("Id,Title,Description,Length,ElevationGain,Location,CategoryId,CreatedDate")] Trail trail, IFormFile? imageFile) // collect inputs and file input from form
    {
        // Validate if image file is uploaded
        if(imageFile == null)
        {
            // Manually add a validation failure
            ModelState.AddModelError("ImageFile", "The image file upload is required.");
        }

        // Check if the model state is valid before adding the trail to the database
        if (ModelState.IsValid)
        {
            // Save file in Azure Storage
           
            // Use a unique file name so files don't get overwritten. E.g. GUID.jpg            
            var extension = Path.GetExtension(imageFile!.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";


            string imageUrl = await _blobService.UploadFileAsync(imageFile, uniqueFileName);
            // Save image Url in Trail record
            trail.ImageUrl = imageUrl;

            // Save record in database
            _context.Add(trail);
            await _context.SaveChangesAsync();

            // Redirect to the Index action after successfully creating the trail
            return RedirectToAction(nameof(Index));
        }

        // create a select list for the categories
        var categories = _context.Category.ToList();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Title");

        return View(trail);
    }

    // GET: TRAILS/Edit/5
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trail = await _context.Trail.FindAsync(id);

        if (trail == null)
        {
            return NotFound();
        }

        // create a select list for the categories
        var categories = _context.Category.ToList();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Title");

        return View(trail);
    }

    // POST: TRAILS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,Description,Length,ElevationGain,Location,CategoryId,CreatedDate,ImageUrl")] Trail trail)
    {
        if (id != trail.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(trail);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrailExists(trail.Id))
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

        // create a select list for the categories
        var categories = _context.Category.ToList();
        ViewBag.CategoryId = new SelectList(categories, "Id", "Title");

        return View(trail);
    }

    // GET: TRAILS/Delete/5
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var trail = await _context.Trail
            .FirstOrDefaultAsync(m => m.Id == id);
        if (trail == null)
        {
            return NotFound();
        }

        return View(trail);
    }

    // POST: TRAILS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "administrator")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var trail = await _context.Trail.FindAsync(id);
        if (trail != null)
        {
            _context.Trail.Remove(trail);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TrailExists(int? id)
    {
        return _context.Trail.Any(e => e.Id == id);
    }
}
