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
    class NextLevel
    {
        Texture2D scoreField;
        Rectangle scoreFieldDrawRectangle;
        Rectangle scoreFieldDrawRectangle2;

        StringSprite stageText;
        NumberSprite stageNumberSprite;

        int halfWindowHeight = GameConstants.WINDOW_HEIGHT / 2;
        int timeDelay = 0;

        bool isDone = false;

        public bool IsDone
        {
            get { return isDone; }
            set { isDone = value; }
        }

        public NextLevel(ContentManager contentManager, GraphicsDevice graphicsDevice, int currentLevel)
        {
            scoreField = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);

            scoreField.SetData<Color>(new Color[] { new Color(99, 99, 99) });

            scoreFieldDrawRectangle = new Rectangle(0, 0,
                    GameConstants.WINDOW_WIDTH, 0);

            scoreFieldDrawRectangle2 = new Rectangle(0, GameConstants.WINDOW_HEIGHT,
                    GameConstants.WINDOW_WIDTH, 0);

            stageText = new StringSprite(contentManager, "STAGE", Color.Black,
                GameConstants.TILE_WIDTH * 5 + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH * 6);

            stageNumberSprite = new NumberSprite(contentManager, currentLevel, TextDirection.Right, Color.Black,
                GameConstants.TILE_WIDTH * 9 + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH * 6);
        }

        public void ChangeStage(int stageNumber)
        {
            stageNumberSprite.UpdateNumber(stageNumber);
        }

        public void Update(GameTime gameTime)
        {
            if (scoreFieldDrawRectangle.Height < halfWindowHeight)
            {
                scoreFieldDrawRectangle.Height += 15;
                scoreFieldDrawRectangle2.Y -= 15;
                scoreFieldDrawRectangle2.Height += 15;
            }
            else if (timeDelay < 2500)
            {
                timeDelay += gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                scoreFieldDrawRectangle.Height = 0;
                scoreFieldDrawRectangle2.Y = GameConstants.WINDOW_HEIGHT;
                scoreFieldDrawRectangle2.Height = 0;

                timeDelay = 0;
                isDone = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(scoreField, scoreFieldDrawRectangle, Color.White);
            spriteBatch.Draw(scoreField, scoreFieldDrawRectangle2, Color.White);
            stageText.Draw(spriteBatch);
            stageNumberSprite.Draw(spriteBatch);
        }
    }
}
