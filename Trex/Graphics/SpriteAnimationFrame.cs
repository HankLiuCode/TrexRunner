using System;
using System.Collections.Generic;
using System.Text;

namespace TrexRunner.Graphics
{
    public class SpriteAnimationFrame
    {
        private Sprite _sprite;

        //// automatically created by the property TimeStamp
        //private readonly float _timeStamp;

        public Sprite Sprite
        {
            get
            {
                return _sprite;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value", "The Sprite cannot be null");
                _sprite = value;
            }
        }

        public float TimeStamp { get; }

        public SpriteAnimationFrame(Sprite sprite, float timeStamp)
        {
            Sprite = sprite;
            TimeStamp = timeStamp;
        }
    }
}
