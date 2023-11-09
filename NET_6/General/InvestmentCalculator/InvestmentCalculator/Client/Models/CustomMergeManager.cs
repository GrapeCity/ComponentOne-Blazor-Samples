using C1.Blazor.Grid;

namespace InvestmentCalculator.Client.Pages
{
    public class CustomMergeManager : GridMergeManager
    {
        public override GridCellRange GetMergedRange(GridCellType cellType, GridCellRange range)
        {
            //Merge cells containing calculator description
            if (cellType == GridCellType.Cell && range.Column == 0)
            {
                if (range.Row >= 0 && range.Row <= 2)
                {
                    return new GridCellRange(0, 0, 2, 0);
                }
                else if (range.Row >= 4 && range.Row <= 6)
                {
                    return new GridCellRange(4, 0, 6, 0);
                }
                else if (range.Row >= 8 && range.Row <= 9)
                {
                    return new GridCellRange(8, 0, 9, 0);
                }
            }

            return base.GetMergedRange(cellType, range);
        }
    }
}
