namespace Entities.RedditEntities
{
    public class ListingData
    {
        public string modhash { get; set; }
        public string whitelist_status { get; set; }
        public ChildNode[] children { get; set; }
        public string after { get; set; }
        public string before { get; set; }
    }
}