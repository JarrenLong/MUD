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
    public List<Item> Items { get; }

    public ConsoleWindow()
    {
      Console.CursorVisible = false;
      Console.OutputEncoding = Encoding.ASCII;

      TargetFPS = 30;
      WindowSize = new Rectangle(0, 0, 80, 40);
      Buffers = new List<RenderArea>();
      Items = new List<Item>();
    }

    public void LoadItems(string itemFile)
    {
      if (!File.Exists(itemFile))
        return;

      string[] lines = File.ReadAllLines(itemFile);
      foreach (string r in lines)
        Items.Add(new Item(r));
    }

    public void Print()
    {
      List<char[,]> data = new List<char[,]>();

      // Print all of the render areas in order
      Buffers.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

      foreach (RenderArea b in Buffers)
        data.Add(b.Print(this));

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
      for (int y = 0; y < wndH; y++)
        for (int x = 0; x < wndW; x++)
          if (lastRender == null || lastRender[y, x] != renderdata[y, x])
          {
            Item it = null;
            foreach (Item i in Items)
            {
              if (i.RenderChar == renderdata[y, x])
              {
                it = i;
                break;
              }
            }

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
