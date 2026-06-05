using Microsoft.AspNetCore.Mvc;
using DapperApi.Models;
using DapperApi.Repositories;
namespace DapperApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _repo;
    public StudentsController(IStudentRepository repo)
    {
        _repo = repo;
    }
    // GET /api/students
    [HttpGet]
    public IActionResult GetAll() => Ok(_repo.GetAll());
    // GET /api/students/1
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var student = _repo.GetById(id);
        return student is null ? NotFound() : Ok(student);
    }
    [HttpGet("{name}")]
    public IActionResult GetByName(string name)
    {
        var student = _repo.GetByName(name);
        return student is null ? NotFound() : Ok(student);
    }

    // POST /api/students
    [HttpPost]
    public IActionResult Create([FromBody] Student student)
    {
        _repo.Create(student);
        return CreatedAtAction(nameof(GetById), new { id = student.Id },
        student);
    }

    // PUT /api/students
    [HttpPut]
    public IActionResult Update([FromBody] Student student)
    {
        _repo.Update(student);
        return NoContent();
    }

    // DELETE /api/students/1
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _repo.Delete(id);
        return NoContent();
    }

    // Đường dẫn thực tế sẽ là: GET /api/students/with-courses
    [HttpGet("with-courses")]
    public IActionResult GetStudentsWithCourses()
    {
        var kq = _repo.GetAllWithCourses();
        return Ok(kq);
    }
}