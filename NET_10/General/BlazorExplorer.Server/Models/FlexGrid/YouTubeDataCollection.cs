using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorExplorer
{
    public class YouTubeDataCollection : C1CursorDataCollection<YouTubeVideo>
    {
        private string _q;
        //private string _orderBy = "relevance";

        public YouTubeDataCollection()
        {
            PageSize = 20;
        }

        public int PageSize { get; set; }

        public override bool HasMoreItems
        {
            get { return _q != null && base.HasMoreItems; }
        }

        private SemaphoreSlim _searchSemaphore = new SemaphoreSlim(1);

        public async Task SearchAsync(string q)
        {
            //Sets the filter string and wait the Delay time.
            _q = q;
            try
            {
                await _searchSemaphore.WaitAsync();

                if (_q != q)//If the text changed means there was another keystroke.
                    return;

                await RefreshAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            finally
            {
                _searchSemaphore.Release();
            }
        }

        protected override async Task<Tuple<string, IReadOnlyList<YouTubeVideo>>> GetPageAsync(int startingIndex, string pageToken, int? count = null, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpresssion = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await LoadVideosAsync(_q, "date", pageToken, PageSize, cancellationToken);
        }

        public static async Task<Tuple<string, IReadOnlyList<YouTubeVideo>>> LoadVideosAsync(string q, string orderBy, string pageToken, int maxResults, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(q)) return new Tuple<string, IReadOnlyList<YouTubeVideo>>(default, Array.Empty<YouTubeVideo>());

            var youtubeUrl = string.Format("https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&q={0}&order={1}&maxResults={2}{3}&key={4}", Uri.EscapeUriString(q), orderBy, maxResults, string.IsNullOrWhiteSpace(pageToken) ? "" : "&pageToken=" + pageToken, "AIzaSyCGIvlreIsHaOVVoR2JAPJnbHkSUv3v4y0");

            var client = new HttpClient();
            var response = await client.GetAsync(youtubeUrl, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var videos = new List<YouTubeVideo>();
                var text = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize(text, YouTubeSearchResultContext.Default.YouTubeSearchResult);
                foreach (var item in result.Items)
                {
                    var video = new YouTubeVideo()
                    {
                        Title = System.Net.WebUtility.HtmlDecode(item.Snippet.Title),
                        Description = item.Snippet.Description,
                        Thumbnail = item.Snippet.Thumbnails.Default.Url,
                        Link = "http://www.youtube.com/watch?v=" + item.Id.VideoId,
                        ChannelTitle = item.Snippet.ChannelTitle,
                        PublishedAt = DateTime.Parse(item.Snippet.PublishedAt),
                    };
                    videos.Add(video);
                }
                return new Tuple<string, IReadOnlyList<YouTubeVideo>>(result.NextPageToken, videos);
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }

        }
    }

    [JsonSerializable(typeof(YouTubeSearchResult))]
    [JsonSerializable(typeof(YouTubeSearchItemResult))]
    [JsonSerializable(typeof(YouTubeVideoId))]
    [JsonSerializable(typeof(YouTubeSnippet))]
    [JsonSerializable(typeof(YouTubeThumbnails))]
    [JsonSerializable(typeof(YouTubeThumbnail))]
    [JsonSerializable(typeof(YouTubeVideo))]
    public partial class YouTubeSearchResultContext : JsonSerializerContext
    {
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeSearchResult
    {
        [JsonPropertyName("nextPageToken")]
        public string NextPageToken { get; set; }
        [JsonPropertyName("items")]
        public YouTubeSearchItemResult[] Items { get; set; }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeSearchItemResult
    {
        [JsonPropertyName("id")]
        public YouTubeVideoId Id { get; set; }

        [JsonPropertyName("snippet")]
        public YouTubeSnippet Snippet { get; set; }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeVideoId
    {
        [JsonPropertyName("videoId")]
        public string VideoId { get; set; }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeSnippet
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnails")]
        public YouTubeThumbnails Thumbnails { get; set; }

        [JsonPropertyName("channelTitle")]
        public string ChannelTitle { get; set; }
        
        [JsonPropertyName("publishedAt")]
        public string PublishedAt { get; set; }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeThumbnails
    {
        [JsonPropertyName("default")]
        public YouTubeThumbnail Default { get; set; }

        [JsonPropertyName("medium")]
        public YouTubeThumbnail Medium { get; set; }

        [JsonPropertyName("high")]
        public YouTubeThumbnail High { get; set; }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeThumbnail
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
    public class YouTubeVideo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string Link { get; set; }
        public string ChannelTitle { get; set; }
        public DateTime PublishedAt { get; set; }

        public string PublishedDay
        {
            get
            {
                var today = DateTime.Today;
                var publishedDate = PublishedAt.Date;
                var firstDayOfThisWeek = today.AddDays(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - today.DayOfWeek);
                var firstDayOfLastWeek = firstDayOfThisWeek - TimeSpan.FromDays(7);
                var firstDayOfTwoWeeksAgo = firstDayOfThisWeek - TimeSpan.FromDays(14);
                var firstDayOfThreeWeeksAgo = firstDayOfThisWeek - TimeSpan.FromDays(21);
                var firstDayOfThisMonth = new DateTime(today.Year, today.Month, 1);
                var firstDayOfLastMonth = today.Month == 1 ? new DateTime(today.Year - 1, 12, 1) : new DateTime(today.Year, today.Month - 1, 1);
                if (publishedDate == today)
                {
                    return "Today";
                }
                else if (today.Subtract(publishedDate).TotalDays == 1)
                {
                    return "Yesterday";
                }
                else if (publishedDate >= firstDayOfThisWeek)
                {
                    return publishedDate.DayOfWeek.ToString();
                }
                else if (publishedDate >= firstDayOfLastWeek)
                {
                    return "Last Week";
                }
                else if (publishedDate >= firstDayOfTwoWeeksAgo)
                {
                    return "Two Weeks Ago";
                }
                else if (publishedDate >= firstDayOfThreeWeeksAgo)
                {
                    return "Three Weeks Ago";
                }
                else if (publishedDate >= firstDayOfThisMonth)
                {
                    return "This Month";
                }
                else if (publishedDate >= firstDayOfLastMonth)
                {
                    return "Last Month";
                }
                else if (publishedDate >= new DateTime(today.Year, 1, 1))
                {
                    return "This Year";
                }
                else if (publishedDate >= new DateTime(today.Year - 1, 1, 1))
                {
                    return "Last Year";
                }
                else if (publishedDate >= new DateTime(today.Year - 1, 1, 1))
                {
                    return "Two Years Ago";
                }
                else
                {
                    return "Older";
                }
            }
        }
    }
}
