using Microsoft.Xna.Framework;

namespace SoundVisualizer3D.Desktop.Render.Objects.Cameras
{
    interface ICamera
        : IGameComponent
    {
        Matrix Projection { get; }
        Matrix View { get; }
        Matrix World { get; }

        Vector3 Position { get; }
    }
}