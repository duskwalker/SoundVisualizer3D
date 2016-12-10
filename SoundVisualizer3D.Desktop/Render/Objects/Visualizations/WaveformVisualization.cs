﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace SoundVisualizer3D.Desktop.Render.Objects.Visualizations
{
    sealed class WaveformVisualization
        : SceneObject
    {
        #region Fields

        private const float Scale = 25.0f;
        private const float Min = -10.0f;
        private const float Max = 10.0f;

        private VertexPositionColor[] _vertices1;
        private VertexPositionColor[] _vertices2;

        private BasicEffect _effect;
        private SoundSource _soundSource;

        #endregion

        public WaveformVisualization(SoundSource soundSource)
        {
            _soundSource = soundSource;
        }

        #region Overrides

        public override void Initialize()
        {
            base.Initialize();

            _effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };
            
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Audio\01. Alan Walker - Faded.mp3");
            if(File.Exists(fileName))
            { 
                _soundSource.Play(fileName, 25);
            }
        }

        public override void Render(ICamera camera)
        {
            var height = GraphicsDevice.DisplayMode.Height;
            var width = GraphicsDevice.DisplayMode.Width;

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
                _vertices1[i].Color = Color.WhiteSmoke;

                //float sfft = MathHelper.Clamp(_soundSource.FrequenciesValues[i], Min, Max) * Scale;

                //_vertices2[i].Position = new Vector3(currentX, centerHeight + sfft, 0);
                //_vertices2[i].Color = Color.Green;
            }

            _effect.Projection = camera.Projection;
            _effect.View = camera.View;
            _effect.World = camera.World;

            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices1, 0, _vertices1.Length - 1, VertexPositionColor.VertexDeclaration);
                //GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, _vertices2, 0, _vertices2.Length - 1, VertexPositionColor.VertexDeclaration);
            }
        }

        #endregion
    }
}