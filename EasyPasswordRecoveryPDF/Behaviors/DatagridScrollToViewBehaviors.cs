using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace EasyPasswordRecoveryPDF.Behaviors
{
    class DatagridScrollToViewBehaviors : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= new SelectionChangedEventHandler(AssociatedObject_SelectionChanged);
        }

        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid)
            {
                DataGrid grid = (sender as DataGrid);
                if (grid.SelectedItem != null)
                {
                    Action action = delegate () {
                        grid.UpdateLayout();
                        grid.ScrollIntoView(grid.SelectedItem, null);
                        grid.Focus(); // added this to make it focus to the grid
                    };
                    grid.Dispatcher.BeginInvoke(action);
                }
            }
        }
    }
}
