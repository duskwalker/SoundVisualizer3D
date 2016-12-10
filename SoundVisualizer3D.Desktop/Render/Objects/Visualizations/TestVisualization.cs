using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoundVisualizer3D.Desktop.Render.Objects.Visualizations
{
    sealed class TestVisualization
        : DrawableGameComponent
    {
        #region Fields

        private SpriteBatch _spriteBatch;

        //Camera
        private Vector3 _camTarget;
        private Vector3 _camPosition;
        private Matrix _projectionMatrix;
        private Matrix _viewMatrix;
        private Matrix _worldMatrix;

        //BasicEffect for rendering
        private BasicEffect _effect;

        //Geometric info
        private VertexPositionColor[] _triangleVertices;
        private VertexBuffer _vertexBuffer;

        //Orbit
        private bool _orbit = false;
        private ICamera _camera;

        #endregion

        public TestVisualization(Game game)
            : base(game) { }

        #region DrawableGameComponent Implementations

        public override void Initialize()
        {
            _camera = Game.Services.GetService<ICamera>();

            //Setup Camera
            _camTarget = new Vector3(0f, 0f, 0f);
            _camPosition = new Vector3(0f, 0f, -100f);
            _projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), Game.GraphicsDevice.DisplayMode.AspectRatio, 1f, 1000f);
            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, new Vector3(0f, 1f, 0f)); // Y up
            _worldMatrix = Matrix.CreateWorld(_camTarget, Vector3.Forward, Vector3.Up);

            //BasicEffect
            _effect = new BasicEffect(Game.GraphicsDevice);
            _effect.Alpha = 1f;

            // Want to see the colors of the vertices, this needs to be on
            _effect.VertexColorEnabled = true;

            //Lighting requires normal information which VertexPositionColor does not have
            //If you want to use lighting and VPC you need to create a custom def
            _effect.LightingEnabled = false;

            //Geometry  - a simple triangle about the origin
            _triangleVertices = new VertexPositionColor[3];
            _triangleVertices[0] = new VertexPositionColor(new Vector3(0, 20, 0), Color.Red);
            _triangleVertices[1] = new VertexPositionColor(new Vector3(-20, -20, 0), Color.Green);
            _triangleVertices[2] = new VertexPositionColor(new Vector3(20, -20, 0), Color.Blue);

            //Vert buffer
            _vertexBuffer = new VertexBuffer(Game.GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            _vertexBuffer.SetData<VertexPositionColor>(_triangleVertices);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _camPosition.X -= 1f;
                _camTarget.X -= 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _camPosition.X += 1f;
                _camTarget.X += 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _camPosition.Y -= 1f;
                _camTarget.Y -= 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _camPosition.Y += 1f;
                _camTarget.Y += 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                _camPosition.Z += 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                _camPosition.Z -= 1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _orbit = !_orbit;
            }

            if (_orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(1f));
                _camPosition = Vector3.Transform(_camPosition, rotationMatrix);
            }

            _viewMatrix = Matrix.CreateLookAt(_camPosition, _camTarget, Vector3.Up);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _effect.Projection = _camera.Projection;
            _effect.View = _camera.View;
            _effect.World = _camera.World;

            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetVertexBuffer(_vertexBuffer);

            //Turn off culling so we see both sides of our rendered triangle
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}