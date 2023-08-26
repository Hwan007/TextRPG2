using System.Reflection.Emit;
using System.Text;

public partial class Game
{
    public class Monster : ICharacter
    {
        public string Name { get; protected set; }
        public int Health { get; protected set; }
        public int Attack { get; protected set; }
        public bool IsDead { get => Health == 0 ? true : false; }
        public LinkedList<IEffect> Effects { get; private set; }

        public void TakeDamage(int damage)
        {
            Health = Health - damage < 0 ? 0 : Health;
        }

        public virtual void DrawStatus(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Display.SBWithCustomColor($"{Name}\n"));
            sb.Append(Display.SBWithCustomColor($"공격력 : "));
            sb.Append(Display.SBWithCustomColor($"{Attack}\n", eColorType.Red));
            sb.Append(Display.SBWithCustomColor($"체  력 : "));
            sb.Append(Display.SBWithCustomColor($"{Health}\n", eColorType.Green));
            Display.AddSBToWindow(window, sb);
        }

        public virtual void Draw(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Display.SBWithCustomColor($"{Name}\n"));
            sb.Append(Display.SBWithCustomColor($"{Attack}", eColorType.Red));
            sb.Append(" | ");
            sb.Append(Display.SBWithCustomColor($"{Health}\n", eColorType.Green));
            Display.AddSBToWindow(window, sb);
        }

        public Monster(string name, int hp, int atk)
        {
            Name = name;
            Health = hp;
            Attack = atk;
            Effects = new LinkedList<IEffect>();
        }

    }
}