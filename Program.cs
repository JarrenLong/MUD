using System;

namespace MUD
{
  class Program
  {
    static void Main(string[] args)
    {
      ConsoleWindow wnd = new ConsoleWindow()
      {
        WindowSize = new Rectangle(0, 0, 80, 40)
      };
      wnd.LoadItems(".\\maps\\test.items");

      Map map = new Map(wnd);
      map.Load(".\\maps\\test.map");
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
