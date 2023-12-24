using MenuWebapi.Models.Data;
using MenuWebapi.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MenuWebapi.Models.ResponseModel;
using MenuWebapi.Models.InputModel;
using Microsoft.AspNetCore.Authorization;
namespace MenuWebapi.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class FoodController : BaseController
{
    public FoodController(ApplicationDbContext _dbContext) : base(_userManager: null, _dbContext: _dbContext)
    {
    }


    [HttpGet]
    public GetAllPaginatedSortedResponse<Food> GetAll([FromQuery] GetAllPaginatedSortedRequest inputModel)
    {
        var foods = dbContext.Foods?.Include(i => i.Category).AsQueryable();
        if (foods != null)
        {
            if (inputModel.Paginated)
            {
                if (inputModel.SearchKey != null)
                {
                    Func<Food, bool> filter = w =>
                    {
                        bool categoryFilter = false;
                        bool nameFilter = w.Name.ToLower().Contains(inputModel.SearchKey.ToLower());
                        bool ingredientFilter = false;

                        if (w.Category != null)
                        {
                            categoryFilter = w.Category.Name.ToLower().Contains(inputModel.SearchKey.ToLower());
                        }

                        if (w.Ingredients != null)
                        {
                            ingredientFilter = w.Ingredients.ToLower().Contains(inputModel.SearchKey.ToLower());
                        }

                        return categoryFilter || nameFilter || ingredientFilter;

                    };

                    foods = foods.Where(filter).AsQueryable();
                }
                int count = foods.Count();
                int pages = Convert.ToInt32(Math.Ceiling((double)count / inputModel.PerPage));
                switch (inputModel.OrderBy)
                {
                    case "id":
                        foods = inputModel.Ascending ? foods.OrderBy(o => o.Id) : foods.OrderByDescending(o => o.Id);
                        break;
                    case "name":
                        foods = inputModel.Ascending ? foods.OrderBy(o => o.Name) : foods.OrderByDescending(o => o.Name);
                        break;
                    case "category":
                        foods = inputModel.Ascending ? foods.OrderBy(o => o.Category!.Name) : foods.OrderByDescending(o => o.Category!.Name);
                        break;
                    case "price":
                        foods = inputModel.Ascending ? foods.OrderBy(o => o.Price) : foods.OrderByDescending(o => o.Price);
                        break;
                    default:
                        foods = inputModel.Ascending ? foods.OrderBy(o => o.Id) : foods.OrderByDescending(o => o.Id);
                        break;
                }
                return new GetAllPaginatedSortedResponse<Food>
                {
                    Count = count,
                    Items = foods.Skip(inputModel.PerPage * (inputModel.Page - 1)).Take(inputModel.PerPage).ToList(),
                    Pages = pages
                };
            }
            else
            {
                int count = foods.Count();
                return new GetAllPaginatedSortedResponse<Food>
                {
                    Count = count,
                    Items = foods.ToList(),
                    Pages = 1
                };
            }
        }
        return new GetAllPaginatedSortedResponse<Food>
        {
            Count = 0,
            Items = null,
            Pages = 0
        };
    }
    [HttpGet("{id}")]
    public Food? GetById([FromRoute] int id)
    {
        if (dbContext.Foods != null)
        {
            try
            {
                return dbContext.Foods.First(f => f.Id == id);
            }
            catch (InvalidOperationException)
            {
                HttpContext.Response.StatusCode = 500;
                return null;
            }
        }
        else
        {
            HttpContext.Response.StatusCode = 500;
            return null;
        }
    }
    [HttpGet("{slug}")]
    public GetFoodResponse GetByCategorySlug([FromRoute] string slug)
    {
        if (dbContext.Foods != null)
        {
            try
            {
                var category = dbContext.Categories?.First(f => f.Slug == slug);
                if (category != null)
                {
                    List<Food> Items = dbContext.Foods.Where(f => f.Category == category).ToList();
                    return new GetFoodResponse
                    {
                        Items = Items,
                        Count = Items.Count
                    };
                }
                else
                {
                    return new GetFoodResponse();
                }
            }
            catch (InvalidOperationException)
            {
                return new GetFoodResponse();
            }
        }
        else
        {
            return new GetFoodResponse();
        }
    }
    [HttpGet]
    public GetSearchFoodResponse Search([FromQuery] string search)
    {
        if (dbContext.Foods != null)
        {
            try
            {
                var results = dbContext.Foods?.Where(f =>
                    f.Name.ToLower().Contains(search.ToLower())
                    || (f.Ingredients != null && f.Ingredients.ToLower().Contains(search.ToLower()))
                    || (f.Category != null && f.Category.Name.ToLower().Contains(search.ToLower()))
                ).Include(i => i.Category).ToList();
                if (results != null)
                {
                    return new GetSearchFoodResponse
                    {
                        Count = results.Count,
                        Items = results
                    };
                }
                else
                {
                    return new GetSearchFoodResponse
                    {
                        Count = 0,
                        Items = new List<Food>()
                    };
                }
            }
            catch (InvalidOperationException)
            {
                return new GetSearchFoodResponse
                {
                    Count = 0,
                    Items = new List<Food>()
                };
            }
        }
        else
        {
            return new GetSearchFoodResponse
            {
                Count = 0,
                Items = new List<Food>()
            };
        }
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostCreateFood([FromBody] PostCreateFoodRequest inputModel)
    {
        try
        {
            var food = new Food
            {
                Name = inputModel.Name!,
                Ingredients = inputModel.Ingredients,
                Price = inputModel.Price,
                Category = dbContext.Categories?.Find(inputModel.CategoryId)
            };
            dbContext.Add(food);
            await dbContext.SaveChangesAsync();
            return new PostGenericResponse
            {
                Status = "Food created"
            };
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Food creation failed",
                ErrorMessage = ex.Message
            };
        }
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostUpdateFood([FromBody] PostUpdateFoodRequest inputModel)
    {
        try
        {
            var food = dbContext.Foods?.Find(inputModel.Id);
            if (food != null)
            {
                food.Category = dbContext.Categories?.Find(inputModel.CategoryId);
                food.Ingredients = inputModel.Ingredients;
                food.Price = inputModel.Price;
                food.Name = inputModel.Name!;
                dbContext.Update(food);
                await dbContext.SaveChangesAsync();
                return new PostGenericResponse
                {
                    Status = "Food updated"
                };
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Food update failed",
                };
            }
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Food update failed",
                ErrorMessage = ex.Message
            };
        }
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostDeleteFood([FromBody] PostDeleteFoodRequest inputModel)
    {
        try
        {
            var food = dbContext.Foods?.Find(inputModel.Id);
            if (food != null)
            {
                dbContext.Foods?.Remove(food);
                await dbContext.SaveChangesAsync();
                return new PostGenericResponse
                {
                    Status = "Food deleted"
                };
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                return new PostGenericResponse
                {
                    Status = "Food delete failed",
                };
            }
        }
        catch (InvalidOperationException ex)
        {
            HttpContext.Response.StatusCode = 500;
            return new PostGenericResponse
            {
                Status = "Food delete failed",
                ErrorMessage = ex.Message
            };
        }
    }
}
