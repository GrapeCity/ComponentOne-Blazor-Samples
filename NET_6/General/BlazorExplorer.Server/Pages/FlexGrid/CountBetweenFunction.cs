using C1.Blazor.Grid;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorExplorer.Pages.FlexGrid
{
    /// <summary>
    /// Returns the number of rows whose value for the column is between <see cref="Minimum"/> and <see cref="Maximum"/>.
    /// </summary>
    public class CountBetweenFunction : GridAggregateFunction
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        [Parameter]
        public new double Minimum { get; set; } = double.MinValue;

        /// <summary>
        /// The maximum value.
        /// </summary>
        [Parameter]
        public new double Maximum { get; set; } = double.MaxValue;

        ///<inheritdoc/>
        public override double GetValue(GridColumn column, IEnumerable<GridRow> rows)
        {
            var count = 0;
            var grid = column.Grid;
            foreach (var row in rows)
            {
                // get raw value
                var val = grid[row, column];
                if (val is double dVal)
                    if (dVal >= Minimum && dVal <= Maximum)
                        count++;
            }

            return count;
        }
    }
}
