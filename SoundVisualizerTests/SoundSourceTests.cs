using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundVisualizer3D;

namespace SoundVisualizerTests
{
    [TestClass]
    public class SoundSourceTests    
    {
        [TestMethod]
        public void PlayFileTest()
        {
            //given
            SoundSource soundSource = new SoundSource();

            //when
            soundSource.Play(@"d:\Temp\01. Alan Walker - Faded.mp3");

            //then

        }
    }
}