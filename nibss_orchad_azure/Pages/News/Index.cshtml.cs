using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OrchardCore;
using OrchardCore.ContentManagement;

namespace nibss_orchad_azure.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly IOrchardHelper _orchard;

       
        

        public IndexModel(IOrchardHelper  orchard)
        {
            _orchard = orchard;
          
            
        }

        public ContentItem Blog { get; private set; }
       
        public IEnumerable<ContentItem> Selection { get; private set; }
        public bool HasNext { get; private set; }
        public bool HasPrevious { get; private set; }
        public int NextPage { get; private set; }
        public int PreviousPage { get; private set; }
        public bool NoData { get; private set; }
        public string Category { get; private set; }

        public async Task OnGetAsync(string pg, string cat)
        {
            Blog = await _orchard.GetContentItemByHandleAsync($"alias:Blog");
          
            Selection = await GetBlogPosts(pg, cat);

        }

        private async Task<IEnumerable<ContentItem>> GetBlogPosts(string pg, string cat) {
            IDictionary<string, object> para = new Dictionary<string, object>();

            int start;

            bool isParsable = Int32.TryParse(pg, out start);

            if (!isParsable)
            {
                start = 1;
            }
            const int count = 6;
            int st = (start - 1) * count;
            

            para.Add("from", st);
            para.Add("size", count);


            IEnumerable<ContentItem> selection;
            if (string.IsNullOrEmpty(cat))
            {
                Category = null;
                selection = await _orchard.ContentQueryAsync("AllBlogPosts", para);
            }
            else
            {
                Category = cat;
                para.Add("cat", cat);
               
                selection = await _orchard.ContentQueryAsync("BlogPostsByCategory", para);
            }

            if (start == 1)
            {
                HasPrevious = false;
            }
            else {
                HasPrevious = true;
            }

            var contentItems = selection as ContentItem[] ?? selection.ToArray();
            if (contentItems.Count() < count)
            {
                HasNext = false;
            }
            else {
                HasNext = true;

            }

            if (!contentItems.Any()) {
                NoData = true;
            }

            NextPage = start + 1;
            PreviousPage = start - 1;
            if (PreviousPage <= 0) {
                PreviousPage = 1;
            }
            return contentItems;
        }
    }
}