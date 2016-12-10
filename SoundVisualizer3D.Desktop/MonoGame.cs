using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoundVisualizer3D.Desktop.Render;
using SoundVisualizer3D.Desktop.Render.Objects.Visualizations;
using SoundVisualizer3D.Desktop.Render.Screen;

namespace SoundVisualizer3D.Desktop
{
    sealed class MonoGame
        : Game
    {
        #region Fields

        private SoundSource _soundSource;
        private IScene _scene;
        private GraphicsDeviceManager _graphics;

        #endregion

        public MonoGame(SoundSource soundSource)
        {
            _soundSource = soundSource;
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false
            };

            Content.RootDirectory = "Content";
        }

        #region Game Implementations

        protected override void LoadContent()
        {
            base.LoadContent();

            _scene = new Scene(this);

            
            _scene.AddObject(new WaveformVisualization(_soundSource));
            _scene.AddObject(new Arrow());
            _scene.AddObject(new Hud());
            
            _scene.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();

            _scene.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            _scene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);

                _scene.Render();
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}