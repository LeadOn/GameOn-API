using GameOn.Domain;

namespace GameOn.FutDbParser.Entities
{
    public class ClubPaginationResult
    {
        public Pagination Pagination { get; set; }
        public List<FifaTeam> Items { get; set; }
    }
}
