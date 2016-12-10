using System;

namespace SoundVisualizer3D
{
    public class TrackChangedEventHandlerArgs
        : EventArgs
    {
        public string Title { get; set; }   
        public int TrackLength { get; set; }
    }
}