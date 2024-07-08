using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request.User
{
    public class AddUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
