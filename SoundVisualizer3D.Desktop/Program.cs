using Ninject;

namespace SoundVisualizer3D.Desktop
{
    class Program
    {
        static void Main()
        {
            using (IKernel kernel = new StandardKernel(new MonoGameModule()))
            {
                using (MonoGame monoGame = kernel.Get<MonoGame>())
                {
                    monoGame.Run();
                }
            }
        }
    }
}