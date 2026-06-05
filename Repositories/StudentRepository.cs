
using System.Data;
using Dapper;
using DapperApi.Models;
using DapperApi.Repositories;
using Microsoft.Data.SqlClient;

public class StudentRepository : IStudentRepository
{
    public readonly string _connStr;

    public StudentRepository(IConfiguration config)
    {
        _connStr = config.GetConnectionString("DefaultConnection")!;
    }

    private IDbConnection NewConnection() => new SqlConnection(_connStr);


    public void Create(Student student)
    {
        using var db = NewConnection();
        db.Execute("INSERT INTO Students (Name , Age, Email) VALUES (@Name , @Age, @Email)", student);
    }

    public void Delete(int id)
    {
        using var db = NewConnection();
        db.Execute("DELETE FROM Students WHERE Id = @Id)", new { Id = id });
    }

    public IEnumerable<Student> GetAll()
    {
        using var db = NewConnection();
        return db.Query<Student>("SELECT * FROM STUDENTS");
    }

    public Student? GetById(int id)
    {
        using var db = NewConnection();
        return db.QuerySingleOrDefault<Student>("SELECT * FROM STUDENTS WHERE ID = @id", new { Id = id });
    }
    public Student? GetByName(string name)
    {
        using var db = NewConnection();
        return db.QuerySingleOrDefault<Student>("SELECT * FROM STUDENTS WHERE Name = @name", new { Name = name });
    }

    public void Update(Student student)
    {
        using var db = NewConnection();
        db.Execute("UPDATE Students SET Name=@Name , Age=@Age, Email=@Email WHERE Id=@Id", student);
    }
    public IEnumerable<StudentWithCourses> GetAllWithCourses()
    {
        var sql = @"SELECT s.Id, s.Name , c.Id, c.CourseName
                    FROM Students s
                    JOIN StudentCourses sc ON s.Id = sc.StudentId
                    JOIN Courses c ON sc.CourseId = c.Id
                    ORDER BY s.Id";
        using var db = NewConnection();

        var dict = new Dictionary<int, StudentWithCourses>();

        db.Query<StudentWithCourses, Course, StudentWithCourses>(
        sql,
        (student, course) =>
        {
            if (!dict.TryGetValue(student.Id, out var existing))
            {
                existing = student;
                dict[student.Id] = existing;
            }
            existing.Courses.Add(course);
            return existing;
        },
        splitOn: "Id" // cot phan tach Student / Course
        );

        return dict.Values;
    }

    

}