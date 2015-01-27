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
    public class PlayerTank
    {
        #region Fields

        // graphic and drawing info
        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        Texture2D shieldSprite;
        Rectangle shieldDrawRectangle;
        Rectangle shieldSourceRectangle;

        // tank stats
        int lives = GameConstants.PLAYER_ONE_LIVES_COUNT;
        bool visible = true;
        bool isActive = true;
        int tankType = 0; 

        int respawnCooldownTime = 0;

        // shield support
        bool shieldIsActive = true;
        int shieldActiveTime = 0;
        int shieldElapsedCooldownTime = 0;
        bool shieldChangeSprite = false;

        // shooting support
        bool canShoot = true;

        // moving support

        bool changeSprite = false;
        int elapsedGameTime = 0;
        string lastKey;
        string direction = "Up";
        string prevDirection = "Up";

        int canMoveLeft = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
        int canMoveRight = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
        int canMoveUp = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
        int canMoveDown = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;

        List<string> movingDirection = new List<string>();

        Dictionary<string, int> movingSprite = 
            new Dictionary<string, int>(4);

        Dictionary<string, bool> keyState =
            new Dictionary<string, bool>(4);
        Dictionary<string, bool> keyPrevState;

        bool keyPrevStateSpace = false;

        #endregion


        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the PlayerTank
        /// </summary>
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

        public string LastKey
        {
            get { return lastKey; }
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
                if (value < 0)
                {
                    lives = 0;
                }
                else if (value > 9)
                {
                    lives = 9;
                }
                else
                {
                    lives = value;
                }
            }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public bool CanShoot
        {
            get { return canShoot; }
            set { canShoot = value; }
        }

        public bool ShieldIsActive
        {
            get { return shieldIsActive; }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a tank
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="spriteName">the sprite name</param>
        public PlayerTank(ContentManager contentManager, string spriteName)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            shieldSprite = contentManager.Load<Texture2D>("bulletSprite");

            movingSprite.Add("Left", 2);
            movingSprite.Add("Right", 6);
            movingSprite.Add("Up", 0);
            movingSprite.Add("Down", 4);

            shieldActiveTime = GameConstants.PLAYER_SHIELD_ACTIVE_TIME;

            // set initial draw and source rectangles

            sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * movingSprite[direction], GameConstants.TILE_WIDTH * tankType, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            drawRectangle = new Rectangle(0, 0, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            shieldSourceRectangle = new Rectangle(0, GameConstants.TILE_WIDTH * 3, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            shieldDrawRectangle = drawRectangle;

            keyState.Add("Left", false);
            keyState.Add("Right", false);
            keyState.Add("Up", false);
            keyState.Add("Down", false);

            keyPrevState = new Dictionary<string, bool>(keyState);

            // установка танка на место
            Respawn("newgame");

        }

        #endregion


        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="gamepad">the current state of the gamepad</param>
        /// <param name="soundBank">the sound bank</param>
        public void Update(GameTime gameTime, KeyboardState keyboard, SoundBank soundBank, Cue cue)
        {

            // смена спрайта shield
            if (shieldIsActive)
            {
                shieldElapsedCooldownTime += gameTime.ElapsedGameTime.Milliseconds;
                if (shieldElapsedCooldownTime > 45)
                {
                    shieldElapsedCooldownTime = 0;
                    if (shieldChangeSprite)
                    {
                        shieldSourceRectangle.X = GameConstants.TILE_WIDTH;
                        shieldChangeSprite = false;
                    }
                    else
                    {
                        shieldSourceRectangle.X = 0;
                        shieldChangeSprite = true;
                    }
                }

                if (shieldActiveTime > 0)
                {
                    shieldActiveTime -= gameTime.ElapsedGameTime.Milliseconds;
                    if (shieldActiveTime <= 0)
                    {
                        shieldActiveTime = 0;
                        shieldIsActive = false;
                    }
                }
            }

            // задержка перед появлением танка
            if (respawnCooldownTime > 0)
            {
                respawnCooldownTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (respawnCooldownTime <= 0)
                {
                    respawnCooldownTime = 0;
                    isActive = true;
                    visible = true;
                }
            }

            if (!isActive) return;

            keyState["Left"] = keyboard.IsKeyDown(Keys.Left);
            keyState["Right"] = keyboard.IsKeyDown(Keys.Right);
            keyState["Up"] = keyboard.IsKeyDown(Keys.Up);
            keyState["Down"] = keyboard.IsKeyDown(Keys.Down);    

            foreach (KeyValuePair<string, bool> direction in keyState)
            {
                if (!keyPrevState[direction.Key] && keyState[direction.Key])
                {
                    movingDirection.Add(direction.Key);
                }
                else if (keyPrevState[direction.Key] && !keyState[direction.Key])
                {
                    movingDirection.Remove(direction.Key);
                }
            }

            keyPrevState = new Dictionary<string, bool>(keyState);

            lastKey = "";


            if (movingDirection.Count > 0 && isActive == true)
            {
                lastKey = movingDirection.Last();

                foreach (KeyValuePair<string, int> spriteData in movingSprite)
                {
                    if (spriteData.Key == lastKey)
                    {
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

                    direction = lastKey;
                }

                if (lastKey == "Left" && drawRectangle.Left > 0 && canMoveLeft > 0)
                {
                    if (canMoveLeft > drawRectangle.Left)
                        drawRectangle.X -= drawRectangle.Left;
                    else
                        drawRectangle.X -= canMoveLeft;

                    // округление координаты при повороте
                    RoundTankPosition(prevDirection, lastKey);

                }
                if (lastKey == "Right" && drawRectangle.Right < GameConstants.FIELD_WIDTH && canMoveRight > 0)
                {
                    if (canMoveRight > (GameConstants.FIELD_WIDTH - drawRectangle.Right))
                        drawRectangle.X += GameConstants.FIELD_WIDTH - drawRectangle.Right;
                    else
                        drawRectangle.X += canMoveRight;

                    RoundTankPosition(prevDirection, lastKey);
                }
                if (lastKey == "Up" && drawRectangle.Top > 0 && canMoveUp > 0)
                {
                    if (canMoveUp > drawRectangle.Top)
                        drawRectangle.Y -= drawRectangle.Top;
                    else
                        drawRectangle.Y -= canMoveUp;

                    RoundTankPosition(prevDirection, lastKey);
                }
                if (lastKey == "Down" && drawRectangle.Bottom < GameConstants.FIELD_HEIGHT && canMoveDown > 0)
                {
                    if (canMoveDown > (GameConstants.FIELD_HEIGHT - drawRectangle.Bottom))
                        drawRectangle.Y += GameConstants.FIELD_HEIGHT - drawRectangle.Bottom;
                    else
                        drawRectangle.Y += canMoveDown;

                    RoundTankPosition(prevDirection, lastKey);
                }

                shieldDrawRectangle = drawRectangle;

                
                if (cue.IsPaused)
                    cue.Resume();
                else if (!cue.IsPlaying && !cue.IsPaused)
                    cue.Play();
            }
            else
            {
                if (!cue.IsPaused)
                    cue.Pause();
            }

            prevDirection = direction;

            // shoot if appropriate
            if (keyPrevStateSpace == false && keyboard.IsKeyDown(Keys.Space) && canShoot && isActive == true)
            {
                Bullet bullet = new Bullet(Game1.GetBulletSprite(), direction, new Vector2(drawRectangle.Center.X, drawRectangle.Center.Y),
                    BulletType.Player, tankType > 0 ? BulletSpeed.Fast : BulletSpeed.Slow,
                    tankType == 3 ? BulletPower.High : BulletPower.Low, 0);

                Game1.AddBullet(bullet);

                soundBank.PlayCue("w3_playerShoot");

                canShoot = false;
            }

            int playerBulletCount = Game1.PlayerBulletCount();

            // стреляет два раза если tankType > 1
            if (playerBulletCount == 0 || playerBulletCount == 1 && tankType > 1)
            {
                canShoot = true;
            }

            keyPrevStateSpace = keyboard.IsKeyDown(Keys.Space);

        }

        /// <summary>
        /// Draws the tank
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);
                if (shieldIsActive)
                    spriteBatch.Draw(shieldSprite, shieldDrawRectangle, shieldSourceRectangle, Color.White);
            }
        }

        public void Respawn(string respType)
        {
            if (respType != "newlevel")
                tankType = 0;
            respawnCooldownTime = GameConstants.PLAYER_RESPAWN_COOLDOWN_TIME;
            shieldIsActive = true;
            shieldActiveTime = GameConstants.PLAYER_SHIELD_ACTIVE_TIME;
            direction = "Up";
            lastKey = "Up";
            sourceRectangle.X = 0;
            sourceRectangle.Y = GameConstants.TILE_WIDTH * tankType;
            drawRectangle.X = GameConstants.TILE_WIDTH * 4;
            drawRectangle.Y = GameConstants.FIELD_HEIGHT - GameConstants.TILE_WIDTH;
            shieldDrawRectangle = drawRectangle;
        }

        public void RefreshMovementSpeed()
        {
            canMoveLeft = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
            canMoveRight = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
            canMoveUp = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
            canMoveDown = GameConstants.PLAYER_TANK_MOVEMENT_SPEED;
        }

        public void BonusShield()
        {
            shieldActiveTime = GameConstants.PLAYER_SHIELD_ACTIVE_TIME * GameConstants.PLAYER_BONUS_LENGTH;
            shieldIsActive = true;
        }

        public void BonusStar()
        {
            if (tankType < 3) tankType += 1;
            sourceRectangle.Y = GameConstants.TILE_WIDTH * tankType;

        }

        #endregion

        /// <summary>
        /// округление координат танка при повороте
        /// </summary>
        /// <param name="prevDirection"></param>
        private void RoundTankPosition(string prevDirection, string direction)
        {
            if (direction == "Up" || direction == "Down")
            {
                if (prevDirection == "Left" || prevDirection == "Right")
                {
                    int ost = drawRectangle.X % GameConstants.SMALL_TILE_WIDTH;

                    if (ost > GameConstants.SMALL_TILE_WIDTH / 2)
                        drawRectangle.X += GameConstants.SMALL_TILE_WIDTH - ost;
                    else
                        drawRectangle.X -= ost;
                }
            }
            else
            {
                if (prevDirection == "Up" || prevDirection == "Down")
                {
                    int ost = drawRectangle.Y % GameConstants.SMALL_TILE_WIDTH;

                    if (ost > GameConstants.SMALL_TILE_WIDTH / 2)
                        drawRectangle.Y += GameConstants.SMALL_TILE_WIDTH - ost;
                    else
                        drawRectangle.Y -= ost;
                }
            }
        }
    }
}
