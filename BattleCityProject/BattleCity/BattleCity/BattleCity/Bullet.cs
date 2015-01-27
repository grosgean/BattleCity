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
    public class Bullet
    {
        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        bool active = true;
        string direction;
        float speed;

        BulletPower bulletPower;

        BulletType type;

        int tankNumber;


        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the Tile
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        public string Direction
        {
            get { return direction; }
        }

        public BulletType Type
        {
            get { return type; }
        }

        public BulletPower BulletPower
        {
            get { return bulletPower; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public int TankNumber
        {
            get { return tankNumber; }
        }

        #endregion

        public Bullet(Texture2D sprite, string direction, Vector2 tankCenterPosition, BulletType type, BulletSpeed bulletSpeed,
                        BulletPower bulletPower, int tankNumber)
        {
            this.sprite = sprite;
            this.direction = direction;
            this.type = type;
            this.bulletPower = bulletPower;
            this.tankNumber = tankNumber;

            speed = (bulletSpeed == BulletSpeed.Fast) ? GameConstants.BULLET_SPEED_FAST : GameConstants.BULLET_SPEED;

            switch (direction)
            {
                case "Left":
                    sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * 4, 0,
                                        GameConstants.BULLET_HEIGHT, GameConstants.BULLET_WIDTH);
                    drawRectangle = new Rectangle(
                                            (int)tankCenterPosition.X - GameConstants.TILE_WIDTH/2,
                                            (int)tankCenterPosition.Y - GameConstants.BULLET_WIDTH/2,
                                            GameConstants.BULLET_HEIGHT, GameConstants.BULLET_WIDTH
                                        );
                    break;

                case "Right":

                    sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * 4 + GameConstants.BULLET_HEIGHT, 0,
                                        GameConstants.BULLET_HEIGHT, GameConstants.BULLET_WIDTH);
                    drawRectangle = new Rectangle(
                                            (int)tankCenterPosition.X + GameConstants.TILE_WIDTH/2 - GameConstants.BULLET_HEIGHT,
                                            (int)tankCenterPosition.Y - GameConstants.BULLET_WIDTH / 2,
                                            GameConstants.BULLET_HEIGHT, GameConstants.BULLET_WIDTH
                                        );
                    break;

                case "Up":

                    sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * 4 + GameConstants.BULLET_HEIGHT * 2, 0,
                                        GameConstants.BULLET_WIDTH, GameConstants.BULLET_HEIGHT);
                    drawRectangle = new Rectangle(
                                            (int)tankCenterPosition.X - GameConstants.BULLET_WIDTH / 2,
                                            (int)tankCenterPosition.Y - GameConstants.TILE_WIDTH/2,
                                            GameConstants.BULLET_WIDTH, GameConstants.BULLET_HEIGHT
                                        );
                    break;

                case "Down":

                    sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * 4 + GameConstants.BULLET_HEIGHT * 2 + GameConstants.BULLET_WIDTH, 0,
                                        GameConstants.BULLET_WIDTH, GameConstants.BULLET_HEIGHT);
                    drawRectangle = new Rectangle(
                                            (int)tankCenterPosition.X - GameConstants.BULLET_WIDTH/2,
                                            (int)tankCenterPosition.Y + GameConstants.TILE_WIDTH / 2 - GameConstants.BULLET_HEIGHT,
                                            GameConstants.BULLET_WIDTH, GameConstants.BULLET_HEIGHT
                                        );
                    break;

                default:
                    break;
            }

            
                                            
        }

        public void Update(GameTime gameTime, SoundBank soundBank)
        {
            switch (direction)
            {
                case "Left":
                    drawRectangle.X -= (int)(speed * gameTime.ElapsedGameTime.Milliseconds);
                    break;

                case "Right":
                    drawRectangle.X += (int)(speed * gameTime.ElapsedGameTime.Milliseconds);
                    break;

                case "Up":
                    drawRectangle.Y -= (int)(speed * gameTime.ElapsedGameTime.Milliseconds);
                    break;

                case "Down":
                    drawRectangle.Y += (int)(speed * gameTime.ElapsedGameTime.Milliseconds);
                    break;

                default:
                    break;
            }
            

            // check for outside game window
            if (drawRectangle.Bottom > GameConstants.FIELD_HEIGHT || drawRectangle.Top < 0
                || drawRectangle.Left > GameConstants.FIELD_WIDTH || drawRectangle.Right < 0)
            {
                active = false;

                if (type == BulletType.Player)
                    soundBank.PlayCue("w15_bullet2wall");
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);
        }
    }
}
