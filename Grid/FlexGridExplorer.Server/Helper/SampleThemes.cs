using C1.Blazor.Core;
using C1.Blazor.Grid;
using System;
using System.Collections.Generic;

namespace FlexGridExplorer
{
    public static class SampleThemes
    {
        public static List<string> ThemeVariations = new()
        {
            "C1Blue",
            "Material",
            "Material Dark",
            "Expression Dark",
        };
        public static void ApplyThemes(FlexGrid grid, string theme)
        {
            if (grid == null)
                return;
            if (!ThemeVariations.Contains(theme))
            {
                throw new Exception("Unknown Theme");
            }

            switch (theme)
            {
                case "Classic":
                    grid.StyleKind = GridStyle.Classic;
                    break;
                case "C1Blue":
                    #region C1Blue

                    #region Style Abstractions

                    grid.Style = new()
                    {
                        FontSize = 14,
                        BorderRadius = 0,
                        BorderStyle = C1StyleBorderStyle.None,
                        BorderWidth = 0,
                        BorderColor = null,
                    };
                    grid.Style["font-family"] = "'Roboto', sans-serif";

                    grid.CellStyle = new()
                    {
                        BackgroundColor = null,
                        Color = null,
                    };

                    grid.RowStyle = new()
                    {
                        BackgroundColor = "#EFF3F7",
                        Color = "#000000",
                    };

                    grid.AlternatingRowStyle = new()
                    {
                        BackgroundColor = "#FFFFFF",
                        Color = "#000000",
                    };

                    grid.ColumnHeaderStyle = new()
                    {
                        FontWeight = "500",
                        Color = "#000000",
                        BackgroundImage = "linear-gradient(to bottom, #f8fafb, #d5dfea)",
                    };

                    grid.RowHeaderStyle = new()
                    {
                        FontWeight = "500",
                        Color = "#000000",
                        BackgroundImage = "linear-gradient(to right, #f8fafb, #d5dfea)",
                    };

                    grid.TopLeftHeaderStyle = new()
                    {
                        BackgroundColor = "#FFFFFF",
                        BackgroundImage = "linear-gradient(to bottom right, #f8fafb, #d5dfea)",
                    };

                    grid.SelectionStyle = new()
                    {
                        BackgroundColor = "#BADDE9",
                        BorderStyle = C1StyleBorderStyle.Solid,
                        BorderWidth = 2,
                        BorderColor = "#000000",
                    };

                    grid.ColumnHeaderSelectedStyle = new();

                    grid.RowHeaderSelectedStyle = new();

                    grid.GroupRowStyle = new();

                    #endregion

                    #region Grid Lines

                    grid.GridLinesBrush = "#F3F3F3";
                    grid.GridLinesVisibility = GridLinesVisibility.All;

                    grid.ColumnHeaderGridLinesBrush = "#F3F3F3";
                    grid.ColumnHeaderGridLinesVisibility = GridLinesVisibility.All;

                    grid.RowHeaderGridLinesBrush = "#F3F3F3";
                    grid.RowHeaderGridLinesVisibility = GridLinesVisibility.All;

                    grid.TopLeftHeaderGridLinesBrush = "#F3F3F3";
                    grid.TopLeftHeaderGridLinesVisibility = GridLinesVisibility.All;

                    #endregion

                    grid.DefaultRowHeight = 52;
                    grid.DefaultColumnHeaderRowHeight = 56;

                    #endregion
                    break;
                case "Expression Dark":

                    #region Style Abstractions

                    grid.Style = new()
                    {
                        FontSize = 14,
                        BorderRadius = 0,
                        BorderStyle = C1StyleBorderStyle.None,
                        BorderWidth = 0,
                        BorderColor = null,
                    };

                    grid.CellStyle = new()
                    {
                        BackgroundColor = null,
                        Color = null,
                        BorderWidth = 2,
                        BorderColor = "#383838",
                    };

                    grid.RowStyle = new()
                    {
                        BackgroundColor = "#333333",
                        Color = "#FFFFFF",
                    };

                    grid.AlternatingRowStyle = new()
                    {
                        BackgroundColor = "#333333",
                        Color = "#FFFFFF",
                    };

                    grid.ColumnHeaderStyle = new()
                    {
                        FontWeight = "600",
                        Color = "#E2E2E2",
                        BackgroundImage = "linear-gradient(to bottom, #9e9e9e, #727272)",
                    };

                    grid.RowHeaderStyle = new()
                    {
                        FontWeight = "500",
                        Color = "#E2E2E2",
                        BackgroundColor = "#333333",
                    };

                    grid.TopLeftHeaderStyle = new()
                    {
                        BackgroundColor = "#333333",
                        BackgroundImage = "linear-gradient(to bottom, #9e9e9e, #727272)",
                    };

                    grid.SelectionStyle = new()
                    {
                        BackgroundColor = "#505050",
                        BorderStyle = C1StyleBorderStyle.Solid,
                        BorderWidth = 2,
                        BorderColor = "#000000",
                    };

                    grid.ColumnHeaderSelectedStyle = new();

                    grid.RowHeaderSelectedStyle = new();

                    grid.GroupRowStyle = new();

                    #endregion

                    #region Grid Lines

                    grid.GridLinesBrush = "#404040";
                    grid.GridLinesVisibility = GridLinesVisibility.All;

                    grid.ColumnHeaderGridLinesBrush = "#404040";
                    grid.ColumnHeaderGridLinesVisibility = GridLinesVisibility.All;

                    grid.RowHeaderGridLinesBrush = "#404040";
                    grid.RowHeaderGridLinesVisibility = GridLinesVisibility.All;

                    grid.TopLeftHeaderGridLinesBrush = "#404040";
                    grid.TopLeftHeaderGridLinesVisibility = GridLinesVisibility.All;

                    #endregion

                    grid.DefaultRowHeight = 52;
                    grid.DefaultColumnHeaderRowHeight = 56;

                    break;
                case "Material":
                    #region Material

                    #region Style Abstractions

                    grid.Style = new()
                    {
                        FontSize = 14,
                        BorderRadius = 8,
                        BorderStyle = C1StyleBorderStyle.Solid,
                        BorderWidth = 1,
                        BorderColor = "#0039CB",
                    };
                    grid.Style["font-family"] = "'Roboto', sans-serif";

                    var cellStyle = new C1Style()
                    {
                        Color = "#3D3D3D",
                        BackgroundColor = "#FFFFFF",
                    };
                    grid.CellStyle = cellStyle;

                    grid.RowStyle = cellStyle;

                    grid.AlternatingRowStyle = cellStyle;

                    grid.ColumnHeaderStyle = new()
                    {
                        FontWeight = "500",
                    };

                    grid.RowHeaderStyle = new();

                    grid.TopLeftHeaderStyle = new()
                    {
                        BackgroundColor = "#FFFFFF",
                    };

                    grid.SelectionStyle = new()
                    {
                        BackgroundColor = "#EEEEEE",
                        BorderStyle = C1StyleBorderStyle.Solid,
                        BorderWidth = 1,
                        BorderColor = "#0039CB",
                    };

                    grid.ColumnHeaderSelectedStyle = new();

                    grid.RowHeaderSelectedStyle = new();

                    grid.GroupRowStyle = new();

                    #endregion

                    #region Grid Lines

                    grid.GridLinesBrush = "#E0E0E0";
                    grid.GridLinesVisibility = GridLinesVisibility.Horizontal;

                    grid.ColumnHeaderGridLinesBrush = "#E0E0E0";
                    grid.ColumnHeaderGridLinesVisibility = GridLinesVisibility.Horizontal;

                    grid.RowHeaderGridLinesBrush = "#E0E0E0";
                    grid.RowHeaderGridLinesVisibility = GridLinesVisibility.Horizontal;

                    grid.TopLeftHeaderGridLinesBrush = "#E0E0E0";
                    grid.TopLeftHeaderGridLinesVisibility = GridLinesVisibility.Horizontal;

                    #endregion

                    grid.DefaultRowHeight = 52;
                    grid.DefaultColumnHeaderRowHeight = 56;

                    #endregion
                    break;
                case "Material Dark":
                    #region Material Deep Purple

                    #region Style Abstractions

                    grid.Style = new()
                    {
                        FontSize = 14,
                        BorderRadius = 8,
                        BorderStyle = C1StyleBorderStyle.Solid,
                        BorderWidth = 1,
                        BorderColor = "#212121",
                    };
                    grid.Style["font-family"] = "'Roboto', sans-serif";

                    var materialDarkCellStyle = new C1Style()
                    {
                        Color = "#FAFAFA",
                        BackgroundColor = "#212121",
                    };
                    grid.CellStyle = materialDarkCellStyle;

                    grid.RowStyle = materialDarkCellStyle;

                    grid.AlternatingRowStyle = materialDarkCellStyle;

                    var materialDarkHeaderStyle = new C1Style()
                    {
                        FontWeight = "500",
                        BackgroundColor = "#212121",
                        Color = "#FAFAFA",
                    };
                    grid.ColumnHeaderStyle = materialDarkHeaderStyle;

                    grid.RowHeaderStyle = materialDarkHeaderStyle;

                    grid.TopLeftHeaderStyle = materialDarkHeaderStyle;

                    grid.SelectionStyle = new()
                    {
                        BackgroundColor = "#B388FF",
                        BorderStyle = C1StyleBorderStyle.Solid,
                        BorderWidth = 1,
                        BorderColor = "#805ACB",
                        Color = "#212121",
                    };

                    grid.ColumnHeaderSelectedStyle = new();

                    grid.RowHeaderSelectedStyle = new();

                    grid.GroupRowStyle = new();

                    #endregion

                    #region Grid Lines

                    grid.GridLinesBrush = "#484848";
                    grid.GridLinesVisibility = GridLinesVisibility.Horizontal;

                    grid.ColumnHeaderGridLinesBrush = "#484848";
                    grid.ColumnHeaderGridLinesVisibility = GridLinesVisibility.Horizontal;

                    grid.RowHeaderGridLinesBrush = "#484848";
                    grid.RowHeaderGridLinesVisibility = GridLinesVisibility.Horizontal;

                    grid.TopLeftHeaderGridLinesBrush = "#484848";
                    grid.TopLeftHeaderGridLinesVisibility = GridLinesVisibility.Horizontal;

                    #endregion

                    grid.DefaultRowHeight = 52;
                    grid.DefaultColumnHeaderRowHeight = 56;

                    #endregion
                    break;
            }
        }
    }
}
