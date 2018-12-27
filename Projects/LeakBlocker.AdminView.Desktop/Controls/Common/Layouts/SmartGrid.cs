using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Layouts
{
    internal sealed class SmartGrid : Grid
    {
        private GridLengthCollection rows;
        private GridLengthCollection columns;

        public GridLengthCollection Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
                RowDefinitions.Clear();
                if (rows == null)
                    return;
                foreach (var length in rows)
                {
                    RowDefinitions.Add(new RowDefinition
                    {
                        Height = length
                    });
                }
            }
        }

        public GridLengthCollection Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
                if (columns == null)
                    return;
                ColumnDefinitions.Clear();
                foreach (var length in columns)
                {
                    ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = length
                    });
                }
            }
        }
    }
}
