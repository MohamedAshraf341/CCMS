using CCMS.BE.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using CCMS.BE.Data.Models;
using CCMS.Common.Dto.Response;
using CCMS.Common.Dto;
using CCMS.Common.Dto.Request.Phone;

namespace CCMS.BE.Services
{
    public class PhoneService
    {
        private readonly IUnitOfWork _uow;
        public PhoneService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<PhoneNumberDto>> GetPhonesByBranch(Guid branchId)
        {
            var items = await _uow.BranchPhone.GetAll(branchId);
            var res = items.Select(p => new PhoneNumberDto
            {
                Id = p.Id,
                BranchId = p.BranchId,
                PhoneNumber = p.Phone
            }).ToList();
            return res;
        }
        public async Task<BaseResponse> AddPhone(AddOrEditPhone model)
        {
            try
            {
                var item = new BranchPhone
                {
                    BranchId = model.BranchId,
                    Phone = model.PhoneNumber
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
        public async Task<BaseResponse> EditPhone(AddOrEditPhone model)
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
    }
}
