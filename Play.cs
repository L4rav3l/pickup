using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace trickortreat;

public class Play : IScene
{
    private readonly ContentManager contentManager;
    private readonly GraphicsDevice graphicsDevice;
    private readonly SceneManager sceneManager;

    private float _speed = 5f;
    private int _score = 0;
    private int _screen = 0;
    private double _time = 180000;
    private bool[] _visible = new bool[6];
    private bool[] _visiblemush = new bool[6];
    private bool grapeinhand = false;
    private bool mushinhand = false;
    private SpriteFont _pixelfont;
    private Vector2 _position;
    private Vector2[] _grapesPos = new Vector2[6];
    private Vector2[] _mushroomPos = new Vector2[6];

    private Texture2D _grassTexture;
    private int[,] _map;
    private int _tileSize = 64;

    private Texture2D _grapesTexture;
    private Texture2D _mushroomTexture;
    private Texture2D _grapesHeap;
    private Texture2D _mushroomHeap;
    private Texture2D _player;

    public Play(GraphicsDevice graphicsDevice, ContentManager content, SceneManager sceneManager)
    {
        this.graphicsDevice = graphicsDevice;
        this.contentManager = content;
        this.sceneManager = sceneManager;

        Random random = new Random();

        for(int i = 0; i < 6; i++)
        {
            _mushroomPos[i].X = random.Next(graphicsDevice.Viewport.Width);
            _mushroomPos[i].Y = random.Next(graphicsDevice.Viewport.Height);

            if(_mushroomPos[i].X < 100)
            {
                _mushroomPos[i].X += 100;
            }

            if(_mushroomPos[i].X > graphicsDevice.Viewport.Width - 100)
            {
                _mushroomPos[i].X -= 100;
            }

            if(_mushroomPos[i].Y < 100)
            {
                _mushroomPos[i].Y += 100;
            }

            if(_mushroomPos[i].Y > graphicsDevice.Viewport.Height - 100)
            {
                _mushroomPos[i].Y -= 100;
            }

            _visiblemush[i] = true;
        }

        for(int i = 0; i < 6; i++)
        {
            _grapesPos[i].X = random.Next(graphicsDevice.Viewport.Width);
            _grapesPos[i].Y = random.Next(graphicsDevice.Viewport.Height);

            if(_grapesPos[i].X < 100)
            {
                _grapesPos[i].X += 100;
            }

            if(_grapesPos[i].X > graphicsDevice.Viewport.Width - 100)
            {
                _grapesPos[i].X -= 100;
            }

            if(_grapesPos[i].Y < 100)
            {
                _grapesPos[i].Y += 100;
            }

            if(_grapesPos[i].Y > graphicsDevice.Viewport.Height - 100)
            {
                _grapesPos[i].Y -= 100;
            }

            _visible[i] = true;
        }

    }

    private void InitializeMap()
    {
        
    int tileSize = 64;
    int width = (int)Math.Ceiling(graphicsDevice.Viewport.Width / (float)tileSize);
    int height = (int)Math.Ceiling(graphicsDevice.Viewport.Height / (float)tileSize);

        _map = new int[height, width];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                _map[y, x] = 0;
    }

    public void LoadContent()
    {
        _pixelfont = contentManager.Load<SpriteFont>("pixelfont");
        _grassTexture = contentManager.Load<Texture2D>("grass");
        _grapesTexture = contentManager.Load<Texture2D>("grapes");
        _mushroomTexture = contentManager.Load<Texture2D>("mushroom");
        _grapesHeap = contentManager.Load<Texture2D>("grapes_heap");
        _mushroomHeap = contentManager.Load<Texture2D>("mushroom_heap");
        _player = contentManager.Load<Texture2D>("player");
        InitializeMap();
    }

