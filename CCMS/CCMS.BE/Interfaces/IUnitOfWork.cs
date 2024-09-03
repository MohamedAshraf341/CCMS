using CCMS.BE.Data.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IVerifyCodeRepository VerifyCodes {  get; }
    IOrderRepository Order { get; }
    IBaseRepository<MenuItemOrder> MenuItemOrder {  get; }
    IBaseRepository<Update> Update { get; }
    IBaseRepository<UpdateOrder> UpdateOrder { get; }
    IBaseRepository<BranchUser> BranchUser { get; }
    IBaseRepository<AppSetting> AppSetting { get; }

    IRestaurantRepository Restaurant { get; }
    IBrancheRepository Branche { get; }
    IBranchPhoneRepository BranchPhone { get; }
    ICustomerRepository Customer { get; }
    IMenuItemRepository MenuItem { get; }
    IUserSettingRepository UserSetting { get; }

    Task<int> CompleteAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();

}
