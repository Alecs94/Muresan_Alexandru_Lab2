using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Muresan_Alexandru_Lab2.Models;

namespace Muresan_Alexandru_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Muresan_Alexandru_Lab2Context _context;

    public EditModel(Muresan_Alexandru_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        Book = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.BookCategories).ThenInclude(b => b.Category)
            .Include(b => b.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ID == id);

        if (Book is null)
        {
            return NotFound();
        }
        PopulateAssignedCategoryData(_context, Book);

        RepopulateDropDownLists(_context, Book);
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
    {
        if (id == null)
        {
            return NotFound();
        }

        var bookToUpdate = await _context.Book
            .Include(i => i.Author)
            .Include(i => i.Publisher)
            .Include(i => i.BookCategories)
                .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id); 
        
        if (bookToUpdate == null) 
        { 
            return NotFound(); 
        }

        var updated = await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title, i => i.AuthorID, i => i.Price,
            i => i.PublishingDate, i => i.PublisherID);

        if (updated) 
        { 
            UpdateBookCategories(_context, selectedCategories, bookToUpdate); 
            await _context.SaveChangesAsync(); 
            return RedirectToPage("./Index"); 
        }

        UpdateBookCategories(_context, selectedCategories, bookToUpdate);
        PopulateAssignedCategoryData(_context, bookToUpdate);
        RepopulateDropDownLists(_context, bookToUpdate);
        return Page();
    }

    public void RepopulateDropDownLists(Muresan_Alexandru_Lab2Context context, Book book)
    {
        ViewData["PublisherID"] = new SelectList(context.Set<Publisher>(), "ID", "PublisherName", book.PublisherID);
        ViewData["AuthorID"] = new SelectList(
            context.Set<Author>()
            .Select(a => new
            {
                a.ID,
                FullName = $"{a.FirstName} {a.LastName}"
            }
            ),
            "ID",
            "FullName",
            book.AuthorID);
    }
}