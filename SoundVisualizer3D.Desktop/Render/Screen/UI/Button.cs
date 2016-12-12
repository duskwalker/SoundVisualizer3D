using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoundVisualizer3D.Desktop.Render.Screen.UI
{
    sealed class Button
        : ClickableBase
    {
        #region Fields

        private readonly string _textureName;
        private Texture2D _texture;
        private SpriteBatch _spriteBatch;

        #endregion

        public Button(Game game, string textureName, Rectangle rectangle, ScreenAllocation allocation, Action action)
            : base(game, rectangle, allocation, action)
        {
            _textureName = textureName;
        }

        #region DrawableGameComponent Implementations

        public override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _texture = Game.Content.Load<Texture2D>(_textureName);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            HandleInput();

            if (IsClicked)
            {
                HandleAction();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Color color = IsTouching ? IsClicked ? Color.LightYellow : Color.AntiqueWhite : Color.White;

            _spriteBatch.Begin();
            _spriteBatch.Draw(_texture, Rectangle, color);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}