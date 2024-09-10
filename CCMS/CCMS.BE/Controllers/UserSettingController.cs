using AutoMapper;
using CCMS.BE.Data.Models;
using CCMS.BE.Interfaces;
using CCMS.Common.Const;
using CCMS.Common.Dto.Request.UserSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dto = CCMS.Common.Dto;
using models= CCMS.BE.Data.Models;

namespace CCMS.BE.Controllers
{
    [Route(Router.Root)]
    [ApiController]
    public class UserSettingController : ControllerBase
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UserSettingController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet(Router.UserSetting.GetByUser+"/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var items = await _uow.UserSetting.GetByUser(userId);
            var dalItems = _mapper.Map<IEnumerable<dto.UserSettingDto>>(items);
            return Ok(dalItems);
        }
        [HttpPost(Router.UserSetting.GetByUserAndKey )]
        public async Task<IActionResult> GetByUserAndKey(GetByUserAndKey dto)
        {
            var items = await _uow.UserSetting.GetByUserAndKey(dto.UserId,dto.Key);
            var dalItems = _mapper.Map<dto.UserSettingDto>(items);
            return Ok(dalItems);
        }
        [Authorize]
        [HttpGet(Router.UserSetting.Prefix)]
        public async Task<IActionResult> GetAll()
        {
            var items=await _uow.UserSetting.GetAllAsync();
            var dalItems = _mapper.Map<IEnumerable<dto.UserSettingDto>>(items);
            return Ok(dalItems);
        }
        [Authorize]
        [HttpGet(Router.UserSetting.Prefix+"/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var items = await _uow.UserSetting.GetByIdAsync(id);
            var dalItems = _mapper.Map<dto.UserSettingDto>(items);
            return Ok(dalItems);
        }
        [Authorize]
        [HttpPost(Router.UserSetting.Prefix)] 
        public async Task<IActionResult> Add(dto.UserSettingDto dto)
        {
            var item = _mapper.Map<UserSetting>(dto);
            var res =await _uow.UserSetting.AddAsync(item);
            var sv =await _uow.CompleteAsync();
            if(sv>0)
                return Ok(true);
            return Ok(false);
        }
        [Authorize]
        [HttpPut(Router.UserSetting.Prefix)]
        public async Task<IActionResult> Edit(dto.UserSettingDto dto)
        {
            var item = _mapper.Map<UserSetting>(dto);
            var res = _uow.UserSetting.Update(item);
            var sv = await _uow.CompleteAsync();
            if (sv > 0)
                return Ok(true);
            return Ok(false);
        }
        [Authorize]
        [HttpDelete(Router.UserSetting.Prefix + "/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _uow.UserSetting.GetByIdAsync(id);
            _uow.UserSetting.Delete(item);
            var sv = await _uow.CompleteAsync();
            if (sv > 0)
                return Ok(true);
            return Ok(false);
        }

    }
}
