using C1.Blazor.Core;
using C1.Blazor.Grid;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorExplorer.Pages
{
    public class FinancialCellFactory : GridCellFactory
    {
        public override void PrepareCellStyle(GridCellType cellType, GridCellRange range, C1Style style, C1Thickness internalBorders)
        {
            base.PrepareCellStyle(cellType, range, style, internalBorders);
            var lastSaleColumn = Grid.Columns["LastSale"];
            var changeColumn = Grid.Columns["Change"];
            if (cellType == GridCellType.Cell && range.Column == lastSaleColumn.Index)
            {
                var cellValue = (decimal)Grid[range.Row, range.Column];
                style.Color = cellValue < 500 ? C1Color.Red : C1Color.Green;
            }
            if (cellType == GridCellType.Cell && range.Column == changeColumn.Index)
            {
                var cellValue = (decimal)Grid[range.Row, range.Column];
                style.Color = cellValue < 0 ? C1Color.Red : C1Color.Green;
            }
        }
    }
}
