namespace DSD_App.Models
{
    public class Game
    {
        public string? GameName { get; set; }
        public List<Boss>? Bosses { get; set; }
        public List<Restriction>? Restrictions { get; set; }
        public string? Submission { get; set; }
    }
}
