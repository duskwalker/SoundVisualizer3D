using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoundVisualizer3D.Desktop.Render.Objects;
using SoundVisualizer3D.Desktop.Render.Objects.Visualizations;
using SoundVisualizer3D.Desktop.Render.Screen;

namespace SoundVisualizer3D.Desktop
{
    sealed class MonoGame
        : Game
    {
        #region Fields

        private SoundSource _soundSource;
        private GraphicsDeviceManager _graphics;
        private ICamera _camera;

        #endregion

        public MonoGame(SoundSource soundSource)
        {
            _soundSource = soundSource;
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false
            };

            _camera = new Camera(this);
            Services.AddService<ICamera>(_camera);

            Components.Add(_camera);
            Components.Add(new Arrow(this));
            Components.Add(new Hud(this));

            Content.RootDirectory = "Content";
        }

        #region Game Implementations

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            { 
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}