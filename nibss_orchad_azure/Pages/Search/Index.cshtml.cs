using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lucene.Net.QueryParsers.Classic;
using Microsoft.AspNetCore.Mvc;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.DisplayManagement;
using OrchardCore.Entities;
using OrchardCore.Lucene;
using OrchardCore.Lucene.Model;
using OrchardCore.Lucene.Services;
using OrchardCore.Navigation;
using OrchardCore.Search.Abstractions.ViewModels;
using OrchardCore.Settings;
using YesSql;
using YesSql.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;

namespace nibss_orchad_azure.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly ISiteService _siteService;
        private readonly LuceneIndexingService _luceneIndexingService;
        private readonly LuceneIndexSettingsService _luceneIndexSettingsService;
        private readonly LuceneAnalyzerManager _luceneAnalyzerManager;
        private readonly ISearchQueryService _searchQueryService;
        private readonly ISession _session;
        private readonly dynamic _new;


        public IndexModel(

            ISiteService siteService,
            LuceneIndexingService luceneIndexingService,
            LuceneIndexSettingsService luceneIndexSettingsService,
            LuceneAnalyzerManager luceneAnalyzerManager,
            ISearchQueryService searchQueryService,
            ISession session,
            IShapeFactory shapeFactory

            )
        {
            _siteService = siteService;
            _luceneIndexingService = luceneIndexingService;
            _luceneIndexSettingsService = luceneIndexSettingsService;
            _luceneAnalyzerManager = luceneAnalyzerManager;
            _searchQueryService = searchQueryService;
            _session = session;
            _new = shapeFactory;
        }


        [BindProperty]
        public SearchIndexViewModel SearchIndexViewModel { get; set; }


        public async Task OnGet(SearchIndexViewModel viewModel, PagerSlimParameters pagerParameters)
        {
            viewModel.Terms = HtmlEncoder.Default.Encode(viewModel.Terms);
            var siteSettings = await _siteService.GetSiteSettingsAsync();
            var searchSettings = siteSettings.As<LuceneSettings>();
            var luceneSettings = await _luceneIndexingService.GetLuceneSettingsAsync();
            var pager = new PagerSlim(pagerParameters, siteSettings.PageSize);
            var luceneIndexSettings = await _luceneIndexSettingsService.GetSettingsAsync(searchSettings.SearchIndex);
            var analyzer = _luceneAnalyzerManager.CreateAnalyzer(await _luceneIndexSettingsService.GetIndexAnalyzerAsync(luceneIndexSettings.IndexName));
            var queryParser = new MultiFieldQueryParser(LuceneSettings.DefaultVersion, luceneSettings?.DefaultSearchFields, analyzer);
            if (viewModel.Terms != null)
            {
                var query = queryParser.Parse(QueryParser.Escape(viewModel.Terms));

                // Fetch one more result than PageSize to generate "More" links
                var start = 0;
                var end = pager.PageSize + 1;

                if (pagerParameters.Before != null)
                {
                    start = Convert.ToInt32(pagerParameters.Before) - pager.PageSize - 1;
                    end = Convert.ToInt32(pagerParameters.Before);
                }
                else if (pagerParameters.After != null)
                {
                    start = Convert.ToInt32(pagerParameters.After);
                    end = Convert.ToInt32(pagerParameters.After) + pager.PageSize + 1;
                }

                var contentItemIds = (await _searchQueryService.ExecuteQueryAsync(query, searchSettings.SearchIndex, start, end))
                    .ToList();

                // We Query database to retrieve content items.
                IQuery<ContentItem> queryDb;

                if (luceneIndexSettings.IndexLatest)
                {
                    queryDb = _session.Query<ContentItem, ContentItemIndex>()
                        .Where(x => x.ContentItemId.IsIn(contentItemIds) && x.Latest == true)
                        .Take(pager.PageSize + 1);
                }
                else
                {
                    queryDb = _session.Query<ContentItem, ContentItemIndex>()
                        .Where(x => x.ContentItemId.IsIn(contentItemIds) && x.Published == true)
                        .Take(pager.PageSize + 1);
                }

                // Sort the content items by their rank in the search results returned by Lucene.
                var containedItems = (await queryDb.ListAsync()).OrderBy(x => contentItemIds.IndexOf(x.ContentItemId));

                // We set the PagerSlim before and after links
                if (pagerParameters.After != null || pagerParameters.Before != null)
                {
                    pager.Before = start + 1 > 1 ? (start + 1).ToString() : null;
                }

                pager.After = containedItems.Count() == pager.PageSize + 1 ? (end - 1).ToString() : null;

                var model = new SearchIndexViewModel
                {
                    Terms = viewModel.Terms,
                    SearchForm = new SearchFormViewModel("Search__Form") { Terms = viewModel.Terms },
                    SearchResults = new SearchResultsViewModel("Search__Results") { ContentItems = containedItems.Take(pager.PageSize) },
                    Pager = (await _new.PagerSlim(pager)).UrlParams(new Dictionary<string, string>() { { "Terms", viewModel.Terms } })
                };

                this.SearchIndexViewModel = model;
            }
        }
    }
}