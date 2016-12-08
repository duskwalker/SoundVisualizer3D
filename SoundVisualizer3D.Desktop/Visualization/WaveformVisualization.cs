using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace SoundVisualizer3D.Desktop.Visualization
{
    sealed class WaveformVisualization
        : DrawableGameComponent
    {
        #region Fields

        private const float Scale = 300.0f;
        private const float Min = -1.0f;
        private const float Max = 1.0f;

        private VertexPositionColor[] _vertices1;
        private VertexPositionColor[] _vertices2;
        private BasicEffect _basicEffect;
        private SoundSource _soundSource;

        #endregion

        public WaveformVisualization(Game game, SoundSource soundSource)
            : base(game)
        {
            _soundSource = soundSource;
        }

        #region Overrides

        public override void Initialize()
        {
            base.Initialize();

            _basicEffect = new BasicEffect(GraphicsDevice);
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.World = Matrix.Identity;
            
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Audio\Kalimba.mp3");
            if(File.Exists(fileName))
            { 
                _soundSource.Play(fileName);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var height = Game.Window.ClientBounds.Height;
            var width = Game.Window.ClientBounds.Width;

            int pointSamples = _soundSource.FrequenciesValues.Length;
            if (_vertices1 == null || _vertices2 == null)
            {
                _vertices1 = new VertexPositionColor[pointSamples];
                _vertices2 = new VertexPositionColor[pointSamples];
            }

            float centerHeight = height / 2.0f;
            float sampleWidth = width / (float)pointSamples;

            for (int i = 0; i < pointSamples; i++)
            {
                float currentX = sampleWidth * i;

                float fft = MathHelper.Clamp(_soundSource.FrequenciesValues[i], Min, Max) * Scale;

                _vertices1[i].Position = new Vector3(currentX, centerHeight + fft, 0);
                _vertices1[i].Color = Color.Blue;

                float sfft = MathHelper.Clamp(_soundSource.FrequenciesValues[i], Min, Max) * Scale;

                _vertices2[i].Position = new Vector3(currentX, centerHeight + sfft, 0);
                _vertices2[i].Color = Color.Green;
            }

            var View = Matrix.Identity;
            var Projection = Matrix.CreateOrthographicOffCenter(0, width, height, 0, -1.0f, 1.0f);

            _basicEffect.View = View;
            _basicEffect.Projection = Projection;

            foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices1, 0, _vertices1.Length - 1, VertexPositionColor.VertexDeclaration);
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices2, 0, _vertices2.Length - 1, VertexPositionColor.VertexDeclaration);
            }
        }

        #endregion
    }
}