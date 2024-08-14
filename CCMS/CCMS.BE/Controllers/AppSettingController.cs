using AutoMapper;
using CCMS.BE.Interfaces;
using CCMS.Common.Const;
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
    public class AppSettingController : ControllerBase
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AppSettingController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet(Router.AppSetting.Prefix)]
        public async Task<IActionResult> GetAll()
        {
            var items=await _uow.AppSetting.GetAllAsync();
            var dalItems = _mapper.Map<IEnumerable<dto.AppSettingDto>>(items);
            return Ok(dalItems);
        }
        [HttpGet(Router.AppSetting.Prefix+"/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var items = await _uow.AppSetting.GetByIdAsync(id);
            var dalItems = _mapper.Map<dto.AppSettingDto>(items);
            return Ok(dalItems);
        }
        [HttpPost(Router.AppSetting.Prefix)] 
        public async Task<IActionResult> Add(dto.AppSettingDto dto)
        {
            var item = new models.AppSetting { Id = dto.Id, Key = dto.Key, Value = dto.Value, ValueType = dto.ValueType };
            var res =await _uow.AppSetting.AddAsync(item);
            var sv =await _uow.CompleteAsync();
            if(sv>0)
                return Ok(true);
            return Ok(false);
        }
        [HttpPut(Router.AppSetting.Prefix)]
        public async Task<IActionResult> Edit(dto.AppSettingDto dto)
        {
            var item = new models.AppSetting { Id = dto.Id,Key=dto.Key,Value=dto.Value,ValueType=dto.ValueType };
            var res = _uow.AppSetting.Update(item);
            var sv = await _uow.CompleteAsync();
            if (sv > 0)
                return Ok(true);
            return Ok(false);
        }
        [HttpDelete(Router.AppSetting.Prefix + "/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _uow.AppSetting.GetByIdAsync(id);
            _uow.AppSetting.Delete(item);
            var sv = await _uow.CompleteAsync();
            if (sv > 0)
                return Ok(true);
            return Ok(false);
        }

    }
}
