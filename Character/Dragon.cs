public partial class Game
{
    public class Dragon : Monster
    {
        private static int count;
        public Dragon(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
        }

        public override void Draw(eWindowType window)
        {
            Display.DrawImage(window, Display.eImageType.Dragon);
            base.Draw(window);
        }
    }
}