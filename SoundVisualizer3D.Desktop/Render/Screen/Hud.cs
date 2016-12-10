using SoundVisualizer3D.Desktop.Render.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundVisualizer3D.Desktop.Render.Objects.Cameras;

namespace SoundVisualizer3D.Desktop.Render.Screen
{
    sealed class Hud
        : ScreenObject
    {
        #region Fields

        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private ICamera _camera;

        #endregion

        public Hud(Game game)
            : base(game) { }

        #region DrawableGameComponent Implementations

        protected override void LoadContent()
        {
            _font = Game.Content.Load<SpriteFont>("DefaultFont");

            base.LoadContent();
        }

        public override void Initialize()
        {
            _camera = Game.Services.GetService<ICamera>();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenAxisPosition = GetScreenPosition(new Vector2(10, 10), ScreenAllocation.TopLeft);

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {            
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, string.Format("Camera: X:{0:00.00} Y:{1:00.00} Z:{2:00.00}", _camera.Position.X, _camera.Position.Y, _camera.Position.Z), ScreenAxisPosition, Color.WhiteSmoke);
            
            _spriteBatch.DrawString(_font, "Controls: A,S,D,W - Movement (Left, Back, Right, Forward)", Vector2.Add(ScreenAxisPosition, new Vector2(0, 50)), Color.WhiteSmoke);
            _spriteBatch.DrawString(_font, "Controls: Q,E - Movement (Up, Down)", Vector2.Add(ScreenAxisPosition, new Vector2(0, 75)), Color.WhiteSmoke);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}