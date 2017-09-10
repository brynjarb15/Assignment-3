namespace CoursesApi.Models.EntityModels
{
    /// <summary>
    /// This works as a blueprint or template for a given course
    /// </summary>
    public class CourseTemplate
    {
        /// <summary>
        /// The id of the template
        /// </summary>
        /// <returns></returns>
        public int Id { get; set; }
        /// <summary>
        /// The name of the template
        /// </summary>
        /// <returns></returns>
        public string Template { get; set; }
        /// <summary>
        /// The friendly name of the course
        /// </summary>
        /// <returns></returns>
        public string CourseName { get; set; }
    }
}