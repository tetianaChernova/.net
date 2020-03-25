using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workers.Models;

namespace Workers.Controllers
{
    [Route("/")]
    public class WorkerController : Controller
    {
        private static string _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        static DbContextOptions<WorkersContext> _dbContextOptions =
            new DbContextOptionsBuilder<WorkersContext>().UseNpgsql(_connectionString).Options;

        WorkersContext db = new WorkersContext(_dbContextOptions);

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Users = db.Workers.ToList();
            return View();
        }

        [HttpPost]
        [Route("/save")]
        public IActionResult Post(Worker worker)
        {
            db.Workers.Add(worker);
            db.SaveChanges();
            return RedirectPermanent("/");
        }

        [HttpPost]
        [Route("/update")]
        public IActionResult Put(Worker worker)
        {
            var foundWorker = db.Workers.FirstOrDefault(w => w.Id == worker.Id);
            if (foundWorker != null)
            {
                foundWorker.Name = worker.Name;
                foundWorker.Position = worker.Position;
                foundWorker.Department = worker.Department;
                foundWorker.Salary = worker.Salary;
                db.Entry(foundWorker).State = EntityState.Modified;
            }

            db.SaveChanges();
            return RedirectPermanent("/");
        }

        [HttpPost]
        [Route("/delete")]
        public IActionResult Delete(WorkerDto workerDto)
        {
            // using MyWebApiContext db = new MyWebApiContext(DbContextOptions);
            var worker = new Worker {Id = workerDto.Id};
            db.Workers.Attach(worker);
            db.Workers.Remove(worker);
            db.SaveChanges();
            return RedirectPermanent("/");
        }

        public class WorkerDto
        {
            public int Id { get; set; }
        }
    }
}