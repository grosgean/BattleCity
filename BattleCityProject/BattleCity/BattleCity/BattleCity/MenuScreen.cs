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
    class MenuScreen
    {
        Texture2D tilesSprite;

        List<Tile> tiles = new List<Tile>();

        List<StringSprite> text = new List<StringSprite>();

        int player1record = 2000;
        int hiRecord = 20000;

        NumberSprite player1recordSprite;
        NumberSprite hiRecordSprite;

        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        int elapsedGameTime = 0;
        bool changeSprite = false;
        

        int[,] menuScreenTilesContent = { 
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,10,7,3,13,0,14,11,4,0,3,15,3,0,3,15,3,0,15,0,0,0,15,3,3,0},
            {0,10,13,12,7,10,5,0,15,0,0,15,0,0,0,15,0,0,15,0,0,0,15,12,4,0},
            {0,10,5,0,15,10,7,3,15,0,0,15,0,0,0,15,0,0,15,0,0,0,15,0,0,0},
            {0,2,3,3,1,2,1,0,3,0,0,3,0,0,0,3,0,0,3,3,3,0,3,3,3,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,8,7,11,4,2,11,7,1,2,11,7,1,10,5,10,5,0,0,0,0},
            {0,0,0,0,0,0,15,0,0,0,0,10,5,0,0,10,5,0,2,13,14,1,0,0,0,0},
            {0,0,0,0,0,0,11,4,8,4,0,10,5,0,0,10,5,0,0,10,5,0,0,0,0,0},
            {0,0,0,0,0,0,0,3,3,0,2,3,3,1,0,2,1,0,0,2,1,0,0,0,0,0}
        };

        public MenuScreen(ContentManager contentManager)
        {
            tilesSprite = contentManager.Load<Texture2D>("minitiles");

            for (int row = 0; row < menuScreenTilesContent.GetLength(0); row++)
            {
                for (int col = 0; col < menuScreenTilesContent.GetLength(1); col++)
                {
                    tiles.Add(new Tile(tilesSprite, 1, 2, menuScreenTilesContent[row, col], col, row, false));
                }
            }

            sprite = contentManager.Load<Texture2D>("playertank1");
            sourceRectangle = new Rectangle(GameConstants.TILE_WIDTH * 6, 0, GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);
            drawRectangle = new Rectangle(GameConstants.TILE_WIDTH * 4, GameConstants.TILE_WIDTH * 8 - GameConstants.SMALL_TILE_WIDTH/2,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH);

            text.Add(new StringSprite(contentManager, "I-", Color.White,
                GameConstants.TILE_WIDTH, GameConstants.TILE_WIDTH));

            player1recordSprite = new NumberSprite(contentManager, player1record, TextDirection.Right, Color.White,
                GameConstants.TILE_WIDTH * 5, GameConstants.TILE_WIDTH);
            hiRecordSprite = new NumberSprite(contentManager, hiRecord, TextDirection.Right, Color.White,
                GameConstants.TILE_WIDTH * 10, GameConstants.TILE_WIDTH);

            text.Add(new StringSprite(contentManager, "HI-", Color.White,
                GameConstants.TILE_WIDTH * 5 + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH));

            text.Add(new StringSprite(contentManager, "1 PLAYER", Color.White,
                GameConstants.TILE_WIDTH * 5 + GameConstants.SMALL_TILE_WIDTH, GameConstants.TILE_WIDTH * 8));
        }

        public void Update(GameTime gameTime)
        {
            elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedGameTime > 40)
            {
                elapsedGameTime = 0;
                if (changeSprite)
                {
                    sourceRectangle.X = GameConstants.TILE_WIDTH * (6 + 1);
                    changeSprite = false;
                }
                else
                {
                    sourceRectangle.X = GameConstants.TILE_WIDTH * 6;
                    changeSprite = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }

            spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, Color.White);

            foreach (StringSprite txt in text)
            {
                txt.Draw(spriteBatch);
            }

            player1recordSprite.Draw(spriteBatch);
            hiRecordSprite.Draw(spriteBatch);
        }
    }
}
