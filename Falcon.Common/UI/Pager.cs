using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using System.Collections;


namespace Falcon.Common.UI
{
    public class Pager
    {
        private readonly PaginationModels _pagination;
        private readonly HttpRequestBase _request;

        private string _paginationFormat = "Hiển thị kết quả từ {0} - {1} trên tổng {2} ";
        private string _paginationStoreAdminFormat = "Hiển thị kết quả từ <span>{0}</span> - <span>{1}</span> trên tổng <span>{2}</span> ";
        private string _paginationSingleFormat = "Showing {0} of {1} ";
        private string _paginationFirst = "Đầu";
        private string _paginationPrev = "Trang trước";
        private string _paginationNext = "Trang sau";
        private string _paginationLast = "Cuối";
        private string _pageQueryName = "page";
        private int _NumAroundPages = 2;
        private bool _HasNextPage = false;
        private bool _HasPreviousPage = false;
        private bool _DisplayFormat = false;

        private string _classActive = "active";
        private string _classFirst = "p-first";
        private string _classPrev = "p-prev";
        private string _classNext = "p-next";
        private string _classLast = "p-last";
        private string _classLink = "p-link";
        public int TotalPages { get; set; }
        private string _FilePath = "";

        private Func<int, string> urlBuilder;
        private readonly string _strParam;

        /// <summary>
        /// Creates a new instance of the Pager class.
        /// </summary>
        /// <param name="pagination">The IPagination datasource</param>
        /// <param name="request">The current HTTP Request</param>
        public Pager(PaginationModels pagination, HttpRequestBase request, string strParam = "")
        {
            _pagination = pagination;
            if (!string.IsNullOrEmpty(_pagination.PageQueryName))
            {
                _pageQueryName = _pagination.PageQueryName;
            }
            _request = request;
            _strParam = strParam;
            urlBuilder = CreateDefaultUrl;
        }
        /// <summary>
        /// Số bản ghi được hiển thị
        /// </summary>
        public Pager DisplayFormat(bool displayFormat)
        {
            _DisplayFormat = displayFormat;
            return this;
        }

        public Pager FilePath(string filePath)
        {
            _FilePath = filePath;
            return this;
        }

        /// <summary>
        /// Số bản ghi được hiển thị
        /// </summary>
        public Pager HasNextPage(bool hasNext)
        {
            _HasNextPage = hasNext;
            return this;
        }

        /// <summary>
        /// Số bản ghi được hiển thị
        /// </summary>
        public Pager HasPreviousPage(bool HasPrevious)
        {
            _HasPreviousPage = HasPrevious;
            return this;
        }

        /// <summary>
        /// Số bản ghi được hiển thị
        /// </summary>
        public Pager NumAroundPages(int numberPage)
        {
            _NumAroundPages = numberPage;
            return this;
        }

        /// <summary>
        /// Specifies the query string parameter to use when generating pager links. The default is 'page'
        /// </summary>
        public Pager QueryParam(string queryStringParam)
        {
            _pageQueryName = queryStringParam;
            return this;
        }

        /// <summary>
        /// Specifies the format to use when rendering a pagination containing a single page. 
        /// The default is 'Showing {0} of {1}' (eg 'Showing 1 of 3')
        /// </summary>
        public Pager SingleFormat(string format)
        {
            _paginationSingleFormat = format;
            return this;
        }

        /// <summary>
        /// Specifies the format to use when rendering a pagination containing multiple pages. 
        /// The default is 'Showing {0} - {1} of {2}' (eg 'Showing 1 to 3 of 6')
        /// </summary>
        public Pager Format(string format)
        {
            _paginationFormat = format;
            return this;
        }

        /// <summary>
        /// Text for the 'first' link.
        /// </summary>
        public Pager First(string first)
        {
            _paginationFirst = first;
            return this;
        }

        /// <summary>
        /// Text for the 'prev' link
        /// </summary>
        public Pager Previous(string previous)
        {
            _paginationPrev = previous;
            return this;
        }

        /// <summary>
        /// Text for the 'next' link
        /// </summary>
        public Pager Next(string next)
        {
            _paginationNext = next;
            return this;
        }

        /// <summary>
        /// Text for the 'last' link
        /// </summary>
        public Pager Last(string last)
        {
            _paginationLast = last;
            return this;
        }

        /// <summary>
        /// class for the 'active' link
        /// </summary>
        public Pager ClassActive(string active)
        {
            _classActive = active;
            return this;
        }

        /// <summary>
        /// class for the 'first' link
        /// </summary>
        public Pager ClassFirst(string first)
        {
            _classFirst = first;
            return this;
        }

        /// <summary>
        /// class for the 'prev' link
        /// </summary>
        public Pager ClassPrev(string prev)
        {
            _classPrev = prev;
            return this;
        }

