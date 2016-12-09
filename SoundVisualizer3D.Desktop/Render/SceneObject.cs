using SoundVisualizer3D.Desktop.Render.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SoundVisualizer3D.Desktop.Render
{
    abstract class SceneObject
    {
        #region Fields

        private bool _changed;
        private bool _visible;

        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;

        private Vector3 _position;
        private Vector3 _angle;
        private bool _solid;

        #endregion

        #region Properties

        public ContentManager Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _graphicsDevice; }
            set { _graphicsDevice = value; }
        }

        public virtual bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public virtual bool Solid
        {
            get { return _solid; }
            set { _solid = value; }
        }

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; _changed = true; }
        }

        public Vector3 Angle
        {
            get { return _angle; }
            set { _angle = value; _changed = true; }
        }

        #endregion

        protected SceneObject() { }

        protected SceneObject(Vector3 position, Vector3 angle)
        {
            _position = position;
            _angle = angle;

            _changed = true;
        }

        public void Change()
        {
            if (_changed)
            {
                OnChange();

                _changed = false;
            }
        }

        #region Virtuals

        public virtual void LoadContent() { }

        public virtual void Initialize() { }

        public virtual void Update() { }

        public virtual void Render(ICamera camera) { }

        public virtual void OnChange() { }

        #endregion
    }
}