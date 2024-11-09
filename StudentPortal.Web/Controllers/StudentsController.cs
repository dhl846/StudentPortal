using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                Subscribed = viewModel.Subscribed,
            };
            await dbContext.Students.AddAsync(student);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var student = await dbContext.Students.ToListAsync();
            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var student = await dbContext.Students.FindAsync(Id);
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            var updateStudent = await dbContext.Students.FindAsync(student.Id);

            if(updateStudent is not null)
            {
                updateStudent.Name = student.Name;
                updateStudent.Email = student.Email;
                updateStudent.Phone = student.Phone;
                updateStudent.Subscribed = student.Subscribed;

                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List","Students");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Student student)
        {
            var deleteStudent = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == student.Id);
            if(deleteStudent is not null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }
    }
}
