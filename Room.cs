using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using trickortreat;

public class Room : IScene
{
    private readonly GraphicsDevice graphicsDevice;
    private readonly ContentManager contentManager;
    private readonly SceneManager sceneManager;

    private bool _Blinking = false;
    private bool _Select = false;

    private int _Selected = 0;

    private double _BlinkCountdown = 0;
    private double _KeysCountdown = 500;

    private string _Ip;

    private SpriteFont _pixelfont;
    
    public Room(GraphicsDevice graphicsDevice, ContentManager contentManager, SceneManager sceneManager)
    {
        this.graphicsDevice = graphicsDevice;
        this.contentManager = contentManager;
        this.sceneManager = sceneManager;
    }

    public void LoadContent()
    {
        _pixelfont = contentManager.Load<SpriteFont>("pixelfont");
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds * 1000;

        if(state.IsKeyDown(Keys.Up) && _KeysCountdown <= 0)
        {
            if(_Selected == 0)
            {
                _Selected = 1;
            } else {
                _Selected = 0;
            }

            _KeysCountdown = 300;
        }

        if(state.IsKeyDown(Keys.Down) && _KeysCountdown <= 0)
        {
            if(_Selected == 0)
            {
                _Selected = 1;
            } else {
                _Selected = 0;
            }

            _KeysCountdown = 300;
        }

        if(_Blinking == true && _BlinkCountdown <= 0)
        {
            _Blinking = false;
            _BlinkCountdown = 500;
        }

        if(_Blinking == false && _BlinkCountdown <= 0)
        {
            _Blinking = true;
            _BlinkCountdown = 300;
        }

        if(_BlinkCountdown >= 0)
        {
            _BlinkCountdown -= elapsed;
        }

        if(_KeysCountdown >= 0)
        {
            _KeysCountdown -= elapsed;
        }

        if(state.IsKeyDown(Keys.Enter) && _KeysCountdown <= 0)
        {
            _Select = true;
            _KeysCountdown = 300;
        }

        if(state.IsKeyDown(Keys.K) && _KeysCountdown <= 0)
        {
            _Select = false;
            _KeysCountdown = 300;
        }

        if(_Select)
        {
            if(_Selected == 0)
            {

                KeyboardState text = Keyboard.GetState();

                foreach(Keys key in text.GetPressedKeys())
                {
                    if(text.IsKeyUp(key))
                    {
                        if(key == Keys.Back && _Ip.Length > 0)
                        {
                            _Ip = _Ip.Substring(0, _Ip.Length - 1);
                        } else if (key == Keys.Enter)
                        {
                            _Ip = "";
                        }
                        else
                        {
                        
                        char bind = '\0';

                        if (key >= Keys.A && key <= Keys.Z)
                        {
                            bind = (char)('A' + (key - Keys.A));
                        }

                        if (key >= Keys.D0 && key <= Keys.D9)
                        {
                            bind = (char)('0' + (key - Keys.D0));
                        }

                            _Ip += bind;
                        }
                    }
                }

                _Ip = Console.ReadLine();
                Console.WriteLine(_Ip);
            } else {

            }
        }

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        graphicsDevice.Clear(Color.Gray);

        int Height = graphicsDevice.Viewport.Height;
        int Width = graphicsDevice.Viewport.Width;

        Vector2 JoinRoomM = _pixelfont.MeasureString("Join Room");
        Vector2 JoinRoom = new Vector2((int)((Width / 4) * 2) - JoinRoomM.X, (Height / 2) - JoinRoomM.Y);

        Vector2 CreateRoomM = _pixelfont.MeasureString("Create Room");
        Vector2 CreateRoom = new Vector2((int)((Width / 4) * 2) - CreateRoomM.X + 225, (Height / 2 + 50) - CreateRoomM.Y);
        
        if(_Selected == 0 && !_Select)
        {
            if(_Blinking == false)
            {
            spriteBatch.DrawString(_pixelfont, "Join Room", JoinRoom, Color.Orange);
            }

            spriteBatch.DrawString(_pixelfont, "Create Room", CreateRoom, Color.Orange);
        }

        if(_Selected == 1 && !_Select)
        {
            spriteBatch.DrawString(_pixelfont, "Join Room", JoinRoom, Color.Orange);
            
            if(_Blinking == false)
            {
                spriteBatch.DrawString(_pixelfont, "Create Room", CreateRoom, Color.Orange);
            }
        }

        if(_Selected == 0 && _Select)
        {
            spriteBatch.DrawString(_pixelfont, "Join Room", JoinRoom, Color.Orange);
        }

        if(_Selected == 1 && _Select)
        {
            spriteBatch.DrawString(_pixelfont, "Create Room", CreateRoom, Color.Orange);
        }

    }
}