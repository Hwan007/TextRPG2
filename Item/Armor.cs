using System.Text;

public partial class Game
{
    public class Armor : IEquipment
    {
        public int Health { get; private set; }
        public int Point { get; private set; }
        public string Name { get; private set; }
        public EquipEffet? EquipEffect { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public Armor(string name, int hp, int def)
        {
            Name = name;
            Health = hp;
            Point = def;
        }
        public Armor(string name, int hp, int def, EquipEffet effect)
        {
            Name = name;
            Health = hp;
            Point = def;
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

        public void Draw(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} | HP +{Health} | DEF +{Point} | {EquipEffect?.Name}");
            Display.AddSBToWindow(window, sb);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Name} | HP +{Health} | DEF +{Point} | {EquipEffect?.Name}");
            return sb.ToString();
        }
    }
}