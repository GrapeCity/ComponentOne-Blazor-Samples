using System.Collections.Generic;

namespace BlazorExplorer
{
    public class NavItem
    {
        public static List<NavItem> NavItems;
        public static NavItem SelectedItem { get; set; }
        public string Header { get; set; }
        public string Href { get; set; }
        public IEnumerable<NavItem> Items { get; set; }
        public IEnumerable<NavItem> ParentItems { get; set; }
        public Models.ControlPage Page { get; set; }
        public bool IsCollapsed { get; set; }
        public NavItem ParentItem { get; set; }
        public void Toggle()
        {
            IsCollapsed = !IsCollapsed;
            if (!IsCollapsed && ParentItems != null)
            {
                foreach (var item in ParentItems)
                {
                    if (item != this)
                    {
                        item.IsCollapsed = true;
                    }
                }
            }
        }

    }
}
