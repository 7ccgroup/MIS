using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace BaseAppUI.Controls
{
    public class RippleEffectDecorator : ContentControl
    {
        static RippleEffectDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RippleEffectDecorator), new FrameworkPropertyMetadata(typeof(RippleEffectDecorator)));
        }

        public Brush HighlightBackground
        {
            get { return (Brush)GetValue(HighlightBackgroundProperty); }
            set { SetValue(HighlightBackgroundProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackground", typeof(Brush), typeof(RippleEffectDecorator), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty RadiusProperty =
           DependencyProperty.Register("Radius", typeof(double), typeof(RippleEffectDecorator), new PropertyMetadata(0.0));
   

       

        Ellipse ellipse;
        Grid grid;
        Storyboard animation;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ellipse = GetTemplateChild("PART_ellipse") as Ellipse;
            grid = GetTemplateChild("PART_grid") as Grid;
            animation = grid.FindResource("PART_animation") as Storyboard;

           

            var handler=new RoutedEventHandler((sender, e) =>
            {
                if (Radius != 0)
                    this.Clip = new RectangleGeometry(new Rect(new Size(this.ActualWidth, this.ActualHeight)), Radius, Radius);


                var targetWidth = Math.Max(ActualWidth, ActualHeight) * 2;
                var mousePosition = (e as MouseButtonEventArgs).GetPosition(this);
                var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
                ellipse.Margin = startMargin;
                (animation.Children[0] as DoubleAnimation).To = targetWidth;
                (animation.Children[1] as ThicknessAnimation).From = startMargin;
                (animation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - targetWidth / 2, mousePosition.Y - targetWidth / 2, 0, 0);
                ellipse.BeginStoryboard(animation);
            });

            this.AddHandler(MouseDownEvent, handler, true);
       
        }
    }
}