        /// <summary>
        /// class for the 'next' link
        /// </summary>
        public Pager ClassNext(string next)
        {
            _classNext = next;
            return this;
        }

        /// <summary>
        /// class for the 'last' link
        /// </summary>
        public Pager ClassLast(string last)
        {
            _classLast = last;
            return this;
        }

        /// <summary>
        /// Uses a lambda expression to generate the URL for the page links.
        /// </summary>
        /// <param name="urlBuilder">Lambda expression for generating the URL used in the page links</param>
        public Pager Link(Func<int, string> urlBuilder)
        {
            this.urlBuilder = urlBuilder;
            return this;
        }

        public string NextLink()
        {
            if (_pagination.PageNumber < _pagination.TotalPages)
            {
                return urlBuilder(_pagination.PageNumber + 1);
            }
            else
            {
                return null;
            }
        }

        public string PrevLink()
        {
            if (_pagination.PageNumber > 1)
            {
                return urlBuilder(_pagination.PageNumber - 1);
            }
            else
            {
                return null;
            }
        }

        public MvcHtmlString Render()
        {
            if (_pagination.TotalRecords == 0)
            {
                return null;
            }

            var divPage = new TagBuilder("div");
            divPage.AddCssClass("pagination");

            if (_DisplayFormat)
            {
                divPage.Attributes.Add("style", "width:100%");
                var spanTag = new TagBuilder("div");
                spanTag.AddCssClass("paginationLeft");

                // pagination Format
                if (_pagination.PageSize == 1)
                    spanTag.InnerHtml = string.Format(_paginationSingleFormat, _pagination.FirstItem, _pagination.TotalRecords);
                else
                {
                    //spanTag.InnerHtml = string.Format(_paginationFormat, _pagination.FirstItem, _pagination.LastItem, _pagination.TotalRecords);
                    int maxRecords = 0;
                    if (TotalPages == _pagination.PageNumber)
                        maxRecords = _pagination.TotalRecords;
                    else
                        maxRecords = _pagination.PageNumber * _pagination.PageSize;
                    spanTag.InnerHtml = string.Format(_paginationFormat, ((_pagination.PageNumber - 1) * _pagination.PageSize) + 1, maxRecords, _pagination.TotalRecords);
                }
                divPage.InnerHtml += spanTag.ToString();
            }
            // lấy số trang dựa vào tổng số bản ghi
            TotalPages = _pagination.TotalRecords % _pagination.PageSize == 0 ? _pagination.TotalRecords / _pagination.PageSize : (_pagination.TotalRecords / _pagination.PageSize) + 1;

            if (TotalPages > 1)
            {
                var ulPage = new TagBuilder("div");

                //first
                //if (_pagination.PageNumber > _NumAroundPages + 1)
                //    ulPage.InnerHtml += CreatePageLink1(1, _paginationFirst);
                if (_pagination.PageNumber > TotalPages)
                    _pagination.PageNumber = TotalPages;
                // prev
                if (_pagination.PageNumber != 1)
                {
                    ulPage.InnerHtml += CreatePageLink(_pagination.PageNumber - 1, _paginationPrev);
                }
                else
                {
                    ulPage.InnerHtml += CreatePageLink(_pagination.PageNumber, _paginationPrev);
                }

                //// link item
                int _pageNumber = _pagination.PageNumber;

                //int iPage = 0;

                ////Xem xét đến các trường hợp:
                //if (_pageNumber <= _NumAroundPages)
                //{
                //    if (TotalPages <= _NumAroundPages)
                //    {
                //        for (iPage = 1; iPage <= TotalPages; iPage++)
                //        {
                //            ulPage.InnerHtml += CreatePageLink(iPage, iPage.ToString());
                //        }
                //    }
                //    else
                //    {
                //        for (iPage = 1; iPage < _NumAroundPages + _pageNumber; iPage++)
                //        {
                //            if (iPage <= TotalPages)
                //                ulPage.InnerHtml += CreatePageLink(iPage, iPage.ToString());
                //        }
                //        // link ...
                //        //if (_pageNumber + _NumAroundPages <= TotalPages)
                //        //    ulPage.InnerHtml += CreatePageLink(_pageNumber + _NumAroundPages, "...");
                //    }
                //}
                //else
                //{
                //    //if (_pageNumber - _NumAroundPages > 1)
                //    //{
                //    //    // link ...
                //    //    ulPage.InnerHtml += CreatePageLink((_pageNumber - _NumAroundPages) - 1, "...");
                //    //}
                //    if (TotalPages <= _pageNumber + _NumAroundPages)
                //    {
                //        for (iPage = _pageNumber - _NumAroundPages; iPage <= TotalPages; iPage++)
                //        {
                //            ulPage.InnerHtml += CreatePageLink(iPage, iPage.ToString());
                //        }
                //    }
                //    else
                //    {
                //        for (iPage = _pageNumber - _NumAroundPages; iPage <= _pageNumber + _NumAroundPages; iPage++)
                //        {
                //            ulPage.InnerHtml += CreatePageLink(iPage, iPage.ToString());
                //        }
                //        // link ...
                //        //ulPage.InnerHtml += CreatePageLink((_pageNumber + _NumAroundPages)+1, "...");
                //    }
                //}


                // full page
                if (_pageNumber <= 4)
                {
                    switch (_pageNumber)
                    {
                        case 1:
                        case 2:
                            ulPage.InnerHtml += CreatePageLink(1, "1");
                            ulPage.InnerHtml += CreatePageLink(2, "2");
                            if (3 <= TotalPages)
                                ulPage.InnerHtml += CreatePageLink(3, "3");
                            if (4 <= TotalPages && TotalPages < 6)
                                ulPage.InnerHtml += CreatePageLink(4, "4");
                            if (5 == TotalPages)
                                ulPage.InnerHtml += CreatePageLink(5, "5");
                            if (5 < TotalPages && TotalPages <= 7)
                            {
                                ulPage.InnerHtml += CreateSpanText("...");
                                ulPage.InnerHtml += CreateSpanDisableText(TotalPages.ToString());
                            }
                            break;
                        case 3:
                            ulPage.InnerHtml += CreatePageLink(1, "1");
                            ulPage.InnerHtml += CreatePageLink(2, "2");
                            ulPage.InnerHtml += CreatePageLink(3, "3");
                            if (4 <= TotalPages)
                                ulPage.InnerHtml += CreatePageLink(4, "4");
                            if (5 <= TotalPages && TotalPages < 7)
                                ulPage.InnerHtml += CreatePageLink(5, "5");
                            if (6 == TotalPages)
                                ulPage.InnerHtml += CreatePageLink(6, "6");
                            if (7 == TotalPages)
                            {
                                ulPage.InnerHtml += CreateSpanText("...");
                                ulPage.InnerHtml += CreateSpanDisableText(TotalPages.ToString());
                            }
                            break;
                        case 4:
                            ulPage.InnerHtml += CreatePageLink(1, "1");
                            ulPage.InnerHtml += CreatePageLink(2, "2");
                            ulPage.InnerHtml += CreatePageLink(3, "3");
                            ulPage.InnerHtml += CreatePageLink(4, "4");
                            if (5 <= TotalPages)
                                ulPage.InnerHtml += CreatePageLink(5, "5");
                            if (6 <= TotalPages && TotalPages <= 7)
                                ulPage.InnerHtml += CreatePageLink(6, "6");
                            if (7 == TotalPages)
                                ulPage.InnerHtml += CreatePageLink(7, "7");
                            break;
                        default:
                            break;
                    }
                    if (TotalPages > 7)
                    {
                        ulPage.InnerHtml += CreateSpanText("...");
                        ulPage.InnerHtml += CreateSpanDisableText(TotalPages.ToString());
                    }
                }

                // full page
                //if (_pageNumber >= TotalPages - 3 && TotalPages >= 8)
                //{
                //    ulPage.InnerHtml += CreatePageLink(1, "1");
                //    ulPage.InnerHtml += CreateSpanText("...");

                //    for (int i = TotalPages - 4; i <= TotalPages ; i++)
                //    {
                //        ulPage.InnerHtml += CreatePageLink(i, i.ToString());    
                //    }
                //}

                if (_pageNumber > 4 && TotalPages < 8 && _pageNumber <= TotalPages)
                {
                    if (TotalPages == 5)
                    {
                        ulPage.InnerHtml += CreatePageLink(1, "1");
                        ulPage.InnerHtml += CreatePageLink(2, "2");
                        ulPage.InnerHtml += CreatePageLink(3, "3");
                        ulPage.InnerHtml += CreatePageLink(4, "4");
                        ulPage.InnerHtml += CreatePageLink(5, "5");
                    }
                    if (TotalPages > 5)
                    {
                        ulPage.InnerHtml += CreatePageLink(1, "1");
                        ulPage.InnerHtml += CreateSpanText("...");
                        if (_pageNumber == TotalPages)
                        {
                            ulPage.InnerHtml += CreatePageLink(_pageNumber - 2, (_pageNumber - 2).ToString());
                        }
                        ulPage.InnerHtml += CreatePageLink(_pageNumber - 1, (_pageNumber - 1).ToString());
                        for (int i = _pageNumber; i <= TotalPages; i++)
                        {
                            ulPage.InnerHtml += CreatePageLink(i, i.ToString());
                        }
                    }
                }

                if (_pageNumber > 4 && _pageNumber <= TotalPages && TotalPages >= 8)
                {
                    ulPage.InnerHtml += CreatePageLink(1, "1");
                    ulPage.InnerHtml += CreateSpanText("...");
                    if (_pageNumber == TotalPages)
                    {
                        ulPage.InnerHtml += CreatePageLink(_pageNumber - 2, (_pageNumber - 2).ToString());
                    }
                    ulPage.InnerHtml += CreatePageLink(_pageNumber - 1, (_pageNumber - 1).ToString());
                    if (_pageNumber < TotalPages - 3)
                    {
                        ulPage.InnerHtml += CreatePageLink(_pageNumber, _pageNumber.ToString());
                        ulPage.InnerHtml += CreatePageLink(_pageNumber + 1, (_pageNumber + 1).ToString());
                        ulPage.InnerHtml += CreateSpanText("...");
                        ulPage.InnerHtml += CreateSpanDisableText(TotalPages.ToString());
                    }
                    else
                    {
                        for (int i = _pageNumber; i <= TotalPages; i++)
                        {
                            ulPage.InnerHtml += CreatePageLink(i, i.ToString());
                        }
                    }
                }

                //Next
                if (_pagination.PageNumber != TotalPages)
                {
                    ulPage.InnerHtml += CreatePageLink(_pagination.PageNumber + 1, _paginationNext);
                }
                else
                {
                    ulPage.InnerHtml += CreatePageLink(_pagination.PageNumber, _paginationNext);
                }

                // Last
                //if (_pagination.PageNumber + _NumAroundPages < TotalPages)
                //    ulPage.InnerHtml += CreatePageLink1(TotalPages, _paginationLast);

                divPage.InnerHtml += ulPage.ToString();
            }
            else
            {
                return null;
            }
            return new MvcHtmlString(divPage.ToString());
        }

