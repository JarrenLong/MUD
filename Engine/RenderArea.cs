namespace MUD
{
  public class RenderArea
  {
    public int RenderOrder { get; set; }
    public bool IsVisibleArea { get; set; }
    public bool Scrollable { get; set; }
    public int ScrollCenterX { get; set; }
    public int ScrollCenterY { get; set; }
    private Rectangle _BufferBounds { get; set; }
    public Rectangle BufferBounds
    {
      get { return _BufferBounds; }
      set
      {
        if ((_BufferBounds == null && value != null) ||
          _BufferBounds.X != value.X ||
          _BufferBounds.Y != value.Y ||
          _BufferBounds.Width != value.Width ||
          _BufferBounds.Height != value.Height)
        {
          _BufferBounds = value;
          Buffer = new char[_BufferBounds.Height, _BufferBounds.Width];
        }
      }
    }
    public Rectangle RenderBounds { get; set; }
    public Rectangle WindowOffset { get; set; }
    public char[,] Buffer { get; set; }
    public ConsoleWindow Window { get; private set; }

    public RenderArea(ConsoleWindow wnd)
    {
      Window = wnd;

      Rectangle ws = Window.WindowSize;

      IsVisibleArea = true;
      BufferBounds = new Rectangle(0, 0, ws.Width, ws.Height);
      RenderBounds = new Rectangle(0, 0, ws.Width, ws.Height);
      WindowOffset = new Rectangle(0, 0, ws.Width, ws.Height);

      Window.Buffers.Add(this);
    }

    public char[,] Print()
    {
      // Update the buffer for this render area
      Update();

      // Grab all data from the buffer that is within the rendering window and return it
      char[,] toPrint = GetRegion(Buffer, BufferBounds, RenderBounds);

      // Now, the render area needs to be translated into a buffer the same size as the window
      toPrint = SetRegion(toPrint, RenderBounds, Window.WindowSize, WindowOffset);

      // return the formatted area to print
      return toPrint;
    }

    public virtual void Update() { }

    public static char[,] GetRegion(char[,] orig, Rectangle origBounds, Rectangle regionToGet)
    {
      char[,] ret = new char[regionToGet.Height, regionToGet.Width];

      if (orig == null)
        return ret;

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

      if (orig == null)
        return ret;

      for (int y = 0; y < origBounds.Height; y++)
        for (int x = 0; x < origBounds.Width; x++)
          if (x + offset.X >= 0 && x + offset.X < newBounds.Width - newBounds.X &&
            y + offset.Y >= 0 && y + offset.Y < newBounds.Height - newBounds.Y)
            ret[y + offset.Y, x + offset.X] = orig[y, x];

      return ret;
    }
  }
}
