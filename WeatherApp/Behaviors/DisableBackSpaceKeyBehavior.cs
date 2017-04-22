using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WeatherApp.Behaviors
{
    class DisableBackSpaceKeyBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += Window_KeyDown;
        }

        private static void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Back
                || e.Key == Key.BrowserBack)
            {
                e.Handled = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= Window_KeyDown;
        }
    }
}
