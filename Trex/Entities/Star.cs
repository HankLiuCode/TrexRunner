using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    class Star : SkyObject
    {
        public override float Speed => _trex.Speed * 0.2f;
        private const int INITIAL_FRAME_TEXTURE_COORDS_X = 644;
        private const int INITIAL_FRAME_TEXTURE_COORDS_Y = 2;

        private const int SPRITE_WIDTH = 9;
        private const int SPRITE_HEIGHT = 9;

        private const float ANIMATION_FRAME_LENGTH = 1 / 3f;

        private SpriteAnimation _animation;

        public Star(Texture2D spriteSheet, Trex trex, Vector2 Position) : base(trex, Position)
        {
            _animation = SpriteAnimation.CreateSimpeAnimation(
                spriteSheet,
                new Point(INITIAL_FRAME_TEXTURE_COORDS_X, INITIAL_FRAME_TEXTURE_COORDS_Y),
                SPRITE_WIDTH,
                SPRITE_HEIGHT,
                new Point(0, SPRITE_HEIGHT),
                3,
                ANIMATION_FRAME_LENGTH
            );
            _animation.ShouldLoop = true;
            _animation.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(_trex.IsAlive)
                _animation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _animation.Draw(spriteBatch, Position);
        }
    }
}
