using System;
using SFML.Graphics;
using SFML.Window;

namespace GUI {
    public class Checkbox : Drawable
    {
        private readonly RectangleShape _outboxRect;
        private readonly RectangleShape _innerRect;
        private readonly Text _text;

        public Vector2f Position { get; }
        public Vector2f Size { get; }
        public string Label { get; }

        public bool IsActive { get; set; }
        public bool IsChecked { get; set; }

        public event EventHandler<ValueChangedEventArgs<bool>> ValueChanged;

        public Checkbox(Vector2f position, Vector2f size, string label)
        {
            IsActive = true;
            Position = position;
            Size = size;
            Label = label;

            _text  = new Text(label,Defaults.Font, (uint) Size.Y)
            {
                Position = Position + new Vector2f(15,0),
                Origin = new Vector2f(0,0),
                Color = Color.Black
            };
            _outboxRect = new RectangleShape(Size){Position = Position, OutlineColor = Color.Black, OutlineThickness = 2};
            _innerRect =
                new RectangleShape(Size * 0.66f) {Position = Position + Size * 0.166f, FillColor = Color.Black};
        }

        public void Handler(object sender, MouseButtonEventArgs e)
        {
            if (IsActive &&
                ClickAreaChecker.ClickedInRange(e, Position, Size)) ChangeValue();
        }

        private void ChangeValue()
        {
            IsChecked = !IsChecked;
            ValueChanged?.Invoke(this, new ValueChangedEventArgs<bool>(!IsChecked, IsChecked));
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            _outboxRect.Draw(target,states);
            _text.Draw(target, states);
            if(IsChecked)
                _innerRect.Draw(target,states);
        }
    }
}