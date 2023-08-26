public partial class Game
{
    public class EquipEffet : IEffect
    {
        public string Name { get; private set; }
        public int ATK { get; private set; }
        public int HP { get; private set; }
        public eEffectType EffectType { get; private set; }

        public EquipEffet(string name, eEffectType effectType)
        {
            Name = name;
            EffectType = effectType;
        }

        public void AddEffect(ICharacter character)
        {
            character.Effects.AddLast(this);
        }

        public int CalEffectPoint(int basePoint)
        {
            throw new NotImplementedException();
        }

        public void Draw(eWindowType window)
        {
            throw new NotImplementedException();
        }
    }
}