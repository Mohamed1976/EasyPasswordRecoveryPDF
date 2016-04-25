using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace EasyPasswordRecoveryPDF.Behaviors
{
    public class BrowserNavigateBehavior
    {
        public static readonly DependencyProperty NavigateToProperty =
             DependencyProperty.RegisterAttached("NavigateTo", typeof(Uri),
             typeof(BrowserNavigateBehavior), new PropertyMetadata(null, NavigateToPropertyChanged));

        public static Uri GetNavigateTo(DependencyObject obj)
        {
            return (Uri)obj.GetValue(NavigateToProperty);
        }

        public static void SetNavigateTo(DependencyObject obj, Uri value)
        {
            obj.SetValue(NavigateToProperty, value);
        }

        public static void NavigateToPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = sender as WebBrowser;
            if (webBrowser != null)
            {
                var uri = e.NewValue as Uri;
                if (uri != null)
                {
                    try
                    {
                        webBrowser.Navigate(uri);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception NavigationUrlPropertyChanged: " + ex.Message);
                    }
                }
            }
        }
    }
}
