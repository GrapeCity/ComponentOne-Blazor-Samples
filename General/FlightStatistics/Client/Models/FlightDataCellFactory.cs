using C1.Blazor.Core;
using C1.Blazor.Grid;
using FlightStatistics.Shared;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace FlightStatistics.Client.Models
{
    public class FlightDataCellFactory : GridCellFactory
    {
        public override void PrepareCellStyle(GridCellType cellType, GridCellRange range, C1Style style, C1Thickness internalBorders)
        {
            base.PrepareCellStyle(cellType, range, style, internalBorders);
            var delayedColumn = Grid.Columns["Delayed"];
            if (cellType == GridCellType.Cell && range.Column == delayedColumn.Index)
            {
                var cellValue = (double)Grid[range.Row, range.Column];
                style.Color = cellValue > 0.2 ? C1Color.Red : C1Color.Green;
            }
        }

        public override RenderFragment GetCellContentRenderFragment(GridCellType cellType, GridCellRange range)
        {
            if (Grid.Rows.Any() && Grid.Rows[range.Row] is not GridGroupRow && cellType == GridCellType.Cell && range.Row != 0)
            {
                if (Grid.Columns[range.Column].Binding == nameof(Airport.AirportCity))
                {
                    var row = (Airport)Grid.Rows[range.Row].DataItem;

                    return new RenderFragment(b =>
                    {
                        b.OpenElement(0, "img");
                        b.AddAttribute(1, "style", new C1Style { Height = "25px", Width = "25px", Padding = new C1Thickness(0, 0, 3, 0) });
                        b.AddAttribute(2, "alt", "flag");
                        b.AddAttribute(3, "src", "images/flags/" + row.CountryName + ".png");
                        b.CloseElement();
                        b.AddContent(4, row.AirportCity + " (" + row.CountryCode + ")");
                    });
                }
                else
                {
                    var userRating = Grid.Columns[nameof(Airport.UserRating)];
                    if (range.Column == userRating.Index)
                    {
                        var cellValue = (int)Grid[range.Row, range.Column];
                        string starType;
                        return new RenderFragment(b =>
                        {
                            for (var j = 0; j < 5; j++)
                            {
                                starType = j < cellValue ? "star star-highlighted" : "star star-dimmed";
                                b.OpenElement(0, "span");
                                b.AddAttribute(1, "class", starType);
                                b.AddContent(2, '\u2605');
                                b.CloseElement();
                            }
                        });
                    }
                }
            }
            return base.GetCellContentRenderFragment(cellType, range);
        }

    }
}
