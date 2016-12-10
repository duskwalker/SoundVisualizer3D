using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundVisualizer3D.Desktop.Render.Objects.Cameras;
using System.Collections.Generic;
using SoundVisualizer3D.Desktop.Utils;

namespace SoundVisualizer3D.Desktop.Render.Objects.Visualizations
{
    sealed class BlocksVisualization
        : SceneObject
    {
        #region Fields

        private VertexPositionColor[] _vertices;
        private BasicEffect _effect;
        private ICamera _camera;

        #endregion

        public BlocksVisualization(Game game)
            : base(game) { }

        #region DrawableGameComponent Implementations

        public override void Initialize()
        {
            _camera = Game.Services.GetService<ICamera>();

            _effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            var vertices = new List<VertexPositionColor>();
            for (int i = 0; i < 5; i++)
            {
                var origin = Vector3.Add(Vector3.Zero, new Vector3(i * 5, 0, 0));
                vertices.AddRange(CreateCube(origin));
            }
            _vertices = vertices.ToArray();

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            _effect.Projection = _camera.Projection;
            _effect.View = _camera.View;
            _effect.World = _camera.World;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length / 3);
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Private Methods

        private VertexPositionColor[] CreateCube(Vector3 origin)
        {
            Color frontSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color backSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color leftSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color rightSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color topSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color bottomSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);

            return new VertexPositionColor[36]
            {
                // Front Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)), frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)), frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)), frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)), frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)), frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)), frontSurfaceColor),

                // Back Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)), backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)), backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)), backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)), backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)), backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)), backSurfaceColor), 

                // Left Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)), leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)), leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)), leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)), leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)), leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)), leftSurfaceColor),

                // Right Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)), rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)), rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)), rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)), rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)), rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)), rightSurfaceColor),

                // Top Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)), topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)), topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)), topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)), topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)), topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)), topSurfaceColor),

                // Bottom Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)), bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)), bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)), bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)), bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)), bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)), bottomSurfaceColor),
            };
        }

        #endregion
    }
}