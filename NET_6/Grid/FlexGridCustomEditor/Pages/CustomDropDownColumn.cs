using System;
using C1.Blazor.Core;
using C1.Blazor.Grid;
using C1.Blazor.Input;
using Microsoft.AspNetCore.Components;

namespace FlexGridCustomEditor.Pages
{
    public class CustomDropDownColumn : GridColumn
    {
        public C1DropDown CurrentDropDown { get; private set; }
        public object CurrentValue { get; private set; }

        protected override RenderFragment GetCellEditorFragment(GridRow row, Action<ComponentBase> editorCreated)
        {
            return new RenderFragment(builder =>
            {
                var value = Grid[row, this];
                CurrentValue = value;
                builder.OpenComponent<C1DropDown>(0);
                builder.AddAttribute(1, nameof(C1DropDown.Style), new C1Style(Grid.EditorStyle) { Width = C1StyleLength.FitParent, Height = C1StyleLength.FitParent, BorderWidth = 0, BorderRadius = 0 });
                builder.AddAttribute(2, nameof(C1DropDown.DropDownStyle), new C1Style { Width = 300, MaxHeight = 400 });
                builder.AddAttribute(3, nameof(C1DropDown.ChildContent), new RenderFragment(b =>
                {
                    b.OpenComponent<FlexGrid>(0);
                    b.AddAttribute(1, nameof(FlexGrid.AutoGenerateColumns), false);
                    b.AddAttribute(2, nameof(FlexGrid.IsReadOnly), true);
                    b.AddAttribute(3, nameof(FlexGrid.FlexGridColumns), (RenderFragment)((b2) =>
                    {
                        b2.OpenComponent<GridImageColumn>(0);
                        b2.AddAttribute(1, "Binding", "FlagUrl");
                        b2.AddAttribute(2, "Header", " ");
                        b2.AddAttribute(3, "Width", new GridLength(50));
                        b2.CloseComponent();
                        b2.OpenComponent<GridColumn>(4);
                        b2.AddAttribute(5, "Binding", "Code");
                        b2.AddAttribute(6, "Width", new GridLength(70));
                        b2.CloseComponent();
                        b2.OpenComponent<GridColumn>(7);
                        b2.AddAttribute(8, "Binding", "Name");
                        b2.AddAttribute(9, "Width", GridLength.Star);
                        b2.CloseComponent();
                    }
                    ));
                    b.AddAttribute(4, nameof(FlexGrid.ItemsSource), Country.GetCountries());
                    b.AddAttribute(5, nameof(FlexGrid.SelectionChanged), (EventHandler<GridCellRangeEventArgs>)OnSelectionChanged);
                    b.CloseComponent();
                }));
                builder.AddAttribute(4, nameof(C1DropDown.IsEditable), true);
                builder.AddAttribute(5, nameof(C1DropDown.EditableHeader), new RenderFragment(b =>
                 {
                     b.OpenComponent<C1TextBox>(0);
                     b.AddAttribute(1, nameof(C1TextBox.Style), new C1Style { Width = C1StyleLength.FitParent, Height = C1StyleLength.FitParent, BorderWidth = 0, BorderRadius = 0 });
                     b.AddAttribute(2, nameof(C1TextBox.Text), CurrentValue.ToString());
                     b.AddComponentReferenceCapture(3, r => { editorCreated(r as ComponentBase); });
                     b.CloseComponent();
                 }));
                builder.AddComponentReferenceCapture(6, r => { CurrentDropDown = r as C1DropDown; });
                builder.CloseComponent();
            });
        }

        private void OnSelectionChanged(object sender, GridCellRangeEventArgs e)
        {
            if (CurrentDropDown != null)
                CurrentDropDown.IsDropDownOpen = false;
            if (e.CellRange != null)
                CurrentValue = (sender as FlexGrid).Rows[e.CellRange.Row].DataItem;

        }

        protected override object GetEditorValue(GridRow row, ComponentBase editor)
        {
            return CurrentValue;
        }
    }
}
