using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories;

public class VerifyCodeRepository:BaseRepository<VerifyCode>, IVerifyCodeRepository
{
    public VerifyCodeRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<VerifyCode?> GetByUserId(string userId)
    {
        var res= await _context.VerifyCodes.Where(v => v.UserId==userId).FirstOrDefaultAsync(); 
        return res;
    }
}
