using System.Threading;

namespace MUD.Game
{
  class Program
  {
    static void Main(string[] args)
    {
      int wndX = 80, wndY = 40;
      string itemFile = "..\\..\\..\\maps\\test.items", mapFile = "..\\..\\..\\maps\\test.map";

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

      wnd.Player = new Player(wnd);
      Map map = new Map(wnd);
      map.Load(mapFile);
      HUD hud = new HUD(wnd);

      Enemy test = new Enemy(wnd);
      test.Health = 10;
      test.X = 10;
      test.Y = 10;

      wnd.Enemies.Add(test);

      ThreadPool.QueueUserWorkItem(o =>
      {
        while (true)
        {
          Thread.Sleep(3000);

          test.MoveTo(2, 2);

          while (test.IsMovingTo)
            Thread.Sleep(10);

          test.MoveTo(10, 10);

          while (test.IsMovingTo)
            Thread.Sleep(10);
        }
      });

      hud.ShowMessage(@"Hello " + wnd.Player.Name + @"!

Use the arrow keys to move around. This is a
test of a really, really long message
that may or may not actually fit within a line in the head's up display. This should wrap around to the next line, everything should be readable.



Got it?
");

      wnd.GameLoop();
    }
  }
}
