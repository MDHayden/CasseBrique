using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CasseBrique
{
    class Bouton
    {
     
        public Color Color
        {
            get { return _color; }
        }
        private Color _color;

        public Texture2D Texture
        {
            get { return _texture; }
        }
        private Texture2D _texture;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _position;

        public Rectangle Rectangle
        {
            get { return _rectangle; }
        }
        private Rectangle _rectangle;

        private bool down;
        public bool isClicked;

        public Bouton(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            _color = new Color(255, 255, 255, 255);
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        public void Update(MouseState mouseState)
        {
            Rectangle mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            if(mouseRectangle.Intersects(_rectangle))
            {
                if(_color.A == 255) down = false;
                if(_color.A == 0) down = true;
                if(down) _color.A += 3; else _color.A -= 3;
                if(mouseState.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else if(_color.A < 255)
            {
                _color.A += 3;
                isClicked = false;
            }
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(_texture, _rectangle, null, _color);
        //}        
    }
}
