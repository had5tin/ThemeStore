using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.Common.UI
{
    public class PaginationModels 
    {
        // Summary:
        //     The index of the first item in the page.
        public int FirstItem { get; set; }
        //
        // Summary:
        //     Whether there are pages after the current page.
        public bool HasNextPage { get; set; }

        public bool DisplayFormat { get; set; }
        //
        // Summary:
        //     Whether there are pages before the current page.
        public bool HasPreviousPage { get; set; }
        //
        // Summary:
        //     The index of the last item in the page.
        public int LastItem { get; set; }
        //
        // Summary:
        //     The current page number
        public int PageNumber { get; set; }
        //
        // Summary:
        //     The number of items in each page.
        public int PageSize { get; set; }
        //
        // Summary:
        //     The total number of items.
        public int TotalRecords { get; set; }
        //
        // Summary:
        //     The total number of pages.
        public int TotalPages { get; set; }

        public int NumAroundPages { get; set; }

        public string ClassActive { get; set; }

        public string ClassFirst { get; set; }
        public string ClassPrev { get; set; }
        public string ClassNext { get; set; }
        public string ClassLast { get; set; }

        public string Themes { get; set; }

        public string FilePath { get; set; }

        public string PageQueryName { get; set; }

    }
}
