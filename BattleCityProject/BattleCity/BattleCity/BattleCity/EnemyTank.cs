using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BattleCity
{
    public class EnemyTank
    {
        #region Fields

        // graphic and drawing info
        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        Texture2D sprite1;
        Texture2D sprite2;

        // tank stats
        int lives = 1;

        int tankType;
        int tankNumber;

        bool hasBonus = false;
        int elapsedTimeHasBonus = 0;
        bool changeBonusSprite = false;

        int movementSpeed;

        // moving support

        bool changeSprite = false;
        int elapsedGameTime = 0;
        string direction;
        string prevDirection;

        int periodDuration;
        int lifeTime;
        int pursuePeriod = 0;

        Color[] armoredTankColors = { Color.White, Color.White, new Color(110, 210, 140), new Color(220, 175, 100), new Color(60, 160, 90) };

        int canMoveLeft;
        int canMoveRight;
        int canMoveUp;
        int canMoveDown;

        string[] movingDirection = new string[] { "Left", "Right", "Up", "Down" };

        Dictionary<string, int> movingSprite = 
            new Dictionary<string, int>(4);

        Vector2 destination = new Vector2(0, 0);

        int canChangeDirectionDelay;
        bool canChangeDirection = true;

        #endregion

        #region Properties


        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        public int CollisionRectangleX
        {
            get { return drawRectangle.X; }
            set { drawRectangle.X = value; }
        }
        public int CollisionRectangleY
        {
            get { return drawRectangle.Y; }
            set { drawRectangle.Y = value; }
        }

        public string Direction
        {
            get { return direction; }
        }

        public int MovementSpeed
        {
            get { return movementSpeed; }
        }

        public int CanMoveLeft
        {
            get { return canMoveLeft; }
            set { canMoveLeft = value; }
        }
        public int CanMoveRight
        {
            get { return canMoveRight; }
            set { canMoveRight = value; }
        }
        public int CanMoveUp
        {
            get { return canMoveUp; }
            set { canMoveUp = value; }
        }
        public int CanMoveDown
        {
            get { return canMoveDown; }
            set { canMoveDown = value; }
        }

        public bool HasBonus
        {
            get { return hasBonus; }
        }

        public int TankType
        {
            get { return tankType; }
        }

        public int Lives
        {
            get { return lives; }
            set
            {
                lives = value < 0 ? 0 : value;
            }
        }

        public Vector2 Destination
        {
            get { return destination; }
            set { if (pursuePeriod < 2) destination = value; }
        }

        #endregion


        #region Constructors

        public EnemyTank(ContentManager contentManager, int tankType, int tankNumber, int startX, int startY, int enemyTankRespawnTime)
        {
            this.tankType = tankType;
            this.tankNumber = tankNumber;

            this.periodDuration = (int)enemyTankRespawnTime * 8;    // 2000 для теста

            sprite1 = contentManager.Load<Texture2D>("enemytank1");
            sprite2 = contentManager.Load<Texture2D>("enemytank2");

            sprite = sprite1;

            if (tankNumber == 3 || tankNumber == 10 || tankNumber == 17)
            {
                sprite = sprite2;
                hasBonus = true;
            }

            movementSpeed = tankType == 1 ? GameConstants.ENEMY_TANK_MOVEMENT_SPEED_FAST : GameConstants.ENEMY_TANK_MOVEMENT_SPEED;

            canMoveLeft = movementSpeed;
            canMoveRight = movementSpeed;
            canMoveUp = movementSpeed;
            canMoveDown = movementSpeed;

            if (tankType == 3)
            {
                lives = 4;
            }

            // set initial draw and source rectangles

            sourceRectangle = new Rectangle(0, (GameConstants.TILE_WIDTH * 4 + GameConstants.TILE_WIDTH * tankType), GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            drawRectangle = new Rectangle(startX, startY, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            direction = movingDirection[RandomNumberGenerator.Next(4)];

            movingSprite.Add("Left", 2);
            movingSprite.Add("Right", 6);
            movingSprite.Add("Up", 0);
            movingSprite.Add("Down", 4);

        }

        #endregion

        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="gamepad">the current state of the gamepad</param>
        /// <param name="soundBank">the sound bank</param>
        public void Update(GameTime gameTime, bool bonusTimerIsActive)
        {
            // в периоде 0 двигается хаотично
            // в периоде 1 преследует playerTank
            // в периоде 2 двигается к штабу
            if (pursuePeriod < 2)
            {
                lifeTime += gameTime.ElapsedGameTime.Milliseconds;

                if (lifeTime > periodDuration)
                {
                    pursuePeriod += 1;

                    if (pursuePeriod == 1)
                    {
                        periodDuration *= 2;
                    }
                    else if (pursuePeriod == 2)
                    {
                        destination.X = GameConstants.TILE_WIDTH * 6;
                        destination.Y = GameConstants.FIELD_HEIGHT - GameConstants.TILE_WIDTH;
                    }
                }
            }

            if (hasBonus)
            {
                elapsedTimeHasBonus += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedTimeHasBonus > 120)
                {
                    elapsedTimeHasBonus = 0;
                    if (changeBonusSprite)
                    {
                        sprite = sprite2;
                        changeBonusSprite = false;
                    }
                    else
                    {
                        sprite = sprite1;
                        changeBonusSprite = true;
                    }
                }
            }

            if (bonusTimerIsActive)
            {
                return;
            }

            prevDirection = direction;

            canChangeDirectionDelay += gameTime.ElapsedGameTime.Milliseconds;
            if (canChangeDirectionDelay > 500)
            {
                canChangeDirection = true;
                canChangeDirectionDelay = 0;
            }

            if (canChangeDirection && drawRectangle.X % GameConstants.SMALL_TILE_WIDTH == 0 &&    // на границе тайла
                drawRectangle.Y % GameConstants.SMALL_TILE_WIDTH == 0 &&
                RandomNumberGenerator.Next(16) == 0)
            {
                if (pursuePeriod > 0)
                {
                    while (direction == prevDirection)
                        direction = movingDirection[RandomNumberGenerator.Next(4)];
                }
                else
                {
                    ChangeDirection(direction, "change");
                }
                
                canChangeDirection = false;
            }
            else if (canChangeDirection && RandomNumberGenerator.Next(2) == 0 && (                // упирается в препятствие
                (direction == "Left" && (CanMoveLeft == 0 || drawRectangle.Left == 0)) ||
                (direction == "Right" && (CanMoveRight == 0 || drawRectangle.Right == GameConstants.FIELD_WIDTH)) ||
                (direction == "Up" && (CanMoveUp == 0 || drawRectangle.Top == 0)) ||
                (direction == "Down" && (CanMoveDown == 0 || drawRectangle.Bottom == GameConstants.FIELD_HEIGHT))))
            {
                if (drawRectangle.X % GameConstants.SMALL_TILE_WIDTH != 0 ||
                    drawRectangle.Y % GameConstants.SMALL_TILE_WIDTH != 0 )
                {
                    ChangeDirection(direction, "invert");
                }
                else
                {
                    ChangeDirection(direction, "change");
                }
                canChangeDirection = false;
            }

            foreach (KeyValuePair<string, int> spriteData in movingSprite)
            {
                if (spriteData.Key == direction)
                {
                    sourceRectangle.X = GameConstants.TILE_WIDTH * spriteData.Value;
                    elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedGameTime > 30)
                    {
                        elapsedGameTime = 0;
                        if (changeSprite)
                        {
                            sourceRectangle.X = GameConstants.TILE_WIDTH * (spriteData.Value + 1);
                            changeSprite = false;
                        }
                        else
                        {
                            sourceRectangle.X = GameConstants.TILE_WIDTH * spriteData.Value;
                            changeSprite = true;
                        }
                    }
                }
            }


            if (direction == "Left" && drawRectangle.Left > 0 && canMoveLeft > 0)
            {
                if (canMoveLeft > drawRectangle.Left)
                    drawRectangle.X -= drawRectangle.Left;
                else
                    drawRectangle.X -= canMoveLeft;
            }
            if (direction == "Right" && drawRectangle.Right < GameConstants.FIELD_WIDTH && canMoveRight > 0)
            {
                if (canMoveRight > (GameConstants.FIELD_WIDTH - drawRectangle.Right))
                    drawRectangle.X += GameConstants.FIELD_WIDTH - drawRectangle.Right;
                else
                    drawRectangle.X += canMoveRight;
            }
            if (direction == "Up" && drawRectangle.Top > 0 && canMoveUp > 0)
            {
                if (canMoveUp > drawRectangle.Top)
                    drawRectangle.Y -= drawRectangle.Top;
                else
                    drawRectangle.Y -= canMoveUp;
            }
            if (direction == "Down" && drawRectangle.Bottom < GameConstants.FIELD_HEIGHT && canMoveDown > 0)
            {
                if (canMoveDown > (GameConstants.FIELD_HEIGHT - drawRectangle.Bottom))
                    drawRectangle.Y += GameConstants.FIELD_HEIGHT - drawRectangle.Bottom;
                else
                    drawRectangle.Y += canMoveDown;
            }


            // shoot if appropriate
            int enemyBulletCount = Game1.EnemyBulletCount(tankNumber);

            if (enemyBulletCount == 0 && RandomNumberGenerator.Next(32) == 0)
            {

                Bullet bullet = new Bullet(Game1.GetBulletSprite(), direction, new Vector2(drawRectangle.Center.X, drawRectangle.Center.Y),
                    BulletType.Enemy, tankType == 2 ? BulletSpeed.Fast : BulletSpeed.Slow, BulletPower.Low, tankNumber);

                Game1.AddBullet(bullet);
            }

        }

        /// <summary>
        /// Draws the tank
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, tankType == 3 && !hasBonus ? armoredTankColors[lives] : Color.White);
        }

        #endregion


        public void RefreshMovementSpeed()
        {
            canMoveLeft = movementSpeed;
            canMoveRight = movementSpeed;
            canMoveUp = movementSpeed;
            canMoveDown = movementSpeed;
        }

        public void DisableBonus()
        {
            sprite = Game1.GetEnemyTankSprite();
            hasBonus = false;
        }

        private int GetRandomFiringDelay()
        {
            return GameConstants.ENEMY_TANK_MIN_FIRING_DELAY +
                RandomNumberGenerator.Next(GameConstants.ENEMY_TANK_FIRING_RATE_RANGE);
        }

        private void ChangeDirection(string oldDirection, string action)
        {
            switch (oldDirection)
            {
                case "Left":
                    if (action == "invert")
                    {
                        if (pursuePeriod > 0 && drawRectangle.X > destination.X)
                        {
                            direction = RandomNumberGenerator.Next(3) == 0 ? "Right" : "Left";
                        }
                        else
                        {
                            direction = "Right";
                        }
                    }
                    else if (action == "change")
                    {
                        if (pursuePeriod > 0)
                        {
                            if (drawRectangle.Y < destination.Y)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Up" : "Down";
                            }
                            else if (drawRectangle.Y >= destination.Y)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Down" : "Up";
                            }
                        }
                        else
                        {
                            direction = RandomNumberGenerator.Next(2) == 0 ? "Up" : "Down";
                        }
                    }
                    break;

                case "Right":
                    if (action == "invert")
                    {
                        if (pursuePeriod > 0 && drawRectangle.X < destination.X)
                        {
                            direction = RandomNumberGenerator.Next(3) == 0 ? "Left" : "Right";
                        }
                        else
                        {
                            direction = "Left";
                        }
                    }
                    else if (action == "change")
                    {
                        if (pursuePeriod > 0)
                        {
                            if (drawRectangle.Y < destination.Y)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Up" : "Down";
                            }
                            else if (drawRectangle.Y >= destination.Y)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Down" : "Up";
                            }
                        }
                        else
                        {
                            direction = RandomNumberGenerator.Next(2) == 0 ? "Up" : "Down";
                        }
                    }
                    break;

                case "Up":
                    if (action == "invert")
                    {
                        if (pursuePeriod > 0 && drawRectangle.Y > destination.Y)
                        {
                            direction = RandomNumberGenerator.Next(3) == 0 ? "Down" : "Up";
                        }
                        else
                        {
                            direction = "Down";
                        }
                    }
                    else if (action == "change")
                    {
                        if (pursuePeriod > 0)
                        {
                            if (drawRectangle.X < destination.X)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Left" : "Right";
                            }
                            else if (drawRectangle.X >= destination.X)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Right" : "Left";
                            }
                        }
                        else
                        {
                            direction = RandomNumberGenerator.Next(2) == 0 ? "Left" : "Right";
                        }
                    }
                    break;

                case "Down":
                    if (action == "invert")
                    {
                        if (pursuePeriod > 0 && drawRectangle.Y < destination.Y)
                        {
                            direction = RandomNumberGenerator.Next(3) == 0 ? "Up" : "Down";
                        }
                        else
                        {
                            direction = "Up";
                        }
                    }
                    else if (action == "change")
                    {
                        if (pursuePeriod > 0)
                        {
                            if (drawRectangle.X < destination.X)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Left" : "Right";
                            }
                            else if (drawRectangle.X >= destination.X)
                            {
                                direction = RandomNumberGenerator.Next(3) == 0 ? "Right" : "Left";
                            }
                        }
                        else
                        {
                            direction = RandomNumberGenerator.Next(2) == 0 ? "Left" : "Right";
                        }
                    }
                    break;

                default:
                    break;
            }
        }

    }
}
