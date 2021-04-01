using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace GUI
{
    class Settings
    {
        public static readonly Settings Default = new Settings();

        private Font Font { get; set; }
    }
}
