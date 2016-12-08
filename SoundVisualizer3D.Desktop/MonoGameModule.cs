﻿using Ninject.Modules;

namespace SoundVisualizer3D.Desktop
{
    public sealed class MonoGameModule
        : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<MonoGame>()
                    .ToSelf()
                        .InSingletonScope();

            Kernel.Bind<SoundSource>()
                    .ToSelf()
                        .InSingletonScope();
        }
    }
}