using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using trickortreat;

public class Room : IScene
{
    private KeyboardState previousKeyboardState;

    private readonly GraphicsDevice graphicsDevice;
    private readonly ContentManager contentManager;
    private readonly SceneManager sceneManager;

    private bool _Blinking = false;
    private bool _Select = false;

    private int _Selected = 0;

    private double _BlinkCountdown = 0;
    private double _KeysCountdown = 500;

    private string _Ip = "";

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

    public async void Update(GameTime gameTime)
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

        if(state.IsKeyDown(Keys.Enter) && _KeysCountdown <= 0 && !_Select)
        {
            _Select = true;
            _KeysCountdown = 500;
        }

        if(state.IsKeyDown(Keys.K) && _KeysCountdown <= 0)
        {
            _Select = false;
            _KeysCountdown = 300;
        }

        if(_Select)
        {
            if(_Selected == 0 && _KeysCountdown <= 0)
            {

                KeyboardState currentKeyboardState = Keyboard.GetState();
                
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {

                    if (previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key))
                    {
                        if (key == Keys.Back && _Ip.Length > 0)
                        {
                            _Ip = _Ip.Substring(0, _Ip.Length - 1);
                        }
                        else if (key == Keys.Enter)
                        {
                            using var client = new TcpClient();

                            await client.ConnectAsync(_Ip, 6967);

                            await using var stream = client.GetStream();
                            using var reader = new StreamReader(stream, Encoding.UTF8);
                            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                            var payload = new {Name = "L4rav3l"};
                            string json = JsonSerializer.Serialize(payload);

                            await writer.WriteLineAsync(json);
                        }
                        else
                        {
                            char bind = '\0';
                            if (key >= Keys.A && key <= Keys.Z)
                                bind = (char)('A' + (key - Keys.A));
                            else if (key >= Keys.D0 && key <= Keys.D9)
                                bind = (char)('0' + (key - Keys.D0));
                            else if (key == Keys.OemPeriod)
                                bind = '.';

                            if (bind != '\0')
                                _Ip += bind;
                        }
                    }
                }

                previousKeyboardState = currentKeyboardState;

            } else {

            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        graphicsDevice.Clear(Color.Gray);

        int Height = graphicsDevice.Viewport.Height;
        int Width = graphicsDevice.Viewport.Width;

        Vector2 Ip = Vector2.Zero;

        Vector2 JoinRoomM = _pixelfont.MeasureString("Join Room");
        Vector2 JoinRoom = new Vector2((int)((Width / 4) * 2) - JoinRoomM.X, (Height / 2) - JoinRoomM.Y);

        Vector2 CreateRoomM = _pixelfont.MeasureString("Create Room");
        Vector2 CreateRoom = new Vector2((int)((Width / 4) * 2) - CreateRoomM.X + 225, (Height / 2 + 50) - CreateRoomM.Y);

        Vector2 IpM = _pixelfont.MeasureString("IP: " + _Ip);
        Ip = new Vector2((int)((Width / 4) * 2) - IpM.X + 225, (Height / 2 + 50) - IpM.Y);

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
            spriteBatch.DrawString(_pixelfont, "IP: " + _Ip, Ip, Color.Orange);
        }

        if(_Selected == 1 && _Select)
        {
            spriteBatch.DrawString(_pixelfont, "Create Room", CreateRoom, Color.Orange);
        }
    }
}