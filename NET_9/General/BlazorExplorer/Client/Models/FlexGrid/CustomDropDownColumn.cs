﻿using C1.Blazor.Core;
using C1.Blazor.Grid;
using C1.Blazor.Input;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorExplorer
{
    public class CustomDropDownColumn : GridColumn
    {
        private static readonly List<Country> _countries = Country.GetCountries();
        public CustomDropDownColumn()
        {
            CellTemplate = data => (RenderFragment) (b =>
            {
                b.OpenElement(0, "div");

                b.OpenElement(10, "img");
                b.AddAttribute(11, "src", ((Country)data).FlagUrl);
                b.AddAttribute(12, "style", "margin-right: 12px;");
                b.CloseElement();

                b.AddContent(20, data.ToString());

                b.CloseElement();
            });
        }

        public C1DropDown CurrentDropDown { get; set; }
        public object CurrentValue { get; private set; }

        protected override RenderFragment GetCellEditorFragment(GridRow row, Action<ComponentBase> editorCreated)
        {
            return new RenderFragment(builder =>
            {
                var value = Grid[row, this];
                CurrentValue = value;

                builder.OpenComponent<C1DropDown>(0);
                builder.AddAttribute(1, nameof(C1DropDown.Style), new C1Style(Grid.EditorStyle) { Width = C1StyleLength.FitParent, Height = C1StyleLength.FitParent, BorderWidth = 0, BorderRadius = 0 });
                builder.AddAttribute(3, nameof(C1DropDown.ChildContent), new RenderFragment(b =>
                {
                    b.OpenComponent<FlexGrid>(0);
                    b.AddAttribute(1, nameof(FlexGrid.Style), new C1Style("max-height: 400px; width: 300px;"));
                    b.AddAttribute(2, nameof(FlexGrid.AutoGenerateColumns), false);
                    b.AddAttribute(3, nameof(FlexGrid.IsReadOnly), true);
                    b.AddAttribute(4, nameof(FlexGrid.ShowSelectionMenu), false);
                    b.AddAttribute(5, nameof(FlexGrid.ColumnOptionsMenuVisibility), GridColumnOptionsMenuVisibility.Collapsed);
                    b.AddAttribute(6, nameof(FlexGrid.FlexGridColumns), (RenderFragment)((b2) =>
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
                    b.AddAttribute(7, nameof(FlexGrid.ItemsSource), _countries);
                    b.AddAttribute(8, nameof(FlexGrid.SelectionChanged), (EventHandler<GridSelectionEventArgs>)OnSelectionChanged);
                    b.CloseComponent();
                }));
                builder.AddAttribute(4, nameof(C1DropDown.IsEditable), true);
                builder.AddAttribute(5, nameof(C1DropDown.EditableHeader), new RenderFragment(b =>
                {
                    b.AddContent(0, CurrentValue.ToString());
                }));
                builder.AddComponentReferenceCapture(6, r => { CurrentDropDown = r as C1DropDown; editorCreated(r as ComponentBase); });
                builder.CloseComponent();
            });
        }

        private void OnSelectionChanged(object sender, GridSelectionEventArgs e)
        {
            if (CurrentDropDown != null)
            {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                CurrentDropDown.IsDropDownOpen = false;
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            }

            if (e.CellRange != null)
            {
                CurrentValue = (sender as FlexGrid).Rows[e.CellRange.Row].DataItem;
            }
        }

        protected override object GetEditorValue(GridRow row, ComponentBase editor)
        {
            return CurrentValue;
        }
    }
}
