using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Restaurant;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto.Response.Reasturant;
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
        public async Task<GetReasturants> GetReasturant()
        {
            try
            {
                var items = await _uow.Restaurant.GetIncludeBranches();
                var Restaurants = items.Select(r => new RestaurantDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    BranchCount = r.Branches != null ? r.Branches.Count() : 0,
                }).ToList();
                var res= new GetReasturants { Message= "List of Restaurants",Success=true,Reasturants= Restaurants };
                return res;
            }
            catch (Exception ex) { 
                return new GetReasturants { Success=false,Message=ex.Message};
            }

        }
        public async Task<BaseResponse> AddReasturant(AddOrEditRestaurant model)
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
        public async Task<BaseResponse> EditReasturant(AddOrEditRestaurant model)
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
