﻿using CCMS.BE.Data.Models;
using CCMS.BE.Data.Repositories;
using CCMS.BE.Interfaces;
using System.Threading.Tasks;
namespace CCMS.BE.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IVerifyCodeRepository VerifyCodes { get; private set; }

    public IOrderRepository OrderRepository { get; private set; }

    public IBaseRepository<MenuItemOrder> MenuItemOrder { get; private set; }

    public IBaseRepository<Update> Update { get; private set; }

    public IBaseRepository<UpdateOrder> UpdateOrder { get; private set; }
    public IRestaurantRepository Restaurant { get; private set; }

    public IBrancheRepository Branche { get; private set; }
    public IBranchPhoneRepository BranchPhone { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        VerifyCodes=new VerifyCodeRepository(_context);
        OrderRepository=new OrderRepository(_context);
        MenuItemOrder = new BaseRepository<MenuItemOrder>(context);
        Update = new BaseRepository<Update>(context);
        UpdateOrder = new BaseRepository<UpdateOrder>(context);
        Restaurant = new RestaurantRepository(context);
        Branche=new BrancheRepository(context);
        BranchPhone = new BranchPhoneRepository(context);

    }


    public async Task<int> CompleteAsync()
    {
        var num = await _context.SaveChangesAsync();
        return num;
    }
    public void Dispose()
    {
        _context.Dispose();
    }
}
