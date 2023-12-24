namespace MenuWebapi.Controllers;
using MenuWebapi.Models.Data;
using MenuWebapi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using MenuWebapi.Models.InputModel;
using MenuWebapi.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Slugify;

[ApiController]
[Route("[controller]/[action]")]
public class CategoryController : BaseController
{
    private readonly IWebHostEnvironment env;
    private SlugHelper slugHelper;
    public CategoryController(ApplicationDbContext _dbContext, IWebHostEnvironment _env) : base(_userManager: null, _dbContext: _dbContext)
    {
        this.slugHelper = new SlugHelper();
        this.env = _env;
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        if (dbContext.Categories != null)
        {
            try
            {
                return Ok(dbContext.Categories.First(f => f.Id == id));
            }
            catch (InvalidOperationException)
            {
                return BadRequest("No category founded");
            }
        }
        else
        {
            return BadRequest("No category founded");
        }
    }
    [HttpGet("{slug}")]
    public IActionResult GetBySlug([FromRoute] string slug)
    {
        if (dbContext.Categories != null)
        {
            try
            {
                return Ok(dbContext.Categories.First(f => f.Slug == slug));
            }
            catch (InvalidOperationException)
            {
                return BadRequest("No category founded");
            }
        }
        else
        {
            return BadRequest("No category founded");
        }
    }
    [HttpGet]
    public GetAllPaginatedSortedResponse<Category> GetAll([FromQuery] GetAllPaginatedSortedRequest inputModel)
    {
        var categories = dbContext.Categories?.AsQueryable();
        if (categories != null)
        {
            if (inputModel.Paginated)
            {
                if (inputModel.SearchKey != null)
                {
                    categories = categories.Where(w => w.Name.ToLower().Contains(inputModel.SearchKey.ToLower())).AsQueryable();
                }
                int count = categories.Count();
                int pages = Convert.ToInt32(Math.Ceiling((double)count / inputModel.PerPage));
                switch (inputModel.OrderBy)
                {
                    case "Id":
                        categories = inputModel.Ascending ? categories.OrderBy(o => o.Id) : categories.OrderByDescending(o => o.Id);
                        break;
                    case "Name":
                        categories = inputModel.Ascending ? categories.OrderBy(o => o.Name) : categories.OrderByDescending(o => o.Name);
                        break;
                    default:
                        categories = inputModel.Ascending ? categories.OrderBy(o => o.Id) : categories.OrderByDescending(o => o.Id);
                        break;
                }
                return new GetAllPaginatedSortedResponse<Category>
                {
                    Count = count,
                    Items = categories.Skip(inputModel.PerPage * (inputModel.Page - 1)).Take(inputModel.PerPage).ToList(),
                    Pages = pages
                };
            }
            else
            {
                int count = categories.Count();
                return new GetAllPaginatedSortedResponse<Category>
                {
                    Count = count,
                    Items = categories.ToList(),
                    Pages = 1
                };
            }
        }
        return new GetAllPaginatedSortedResponse<Category>
        {
            Count = 0,
            Items = null,
            Pages = 0
        };
    }

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostCreateCategory([FromForm] PostCreateCategoryRequest inputModel)
    {
        try
        {
            var category = new Category
            {
                Name = inputModel.Name!,
                Slug = slugHelper.GenerateSlug(inputModel.Name!)
            };
            dbContext.Add(category);
            await dbContext.SaveChangesAsync();
            if (inputModel.Image != null && category.Id > 0)
            {
                var extension = Path.GetExtension(Path.GetFileName(inputModel.Image!.FileName));
                var fileName = $"category-{category.Id}{extension}";
                var filePath = Path.Combine(env.WebRootPath, "Assets", "Images", "Category", fileName);
                var fileUrl = Path.Combine("/", "Assets", "Images", "Category", fileName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await inputModel.Image!.CopyToAsync(stream);
                }
                category.ImageUrl = fileUrl;
                dbContext.Update(category);
                await dbContext.SaveChangesAsync();
            }
            return new PostGenericResponse
            {
                Status = "Category created"
            };
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Category creation failed",
                ErrorMessage = ex.Message
            };
        }
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostUpdateCategory([FromForm] PostUpdateCategoryRequest inputModel)
    {
        try
        {
            var category = dbContext.Categories?.Find(inputModel.Id);
            if (category != null)
            {
                category.Slug = slugHelper.GenerateSlug(inputModel.Name);
                category.Name = inputModel.Name!;
                dbContext.Update(category);
                await dbContext.SaveChangesAsync();
                if (inputModel.Image != null)
                {
                    var inputFileName = Path.GetFileName(inputModel.Image!.FileName);
                    var extension = Path.GetExtension(inputFileName);
                    var fileName = $"category-{category.Id}{extension}";

                    Console.WriteLine($"FileName: {fileName}");
                    Console.WriteLine($"WebRootPath: {env.WebRootPath}");

                    var filePath = Path.Combine(env.WebRootPath, "Assets", "Images", "Category", fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    var fileUrl = Path.Combine("/", "Assets", "Images", "Category", fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await inputModel.Image!.CopyToAsync(stream);
                    }
                    category.ImageUrl = fileUrl;
                    dbContext.Update(category);
                    await dbContext.SaveChangesAsync();
                }
                return new PostGenericResponse
                {
                    Status = "Category updated"
                };
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Category update failed",
                };
            }
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Category update failed",
                ErrorMessage = ex.Message
            };
        }
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostDeleteCategory([FromBody] PostDeleteCategoryRequest inputModel)
    {
        try
        {
            var category = dbContext.Categories?.Find(inputModel.Id);
            if (category != null)
            {
                dbContext.Categories?.Remove(category);
                await dbContext.SaveChangesAsync();
                if (category.ImageUrl != null)
                {
                    var imageUrl = category.ImageUrl!;
                    var filePath = Path.Combine(env.WebRootPath, imageUrl.Substring(1));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                return new PostGenericResponse
                {
                    Status = "Category deleted"
                };
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Category delete failed",
                };
            }
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Category delete failed",
                ErrorMessage = ex.Message
            };
        }
    }
}
