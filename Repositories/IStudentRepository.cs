using DapperApi.Models;
namespace DapperApi.Repositories;

public interface IStudentRepository
{
    IEnumerable<Student> GetAll();
    Student? GetById(int id);

    Student? GetByName(string name);
    void Create(Student student);
    void Update(Student student);
    void Delete(int id);
    public IEnumerable<StudentWithCourses> GetAllWithCourses();
}