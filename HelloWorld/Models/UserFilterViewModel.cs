using HelloWorld.Helpers;

namespace HelloWorld.Models;

public class UserFilterViewModel
{
    public string? Keyword { get; set; }
    public string? DateRange { get; set; } // "MM/DD/YYYY - MM/DD/YYYY"
    public string? SortOrder { get; set; }
    public int PageIndex { get; set; } = 1;
    public PaginatedList<User> Users { get; set; } = null!;

}
