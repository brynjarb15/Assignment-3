using System;
using System.Linq;
using System.Collections.Generic;
using CoursesApi.Models.DTOModels;
using CoursesApi.Models.EntityModels;
using AutoMapper;
using CoursesApi.Models.ViewModels;

namespace CoursesApi.Repositories
{
	public class CoursesRepository : ICoursesRepository
	{
		private AppDataContext _db;

		public CoursesRepository(AppDataContext db)
		{
			_db = db;
		}

		public IEnumerable<CoursesListItemDTO> GetCourses(string semsester)
		{
			var courses = (from c in _db.Courses
						   join t in _db.CourseTemplates on c.CourseTemplate equals t.Template
						   where c.Semester == semsester
						   select new CoursesListItemDTO
						   {
							   Id = c.Id,
							   Name = t.CourseName,
							   NumberOfStudents = (_db.Enrollments.Count(s => s.CourseId == c.Id))
						   }).ToList();

			return courses;
		}

		public CourseDetailsDTO GetCourseById(int courseId)
		{
			var course = _db.Courses.SingleOrDefault(c => c.Id == courseId);

			if (course == null)
			{
				return null;
			}

			var result = new CourseDetailsDTO
			{
				Id = course.Id,
				StartDate = course.StartDate,
				EndDate = course.EndDate,
				Name = _db.CourseTemplates.Where(t => t.Template == course.CourseTemplate)
														 .Select(c => c.CourseName).FirstOrDefault(),
				MaxStudents = course.MaxStudents,
				Students = (from sr in _db.Enrollments
						   where sr.CourseId == course.Id
						   join s in _db.Students on sr.StudentSSN equals s.SSN
						   select new StudentDTO
						   {
							   SSN = s.SSN,
							   Name = s.Name
						   }).ToList()
			};

			return result;

		}
		public CourseDetailsDTO UpdateCourse(int courseId, CourseViewModel updatedCourse)
		{
			var course = _db.Courses.SingleOrDefault(c => c.Id == courseId);

			if (course == null)
			{
				return null;
			}

			course.StartDate = updatedCourse.StartDate;
			course.EndDate = updatedCourse.EndDate;
			course.MaxStudents = updatedCourse.MaxStudents;

			_db.SaveChanges();

			return GetCourseById(courseId);
		}

		public IEnumerable<StudentDTO> GetStudentsByCourseId(int courseId)
		{
			var course = _db.Courses.SingleOrDefault(c => c.Id == courseId);

			if (course == null)
			{
				return null;
			}

			var students = (from sr in _db.Enrollments
							where sr.CourseId == courseId
							join s in _db.Students on sr.StudentSSN equals s.SSN
							select new StudentDTO
							{
								SSN = s.SSN,
								Name = s.Name
							}).ToList();

			return students;
		}

		public StudentDTO AddStudentToCourse(int courseId, StudentViewModel newStudent)
		{
			var course = (from c in _db.Courses
						  where c.Id == courseId
						  select c).SingleOrDefault();

			var student = (from s in _db.Students
						   where s.SSN == newStudent.SSN
						   select s).SingleOrDefault();

			if (course == null || student == null)
			{
				return null;
			}

			_db.Enrollments.Add(
				new Enrollment {CourseId = courseId, StudentSSN = newStudent.SSN}
			);
			_db.SaveChanges();

			return new StudentDTO
			{
				SSN = newStudent.SSN,
				Name = (from st in _db.Students
					   where st.SSN == newStudent.SSN
					   select st).SingleOrDefault().Name
			};
		}

		public bool DeleteCourseById(int courseId)
		{
			var course = (from c in _db.Courses
							where c.Id == courseId
							select c).SingleOrDefault();

			if (course == null)
			{
				return false;
			}
			_db.Courses.Remove(course);
			_db.SaveChanges();

			return true;
		}

		public CourseDetailsDTO AddCourse(CourseViewModel newCourse)
		{
			var entity = new Course { CourseTemplate = newCourse.CourseID,
									  Semester = newCourse.Semester,
									  StartDate = newCourse.StartDate,
									  EndDate = newCourse.EndDate,
									  MaxStudents = newCourse.MaxStudents };

			_db.Courses.Add(entity);
			_db.SaveChanges();

			return new CourseDetailsDTO
			{
				Id = entity.Id,
				Name = _db.CourseTemplates.FirstOrDefault(ct => ct.Template == newCourse.CourseID).CourseName,
				StartDate = entity.StartDate,
				EndDate = entity.EndDate,
				Students = _db.Enrollments
					.Where(e => e.CourseId == entity.Id)
					.Join(_db.Students, enroll => enroll.StudentSSN, stud => stud.SSN, (e, s) => s)
					.Select(s => new StudentDTO {
						SSN = s.SSN,
						Name = s.Name
					}).ToList()
			};
		}
	}
}

