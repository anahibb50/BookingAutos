using System;
using System.Collections.Generic;

namespace Microservicio.Clientes.DataAccess.Common
{
    public class PagedResult<T> where T : class
    {
        // Datos de la página (materializados)
        public IReadOnlyList<T> Items { get; private set; }

        // Metadatos
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalRecords { get; private set; }

        // Cálculos
        public int TotalPages => PageSize == 0
            ? 0
            : (int)Math.Ceiling(TotalRecords / (double)PageSize);

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedResult(
            IEnumerable<T> items,
            int totalRecords,
            int currentPage,
            int pageSize)
        {
            // 🔥 Materializamos para evitar reevaluaciones
            Items = items is IReadOnlyList<T> list ? list : new List<T>(items);

            // 🔥 Protección básica (sin exagerar)
            TotalRecords = totalRecords;
            CurrentPage = currentPage < 1 ? 1 : currentPage;
            PageSize = pageSize <= 0 ? 10 : pageSize;
        }
    }
}