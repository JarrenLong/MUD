﻿using System.Collections.Generic;

namespace MUD
{
  /// <summary>
  /// Head's-up display
  /// </summary>
  public class HUD : RenderArea
  {
    private string Message = "";
    public bool ShowingInventory { get; private set; } = false;

    public HUD(ConsoleWindow wnd) : base(wnd)
    {
      Rectangle ws = Window.WindowSize;

      BufferBounds = new Rectangle(0, 0, ws.Width, ws.Height / 4);
      RenderBounds = new Rectangle(0, 0, ws.Width, ws.Height / 4); ;
      WindowOffset = new Rectangle(0, (ws.Height / 4) * 3, ws.Width, ws.Height / 4);

      RenderOrder = 0;
      IsVisibleArea = false;

      Window.HUD = this;
    }

    public void ShowMessage(string msg)
    {
      Message = msg.Replace("\r\n", "\n");
    }

    public void ToggleInventory()
    {
      ShowingInventory = !ShowingInventory;
    }

    public override void Update()
    {
      IsVisibleArea = false;

      for (int y = 0; y < BufferBounds.Height; y++)
      {
        for (int x = 0; x < BufferBounds.Width; x++)
        {
          if (x == 0 && y == 0)
            Buffer[y, x] = '*'; // Top left
          else if (x == BufferBounds.Width - 1 && y == 0)
            Buffer[y, x] = '*'; // Top right
          else if (x == 0 && y == BufferBounds.Height - 1)
            Buffer[y, x] = '*'; // Bottom left
          else if (x == BufferBounds.Width - 1 && y == BufferBounds.Height - 1)
            Buffer[y, x] = '*'; // Bottom right
          else if (y == 0 || y == BufferBounds.Height - 1)
            Buffer[y, x] = '-'; // Top or bottom lines
          else if (x == 0 || x == BufferBounds.Width - 1)
            Buffer[y, x] = '|'; // Left or right of area
          else
            Buffer[y, x] = ' '; // Body area
        }
      }

      // Show inventory if inventory window is open
      if (ShowingInventory)
      {
        IsVisibleArea = true;

        List<InventoryItem> inv = Window.Player.Inventory;
        if (inv == null || inv.Count == 0)
          Message = "You don't have anything in your inventory!";
        else
          Message = "";

        foreach (InventoryItem it in inv)
          Message += (it.Selected ? "* " : "") + string.Format("{0} -  {1} ({2})\n", it.RenderChar, it.Name, it.Quantity);
      }

      // Show messages if any are available
      if (!string.IsNullOrEmpty(Message))
      {
        IsVisibleArea = true;

        int y = 1;
        do
        {
          string msg = Message;
          int len = msg.Length;

          if (msg.Contains("\n"))
          {
            msg = msg.Substring(0, msg.IndexOf('\n') + 1);
            len = msg.Length;
          }
          if (len >= BufferBounds.Width - 4)
          {
            msg = msg.Substring(0, BufferBounds.Width - 4);
            len = msg.Length;
          }

          int leftPad = (BufferBounds.Width - 4 - len) / 2 + 1;
          int i = 0;
          for (int x = 1; x <= len + leftPad; x++)
          {
            if (x != 0 && x != BufferBounds.Width && x <= leftPad)
              Buffer[y, x] = ' ';

            if (x > leftPad && x < BufferBounds.Width)
            {
              if (msg[i] != '\n')
                Buffer[y, x] = msg[i];

              i++;
            }
          }

          Message = Message.Substring(len);
          y++;
        } while (y < BufferBounds.Height - 2);

        // If we still have more message to show, show an indicator in the bottom right corner of the HUD.
        if (!string.IsNullOrEmpty(Message))
          Buffer[BufferBounds.Height - 3, BufferBounds.Width - 2] = '~';
      }

      // TODO: Resize the maps rendering area based on whether or not we are showing the HUD
      Window.Map.RenderBounds = !IsVisibleArea ? Window.WindowSize : new Rectangle(0, 0, Window.WindowSize.Width, (Window.WindowSize.Height / 4) * 3);
    }
  }
}
