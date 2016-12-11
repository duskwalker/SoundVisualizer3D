using System;
using System.Drawing;

namespace SoundVisualizer3D
{
    public class TrackChangedEventHandlerArgs
        : EventArgs
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Year { get; set; }
        public Image Image { get; set; } 
        public int TrackLength { get; set; }
    }
}