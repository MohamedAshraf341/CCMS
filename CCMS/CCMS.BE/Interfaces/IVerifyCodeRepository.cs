using CCMS.BE.Data.Models;
using System.Threading.Tasks;
namespace CCMS.BE.Interfaces;

public interface IVerifyCodeRepository : IBaseRepository<VerifyCode>
{
    Task<VerifyCode?> GetByUserId(string userId);

}
