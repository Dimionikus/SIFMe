using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace SIFMe.ExternalClasses
{
    public static class Animator
    {
        public static void AnimateDouble(DependencyObject target, DependencyProperty property, double to)
        {
            var animation = new DoubleAnimation
            {
                To = to,
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new ElasticEase { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(property));
            var sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin();
        }
    }
}
