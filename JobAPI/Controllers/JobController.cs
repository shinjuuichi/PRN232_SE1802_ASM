using JobAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using SharedLibrary.DTOs;

[Route("odata/Jobs")]
[ApiController]
public class JobController(IJobService _accountService) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public IQueryable<JobReadDto> GetAll()
    {
        var accounts = _accountService.GetAllAsQueryable();
        return accounts;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobReadDto>> GetById(int id)
    {
        var account = await _accountService.GetStudentByIdAsync(id);
        if (account == null)
        {
            return NotFound();
        }
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobCreateDto createAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdAccount = await _accountService.CreateStudentAsync(createAccountDto);
        return CreatedAtAction(nameof(GetById), new { id = createdAccount.Id }, createdAccount);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobUpdateDto updateAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedAccount = await _accountService.UpdateStudentAsync(id, updateAccountDto);
        if (updatedAccount == null)
        {
            return NotFound();
        }
        return Ok(updatedAccount);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _accountService.DeleteStudentAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}
