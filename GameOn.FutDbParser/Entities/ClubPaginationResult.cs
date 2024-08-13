using GameOn.Domain;

namespace GameOn.FutDbParser.Entities
{
    public class ClubPaginationResult
    {
        /// <summary>
        /// Gets or sets Pagination.
        /// </summary>
        public Pagination Pagination { get; set; } = new Pagination();

        /// <summary>
        /// Gets or sets Items.
        /// </summary>
        public List<FifaTeam> Items { get; set; } = new List<FifaTeam>();
    }
}
