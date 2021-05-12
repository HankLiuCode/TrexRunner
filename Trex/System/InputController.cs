using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrexRunner.Entities;

namespace TrexRunner.System
{
    class InputController
    {
        private Trex _trex;

        private KeyboardState _previousKeyboardState;
        private bool _isBlocked;

        public InputController(Trex trex)
        {
            _trex = trex;
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (!_isBlocked)
            {
                bool isJumpKeyPressed = keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.Space);
                bool wasJumpKeyPressed = _previousKeyboardState.IsKeyDown(Keys.Up) || _previousKeyboardState.IsKeyDown(Keys.Space);

                if (isJumpKeyPressed && !wasJumpKeyPressed)
                {
                    if (_trex.State != TrexState.Jumping)
                        _trex.BeginJump();
                }
                // if we check _previousKeyboardState.IsKeyDown
                // Cancel Jump will only be called on the frame the keyboard is released
                else if (!isJumpKeyPressed && _trex.State == TrexState.Jumping)
                {
                    _trex.CancelJump();
                }

                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    if (_trex.State == TrexState.Jumping || _trex.State == TrexState.Falling)
                        _trex.Drop();
                    else
                        _trex.Duck();
                }

                else if (_trex.State == TrexState.Ducking && !keyboardState.IsKeyDown(Keys.Down))
                {
                    _trex.Getup();
                }
            }

            // keyboardState is a struct, so it won't be referencing the same instance
            _previousKeyboardState = keyboardState;
            _isBlocked = false;
        }

        public void BlockInputTemporarily()
        {
            _isBlocked = true;
        }
    }
}
