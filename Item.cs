using System.Collections.Generic;

namespace MUD
{
  public class PrintChar
  {
    public char RenderChar { get; set; } = (char)0;
    public int BackgroundColor { get; set; } = 0;
    public int ForegroundColor { get; set; } = 15;

    public PrintChar() { }
    public PrintChar(char c)
    {
      RenderChar = c;
    }
  }

  public class PrintCharString
  {
    public List<PrintChar> String { get; }
    public PrintCharString()
    {
      String = new List<PrintChar>();
    }
    public PrintCharString(string msg)
    {
      String = new List<PrintChar>();

      if (!string.IsNullOrEmpty(msg))
        foreach (char c in msg)
          String.Add(new PrintChar() { RenderChar = c });
    }

    public void Print()
    {

    }

    public override string ToString()
    {
      string ret = "";
      foreach (PrintChar pc in String)
        ret += pc.RenderChar;
      return ret;
    }
  }

  public class Item : PrintChar
  {
    public string Name { get; set; } = "";
    public bool IsSolid { get; set; } = false;
    public bool IsItem { get; set; } = false;
    public int DamageItCauses { get; set; } = 0;

    public Item() { }

    public Item(string definition)
    {
      // Line Format: [char]|[name]|[isSolid (0|1)]|[isItem (0|1)]|[bgColor]|[fgColor]|[damageItCauses (-100 to +100)]
      RenderChar = definition[0];

      string[] vals = definition.Substring(2).Split(new char[] { '|' });
      if (vals.Length > 0)
        Name = vals[0];
      if (vals.Length > 1)
        IsSolid = vals[1] == "1";
      if (vals.Length > 2)
        IsItem = vals[2] == "1";
      if (vals.Length > 3)
        BackgroundColor = int.Parse(vals[3]);
      if (vals.Length > 4)
        ForegroundColor = int.Parse(vals[4]);
      if (vals.Length > 5)
        DamageItCauses = int.Parse(vals[5]);
    }
  }

  public class InventoryItem : Item
  {
    public int Quantity { get; set; }
    public bool Selected { get; set; }

    public InventoryItem(string definition) : base(definition)
    {
      Quantity = 0;
    }
    public InventoryItem(Item it)
    {
      RenderChar = it.RenderChar;
      Name = it.Name;
      IsSolid = it.IsSolid;
      IsItem = it.IsItem;
      BackgroundColor = it.BackgroundColor;
      ForegroundColor = it.ForegroundColor;
      DamageItCauses = it.DamageItCauses;
      Quantity = 0;
    }
  }
}
