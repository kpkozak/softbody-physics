using SFML.Graphics;

namespace GUI {
    public static class Defaults
    {
        static Defaults()
        {
            Font = new Font("font.ttf");
        }

        public static Font Font { get; set; } 
    }
}