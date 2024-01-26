using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoheasApparel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoheasApparel.Models.Models
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int ItemTotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageTotalCount { get; set; }

     

        public ProductViewModel ProductVM { get; set; }

        //paged list constructor
        public PagedList(List<T> items,int count, int pageIndex, int pageSize, ProductViewModel productViewModel) 
        {
            PageIndex = pageIndex;
            PageTotalCount = (int)Math.Ceiling(count / (decimal)pageSize);
            Items = items;
            ProductVM = productViewModel;
        }

        public bool HasNextPage => (PageIndex < PageTotalCount);
       

        public bool HasPrevPage => (PageIndex > 1);
        public int FirstItemIndex => (PageIndex - 1) * PageSize + 1;
        public int LastItemIndex => Math.Min(PageIndex * PageSize, ItemTotalCount);

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize, ProductViewModel productViewModel)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>( items, count, pageIndex, pageSize ,productViewModel);
        }
    }
}
