using System;
using System.Collections.Generic;
using System.Linq;

namespace MUD
{
  public class ConsoleWindow
  {
    public Rectangle WindowSize { get; set; }
    public List<RenderArea> Buffers { get; }

    public ConsoleWindow()
    {
      WindowSize = new Rectangle(0, 0, 80, 40);
      Buffers = new List<RenderArea>();
    }

    public string Print()
    {
      List<char[,]> data = new List<char[,]>();

      // Print all of the render areas in order
      var buffers = Buffers.OrderBy(x => x.RenderOrder);
      foreach (var b in buffers)
        data.Add(b.Print(this));

      // Now, we'll overlay them in the order that they need to be rendered
      int wndW = WindowSize.Width - WindowSize.X;
      int wndH = WindowSize.Height - WindowSize.Y;
      char[,] renderdata = new char[wndH, wndW];
      foreach (var dl in data)
      {
        for (int y = 0; y < wndH; y++)
        {
          for (int x = 0; x < wndW; x++)
          {
            var c = dl[y, x];
            if (c != 0)
              renderdata[y, x] = c;
          }
        }
      }

      // Finally, print the composite to the console
      string toPrint = "";
      for (int y = 0; y < wndH; y++)
      {
        for (int x = 0; x < wndW; x++)
          toPrint += renderdata[y, x];
        toPrint += Environment.NewLine;
      }

      return toPrint;
    }
  }
}
