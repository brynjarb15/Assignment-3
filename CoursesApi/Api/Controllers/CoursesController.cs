using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoursesApi.Models.DTOModels;
using CoursesApi.Models.EntityModels;
using CoursesApi.Models.ViewModels;
using CoursesApi.Repositories.Exceptions;
using CoursesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
	/// <summary>
	/// A resource for courses
	/// </summary>
	[Route("api/[controller]")]
	public class CoursesController : Controller
	{
		private ICoursesService _coursesService;

		public CoursesController(ICoursesService coursesService)
		{
			  _coursesService = coursesService;
		}

		/// <summary>
		/// A route which should receive all courses from the database
		/// </summary>
		/// <param name="semester">The semester which filters the courses</param>
		/// <returns>A list of CourseDTO's</returns>
		[HttpGet]
		public IActionResult GetCourses(string semester = "20173")
		{
			var courses = _coursesService.GetCourses(semester);
			
			return Ok(courses);
		}

		/// <summary>
		/// A route which should return a course by providing a valid id
		/// </summary>
		/// <param name="courseId">An integer id for a course</param>
		/// <returns>A single CourseDTO object</returns>
		[HttpGet]
		[Route("{courseId:int}", Name = "GetCourseById")]
		public IActionResult GetCourseById(int courseId)
		{
			var courses = _coursesService.GetCourseById(courseId);
			
			if (courses == null)
			{
				return NotFound();
			}

			return Ok(courses);
		}

		[HttpPost]
		[Route("", Name = "AddCourse")]
		public IActionResult AddCourse([FromBody] CourseViewModel course) 
		{
			if (course == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }

			var newCourse = _coursesService.AddCourse(course);

			return Ok(newCourse);
		}

		/// <summary>
		/// Updates a course
		/// </summary>
		/// <param name="courseId">An integer id for a course</param>
		/// <param name="updatedCourse">The updated values for the course</param>
		/// <returns>The updated course</returns>
		[HttpPut]
		[Route("{courseId:int}")]
		public IActionResult UpdateCourse(int courseId, [FromBody] CourseViewModel updatedCourse)
		{
			if (updatedCourse == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }

			var course = _coursesService.UpdateCourse(courseId, updatedCourse);

			if (course == null)
			{
				return NotFound();
			}

			return Ok(course);
		}

		/// <summary>
		/// Retrieves a list of students which are enrolled in a given course
		/// </summary>
		/// <param name="courseId">The id of the course</param>
		/// <returns>A list of StudentDTO's</returns>
		[HttpGet]
		[Route("{courseId:int}/students")]
		public IActionResult GetStudentsByCourseId(int courseId)
		{
			var students = _coursesService.GetStudentsByCourseId(courseId);

			if (students == null)
			{
				return NotFound();
			}

			return Ok(students);
		}

		/// <summary>
		/// Adds a student to a course. Student must be already in the system
		/// </summary>
		/// <param name="courseId">The id of the course</param>
		/// <param name="newStudent">The new student to add</param>
		/// <returns>The newly created student</returns>
		[HttpPost]
		[Route("{courseId:int}/students")]
		public IActionResult AddStudentToCourse(int courseId, [FromBody] StudentViewModel newStudent)
		{
			if (newStudent == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }

			try 
			{
				var response = _coursesService.AddStudentToCourse(courseId, newStudent);
				return Ok(response);
			}
			catch(StudentNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(FullCourseException e)
			{
				return StatusCode(409, e.Message);
			}
			catch(AlreadyInCourseException e)
			{
				return StatusCode(412, e.Message); // The slides had it like this
			}
		}

		/// <summary>
		/// Deletes a student from a course
		/// </summary>
		/// <param name="courseId">The id of the course</param>
		/// <returns>A status code 204 (if successful)</returns>
		[HttpDelete]
		[Route("{courseId:int}")]
		public IActionResult DeleteCourse(int courseId)
		{
			var success = _coursesService.DeleteCourseById(courseId);

			if (!success)
			{
				return NotFound();
			}

			return NoContent();
		}
		///<summary>
		///returns a list of all students on the waiting list for a course
		///</summary>
		[HttpGet]
		[Route("{Id}/waitinglist")]
		public IActionResult GetWaitinglistForCourse(int Id)
		{
			var students = _coursesService.GetWaitinglistForCourse(Id);

			if (students == null)
			{
				return NotFound();
			}
			return Ok(students);
		}
		
		///<summary>
		///Adds a student to the waiting list for a course
		///</summary>
		[HttpPost]
		[Route("{Id}/waitinglist")]
		public IActionResult AddToWaitinglist([FromBody] StudentViewModel student, int Id)
		{
			if (student == null) { return BadRequest(); }
			if (!ModelState.IsValid) { return StatusCode(412); }
			try
			{
				var waitingList = _coursesService.AddToWaitinglist(student, Id);
				return Ok(waitingList);
			}
			catch(StudentNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpDelete]
		[Route("{courseId:int}/students/{ssn}")]
		public IActionResult RemoveStudentFromCourse(int courseId, string ssn)
		{
			try
			{
				_coursesService.RemoveStudentFromCourse(courseId, ssn);
				return NoContent();
			}
			catch(StudentNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(CourseNotFoundException e)
			{
				return NotFound(e.Message);
			}
			catch(StudentWasNotInCourseException e)
			{
				return StatusCode(412, e.Message);
			}
		}
	}
}
