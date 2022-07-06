using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.DTO;
using nyschub.Entities;
using nyschub.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CorperController : ControllerBase
    {
        private readonly ICorperRepository _corperRepository;

        private readonly AppDbContext _database;

        public CorperController(ICorperRepository corperRepository, AppDbContext database)
        {
            _corperRepository = corperRepository;
            _database = database;
        }

        // get all corpers
        // cd nyschub

        [SwaggerOperation(Summary = "getting all corpers in a particular page")]
        [HttpGet]
        [Route("AllCorpers/{page}", Name = "Corpers")]
        public async Task<IActionResult> GetCorpers(int page)
        {
            var corpers = await _corperRepository.GetPaginated(page, 10);

            var showUsers = new List<CorperDto>();
            foreach(var corper in corpers)
            {
                var User = new CorperDto()
                {
                    FirstName = corper.FirstName,
                    LastName = corper.LastName,
                    NyscRegNumber = corper.NyscRegNumber,
                    UserName = corper.UserName
                };
                showUsers.Add(User);
            }
            return Ok(corpers);
            
        }

        // get particular corper
        [SwaggerOperation(Summary = "getting a corper in a particular page")]
        [HttpGet]
        [Route("GetCorper/{id}", Name = "Corper")]
        public async Task<IActionResult> GetCorper(string id)
        {
            var Iscorper = await _corperRepository.GetById(id);
            var corper = _database.Corpers.FirstOrDefault(corper => corper.Id == id);
            return Ok(corper);
        }

        // update corper details
        [SwaggerOperation(Summary = "updating a corpers details")]
        [HttpPut]
        [Route("UpdateCorper/{id}", Name = "UpdateCorper")]
        public async Task<IActionResult> UpdateCorper(string id, UpdateCorperDto corperDto)
        {
            var _corper = await _database.Corpers.FindAsync(id);
            _corper.FirstName = corperDto.FirstName;
            _corper.LastName = corperDto.LastName;
            _corper.UserName = corperDto.UserName;
            _corper.StateOfDeployment = corperDto.state;
            _corper.Status = 1;

            await _corperRepository.Update(_corper);

            return Ok(_corper);
        }
    }
}
