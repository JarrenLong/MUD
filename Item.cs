namespace MUD
{
  public class Item
  {
    public char RenderChar { get; set; }
    public string Name { get; set; }
    public bool IsSolid { get; set; }
    public bool IsItem { get; set; }
    public int DamageItCauses { get; set; }

    public Item(string definition)
    {
      // Line Format: [char]|[name]|[isSolid (0|1)]|[isItem (0|1)]|[damageItCauses (-100 to +100)]
      RenderChar = definition[0];

      string[] vals = definition.Substring(2).Split(new char[] { '|' });
      if (vals.Length > 0)
        Name = vals[0];
      if (vals.Length > 1)
        IsSolid = vals[1] == "1";
      if (vals.Length > 2)
        IsItem = vals[2] == "1";
      if (vals.Length > 3)
        DamageItCauses = int.Parse(vals[3]);
    }
  }
}
