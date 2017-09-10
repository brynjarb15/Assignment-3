using System;
using System.ComponentModel.DataAnnotations;

namespace CoursesApi.Models.ViewModels
{
    /// <summary>
    /// A view model of the course, which is the data required from users
    /// </summary>
    public class CourseViewModel
    {
        /// <summary>
        /// The id of the course, e.g. T-111-PROG
        /// </summary>
        /// <returns></returns>
        [Required]
        public string CourseID { get; set; }
        /// <summary>
        /// The semester the course is in
        /// </summary>
        /// <returns></returns>
        [Required]
        [RegularExpression("[0-9]{5}")]
        public string Semester { get; set; }
        /// <summary>
        /// The starting date of the course
        /// </summary>
        /// <returns></returns>
        [Required]
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// The end date of the course
        /// </summary>
        /// <returns></returns>
        [Required]
        public DateTime? EndDate { get; set; }
    }
}