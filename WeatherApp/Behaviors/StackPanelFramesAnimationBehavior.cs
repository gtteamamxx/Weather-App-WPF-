using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
using WeatherApp.ViewModels;
using WeatherApp.Models.Application;

namespace WeatherApp.Behaviors
{
    class StackPanelFramesAnimationBehavior : Behavior<StackPanel>
    {
        private List<Frame> _Frames;
        private WeatherWindowViewModel _WeatherWindowViewModel;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += StackPanel_Loaded;
            MVVMMessagerService.RegisterReceiver<int>(typeof(StackPanelFramesAnimationBehavior), PageChangeRequest);
        }

        private void PageChangeRequest(int frameIndexToAnimate)
        {
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.3),
                To = new Thickness(frameIndexToAnimate == 1 ? -800 : 0, 0, 0, 0),
                DecelerationRatio = 0.1,
                From = new Thickness(frameIndexToAnimate == 1 ? 0 : -800, 0, 0, 0),
                FillBehavior = FillBehavior.HoldEnd
            };

            _Frames[0].BeginAnimation(Frame.MarginProperty, thicknessAnimation);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _Frames = new List<Frame>
            {
                AssociatedObject.Children[0] as Frame,
                AssociatedObject.Children[1] as Frame
            };

            _WeatherWindowViewModel = AssociatedObject.DataContext as WeatherWindowViewModel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= StackPanel_Loaded;
            MVVMMessagerService.UnregisterReceiver(typeof(StackPanelFramesAnimationBehavior));
        }
    }
}
