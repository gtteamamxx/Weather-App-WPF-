using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;

namespace WeatherApp.Behaviors
{
    public class ProgresBarAnimateBehavior : Behavior<ProgressBar>
    {
        bool _isAnimating = false;

        protected override void OnAttached()
        {
            base.OnAttached();
            ProgressBar progressBar = this.AssociatedObject;
            progressBar.ValueChanged += ProgressBar_ValueChanged;
        }

        private void ProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_isAnimating)
                return;

            _isAnimating = true;

            DoubleAnimation doubleAnimation = new DoubleAnimation
                (e.OldValue, e.NewValue, new Duration(TimeSpan.FromSeconds(0.3)), FillBehavior.Stop);
            doubleAnimation.Completed += doubleAnimationCompleted;

            ((ProgressBar)sender).BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);

            e.Handled = true;
        }

        private void doubleAnimationCompleted(object sender, EventArgs e)
        {
            _isAnimating = false;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ProgressBar progressBar = this.AssociatedObject;
            progressBar.ValueChanged -= ProgressBar_ValueChanged;
        }
    }
}
