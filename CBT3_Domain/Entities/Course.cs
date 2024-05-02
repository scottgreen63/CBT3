using System.Collections.Frozen;

namespace CBT3_Domain.Entities;

public sealed class Course : BaseAuditableEntity
{
    public Course(CourseID id) : base(id) { }
    public string CourseName { get; set; }
    public int ExpiryMonths { get; set; }
    public string CourseType { get; set; }
    public List<Lesson> Lessons { get; set; } = new();
}
public class CourseID : BaseId<string>
{
    public CourseID(string id) : base(id) { }
}