    public void Update(GameTime gameTime)
    {
        if(_screen == 0)
        {
        Vector2 movement = Vector2.Zero;
        KeyboardState inputs = Keyboard.GetState();
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 1000;

        if(_time >= 0)
        {
            _time -= elapsed;
        }

        if(_time <= 0)
        {
            _screen = 1;
        }


        if (inputs.IsKeyDown(Keys.W)) movement.Y -= 1;
        if (inputs.IsKeyDown(Keys.S)) movement.Y += 1;
        if (inputs.IsKeyDown(Keys.A)) movement.X -= 1;
        if (inputs.IsKeyDown(Keys.D)) movement.X += 1;

        if (movement != Vector2.Zero)
        {
            movement.Normalize();
            movement *= _speed;
        }

        if(inputs.IsKeyDown(Keys.E))
        {
            if(grapeinhand || mushinhand)
            {

                if(grapeinhand)
                {
                    if(Vector2.Distance(_position, new Vector2(graphicsDevice.Viewport.Width / 4, graphicsDevice.Viewport.Height / 2)) < 160)
                    {
                        _score++;

                        for(int i = 0; i < 6; i++)
                        {
                                if(_visible[i] == false)
                                {
                                    Random random = new Random();

                                    _grapesPos[i].X = random.Next(graphicsDevice.Viewport.Width);
                                    _grapesPos[i].Y = random.Next(graphicsDevice.Viewport.Height);

                                    if(_grapesPos[i].X < 100)
                                    {
                                        _grapesPos[i].X += 100;
                                    }

                                    if(_grapesPos[i].X > graphicsDevice.Viewport.Width - 100)
                                    {
                                        _grapesPos[i].X -= 100;
                                    }

                                    if(_grapesPos[i].Y < 100)
                                    {
                                        _grapesPos[i].Y += 100;
                                    }

                                    if(_grapesPos[i].Y > graphicsDevice.Viewport.Height - 100)
                                    {
                                        _grapesPos[i].Y -= 100;
                                    }

                                    _visible[i] = true;
                                }
                            }
                        

                        grapeinhand = false;
                    }
                }

                if(mushinhand)
                {
                    if(Vector2.Distance(_position, new Vector2(graphicsDevice.Viewport.Width / 4 * 3, graphicsDevice.Viewport.Height / 2)) < 160)
                    {
                        _score++;

                        for(int i = 0; i < 6; i++)
                        {
                                if(_visiblemush[i] == false)
                                {
                                    Random random = new Random();

                                    _mushroomPos[i].X = random.Next(graphicsDevice.Viewport.Width);
                                    _mushroomPos[i].Y = random.Next(graphicsDevice.Viewport.Height);

                                    if(_mushroomPos[i].X < 100)
                                    {
                                        _mushroomPos[i].X += 100;
                                    }

                                    if(_mushroomPos[i].X > graphicsDevice.Viewport.Width - 100)
                                    {
                                        _mushroomPos[i].X -= 100;
                                    }

                                    if(_mushroomPos[i].Y < 100)
                                    {
                                        _mushroomPos[i].Y += 100;
                                    }

                                    if(_mushroomPos[i].Y > graphicsDevice.Viewport.Height - 100)
                                    {
                                        _mushroomPos[i].Y -= 100;
                                    }

                                    _visiblemush[i] = true;
                                }
                            }
                                mushinhand = false;
                        }
                    }
                }

                for(int i = 0; i < 6; i++)
                {
                
                    if(64 > Vector2.Distance(_position, _grapesPos[i]) && _visible[i])
                    {
                        if(!grapeinhand && !mushinhand) {
                        _visible[i] = false;
                        grapeinhand = true;
                        }
                    }
                }
                    for(int i = 0; i < 6; i++)
                    {
                        if(64 > Vector2.Distance(_position, _mushroomPos[i]) && _visiblemush[i])
                        {
                            if(!grapeinhand && !mushinhand) {
                            _visiblemush[i] = false;
                            mushinhand = true;
                            }
                        }
                    }
                }
        

        _position += movement;
        }
        
        if(_screen == 1)
        {
            KeyboardState inputs = Keyboard.GetState();

            if(inputs.IsKeyDown(Keys.E))
            {
                sceneManager.ChangeScene("menu");
            } 
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if(_screen == 0)
        {
        
        graphicsDevice.Clear(Color.Gray);

        for (int y = 0; y < _map.GetLength(0); y++)
        {
            for (int x = 0; x < _map.GetLength(1); x++)
            {
                Vector2 position = new Vector2(x * _tileSize, y * _tileSize);
                spriteBatch.Draw(_grassTexture, position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
            }
        }


        for(int i = 0; i < 6; i++)
        {
            if(_visible[i])
            {
                spriteBatch.Draw(_grapesTexture, _grapesPos[i], null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
            }
        }

        for(int i = 0; i < 6; i++)
        {
            if(_visiblemush[i])
            {
                spriteBatch.Draw(_mushroomTexture, _mushroomPos[i], null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
            }
        }

        if(grapeinhand)
        {
            spriteBatch.Draw(_grapesTexture, new Vector2(_position.X + 30, _position.Y + 30), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
        }

        if(mushinhand)
        {
            spriteBatch.Draw(_mushroomTexture, new Vector2(_position.X + 30, _position.Y + 30), null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.4f);
        }
        
        spriteBatch.Draw(_grapesHeap, new Vector2(graphicsDevice.Viewport.Width / 4, graphicsDevice.Viewport.Height / 2), null, Color.White, 0f, new Vector2(_grapesHeap.Width / 2f, _grapesHeap.Height / 2f), 1f, SpriteEffects.None, 0.2f);
        spriteBatch.Draw(_mushroomHeap, new Vector2(graphicsDevice.Viewport.Width / 4 * 3, graphicsDevice.Viewport.Height / 2), null, Color.White, 0f, new Vector2(_mushroomHeap.Width / 2f, _mushroomHeap.Height / 2f), 0.5f, SpriteEffects.None, 0.2f);

        spriteBatch.DrawString(_pixelfont, _score.ToString(), new Vector2(graphicsDevice.Viewport.Width / 2, 100), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);
        
        spriteBatch.DrawString(_pixelfont, Math.Ceiling(_time / 1000).ToString(), new Vector2(graphicsDevice.Viewport.Width / 2, 150), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.4f);

        int Height = graphicsDevice.Viewport.Height;
        int Width = graphicsDevice.Viewport.Width;

        spriteBatch.Draw(_player, _position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.3f);
        }

        if(_screen == 1)
        {
            graphicsDevice.Clear(Color.Gray);

            int height = graphicsDevice.Viewport.Height;
            int width = graphicsDevice.Viewport.Width;

            Vector2 congrat = new Vector2(width / 2 - 900, height / 2);

            spriteBatch.DrawString(_pixelfont, $"Congratulations! You brought {_score} mushrooms and grapes in 180 seconds.", congrat, Color.Orange);

            Vector2 Press = new Vector2(width / 2, height / 2 + 50);

            spriteBatch.DrawString(_pixelfont, "Press E key.", Press, Color.Orange);
        }
    }
}
