using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using SoundVisualizer3D.Annotations;

namespace SoundVisualizer3D
{
    public class MainWindowViewModel
        : INotifyPropertyChanged
    {
        private readonly SoundSource _soundSource;
        private readonly string _currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        public float[] FrequenciesValues { get; set; }
       
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
            FrequenciesValues = _soundSource.FrequenciesValues;
        }

        private void SoundSourceOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            FrequenciesValues = _soundSource.FrequenciesValues;
            OnPropertyChanged(nameof(FrequenciesValues));
        }

        public void Play()
        {
            if (!string.IsNullOrEmpty(SelectedFile))
            {
                _soundSource.Play(Path.Combine(_currentDirectory,"Audio",SelectedFile));
            }
        }

        public void Stop()
        {
            _soundSource.Stop();
        }

        private List<string> GetAudioFiles()
        {
            return Directory.GetFiles(Path.Combine(_currentDirectory, "Audio"),"*.mp3").Select(Path.GetFileName).ToList();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
