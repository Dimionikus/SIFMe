using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SIFMe.ExternalClasses
{
    public abstract class form_controls : UserControl
    {
        public FrameworkElement Dragger { get; set; }
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);
        public struct POINT { public int X; public int Y; }
        protected abstract POINT GetBottomBorderPos();
        public bool resizable
        {
            get => _resizable; set
            {
                if (_resizable != value) { _resizable = value; ResizeStateChange(); }
            }
        }
        protected bool _resizable = false;
        public Canvas parent;
        protected DispatcherTimer timer;
        public form_controls()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(20);
            timer.Tick += Timer_Tick;
        }
        private void ResizeStateChange()
        {
            if (parent == null) return;
            if (_resizable == true) { timer.Start(); }
            else { timer.Stop(); }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                POINT p;
                GetCursorPos(out p);
                PresentationSource source = PresentationSource.FromVisual(parent);
                double dpiX = 1.0, dpiY = 1.0;
                if (source != null)
                {
                    dpiX = source.CompositionTarget.TransformFromDevice.M11;
                    dpiY = source.CompositionTarget.TransformFromDevice.M22;
                }
                double cursorX = p.X * dpiX;
                double cursorY = p.Y * dpiY;
                POINT framePos = GetBottomBorderPos();
                Point mouseOffset = new Point(cursorX - Canvas.GetLeft(this) - framePos.X, cursorY - Canvas.GetTop(this) - framePos.Y);
                if (this.Width < 75 && mouseOffset.X < 0) { this.Width = 74; return; }
                this.Width += mouseOffset.X;
            }
            else { resizable = false; }
        }
    }
}
