using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MUD
{
  public class ConsoleWindow
  {
    public int TargetFPS { get; set; }
    public Rectangle WindowSize { get; set; }
    public List<RenderArea> Buffers { get; }
    private char[,] lastRender = null;
    public Dictionary<char, Item> Items { get; }
    public IMap Map { get; set; }
    public IPlayer Player { get; set; }

    public ConsoleWindow()
    {
      Console.CursorVisible = false;
      Console.OutputEncoding = Encoding.ASCII;

      TargetFPS = 30;
      WindowSize = new Rectangle(0, 0, 80, 40);
      Buffers = new List<RenderArea>();
      Items = new Dictionary<char, Item>();
    }

    public void LoadItems(string itemFile)
    {
      if (!File.Exists(itemFile))
        return;

      string[] lines = File.ReadAllLines(itemFile);
      Item buf = null;
      foreach (string r in lines)
      {
        buf = new Item(r);
        Items.Add(buf.RenderChar, buf);
      }

      // Fill in the rest of the item map with blanks
      for (int i = 0; i < 256; i++)
        if (!Items.ContainsKey((char)i))
          Items.Add((char)i, new Item((char)i + "|"));
    }

    public void Print()
    {
      List<char[,]> data = new List<char[,]>();

      // Print all of the render areas in order
      Buffers.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

      char[,] raBuf = null;
      foreach (RenderArea b in Buffers)
      {
        raBuf = b.Print();
        if (b.IsVisibleArea)
          data.Add(raBuf);
      }

      // Now, we'll overlay them in the order that they need to be rendered
      int wndW = WindowSize.Width - WindowSize.X;
      int wndH = WindowSize.Height - WindowSize.Y;
      char[,] renderdata = new char[wndH, wndW];

      foreach (char[,] dl in data)
        for (int y = 0; y < wndH; y++)
          for (int x = 0; x < wndW; x++)
            if (dl[y, x] != 0)
              renderdata[y, x] = dl[y, x];

      // Finally, print the composite to the console
      Item it = null;
      for (int y = 0; y < wndH; y++)
        for (int x = 0; x < wndW; x++)
          if (lastRender == null || lastRender[y, x] != renderdata[y, x])
          {
            try
            {
              it = Items[renderdata[y, x]];
            }
            catch { it = null; }

            Console.SetCursorPosition(x, y);

            if (it != null)
            {
              Console.BackgroundColor = (ConsoleColor)it.BackgroundColor;
              Console.ForegroundColor = (ConsoleColor)it.ForegroundColor;
            }
            Console.Write(renderdata[y, x]);
          }

      // Store a copy of what we just rendered for the next differential update
      lastRender = renderdata;
    }
  }
}
