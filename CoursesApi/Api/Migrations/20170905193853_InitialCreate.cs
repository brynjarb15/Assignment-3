using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Courses",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					CourseTemplate = table.Column<string>(type: "TEXT", nullable: true),
					EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
					Semester = table.Column<string>(type: "TEXT", nullable: true),
					StartDate = table.Column<DateTime>(type: "TEXT", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Courses", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "CourseTemplates",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					CourseName = table.Column<string>(type: "TEXT", nullable: true),
					Template = table.Column<string>(type: "TEXT", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CourseTemplates", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Enrollments",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					CourseId = table.Column<int>(type: "INTEGER", nullable: false),
					StudentSSN = table.Column<string>(type: "TEXT", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Enrollments", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Students",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					Name = table.Column<string>(type: "TEXT", nullable: true),
					SSN = table.Column<string>(type: "TEXT", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Students", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Courses");

			migrationBuilder.DropTable(
				name: "CourseTemplates");

			migrationBuilder.DropTable(
				name: "Enrollments");

			migrationBuilder.DropTable(
				name: "Students");
		}
	}
}
