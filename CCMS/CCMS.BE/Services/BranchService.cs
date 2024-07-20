using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using CCMS.Common.Dto.Response.Branch;
using CCMS.Common.Dto.Request.Branch;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto;

namespace CCMS.BE.Services
{
    public class BranchService
    {
        private readonly IManagementUsersService _managementUsersService;

        private readonly IUnitOfWork _uow;
        public BranchService(IUnitOfWork uow, IManagementUsersService managementUsersService)
        {
            _uow = uow;
            _managementUsersService = managementUsersService;
        }
        public async Task<Common.Dto.Response.Branch.GetBranches> GetBranches(Common.Dto.Request.Branch.GetBranches req)
        {
            try
            {
                var items = await _uow.Branche.GetAll(req);
                var branches = items.Select(b => new BranchDto
                {
                    Id = b.Id,
                    Address = GetFormattedAddress(b.Area,b.City,b.Government),
                    Restaurant = b.Restaurant != null ? b.Restaurant.Name : string.Empty,
                    phoneNumbers = b.BranchPhones.Select(p => new PhoneNumberDto
                    {
                        Id = p.Id,
                        PhoneNumber = p.Phone,
                        BranchId = b.Id
                    }).ToList(),
                }).ToList();

                return new Common.Dto.Response.Branch.GetBranches { Success = true, Message = "List of Branches",Branches=branches};
            }
            catch(Exception ex) 
            {
                return new Common.Dto.Response.Branch.GetBranches { Success = false, Message = ex.Message };
            }

        }
        private string GetFormattedAddress( string Area, string City, string Government)
        {
            return $"{Area}, {City}, {Government}";
        }
        public async Task<BaseResponse> AddBranche(AddOrEditBranche model)
        {
            try
            {
                var item = new Branch
                {
                    Area = model.Area,
                    City = model.City,
                    Government = model.Government,
                    RestaurantId = model.RestaurantId,
                    BranchPhones = model.Phones.Select(p => new BranchPhone { Phone = p }).ToList()
                };
                await _uow.Branche.AddAsync(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Branches Added successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<BaseResponse> EditBranche(AddOrEditBranche model)
        {
            try
            {
                var item = await _uow.Branche.GetByIdWithInclude(model.Id);
                //var item = new Branch
                item.Area = model.Area;
                item.City = model.City;
                item.Government = model.Government;
                item.RestaurantId = model.RestaurantId;
                item.BranchPhones = model.Phones.Select(p => new BranchPhone { Phone = p }).ToList();
                _uow.Branche.Update(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Branches Updated successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<BaseResponse> DeleteBranch(Guid id)
        {
            try
            {
                var item = await _uow.Branche.GetByIdWithInclude(id);
                _uow.Branche.Delete(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Branches Deleted successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }


        }
    }
}
