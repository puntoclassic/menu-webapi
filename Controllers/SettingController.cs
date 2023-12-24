using MenuWebapi.Models.Data;
using MenuWebapi.Models.Entities;
using MenuWebapi.Models.InputModel;
using MenuWebapi.Models.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MenuWebapi.Controllers;
[ApiController]
[Route("[controller]/[action]")]
public class SettingController : BaseController
{
    public SettingController(ApplicationDbContext _dbContext) : base(_userManager: null, _dbContext: _dbContext)
    {
    }

    [HttpGet]
    public Setting? GetAll()
    {
        var item = new Setting();
        try
        {
            item = dbContext.Settings!.First();
        }
        catch (InvalidOperationException)
        {

        }
        return item;
    }
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<PostGenericResponse> PostUpdatedSettings([FromBody] PostUpdatedSettingsRequest inputModel)
    {
        var settings = dbContext.Settings?.First();
        if (settings != null)
        {
            settings.SiteName = inputModel.SiteName ?? settings.SiteName;
            settings.SiteSubtitle = inputModel.SiteSubtitle ?? settings.SiteSubtitle;
            settings.OrderCreatedStateId = inputModel.OrderCreatedStateId ?? settings.OrderCreatedStateId;
            settings.OrderPaidStateId = inputModel.OrderPaidStateId ?? settings.OrderPaidStateId;
            dbContext.Update(settings);
            await dbContext.SaveChangesAsync();
            return new PostGenericResponse
            {
                Status = "Ok"
            };
        }
        else
        {
            return new PostGenericResponse
            {
                Status = "No settings to update"
            };
        }
    }
}
