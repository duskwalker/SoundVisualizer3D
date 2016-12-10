﻿using System;
using System.Linq;
using Un4seen.Bass;
using System.Timers;

namespace SoundVisualizer3D
{
    public delegate void TrackChangedEventHandler(object sender, TrackChangedEventHandlerArgs args);
    public delegate void FrequencesBandChangedEventHandler(object sender, FrequencesBandChangedEventHandlerArgs args);
    public delegate void TrackPositionProgrressChangedEventHandler(object sender, TrackPositionProgrressChangedEventHandlerArgs args);

    public class SoundSource
        : IDisposable
    {
        private readonly bool _deviceReady;
        private int _handle;
        private readonly float[] _frequenciesValues = new float[512];
        private readonly Timer _timer;
        private int _currentPosition;

        public event TrackChangedEventHandler TrackChanged;
        public event FrequencesBandChangedEventHandler FrequencesBandChanged;
        public event TrackPositionProgrressChangedEventHandler TrackPositionProgressChanged;

        public SoundSource(int frequencyScanInterval = 1)
        {
            // here Registration for BASS.NET
          
            _deviceReady = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            _timer = new Timer(frequencyScanInterval);
            _timer.Elapsed += TimerOnElapsed;
        }

        public void SetPosition(int seconds)
        {
            if (_deviceReady && _handle != 0 && GetTrackLength() >= seconds)
            {
                Bass.BASS_ChannelSetPosition(_handle, (double)seconds);
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
                OnTrackChanged(new TrackChangedEventHandlerArgs()
                {
                    TrackLength = GetTrackLength(),
                    Title = string.Join(" | ", Bass.BASS_ChannelGetTags(_handle, BASSTag.BASS_TAG_ID3))
                });
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

        private int GetTrackLength()
        {
            if (!_deviceReady || _handle == 0)
            {
                return 0;
            }

            long length = Bass.BASS_ChannelGetLength(_handle);
            return (int)Bass.BASS_ChannelBytes2Seconds(_handle, length);
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_deviceReady && _handle != 0)
            {
                Bass.BASS_ChannelGetData(_handle, _frequenciesValues, (int)BASSData.BASS_DATA_FFT512);
                OnFrequencesBandChanged(new FrequencesBandChangedEventHandlerArgs
                { FrequencesBand = _frequenciesValues.Select(f => f * 1000).ToArray() });

                long bytePosition = Bass.BASS_ChannelGetPosition(_handle);
               
                int position = (int)Bass.BASS_ChannelBytes2Seconds(_handle, bytePosition);
                if (_currentPosition < position)
                {
                    _currentPosition = position;
                    OnTrackPositionProgressChanged(new TrackPositionProgrressChangedEventHandlerArgs()
                    {
                        PositionSeconds = _currentPosition
                    });
                }
            }
        }

        protected virtual void OnTrackChanged(TrackChangedEventHandlerArgs args)
        {
            TrackChanged?.Invoke(this, args);
        }

        protected virtual void OnFrequencesBandChanged(FrequencesBandChangedEventHandlerArgs args)
        {
            FrequencesBandChanged?.Invoke(this, args);
        }

        protected virtual void OnTrackPositionProgressChanged(TrackPositionProgrressChangedEventHandlerArgs args)
        {
            TrackPositionProgressChanged?.Invoke(this, args);
        }

        public void Dispose()
        {
            _timer.Elapsed -= TimerOnElapsed;
            Bass.BASS_Free();
        }
    }
}