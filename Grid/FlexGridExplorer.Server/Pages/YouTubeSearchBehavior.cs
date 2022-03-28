using C1.Blazor.Core;
using C1.Blazor.Grid;
using C1.DataCollection;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace FlexGridExplorer.Pages
{
    public class YouTubeSearchBehavior : C1Behavior<FlexGrid>
    {
        YouTubeDataCollection _dataCollection;
        string _searchText;

        [Parameter]
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (_searchText == value)
                    return;

                _searchText = value;
                if (AssociatedObject != null)
                    _ = PerformSearch();
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _dataCollection = new YouTubeDataCollection() { PageSize = 25 };
            var grouping = new C1GroupDataCollection<YouTubeVideo>(_dataCollection, false);
            await grouping.GroupAsync("PublishedDay");
            AssociatedObject.ItemsSource = grouping;
            await PerformSearch();
        }

        private async Task PerformSearch()
        {
            try
            {
                //activityIndicator.IsRunning = true;
                //grid.Focus();
                AssociatedObject?.ChangeView(0, 0);
                await _dataCollection.SearchAsync(SearchText);
            }
            finally
            {
                //activityIndicator.IsRunning = false;
            }
        }
    }
}
