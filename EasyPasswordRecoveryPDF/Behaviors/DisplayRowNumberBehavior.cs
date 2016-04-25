using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace EasyPasswordRecoveryPDF.Behaviors
{
    public static class DisplayRowNumberBehavior
    {
        #region DisplayRowNumberOffset

        /// <summary>
        /// Sets the starting value of the row header if enabled
        /// </summary>
        public static DependencyProperty DisplayRowNumberOffsetProperty =
            DependencyProperty.RegisterAttached("DisplayRowNumberOffset",
                typeof(int),
                typeof(DisplayRowNumberBehavior),
                new FrameworkPropertyMetadata(0, OnDisplayRowNumberOffsetChanged));

        public static int GetDisplayRowNumberOffset(DependencyObject target)
        {
            return (int)target.GetValue(DisplayRowNumberOffsetProperty);
        }

        public static void SetDisplayRowNumberOffset(DependencyObject target, int value)
        {
            target.SetValue(DisplayRowNumberOffsetProperty, value);
        }

        private static void OnDisplayRowNumberOffsetChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = target as DataGrid;
            int offset = (int)e.NewValue;

            if (GetDisplayRowNumber(target))
            {
                GetVisualChildCollection<DataGridRow>(dataGrid).
                        ForEach(d => d.Header = d.GetIndex() + offset);
            }
        }

        #endregion

        #region DisplayRowNumber

        /// <summary>
        /// Enable display of row header automatically
        /// </summary>
        /// <remarks>
        /// Source: 
        /// </remarks>
        public static DependencyProperty DisplayRowNumberProperty =
            DependencyProperty.RegisterAttached("DisplayRowNumber",
                typeof(bool),
                typeof(DisplayRowNumberBehavior),
                new FrameworkPropertyMetadata(false, OnDisplayRowNumberChanged));

        public static bool GetDisplayRowNumber(DependencyObject target)
        {
            return (bool)target.GetValue(DisplayRowNumberProperty);
        }

        public static void SetDisplayRowNumber(DependencyObject target, bool value)
        {
            target.SetValue(DisplayRowNumberProperty, value);
        }

        private static void OnDisplayRowNumberChanged(DependencyObject target,
            DependencyPropertyChangedEventArgs e)
        {
            DataGrid dataGrid = target as DataGrid;
            if ((bool)e.NewValue == true)
            {
                int offset = GetDisplayRowNumberOffset(target);

                EventHandler<DataGridRowEventArgs> loadedRowHandler = null;
                loadedRowHandler = (object sender, DataGridRowEventArgs ea) =>
                {
                    if (GetDisplayRowNumber(dataGrid) == false)
                    {
                        dataGrid.LoadingRow -= loadedRowHandler;
                        return;
                    }
                    ea.Row.Header = ea.Row.GetIndex() + offset;
                };
                dataGrid.LoadingRow += loadedRowHandler;

                ItemsChangedEventHandler itemsChangedHandler = null;
                itemsChangedHandler = (object sender, ItemsChangedEventArgs ea) =>
                {
                    if (GetDisplayRowNumber(dataGrid) == false)
                    {
                        dataGrid.ItemContainerGenerator.ItemsChanged -= itemsChangedHandler;
                        return;
                    }
                    GetVisualChildCollection<DataGridRow>(dataGrid).
                        ForEach(d => d.Header = d.GetIndex() + offset);
                };
                dataGrid.ItemContainerGenerator.ItemsChanged += itemsChangedHandler;
            }
        }

        #endregion // DisplayRowNumber

        #region Get Visuals

        private static List<T> GetVisualChildCollection<T>(object parent) where T : Visual
        {
            List<T> visualCollection = new List<T>();
            GetVisualChildCollection(parent as DependencyObject, visualCollection);
            return visualCollection;
        }

        private static void GetVisualChildCollection<T>(DependencyObject parent,
            List<T> visualCollection) where T : Visual
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    visualCollection.Add(child as T);
                }
                if (child != null)
                {
                    GetVisualChildCollection(child, visualCollection);
                }
            }
        }

        #endregion // Get Visuals
    }
}
