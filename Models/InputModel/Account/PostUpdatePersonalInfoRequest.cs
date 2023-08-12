using System.ComponentModel.DataAnnotations;
using MenuBackend.Models.Annotations;
#nullable disable
namespace MenuBackend.Models.InputModel
{
    public class PostUpdatePersonalInfoRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
