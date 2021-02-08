using Microsoft.Xaml.Behaviors;
using Roix.Core;
using Roix.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Input;

namespace Roix.Wpf.Actions
{
    class CaptureMousePointAction : TriggerAction<FrameworkElement>
    {
        enum CaptureButton { None, Left, Right, Middle }
        static CaptureButton _captureButton = CaptureButton.None;

        public static readonly DependencyProperty MousePointProperty
            = DependencyProperty.Register(nameof(MousePoint), typeof(RoixPointDouble), typeof(CaptureMousePointAction));
        public RoixPointDouble MousePoint
        {
            get => (RoixPointDouble)GetValue(MousePointProperty);
            set => SetValue(MousePointProperty, value);
        }

        protected override void Invoke(object parameter)
        {
            if (parameter is not MouseEventArgs e) return;
            if (AssociatedObject is not IInputElement inputElement) return;

            CaptureMouse(inputElement, e);
            MousePoint = e.GetPosition(inputElement).ToRoixPointDouble();
        }

        /// <summary>マウス操作の補足/解除</summary>
        private static void CaptureMouse(IInputElement inputElement, MouseEventArgs e)
        {
            if (_captureButton == CaptureButton.None)
            {
                var button = e switch
                {
                    { LeftButton: MouseButtonState.Pressed } => CaptureButton.Left,
                    { RightButton: MouseButtonState.Pressed } => CaptureButton.Right,
                    { MiddleButton: MouseButtonState.Pressed } => CaptureButton.Middle,
                    _ => CaptureButton.None,
                };

                if (button != CaptureButton.None)
                {
                    _captureButton = button;
                    inputElement.CaptureMouse();
                }
            }
            else
            {
                var release = _captureButton switch
                {
                    CaptureButton.Left when e.LeftButton == MouseButtonState.Released => true,
                    CaptureButton.Right when e.RightButton == MouseButtonState.Released => true,
                    CaptureButton.Middle when e.MiddleButton == MouseButtonState.Released => true,
                    _ => false,
                };

                if (release)
                {
                    inputElement.ReleaseMouseCapture();
                    _captureButton = CaptureButton.None;
                }
            }
        }

    }
}
