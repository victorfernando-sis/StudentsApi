using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentsApi.Class;
using StudentsApi.Models;

namespace StudentsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Student>>> GetStudents()
        {
            var students = await _studentService.GetStudents();
            return Ok(students);

        }

        [HttpGet("StudentByName")]
        public async Task<ActionResult<IAsyncEnumerable<Student>>>
            GetStudentsByName([FromQuery] string name)
        {
            var students = await _studentService.GetStudentByName(name);
            if (students == null) return NotFound("There is no student with this name");
            return Ok(students);
        }

        [HttpGet("{id:int}", Name="GetStudent")]
        public async Task<ActionResult<Student>>GetStudent(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student == null) return NotFound($"There is no student with this id={id}");
            return Ok(student);

        }

        [HttpPost]
        public async Task<ActionResult> Create(Student student)
        {
            await _studentService.CreateStudent(student);
            return CreatedAtRoute(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Student student)
        {
            if (student.StudentId == id)
            {
                await _studentService.UpdateStudent(student);
                return Ok($"Student with id={id} was updated");
            }
            else
            {
                return BadRequest("Inconsistent data");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var student = await _studentService.GetStudent(id);
            if (student != null)
            {
                await _studentService.DeleteStudent(student);
                return Ok($"Student with id={id} was deleted");

            }
            else
            {
                return NotFound($"Student id = {id} not found");
            }
            
        }

    }
 }
