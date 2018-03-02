using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Autyan.NiChiJou.Core.Mvc.TagHelpers
{
    [HtmlTargetElement("nav", Attributes = AutyanPaginationAttributeName)]
    public class AutyanPaginationTagHelper : TagHelper
    {
        private const string AutyanPaginationAttributeName = "autyan-pagination";

        private const string AutyanPaginationSkipAttributeName = "autyan-pagination-skip";

        private const string AutyanPaginationTakeAttributeName = "autyan-pagination-take";

        private const string AutyanPaginationTotalItemAttributeName = "autyan-pagination-totalItem";

        private const string AutyanPaginationControllerAttributeName = "autyan-pagination-controller";

        private const string AutyanPaginationActionAttributeName = "autyan-pagination-action";

        private const string AutyanPaginationRouteValueAttributeName = "autyan-pagination-routeValue";

        private const string AutyanPaginationWidthAttributeName = "autyan-pagination-width";

        private int? _totalPage;

        private int? _pagerCount;

        private string _linkHref;

        private int? _pageIndex;

        private readonly StringBuilder _tageBuilder = new StringBuilder();

        [HtmlAttributeName(AutyanPaginationSkipAttributeName)]
        public int? Skip { get; set; }

        [HtmlAttributeName(AutyanPaginationTakeAttributeName)]
        public int? Take { get; set; }

        [HtmlAttributeName(AutyanPaginationTotalItemAttributeName)]
        public int TotalItem { get; set; }

        [HtmlAttributeName(AutyanPaginationControllerAttributeName)]
        public string Controller { get; set; }

        [HtmlAttributeName(AutyanPaginationActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(AutyanPaginationRouteValueAttributeName)]
        public string RouteValue { get; set; }

        [HtmlAttributeName(AutyanPaginationWidthAttributeName)]
        public double Width { get; set; } = 102;

        public int PagerCount
        {
            get
            {
                if (_pagerCount != null) return _pagerCount.Value;
                _pagerCount = Convert.ToInt32((Width - 68) / 34.0);

                return _pagerCount.Value;
            }
        }

        private int? TotalPage
        {
            get
            {
                if (_totalPage != null) return _totalPage.Value;
                var maxItempage = TotalItem / Take;
                if (TotalItem % Take > 0)
                {
                    maxItempage += 1;
                }

                _totalPage = maxItempage;
                return _totalPage;
            }
        }

        private int? PageIndex => _pageIndex ?? (_pageIndex = Skip / Take + 1);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //append nav element
            _tageBuilder.Append("<ul class=\"pagination\">");

            //append previous button
            _tageBuilder.Append(HasPreviousPagers
                ? $"<li><a href=\"{LinkHref}?skip=0&take={Take}\" aria-label=\"Head\"><span aria-hidden=\"true\">&laquo;</span></a></li>"
                : "<li class=\"disabled\"><a href=\"#\" aria-label=\"Head\"><span aria-hidden=\"true\">&laquo;</span></a></li>");

            //append pagers before currentPage
            var beforeIndexCount = PageIndex - PagerCount / 2;
            var startIndex = beforeIndexCount <= 1 ? 1 : beforeIndexCount;
            var currentIndex = startIndex;
            while (currentIndex < PageIndex)
            {
                _tageBuilder.Append($"<li><a href=\"{LinkHrefWithQueryParamters(startIndex)}\">{startIndex}</a></li>");
                currentIndex += 1;
            }

            //append currentPager
            _tageBuilder.Append($"<li class=\"active\"><a href=\"{LinkHrefWithQueryParamters(PageIndex)}\">{PageIndex}</a></li>");
            startIndex += 1;

            //append pagers after currentPage
            var finalPage = (startIndex + PagerCount) > TotalPage ? TotalPage : PagerCount;
            while (currentIndex < finalPage)
            {
                _tageBuilder.Append($"<li><a href=\"{LinkHrefWithQueryParamters(startIndex)}\">{startIndex}</a></li>");
                currentIndex += 1;
            }

            _tageBuilder.Append(IsFinalPager
                ? "<li class=\"disabled\"><a href=\"#\" aria-label=\"End\"><span aria-hidden=\"true\">&raquo;</span></a></li>"
                : $"<li href=\"{LinkHref}?skip={(PageIndex - 1) * Take}&take={Take}\"><a href=\"#\" aria-label=\"End\"><span aria-hidden=\"true\">&raquo;</span></a></li>");

            _tageBuilder.Append("</ul>");

            output.Content.AppendHtml(_tageBuilder.ToString());
        }

        private bool HasPreviousPagers => PageIndex > 1;

        private bool IsFinalPager => PageIndex == TotalPage;

        private string LinkHref
        {
            get
            {
                if (_linkHref == null)
                {
                    var builder = new StringBuilder();
                    if (Controller == null)
                    {
                        builder.Append("/");
                        _linkHref = builder.ToString();
                        return _linkHref;
                    }

                    builder.Append(Controller);
                    if (Action != null)
                    {
                        builder.Append("/").Append(Action);
                    }

                    if (RouteValue != null)
                    {
                        builder.Append("/").Append(RouteValue);
                    }
                }

                return _linkHref;
            }
        }

        private string LinkHrefWithQueryParamters(int? index) => $"{LinkHref}?skip={(index - 1) * Take}&take={Take}";
    }
}
