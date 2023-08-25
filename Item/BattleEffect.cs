public partial class Game
{
    public class BattleEffect : IEffect
    {
        public string Name { get; private set; }
        public int Time { get; private set; }
        public int? ATK { get; private set; }
        public int? HP { get; private set; }
        public eEffectType EffectType { get; private set; }

        public BattleEffect(string name, eEffectType ef, int time, int? atk, int? hp)
        {
            Name = name;
            EffectType = ef;
            Time = time;

            if ((ef == eEffectType.OnBattleAttack || ef == eEffectType.OnAttack) && atk.HasValue) { ATK = atk.Value; }
            else throw new Exception("Effect type and stat is not match.");
            if ((ef == eEffectType.OnBattleHealth || ef == eEffectType.OnTotalHealth) && hp.HasValue) { HP = hp.Value; }
            else throw new Exception("Effect type and stat is not match.");
        }

        public int CalEffectPoint(int basePoint)
        {
            int point = 0;
            if (Time > 0) --Time;
            switch (EffectType)
            {
                case eEffectType.OnTotalHealth:

                    break;
                case eEffectType.OnAttack:

                    break;
                case eEffectType.OnBattleHealth:

                    break;
                case eEffectType.OnBattleAttack:

                    break;
            }
            return point;
        }

        public void AddEffect(ICharacter character)
        {
            character.Effects.AddLast(this);
        }

        public void Draw(WindowType window)
        {
            throw new NotImplementedException();
        }
    }
}