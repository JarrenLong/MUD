using System;
using System.Collections.Generic;

namespace MUD
{
  public class Player : RenderArea
  {
    public const char DefaultCharacter = 'X';
    public char Character = DefaultCharacter;

    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Facing { get; set; }
    public int Health { get; set; }
    public int Power { get; set; }
    public int Money { get; set; }
    public List<InventoryItem> Inventory { get; set; }

    public Player(ConsoleWindow wnd) : base(wnd)
    {
      RenderOrder = 10000;

      Name = "Test Player";
      X = 10;
      Y = 10;
      Facing = Direction.Up;
      Health = 100;
      Power = 100;
      Money = 50;
      Inventory = new List<InventoryItem>();
    }

    public override void Update()
    {
      Array.Clear(Buffer, 0, Buffer.Length);

      // If the player is supposed to be moving along a path, do it (one step in each direction per update).
      if (targetX != -1)
      {
        if (X < targetX)
          X++;
        else if (X > targetX)
          X--;

        if (targetX == X)
          targetX = -1;
      }
      if (targetY != -1)
      {
        if (Y < targetY)
          Y++;
        else if (Y > targetY)
          Y--;

        if (targetY == Y)
          targetY = -1;
      }

      if (IsMovingTo && targetX == -1 && targetY == -1)
        IsMovingTo = false;

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
          //{
          //Window.HUD.ShowMessage("That's a wall, and you're not Bobby.");
          return;
        //}
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
        else if (!string.IsNullOrEmpty(it.Description))
        {
          Window.HUD.ShowMessage(it.Description);
        }
      }

      Rectangle rb = RenderBounds;
      // Keep the render window centered on the player if possible
      rb.X = newX - (rb.Width / 2);
      rb.Y = newY - (rb.Height / 2);

      // Make sure the render window stays within the bounds of the map
      if (rb.X < 0)
        rb.X = 0;
      if (rb.X > BufferBounds.Width - rb.Width)
        rb.X = BufferBounds.Width - rb.Width;
      if (rb.Y < 0)
        rb.Y = 0;
      if (rb.Y > BufferBounds.Height - rb.Height)
        rb.Y = BufferBounds.Height - rb.Height;

      // Store the player's new position
      X = newX;
      Y = newY;
      Facing = dir;
    }

    private int targetX = -1, targetY = -1;
    public bool IsMovingTo { get; private set; } = false;
    public void MoveTo(int x, int y)
    {
      targetX = x;
      targetY = y;
      IsMovingTo = true;
    }

    public virtual void SelectInventory(Direction dir)
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

    public virtual void UseInventoryItem()
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

    /// <summary>
    /// Displays the description of the item directly in front of the player
    /// </summary>
    public virtual void ShowItemInfo()
    {
      int mX = X, mY = Y;

      switch (Facing)
      {
        case Direction.Up: mY--; break;
        case Direction.Down: mY++; break;
        case Direction.Left: mX--; break;
        case Direction.Right: mX++; break;
      }

      // Solid object check (can't walk through them)
      Item it = Window.Items[Window.Map.Buffer[mY, mX]];
      if (it != null && !string.IsNullOrEmpty(it.Description))
      {
        Window.HUD.ShowMessage(it.Description);
      }
    }

    public virtual void ShowPlayerStats()
    {
      Window.HUD.ShowMessage(string.Format("HP: {0}, MP: {1}, Money: {2}, Inventory: {3} Items", Health, Power, Money, Inventory.Count));
    }
  }
}
