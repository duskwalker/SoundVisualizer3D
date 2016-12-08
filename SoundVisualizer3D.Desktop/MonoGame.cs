using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoundVisualizer3D.Desktop.Visualization;

namespace SoundVisualizer3D.Desktop
{
    sealed class MonoGame
        : Game
    {
        #region Fields

        private SoundSource _soundSource;
        private GraphicsDeviceManager _graphics;

        #endregion

        public MonoGame(SoundSource soundSource)
        {
            _soundSource = soundSource;
            _graphics = new GraphicsDeviceManager(this);

            // Add Component
            Components.Add(new TestVisualization(this));
            Components.Add(new WaveformVisualization(this, soundSource));

            Content.RootDirectory = "Content";
        }

        #region Game Implementations

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                    || Keyboard.GetState().IsKeyDown(Keys.Escape))
            { 
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        #endregion
    }
}