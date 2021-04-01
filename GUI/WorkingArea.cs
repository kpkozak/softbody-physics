using System;
using SFML.Graphics;
using SFML.Window;

namespace GUI {
    public class WorkingArea:Drawable
    {
        private RectangleShape _rectangle;
        public Vector2f Size { get; }
        public Vector2f Position { get; }
        public Color FillColor { get; set; }

        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;

        public WorkingArea(Vector2f position, Vector2f size, Color fillColor)
        {
            Position = position;
            Size = size;
            FillColor = fillColor;
            _rectangle = new RectangleShape(Size) {Position = Position, FillColor = FillColor};

        }

        protected virtual void RaiseMousePressed(MouseButtonEventArgs e)
        {
            MouseButtonPressed?.Invoke(this, e);
        }

        public void MousePressedHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.X >= Position.X && e.X <= Position.X + Size.X &&
                e.Y >= Position.Y && e.Y <= Position.Y + Size.Y)
                RaiseMousePressed(new MouseButtonEventArgs(new MouseButtonEvent())
                {
                    Button = e.Button,
                    X = (int) (e.X - Position.X),
                    Y = (int) (e.Y - Position.Y)
                });
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _rectangle.Draw(target, states);
        }
    }
}