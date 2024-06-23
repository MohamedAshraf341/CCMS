using CCMS.BE.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces
{
    public interface IBrancheRepository:IBaseRepository<Branch>
    {
        Task<IEnumerable<Branch>> GetAll(Guid? restaurantId);
        Task<Branch> GetByIdWithInclude(Guid id);
    }
}
