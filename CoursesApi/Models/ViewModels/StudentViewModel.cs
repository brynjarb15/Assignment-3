using System.ComponentModel.DataAnnotations;

namespace CoursesApi.Models.ViewModels
{
    /// <summary>
    /// A view model for a student
    /// </summary>
    public class StudentViewModel
    {
        /// <summary>
        /// Social security number of the student (kennitala)
        /// </summary>
        /// <returns></returns>
        [Required]
        public string SSN { get; set; }
    }
}