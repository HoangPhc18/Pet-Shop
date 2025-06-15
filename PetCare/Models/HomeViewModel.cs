namespace PetCare.Models
{
    public class HomeViewModel
    {
        public List<Service> Services { get; set; }
        public Stats Stats { get; set; }
        public List<Feedback> Feedback { get; set; }
    }

    public class Service
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
    }

    public class Stats
    {
        public int MembersOfStaff { get; set; }
        public int PetsTreatedLastYear { get; set; }
        public int YearsInBusiness { get; set; }
    }

    public class Feedback
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public int Rating { get; set; }
    }
}
