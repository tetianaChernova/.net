namespace Workers.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public int Salary { get; set; }
    }
}