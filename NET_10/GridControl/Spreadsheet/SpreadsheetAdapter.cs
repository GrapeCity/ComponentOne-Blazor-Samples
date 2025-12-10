using System;
using System.Collections.Generic;
using System.Text;
using C1.Blazor.Core;
using C1.Blazor.Grid;
using C1.Blazor.Input;
using Microsoft.AspNetCore.Components;

namespace Spreadsheet
{
    public class SpreadsheetAdapter : GridControlAdapter
    {
        #region fields

        private static C1Color HeaderBackground = "#f5f5f5";
        private static C1Color HeaderForeground = "#5e5e5e";
        private static C1Color HeaderBorderColor = "#bbbbbb";
        private static C1Color SelectedHeaderBackground = "#dad8d6";
        private static C1Color SelectedHeaderForeground = "#217346";
        private static C1Color CellBorderColor = "#d4d4d4";

        private Dictionary<KeyValuePair<int, int>, string> _storedValues = new Dictionary<KeyValuePair<int, int>, string>();
        private int A = 'A';
        private int Z = 'Z';
        private C1Thickness CellPadding { get; set; } = new C1Thickness(2, 1, 2, 1);
        private Dictionary<int, double> _columnSizes = new Dictionary<int, double>();
        private Dictionary<int, double> _rowSizes = new Dictionary<int, double>();
        private ColumnInfo DefaultColumnInfo = new ColumnInfo(new GridLength(65), 27, double.PositiveInfinity, true);
        private RowInfo DefaultRowInfo = new RowInfo(new GridLength(23), 0, double.PositiveInfinity, true);
        private int ROWS_COUNT = 10_000;
        private int COLUMNS_COUNT = 10_000;

        #endregion

        #region dimensions

        public override int RowsCount => ROWS_COUNT + 1;
        public override int ColumnsCount => COLUMNS_COUNT + 1;

        public override ColumnInfo GetDefaultColumn()
        {
            return DefaultColumnInfo;
        }

        public override RowInfo GetDefaultRow()
        {
            return DefaultRowInfo;
        }

        protected override ColumnInfo GetColumnInfo(int column)
        {
            double width;
            if (column < 1)
            {
                return new ColumnInfo(42, 42, double.PositiveInfinity, true);
            }
            else if (_columnSizes.TryGetValue(column + 1, out width))
            {
                return new ColumnInfo(new GridLength(width), 0, double.PositiveInfinity, true);
            }
            else
            {
                return GetDefaultColumn();
            }
        }

        protected override RowInfo GetRowInfo(int row)
        {
            double height;
            if (row < 1)
            {
                return GetDefaultRow();
            }
            else if (_rowSizes.TryGetValue(row + 1, out height))
            {
                return new RowInfo(new GridLength(height), 0, double.PositiveInfinity, true);
            }
            else
            {
                return GetDefaultRow();
            }
        }

        public override GridControlRange NavigableRange => new GridControlRange(1, 1, ROWS_COUNT, COLUMNS_COUNT);

        public override int GetFrozenColumns()
        {
            return 1;
        }

        public override int GetFrozenRows()
        {
            return 1;
        }

        protected override bool CanSetColumnWidth(int row, int column)
        {
            return column >= 1 && row <= 0;
        }

        protected override bool CanSetRowHeight(int row, int column)
        {
            return row >= 1 && column <= 0;
        }

        protected override void SetColumnWidth(int column, double width)
        {
            if (!double.IsFinite(width))
                _columnSizes.Remove(column + 1);
            else
                _columnSizes[column + 1] = width;
            base.SetColumnWidth(column, width);
        }

        protected override void SetRowHeight(int row, double height)
        {
            if (!double.IsFinite(height))
                _rowSizes.Remove(row + 1);
            else
                _rowSizes[row + 1] = height;
            base.SetRowHeight(row, height);
        }

        protected override bool SaveDesiredCellSize(GridControlRange range)
        {
            return range.Column < 1;
        }
        #endregion

        #region cell

        public C1Style ColumnHeaderSelectedStyle { get; set; }

        public C1Style RowHeaderSelectedStyle { get; set; }