        public MvcHtmlString RenderAdmin()
        {
            if (_pagination.TotalRecords == 0)
            {
                return null;
            }

            var divPage = new TagBuilder("div");
            divPage.AddCssClass("t-pager t-reset");


            // lấy số trang dựa vào tổng số bản ghi
            TotalPages = _pagination.TotalRecords % _pagination.PageSize == 0 ? _pagination.TotalRecords / _pagination.PageSize : (_pagination.TotalRecords / _pagination.PageSize) + 1;

            if (_DisplayFormat)
            {
                var spanTag = new TagBuilder("div");
                spanTag.AddCssClass("t-status-text");
                spanTag.Attributes.Add("style", "float:right;");
                // pagination Format
                if (_pagination.PageSize == 1)
                    spanTag.InnerHtml = string.Format(_paginationSingleFormat, _pagination.FirstItem, _pagination.TotalRecords);
                else
                {
                    int maxRecords = 0;
                    if (TotalPages == _pagination.PageNumber)
                        maxRecords = _pagination.TotalRecords;
                    else
                        maxRecords = _pagination.PageNumber * _pagination.PageSize;
                    spanTag.InnerHtml = string.Format(_paginationFormat, ((_pagination.PageNumber - 1) * _pagination.PageSize) + 1, maxRecords, _pagination.TotalRecords);
                }
                divPage.InnerHtml += spanTag.ToString();
            }

            if (TotalPages > 1)
            {
                var aPage = new TagBuilder("a");

                //first
                //if (_pagination.PageNumber != 1)
                divPage.InnerHtml += CreatePageLinkAdmin(1, _paginationFirst);

                // prev
                //if (_HasPreviousPage && _pagination.PageNumber != 1)
                divPage.InnerHtml += CreatePageLinkAdmin(_pagination.PageNumber - 1, _paginationPrev);


                // link item
                int _pageNumber = _pagination.PageNumber;
                int iPage = 0;

                var divNumber = new TagBuilder("div");
                divNumber.AddCssClass("t-numeric");

                //Xem xét đến các trường hợp:
                if (_pageNumber <= _NumAroundPages)
                {
                    if (TotalPages <= _NumAroundPages)
                    {
                        for (iPage = 1; iPage <= TotalPages; iPage++)
                        {
                            divNumber.InnerHtml += CreatePageLinkAdmin1(iPage, iPage.ToString());
                        }
                    }
                    else
                    {
                        for (iPage = 1; iPage < _NumAroundPages + _pageNumber + 1; iPage++)
                        {
                            if (iPage <= TotalPages)
                                divNumber.InnerHtml += CreatePageLinkAdmin1(iPage, iPage.ToString());
                        }
                        // link ...
                        //if ((_pageNumber + _NumAroundPages) + 1 <= TotalPages)
                        //    divNumber.InnerHtml += CreatePageLinkAdmin1((_pageNumber+_NumAroundPages) + 1, "...");
                    }
                }
                else
                {
                    //if (_pageNumber - _NumAroundPages > 1)
                    //{
                    //    // link ...
                    //    divNumber.InnerHtml += CreatePageLinkAdmin1((_pageNumber - _NumAroundPages) - 1, "...");
                    //}
                    if (TotalPages <= _pageNumber + _NumAroundPages)
                    {
                        for (iPage = _pageNumber - _NumAroundPages; iPage <= TotalPages; iPage++)
                        {
                            divNumber.InnerHtml += CreatePageLinkAdmin1(iPage, iPage.ToString());
                        }
                    }
                    else
                    {
                        for (iPage = _pageNumber - _NumAroundPages; iPage <= _pageNumber + _NumAroundPages; iPage++)
                        {
                            divNumber.InnerHtml += CreatePageLinkAdmin1(iPage, iPage.ToString());
                        }
                        // link ...
                        //divNumber.InnerHtml += CreatePageLinkAdmin1((_pageNumber + _NumAroundPages) + 1, "...");
                    }
                }
                divPage.InnerHtml += divNumber.ToString();
                //Next
                //if (_HasNextPage && _pagination.PageNumber != TotalPages)
                divPage.InnerHtml += CreatePageLinkAdmin(_pagination.PageNumber + 1, _paginationNext);

                // Last
                //if (_pagination.PageNumber < TotalPages)
                divPage.InnerHtml += CreatePageLinkAdmin(TotalPages, _paginationLast);
            }
            return new MvcHtmlString(divPage.ToString());
        }

