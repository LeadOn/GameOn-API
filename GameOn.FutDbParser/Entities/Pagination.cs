namespace GameOn.FutDbParser.Entities
{
    public class Pagination
    {
        public int CountCurrent { get; set; }
        public int CountTotal { get; set; }
        public int PageCurrent { get; set; }
        public int PageTotal { get; set; }
        public int ItemsPerPage { get; set; }

    }
}
