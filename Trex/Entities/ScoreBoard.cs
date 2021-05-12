using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrexRunner.Entities
{
    public class ScoreBoard : IGameEntity
    {
        private const int TEXTURE_COORDS_NUMBER_X = 655;
        private const int TEXTURE_COORDS_NUMBER_Y = 0;
        private const int TEXTURE_COORDS_NUMBER_WIDTH = 10;
        private const int TEXTURE_COORDS_NUMBER_HEIGHT = 13;

        private const byte NUMBER_DIGITS_TO_DRAW = 5;

        private const int TEXTURE_COORDS_HI_X = 755;
        private const int TEXTURE_COORDS_HI_Y = 0;
        private const int TEXTURE_COORDS_HI_WIDTH = 20;
        private const int TEXTURE_COORDS_HI_HEIGHT = 13;

        private const int HI_TEXT_MARGIN = 30;

        private const int SCORE_MARGIN = 70;
        private const float SCORE_MULTIPLIER = 0.05f;

        private const float FLASH_ANIMATION_FRAME_LENGTH = 1/5f;
        private const int FLASH_ANIMTION_FLASH_COUNT = 4;

        private Texture2D _texture;
        private SoundEffect _scoreSfx;

        public double Score { get; set; }
        public int DisplayScore => (int) Math.Floor(Score);
        public int HighScore { get; set; }
        public bool HasHighScore => HighScore > 0;

        public int DrawOrder => 100;
        public Vector2 Position { get; set; }
        private Trex _trex;

        private bool _isPlayingFlashAnimation;
        private float _flashAnimationTime;

        public ScoreBoard(Texture2D texture, Vector2 position, Trex trex, SoundEffect scoreSfx)
        {
            _texture = texture;
            _trex = trex;
            Position = position;
            _scoreSfx = scoreSfx;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (HasHighScore)
            {
                spriteBatch.Draw(_texture, new Vector2(Position.X - HI_TEXT_MARGIN, Position.Y), new Rectangle(TEXTURE_COORDS_HI_X, TEXTURE_COORDS_HI_Y, TEXTURE_COORDS_HI_WIDTH, TEXTURE_COORDS_HI_HEIGHT), Color.White);
                DrawScore(spriteBatch, HighScore, Position.X);
            }
            if (!_isPlayingFlashAnimation || (int)(_flashAnimationTime / FLASH_ANIMATION_FRAME_LENGTH) % 2 != 0)
                DrawScore(spriteBatch, DisplayScore, Position.X + SCORE_MARGIN);
        }

        private void DrawScore(SpriteBatch spriteBatch, int score, float startPosX)
        {
            int[] scoreDigits = SplitDigits(DisplayScore);

            float posX = startPosX;

            foreach (int digit in scoreDigits)
            {
                Rectangle textureCoords = GetDigitTextureBounds(digit);
                Vector2 screenPos = new Vector2(posX, Position.Y);
                spriteBatch.Draw(_texture, screenPos, textureCoords, Color.White);

                posX += TEXTURE_COORDS_NUMBER_WIDTH;
            }
        }

        private int[] SplitDigits(int input)
        {
            string inputStr = input.ToString().PadLeft(NUMBER_DIGITS_TO_DRAW, '0');

            int[] result = new int[inputStr.Length];
            for(int i=0; i < result.Length; i++)
            {
                result[i] = (int) char.GetNumericValue(inputStr[i]);
            }

            return result;
        }

        public void Update(GameTime gameTime)
        {
            int oldScore = DisplayScore;
            Score += _trex.Speed * SCORE_MULTIPLIER * gameTime.ElapsedGameTime.TotalSeconds;

            // only plays when the score crosses the 100 * n boundary
            // DisplayScore % 100 isn't cleaner than this solution it will play at the beginning
            if(!_isPlayingFlashAnimation && DisplayScore / 100 != oldScore / 100)
            {
                _isPlayingFlashAnimation = true;
                _flashAnimationTime = 0;
                _scoreSfx.Play(0.3f, 0, 0);
            }

            if (_isPlayingFlashAnimation)
            {
                _flashAnimationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if(_flashAnimationTime >= FLASH_ANIMATION_FRAME_LENGTH * FLASH_ANIMTION_FLASH_COUNT * 2)
                {
                    _isPlayingFlashAnimation = false;
                }
            }
        }

        private Rectangle GetDigitTextureBounds(int digit)
        {
            if (digit < 0 || digit > 9)
                throw new ArgumentOutOfRangeException("digit", "The value of the digit must be between 0 and 9");

            int posX = TEXTURE_COORDS_NUMBER_X + digit * TEXTURE_COORDS_NUMBER_WIDTH;
            int posY = TEXTURE_COORDS_NUMBER_Y;

            return new Rectangle(posX, posY, TEXTURE_COORDS_NUMBER_WIDTH, TEXTURE_COORDS_NUMBER_HEIGHT);
        }
    }
}
