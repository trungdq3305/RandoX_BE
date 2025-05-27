using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Common
{
    public class PaginationResult<T>
    {
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyCollection<T> Items { get; set; } = new List<T>();

        public PaginationResult(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            TotalItems = count;
            CurrentPage = pageSize <= 0 ? 0 : pageNumber;
            PageSize = pageSize <= 0 ? 0 : pageSize;
            TotalPages = (PageSize <= 0 || count <= 0) ? 0 : (int)Math.Ceiling(count / (double)PageSize);
            Items = items.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                           .ToList();
        }

        // Phương thức để kiểm tra nếu có trang trước đó
        public bool HasPreviousPage => CurrentPage > 1;

        // Phương thức để kiểm tra nếu có trang kế tiếp
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
