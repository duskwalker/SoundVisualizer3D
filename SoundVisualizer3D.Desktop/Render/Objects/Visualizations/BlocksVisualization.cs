using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundVisualizer3D.Desktop.Render.Objects.Cameras;
using System.Collections.Generic;
using SoundVisualizer3D.Desktop.Utils;
using System.Linq;

namespace SoundVisualizer3D.Desktop.Render.Objects.Visualizations
{
    sealed class BlocksVisualization
        : SceneObject
    {
        #region Fields

        //private BasicEffect _effect;
        private ICamera _camera;
        private BlockModel[] _models;

        #endregion

        public BlocksVisualization(Game game)
            : base(game) { }

        #region DrawableGameComponent Implementations

        public override void Initialize()
        {
            _camera = Game.Services.GetService<ICamera>();

            /*_effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };*/

            var models = new List<BlockModel>();
            for (int i = 0; i < 5; i++)
            {
                Vector3 origin = Vector3.Add(Vector3.Zero, new Vector3(i * 5, 0, 0));
                BlockModel model = new BlockModel(Game, origin);

                model.Initialize();
                models.Add(model);
            }
            _models = models.ToArray();

            base.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            //_effect.Projection = _camera.Projection;
            //_effect.View = _camera.View;
            //_effect.World = _camera.World;

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            rasterizerState.FillMode = FillMode.Solid;
            GraphicsDevice.RasterizerState = rasterizerState;

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (BlockModel model in _models)
            {
                model.Effect.Projection = _camera.Projection;
                model.Effect.View = _camera.View;
                model.Effect.World = _camera.World;

                model.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        #endregion
    }

    sealed class BlockModel
    {
        #region Fields

        private VertexPositionColor[] _vertices;
        private Vector3 _origin;

        private BasicEffect _effect;
        private Game _game;
        private ICamera _camera;

        #endregion

        #region Properties

        public BasicEffect Effect { get { return _effect; } }

        #endregion

        public BlockModel(Game game, Vector3 origin)
        {
            _game = game;
            _origin = origin;
        }

        public void Initialize()
        {
            _camera = _game.Services.GetService<ICamera>();
            _vertices = CreateCube(_origin);

            _effect = new BasicEffect(_game.GraphicsDevice)
            {
                VertexColorEnabled = true
            };
        }

        public void Draw(GameTime gameTime)
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length / 3);
            }
        }

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