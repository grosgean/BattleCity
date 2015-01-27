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
    class EndGameScreen
    {
        Texture2D tilesSprite;

        List<Tile> tiles = new List<Tile>();

        int[,] menuScreenTilesContent = { 
            {8,7,3,1,8,7,13,0,15,4,14,5,10,7,3,1},
            {15,0,12,4,15,0,10,5,15,15,15,5,10,13,12,0},
            {11,4,10,5,15,3,11,5,15,2,10,5,10,5,0,0},
            {0,3,3,1,3,0,2,1,3,0,2,1,2,3,3,1},

            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},

            {8,12,12,0,12,0,8,4,8,12,12,4,12,12,12,0},
            {15,0,10,5,15,0,10,5,10,5,0,0,15,0,10,5},
            {15,0,10,5,11,13,15,1,10,7,3,0,15,12,7,1},
            {11,12,14,1,0,11,1,0,10,13,12,4,15,2,15,4},
        };

        public EndGameScreen(ContentManager contentManager)
        {
            tilesSprite = contentManager.Load<Texture2D>("minitiles");

            for (int row = 0; row < menuScreenTilesContent.GetLength(0); row++)
            {
                for (int col = 0; col < menuScreenTilesContent.GetLength(1); col++)
                {
                    tiles.Add(new Tile(tilesSprite, 3, 3, menuScreenTilesContent[row, col], col+1, row+1, false));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
