using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeyenZylstra.PhotoKiosk.Options
{
    public class PhotoOptions
    {
        public string StoragePath { get; set; }
        public string LogoFilename { get; set; }
        public int LogoX { get; set; }
        public int LogoY { get; set; }
        public int LogoWidth { get; set; }
        public int LogoHeight { get; set; }
    }
}
