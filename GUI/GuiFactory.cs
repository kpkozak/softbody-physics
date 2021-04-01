using SFML.Graphics;
using SFML.Window;

namespace GUI {
    public class GuiFactory
    {
        private readonly RenderWindow _window;
        private readonly Vector2f _offset;
        private readonly Vector2u _workingAreaSize;
        private readonly int _xCellsCount;
        private readonly int _yCellsCount;
        private readonly Vector2f _xCellSize;
        private readonly Vector2f _yCellSize;

        public GuiFactory(RenderWindow window, Vector2u workingAreaSize,Vector2f offset, int xCellsCount, int yCellsCount)
        {
            _window = window;
            _offset = offset;
            _workingAreaSize = workingAreaSize;
            _xCellsCount = xCellsCount;
            _yCellsCount = yCellsCount;

            _xCellSize = new Vector2f(workingAreaSize.X / xCellsCount,0);
            _yCellSize = new Vector2f(0, workingAreaSize.Y / yCellsCount);
        }

        public Label CreateLabel(int cellX, int cellY, int xSize, int ySize, string text)
        {
            return new Label()
            {
                Position = CountPosition(cellX, cellY),
                Size = CountSize(xSize, ySize),
                Text = text
            };
        }

        public Button CreateButton(int cellX, int cellY, int xSize, int ySize, string text)
        {
            var button = new Button()
            {
                Position = CountPosition(cellX, cellY),
                Size = CountSize(xSize, ySize),
                Text = text
            };
            _window.MouseButtonPressed += button.Handler;
            return button;
        }

        public Checkbox CreateCheckbox(int cellX, int cellY, int xSize, int ySize, string text, bool isChecked = false)
        {
            var checkbox = new Checkbox(CountPosition(cellX, cellY), CountSize(xSize, ySize), text);
            checkbox.IsChecked = isChecked;
            _window.MouseButtonPressed += checkbox.Handler;
            return checkbox;
        }

        public Slider CreateSlider(double minValue, double maxValue, int cellX, int cellY, int xSize, int ySize)
        {
            var slider = new Slider(minValue, maxValue, minValue, CountPosition(cellX, cellY), CountSize(xSize, ySize));
            _window.MouseButtonPressed += slider.MousePressedHandler;
            _window.MouseMoved += slider.MouseDragHandler;
            _window.MouseButtonReleased += slider.MouseReleasedHandler;
            return slider;
        }

        public Switch CreateSwitch(int cellX, int cellY, int xSize, int ySize, string uncheckedLabel,
            string checkedLabel)
        {
            var sw = new Switch(CountPosition(cellX, cellY), CountSize(xSize, ySize), uncheckedLabel, checkedLabel);
            _window.MouseButtonPressed += sw.Handler;
            return sw;
        }

        private Vector2f CountSize(int xSize, int ySize)
        {
            return xSize * _xCellSize + ySize * _yCellSize;
        }

        private Vector2f CountPosition(int cellX, int cellY)
        {
            return _offset + cellX*_xCellSize + cellY*_yCellSize;
        }
    }
}