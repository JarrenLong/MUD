using System.Collections.Generic;

namespace MUD
{
  public interface IHeadsUpDisplay
  {
    void ShowMessage(string msg);
  }

  /// <summary>
  /// Head's-up display
  /// </summary>
  public class HUD : RenderArea, IHeadsUpDisplay
  {
    private string Message = "";
    private bool ShowingInventory = false;

    public HUD(ConsoleWindow wnd) : base(wnd)
    {
      Rectangle ws = Window.WindowSize;

      BufferBounds = new Rectangle(0, 0, ws.Width, ws.Height / 4);
      RenderBounds = BufferBounds;
      WindowOffset = new Rectangle(0, (ws.Height / 4) * 3, ws.Width, ws.Height / 4);

      RenderOrder = 1;

      Window.HUD = this;
    }

    public void ShowMessage(string msg)
    {
      Message = msg;
    }

    public void ToggleInventory()
    {
      ShowingInventory = !ShowingInventory;
    }

    public override void Update()
    {
      Buffer = new char[BufferBounds.Height, BufferBounds.Width];

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

      if (ShowingInventory)
      {
        List<InventoryItem> inv = Window.Player.Inventory;
        if (inv == null || inv.Count == 0)
          Message = "You don't have anything in your inventory!";
        else
          Message = "";

        foreach (InventoryItem it in inv)
          Message += string.Format("{0} -  {1} ({2})\r\n", it.RenderChar, it.Name, it.Quantity);
      }

      // Show messages if any are available
      // TODO: Account for multiline messages with scrolling text
      if (!string.IsNullOrEmpty(Message))
      {
        int y = 1;
        do
        {
          string msg = Message;
          int len = msg.Length;
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
              Buffer[y, x] = msg[i];
              i++;
            }
          }

          Message = Message.Substring(len);
          y++;
        } while (y < BufferBounds.Height - 2);
      }
    }
  }
}
