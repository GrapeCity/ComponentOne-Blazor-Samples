using CommunityToolkit.Mvvm.ComponentModel;

namespace LocaleDateGrid.Components.Pages
{
    [ObservableObject]
    public partial class DateTimeItem
    {
        [ObservableProperty]
        public partial string Label { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset DateTime { get; set; }
    }
}
