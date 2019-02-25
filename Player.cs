using System;
using System.Collections.Generic;
using System.Text;

namespace MUD
{
  public class Player : RenderArea
  {
    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int Health { get; set; }
    public int Power { get; set; }
    public int Money { get; set; }
    public List<Item> Inventory { get; set; }

    public Player(ConsoleWindow wnd) : base(wnd) { }

    public override void Update()
    {
      // Do render updates here
    }
  }
}
