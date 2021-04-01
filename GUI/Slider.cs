using System;
using SFML.Graphics;
using SFML.Window;

namespace GUI {
    public class Slider : Drawable
    {
        private bool _dragging;
        private readonly RectangleShape _rectangle;
        private readonly RectangleShape _bar;
        public double Value { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public bool IsActive { get; set; }
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }

        public Slider(double minValue, double maxValue, double value, Vector2f position, Vector2f size)
        {
            IsActive = true;
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
            Position = position;
            Size = size;
            _rectangle = new RectangleShape(Size)
            {
                FillColor = new Color(127,127,127)
            };
            _bar = new RectangleShape(new Vector2f(Size.Y, Size.Y))
            {
                FillColor = Color.Black
            };
            _rectangle.Position = Position;
            AdjustBarPosition();
        }

        private void AdjustBarPosition()
        {
            _bar.Position = Position + new Vector2f((float) (GetPercentageValue(Value) * (Size.X-Size.Y)), 0);
        }

        private double GetPercentageValue(double value)
        {
            return (value - MinValue) / (MaxValue - MinValue);
        }

        public void MouseDragHandler(object sender, MouseMoveEventArgs eventArgs)
        {
            if (!_dragging)
                return;

            var percentPosition = (eventArgs.X-Position.X)/Size.X;

            ChangeValue(MinValue + (percentPosition * (MaxValue - MinValue)));
        }

        public void MouseReleasedHandler(object sender, MouseButtonEventArgs eventArgs)
        {
            if (_dragging) _dragging = false;
        }

        public void MousePressedHandler(object sender, MouseButtonEventArgs eventArgs)
        {
            if (IsActive && !_dragging &&
                ClickAreaChecker.ClickedInRange(eventArgs,Position,Size))
            {
                _dragging = true;
            }
        }

        private void ChangeValue(double newValue)
        {
            newValue = Math.Min(Math.Max(MinValue, newValue), MaxValue);
            var eventArgs = new ValueChangedEventArgs<double>(Value, newValue);
            Value = newValue;
            AdjustBarPosition();
            RaiseValueChanged(eventArgs);
        }

        private void RaiseValueChanged(ValueChangedEventArgs<double> eventArgs)
        {
            ValueChanged?.Invoke(this, eventArgs);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            var text = new Text(Value.ToString("F1"),Defaults.Font, (uint) Size.Y)
            {
                Color = Color.Black,
                Position = Position + new Vector2f(Size.X + 15,Size.Y/3),
                Origin = new Vector2f(0,Size.Y/2),
               
            };
            
            target.Draw(_rectangle, states);
            target.Draw(_bar, states);
            target.Draw(text, states);
        }

        public event ValueChangedEventHandler ValueChanged;
        public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs<double> eventArgs);

    }
}