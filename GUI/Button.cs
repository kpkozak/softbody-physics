using System;
using DemoGame.GUI;
using SFML.Graphics;
using SFML.Window;

namespace GUI
{
    public class Button : Drawable
    {
        private readonly RectangleShape _internalRectShape;
        private readonly Text _internalText;
        private bool _isActive;

        public bool IsActive { get { return _isActive; }
            set
            {
                _isActive = value;
                RefreshButtonColor(value);
            } }

        private void RefreshButtonColor(bool value)
        {
            var newColor = value ? new Color(0x12, 0x59, 0x70) : new Color(127, 127, 127);
            _internalRectShape.FillColor = newColor;
        }

        public Button()
        {
            _isActive = true;
            _internalRectShape = new RectangleShape()
            {
                FillColor = new Color(0x12, 0x59, 0x70),
                OutlineColor = Color.Black,
                OutlineThickness = 2
            };
            _internalText = new Text()
            {
                Color = Color.White,
                Font = Defaults.Font
            };
        }

        public Vector2f Position
        {
            get { return _internalRectShape.Position; }
            set
            {
                _internalRectShape.Position = value;
                RefreshTextPosition();
            }
        }

        public Vector2f Size
        {
            get { return _internalRectShape.Size; }
            set
            {
                _internalRectShape.Size = value;
                RefreshTextPosition();
            }
        }

        public string Text
        {
            get { return _internalText.DisplayedString; }
            set
            {
                _internalText.DisplayedString = value; 
                RefreshTextPosition();
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(_internalRectShape);
            target.Draw(_internalText);
        }

        private void RefreshTextPosition()
        {
            _internalText.Position = Position + (Size / 2) - 
                new Vector2f(_internalText.GetLocalBounds().Width/2, 
                _internalText.CharacterSize / 2);
        }

        public event EventHandler<ButtonPressedEventArgs> ButtonPressed;

        protected virtual void OnButtonPressed(ButtonPressedEventArgs e)
        {
            ButtonPressed?.Invoke(this, e);
        }

        public void Handler(object sender, MouseButtonEventArgs eventArgs)
        {
            if (IsActive && 
                ClickAreaChecker.ClickedInRange(eventArgs,Position, Size))
                OnButtonPressed(new ButtonPressedEventArgs());
        }
    }
}