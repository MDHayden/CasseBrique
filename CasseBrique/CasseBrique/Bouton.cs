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
        
        private Color _color = new Color(255, 255, 255, 255);

        private Texture2D _texture;
        private Vector2 _position;
        private Rectangle _rectangle;

        private bool down;
        public bool isClicked;

        public Bouton(Texture2D texture, GraphicsDevice graphics)
        {
            _texture = texture;

            Vector2 windowSize = new Vector2(graphics.Viewport.Width, graphics.Viewport.Height);
            //_size = new Vector2(windowSize.X / 8, windowSize.Y / 30);
            _position = new Vector2(windowSize.X / 2 - _texture.Width / 2, windowSize.Y / 3 - _texture.Height);
        }

        public void Update(MouseState mouseState)
        {
            _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);

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

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, null, _color);
        }        
    }
}
