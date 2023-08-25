public partial class FourWeekHomework
{
    public class Weapon : IEquipment
    {
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public string Name { get; private set; }
        public EquipEffet? EquipEffect { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public Weapon(string name, int hp, int atk)
        {
            Name = name;
            Health = hp;
            Attack = atk;
        }
        public Weapon(string name, int hp, int atk, EquipEffet effect)
        {
            Name = name;
            Health = hp;
            Attack = atk;
            EquipEffect = effect;
            EffectNode = new LinkedListNode<IEffect>(effect);
        }
        public void UnuseItem(Warrior character)
        {
            character.Equips.Remove(this);
            if (EquipEffect != null && EffectNode != null)
                character.Effects.Remove(EffectNode);
            character.ReStat();
        }
        public void UseItem(Warrior character)
        {
            character.Equips.AddLast(this);
            if (EquipEffect != null && EffectNode != null)
                character.Effects.AddLast(EffectNode);
            character.ReStat();
        }
        public void Draw(WindowType window)
        {
            throw new NotImplementedException();
        }
    }
}