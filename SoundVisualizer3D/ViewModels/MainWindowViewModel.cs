using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SoundVisualizer3D.Properties;
using SoundVisualizer3D.Utilities;

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

        public int CurrentPosition
        {
            get { return _currentPosition; }
            set { _soundSource.CurrentPositionSeconds = value; }
        }

        public int TrackLength
        {
            get { return _soundSource.TrackLength; }
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
            _soundSource.PropertyChanged += SoundSourceOnPropertyChanged;

            OnPlayCommand = new DelegateCommand(Play);
            OnStopCommand = new DelegateCommand(Stop);
            
            FrequenciesValues = _soundSource.FrequenciesValues;
        }

        public void Play()
        {
            if (!string.IsNullOrEmpty(SelectedFile))
            {
                _soundSource.Play(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio", SelectedFile));
            }
        }

        public void Stop()
        {
            _soundSource.Stop();
        }

        private void SoundSourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName.Equals("FrequenciesValues"))
            {
                FrequenciesValues = _soundSource.FrequenciesValues;
                OnPropertyChanged(nameof(FrequenciesValues));
            }

            if (propertyChangedEventArgs.PropertyName.Equals("CurrentPositionSeconds"))
            {
                _currentPosition = _soundSource.CurrentPositionSeconds;
                OnPropertyChanged(nameof(CurrentPosition));
            }

            if (propertyChangedEventArgs.PropertyName.Equals("TrackLength"))
            {
                OnPropertyChanged(nameof(TrackLength));
            }
        }

        private List<string> GetAudioFiles()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Audio");
            var files = Directory.GetFiles(path, "*.mp3");

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
