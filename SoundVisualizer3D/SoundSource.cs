using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SoundVisualizer3D.Annotations;
using Un4seen.Bass;
using System.Timers;

namespace SoundVisualizer3D
{
    public class SoundSource
        : INotifyPropertyChanged
        , IDisposable
    {
        private readonly bool _deviceReady;
        private int _handle;
        private readonly float[] _frequenciesValues = new float[512];
        private readonly Timer _timer;

        public float[] FrequenciesValues
        {
            get { return _frequenciesValues.Select(f=> f * 1000).ToArray(); }
        }

        public SoundSource(int frequencyScanInterval=1)
        {
            BassNet.Registration("maximfleitling@yahoo.de", "2X223152334323");
            _deviceReady = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            _timer = new Timer(frequencyScanInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_deviceReady && _handle != 0)
            {
                Bass.BASS_ChannelGetData(_handle, _frequenciesValues, (int)BASSData.BASS_DATA_FFT512);
                OnPropertyChanged(nameof(FrequenciesValues));
            }
        }

        public void Play(string fileName, int vol = 100)
        {
            if (_handle != 0)
            {
                Stop();
            }

            _handle = Bass.BASS_StreamCreateFile(fileName, 0, 0, BASSFlag.BASS_DEFAULT);
            if (_handle != 0)
            {
                Bass.BASS_ChannelSetAttribute(_handle, BASSAttribute.BASS_ATTRIB_VOL, vol / 100f);
                Bass.BASS_ChannelPlay(_handle, false);
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_deviceReady && _handle != 0)
            {
                Bass.BASS_ChannelStop(_handle);
                _timer.Stop();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _timer.Elapsed -= TimerOnElapsed;           
        }
    }
}
