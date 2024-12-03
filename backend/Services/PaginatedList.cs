namespace PublicTransportNavigator.Services
{
    public static class PaginatedList<T> 
    {
        public static IEnumerable<T> Create(IEnumerable<T> source, int pageIndex, int pageSize = 20)
        {
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return items;
        }
    }
}
