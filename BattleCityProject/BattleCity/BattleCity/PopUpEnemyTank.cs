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
    class PopUpEnemyTank
    {
        Texture2D popUpSprite;
        Rectangle drawRectangle;
        Rectangle popUpSourceRectangle;

        int popUpTimer = 1000;
        int elapsedTimePopUpSprite = 0;
        int popUpCurrentSprite = 0;
        string popUpSpriteDirection = "up";

        public int PopUpTimer
        {
            get { return popUpTimer; }
        }

        public PopUpEnemyTank(ContentManager contentManager, int startX, int startY)
        {
            popUpSprite = contentManager.Load<Texture2D>("bulletSprite");
            popUpSourceRectangle = new Rectangle(0, 0, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            drawRectangle = new Rectangle(startX, startY, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
        }


        public void Update(GameTime gameTime)
        {
            if (popUpTimer > 0)
            {
                popUpTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (popUpTimer < 0)
                {
                    popUpTimer = 0;
                }

                elapsedTimePopUpSprite += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedTimePopUpSprite > 60)
                {
                    elapsedTimePopUpSprite = 0;
                    if (popUpSpriteDirection == "up")
                    {
                        popUpCurrentSprite += 1;
                        if (popUpCurrentSprite == 3) popUpSpriteDirection = "down";
                    }
                    else
                    {
                        popUpCurrentSprite -= 1;
                        if (popUpCurrentSprite == 0) popUpSpriteDirection = "up";
                    }
                    popUpSourceRectangle.X = GameConstants.TILE_WIDTH * popUpCurrentSprite;
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(popUpSprite, drawRectangle, popUpSourceRectangle, Color.White);
        }
    }

}
