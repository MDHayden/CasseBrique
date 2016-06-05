using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CasseBrique
{
    class Brique
    {
        public bool Touched
        {
            get { return _touched; }
            set { _touched = value; }
        }
        private bool _touched;

        public Vector2 Position
        {
            get { return _position; }
        }
        private Vector2 _position;

        public int Color
        {
            get { return _color; }
        }
        private int _color;

        public int Score
        {
            get { return _score; }
        }
        private int _score;

        public Vector2 Size
        {
            get { return _size; }
        }
        private Vector2 _size;

        public Rectangle CollisionRectangle
        {
            get
            {
                return new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y);
            }
        }

        public Brique(Vector2 position, Vector2 size, int color, int score)
        {
            _position = position;
            _color = color;
            _score = score;
            _size = size;
            _touched = false;
        }
    }
}
