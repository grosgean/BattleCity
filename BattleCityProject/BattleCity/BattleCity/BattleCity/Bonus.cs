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
    class Bonus
    {
        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        int type;

        int elapsedGameTime = 0;
        int elapsedGameTimeLength = GameConstants.PLAYER_SHIELD_ACTIVE_TIME * GameConstants.PLAYER_BONUS_LENGTH;

        bool active = true;
        bool visible = true;

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the Tile
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion
        
        public Bonus(Texture2D sprite)
        {
            this.sprite = sprite;
            type = RandomNumberGenerator.Next(6);
            //type = 4;
            
            sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * type, GameConstants.TILE_WIDTH,
                                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            drawRectangle = new Rectangle(
                    GameConstants.SMALL_TILE_WIDTH * 2 * (RandomNumberGenerator.Next(GameConstants.FIELD_WIDTH_TILES)),
                    GameConstants.SMALL_TILE_WIDTH * 2 * (RandomNumberGenerator.Next(GameConstants.FIELD_WIDTH_TILES)),
                    GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH
            );                               
        }

        public void Update(GameTime gameTime)
        {
            elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedGameTime > 200)
            {
                elapsedGameTime = 0;
                visible = visible ? false : true;
            }

            elapsedGameTimeLength -= gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedGameTimeLength < 0)
            {
                elapsedGameTimeLength = 0;
                active = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);
            }
        }
    }
}
