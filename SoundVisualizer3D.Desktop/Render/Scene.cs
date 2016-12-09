using SoundVisualizer3D.Desktop.Render.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace SoundVisualizer3D.Desktop.Render
{
    sealed class Scene
        : IScene
    {
        #region Fields

        private List<SceneObject> _objects = new List<SceneObject>();

        private GraphicsDevice _graphicsDevice;
        private ContentManager _contentManager;

        private SceneObject this[SceneObject obj]
        {
            get
            {
                if (_objects.Contains(obj))
                {
                    return _objects[_objects.IndexOf(obj)];
                }

                return null;
            }
        }

        #endregion

        #region Properties

        public ICamera Camera { get; private set; }

        #endregion

        public Scene(Game game)
        {
            _graphicsDevice = game.GraphicsDevice;
            _contentManager = game.Content;

            Camera = new Camera(game);
        }

        public void AddObject(SceneObject obj)
        {
            if (!_objects.Contains(obj))
            {
                _objects.Add(obj);

                this[obj].GraphicsDevice = _graphicsDevice;
                this[obj].Content = _contentManager;
            }
        }

        public void RemoveObject(SceneObject obj)
        {
            _objects.Remove(obj);
        }

        #region IScene Implementations

        public void LoadContent()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].LoadContent();
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Initialize();
            }
        }

        public void Render()
        {
            for (int i = 0; i < _objects.Count; i++)
                _objects[i].Change();

            for (int i = 0; i < _objects.Count; i++)
            {
                SceneObject obj = _objects[i];

                if (obj.Visible)
                    obj.Render(Camera);
            }
        }

        public void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            for (int i = 0; i < _objects.Count; i++)
            {
                _objects[i].Update();
            }
        }

        #endregion
    }
}