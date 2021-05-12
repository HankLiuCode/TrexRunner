using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TrexRunner.Entities
{
    public class ObstacleManager : IGameEntity
    {

        private static readonly int[] FLYING_DINO_Y_POSITIONS = new int[] { 90, 60, 20 };

        private const float MIN_SPAWN_DISTANCE = 20;
        private const int MIN_OBSTACLE_DISTANCE = 10;
        private const int MAX_OBSTACLE_DISTANCE = 50;

        private const int OBSTACLE_DISTANCE_SPEED_TOLERANCE = 5;
        private const int LARGE_CACTUS_POS_Y = 80;
        private const int SMALL_CACTUS_POS_X = 94;

        private const int OBSTACLE_DRAW_ORDER = 12;
        private const int OBSTACLE_DESPAWN_POS_X = -200;

        private const int FLYING_DINO_SPAWN_SCORE_MIN = 100;

        private double _lastSpawnScore = -1;
        private double _currentTargetDistance;

        public int DrawOrder => 0;
        private readonly EntityManager _entityManager;
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreBoard;

        private readonly Random _random;
        private Texture2D _spriteSheet;

        public bool IsEnabled { get; set; }
        public bool CanSpawnObstacles => IsEnabled && _scoreBoard.Score >= MIN_SPAWN_DISTANCE;

        public ObstacleManager(Texture2D spriteSheet, EntityManager entityManager, Trex trex, ScoreBoard scoreBoard)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreBoard = scoreBoard;
            _random = new Random();
            _spriteSheet = spriteSheet;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;
            
            if(CanSpawnObstacles && 
                (_lastSpawnScore <= 0 || (_scoreBoard.Score - _lastSpawnScore >= _currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble() 
                    * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE) + MIN_OBSTACLE_DISTANCE;

                _currentTargetDistance += ((_trex.Speed - Trex.START_SPEED) / (Trex.MAX_SPEED - Trex.START_SPEED)) * OBSTACLE_DISTANCE_SPEED_TOLERANCE;


                _lastSpawnScore = _scoreBoard.Score;
                SpawnRandomObstacle();
            }

            foreach(Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                if (obstacle.Position.X < OBSTACLE_DESPAWN_POS_X)
                    _entityManager.RemoveEntity(obstacle);
            }
        }

        private void SpawnRandomObstacle()
        {
            // TODO: Create instance of obstacle and add to entity manager
            Obstacle obstacle = null;

            int cactusGroupSpawnRate = 75;
            int flyingDinoSpawnRate = _scoreBoard.Score >= FLYING_DINO_SPAWN_SCORE_MIN ? 25 : 0;

            int rng = _random.Next(0, cactusGroupSpawnRate + flyingDinoSpawnRate + 1);
            if(rng <= cactusGroupSpawnRate)
            {
                CactusGroup.GroupSize randomGroupSize = (CactusGroup.GroupSize)_random.Next((int)CactusGroup.GroupSize.Small, (int)CactusGroup.GroupSize.Large + 1);
                bool isLarge = _random.NextDouble() > 0.5f;
                float posY = isLarge ? LARGE_CACTUS_POS_Y : SMALL_CACTUS_POS_X;

                obstacle = new CactusGroup(_spriteSheet, isLarge, randomGroupSize, _trex, new Vector2(TRexGame.WINDOW_WIDTH, posY));
            }
            else
            {
                int verticalPosIndex = _random.Next(0, FLYING_DINO_Y_POSITIONS.Length);
                float posY = FLYING_DINO_Y_POSITIONS[verticalPosIndex];

                obstacle = new FlyingDino(_trex, new Vector2(TRexGame.WINDOW_WIDTH, posY), _spriteSheet);
            }

            
            obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;
            _entityManager.AddEntity(obstacle);
        }

        public void Reset()
        {
            foreach(Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                _entityManager.RemoveEntity(obstacle);
            }
            _currentTargetDistance = 0;
            _lastSpawnScore = -1;
        }
    }
}
