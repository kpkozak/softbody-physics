namespace Samples
{
    public class Configuration
    {
        public static Configuration Default = new Configuration()
        {
            Mode = Mode.Auto
        };

        public Mode Mode { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
    }
}