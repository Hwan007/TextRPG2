using System.Text;

public partial class Game
{
    public class EquipEffet : IEffect
    {
        public string Name { get; private set; }
        public int ATK { get; private set; }
        public int HP { get; private set; }
        public eEffectType EffectType { get; private set; }

        public EquipEffet(string name, eEffectType effectType, int? hp, int? atk)
        {
            Name = name;
            EffectType = effectType;
            if (hp != null)
                HP = hp.Value;
            else
                HP = 0;
            if (atk != null)
                ATK = atk.Value;
            else
                ATK = 0;
        }

        public void AddEffect(ICharacter character)
        {
            character.Effects.AddLast(this);
        }

        public int CalEffectPoint(int basePoint)
        {
            int point = 0;
            switch (EffectType)
            {
                case eEffectType.OnTotalHealth:

                    break;
                case eEffectType.OnAttack:

                    break;
            }
            return point;
        }

        public void Draw(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} | ");
            if (ATK != 0)
            {
                sb.Append($"ATK +{ATK}");
            }
            else if (HP != 0)
            {
                sb.Append($"HP +{HP}");
            }
            Display.AddSBToWindow(window, sb);
        }
    }
}