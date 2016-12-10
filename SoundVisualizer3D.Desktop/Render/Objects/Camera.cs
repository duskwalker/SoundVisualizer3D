﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SoundVisualizer3D.Desktop.Render.Objects
{
    sealed class Camera
            : SceneObject
                , ICamera
    {
        #region Fields

        // Set the position and rotation variables.
        private static Vector3 _position = new Vector3(0, 0, 0);
        private Vector3 _cameraPosition = _position;

        private float _yaw;

        // Set the direction the camera points without rotation.
        private Vector3 _cameraReference = new Vector3(0, 0, 1);

        // Set rates in world units per 1/60th second (the default fixed-step interval).
        private float _rotationSpeed = 2f / 60f; //private float _rotationSpeed = 1f / 60f;
        private float _forwardSpeed = 50f / 60f;

        // Set field of view of the camera in radians (pi/4 is 45 degrees).
        private const float _viewAngle = MathHelper.PiOver4;

        // Set distance from the camera of the near and far clipping planes.
        private const float _nearClip = 1.0f;
        private const float _farClip = 2000.0f;

        private readonly Game _game;

        #endregion

        #region Properties

        public Matrix Projection { get; private set; }
        public Matrix View { get; private set; }
        public Matrix World { get; private set; }

        public override bool Solid
        {
            get { return false; }
        }
        public override bool Visible
        {
            get { return false; }
        }

        #endregion

        public Camera(Game game)
        {
            _game = game;
        }

        public void Update(GameTime gameTime)
        {
            UpdatePosition();
            UpdateCamera();

            if (_game.IsActive)
            {
                Mouse.SetPosition(_game.Window.ClientBounds.X / 2, _game.Window.ClientBounds.Y / 2);
            }
        }

        #region Private Methods

        private void UpdatePosition()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                // Rotate left.
                _yaw += _rotationSpeed;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                // Rotate right.
                _yaw -= _rotationSpeed;
            }

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
                Vector3 vector = new Vector3(0, -_forwardSpeed, 0);
                vector = Vector3.Transform(vector, forwardMovement);

                _position.Y += vector.Y;
            }
            if (keyboardState.IsKeyDown(Keys.E))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
                Vector3 vector = new Vector3(0, _forwardSpeed, 0);
                vector = Vector3.Transform(vector, forwardMovement);

                _position.Y += vector.Y;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
                Vector3 vector = new Vector3(0, 0, _forwardSpeed);
                vector = Vector3.Transform(vector, forwardMovement);

                _position.Z += vector.Z;
                _position.X += vector.X;
            }
            if (keyboardState.IsKeyDown(Keys.W))
            {
                Matrix forwardMovement = Matrix.CreateRotationY(_yaw);
                Vector3 vector = new Vector3(0, 0, -_forwardSpeed);
                vector = Vector3.Transform(vector, forwardMovement);

                _position.Z += vector.Z;
                _position.X += vector.X;
            }

            Position = _position;
        }

        private void UpdateCamera()
        {
            // Calculate the camera's current position.
            Matrix rotationMatrix = Matrix.CreateRotationY(_yaw);

            // Create a vector pointing the direction the camera is facing.
            Vector3 transformedReference = Vector3.Transform(_cameraReference, rotationMatrix);

            // Calculate the position the camera is looking at.
            Vector3 cameraLookat = _cameraPosition + transformedReference;

            // Set up the view matrix and projection matrix.
            View = Matrix.CreateLookAt(_cameraPosition, cameraLookat, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(_viewAngle, _game.GraphicsDevice.Viewport.AspectRatio, _nearClip, _farClip);

            // Set up the world matrix.
            World = Matrix.CreateWorld(Position, Vector3.Forward, Vector3.Up);
        }

        #endregion
    }
}