        public override IEnumerable<string> GetCellCssClasses(GridControlRange range, C1Thickness internalBorders)
        {
            if (range.Row == 0 || range.Column == 0)
            {
                return new string[] { "spreadsheet-header-style" };
            }
            else
            {
                return base.GetCellCssClasses(range, internalBorders);
            }
        }

        public override void PrepareCellStyle(GridControlRange range, C1Style style, C1Thickness internalBorders)
        {
            base.PrepareCellStyle(range, style, internalBorders);

            if ((range.Row == 0 && (Grid.Selection?.ContainsColumn(range.Column) ?? false))
                || (range.Column == 0 && (Grid.Selection?.ContainsRow(range.Row) ?? false)))
            {
                style.Color = SelectedHeaderForeground;
                style.FontWeight = C1StyleFontWeight.Bold;
                style.BackgroundColor = SelectedHeaderBackground;
            }
        }

        #endregion

        #region cell content

        public override RenderFragment GetCellContentRenderFragment(GridControlRange range)
        {
            return new RenderFragment(b =>
            {
                string value;
                if (range.Column < 1 && range.Row < 1)
                {
                }
                else if (range.Row < 1)
                {
                    b.AddContent(0, GetColumnHeader(range.Column - 1));
                }
                else if (range.Column < 1)
                {
                    b.AddContent(0, (range.Row).ToString());
                }
                else if (_storedValues.TryGetValue(new KeyValuePair<int, int>(range.Row, range.Column), out value))
                {
                    b.AddContent(0, value);
                }
            });
        }

        private string GetColumnHeader(int column)
        {
            var charactersCount = (Z - A + 1);
            var sb = new StringBuilder();
            do
            {
                var c = column % charactersCount;
                sb.Insert(0, (char)(A + c));
                column = (column / charactersCount) - 1;
            }
            while (column >= 0);
            return sb.ToString();
        }

        #endregion

        #region selection

        public override bool IsSelectionEnabled => true;

        public override bool IsCellSelection => true;
        public override bool IsMultipleSelection => true;

        public override bool IsMultiRangeSelection => true;

        public override void RefreshSelectedRanges(IReadOnlyList<GridControlRange> ranges)
        {
            foreach (var range in ranges)
            {
                Grid.Refresh(range);
                Grid.Refresh(new GridControlRange(range.Row, 0, range.RowsCount, 1));
                Grid.Refresh(new GridControlRange(0, range.Column, 1, range.ColumnsCount));
            }
        }

        public override int GetSelectedTimes(GridControlRange range)
        {
            if (range.Row <= 0)
            {
                return Grid.SelectedRanges?.Count(r => r.ContainsColumn(range.Column)) ?? 0;
            }
            else if (range.Column <= 0)
            {
                return Grid.SelectedRanges?.Count(r => r.ContainsRow(range.Row)) ?? 0;
            }
            return base.GetSelectedTimes(range);
        }

        public override bool IsMouseOverEnabled => true;

        #endregion

        #region edit

        public override bool AllowEditing(GridControlRange range)
        {
            return range.Row >= 1 && range.Column >= 1;
        }

        protected override RenderFragment GetCellEditorFragment(GridControlRange range, Action<ComponentBase> editorCreated)
        {
            return new RenderFragment(b =>
            {
                string value;
                if (!_storedValues.TryGetValue(new KeyValuePair<int, int>(range.Row, range.Column), out value))
                {
                    value = "";
                }
                b.OpenComponent<C1TextBox>(0);
                b.AddAttribute(1, nameof(C1TextBox.Text), value);
                b.AddAttribute(2, nameof(C1TextBox.BorderThickness), new C1Thickness(0));
                b.AddAttribute(3, nameof(C1TextBox.Padding), CellPadding);
                b.AddComponentReferenceCapture(4, r => editorCreated(r as ComponentBase));
                b.CloseComponent();
            });
        }

        public override void OnEditEnded(GridControlRange range, ComponentBase editor, bool editCancelled)
        {
            if (!editCancelled)
            {
                var value = (editor as C1TextBox).Text;
                _storedValues[new KeyValuePair<int, int>(range.Row, range.Column)] = value;
            }
        }

        #endregion    
    }
}
