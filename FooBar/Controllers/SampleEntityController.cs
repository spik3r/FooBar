// Controllers/SampleEntityController.cs
using Microsoft.AspNetCore.Mvc;
using FooBar.Models;
using FooBar.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FooBar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleEntityController : ControllerBase
    {
        private readonly ISampleEntityService _service;

        public SampleEntityController(ISampleEntityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SampleEntity>>> Get()
        {
            var entities = await _service.GetAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SampleEntity>> Get(int id)
        {
            var entity = await _service.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<SampleEntity>> Post(SampleEntity entity)
        {
            var createdEntity = await _service.AddAsync(entity);
            return CreatedAtAction(nameof(Get), new { id = createdEntity.Id }, createdEntity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SampleEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }

            await _service.UpdateAsync(entity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

