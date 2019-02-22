using System;
using System.Text;

namespace MUD
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.CursorVisible = false;
      Console.OutputEncoding = Encoding.ASCII;

      ConsoleWindow wnd = new ConsoleWindow()
      {
        WindowSize = new Rectangle(0, 0, 80, 40)
      };

      Map map = new Map(wnd);
      map.Load(".\\maps\\test.map");
      HUD hud = new HUD(wnd);

      ConsoleKeyInfo key;
      do
      {
        Console.Clear();
        Console.Write(wnd.Print());

        key = Console.ReadKey(true);

        switch (key.Key)
        {
          case ConsoleKey.UpArrow:
            map.Move(Map.Direction.Up);
            break;
          case ConsoleKey.DownArrow:
            map.Move(Map.Direction.Down);
            break;
          case ConsoleKey.LeftArrow:
            map.Move(Map.Direction.Left);
            break;
          case ConsoleKey.RightArrow:
            map.Move(Map.Direction.Right);
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
