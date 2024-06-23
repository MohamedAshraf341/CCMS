using CCMS.BE.Data.Models;
using CCMS.BE.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces
{
    public interface IRestaurantRepository: IBaseRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetIncludeBranches();
        Task<Restaurant> GetIncludeBranches(Guid Id);

    }
}
