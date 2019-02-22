using System;
using System.Collections.Generic;
using System.IO;

namespace MUD
{
  public class Map
  {
    public const char Character = 'X';
    public int X { get; private set; }
    public int Y { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public char[,] Tiles { get; private set; }

    public Map(string mapFile, ConsoleWindow wnd)
    {
      Load(mapFile);
    }

    public void Load(string mapFile)
    {
      if (!File.Exists(mapFile))
        return;

      List<List<char>> map = new List<List<char>>();

      var lines = File.ReadAllLines(mapFile);
      foreach (var r in lines)
      {
        List<char> buf = new List<char>();
        foreach (var c in r)
          buf.Add(c);
        map.Add(buf);

        if (Width <= buf.Count)
          Width = buf.Count;
      }
      Height = map.Count;

      Tiles = new char[Height, Width];
      int rr = 0, cc = 0;
      foreach (var h in map)
      {
        cc = 0;
        foreach (var w in h)
        {
          Tiles[rr, cc] = w;

          // Start character position
          if (w == Character)
          {
            X = cc;
            Y = rr;
            Tiles[rr, cc] = ' ';
          }

          cc++;
        }
        rr++;
      }
    }

    public void Print()
    {
      Console.Clear();
      for (int r = 0; r < Height; r++)
      {
        for (int c = 0; c < Width; c++)
        {
          if (r == Y && c == X)
            Console.Write(Character);
          else
            Console.Write(Tiles[r, c]);
        }
        Console.Write(Environment.NewLine);
      }
    }

    public enum Direction
    {
      Up,
      Down,
      Left,
      Right
    }

    public void Move(Direction dir)
    {
      int newX = X, newY = Y;

      switch (dir)
      {
        case Direction.Up:
          newY--;
          break;
        case Direction.Down:
          newY++;
          break;
        case Direction.Left:
          newX--;
          break;
        case Direction.Right:
          newX++;
          break;
      }

      // Bounds check
      if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
      {
        //ShowMessage("You can't move that direction!");
        return;
      }

      // Solid object check (can't walk through them)
      if (Tiles[newY, newX] == '#')
      {
        //ShowMessage("That's a wall, and you're not Bobby.");
        return;
      }

      X = newX;
      Y = newY;

      // TODO: Item check, something to pick up?

      // Show the updated map
      Print();
    }
  }
}
