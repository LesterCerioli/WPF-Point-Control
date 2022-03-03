using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlePonto.WPF.framework
{
    public static class InitialFocusBehavior
    {
        public static bool GetFocus(DependencyObject element)
        {
            return (bool)element.GetValue(FocusProperty);
        }

        public static void SetFocus(DependencyObject element, bool value)
        {
            element.SetValue(FocusProperty, value);
        }

        public static readonly DependencyProperty FocusProperty =
            DependencyProperty.RegisterAttached(
            "Focus",
            typeof(bool),
            typeof(InitialFocusBehavior),
            new UIPropertyMetadata(false, OnElementFocused));

        static void OnElementFocused(
            DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = depObj as FrameworkElement;
            if (element == null)
                return;
            element.Focus();
        }
    }
}
