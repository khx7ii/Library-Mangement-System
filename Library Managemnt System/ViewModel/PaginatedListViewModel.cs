namespace Library_Managemnt_System.ViewModel
{
    public class PaginatedListViewModel<T>
    {

        public List<T> Items { get; set; }
        public int ? CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public bool HasPreviousPage => CurrentPage > 1; 
        public bool HasNextPage => CurrentPage < TotalPages; 

    }
}
