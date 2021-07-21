namespace NorthwindApp.DTO
{
    public class BreadcrumbsConfiguration
    {
        public string PageName { get; set; }
        public BreadcrumbsMode Mode { get; set; }
    }

    public enum BreadcrumbsMode
    {
        List,
        Edit,
        Create
    }
}
