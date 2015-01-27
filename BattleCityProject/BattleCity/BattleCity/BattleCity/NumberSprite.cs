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
    class NumberSprite
    {
        Texture2D scoreSprite;
        List<NumberDigit> digits = new List<NumberDigit>();

        TextDirection direction;
        Color color;
        int xPosition;
        int yPosition;
        int numberValue;

        public int NumberValue
        {
            get { return numberValue; }
        }

        /// <summary>
        /// создает графическое представление inputNumber 
        /// </summary>
        /// <param name="xPosition">in pixels</param>
        /// <param name="yPosition">in pixels</param>
        public NumberSprite(ContentManager contentManager, int inputNumber, TextDirection direction, Color color, int xPosition, int yPosition)
        {
            scoreSprite = contentManager.Load<Texture2D>("scoreSprite");

            this.direction = direction;
            this.color = color;
            this.xPosition = xPosition;
            this.yPosition = yPosition;
            numberValue = inputNumber;

            UpdateNumber(inputNumber);
            
        }

        public void UpdateNumber(int inputNumber)
        {
            digits.Clear();
            numberValue = inputNumber;

            int numberLength = inputNumber.ToString().Length;
            int numberLengthDef = inputNumber.ToString().Length;

            int xShift;
            int digit;

            while (numberLength > 0)
            {
                numberLength--;

                xShift = direction == TextDirection.Left ? numberLength : numberLength - numberLengthDef;

                digit = inputNumber % 10;
                inputNumber = inputNumber / 10; // в number оставляем то, что осталось
                digits.Add(new NumberDigit(scoreSprite, Convert.ToString(digit), color,
                    (xPosition + xShift * GameConstants.SMALL_TILE_WIDTH), yPosition));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (NumberDigit digit in digits)
            {
                digit.Draw(spriteBatch);
            }
        }
    }

    class StringSprite
    {
        List<NumberDigit> digits = new List<NumberDigit>();

        /// <summary>
        /// создает графическое представление inputString
        /// </summary>
        /// <param name="xPosition">in pixels</param>
        /// <param name="yPosition">in pixels</param>
        public StringSprite(ContentManager contentManager, string inputString, Color color, int xPosition, int yPosition)
        {
            Texture2D scoreSprite = contentManager.Load<Texture2D>("scoreSprite");

            for (int i = 0; i < inputString.Length; i++)
            {
                digits.Add(new NumberDigit(scoreSprite, inputString[i].ToString(), color,
                    (i * GameConstants.SMALL_TILE_WIDTH + xPosition), yPosition));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (NumberDigit digit in digits)
            {
                digit.Draw(spriteBatch);
            }
        }
    }

    class NumberDigit
    {
        Texture2D sprite;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;
        Color color;

        public NumberDigit(Texture2D scoreSprite, string digit, Color digitColor, int xPosition, int yPosition)
        {
            sprite = scoreSprite;
            color = digitColor;

            drawRectangle = new Rectangle(
                xPosition, yPosition,
                GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);
            sourceRectangle = new Rectangle(
                GameConstants.SMALL_TILE_WIDTH * (int)GameData.CharPosition[digit].X,
                GameConstants.SMALL_TILE_WIDTH * (int)GameData.CharPosition[digit].Y,
                GameConstants.SMALL_TILE_WIDTH, GameConstants.SMALL_TILE_WIDTH);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, sourceRectangle, color);
        }
    }
}
