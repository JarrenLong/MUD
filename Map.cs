using System.Collections.Generic;
using System.IO;

namespace MUD
{
  public class Map : RenderArea
  {
    public const char Character = 'X';
    public int X { get; private set; }
    public int Y { get; private set; }
    private char[,] OriginalMap;

    public Map(ConsoleWindow wnd) : base(wnd)
    {
      Rectangle ws = wnd.WindowSize;

      RenderBounds = new Rectangle(0, 0, ws.Width, (ws.Height / 4) * 3);

      RenderOrder = 0;
    }

    public void Load(string mapFile)
    {
      if (!File.Exists(mapFile))
        return;

      List<List<char>> map = new List<List<char>>();

      string[] lines = File.ReadAllLines(mapFile);
      foreach (string r in lines)
      {
        List<char> buf = new List<char>();
        foreach (char c in r)
          buf.Add(c);
        map.Add(buf);

        if (BufferBounds.Width <= buf.Count)
          BufferBounds.Width = buf.Count;
      }
      BufferBounds.Height = map.Count;

      OriginalMap = new char[BufferBounds.Height, BufferBounds.Width];

      int rr = 0, cc = 0;
      foreach (List<char> h in map)
      {
        cc = 0;
        foreach (char w in h)
        {
          OriginalMap[rr, cc] = w;

          // Start character position
          if (w == Character)
          {
            X = cc;
            Y = rr;
            OriginalMap[rr, cc] = ' ';
          }

          cc++;
        }
        rr++;
      }
    }

    public override void Update()
    {
      // Create a full copy of the map
      Buffer = SetRegion(OriginalMap, BufferBounds, BufferBounds, BufferBounds);

      // Set the player position
      Buffer[Y, X] = Character;
    }

    public enum Direction
    {
      Up,
      Down,
      Left,
      Right
    }

    public void Move(Direction dir)
    {
      int newX = X, newY = Y;

      switch (dir)
      {
        case Direction.Up:
          newY--;
          break;
        case Direction.Down:
          newY++;
          break;
        case Direction.Left:
          newX--;
          break;
        case Direction.Right:
          newX++;
          break;
      }

      // Bounds check
      if (newX < 0 || newX >= BufferBounds.Width || newY < 0 || newY >= BufferBounds.Height)
      {
        //ShowMessage("You can't move that direction!");
        return;
      }

      // Solid object check (can't walk through them)
      if (OriginalMap[newY, newX] == '#')
      {
        //ShowMessage("That's a wall, and you're not Bobby.");
        return;
      }

      // Move the render window for the scrolling effect
      RenderBounds.X += newX - X;
      RenderBounds.Y += newY - Y;

      // TODO: Item check, something to pick up?

      X = newX;
      Y = newY;
    }
  }
}
