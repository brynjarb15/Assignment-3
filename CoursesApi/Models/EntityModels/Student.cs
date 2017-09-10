namespace CoursesApi.Models.EntityModels
{
    /// <summary>
    /// Entity model for a student
    /// </summary>
    public class Student
    {
        /// <summary>
        /// The student id
        /// </summary>
        /// <returns></returns>
        public int Id { get; set; }
        /// <summary>
        /// The social security number for the student, a.k.a. kennitala
        /// </summary>
        /// <returns></returns>
        public string SSN { get; set; }
        /// <summary>
        /// The name of the student
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
    }
}