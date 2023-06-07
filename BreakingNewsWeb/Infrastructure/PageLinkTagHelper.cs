using BreakingNewsWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BreakingNewsWeb.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]

    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext? ViewContext { get; set; }
        public PagingInfo? PageModel { get; set; }
        public string PageAction { get; set; }
        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public string PageClassDisabled { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext != null || PageModel != null)
            {
                IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
                TagBuilder result = new TagBuilder("div");

                // кнопка Previous
                TagBuilder tagPrevious = new TagBuilder("a");
                tagPrevious.Attributes["href"] = urlHelper.Action(PageAction, new { page = (PageModel.CurrentPage - 1) });
                tagPrevious.AddCssClass(PageClass);
                tagPrevious.AddCssClass(PageModel.CurrentPage == 1 ? PageClassDisabled : PageClassNormal);
                tagPrevious.InnerHtml.Append("Previous".ToString());
                result.InnerHtml.AppendHtml(tagPrevious);

                // основная группа кнопок
                for (int i = 1; i <= PageModel.TotalPages(); i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                    if (PageClassesEnabled)
                    {
                        //if ((PageModel.CurrentPage - i) > 4 || (PageModel.CurrentPage - i) < -4)
                        //{
                        //    tag.AddCssClass("d-none");
                        //}
                        tag.AddCssClass(PageClass);
                        tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                    }
                    tag.InnerHtml.Append(i.ToString());
                    result.InnerHtml.AppendHtml(tag);
                }

                // кнопка Next 
                TagBuilder tagNext = new TagBuilder("a");
                tagNext.Attributes["href"] = urlHelper.Action(PageAction, new { page = (PageModel.CurrentPage + 1) });
                tagNext.AddCssClass(PageClass);
                tagNext.AddCssClass(PageModel.CurrentPage == PageModel.TotalPages() ? PageClassDisabled : PageClassNormal);
                tagNext.InnerHtml.Append("Next".ToString());
                result.InnerHtml.AppendHtml(tagNext);

                output.Content.AppendHtml(result.InnerHtml);
            }
        }
    }
}
