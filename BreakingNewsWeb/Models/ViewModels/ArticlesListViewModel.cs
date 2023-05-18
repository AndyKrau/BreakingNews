using BreakingNewsWeb.Models;

namespace BreakingNewsWeb.Models.ViewModels
{
    public class ArticlesListViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
