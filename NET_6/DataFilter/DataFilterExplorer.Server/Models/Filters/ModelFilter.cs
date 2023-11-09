using System.Collections.Generic;
using System.Linq;
using C1.Blazor.Core;
using C1.Blazor.DataFilter;
using C1.Blazor.ListView;
using C1.DataCollection;
using Microsoft.AspNetCore.Components;

namespace DataFilterExplorer.Server
{
    /// <summary>
    /// Custom filter sample
    /// </summary>
    public class ModelFilter : CustomFilter
    {
        private List<Car> _carsModels = new List<Car>();
        private IEnumerable<int> _selectedIndexes = new List<int>();

        /// <summary>
        /// ctor.
        /// </summary>
        public ModelFilter()
        {
            Template = builder =>
            {
                builder.OpenComponent<C1ListView<Car>>(10);
                builder.AddAttribute(12, nameof(C1ListView<Car>.ItemsSource), _carsModels);
                builder.AddAttribute(13, nameof(C1ListView<Car>.SelectionMode), C1SelectionMode.Multiple);
                builder.AddAttribute(14, nameof(C1ListView<Car>.DisplayMemberPath), nameof(Car.Model));
                builder.AddAttribute(15, nameof(C1ListView<Car>.Style), new C1Style("max-height: 200px; padding:16px;"));
                builder.AddAttribute(16, nameof(C1ListView<Car>.SelectionChanged), EventCallback.Factory.Create<IEnumerable<int>>(this, indexes =>
                {
                    _selectedIndexes = indexes;
                    this.OnValueChanged(new ValueChangedEventArgs() { ApplyFilter = true });
                }));
                builder.AddComponentReferenceCapture(20, r =>
                {
                    var listView = (C1ListView<Car>)r;
                    listView.Selection = GetSelection();
                });
                builder.CloseComponent();
            };
        }

        private C1OrderedSet GetSelection()
        {
            var result = new C1OrderedSet();
            foreach (var index in _selectedIndexes)
            {
                result.Add(index);
            }
            return result;
        }

        public override Expression Expression
        {
            get
            {
                var expr = new CombinationExpression() { FilterCombination = FilterCombination.Or };
                foreach (var modelIndex in _selectedIndexes)
                {
                    expr.Expressions.Add(new OperationExpression() { Value = _carsModels[modelIndex].Model, FilterOperation = FilterOperation.Equal, PropertyName = PropertyName });
                }
                return expr;
            }
            set
            {

            }
        }

        public override bool IsApplied => _selectedIndexes.Any();


        [Parameter]
        public List<Car> CarModels
        {
            get => _carsModels; set { SetField(ref _carsModels, value); }
        }
    }

}
