using System.Collections.Generic;
using System.IO;

namespace MUD
{
  public interface IMap
  {
    char[,] OriginalMap { get; set; }
  }

  public class Map : RenderArea, IMap
  {
    public char[,] OriginalMap { get; set; }

    public Map(ConsoleWindow wnd) : base(wnd)
    {
      Rectangle ws = Window.WindowSize;

      RenderBounds = new Rectangle(0, 0, ws.Width, (ws.Height / 4) * 3);

      RenderOrder = 0;

      wnd.Map = this;
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
          if (w == Player.Character)
          {
            Window.Player.X = cc;
            Window.Player.Y = rr;
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
    }
  }
}
