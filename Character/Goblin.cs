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
            if (Random.Shared.Next(1, 11) > 3)
            {
                Items.Add(new HealthPotion("회복약", 1, 10+Random.Shared.Next(-5, 6), eUsableType.HealthRestore));
            }
            else
            {
                Items.Add(new HealthPotion("회복약", 1, 10 + Random.Shared.Next(-5, 6), eUsableType.HealthRestore));
                Items.Add(new StrengthPotion("강화약", 5, 10 + Random.Shared.Next(-5, 6)));
            }

        }
        public override void Draw(eWindowType window)
        {
            Display.DrawImage(window, Display.eImageType.Goblin);
            base.Draw(window);
        }
    }
}