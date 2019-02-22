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
    public Rectangle WindowOffset { get; set; }
    public char[,] Buffer { get; set; }

    public RenderArea(ConsoleWindow wnd)
    {
      var ws = wnd.WindowSize;

      BufferBounds = new Rectangle(0, 0, ws.Width, ws.Height);
      RenderBounds = new Rectangle(0, 0, ws.Width, ws.Height);
      WindowOffset = new Rectangle(0, 0, ws.Width, ws.Height);

      wnd.Buffers.Add(this);
    }

    public char[,] Print(ConsoleWindow wnd)
    {
      // Update the buffer for this render area
      Update();

      // Grab all data from the buffer that is within the rendering window and return it
      var toPrint = GetRegion(Buffer, BufferBounds, RenderBounds);

      // Now, the render area needs to be translated into a buffer the same size as the window
      toPrint = SetRegion(toPrint, RenderBounds, wnd.WindowSize, WindowOffset);

      //List<char> ret = new List<char>();
      //for (int y = 0; y < wnd.WindowSize.Height; y++)
      //{
      //  for (int x = 0; x < wnd.WindowSize.Width; x++)
      //  {
      //    if (x >= RenderBounds.X && x <= RenderBounds.Width &&
      //      y >= RenderBounds.Y && y <= RenderBounds.Height)
      //      ret.Add(toPrint[y * wnd.WindowSize.Width + x]);
      //    else
      //      ret.Add(' ');
      //  }
      //}

      return toPrint;
    }

    public virtual void Update() { }

    public static char[,] GetRegion(char[,] orig, Rectangle origBounds, Rectangle regionToGet)
    {
      char[,] ret = new char[regionToGet.Height - regionToGet.Y, regionToGet.Width - regionToGet.X];

      int xOffset = regionToGet.X - origBounds.X;
      int yOffset = regionToGet.Y - origBounds.Y;

      // Grab all data from the buffer that is within the rendering window and return it
      for (int y = origBounds.Y; y < origBounds.Height; y++)
      {
        for (int x = origBounds.X; x < origBounds.Width; x++)
        {
          if (x >= regionToGet.X && x < regionToGet.Width &&
            y >= regionToGet.Y && y < regionToGet.Height)
          {
            if (y >= origBounds.Y && y < origBounds.Height &&
              x >= origBounds.X && x < origBounds.Width)
              ret[y + yOffset, x + xOffset] = orig[y, x];
          }
        }
      }

      return ret;
    }

    public static char[,] SetRegion(char[,] orig, Rectangle origBounds, Rectangle newBounds, Rectangle offset)
    {
      char[,] ret = new char[newBounds.Height - newBounds.Y, newBounds.Width - newBounds.X];

      for (int y = 0; y < origBounds.Height; y++)
      {
        for (int x = 0; x < origBounds.Width; x++)
        {
          ret[y + offset.Y, x + offset.X] = orig[y, x];
        }
      }

      return ret;
    }
  }
}
