using C1.Blazor.Core;
using C1.Blazor.Grid;

namespace InvestmentCalculator.Client.Pages
{
    public class InvestmentCalculatorCellFactory : GridCellFactory
    {
        public override void PrepareCellStyle(GridCellType cellType, GridCellRange range, C1Style style, C1Thickness internalBorders)
        {
            base.PrepareCellStyle(cellType, range, style, internalBorders);

            #region Style Calculator fields labels and inputs
            //apply white-space for descriptions
            if (cellType == GridCellType.Cell && range.Column == 0)
            {
                if ((range.Row >= 0 && range.Row <= 9) && (range.Row != 3 && range.Row != 7))
                {
                    style.WhiteSpace = C1StyleWhiteSpace.Normal;
                }
            }
            //labels
            if (cellType == GridCellType.Cell && range.Column == 1)
            {
                style.BorderColor = C1Color.Black;
                if (range.Row >= 0 && range.Row <= 9)
                {
                    if (range.Row != 3 && range.Row != 7)
                    {
                        style.BorderWidth = new C1Thickness(1);
                        style.BackgroundColor = C1Color.FromArgb(255, 112, 173, 70);
                        style.Color = C1Color.White;
                        style.JustifyContent = C1StyleJustifyContent.FlexEnd;
                    }
                }
            }
            //inputs
            if (cellType == GridCellType.Cell && range.Column == 2)
            {
                style.BorderColor = C1Color.Black;
                if (range.Row >= 0 && range.Row <= 9)
                {
                    if (range.Row != 3 && range.Row != 7)
                    {
                        style.BorderWidth = new C1Thickness(1);
                        style.JustifyContent = C1StyleJustifyContent.FlexEnd;
                    }
                }
                if (range.Row >= 0 && range.Row <= 5 && range.Row != 3)
                {
                    style.BackgroundColor = C1Color.Yellow;
                }
                else if (range.Row >= 6 && range.Row <= 9 && range.Row != 7)
                {
                    style.BackgroundColor = C1Color.LightGreen;
                }
            }

            if (range.Row == 3 || range.Row == 7 || range.Row == 10)
            {
                style.Position = C1StylePosition.Absolute;
            }
            #endregion
        }
    }
}
