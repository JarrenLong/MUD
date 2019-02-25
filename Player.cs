using System.Collections.Generic;

namespace MUD
{
  public interface IPlayer
  {
    int X { get; set; }
    int Y { get; set; }
  }

  public class Player : RenderArea, IPlayer
  {
    public const char Character = 'X';

    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int Power { get; set; }
    public int Money { get; set; }
    public List<Item> Inventory { get; set; }

    public Player(ConsoleWindow wnd) : base(wnd)
    {
      RenderOrder = 10000;

      Name = "Test Player";
      X = 10;
      Y = 10;
      Health = 100;
      Power = 100;
      Money = 50;
      Inventory = new List<Item>();

      Window.Player = this;
    }

    public override void Update()
    {
      // Do render updates here
      Buffer = new char[BufferBounds.Height, BufferBounds.Width];
      // Set the player's position
      Buffer[Y, X] = Character;
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
      if (newX < 0 || newX >= BufferBounds.Width || newY < 0 || newY >= BufferBounds.Height)
      {
        //ShowMessage("You can't move that direction!");
        return;
      }

      // Solid object check (can't walk through them)
      Item it = Window.Items[Window.Map.OriginalMap[newY, newX]];
      if (it != null)
      {
        if (it.IsSolid)
        {
          //ShowMessage("That's a wall, and you're not Bobby.");
          return;
        }
        else if (it.IsItem)
        {
          // TODO: Pick up the item and do something with it

          // Remove the item from the original map
          Window.Map.OriginalMap[newY, newX] = ' ';
        }
      }

      // Move the render window for the scrolling effect

      // Keep the render window centered on the player if possible
      RenderBounds.X = newX - (RenderBounds.Width / 2);
      RenderBounds.Y = newY - (RenderBounds.Height / 2);

      // Make sure the render window stays within the bounds of the map
      if (RenderBounds.X < 0)
        RenderBounds.X = 0;
      if (RenderBounds.X > BufferBounds.Width - RenderBounds.Width)
        RenderBounds.X = BufferBounds.Width - RenderBounds.Width;
      if (RenderBounds.Y < 0)
        RenderBounds.Y = 0;
      if (RenderBounds.Y > BufferBounds.Height - RenderBounds.Height)
        RenderBounds.Y = BufferBounds.Height - RenderBounds.Height;

      X = newX;
      Y = newY;
    }
  }
}
