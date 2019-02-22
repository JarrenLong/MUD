namespace MUD
{
  public class Rectangle
  {
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle() : this(0, 0, 80, 40) { }

    public Rectangle(int x, int y, int w, int h)
    {
      X = x;
      Y = y;
      Width = w;
      Height = h;
    }
  }
}
