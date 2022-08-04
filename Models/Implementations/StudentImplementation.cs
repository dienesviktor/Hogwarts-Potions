using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Models.Implementations;

public class StudentImplementation : IStudent
{
    private HogwartsContext _context;

    public StudentImplementation(HogwartsContext context)
    {
        _context = context;
    }
    public async Task<Student> GetStudent(long studentId)
    {
        return await _context.Students
            .Include(s => s.Room)
            .FirstOrDefaultAsync(student => student.ID == studentId);
    }

    public async Task<List<Student>> GetAllStudents()
    {
        return await _context.Students
            .Include(s => s.Room)
            .ToListAsync();
    }
}