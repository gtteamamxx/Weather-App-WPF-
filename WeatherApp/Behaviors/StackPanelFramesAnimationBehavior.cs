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
using WeatherApp.MVVMMessages;

namespace WeatherApp.Behaviors
{
    public class StackPanelFramesAnimationBehavior : Behavior<StackPanel>
    {
        private List<Frame> _frames;
        private WeatherWindowViewModel _weatherWindowViewModel;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += StackPanel_Loaded;
            MVVMMessagerService.RegisterReceiver<AnimatePageChangingMessage>(typeof(AnimatePageChangingMessage), PageChangeRequest);
        }

        private void PageChangeRequest(AnimatePageChangingMessage message)
        {
            int frameIndexToAnimate = message.FrameToAnimate;

            ThicknessAnimation thicknessAnimation = new ThicknessAnimation()
            {
                Duration = TimeSpan.FromSeconds(0.3),
                To = new Thickness(frameIndexToAnimate == 1 ? -800 : 0, 0, 0, 0),
                DecelerationRatio = 0.1,
                From = new Thickness(frameIndexToAnimate == 1 ? 0 : -800, 0, 0, 0),
                FillBehavior = FillBehavior.HoldEnd
            };

            _frames[0].BeginAnimation(Frame.MarginProperty, thicknessAnimation);
        }

        private void StackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            _frames = new List<Frame>
            {
                AssociatedObject.Children[0] as Frame,
                AssociatedObject.Children[1] as Frame
            };

            _weatherWindowViewModel = AssociatedObject.DataContext as WeatherWindowViewModel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= StackPanel_Loaded;
            MVVMMessagerService.UnregisterReceiver(typeof(StackPanelFramesAnimationBehavior));
        }
    }
}
