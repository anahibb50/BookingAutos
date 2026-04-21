namespace Booking.Autos.DataManagement.Common
{
    public class DataPagedResult<T>
    {
        // 📦 Datos
        public IEnumerable<T> Items { get; set; } = new List<T>();

        // 📄 Paginación
        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);

        // 🔁 Navegación
        public bool HasPrevious => Page > 1;

        public bool HasNext => Page < TotalPages;

        // 🧠 Constructor vacío
        public DataPagedResult() { }

        // 🧠 Constructor completo
        public DataPagedResult(IEnumerable<T> items, int totalRecords, int page, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            Page = page;
            PageSize = pageSize;
        }
    }
}