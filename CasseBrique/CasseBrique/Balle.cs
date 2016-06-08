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
    public struct Circle
    {
        private Vector2 _center;
        private float _radius;

        public Vector2 Center {
            get { return _center; }
            set { _center = value; } 
        }
        public float Radius {
            get { return _radius; }
            set { _radius = value; }
        }

        public Circle(Vector2 center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        public bool Contains(Vector2 point)
        {
            // "Wordy" version
            Vector2 relativePosition = point - Center;
            float distanceBetweenPoints = relativePosition.Length();
            if (distanceBetweenPoints <= Radius) { return true; }
            else { return false; }
            // Concise version
            //return ((point - Center).Length() <= Radius);
        }

        public bool Intersects(Circle other)
        {
            // "Wordy" version
            Vector2 relativePosition = other.Center - this.Center;
            float distanceBetweenCenters = relativePosition.Length();
            if (distanceBetweenCenters <= this.Radius + other.Radius) { return true; }
            else { return false; }
            //return ((other.Center - Center).Length() < (other.Radius - Radius)); // Concise version
        }

        public bool Intersects(Rectangle rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(_center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(_center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = _center - v;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < _radius * _radius));
        }
    }

    class Balle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private SpriteBatch _spriteBatch;

        private Vector2 _windowSize;

        public Texture2D Texture
        {
            get { return _texture; }
        }
        private Texture2D _texture;

        
        public Vector2 Position {
            get { return _position; }
            set { _position = value; }
        }
        private Vector2 _position;

        private Vector2 _positionInitiale;

        public Vector2 Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        private Vector2 _direction;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }
        private float _speed;

        public const float MAX_SPEED = 0.4f;

        public bool Launched
        {
            get { return _isLaunched; }
            set { _isLaunched = value; }
        }
        private bool _isLaunched;

        public Circle CollisionCircle
        {
            get
            {
                return _circle;
            }
        }
        private Circle _circle;

        public Balle(Game game, Vector2 windowSize/*, SpriteBatch spriteBatch*/)
            : base(game)
        {
            _windowSize = windowSize;
            //_spriteBatch = spriteBatch;
            this.Game.Components.Add(this);
        }

        public override void Initialize()
        {
            _isLaunched = false;
            _speed = 0.1f;
            _direction = this.generateLaunchDirection();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _texture = Game.Content.Load<Texture2D>(@"images\balle-bleu");

            _circle = new Circle(
                new Vector2(
                    _position.X + _texture.Width / 2,
                    _position.Y + _texture.Height / 2
                ),
                _texture.Width / 2
            );

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(((CasseBrique)Game).CurrentGameState == CasseBrique.GameState.Playing)
            {
                if (_isLaunched == true)
                {
                    //SoundEffectInstance soundInstMur = soundMur.CreateInstance();
                    //soundInstMur.Volume = 0.6f; // Pour le son
                    //soundInstMur.Play();

                    // Collision Gauche/Droite :
                    if ((_direction.X < 0 && _position.X <= 0) || (_direction.X > 0 && _position.X + _texture.Width >= _windowSize.X))
                    {
                        _direction = new Vector2(-_direction.X, _direction.Y);
                    }
                    // Collision Haut :
                    if ((_position.Y <= 0 && _direction.Y < 0) /*|| (_position.Y > _windowSize.Y - _texture.Height && _direction.Y > 0)*/)
                    {
                        _direction = new Vector2(_direction.X, -_direction.Y);
                    }

                    _position += _direction * _speed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                _circle.Center = new Vector2(_position.X + _texture.Width / 2,
                    _position.Y + _texture.Height / 2);
                base.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (((CasseBrique)Game).CurrentGameState == CasseBrique.GameState.Playing)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_texture, _position, Color.White);
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private Vector2 generateLaunchDirection()
        {
            Random rand = new Random();
            int result = rand.Next(1, 3); // Borne de fin exclue
            if (result % 2 != 0) // res = 1
                return new Vector2(-1, -1);
            else return new Vector2(1, -1); // res = 2
        }
    }
}
