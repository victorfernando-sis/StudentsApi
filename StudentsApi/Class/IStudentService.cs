using System;
using StudentsApi.Models;

namespace StudentsApi.Class
{
	public interface IStudentService
	{
		Task<IEnumerable<Student>> GetStudents();

		Task<Student> GetStudent(int id);

		Task<IEnumerable<Student>> GetStudentByName(string name);

		Task CreateStudent(Student student);

		Task UpdateStudent(Student student);

        Task DeleteStudent(Student student);
	}
}

