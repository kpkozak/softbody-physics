using System;
using SFML.Graphics;
using SFML.Window;

namespace GUI
{
    public class Switch : Drawable
    {
        private readonly Vector2f _position;
        private readonly Vector2f _size;
        private readonly RectangleShape _background;
        private readonly CircleShape _trigger;
        private readonly Text _checkedLabel;
        private readonly Text _uncheckedLabel;
        private bool _isChecked;

        public bool IsActive { get; set; }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value)
                    return;

                ChangeValue();
            }
        }

        public Switch(Vector2f position, Vector2f size, string uncheckedLabel,string checkedLabel)
        {
            IsActive = true;
            _position = position;
            _size = size;
            _background = new RectangleShape()
            {
                
                FillColor = new Color(127,127,127),
                Position = position,
                Size = size
            };

            var radius = (float) (size.Y * 0.67);
            _trigger = new CircleShape()
            {
                Radius = radius,
                Position = position,
                FillColor = Color.Black,
                Origin = new Vector2f((float) (radius*0.8),(float) (0.1*size.Y))
            };
            _checkedLabel = new Text(checkedLabel, Defaults.Font, (uint) size.Y)
            {
                Color = Color.Black,
                Font = Defaults.Font
            };
            _checkedLabel.Position = position + new Vector2f(size.X+20, 0);

            _uncheckedLabel = new Text(uncheckedLabel, Defaults.Font, (uint)size.Y)
            {
                Color = Color.Black,
                Font = Defaults.Font
            };
            _uncheckedLabel.Position = position - new Vector2f(_uncheckedLabel.GetLocalBounds().Width+15, 0);
        }

        public event EventHandler<ValueChangedEventArgs<bool>> ValueChanged;

        public void Draw(RenderTarget target, RenderStates states)
        {
            _background.Draw(target,states);
            _trigger.Draw(target,states);
            _checkedLabel.Draw(target,states);
            _uncheckedLabel.Draw(target, states);
        }

        protected virtual void RaiseValueChanged()
        {
            ValueChanged?.Invoke(this, new ValueChangedEventArgs<bool>(!IsChecked, IsChecked));
        }

        public void Handler(object sender, MouseButtonEventArgs args)
        {
            if (IsActive &&
                ClickAreaChecker.ClickedInRange(args, _position, _size))
            {
                ChangeValue();
            }
        }

        private void ChangeValue()
        {
            _isChecked = !_isChecked;
            UpdateTriggerPosition();
            RaiseValueChanged();
        }

        private void UpdateTriggerPosition()
        {
            _trigger.Position = IsChecked ? _position + new Vector2f(_size.X, 0) : _position;
        }
    }
}