using System;

namespace MUD
{
  class Program
  {
    static void Main(string[] args)
    {
      int wndX = 80, wndY = 40;
      string itemFile = ".\\maps\\test.items", mapFile = ".\\maps\\test.map";

      #region Command-line arguments
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
      #endregion

      ConsoleWindow wnd = new ConsoleWindow()
      {
        WindowSize = new Rectangle(0, 0, wndX, wndY)
      };
      wnd.LoadItems(itemFile);

      Player player = new Player(wnd);
      Map map = new Map(wnd);
      map.Load(mapFile);
      HUD hud = new HUD(wnd);

      hud.ShowMessage("Hello " + player.Name + "! Use the arrow keys to move around. This is a test of a really, really long message that may or may not actually fit within a single line in the head's up display. This should technically wrap around to the next line, so everything should be readable.");

      ConsoleKeyInfo key;
      do
      {
        wnd.Print();

        key = Console.ReadKey(true);

        switch (key.Key)
        {
          case ConsoleKey.UpArrow:
            player.Move(Player.Direction.Up);
            break;
          case ConsoleKey.DownArrow:
            player.Move(Player.Direction.Down);
            break;
          case ConsoleKey.LeftArrow:
            player.Move(Player.Direction.Left);
            break;
          case ConsoleKey.RightArrow:
            player.Move(Player.Direction.Right);
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
