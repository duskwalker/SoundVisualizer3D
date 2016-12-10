using System;

namespace SoundVisualizer3D
{
    public class FrequencesBandChangedEventHandlerArgs
        : EventArgs
    {
        public float[] FrequencesBand { get; set; } 
    }
}