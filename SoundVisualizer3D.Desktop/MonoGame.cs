using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SoundVisualizer3D.MonoGame.Render;
using SoundVisualizer3D.MonoGame.Render.Objects.Cameras;
using SoundVisualizer3D.MonoGame.Render.Objects.Visualizations;
using SoundVisualizer3D.MonoGame.Render.Screen;
using SoundVisualizer3D.MonoGame.Render.Screen.UI;

namespace SoundVisualizer3D.MonoGame
{
    sealed class MonoGame
        : Game
    {
        #region Fields
        
        private GraphicsDeviceManager _graphics;

        #endregion

        public MonoGame(SoundSource soundSource)
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1024,
                PreferredBackBufferHeight = 768
            };

            IsMouseVisible = true;

            Components.Add(new BlocksVisualization(this));
            Components.Add(new Camera(this, false));
            Components.Add(new Arrow(this, false));
            Components.Add(new Hud(this));
            
            Components.Add(new Button(this, @"Buttons\play",
                new Rectangle(120, 20, 90, 90), ScreenAllocation.TopRight,
                    () =>
                    {
                        string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Audio\Kalimba.mp3");
                        if (File.Exists(fileName))
                        {
                            soundSource.Play(fileName, 50);
                        }

                    }));

            Services.AddService(soundSource);

            Content.RootDirectory = "Content";
        }

        #region Game Implementations

        protected override void Initialize()
        {
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.Hold;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            { 
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (IsActive)
            {
                _graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            }

            base.Draw(gameTime);
        }

        #endregion
    }
}