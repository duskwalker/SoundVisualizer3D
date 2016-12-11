using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Prism.Commands;
using SoundVisualizer3D.Properties;

namespace SoundVisualizer3D.ViewModels
{
    public class MainWindowViewModel
        : INotifyPropertyChanged
    {
        private int _currentPosition;
        private readonly SoundSource _soundSource;

        public float[] FrequenciesValues { get; set; }
        public ICommand OnPlayCommand { get; }
        public ICommand OnStopCommand { get; }
        public int TrackLength { get; set; }
        public Image Image { get; set; }
        public string SongInfo { get; set; }    

        public int CurrentPosition
        {
            get { return _currentPosition; }
            set { _soundSource.SetPosition(value); }
        }

        public List<string> Files
        {
            get
            {
                return GetAudioFiles();
            }
        }

        public string SelectedFile { get; set; }

        public MainWindowViewModel()
        {
            _soundSource = new SoundSource();
           _soundSource.TrackPositionProgressChanged += SoundSourceOnTrackPositionProgressChanged;
            _soundSource.TrackChanged += SoundSourceOnTrackChanged;
            _soundSource.FrequencesBandChanged += SoundSourceOnFrequencesBandChanged;

            OnPlayCommand = new DelegateCommand(Play);
            OnStopCommand = new DelegateCommand(Stop);           
        }

        private void SoundSourceOnTrackPositionProgressChanged(object sender, TrackPositionProgrressChangedEventHandlerArgs args)
        {
            _currentPosition = args.PositionSeconds;
            OnPropertyChanged(nameof(CurrentPosition));
        }

        private void SoundSourceOnFrequencesBandChanged(object sender, FrequencesBandChangedEventHandlerArgs args)
        {
            FrequenciesValues = args.FrequencesBand;
            OnPropertyChanged(nameof(FrequenciesValues));
        }

        private void SoundSourceOnTrackChanged(object sender, TrackChangedEventHandlerArgs args)
        {
            TrackLength = args.TrackLength;
            Image = args.Image;
            SongInfo = args.Artist + " - " + args.Title;
            OnPropertyChanged(nameof(TrackLength));
            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(SongInfo));
        }

        public void Play()
        {
            CurrentPosition = 0;
            if (!string.IsNullOrEmpty(SelectedFile))
            {
                _soundSource.Play(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", SelectedFile));
            }
        }

        public void Stop()
        {
            _soundSource.Stop();
            CurrentPosition = 0;
        }

       private List<string> GetAudioFiles()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio");
            string[] files = Directory.GetFiles(path, "*.mp3");

            return files
                .Select(Path.GetFileName)
                    .ToList();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
