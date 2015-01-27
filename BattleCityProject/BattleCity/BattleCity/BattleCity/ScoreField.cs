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
    class ScoreField
    {
        Texture2D scoreField;
        Rectangle scoreFieldDrawRectangle;

        Texture2D scoreSprite;
        Rectangle playerOneLivesDrawRectangle;
        Rectangle playerOneLivesSourceRectangle;

        NumberSprite playerOneLivesNumberSprite;

        Rectangle stageDrawRectangle;
        Rectangle stageSourceRectangle;

        NumberSprite stageNumberSprite;

        public ScoreField(ContentManager contentManager, GraphicsDevice graphicsDevice, int currentLevel)
        {

            scoreField = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);

            scoreField.SetData<Color>(new Color[] { new Color(99, 99, 99) });

            scoreFieldDrawRectangle = new Rectangle(GameConstants.FIELD_WIDTH, 0,
                    GameConstants.WINDOW_WIDTH - GameConstants.FIELD_WIDTH, GameConstants.WINDOW_HEIGHT);

            scoreSprite = contentManager.Load<Texture2D>("scoreSprite");
            playerOneLivesDrawRectangle = new Rectangle(
                GameConstants.FIELD_WIDTH + GameConstants.SMALL_TILE_WIDTH,
                GameConstants.TILE_WIDTH * 7 + GameConstants.SMALL_TILE_WIDTH,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            playerOneLivesSourceRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * 5, 0,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            playerOneLivesNumberSprite = new NumberSprite(contentManager, GameConstants.PLAYER_ONE_LIVES_COUNT, TextDirection.Right, Color.Black,
                GameConstants.FIELD_WIDTH + GameConstants.TILE_WIDTH + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH * 8);

            stageDrawRectangle = new Rectangle(
                GameConstants.FIELD_WIDTH + GameConstants.SMALL_TILE_WIDTH,
                GameConstants.TILE_WIDTH * 10 + GameConstants.SMALL_TILE_WIDTH,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            stageSourceRectangle = new Rectangle(
                GameConstants.TILE_WIDTH * 6, 0,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            stageNumberSprite = new NumberSprite(contentManager, currentLevel, TextDirection.Right, Color.Black,
                GameConstants.FIELD_WIDTH + GameConstants.TILE_WIDTH + GameConstants.SMALL_TILE_WIDTH,
                GameConstants.TILE_WIDTH * 11 + GameConstants.SMALL_TILE_WIDTH);

        }

        public void ChangeLives(int playerOneLivesCount)
        {
            playerOneLivesNumberSprite.UpdateNumber(playerOneLivesCount);
        }

        public void ChangeStage(int stageNumber)
        {
            stageNumberSprite.UpdateNumber(stageNumber);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(scoreField, scoreFieldDrawRectangle, Color.White);

            spriteBatch.Draw(scoreSprite, playerOneLivesDrawRectangle, playerOneLivesSourceRectangle, Color.White);
            playerOneLivesNumberSprite.Draw(spriteBatch);

            spriteBatch.Draw(scoreSprite, stageDrawRectangle, stageSourceRectangle, Color.White);
            stageNumberSprite.Draw(spriteBatch);
        }
    }

    class EnemyTankIcon
    {
        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        public EnemyTankIcon(ContentManager contentManager, int iconNumber)
        {
            sprite = contentManager.Load<Texture2D>("scoresprite");
            sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * 2, GameConstants.TILE_WIDTH * 2,
                GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);

            int startX = GameConstants.FIELD_WIDTH + GameConstants.SMALL_TILE_WIDTH + GameConstants.SMALL_TILE_WIDTH * (iconNumber % 2);
            int startY = GameConstants.SMALL_TILE_WIDTH + GameConstants.SMALL_TILE_WIDTH * (int)(iconNumber / 2);

            drawRectangle = new Rectangle(startX, startY, GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);
        }
    }
}
