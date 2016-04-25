using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using EasyPasswordRecoveryPDF.Common;
using System.Windows.Controls.Primitives;

namespace EasyPasswordRecoveryPDF.Behaviors
{
    /// <summary>
    /// Double click DatagridRow invoke command
    /// </summary>
    public static class DoubleClickCommandBehavior
    {
        #region Double Click Command Property

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommand",
                typeof(RelayCommand), typeof(DoubleClickCommandBehavior),
                new PropertyMetadata(null, DoubleClickCommandChanged));

        public static RelayCommand GetDoubleClickCommand(DependencyObject obj)
        {
            return (RelayCommand)obj.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommand(DependencyObject obj, RelayCommand value)
        {
            obj.SetValue(DoubleClickCommandProperty, value);
        }

        #endregion

        #region Double Click Command Parameter Property

        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.RegisterAttached("DoubleClickCommandParameter",
                typeof(object), typeof(DoubleClickCommandBehavior),
                new PropertyMetadata(null));

        public static object GetDoubleClickCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(DoubleClickCommandParameterProperty);
        }

        public static void SetDoubleClickCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(DoubleClickCommandParameterProperty, value);
        }

        #endregion


        public static bool GetCanDoubleClickOnChild(DependencyObject obj)
        {
            return (bool)obj.GetValue(CanDoubleClickOnChildProperty);
        }

        public static void SetCanDoubleClickOnChild(DependencyObject obj, bool value)
        {
            obj.SetValue(CanDoubleClickOnChildProperty, value);
        }

        // Using a DependencyProperty as the backing store for Test.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanDoubleClickOnChildProperty =
            DependencyProperty.RegisterAttached("CanDoubleClickOnChild",
                typeof(bool), typeof(DoubleClickCommandBehavior), new UIPropertyMetadata(null));

        #region Double Click Command Changed

        private static void DoubleClickCommandChanged(DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            Selector selector = obj as Selector;
            if (selector != null)
            {
                selector.MouseDoubleClick += HandlePreviewMouseLeftButtonDown;
            }
        }

        private static void HandlePreviewMouseLeftButtonDown(object sender,
            MouseButtonEventArgs mouseEventArgs)
        {
            try
            {

                if (mouseEventArgs.LeftButton == MouseButtonState.Pressed)
                {
                    DependencyObject depObj = sender as DependencyObject;
                    DependencyObject mouseClickObj =
                        mouseEventArgs.OriginalSource as DependencyObject;
                    DataGridRow dataGridRow =
                        ItemsControl.ContainerFromElement(depObj as ItemsControl, mouseClickObj) as DataGridRow;
                    var parentOfMouseClickObj =
                        VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(mouseClickObj));
                    var dataGridDetailsPresenter = FindDataGridDetailsPresenter(mouseClickObj);

                    // Block command execute when click plus button or DataGridRowDetail 
                    //&& (dataGridDetailsPresenter == null)
                    bool canDoubleClickOnChild = GetCanDoubleClickOnChild(depObj);
                    if (!canDoubleClickOnChild && dataGridDetailsPresenter != null)
                        return;

                    //if (!(parentOfMouseClickObj is ToggleButton))
                    if (parentOfMouseClickObj is DataGridCellsPanel ||
                        parentOfMouseClickObj is Border ||
                        parentOfMouseClickObj is TextBlock ||
                        parentOfMouseClickObj is ContentPresenter)
                        if (dataGridRow != null && dataGridRow.IsSelected)
                        {
                            Selector selector = depObj as Selector;
                            if (selector != null)
                            {
                                if (selector.SelectedItem != null &&
                                    Keyboard.Modifiers != ModifierKeys.Control)
                                {
                                    RelayCommand command = GetDoubleClickCommand(depObj);
                                    object commandParameter =
                                        GetDoubleClickCommandParameter(depObj);
                                    if (command.CanExecute(commandParameter))
                                        command.Execute(commandParameter);
                                }
                            }
                        }
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Get ContainerView from children DependencyObject
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        private static DataGridDetailsPresenter
            FindDataGridDetailsPresenter(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            // Check if this is the end of the tree       
            if (parent == null)
                return null;

            DataGridDetailsPresenter parentWindow = parent as DataGridDetailsPresenter;
            if (parentWindow != null)
                return parentWindow;
            else
                // Use recursion until it reaches a Window           
                return FindDataGridDetailsPresenter(parent);
        }

        #endregion
    }
}
