using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Models.Interfaces;

public interface IStudent
{
    public Task<Student> GetStudent(long id);
    public Task<List<Student>> GetAllStudents();
}