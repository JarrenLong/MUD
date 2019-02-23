namespace MUD
{
  public class Item
  {
    public char RenderChar { get; set; } = (char)0;
    public string Name { get; set; } = "";
    public bool IsSolid { get; set; } = false;
    public bool IsItem { get; set; } = false;
    public int DamageItCauses { get; set; } = 0;
    public int BackgroundColor { get; set; } = 0;
    public int ForegroundColor { get; set; } = 15;

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
}
