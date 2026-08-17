using CommunityToolkit.Mvvm.ComponentModel;

namespace LocaleDateGrid.Components.Pages
{
    public partial class DateTimeItem : ObservableObject
    {
        [ObservableProperty]
        public partial string Label { get; set; }

        [ObservableProperty]
        public partial DateTimeOffset DateTime { get; set; }
    }
}
