using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
using WeatherApp.Properties;

namespace WeatherApp.Behaviors
{
    class ShowDetailGridHeightAnimationBehavior : Behavior<Grid>
    {
        private bool _isOpened;

        protected override void OnAttached()
        {
            base.OnAttached();
            Grid grid = AssociatedObject;
            grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isOpened = !_isOpened;

            Storyboard arrowImageAnimation = getArrowImageAnimation();
            arrowImageAnimation.Begin();
            AssociatedObject.BeginAnimation(Grid.HeightProperty, getHeightAnimation());

            Storyboard getArrowImageAnimation()
            {
                var arrowImage = (AssociatedObject.Children[0] as StackPanel).Children.Cast<UIElement>().First(p => p is Image) as Image;
                return arrowImage.FindResource(_isOpened ?
                "RotateImageAsUpArrowFromDownArrow" : "RotateImageAsDownArrowFromUpArrow") as Storyboard; ;
            }

            DoubleAnimation getHeightAnimation()
            {
                return new DoubleAnimation(
                    _isOpened ? 20 : 150, 
                    _isOpened ? 150 : 20, 
                    new Duration(TimeSpan.FromSeconds(0.15)));
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            Grid grid = AssociatedObject;
            grid.MouseLeftButtonDown -= Grid_MouseLeftButtonDown;
        }
    }
}
