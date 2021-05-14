using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrexRunner.Entities
{
    public abstract class SkyObject : IGameEntity
    {
        public int DrawOrder { get; set; }
        public abstract float Speed { get; }

        protected readonly Trex _trex;
        public Vector2 Position { get; set; }

        public SkyObject(Trex trex, Vector2 position)
        {
            _trex = trex;
            Position = position;
        }

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public virtual void Update(GameTime gameTime)
        {
            if (_trex.IsAlive)
                Position = new Vector2(Position.X - Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, Position.Y);
        }
    }
}
