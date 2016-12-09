using Microsoft.Xna.Framework;

namespace SoundVisualizer3D.Desktop.Render
{
    abstract class ScreenObject
        : SceneObject
    {
        #region Fields

        private Vector2 _screenAxisOffset;
        private Vector2 _screenAxisPosition;
        private ScreenAllocation _screenAxisAllocation;

        #endregion

        #region Properties

        protected Vector2 ScreenAxisOffset
        {
            get { return _screenAxisOffset; }
            set { _screenAxisOffset = value; }
        }

        protected Vector2 ScreenAxisPosition
        {
            get { return _screenAxisPosition; }
            set { _screenAxisPosition = value; }
        }

        protected ScreenAllocation ScreenAxisAllocation
        {
            get { return _screenAxisAllocation; }
            set { _screenAxisAllocation = value; }
        }

        protected Vector2 GetScreenPosition()
        {
            return GetScreenPosition(_screenAxisOffset, _screenAxisAllocation);
        }

        #endregion

        protected Vector2 GetScreenPosition(Vector2 offset, ScreenAllocation allocation)
        {
            Vector2 result = Vector2.Zero;

            float Width = GraphicsDevice.Viewport.Width;
            float HalfWidth = GraphicsDevice.Viewport.Width / 2;
            float Height = GraphicsDevice.Viewport.Height;
            float HalfHeight = GraphicsDevice.Viewport.Height / 2;

            switch (allocation)
            {
                case ScreenAllocation.Top:
                    result = new Vector2(HalfWidth, offset.Y);
                    break;
                case ScreenAllocation.TopRight:
                    result = new Vector2(Width - offset.X, offset.Y);
                    break;
                case ScreenAllocation.Right:
                    result = new Vector2(Width - offset.X, HalfHeight);
                    break;
                case ScreenAllocation.BottomRight:
                    result = new Vector2(Width - offset.X, Height - offset.Y);
                    break;
                case ScreenAllocation.Bottom:
                    result = new Vector2(HalfWidth, Height - offset.Y);
                    break;
                case ScreenAllocation.BottomLeft:
                    result = new Vector2(offset.X, Height - offset.Y);
                    break;
                case ScreenAllocation.Left:
                    result = new Vector2(offset.X, HalfHeight);
                    break;
                case ScreenAllocation.TopLeft:
                    result = new Vector2(offset.X, offset.Y);
                    break;
            }

            return result;
        }
    }
}