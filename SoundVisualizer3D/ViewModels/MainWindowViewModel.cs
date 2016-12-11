using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Commands;
using SoundVisualizer3D.Properties;
using System.Windows.Media.Imaging;

namespace SoundVisualizer3D.ViewModels
{
    public class MainWindowViewModel
        : INotifyPropertyChanged
    {
        private int _currentPosition;
        private readonly SoundSource _soundSource;
        private ImageSource _image;

        public float[] FrequenciesValues { get; set; }
        public ICommand OnPlayCommand { get; }
        public ICommand OnStopCommand { get; }
        public int TrackLength { get; set; }

        public ImageSource Image
        {
            get { return _image; }
        }

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
            _image = CreateImageSource(args.Image);
            SongInfo = args.Artist + " - " + args.Title;
            OnPropertyChanged(nameof(TrackLength));
            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(SongInfo));
        }

        private ImageSource CreateImageSource(Image image)
        {
            if (image == null)
            {
                return null;
            }
            MemoryStream stream = new MemoryStream();
            image.Save(stream,ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            BitmapImage imageSource = new BitmapImage();
            imageSource.BeginInit();    
            imageSource.StreamSource = stream;
            imageSource.CacheOption = BitmapCacheOption.OnLoad;
            imageSource.EndInit();
            return imageSource;
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
