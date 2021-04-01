using SFML.Window;

namespace GUI {
    public static class ClickAreaChecker {

        public static bool ClickedInRange(MouseButtonEventArgs eventArgs, Vector2f position, Vector2f size)
        {
            return eventArgs.X > position.X && eventArgs.X < position.X + size.X &&
                   eventArgs.Y > position.Y && eventArgs.Y < position.Y + size.Y;
        }
    }
}