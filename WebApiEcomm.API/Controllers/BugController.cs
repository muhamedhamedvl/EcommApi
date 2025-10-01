﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEcomm.Core.Interfaces.IUnitOfWork;

namespace WebApiEcomm.API.Controllers
{

    public class bugController : BaseController
    {
        public bugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {


        }

        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category = await _work.CategoryRepository.GetByIdAsync(100);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpGet("server-error")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await _work.CategoryRepository.GetByIdAsync(100);
            category.Name = "";  
            return Ok(category);
        }

        [HttpGet("bad-request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id)
        {
            return Ok();
        }       
        [HttpGet("bad-request")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
