namespace Wasla.PR.ViewModels.Admin
{
    public class AdminDriversListViewModel
    {
        public List<AdminDriverListItemViewModel> Drivers { get; set; } = new();
    }

    public class AdminDriverListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }
    }
}