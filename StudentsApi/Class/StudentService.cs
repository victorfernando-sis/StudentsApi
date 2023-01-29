﻿using System;
using Microsoft.EntityFrameworkCore;
using StudentsApi.Context;
using StudentsApi.Models;

namespace StudentsApi.Class
{
	public class StudentService: IStudentService
	{

        private readonly AppDbContext _context;

        public StudentService(AppDbContext context )
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            return student;
        }

        public async Task<IEnumerable<Student>> GetStudentByName(string name)
        {
            IEnumerable<Student> students;
            if (!string.IsNullOrWhiteSpace(name))
            {
                students = await _context.Students.Where(n => n.Name.Contains(name)).ToListAsync();
            }
            else
            {
                students = await GetStudents();
            }
            return students;
        }
        public async Task CreateStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateStudent(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }
        public async Task DeleteStudent(Student student)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }




    }
}

