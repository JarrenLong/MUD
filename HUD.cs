namespace MUD
{
  /// <summary>
  /// Head's-up display
  /// </summary>
  public class HUD : RenderArea
  {
    private string Message = "";

    public HUD(ConsoleWindow wnd) : base(wnd)
    {
      Rectangle ws = Window.WindowSize;

      BufferBounds = new Rectangle(0, 0, ws.Width, ws.Height / 4);
      RenderBounds = BufferBounds;
      WindowOffset = new Rectangle(0, (ws.Height / 4) * 3, ws.Width, ws.Height / 4);

      RenderOrder = 1;
    }

    public void ShowMessage(string msg)
    {
      Message = "";
    }

    public override void Update()
    {
      Buffer = new char[BufferBounds.Height, BufferBounds.Width];

      for (int y = 0; y < BufferBounds.Height; y++)
      {
        for (int x = 0; x < BufferBounds.Width; x++)
        {
          if (x == 0 && y == 0)
            Buffer[y, x] = '*'; // Top left
          else if (x == BufferBounds.Width - 1 && y == 0)
            Buffer[y, x] = '*'; // Top right
          else if (x == 0 && y == BufferBounds.Height - 1)
            Buffer[y, x] = '*'; // Bottom left
          else if (x == BufferBounds.Width - 1 && y == BufferBounds.Height - 1)
            Buffer[y, x] = '*'; // Bottom right
          else if (y == 0 || y == BufferBounds.Height - 1)
            Buffer[y, x] = '-'; // Top or bottom lines
          else if (x == 0 || x == BufferBounds.Width - 1)
            Buffer[y, x] = '|'; // Left or right of area
          else
          {
            // Body area
            // TODO: render HUD in here
            Buffer[y, x] = ' ';
          }
        }
      }

      // TODO: Print this inside the box, account for wrapping
      //Buffer.AddRange(Message);
      Message = "";
    }
  }
}
