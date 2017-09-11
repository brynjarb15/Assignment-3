using System;
using System.Linq;
using System.Collections.Generic;
using CoursesApi.Models.DTOModels;
using CoursesApi.Models.EntityModels;
using AutoMapper;
using CoursesApi.Models.ViewModels;
using CoursesApi.Repositories.Exceptions;


namespace CoursesApi.Repositories
{
	public class CoursesRepository : ICoursesRepository
	{
		private AppDataContext _db;

		#region PrivateFunctions


		private Course checkIfCourseExsists(int courseId)
		{
			Course course = (from c in _db.Courses
						  where c.Id == courseId
						  select c).SingleOrDefault();
			if (course == null)
			{
				throw new CourseNotFoundException();
			}
			return course;
		}

		private Student checkIfStudentExsists(string ssn)
		{
			var student = (from s in _db.Students
						   where s.SSN == ssn
						   select s).SingleOrDefault();
			if (student == null)
			{
				throw new StudentNotFoundException();
			}
			return student;
		}
	#endregion
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
			//var course = _db.Courses.SingleOrDefault(c => c.Id == courseId);
			var course = checkIfCourseExsists(courseId);

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
						   where sr.NotRemoved
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
			//var course = _db.Courses.SingleOrDefault(c => c.Id == courseId);
			var course = checkIfCourseExsists(courseId);

			course.StartDate = updatedCourse.StartDate;
			course.EndDate = updatedCourse.EndDate;
			course.MaxStudents = updatedCourse.MaxStudents;

			_db.SaveChanges();

			return GetCourseById(courseId);
		}

		public IEnumerable<StudentDTO> GetStudentsByCourseId(int courseId)
		{
			//var course = _db.Courses.SingleOrDefault(c => c.Id == courseId);
			var course = checkIfCourseExsists(courseId);

			var students = (from sr in _db.Enrollments
							where sr.CourseId == courseId
							join s in _db.Students on sr.StudentSSN equals s.SSN
							where sr.NotRemoved
							select new StudentDTO
							{
								SSN = s.SSN,
								Name = s.Name
							}).ToList();

			return students;
		}

		public StudentDTO AddStudentToCourse(int courseId, StudentViewModel newStudent)
		{
			// get the course and throw error if it is not found
			var course = checkIfCourseExsists(courseId);

			// get the student and throw error if it is not found
			var student = checkIfStudentExsists(newStudent.SSN);

			// get the number of students in the course and check if the new student can enter
			int numberOfStudents = _db.Enrollments.Count(s => s.CourseId == courseId && s.NotRemoved);
			if (numberOfStudents >= course.MaxStudents)
			{
				throw new FullCourseException();
			}

			Enrollment enrollment = _db.Enrollments.SingleOrDefault(e => e.StudentSSN == student.SSN && e.CourseId == courseId);

			if (enrollment == null ) // Student is not in course
			{
				_db.Enrollments.Add(
					new Enrollment {CourseId = courseId, StudentSSN = newStudent.SSN, NotRemoved = true}
				);
				_db.SaveChanges();

			}
			else if (enrollment.NotRemoved) // Student is in course
			{
				throw new AlreadyInCourseException();
			} 
			else 
			{
				// student has been removed from the course but will now re-enroll
				enrollment.NotRemoved = true;
				_db.SaveChanges();
			}

			//if the student was on the waiting list for that course he is removed.
			removeFromWaitingList(student.SSN, courseId);



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
			var course = checkIfCourseExsists(courseId);

			_db.Courses.Remove(course);
			_db.SaveChanges();

			return true;
		}

		public CourseDetailsDTO AddCourse(CourseViewModel newCourse)
		{
			var entity = new Course { 
									  CourseTemplate = newCourse.CourseID,
									  Semester = newCourse.Semester,
									  StartDate = newCourse.StartDate,
									  EndDate = newCourse.EndDate,
									  MaxStudents = newCourse.MaxStudents 
									};

			_db.Courses.Add(entity);
			_db.SaveChanges();
			var createdCourse = GetCourseById(entity.Id);

			return createdCourse;
			
		}

		public IEnumerable<StudentDTO> GetWaitinglistForCourse(int courseId)
		{
			var students = (from a in _db.Students
							join b in _db.WaitingList on a.SSN equals b.StudentSSN
							join c in _db.Courses on b.CourseId equals c.Id
							where b.CourseId == courseId
							select new StudentDTO
							{
								Name = a.Name,
								SSN = a.SSN
							}).ToList();
			return students;
		}
		public StudentDTO AddToWaitinglist(StudentViewModel student, int Id)
		{
			if(GetStudentFromWaitingList(student.SSN, Id) != null)
			{
				throw new AlreadyOnWaitingListException();
			}
			// get the course and throw error if it is not found
			var course = checkIfCourseExsists(Id);

			// get the student and throw error if it is not found
			var stu = checkIfStudentExsists(student.SSN);

			//Búa til fall með þessu(líka notað í add to course)
			Enrollment enrollment = _db.Enrollments.SingleOrDefault(e => e.StudentSSN == student.SSN && e.CourseId == Id);

			if (enrollment != null && enrollment.NotRemoved)
			{
				throw new AlreadyInCourseException();
			}

			var waitingList = new WaitingList{ CourseId = Id, StudentSSN = student.SSN };
			_db.WaitingList.Add(waitingList);
			_db.SaveChanges();

			return new StudentDTO
			{
				SSN = stu.SSN,
				Name = (from st in _db.Students
					where st.SSN == stu.SSN
					select st).SingleOrDefault().Name
			};
		}

		//help function
		public WaitingList GetStudentFromWaitingList(string studentSSN, int courseId)
		{
			var student = (from stu in _db.WaitingList
						where stu.StudentSSN == studentSSN && stu.CourseId == courseId
						select stu).SingleOrDefault();
			return student;
		}

		//func for rule 3
		public void removeFromWaitingList(string studentSSN, int courseId)
		{
			var student = GetStudentFromWaitingList(studentSSN, courseId);
			if(student != null)
			{
				_db.WaitingList.Remove(student);
			}
			_db.SaveChanges();
		}

		public void RemoveStudentFromCourse(int courseId, string ssn)
		{
			var course = checkIfCourseExsists(courseId);
			var student = checkIfStudentExsists(ssn);
			var enrollment = (from sr in _db.Enrollments
							  where sr.CourseId == courseId &&
									sr.StudentSSN == ssn
							  select sr).SingleOrDefault();
			if (enrollment == null)
			{
				throw new StudentWasNotInCourseException();
			}
			enrollment.NotRemoved = false;
			_db.SaveChanges();
		}
	}
}

