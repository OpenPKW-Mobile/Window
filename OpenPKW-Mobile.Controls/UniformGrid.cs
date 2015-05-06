using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OpenPKW_Mobile.Controls
{
    /// <summary>
    /// Siatka równomiernie rozmieszczonych elementów.
    /// </summary>
    public class UniformGrid : Panel
    {
        /// <summary>
        /// Oszacowanie rozmiarów elementów oraz siatki
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            double cellWidth = double.IsPositiveInfinity(availableSize.Width) ? 0 : availableSize.Width / Columns;
            double cellHeight = double.IsPositiveInfinity(availableSize.Height) ? 0 : availableSize.Width / Columns;
            double cellSide = Math.Max(cellWidth, cellHeight);

            // elementy są wyświetlane w obszarze kwadratu
            Size cellSize = new Size(cellSide, cellSide);
            foreach (UIElement child in Children)
                child.Measure(cellSize);

            double panelWidth = Math.Min(cellSide * Columns, availableSize.Width);
            double panelHeight = Math.Min(cellSide * Rows, availableSize.Height);
            Size panelSize = new Size(panelWidth, panelHeight);

            return panelSize;
        }

        /// <summary>
        /// Rozmieszczenie elementów w siatce.
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size cellSize = new Size(finalSize.Width / Columns, finalSize.Height / Rows);
            int row = 0, col = 0;
            foreach (UIElement child in Children)
            {
                child.Arrange(new Rect(new Point(cellSize.Width * col, cellSize.Height * row), cellSize));
                if (++col == Columns)
                {
                    row++;
                    col = 0;
                }
            }
            return finalSize;
        }

        /// <summary>
        /// Liczba kolumn.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Liczba wierszy.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public static readonly DependencyProperty ColumnsProperty =
             DependencyProperty.Register("Columns", typeof(int), typeof(UniformGrid), new PropertyMetadata(1, OnColumnsChanged));


        public static readonly DependencyProperty RowsProperty =
             DependencyProperty.Register("Rows", typeof(int), typeof(UniformGrid), new PropertyMetadata(1, OnRowsChanged));

        static void OnColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            int cols = (int)e.NewValue;
            if (cols < 1)
                ((UniformGrid)obj).Columns = 1;
        }

        static void OnRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            int rows = (int)e.NewValue;
            if (rows < 1)
                ((UniformGrid)obj).Rows = 1;
        }
    }
}
