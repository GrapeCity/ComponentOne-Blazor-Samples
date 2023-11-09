using C1.DataCollection;
using C1.Blazor.DataFilter;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace DataFilterExplorer.Server
{
    public class TransmissionFilter : CustomFilter
    {
        public TransmissionFilter()
        {
            Template = builder =>
            {
                const string all = "All";
                const string yes = "Yes";
                const string no = "No";

                builder.OpenElement(10, "div");
                builder.AddAttribute(11, "style", "padding:6px 6px 6px 12px;");
                addRadio(20, all);
                addRadio(30, yes);
                addRadio(40, no);
                builder.CloseElement();

                void addRadio(int regionNum, string value)
                {
                    builder.OpenRegion(regionNum);
                    builder.OpenElement(10, "label");
                    builder.OpenElement(20, "input");
                    builder.AddAttribute(21, "name", "TransmissionFilterGroup");
                    builder.AddAttribute(22, "value", value);
                    builder.AddAttribute(23, "checked", value == all ? !_selectedValues.Any() : _selectedValues.Contains(value));
                    builder.AddAttribute(24, "type", "radio");
                    builder.AddAttribute(24, "onchange", EventCallback.Factory.Create(this, (ChangeEventArgs args) =>
                    {
                        _selectedValues.Clear();
                        if (args.Value is string v && v != "All")
                        {
                            _selectedValues.Add(v);
                        }
                        this.OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
                    }));
                    builder.CloseElement();
                    builder.AddMarkupContent(25, $"<span style=\"padding-left: 4px;\">{value}</span>");
                    builder.CloseElement();
                    builder.AddMarkupContent(30, "<br/>");
                    builder.CloseRegion();
                }
            };
        }

        public override Expression Expression
        {
            get
            {
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var tag in _selectedValues)
                {
                    expr.Expressions.Add(new OperationExpression() { Value = tag, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {
                _selectedValues = GetSelectedValues(value).ToList();
            }
        }

        private List<string> _selectedValues = new List<string>();

        private IEnumerable<string> GetSelectedValues(Expression expression)
        {
            switch (expression)
            {
                case CombinationExpression combination when combination.FilterCombination == FilterCombination.Or:
                    foreach (var e in combination.Expressions)
                    {
                        foreach (var v in GetSelectedValues(e))
                        {
                            yield return v;
                        }
                    }
                    break;
                case OperationExpression operation when operation.FilterOperation == FilterOperation.Equal:
                    yield return operation.Value?.ToString();
                    break;
            }
        }

        public override bool IsApplied => _selectedValues.Any();
    }
}
