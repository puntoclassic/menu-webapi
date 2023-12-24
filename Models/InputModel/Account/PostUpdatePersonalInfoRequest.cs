using System.ComponentModel.DataAnnotations;
using MenuWebapi.Models.Annotations;
#nullable disable
namespace MenuWebapi.Models.InputModel
{
    public class PostUpdatePersonalInfoRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
