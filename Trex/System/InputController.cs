using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TrexRunner.Entities;

namespace TrexRunner.System
{
    class InputController
    {
        private Trex _trex;

        private KeyboardState _previousKeyboardState;

        public InputController(Trex trex)
        {
            _trex = trex;
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                if (_trex.State != TrexState.Jumping)
                    _trex.BeginJump();
            }
            // if we check _previousKeyboardState.IsKeyDown
            // Cancel Jump will only be called on the frame the keyboard is released

            else if (!keyboardState.IsKeyDown(Keys.Up))
            {
                if (_trex.State == TrexState.Jumping)
                    _trex.CancelJump();
            }

            // keyboardState is a struct, so it won't be referencing the same instance
            _previousKeyboardState = keyboardState;
        }
    }
}
