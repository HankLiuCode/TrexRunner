using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace TrexRunner.Graphics
{
    public class SpriteAnimation
    {
        private List<SpriteAnimationFrame> _frames = new List<SpriteAnimationFrame>();
        
        // indexer
        // allows us to access the frames in this class by using syntax 
        // spriteAnimation[2]
        public SpriteAnimationFrame this[int index]
        {
            get
            {
                return GetFrame(index);
            }
        }

        public SpriteAnimationFrame CurrentFrame
        {
            get
            {
                // takes a delegate as an argument (lambda expression)
                return _frames
                    .Where(f => f.TimeStamp <= PlaybackProgress)
                    .OrderBy(f => f.TimeStamp)
                    .LastOrDefault();
            }
        }

        public float Duration
        {
            get
            {
                if (!_frames.Any())
                    return 0;
                return _frames.Max(f => f.TimeStamp);
            }
        }

        public bool IsPlaying { get; private set; }
        public float PlaybackProgress { get; private set; }

        public bool ShouldLoop { get; set; } = true;


        public void AddFrame(Sprite sprite, float timeStamp)
        {
            SpriteAnimationFrame frame = new SpriteAnimationFrame(sprite, timeStamp);
            _frames.Add(frame);
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                PlaybackProgress += (float) gameTime.ElapsedGameTime.TotalSeconds;
                if (PlaybackProgress > Duration)
                {
                    if (ShouldLoop)
                        PlaybackProgress -= Duration;
                    else
                        Stop();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            SpriteAnimationFrame frame = CurrentFrame;

            if(frame != null)
            {
                frame.Sprite.Draw(spriteBatch, position);
            }
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            PlaybackProgress = 0;
        }

        public SpriteAnimationFrame GetFrame(int index)
        {
            if (index < 0 || index >= _frames.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "A frame with index" + index + "does not exist in this animation.");
            return _frames[index];
        }

        public void Clear()
        {
            Stop();
            _frames.Clear();
        }

    }
}
