using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace trickortreat;

enum Scenes
{
    MENU,
    ROOM,
    LOBBY,
    GAME
}

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SceneManager sceneManager;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        sceneManager = new();
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        sceneManager.AddScene(new Menu(GraphicsDevice, Content, sceneManager), "menu");
        sceneManager.AddScene(new Room(GraphicsDevice, Content, sceneManager), "room");
        sceneManager.AddScene(new Lobby(GraphicsDevice, Content, sceneManager), "lobby");
        sceneManager.ChangeScene("menu");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        sceneManager.GetCurrentScene().Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        sceneManager.GetCurrentScene().Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