        public MvcHtmlString RenderStoreAdmin()
        {
            if (_pagination.TotalRecords == 0)
            {
                return null;
            }

            var divWrapper = new TagBuilder("div");
            divWrapper.AddCssClass("row");
            var divCol = new TagBuilder("div");
            divCol.AddCssClass("col-md-12");

            // lấy số trang dựa vào tổng số bản ghi
            TotalPages = _pagination.TotalRecords % _pagination.PageSize == 0 ? _pagination.TotalRecords / _pagination.PageSize : (_pagination.TotalRecords / _pagination.PageSize) + 1;

            if (_DisplayFormat)
            {
                var divPagingInfo = new TagBuilder("div");
                divPagingInfo.AddCssClass("pull-left pagination-info");
                // pagination Format
                if (_pagination.PageSize == 1)
                    divPagingInfo.InnerHtml = string.Format(_paginationStoreAdminFormat, _pagination.FirstItem, _pagination.TotalRecords);
                else
                {
                    int maxRecords = 0;
                    if (TotalPages == _pagination.PageNumber)
                        maxRecords = _pagination.TotalRecords;
                    else
                        maxRecords = _pagination.PageNumber * _pagination.PageSize;
                    divPagingInfo.InnerHtml = string.Format(_paginationStoreAdminFormat, ((_pagination.PageNumber - 1) * _pagination.PageSize) + 1, maxRecords, _pagination.TotalRecords);
                }
                divCol.InnerHtml += divPagingInfo.ToString();
            }

            if (TotalPages > 1)
            {
                var divPaging = new TagBuilder("div");
                divPaging.AddCssClass("pull-right");

                var ulTag = new TagBuilder("ul");
                ulTag.AddCssClass("pagination");

                if (_pagination.PageNumber > TotalPages)
                    _pagination.PageNumber = TotalPages;

                if (_pagination.PageNumber != 1)
                {
                    ulTag.InnerHtml += string.Format("<li class=\"prev \"><a title=\"Trước\" href=\"{0}\"><i class=\"icon-angle-left\"></i></a></li>", urlBuilder(_pagination.PageNumber - 1));
                }
                else
                {
                    ulTag.InnerHtml += "<li class=\"prev disabled\"><a title=\"Trước\" href=\"javascript:;\"><i class=\"icon-angle-left\"></i></a></li>";
                }

                int _pageNumber = _pagination.PageNumber;

                if (_pageNumber <= 4)
                {
                    switch (_pageNumber)
                    {
                        case 1:
                        case 2:
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(1, "1");
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(2, "2");
                            if (3 <= TotalPages)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(3, "3");
                            if (4 <= TotalPages && TotalPages < 6)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(4, "4");
                            if (5 == TotalPages)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(5, "5");
                            if (5 < TotalPages && TotalPages <= 7)
                            {
                                ulTag.InnerHtml += "<li><span>...</span></li>";
                                //ulTag.InnerHtml += CreateSpanDisableText(TotalPages.ToString());
                            }
                            break;
                        case 3:
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(1, "1");
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(2, "2");
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(3, "3");
                            if (4 <= TotalPages)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(4, "4");
                            if (5 <= TotalPages && TotalPages < 7)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(5, "5");
                            if (6 == TotalPages)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(6, "6");
                            if (7 == TotalPages)
                            {
                                ulTag.InnerHtml += "<li><span>...</span></li>";
                                //ulTag.InnerHtml += CreateSpanDisableText(TotalPages.ToString());
                            }
                            break;
                        case 4:
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(1, "1");
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(2, "2");
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(3, "3");
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(4, "4");
                            if (5 <= TotalPages)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(5, "5");
                            if (6 <= TotalPages && TotalPages <= 7)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(6, "6");
                            if (7 == TotalPages)
                                ulTag.InnerHtml += CreatePageLinkStoreAdmin(7, "7");
                            break;
                        default:
                            break;
                    }
                    if (TotalPages > 7)
                    {
                        ulTag.InnerHtml += "<li><span>...</span></li>";
                    }
                }

                if (_pageNumber > 4 && TotalPages < 8 && _pageNumber <= TotalPages)
                {
                    if (TotalPages == 5)
                    {
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(1, "1");
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(2, "2");
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(3, "3");
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(4, "4");
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(5, "5");
                    }
                    else if (TotalPages > 5)
                    {
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(1, "1");
                        ulTag.InnerHtml += "<li><span>...</span></li>";
                        if (_pageNumber == TotalPages)
                        {
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(_pageNumber - 2, (_pageNumber - 2).ToString());
                        }
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(_pageNumber - 1, (_pageNumber - 1).ToString());
                        for (int i = _pageNumber; i <= TotalPages; i++)
                        {
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(i, i.ToString());
                        }
                    }
                }

                if (_pageNumber > 4 && _pageNumber <= TotalPages && TotalPages >= 8)
                {
                    ulTag.InnerHtml += CreatePageLinkStoreAdmin(1, "1");
                    ulTag.InnerHtml += "<li><span>...</span></li>";
                    if (_pageNumber == TotalPages)
                    {
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(_pageNumber - 2, (_pageNumber - 2).ToString());
                    }
                    ulTag.InnerHtml += CreatePageLinkStoreAdmin(_pageNumber - 1, (_pageNumber - 1).ToString());
                    if (_pageNumber < TotalPages - 3)
                    {
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(_pageNumber, _pageNumber.ToString());
                        ulTag.InnerHtml += CreatePageLinkStoreAdmin(_pageNumber + 1, (_pageNumber + 1).ToString());
                        ulTag.InnerHtml += "<li><span>...</span></li>";
                    }
                    else
                    {
                        for (int i = _pageNumber; i <= TotalPages; i++)
                        {
                            ulTag.InnerHtml += CreatePageLinkStoreAdmin(i, i.ToString());
                        }
                    }
                }

                //Next
                if (_pagination.PageNumber != TotalPages)
                {
                    ulTag.InnerHtml += string.Format("<li class=\"next\"><a title=\"Tiếp\" href=\"{0}\"><i class=\"icon-angle-right\"></i></a></li>", urlBuilder(_pagination.PageNumber + 1));
                }
                else
                {
                    ulTag.InnerHtml += "<li class=\"next disabled\"><a title=\"Tiếp\" href=\"javascript:;\"><i class=\"icon-angle-right\"></i></a></li>";
                }

                divPaging.InnerHtml += ulTag.ToString();
                divCol.InnerHtml += divPaging.ToString();
                divCol.InnerHtml += "<div class=\"clearfix\"></div>";
            }

            divWrapper.InnerHtml = divCol.ToString();
            return new MvcHtmlString(divWrapper.ToString());
        }

