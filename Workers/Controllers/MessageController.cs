using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workers.Models;

namespace Workers.Controllers
{
    [Route("/send")]
    public class MessageController : Controller
    {
        private static string _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        static DbContextOptions<WorkersContext> _dbContextOptions =
            new DbContextOptionsBuilder<WorkersContext>().UseNpgsql(_connectionString).Options;

        WorkersContext db = new WorkersContext(_dbContextOptions);

        [HttpGet]
        public ViewResult Index()
        {
            ViewBag.Messages = db.Messages.ToList();
            return View();
        }
        
        [HttpPost]
        [Route("/send")]
        public IActionResult Post()
        {
            String messageToSend = Request.Form["radio"].ToString();
            List<Worker> workers = db.Workers.ToList();
            Console.WriteLine(workers);
            var sender = Environment.GetEnvironmentVariable("USERNAME");
            workers.ForEach(x => SendEmailAsync(sender, x.Email, messageToSend).GetAwaiter());
            return RedirectPermanent("/");
        }
        
        private static async Task SendEmailAsync(string sender, string receiver, string text)
        {
            MailAddress from = new MailAddress(sender, "Tetiana");
            MailAddress to = new MailAddress(receiver);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Spammer";
            m.Body = text;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            var username = Environment.GetEnvironmentVariable("USERNAME");
            var emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");
            smtp.Credentials = new NetworkCredential(username, emailPassword);
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            Console.WriteLine("Лист відправлено");
        }
    }
}