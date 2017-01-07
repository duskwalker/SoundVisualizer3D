using Microsoft.Xna.Framework;

namespace SoundVisualizer3D.MonoGame.Render
{
    abstract class SceneObject
        : DrawableGameComponent
    {
        #region Fields

        private Vector3 _position;
        private Vector3 _angle;

        #endregion

        #region Properties

        public Vector3 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        #endregion

        protected SceneObject(Game game)
            : base(game) { }

        protected SceneObject(Game game, Vector3 position, Vector3 angle)
            : base(game)
        {
            _position = position;
            _angle = angle;
        }
    }
}