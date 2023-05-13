using System;

namespace Account.ViewModels
{
    public class Job
    {
        public string Role { get; set; }
        public string MinQ { get; set; }
        public int Experience { get; set; }
        public string Location { get; set; }
        public DateTime DeadLine { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
    }
}