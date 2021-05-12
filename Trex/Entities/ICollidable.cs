using Microsoft.Xna.Framework;

namespace TrexRunner.Entities
{
    public interface ICollidable
    {
        Rectangle CollisionBox { get; }
    }
}
