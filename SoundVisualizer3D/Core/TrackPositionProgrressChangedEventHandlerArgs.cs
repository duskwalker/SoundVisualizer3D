using System;

namespace SoundVisualizer3D
{
    public class TrackPositionProgrressChangedEventHandlerArgs
        : EventArgs
    {
        public int PositionSeconds { get; set; }    
    }
}