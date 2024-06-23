using CCMS.BE.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces
{
    public interface IBranchPhoneRepository:IBaseRepository<BranchPhone>
    {
        Task <IEnumerable<BranchPhone>> GetAll(Guid branchId);
    }

}
