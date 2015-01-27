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

    public class Tile
    {
        Texture2D tilesSprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        bool active = true;
        int tileNumber;
        bool crossable = false;
        bool eagleArmor;


        // левый верхний угол smallTiles
        Vector2[] smallTiles = {
                                new Vector2(0,0),   //0
                                new Vector2(1,0),   //1
                                new Vector2(2,0),   //2
                                new Vector2(3,0),   //3
                                new Vector2(0,1),   //4
                                new Vector2(1,1),   //5
                                new Vector2(2,1),   //6
                                new Vector2(3,1),   //7
                                new Vector2(0,2),   //8
                                new Vector2(1,2),   //9
                                new Vector2(2,2),   //10
                                new Vector2(3,2),   //11
                                new Vector2(0,3),   //12
                                new Vector2(1,3),   //13
                                new Vector2(2,3),   //14
                                new Vector2(3,3),   //15
                                new Vector2(0,4),   //16
                                new Vector2(1,4),   //17
                                new Vector2(2,4),   //18
                                new Vector2(3,4),   //19

                                new Vector2(0,5),   //20    // орел
                                new Vector2(1,5),   //21
                                new Vector2(2,5),   //22
                                new Vector2(3,5),   //23
                                new Vector2(0,6),   //24
                                new Vector2(1,6),   //25
                                new Vector2(2,6),   //26
                                new Vector2(3,6),   //27

                                new Vector2(0,7),   //28    // вода, второй спрайт

                               };

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the Tile
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
            set { drawRectangle = value; }
        }

        public int TileNumber
        {
            get { return tileNumber; }
            set { tileNumber = value; }
        }

        public bool Crossable
        {
            get { return crossable; }
            set { crossable = value; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public bool EagleArmor
        {
            get { return eagleArmor; }
        }

        #endregion



        /// <summary>
        /// Создает tile размером SMALL_TILE_WIDTH
        /// </summary>
        /// <param name="tilesSprite"></param>
        /// <param name="xShiftBig"></param>
        /// <param name="yShiftBig"></param>
        /// <param name="tile"></param>
        /// <param name="xShiftSmall"></param>
        /// <param name="yShiftSmall"></param>
        public Tile(Texture2D tilesSprite, int xShiftBig, int yShiftBig, float tile, int xShiftSmall, int yShiftSmall, bool eagleArmor)
        {
            this.tilesSprite = tilesSprite;
            this.eagleArmor = eagleArmor;
            this.tileNumber = (int)tile;
            if (tileNumber == 0 || tileNumber == 18 || tileNumber == 19)
            {
                crossable = true;
            }

            drawRectangle = new Rectangle(xShiftBig * GameConstants.TILE_WIDTH + xShiftSmall * GameConstants.SMALL_TILE_WIDTH,
                                            yShiftBig * GameConstants.TILE_WIDTH + yShiftSmall * GameConstants.SMALL_TILE_WIDTH,
                                            GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);

            sourceRectangle = new Rectangle((int)smallTiles[tileNumber].X * GameConstants.SMALL_TILE_WIDTH, (int)smallTiles[tileNumber].Y * GameConstants.SMALL_TILE_WIDTH,
                GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);
        }

        public void TileChangeState(int tile)
        {
            sourceRectangle = new Rectangle((int)smallTiles[tile].X * GameConstants.SMALL_TILE_WIDTH, (int)smallTiles[tile].Y * GameConstants.SMALL_TILE_WIDTH,
                GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);
            tileNumber = tile;

            if (tileNumber == 0 && !eagleArmor) active = false;

            if (tileNumber == 0 && eagleArmor) crossable = true;
        }

        public void TileChangeStateEagleArmor(bool armor)
        {
            tileNumber = armor ? 16 : 15;

            sourceRectangle = new Rectangle((int)smallTiles[tileNumber].X * GameConstants.SMALL_TILE_WIDTH, (int)smallTiles[tileNumber].Y * GameConstants.SMALL_TILE_WIDTH,
                GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);

            crossable = false;
        }

        public void WaterChangeState(bool waterChangeSprite)
        {
            if (waterChangeSprite == true)
            {
                sourceRectangle.X = (int)smallTiles[28].X * GameConstants.SMALL_TILE_WIDTH;
                sourceRectangle.Y = (int)smallTiles[28].Y * GameConstants.SMALL_TILE_WIDTH;
            }
            else
            {
                sourceRectangle.X = (int)smallTiles[17].X * GameConstants.SMALL_TILE_WIDTH;
                sourceRectangle.Y = (int)smallTiles[17].Y * GameConstants.SMALL_TILE_WIDTH;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tilesSprite, drawRectangle, sourceRectangle, Color.White);
        }
    }
}
