using System;

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
      BufferBounds = new Rectangle(0, (wnd.WindowSize.Height / 4) * 3, wnd.WindowSize.Width, wnd.WindowSize.Height / 4);
      RenderBounds = BufferBounds;
    }

    public void ShowMessage(string msg)
    {
      Message = "";
    }

    public override void Update()
    {
      Buffer.Clear();

      for (int y = 0; y < BufferBounds.Height; y++)
      {
        for (int x = 0; x < BufferBounds.Width; x++)
        {
          if (x == 0 && y == 0)
            Buffer.Add('*'); // Top left
          else if (x == BufferBounds.Width - 1 && y == 0)
            Buffer.Add('*'); // Top right
          else if (x == 0 && y == BufferBounds.Height - 1)
            Buffer.Add('*'); // Bottom left
          else if (x == BufferBounds.Width - 1 && y == BufferBounds.Height - 1)
            Buffer.Add('*'); // Bottom right
          else if (y == 0 || y == BufferBounds.Height - 1)
            Buffer.Add('-'); // Top or bottom lines
          else if (x == 0 || x == BufferBounds.Width - 1)
            Buffer.Add('|'); // Left or right of area
          else
          {
            // Body area
            // TODO: render HUD in here
            Buffer.Add(' ');
          }
        }

        if (y < BufferBounds.Height - 1)
          Buffer.AddRange(Environment.NewLine);
      }

      // TODO: Print this inside the box, account for wrapping
      Buffer.AddRange(Message);
      Message = "";
    }
  }
}
