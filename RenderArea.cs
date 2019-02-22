using System.Collections.Generic;

namespace MUD
{
  public class RenderArea
  {
    public int RenderOrder { get; set; }
    public bool Scrollable { get; set; }
    public int ScrollCenterX { get; set; }
    public int ScrollCenterY { get; set; }
    public Rectangle BufferBounds { get; set; }
    public Rectangle RenderBounds { get; set; }
    public List<char> Buffer { get; }

    public RenderArea(ConsoleWindow wnd)
    {
      var ws = wnd.WindowSize;

      BufferBounds = new Rectangle(ws.X, ws.Y, ws.Width, ws.Height);
      RenderBounds = new Rectangle(ws.X, ws.Y, ws.Width, ws.Height);

      Buffer = new List<char>();

      wnd.Buffers.Add(this);
    }

    public List<char> Print(ConsoleWindow wnd)
    {
      // Update the buffer for this render area
      Update();

      List<char> toPrint = new List<char>();

      // Grab all data from the buffer that is within the rendering window and return it
      for (int y = 0; y < BufferBounds.Height; y++)
      {
        for (int x = 0; x < BufferBounds.Width; x++)
        {
          if (x >= RenderBounds.X && x <= RenderBounds.Width &&
            y >= RenderBounds.Y && y <= RenderBounds.Height)
          {
            toPrint.Add(Buffer[y * BufferBounds.Width + x]);
          }
        }
      }

      // Now, the render area needs to be translated into a buffer the same size as the window
      List<char> ret = new List<char>();
      for (int y = 0; y < wnd.WindowSize.Height; y++)
      {
        for (int x = 0; x < wnd.WindowSize.Width; x++)
        {
          if (x >= RenderBounds.X && x <= RenderBounds.Width &&
            y >= RenderBounds.Y && y <= RenderBounds.Height)
            ret.Add(toPrint[y * wnd.WindowSize.Width + x]);
          else
            ret.Add(' ');
        }
      }

      return ret;
    }

    public virtual void Update() { }
  }
}
