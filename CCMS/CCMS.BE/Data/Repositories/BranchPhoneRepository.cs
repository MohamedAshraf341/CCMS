using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories
{
    public class BranchPhoneRepository:BaseRepository<BranchPhone>,IBranchPhoneRepository
    {
        public BranchPhoneRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BranchPhone>> GetAll(Guid branchId)
        {
            var items=await _context.BranchPhones.Where(b => b.BranchId == branchId).ToListAsync();
            return items;
        }
    }
}
