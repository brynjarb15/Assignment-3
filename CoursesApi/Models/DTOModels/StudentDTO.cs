namespace CoursesApi.Models.DTOModels
{
    /// <summary>
    /// Student model which is exposed to the user
    /// </summary>
    public class StudentDTO
    {
        /// <summary>
        /// Social security number (kennitala) of the student
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