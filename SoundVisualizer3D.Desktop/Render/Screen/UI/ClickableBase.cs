using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Linq;

namespace SoundVisualizer3D.Desktop.Render.Screen.UI
{
    abstract class ClickableBase
        : ScreenObject
    {
        #region Fields

        private bool _isTouching;
        private Rectangle _rectangle;
        private ScreenAllocation _allocation;
        private Action _action;

        #endregion

        #region Properties

        protected bool IsTouching { get { return _isTouching; } }
        protected bool IsClicked { get { return IsTapedOrClicked(); } }
        protected Rectangle Rectangle { get { return _rectangle; } }

        #endregion

        public ClickableBase(Game game, Rectangle rectangle, ScreenAllocation allocation = ScreenAllocation.TopLeft, Action action = null)
            : base(game)
        {
            _rectangle = rectangle;
            _allocation = allocation;
            _action = action;
        }

        #region DrawableGameComponent Implementations

        public override void Initialize()
        {
            Vector2 position = GetScreenPosition(new Vector2(_rectangle.X, _rectangle.Y), _allocation);
            
            int x = (int) position.X;
            int y = (int) position.Y;

            if(position.X + _rectangle.Width > GraphicsDevice.Viewport.Width)
            {
                x = GraphicsDevice.Viewport.Width - _rectangle.Width;
            }

            if (position.Y + _rectangle.Height > GraphicsDevice.Viewport.Height)
            {
                y = GraphicsDevice.Viewport.Height - _rectangle.Height;
            }

            _rectangle = new Rectangle(x, y, _rectangle.Width, _rectangle.Height);

            base.Initialize();
        }

        #endregion

        protected void HandleInput()
        {
            _isTouching = false;

            MouseState mousteState = Mouse.GetState();
            TouchLocation touchState = TouchPanel.GetState().FirstOrDefault();

            if (touchState != null && touchState.State != TouchLocationState.Invalid)
            {
                Vector2 position = touchState.Position;
                Rectangle rectangle = new Rectangle((int)touchState.Position.X - 5, (int)touchState.Position.Y - 5, 10, 10);

                if (Rectangle.Intersects(rectangle))
                { 
                    _isTouching = true;
                }
            }
            else if(Rectangle.Contains(mousteState.X, mousteState.Y))
            {
                _isTouching = true;
            }
        }

        protected void HandleAction()
        {
            _action();
        }

        #region Private Methods

        private bool IsTapedOrClicked()
        {
            MouseState mouseState = Mouse.GetState();
            TouchLocation touchState = TouchPanel.GetState().FirstOrDefault();

            return touchState.State == TouchLocationState.Pressed || mouseState.LeftButton == ButtonState.Pressed;
        }

        #endregion
    }
}