        public override string ToString()
        {
            if (_pagination.TotalRecords == 0)
            {
                return null;
            }

            var builder = new StringBuilder();
            builder.Append("<div class='pagination'>");
            builder.Append("<span class='paginationLeft'>");
            if (_pagination.PageSize == 1)
            {
                builder.AppendFormat(_paginationSingleFormat, _pagination.FirstItem, _pagination.TotalRecords);
            }
            else
            {
                builder.AppendFormat(_paginationFormat, _pagination.FirstItem, _pagination.LastItem, _pagination.TotalRecords);
            }
            builder.Append("</span>");

            // lấy số trang dựa vào tổng số bản ghi
            int TotalPages = _pagination.TotalRecords % _pagination.PageSize == 0 ? _pagination.TotalRecords / _pagination.PageSize : (_pagination.TotalRecords / _pagination.PageSize) + 1;

            if (TotalPages > 1)
            {
                builder.Append("<span class='paginationRight'>");

                if (_pagination.PageNumber == 1)
                {
                    builder.Append(_paginationFirst);
                }
                else
                {
                    builder.Append(CreatePageLink(1, _paginationFirst));
                }

                builder.Append(" | ");

                if (_pagination.HasPreviousPage)
                {
                    builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev));
                }
                else
                {
                    builder.Append(_paginationPrev);
                }


