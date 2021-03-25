using Microsoft.Xaml.Behaviors;
using Roix.Wpf;
using System;
using System.Windows;
using System.Windows.Shapes;

namespace RoixApp.Wpf.Actions
{
    class SetArrowLineAction : TargetedTriggerAction<DependencyObject>
    {
        public static readonly DependencyProperty ArrowLengthMinProperty
            = DependencyProperty.Register(nameof(ArrowLengthMin), typeof(double), typeof(SetArrowLineAction), new FrameworkPropertyMetadata(20d));

        public double ArrowLengthMin
        {
            get => (double)GetValue(ArrowLengthMinProperty);
            set => SetValue(ArrowLengthMinProperty, value);
        }

        public static readonly DependencyProperty ArrowAngleMinProperty
            = DependencyProperty.Register(nameof(ArrowAngleMin), typeof(double), typeof(SetArrowLineAction), new FrameworkPropertyMetadata(45d));

        public double ArrowAngleMin
        {
            get => (double)GetValue(ArrowAngleMinProperty);
            set => SetValue(ArrowAngleMinProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (Target is not Line line) return;
            if (parameter is not DependencyPropertyChangedEventArgs e) return;

            RoixLine roixLine = e.NewValue switch
            {
                RoixLine l => l,
                RoixIntLine il => il,
                RoixBorderLine bl => bl.Line,
                RoixBorderIntLine bil => bil.Line,
                _ => throw new NotImplementedException()
            };

            // 矢印の長さ
            var length = Math.Min(roixLine.GetDistance(), ArrowLengthMin);

            // 基準線上に載りP2寄りで回転させて矢印の端にする点を求める
            var radian = Math.Atan2(roixLine.Y2 - roixLine.Y1, roixLine.X2 - roixLine.X1);
            var d = radian + AngleToRadian(ArrowAngleMin);
            var x = roixLine.X2 - length * Math.Cos(d);
            var y = roixLine.Y2 - length * Math.Sin(d);

            line.X1 = roixLine.X2;
            line.Y1 = roixLine.Y2;
            line.X2 = x;
            line.Y2 = y;
        }

        private static double AngleToRadian(double angle) => angle * Math.PI / 180d;
    }
}
