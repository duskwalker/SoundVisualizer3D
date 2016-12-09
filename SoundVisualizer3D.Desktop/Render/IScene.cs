using Microsoft.Xna.Framework;

namespace SoundVisualizer3D.Desktop.Render
{
    interface IScene
    {
        void LoadContent();
        void Initialize();
        void Render();

        void Update(GameTime gameTime);
        void AddObject(SceneObject arrow);
    }
}