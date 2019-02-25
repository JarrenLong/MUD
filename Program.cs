using System;

namespace MUD
{
  class Program
  {
    static void Main(string[] args)
    {
      int wndX = 80, wndY = 40;
      string itemFile = ".\\maps\\test.items", mapFile = ".\\maps\\test.map";

      if (args != null && args.Length > 0)
      {
        string argBuf = "";

        for (int i = 0; i < args.Length; i++)
        {
          argBuf = args[i].ToLower();

          // Check if user specified a custom width
          if (argBuf == "-w" || argBuf == "--width")
          {
            try
            {
              i++;
              wndX = int.Parse(args[i]);
            }
            catch { }
          }

          // Check if user specified a custom height
          if (argBuf == "-h" || argBuf == "--height")
          {
            try
            {
              i++;
              wndY = int.Parse(args[i]);
            }
            catch { }
          }

          // Check if user specified a custom item file
          if (argBuf == "-i" || argBuf == "--items")
          {
            try
            {
              i++;
              itemFile = args[i];
            }
            catch { }
          }

          // Check if user specified a custom map file
          if (argBuf == "-m" || argBuf == "--map")
          {
            try
            {
              i++;
              mapFile = args[i];
            }
            catch { }
          }
        }
      }

      ConsoleWindow wnd = new ConsoleWindow()
      {
        WindowSize = new Rectangle(0, 0, wndX, wndY)
      };
      wnd.LoadItems(itemFile);

      Map map = new Map(wnd);
      map.Load(mapFile);
      HUD hud = new HUD(wnd);

      ConsoleKeyInfo key;
      do
      {
        wnd.Print();

        key = Console.ReadKey(true);

        switch (key.Key)
        {
          case ConsoleKey.UpArrow:
            map.Move(wnd, Map.Direction.Up);
            break;
          case ConsoleKey.DownArrow:
            map.Move(wnd, Map.Direction.Down);
            break;
          case ConsoleKey.LeftArrow:
            map.Move(wnd, Map.Direction.Left);
            break;
          case ConsoleKey.RightArrow:
            map.Move(wnd, Map.Direction.Right);
            break;
          case ConsoleKey.Escape:
            hud.ShowMessage("Closing ...");
            break;
          default:
            hud.ShowMessage("Unknown key!");
            break;
        }
      } while (key.Key != ConsoleKey.Escape);
    }
  }
}
