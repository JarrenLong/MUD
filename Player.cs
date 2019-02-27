using System;
using System.Collections.Generic;

namespace MUD
{
  public class Player : RenderArea
  {
    public const char Character = 'X';

    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int Power { get; set; }
    public int Money { get; set; }
    public List<InventoryItem> Inventory { get; }

    public Player(ConsoleWindow wnd) : base(wnd)
    {
      RenderOrder = 10000;

      Name = "Test Player";
      X = 10;
      Y = 10;
      Health = 100;
      Power = 100;
      Money = 50;
      Inventory = new List<InventoryItem>();

      Window.Player = this;
    }

    public override void Update()
    {
      Array.Clear(Buffer, 0, Buffer.Length);
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
      BufferBounds = Window.Map.BufferBounds;
      RenderBounds = Window.Map.RenderBounds;

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
        Window.HUD.ShowMessage("You can't move that direction!");
        return;
      }

      // Solid object check (can't walk through them)
      Item it = Window.Items[Window.Map.Buffer[newY, newX]];
      if (it != null)
      {
        if (it.IsSolid)
        {
          Window.HUD.ShowMessage("That's a wall, and you're not Bobby.");
          return;
        }
        else if (it.IsItem)
        {
          // Pick up the item and add it to the player's inventory
          bool found = false;
          foreach (InventoryItem pi in Inventory)
          {
            if (pi.RenderChar == it.RenderChar)
            {
              pi.Quantity++;
              found = true;
              break;
            }
          }

          if (!found)
            Inventory.Add(new InventoryItem(it) { Quantity = 1 });

          // Remove the item from the original map
          Window.Map.Buffer[newY, newX] = ' ';

          Window.HUD.ShowMessage(string.Format("You found a {0}!", it.Name));
        }
      }

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

      // Store the player's new position
      X = newX;
      Y = newY;
    }

    public void SelectInventory(Direction dir)
    {
      bool up = (dir == Direction.Up || dir == Direction.Left);
      bool found = false;

      for (int i = 0; i < Inventory.Count; i++)
      {
        if (Inventory[i].Selected)
        {
          Inventory[i].Selected = false;

          if (i == 0 && up)
            i = Inventory.Count - 1;
          else if (i == Inventory.Count - 1 && !up)
            i = 0;
          else
            i += (up ? -1 : 1);

          Inventory[i].Selected = true;
          found = true;
          break;
        }
      }

      if (!found && Inventory.Count > 0)
        Inventory[0].Selected = true;
    }

    public void UseInventoryItem()
    {
      // Grab the selected inventory item
      InventoryItem it = null;
      for (int i = 0; i < Inventory.Count; i++)
      {
        if (Inventory[i].Selected)
        {
          it = Inventory[i];
          break;
        }
      }

      if (it == null)
        return;

      // Use it
      Health += it.HealthItCauses;
      Power += it.PowerItCauses;
      Money += it.MoneyItCauses;

      // Reduce the quantity on hand, or remove it from inventory completely
      it.Quantity--;
      if (it.Quantity == 0)
        Inventory.Remove(it);
    }
  }
}
