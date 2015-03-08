﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Controls
{
    /// <summary>
    /// A UniformGrid implementation for Silverlight that arranges children in a
    /// grid with all equal cell sizes.
    /// </summary>
    public class UniformGrid : Panel
    {
        /// <summary>
        /// Gets or sets the computed row value.
        /// </summary>
        private int ComputedRows { get; set; }

        /// <summary>
        /// Gets or sets the computed column value.
        /// </summary>
        private int ComputedColumns { get; set; }

    	public int CellSpacing
    	{
    		get { return GetValue( CellSpacingProperty ).To<int>(); }
    		set { SetValue( CellSpacingProperty, value ); }
    	}	public static readonly DependencyProperty CellSpacingProperty = DependencyProperty.Register( "CellSpacing", typeof(int), typeof(UniformGrid), new PropertyMetadata( 5 ) );


        /// <summary>
        /// Gets or sets the number of first columns to leave blank.
        /// </summary>
        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
        }

        /// <summary>
        /// The FirstColumnProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstColumnProperty =
                DependencyProperty.Register(
                        "FirstColumn",
                        typeof(int),
                        typeof(UniformGrid),
                        new PropertyMetadata(0, OnIntegerDependencyPropertyChanged));

        /// <summary>
        /// Gets or sets the number of columns in the grid. A value of zero
        /// indicates that the count should be dynamically computed based on the
        /// number of rows and the number of non-collapsed children in the grid.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// DependencyProperty for the Columns property.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty =
                DependencyProperty.Register(
                        "Columns",
                        typeof(int),
                        typeof(UniformGrid),
                        new PropertyMetadata(0, OnIntegerDependencyPropertyChanged));

        /// <summary>
        /// Validate the new property value and silently revert if the new value
        /// is not appropriate. Used in place of WPF value coercian by the
        /// dependency properties in UniformGrid.
        /// </summary>
        /// <param name="o">The dependency object.</param>
        /// <param name="e">The dependency property.</param>
        private static void OnIntegerDependencyPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            // Silently coerce the value back to >= 0 if negative.
            if (!(e.NewValue is int) || (int)e.NewValue < 0)
            {
                o.SetValue(e.Property, e.OldValue);
            }
        }

        /// <summary>
        /// Gets or sets the number of rows in the grid. A value of zero
        /// indicates that the row count should be dynamically computed based on
        /// the number of columns and the number of non-collapsed children in
        /// the grid.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// The Rows DependencyProperty.
        /// </summary>
        public static readonly DependencyProperty RowsProperty =
                DependencyProperty.Register(
                        "Rows",
                        typeof(int),
                        typeof(UniformGrid),
                        new PropertyMetadata(0, OnIntegerDependencyPropertyChanged));

        /// <summary>
        /// Compute the desired size of the UniformGrid by measuring all of the
        /// children with a constraint equal to a cell's portion of the given
        /// constraint. The maximum child width and maximum child height are
        /// tracked, and then the desired size is computed by multiplying these
        /// maximums by the row and column count.
        /// </summary>
        /// <param name="availableSize">The size constraint.</param>
        /// <returns>Returns the desired size.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
        	UpdateComputedValues();

        	var spacing = new Size( ( ComputedColumns - 1 ) * CellSpacing, ( ComputedRows - 1 ) * CellSpacing );

        	var childConstraint = new Size( availableSize.Width / ComputedColumns, ( availableSize.Height - spacing.Height ) / ComputedRows );
        	var maxChildDesiredWidth = 0.0;
        	var maxChildDesiredHeight = 0.0;

        	//  Measure each child, keeping track of max desired width & height.
        	for ( int i = 0, count = Children.Count; i < count; ++i )
        	{
        		var child = Children[ i ];
        		child.Measure( childConstraint );
        		var childDesiredSize = child.DesiredSize;
        		if ( maxChildDesiredWidth < childDesiredSize.Width )
        		{
        			maxChildDesiredWidth = childDesiredSize.Width;
        		}
        		if (maxChildDesiredHeight < childDesiredSize.Height)
				{
					maxChildDesiredHeight = childDesiredSize.Height;
				}
        	}
        	var height = maxChildDesiredHeight;
        	var result = new Size( maxChildDesiredWidth * ComputedColumns, height * ComputedRows - spacing.Height );
        	return result;
        }

        /// <summary>
        /// Arrange the children of the UniformGrid by distributing space evenly
        /// among the children, making each child the size equal to a cell
        /// portion of the arrangeSize parameter.
        /// </summary>
        /// <param name="finalSize">The arrange size.</param>
        /// <returns>Returns the updated Size.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
        	var spacing = new Size( ( ComputedColumns - 1 ) * CellSpacing, ( ComputedRows - 1 ) * CellSpacing );
        	var childBounds = new Rect( 0, 0, finalSize.Width / ComputedColumns, ( finalSize.Height - spacing.Height ) / ComputedRows );
        	var xStep = childBounds.Width;
        	var xBound = finalSize.Width - 1.0;

        	childBounds.X += childBounds.Width * FirstColumn;

        	// Arrange and Position each child to the same cell size
        	foreach ( var child in Children )
        	{
        		child.Arrange( childBounds );
        		if ( child.Visibility != Visibility.Collapsed )
        		{
        			childBounds.X += xStep;
        			if ( childBounds.X >= xBound )
        			{
        				childBounds.Y += childBounds.Height + CellSpacing;
        				childBounds.X = 0;
        			}
        		}
        	}

        	return finalSize;
        }

        /// <summary>
        /// If the Rows or Columns values are set to 0, dynamically compute the
        /// values based on the actual number of non-collapsed children.
        /// </summary>
        /// <remarks>
        /// In the case when both Rows and Columns are set to 0, the Rows and
        /// Columns will be equal, laying out a square grid.
        /// </remarks>
        private void UpdateComputedValues()
        {
            ComputedColumns = Columns;
            ComputedRows = Rows;

            // Reset the first column. This is the same logic performed by WPF.
            if (FirstColumn >= ComputedColumns)
            {
                FirstColumn = 0;
            }

            if ((ComputedRows == 0) || (ComputedColumns == 0))
            {
                int nonCollapsedCount = 0;
                for (int i = 0, count = Children.Count; i < count; ++i)
                {
                    UIElement child = Children[i];
                    if (child.Visibility != Visibility.Collapsed)
                    {
                        nonCollapsedCount++;
                    }
                }
                if (nonCollapsedCount == 0)
                {
                    nonCollapsedCount = 1;
                }
                if (ComputedRows == 0)
                {
                    if (ComputedColumns > 0)
                    {
                        ComputedRows = (nonCollapsedCount + FirstColumn + (ComputedColumns - 1)) / ComputedColumns;
                    }
                    else
                    {
                        ComputedRows = (int)Math.Sqrt(nonCollapsedCount);
                        if ((ComputedRows * ComputedRows) < nonCollapsedCount)
                        {
                            ComputedRows++;
                        }
                        ComputedColumns = ComputedRows;
                    }
                }
                else if (ComputedColumns == 0)
                {
                    ComputedColumns = (nonCollapsedCount + (ComputedRows - 1)) / ComputedRows;
                }
            }
        }
    }
}