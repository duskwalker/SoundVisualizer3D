using SoundVisualizer3D.Desktop.Render.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoundVisualizer3D.Desktop.Render.Screen
{
    sealed class Hud
        : ScreenObject
    {
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        public Hud()
        {
            Visible = true;
        }

        public override void LoadContent()
        {
            _font = Content.Load<SpriteFont>("DefaultFont");
        }

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenAxisPosition = GetScreenPosition(new Vector2(10, 10), ScreenAllocation.TopLeft);
        }

        public override void Render(ICamera camera)
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, string.Format("Camera: X:{0:00.00} Y:{1:00.00} Z:{2:00.00}", camera.Position.X, camera.Position.Y, camera.Position.Z), ScreenAxisPosition, Color.Black);
            
            _spriteBatch.DrawString(_font, "Controls: A,S,D,W - Movement (Left,Back,Right,Forward)", Vector2.Add(ScreenAxisPosition, new Vector2(0, 50)), Color.Black);
            _spriteBatch.DrawString(_font, "Controls: Q,E - Movement (Up, Down)", Vector2.Add(ScreenAxisPosition, new Vector2(0, 75)), Color.Black);

            _spriteBatch.End();
        }
    }
}