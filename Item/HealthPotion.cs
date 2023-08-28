using System.Text;

public partial class Game
{
    public class HealthPotion : IUsableItem
    {
        public string Name { get; private set; }
        public int Point { get => Effect.HP??0; }
        public BattleEffect Effect { get; private set; }
        public eUsableType UsableType { get; private set; }

        public HealthPotion(string name, int time, int point, eUsableType type)
        {
            Name = name;
            UsableType = type;
            Effect = new BattleEffect(name, eEffectType.OnBattleHealth, time, null, point);
        }

        public void UseItem(Warrior character)
        {
            Effect.AddEffect(character);
        }

        public void Draw(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} | {Effect.Name} | {Effect.Time}회 적용");
            Display.AddSBToWindow(window, sb);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} | HP +{Point} | {Effect?.Name}");
            return sb.ToString();
        }
    }
}