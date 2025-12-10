using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using C1.Blazor.DataFilter;
using C1.DataCollection;
using C1.Blazor.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DataFilterExplorer.Server
{
    /// <summary>
    /// Custom filter sample
    /// </summary>
    public class ColorFilter : CustomFilter
    {
        private ColorFilterPresenter _colorFilterPresenter;
        private List<Color> _colors = new();
        private List<Color> _selectedColors = new();

        /// <summary>
        /// ctor.
        /// </summary>
        public ColorFilter()
        {
            Template = builder =>
             {
                 builder.OpenComponent<ColorFilterPresenter>(10);
                 builder.AddAttribute(12, nameof(ColorFilterPresenter.Filter), this);
                 builder.AddComponentReferenceCapture(19, r => _colorFilterPresenter = (ColorFilterPresenter)r);
                 builder.CloseComponent();
             };
        }

        public override Expression Expression
        {
            get
            {
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var color in SelectedColors)
                {
                    expr.Expressions.Add(new OperationExpression() { Value = color, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {
                SelectedColors = GetColors(value).ToList();
            }
        }

        private IEnumerable<Color> GetColors(Expression expression)
        {
            if (expression is OperationExpression operation)
            {
                if (operation.PropertyName != PropertyName) yield break;

                var color = Colors.FirstOrDefault(c => c == (Color)operation.Value);
                if (color != default)
                    yield return color;

            }
            else if (expression is CombinationExpression combination)
            {
                foreach (var e in combination.Expressions)
                {
                    foreach (var color in GetColors(e))
                    {
                        yield return color;
                    }
                }
            }
            yield break;
        }


        public override bool IsApplied => SelectedColors.Any();

        public List<Color> SelectedColors { get => _selectedColors; set { SetField(ref _selectedColors, value); } }
        
        [Parameter]
        public List<Color> Colors
        {
            get => _colors; set { SetField(ref _colors, value); }
        }

        internal bool IsColorSelected(Color color) => SelectedColors.Contains(color);
        internal void SelectColor(Color color, bool selected)
        {

            if (selected)
            {
                if (SelectedColors.Contains(color))
                {
                    return;
                }
                SelectedColors.Add(color);
            }
            else
            {
                if (!SelectedColors.Contains(color))
                {
                    return;
                }
                SelectedColors.Remove(color);
            }
            OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
        }
    }


    public class ColorFilterPresenter : ComponentBase, IDisposable
    {
        /// <summary>
        /// Filter.
        /// </summary>
        [Parameter]
        public ColorFilter Filter { get; set; }

        #region initialization
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                SubscribeFilterEvents();
            }
        }

        private void SubscribeFilterEvents()
        {
            Filter.ValueChanged += FilterValueChanged;
            Filter.PropertyChanged += FilterPropertyChanged;
            Filter.AttachedStateChanged += OnAttachedStateChanged;
        }

        private void UnsubscribeFilterEvents()
        {
            Filter.ValueChanged -= FilterValueChanged;
            Filter.PropertyChanged -= FilterPropertyChanged;
            Filter.AttachedStateChanged -= OnAttachedStateChanged;
        }

        ///<inheritdoc/>
        public void Dispose()
        {
            UnsubscribeFilterEvents();
        }
        #endregion

        private void OnAttachedStateChanged(object sender, AttachedFilterState state)
        {
            if (Filter.AttachedState != AttachedFilterState.Detached)
            {
                return;
            }
            UnsubscribeFilterEvents();
        }

        private void FilterPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

        private void FilterValueChanged(object sender, ValueChangedEventArgs e)
        {
            StateHasChanged();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", "padding:6px 6px 6px 12px;");
            foreach (var color in Filter.Colors)
            {
                builder.OpenRegion(11);

                builder.OpenElement(11, "span");
                builder.AddAttribute(12, "style", $"background: {C1Color.FromArgb(color.A, color.R, color.G, color.B)}; width: 30px; height: 30px; border-radius: 50%; display:inline-block; margin-left: 3px; margin-right: 3px;");
                builder.AddAttribute(13, "onclick", EventCallback.Factory.Create(this, ()=> {
                    Filter.SelectColor(color, !Filter.IsColorSelected(color));
                }));

                if (Filter.IsColorSelected(color))
                {
                    builder.OpenElement(21, "span");
                    builder.AddAttribute(22, "style", $"background: white; width: 16px; height: 16px; border-radius: 50%;" +
                        $" position: relative; left: 50%; top: 50%; margin-left: -8px; margin-top: -8px; display:block; border: 3px solid black;");
                    builder.CloseElement();
                }

                builder.CloseElement();
                builder.CloseRegion();

            }
            builder.CloseElement(); //div
        }
    }
}
