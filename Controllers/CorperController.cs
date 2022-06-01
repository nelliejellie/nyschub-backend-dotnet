using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.DTO;
using nyschub.Entities;
using nyschub.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorperController : ControllerBase
    {
        private readonly ICorperRepository _corperRepository;

        private readonly AppDbContext _database;

        public CorperController(ICorperRepository corperRepository, AppDbContext database)
        {
            _corperRepository = corperRepository;
            _database = database;
        }

        [Authorize]
        [HttpGet]
        [Route("AllCorpers", Name = "Corpers")]
        public async Task<IActionResult> GetCorpers()
        {
            var corpers = await _corperRepository.All();
            return Ok(corpers);
        }

        [HttpGet]
        [Route("GetCorper/{id}", Name = "Corper")]
        public async Task<IActionResult> GetCorper(string id)
        {
            var Iscorper = await _corperRepository.GetById(id);
            var corper = _database.Corpers.FirstOrDefault(corper => corper.Id == id);
            return Ok(corper);
        }

        [HttpPost]
        [Route("AddCorper/{id}", Name = "AddCorper")]
        public async Task<IActionResult> AddCorper(CorperDto corperDto)
        {
            var _corper = new Corper();
            _corper.FirstName = corperDto.FirstName;
            _corper.LastName = corperDto.LastName;
            _corper.NyscRegNumber = corperDto.NyscRegNumber;
            _corper.UserName = corperDto.UserName;
            _corper.Status = 1;

            await _corperRepository.Add(_corper);

            return CreatedAtRoute("Corper", corperDto);
        }

        [HttpDelete]
        [Route("DeleteCorper/{id}", Name = "DeleteCorper")]
        public async Task<IActionResult> DeleteCorper(string id)
        {
            var corper = await _database.Corpers.FindAsync(id);

            var isDeleted = await _corperRepository.Delete(id);

            return isDeleted ? Ok($"{corper.UserName} has been deleted") : Ok($"{corper.UserName} has not been deleted");
        }

        [HttpPut]
        [Route("UpdateCorper/{id}", Name = "UpdateCorper")]
        public async Task<IActionResult> UpdateCorper(string id, UpdateCorperDto corperDto)
        {
            var _corper = await _database.Corpers.FindAsync(id);
            _corper.FirstName = corperDto.FirstName;
            _corper.LastName = corperDto.LastName;
            _corper.UserName = corperDto.UserName;
            _corper.Status = 1;

            await _corperRepository.Update(_corper);

            return Ok(_corper);
        }
    }
}
