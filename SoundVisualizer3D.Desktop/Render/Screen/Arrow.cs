﻿using SoundVisualizer3D.Desktop.Render.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoundVisualizer3D.Desktop.Render.Screen
{
    sealed class Arrow
        : ScreenObject
    {
        #region Fields

        private BasicEffect _effect;

        private VertexPositionColor[] _axisArrow;
        private VertexPositionColor[] _axisMain;

        private static bool _screenAxis;
        private static bool _mainAxis;

        private float _resolution;
        private int _scale;

        private Color _xColor;
        private Color _yColor;
        private Color _zColor;

        #endregion

        public Arrow()
        {
            Visible = true;
        }

        public override void Initialize()
        {
            _screenAxis = true;
            _mainAxis = true;

            _resolution = 1;
            _scale = 10;

            _xColor = Color.Red;
            _yColor = Color.Green;
            _zColor = Color.Blue;

            CreateAxis();
            CreateAxisArrow();

            ScreenAxisPosition = GetScreenPosition(new Vector2(100, 50), ScreenAllocation.BottomLeft);

            _effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };
        }

        public override void Update()
        {
            //CreateAxis();
            //CreateAxisArrow();
        }

        public override void Render(ICamera camera)
        {
            _effect.Projection = camera.Projection;
            _effect.View = camera.View;
            _effect.World = camera.World;

            if (_mainAxis)
            {
                DrawLineList(_axisMain);
            }

            if (_screenAxis)
            {
                Vector3 near = GraphicsDevice.Viewport.Unproject(new Vector3(ScreenAxisPosition, 0), camera.Projection, camera.View, Matrix.Identity);
                Vector3 far = GraphicsDevice.Viewport.Unproject(new Vector3(ScreenAxisPosition, 1), camera.Projection, camera.View, Matrix.Identity);

                Vector3 direction = far - near;
                direction.Normalize();

                Vector3 position = near + (direction * 20);

                _effect.World = Matrix.CreateTranslation(position);
                _effect.DiffuseColor = _xColor.ToVector3();

                DrawLineStrip(_axisArrow);

                _effect.DiffuseColor = _yColor.ToVector3();
                _effect.World = Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateTranslation(position);

                DrawLineStrip(_axisArrow);

                _effect.DiffuseColor = _zColor.ToVector3();
                _effect.World = Matrix.CreateRotationY(-MathHelper.PiOver2) * Matrix.CreateTranslation(position);

                DrawLineStrip(_axisArrow);

                _effect.DiffuseColor = Color.White.ToVector3();
                _effect.World = Matrix.Identity;
            }
        }

        #region Private Methods

        private void CreateAxisArrow()
        {
            _axisArrow = new VertexPositionColor[12];

            // Axis Line:  
            _axisArrow[0] = new VertexPositionColor(new Vector3(0, 0, 0), Color.White);
            _axisArrow[1] = new VertexPositionColor(new Vector3(1, 0, 0), Color.White);

            // Arrow on the tip of the Line:  
            _axisArrow[2] = new VertexPositionColor(new Vector3(0.8f, 0, 0.1f), Color.White);
            _axisArrow[3] = new VertexPositionColor(new Vector3(0.8f, 0.1f, 0), Color.White);
            _axisArrow[4] = new VertexPositionColor(new Vector3(1, 0, 0), Color.White);
            _axisArrow[5] = new VertexPositionColor(new Vector3(0.8f, -0.1f, 0), Color.White);
            _axisArrow[6] = new VertexPositionColor(new Vector3(0.8f, 0, -0.1f), Color.White);
            _axisArrow[7] = new VertexPositionColor(new Vector3(0.8f, 0.1f, 0), Color.White);
            _axisArrow[8] = new VertexPositionColor(new Vector3(0.8f, -0.1f, 0), Color.White);
            _axisArrow[9] = new VertexPositionColor(new Vector3(0.8f, 0, 0.1f), Color.White);
            _axisArrow[10] = new VertexPositionColor(new Vector3(0.8f, 0, -0.1f), Color.White);
            _axisArrow[11] = new VertexPositionColor(new Vector3(1, 0, 0), Color.White);
        }

        private void CreateAxis()
        {
            _axisMain = new VertexPositionColor[6];

            _axisMain[0] = new VertexPositionColor(new Vector3(-_scale * _resolution, 0, 0), _xColor);
            _axisMain[1] = new VertexPositionColor(new Vector3(_scale * _resolution, 0, 0), _xColor);
            _axisMain[2] = new VertexPositionColor(new Vector3(0, -_scale * _resolution, 0), _yColor);
            _axisMain[3] = new VertexPositionColor(new Vector3(0, _scale * _resolution, 0), _yColor);
            _axisMain[4] = new VertexPositionColor(new Vector3(0, 0, -_scale * _resolution), _zColor);
            _axisMain[5] = new VertexPositionColor(new Vector3(0, 0, _scale * _resolution), _zColor);
        }

        private void DrawLineList(VertexPositionColor[] vertices)
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length / 2);
            }
        }

        private void DrawLineStrip(VertexPositionColor[] vertices)
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, vertices.Length - 1);
            }
        }

        #endregion
    }
}