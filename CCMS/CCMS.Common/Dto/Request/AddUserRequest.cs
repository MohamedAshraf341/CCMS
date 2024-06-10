using System.ComponentModel.DataAnnotations;

namespace CCMS.Common.Dto.Request
{
    public class AddUserRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
