namespace MUD
{
  public interface IHeadsUpDisplay
  {
    void ShowMessage(string msg);
  }

  /// <summary>
  /// Head's-up display
  /// </summary>
  public class HUD : RenderArea, IHeadsUpDisplay
  {
    private string Message = "";

    public HUD(ConsoleWindow wnd) : base(wnd)
    {
      Rectangle ws = Window.WindowSize;

      BufferBounds = new Rectangle(0, 0, ws.Width, ws.Height / 4);
      RenderBounds = BufferBounds;
      WindowOffset = new Rectangle(0, (ws.Height / 4) * 3, ws.Width, ws.Height / 4);

      RenderOrder = 1;

      Window.HUD = this;
    }

    public void ShowMessage(string msg)
    {
      Message = msg;
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

      // Print this inside the box
      // TODO: Account for multiline messages with scrolling text
      if (!string.IsNullOrEmpty(Message))
      {
        string msg = Message;
        int len = msg.Length;
        if (len >= BufferBounds.Width - 4)
        {
          msg = msg.Substring(0, BufferBounds.Width - 4);
          len = msg.Length;
        }

        int leftPad = (BufferBounds.Width - 4 - len) / 2;
        int i = 0;
        for (int y = 0; y < len + leftPad; y++)
        {
          if (y != 0 && y != BufferBounds.Width && y < leftPad)
            Buffer[1, y] = ' ';

          if (y >= leftPad && y < BufferBounds.Width)
          {
            Buffer[1, y] = msg[i];
            i++;
          }
        }

        Message = Message.Substring(len);
      }

      //Buffer.AddRange(Message);
      //Message = "";
    }
  }
}