                builder.Append(" | ");

                for (int i = _pagination.PageNumber; i < TotalPages; i++)
                {
                    builder.Append(CreatePageLink(i, i.ToString()));
                }

                if (_pagination.HasNextPage)
                {
                    builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext));
                }
                else
                {
                    builder.Append(_paginationNext);
                }


                builder.Append(" | ");

                int lastPage = TotalPages;

                if (_pagination.PageNumber < lastPage)
                {
                    builder.Append(CreatePageLink(lastPage, _paginationLast));
                }
                else
                {
                    builder.Append(_paginationLast);
                }

                builder.Append("</span>");
            }

            builder.Append(@"</div>");

            return new MvcHtmlString(builder.ToString()).ToString();
        }

        private string CreatePageLinkStoreAdmin(int pageNumber, string text)
        {
            var liTag = new TagBuilder("li");
            if (pageNumber == _pagination.PageNumber)
            {
                liTag.AddCssClass("active");
                liTag.InnerHtml += string.Format("<a href=\"javascript:;\">{0}</a>", text);
            }
            else
            {
                liTag.InnerHtml += string.Format("<a href=\"{0}\">{1}</a>", urlBuilder(pageNumber), text);
            }

            return liTag.ToString();
        }

        private string CreatePageLink(int pageNumber, string text)
        {
            var liPage = new TagBuilder("span");
            if (pageNumber == _pagination.PageNumber)
            {

                if (text.Equals(_paginationPrev) || text.Equals(_paginationNext))
                {
                    if (text.Equals(_paginationPrev))
                    {
                        liPage.AddCssClass("span-prev");
                        liPage.InnerHtml = string.Format("<span class='span-prev-first-icon icon'></span><span class='span-prev-string'>{0}</span>", text);
                    }
                    else
                    {
                        liPage.AddCssClass("span-next");
                        liPage.InnerHtml = string.Format("<span class='span-next-string'>{0}</span><span class='span-next-last-icon icon'></span>", text);
                    }
                }
                else
                {
                    liPage.AddCssClass(_classActive);
                    liPage.InnerHtml = string.Format("{0}", text);
                }
            }
            else
            {

                if (text.Equals(_paginationPrev) || text.Equals(_paginationNext))
                {
                    if (pageNumber == _pagination.PageNumber + 1)
                    {
                        liPage.AddCssClass(_classNext);
                        liPage.InnerHtml = string.Format("<a href=\"{0}\"><span class='span-next-string'>{1}</span><span class='span-next-icon icon'></span></a>", urlBuilder(pageNumber), text);
                    }
                    else if (pageNumber == _pagination.PageNumber - 1)
                    {
                        liPage.AddCssClass(_classPrev);
                        liPage.InnerHtml = string.Format("<a href=\"{0}\"><span class='span-prev-icon icon'></span><span class='span-prev-string'>{1}</span></a>", urlBuilder(pageNumber), text);
                    }
                }
                else
                {
                    liPage.AddCssClass(_classLink);
                    liPage.InnerHtml = string.Format("<a href=\"{0}\">{1}</a>", urlBuilder(pageNumber), text);
                }
            }
            return liPage.ToString();
        }

        private string CreateSpanText(string text)
        {
            var liPage = new TagBuilder("span");
            liPage.AddCssClass("span-text");
            liPage.InnerHtml = string.Format("{0}", text);
            return liPage.ToString();
        }

        private string CreateSpanDisableText(string text)
        {
            var liPage = new TagBuilder("span");
            liPage.AddCssClass("span-text-disable");
            liPage.InnerHtml = string.Format("{0}", text);
            return liPage.ToString();
        }

        private string CreatePageLinkAdmin1(int pageNumber, string text)
        {
            TagBuilder htmlPage = null;

            if (pageNumber == _pagination.PageNumber)
            {
                htmlPage = new TagBuilder("span");
                htmlPage.AddCssClass("t-state-active");
                htmlPage.InnerHtml = string.Format("{0}", text);
            }
            else
            {
                htmlPage = new TagBuilder("a");
                htmlPage.AddCssClass("t-link");
                htmlPage.Attributes.Add("href", urlBuilder(pageNumber));
                htmlPage.Attributes.Add("data-page-number", pageNumber.ToString());
                htmlPage.InnerHtml = string.Format("{0}", text);
            }
            return htmlPage.ToString();
        }

        private string CreatePageLinkAdmin(int pageNumber, string text)
        {
            var aPage = new TagBuilder("a");
            aPage.Attributes.Add("data-page-number", pageNumber.ToString());
            if (pageNumber == TotalPages)
            {
                aPage.AddCssClass("t-link");
                if (_pagination.PageNumber != TotalPages)
                    aPage.Attributes.Add("href", urlBuilder(pageNumber));
                else
                {
                    aPage.AddCssClass("t-state-disabled");
                    aPage.Attributes.Add("href", "javascript:");
                }
                if (text.Equals(_paginationLast))
                    aPage.InnerHtml = string.Format("<span class=\"{0}\">{1}</span>", "t-icon t-arrow-last", text);
                else
                    aPage.InnerHtml = string.Format("<span class=\"{0}\">{1}</span>", "t-icon t-arrow-next", text);
            }
            else if ((_pagination.PageNumber + 1) == pageNumber)
            {
                aPage.AddCssClass("t-link");
                if (_pagination.PageNumber != TotalPages)
                    aPage.Attributes.Add("href", urlBuilder(pageNumber));
                else
                {
                    aPage.AddCssClass("t-state-disabled");
                    aPage.Attributes.Add("href", "javascript:");
                }
                aPage.InnerHtml = string.Format("<span class=\"{0}\">{1}</span>", "t-icon t-arrow-next", text);
            }
            else if (pageNumber == 1)
            {
                aPage.AddCssClass("t-link");
                if (_pagination.PageNumber != 1)
                    aPage.Attributes.Add("href", urlBuilder(pageNumber));
                else
                {
                    aPage.AddCssClass("t-state-disabled");
                    aPage.Attributes.Add("href", "javascript:");
                }
                if (text.Equals(_paginationFirst))
                    aPage.InnerHtml = string.Format("<span class=\"{0}\">{1}</span>", "t-icon t-arrow-first", text);
                else
                    aPage.InnerHtml = string.Format("<span class=\"{0}\">{1}</span>", "t-icon t-arrow-prev", text);
            }
            else if ((_pagination.PageNumber - 1) == pageNumber)
            {
                aPage.AddCssClass("t-link");
                if (_pagination.PageNumber != 1)
                    aPage.Attributes.Add("href", urlBuilder(pageNumber));
                else
                {
                    aPage.AddCssClass("t-state-disabled");
                    aPage.Attributes.Add("href", "javascript:");
                }
                aPage.InnerHtml = string.Format("<span class=\"{0}\">{1}</span>", "t-icon t-arrow-prev", text);
            }
            //else
            //{
            //    aPage.InnerHtml = string.Format("<a class=\"t-link\" href=\"{0}\">{1}</a>", urlBuilder(pageNumber), text);
            //}
            return aPage.ToString();
        }

        private string CreatePageLink1(int pageNumber, string text)
        {
            var liPage = new TagBuilder("span");
            if (pageNumber == TotalPages)
                liPage.AddCssClass(_classLast);
            else if (pageNumber == 1)
                liPage.AddCssClass(_classFirst);
            liPage.InnerHtml = string.Format("<a href=\"{0}\">{1}</a>", urlBuilder(pageNumber), text);
            return liPage.ToString();
        }

        private string CreateDefaultUrl(int pageNumber)
        {
            string queryString = "";
            if (string.IsNullOrEmpty(_strParam))
            {
                queryString = CreateQueryString(_request.QueryString);
            }
            else
            {
                queryString = CreateQueryString(HttpUtility.ParseQueryString(_strParam));
            }
            string filePath = "";
            if (_pagination.FilePath == null)
                filePath = _request.FilePath;
            else
                filePath = _pagination.FilePath;
            if (filePath.Contains("?"))
            {
                if (pageNumber == 1)
                {
                    return string.Format("{0}{1}", filePath, queryString);
                }
                else
                {
                    return string.Format("{0}&{1}={2}{3}", filePath, _pageQueryName, pageNumber, queryString);
                }
            }
            else
            {
                if (pageNumber == 1)
                {
                    if (!string.IsNullOrEmpty(queryString))
                    {
                        return string.Format("{0}?{1}", filePath, queryString.Substring(1));
                    }
                    else
                    {
                        return filePath;
                    }
                }
                else
                {
                    return string.Format("{0}?{1}={2}{3}", filePath, _pageQueryName, pageNumber, queryString);
                }
            }
        }

        private string CreateQueryString(NameValueCollection values)
        {
            var builder = new StringBuilder();

            foreach (string key in values.Keys)
            {
                //if (key == "page")
                if (key == _pageQueryName)
                //Don't re-add any existing 'page' variable to the querystring - this will be handled in CreatePageLink.
                {
                    continue;
                }

                foreach (var value in values.GetValues(key))
                {
                    builder.AppendFormat("&{0}={1}", key, HttpUtility.UrlEncode(value));
                }
            }

            return builder.ToString();
        }
    }
}
