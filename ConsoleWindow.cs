using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MUD
{
  public partial class ConsoleWindow
  {
    public int TargetFPS { get; set; }
    private Rectangle _wndSize = null;
    public Rectangle WindowSize
    {
      get { return _wndSize; }
      set
      {
        _wndSize = value;
        Console.SetWindowSize(_wndSize.Width, _wndSize.Height);
      }
    }
    public List<RenderArea> Buffers { get; }
    private char[,] lastRender = null;
    public Dictionary<char, Item> Items { get; }
    public Map Map { get; set; }
    public Player Player { get; set; }
    public HUD HUD { get; set; }

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

    public void GameLoop()
    {
      bool exit = false;
      int loopTime = 1000 / TargetFPS;
      int lastRunTime = 0;

      Print();

      do
      {
        // Wait for the next polling cycle
        while ((Environment.TickCount - lastRunTime) < loopTime)
          Thread.Sleep(10);

        lastRunTime = Environment.TickCount;

        List<Keys> down = GetPressedKeys();
        // If there's no key pressed, reset the polling loop (nothing's gonna change)
        if (down.Count == 0)
          continue;

        foreach (Keys k in down)
        {
          switch (k)
          {
            case Keys.Up:
              if (HUD.ShowingInventory)
                Player.SelectInventory(Player.Direction.Up);
              else
                Player.Move(Player.Direction.Up);
              break;
            case Keys.Down:
              if (HUD.ShowingInventory)
                Player.SelectInventory(Player.Direction.Down);
              else
                Player.Move(Player.Direction.Down);
              break;
            case Keys.Left:
              if (HUD.ShowingInventory)
                Player.SelectInventory(Player.Direction.Up);
              else
                Player.Move(Player.Direction.Left);
              break;
            case Keys.Right:
              if (HUD.ShowingInventory)
                Player.SelectInventory(Player.Direction.Down);
              else
                Player.Move(Player.Direction.Right);
              break;
            case Keys.Space:
              if (HUD.ShowingInventory)
                Player.UseInventoryItem();
              break;
            case Keys.Escape:
              HUD.ShowMessage("Closing ...");
              exit = true;
              break;
            case Keys.I:
              HUD.ToggleInventory();
              break;
          }
        }

        Print();
      } while (!exit);

      Thread.Sleep(1000);
    }

    private List<Keys> GetPressedKeys()
    {
      List<Keys> ret = new List<Keys>();
      int[] keyVals = (int[])Enum.GetValues(typeof(Keys));

      foreach (int k in keyVals)
        if ((GetKeyState(k) & 0x8000) != 0)
          ret.Add((Keys)k);

      return ret;
    }

    [DllImport("user32.dll")]
    static extern short GetKeyState(int nVirtKey);
  }
}
