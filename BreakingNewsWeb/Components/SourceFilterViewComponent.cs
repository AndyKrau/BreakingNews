using DBConnection.Models.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace BreakingNewsWeb.Components
{
    public class SourceFilterViewComponent:ViewComponent
    {
        private readonly NewsContext newsDb;
        public SourceFilterViewComponent(NewsContext newsContext)
        {
            // получаем контекст newsDB
            newsDb = newsContext;
        }
        public IViewComponentResult Invoke()
        {
            var data = newsDb.articles
                .Select(x => x.Source)
                .Distinct()
                .OrderBy(x => x);
            return View(data);
        }
    }
}
