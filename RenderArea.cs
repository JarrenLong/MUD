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
      char[,] toPrint = GetRegion(Buffer, BufferBounds, RenderBounds);

      // Now, the render area needs to be translated into a buffer the same size as the window
      toPrint = SetRegion(toPrint, RenderBounds, wnd.WindowSize, WindowOffset);

      // return the formatted area to print
      return toPrint;
    }

    public virtual void Update() { }

    public static char[,] GetRegion(char[,] orig, Rectangle origBounds, Rectangle regionToGet)
    {
      char[,] ret = new char[regionToGet.Height, regionToGet.Width];

      int xOffset = origBounds.X - regionToGet.X;
      int yOffset = origBounds.Y - regionToGet.Y;

      // Grab all data from the buffer that is within the rendering window and return it
      for (int y = origBounds.Y; y < origBounds.Height; y++)
        for (int x = origBounds.X; x < origBounds.Width; x++)
          if (x >= regionToGet.X && x < regionToGet.X + regionToGet.Width &&
            y >= regionToGet.Y && y < regionToGet.Y + regionToGet.Height)
            ret[y + yOffset, x + xOffset] = orig[y, x];

      return ret;
    }

    public static char[,] SetRegion(char[,] orig, Rectangle origBounds, Rectangle newBounds, Rectangle offset)
    {
      char[,] ret = new char[newBounds.Height - newBounds.Y, newBounds.Width - newBounds.X];

      for (int y = 0; y < origBounds.Height; y++)
        for (int x = 0; x < origBounds.Width; x++)
          ret[y + offset.Y, x + offset.X] = orig[y, x];

      return ret;
    }
  }
}
