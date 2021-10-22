using System.Collections.Generic;

namespace MUD
{
  public class Enemy : Player
  {
    public Enemy(ConsoleWindow wnd) : base(wnd)
    {
      Character = 'E';
      Name = "Test Enemy";

      X = 10;
      Y = 10;
      Facing = Direction.Up;
      Health = 10;
      Power = 0;
      Money = 0;
      Inventory = new List<InventoryItem>();

      //Window.Enemies.Add(this);
    }

    // Enemies do not have inventory or visible stats, so do nothing when these functions are called
    public override void SelectInventory(Direction dir) { }
    public override void UseInventoryItem() { }
    public override void ShowItemInfo() { }
    public override void ShowPlayerStats() { }
  }
}
