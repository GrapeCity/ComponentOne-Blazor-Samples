using C1.Blazor.Core;
using C1.Blazor.Grid;

namespace InvestmentCalculator.Client.Pages
{
    public class InvestmentResultDetailCellFactory : GridCellFactory
    {
        public override void PrepareCellStyle(GridCellType cellType, GridCellRange range, C1Style style, C1Thickness internalBorders)
        {
            base.PrepareCellStyle(cellType, range, style, internalBorders);

            if(cellType == GridCellType.Cell)
            {
                if (range.Row % 2 == 0)
                    style.BackgroundColor = C1Color.FromArgb(255, 226, 239, 219);
                else
                    style.BackgroundColor = C1Color.FromArgb(255, 255, 255, 255);
                style.JustifyContent = C1StyleJustifyContent.FlexEnd;

                if (range.Column == 0)
                {
                    style.JustifyContent = C1StyleJustifyContent.Center;
                }
            }else if (cellType == GridCellType.ColumnHeader)
            {
                style.BackgroundColor = C1Color.FromArgb(255, 112, 173, 70);
                style.Color = C1Color.White;
                style.JustifyContent = C1StyleJustifyContent.FlexEnd;
                if (range.Column == 0)
                {
                    style.JustifyContent = C1StyleJustifyContent.Center;
                }
            }
        }
    }
}
