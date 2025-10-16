using Microsoft.AspNetCore.Mvc;
using PhoneBook.Application.DTOs;
using PhoneBook.Application.Services;

namespace PhoneBook.Api.Controllers
{
    [ApiController]
    [Route("api/phonebook")]
    public class PhoneBookController : ControllerBase
    {
        private readonly IPhoneBookService _service;
        public PhoneBookController(IPhoneBookService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEntryDto dto)
        {
            var created = await _service.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var e = await _service.GetByIdAsync(id);
            if (e is null) return NotFound();
            return Ok(e);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEntryDto dto)
        {
            if (dto.Id != id) return BadRequest("Mismatched id");
            var updated = await _service.UpdateAsync(dto);
            if (updated is null) return NotFound();
            return Ok(updated);
        }

        [HttpGet("tag/{tag}")]
        public async Task<IActionResult> GetByTag(string tag)
        {
            var list = await _service.GetByTagAsync(tag);
            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}