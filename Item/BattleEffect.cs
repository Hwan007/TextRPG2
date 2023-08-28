using System.Text;

public partial class Game
{
    public class BattleEffect : IEffect
    {
        public string Name { get; private set; }
        public int Time { get; private set; }
        public int? ATK { get; private set; }
        public int? HP { get; private set; }
        public eEffectType EffectType { get; private set; }
        private bool mIsUsed=false;

        public BattleEffect(string name, eEffectType ef, int time, int? atk, int? hp)
        {
            Name = name;
            EffectType = ef;
            Time = time;

            if ((ef == eEffectType.OnBattleAttack || ef == eEffectType.OnAttack) && atk.HasValue) { ATK = atk.Value; }
            if ((ef == eEffectType.OnBattleHealth || ef == eEffectType.OnTotalHealth) && hp.HasValue) { HP = hp.Value; }
        }

        public int CalEffectPoint(int basePoint)
        {
            int point = 0;
            if (Time > 0) --Time;
            switch (EffectType)
            {
                case eEffectType.OnTotalHealth:
                    if (mIsUsed == false)
                        point = HP.Value;
                    break;
                case eEffectType.OnAttack:
                    if (mIsUsed == false)
                        point = ATK.Value;
                    break;
                case eEffectType.OnBattleHealth:
                    point = HP.Value;
                    break;
                case eEffectType.OnBattleAttack:
                    if (mIsUsed == false)
                        point = ATK.Value;
                    break;
            }
            mIsUsed = true;
            return point;
        }

        public void AddEffect(ICharacter character)
        {
            character.Effects.AddLast(this);
        }

        public void Draw(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} | ");
            if (ATK.HasValue)
            {
                sb.Append($"ATK +{ATK}");
            }
            else if (HP.HasValue)
            {
                sb.Append($"HP +{HP}");
            }
            Display.AddSBToWindow(window, sb);
        }
    }
}