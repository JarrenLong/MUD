using System.Collections.Generic;
using System.IO;

namespace MUD
{
  public class Map : RenderArea
  {
    public Map(ConsoleWindow wnd, string mapFile = "") : base(wnd)
    {
      Rectangle ws = Window.WindowSize;

      RenderBounds = new Rectangle(0, 0, ws.Width, (ws.Height / 4) * 3);

      RenderOrder = 1;

      wnd.Map = this;

      if (!string.IsNullOrEmpty(mapFile))
        Load(mapFile);
    }

    public void Load(string mapFile)
    {
      if (!File.Exists(mapFile))
        return;

      List<List<char>> map = new List<List<char>>();

      string[] lines = File.ReadAllLines(mapFile);
      int width = 0;
      foreach (string r in lines)
      {
        List<char> buf = new List<char>();
        foreach (char c in r)
          buf.Add(c);
        map.Add(buf);

        if (width <= buf.Count)
          width = buf.Count;
      }

      BufferBounds = new Rectangle(0, 0, width, map.Count);

      int rr = 0, cc = 0;
      foreach (List<char> h in map)
      {
        cc = 0;
        foreach (char w in h)
        {
          Buffer[rr, cc] = w;

          // Start character position
          if (w == Player.Character)
          {
            Window.Player.X = cc;
            Window.Player.Y = rr;
            Buffer[rr, cc] = ' ';
          }

          cc++;
        }
        rr++;
      }
    }
  }
}
