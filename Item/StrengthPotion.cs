using System.Text;
using static Game;

public partial class Game
{
    public class StrengthPotion : IUsableItem
    {
        public string Name { get; private set; }
        public int Point { get => Effect.ATK??0; }
        public BattleEffect Effect { get; private set; }
        public eUsableType UsableType { get; private set; }

        public StrengthPotion(string name, int time, int point)
        {
            Name = name;
            Effect = new BattleEffect(name, eEffectType.OnBattleAttack, time, point, null);
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
            sb.Append($"{Name} | ATK +{Point} | {Effect?.Name}");
            return sb.ToString();
        }
    }
}