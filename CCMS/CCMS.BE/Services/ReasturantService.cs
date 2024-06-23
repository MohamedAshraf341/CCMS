using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request;
using CCMS.Common.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCMS.BE.Services
{
    public class ReasturantService
    {
        private readonly IUnitOfWork _uow;

        public ReasturantService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<PhoneNumbersDto>> GetPhonesByBranch(Guid branchId)
        {
            var items = await _uow.BranchPhone.GetAll(branchId);
            var res= items.Select(p => new PhoneNumbersDto
            {
                Id = p.Id,
                BranchId = p.BranchId,
                PhoneNumber=p.Phone
            }).ToList();
            return res;
        }
        public async Task<BaseResponse> AddPhone(PhoneNumbersDto model)
        {
            try
            {
                var item = new BranchPhone
                {
                    BranchId = model.BranchId,
                    Phone=model.PhoneNumber
                };
                await _uow.BranchPhone.AddAsync(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Phone Added successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<BaseResponse> EditPhone(PhoneNumbersDto model)
        {
            try
            {
                var item = await _uow.BranchPhone.GetByIdAsync(model.Id);
                item.Phone = model.PhoneNumber;
                _uow.BranchPhone.Update(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "phone Added successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<BaseResponse> DeletePhone(Guid id)
        {
            try
            {
                var item = await _uow.BranchPhone.GetByIdAsync(id);
                _uow.BranchPhone.Delete(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Phone Deleted successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }


        }
        public async Task<List<GetBranchesResponse>> GetBranches(Guid? restaurantId)
        {
            var items = await _uow.Branche.GetAll(restaurantId);
            var res= items.Select(b => new  GetBranchesResponse
            {
                Area = b.Area,
                City = b.City,
                Government = b.Government,
                Id = b.Id,
                Reasturant=b.Restaurant!=null?b.Restaurant.Name:string.Empty,
                PhoneNumbers=b.BranchPhones.Select(p => new PhoneNumberDto
                {
                    Id = p.Id,
                    Phone=p.Phone,
                }).ToList()
            }).ToList();
            return res;
        }
        public async Task<BaseResponse> AddBranche(AddBrancheRequest model)
        {
            try
            {
                var item = new Branch
                {
                    Area= model.Area,
                    City= model.City,
                    Government= model.Government,
                    RestaurantId=model.RestaurantId,
                    BranchPhones=model.Phones.Select(p => new BranchPhone { Phone=p}).ToList()
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
        public async Task<BaseResponse> EditBranche(AddBrancheRequest model)
        {
            try
            {
                var item =await _uow.Branche.GetByIdWithInclude(model.Id);
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
        public async Task<List<GetReasturantResponse>> GetReasturant()
        {
            var items = await _uow.Restaurant.GetIncludeBranches();
            var res = items.Select(r => new GetReasturantResponse
            {
                Id = r.Id,
                Name = r.Name,
                BranchCount = r.Branches!=null? r.Branches.Count() : 0,
            }).ToList();
            return res;
        }
        public async Task<BaseResponse> AddReasturant(AddReasturantRequest model)
        {
            try
            {
                var item=new Restaurant { Name = model.Name };
                await _uow.Restaurant.AddAsync(item);
                var res=await _uow.CompleteAsync();
                if(res>0)
                    return new BaseResponse { Success = true, Message = "Reasturant added successfully."};
                else
                    return new BaseResponse { Success = false, Message = "There are error."};

            }
            catch (Exception ex)
            {
                return new BaseResponse { Success=false,Message=ex.Message};
            }
        }
        public async Task<BaseResponse> EditReasturant(GetReasturantResponse model)
        {
            try
            {
                var item = await _uow.Restaurant.GetByIdAsync(model.Id);
                item.Name = model.Name;
                _uow.Restaurant.Update(item);
                var res = await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Reasturant Updated successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };

            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false, Message = ex.Message };
            }
        }
        public async Task<BaseResponse> DeleteReasturant(Guid Id)
        {
            try
            {
                var item = await _uow.Restaurant.GetIncludeBranches(Id);
                _uow.Restaurant.Delete(item);
                var res= await _uow.CompleteAsync();
                if (res > 0)
                    return new BaseResponse { Success = true, Message = "Reasturant Deleted successfully." };
                else
                    return new BaseResponse { Success = false, Message = "There are error." };
            }
            catch (Exception ex)
            {
                return new BaseResponse { Success = false,Message= ex.Message};
            }

        }
    }
}
