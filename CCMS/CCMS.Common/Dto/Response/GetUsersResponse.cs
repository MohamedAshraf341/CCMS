using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Response;

public class GetUsersResponse
{
    public string UserId { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; }
    public string? Gender { get; set; }
    public byte[]? Picture { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public List<string> Roles { get; set; }

}
