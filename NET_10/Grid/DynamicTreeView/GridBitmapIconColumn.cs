using C1.Blazor.Core;
using C1.Blazor.Grid;
using Microsoft.AspNetCore.Components;

namespace DynamicTreeView
{
    public class GridBitmapIconColumn : GridColumn
    {

        [Parameter]
        public string BitmapSourceBinding { get; set; }

        [Parameter]
        public double IconWidth { get; set; }

        [Parameter]
        public bool ShowAsMonochrome { get; set; }

        protected override RenderFragment GetCellContentRenderFragment(GridCellType cellType, GridRow row)
        {
            if (cellType == GridCellType.Cell)
            {
                return new RenderFragment(b =>
                {
                    b.OpenElement(0, "div");
                    b.AddAttribute(1, "style", new C1Style { Display = C1StyleDisplay.Flex });

                    b.OpenElement(10, "img");
                    b.AddAttribute(11, "style", new C1Style { Width = IconWidth, ["margin-right"] = "5px" });
                    try
                    {
                        CacheBitmapSourceGetterFunction();
                    }
                    catch { }
                    var value = _imageSourceGetter.Invoke(row.DataItem);
                    if (value is string valueStr)
                        b.AddAttribute(12, "src", valueStr);
                    b.CloseElement();
                    
                    b.AddContent(20, GetCellText(cellType, row));
                    b.CloseElement();
                });
            }
            else
            {
                return base.GetCellContentRenderFragment(cellType, row);
            }
        }

        private Func<object, object> _imageSourceGetter;

        private void CacheBitmapSourceGetterFunction()
        {
            if (_imageSourceGetter == null && !string.IsNullOrWhiteSpace(BitmapSourceBinding) && Grid?.DataCollection != null)
            {
                var itemType = Grid.DataCollection.GetItemType();
                _imageSourceGetter = CreateBindingFunction(itemType, BitmapSourceBinding);
            }
        }
    }
}
