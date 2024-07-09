using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CCMS.BE.Data.Models;
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public byte[]? Picture { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? SystemType { get; set; }
    public ICollection<VerifyCode>? Codes { get; set; }
    public List<RefreshToken>? RefreshTokens { get; set; }
    public ICollection<BranchUser>? BranchUsers { get; set; }
    public ICollection<Order>? CreatedOrders { get; set; }
    public ICollection<Order>? ReceivedOrders { get; set; }


}
