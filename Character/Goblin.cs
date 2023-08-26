using System.Text;

public partial class Game
{
    public class Goblin : Monster
    {
        private static int count;
        public Goblin(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
        }
        public override void Draw(eWindowType window)
        {
            Display.DrawImage(window, Display.eImageType.Goblin);
            base.Draw(window);
        }
    }
}