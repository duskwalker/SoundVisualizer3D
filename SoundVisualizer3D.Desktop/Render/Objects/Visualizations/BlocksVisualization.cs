using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoundVisualizer3D.Desktop.Render.Objects.Cameras;
using System.Collections.Generic;
using SoundVisualizer3D.Desktop.Utils;
using System.IO;
using System;

namespace SoundVisualizer3D.Desktop.Render.Objects.Visualizations
{
    sealed class BlocksVisualization
        : SceneObject
    {
        #region Fields

        private ICamera _camera;
        private ISet<BlockModel> _models;
        private SoundSource _soundSource;

        #endregion

        public BlocksVisualization(Game game)
            : base(game) { }

        #region DrawableGameComponent Implementations

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Initialize()
        {
            _camera = Game.Services.GetService<ICamera>();
            _soundSource = Game.Services.GetService<SoundSource>();

            var models = new HashSet<BlockModel>();
            for (int ident = 0; ident < _soundSource.CurrentFrequencesBandValues.Length; ident++)
            {
                Vector3 origin = Vector3.Add(Vector3.Zero, new Vector3(ident * 2.0f, 0, 0));
                BlockModel model = new BlockModel(Game, ident, origin);

                models.Add(model);
            }
            _models = models;

            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Audio\Kalimba.mp3");
            if (File.Exists(fileName))
            {
                _soundSource.Play(fileName, 50);
            }

            base.Initialize();

            _camera.SetPosition(new Vector3(0.0f, 0.0f, 5.0f));
        }

        public override void Draw(GameTime gameTime)
        {
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
                model.Effect.World = model.GetWorld();

                model.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        #endregion
    }

    sealed class BlockModel
        : ScreenObject
    {
        #region Consts

        private const float Scale = 10.0f;
        private const float Min = -1.0f;
        private const float Max = 1.0f;

        #endregion

        #region Fields

        private VertexPositionColor[] _vertices;
        private Vector3 _origin;

        private BasicEffect _effect;
        private Game _game;
        private ICamera _camera;
        private SoundSource _soundSource;
        private int _ident;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        #endregion

        #region Properties

        public BasicEffect Effect { get { return _effect; } }
        public Vector3 Position { get { return _origin; } }

        #endregion

        public BlockModel(Game game, int ident, Vector3 origin)
            : base(game)
        {
            _game = game;
            _ident = ident;
            _origin = origin;

            _camera = _game.Services.GetService<ICamera>();
            _soundSource = _game.Services.GetService<SoundSource>();

            _vertices = CreateCube(_origin);

            _effect = new BasicEffect(_game.GraphicsDevice)
            {
                VertexColorEnabled = true
            };
            
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length / 3);
            }
        }

        public Matrix GetWorld()
        {
            float fft = MathHelper.Clamp(_soundSource.CurrentFrequencesBandValues[_ident], Min, Max) * Scale;

            return Matrix.CreateScale(1.0f, fft, 1.0f) * _camera.World;
        }

        #region Private Methods

        private VertexPositionColor[] CreateCube(Vector3 origin)
        {
            Vector3 scale = new Vector3(0.01f, 1.0f, 0.04f);

            Color frontSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color backSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color leftSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color rightSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color topSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);
            Color bottomSurfaceColor = ColorUtils.GenerateRandomColor(Color.WhiteSmoke);

            return new VertexPositionColor[36]
            {
                // Front Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)) * scale, frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)) * scale, frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)) * scale, frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)) * scale, frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)) * scale, frontSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)) * scale, frontSurfaceColor),

                // Back Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)) * scale, backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)) * scale, backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)) * scale, backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)) * scale, backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)) * scale, backSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)) * scale, backSurfaceColor), 

                // Left Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)) * scale, leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)) * scale, leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)) * scale, leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)) * scale, leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)) * scale, leftSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)) * scale, leftSurfaceColor),

                // Right Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)) * scale, rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)) * scale, rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)) * scale, rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)) * scale, rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)) * scale, rightSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)) * scale, rightSurfaceColor),

                // Top Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, 1.0f)) * scale, topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)) * scale, topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)) * scale, topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, 1.0f)) * scale, topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, 1.0f, -1.0f)) * scale, topSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, 1.0f, -1.0f)) * scale, topSurfaceColor),

                // Bottom Surface
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, -1.0f)) * scale, bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)) * scale, bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)) * scale, bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, -1.0f)) * scale, bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(-1.0f, -1.0f, 1.0f)) * scale, bottomSurfaceColor),
                new VertexPositionColor(Vector3.Add(origin, new Vector3(1.0f, -1.0f, 1.0f)) * scale, bottomSurfaceColor),
            };
        }

        #endregion
    }
}