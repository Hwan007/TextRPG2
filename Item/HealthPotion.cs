public partial class Game
{
    public class HealthPotion : IUsableItem
    {
        public string Name { get; private set; }
        public int Point { get; private set; }
        public BattleEffect BattleEffect { get; private set; }
        public eUsableType UsableType { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public HealthPotion(string name, int point, eUsableType type, BattleEffect ef)
        {
            Name = name;
            Point = point;
            UsableType = type;
            BattleEffect = ef;
            EffectNode = new LinkedListNode<IEffect>(ef);
        }

        public void UseItem(Warrior character)
        {
            character.Effects.AddLast(EffectNode);
        }

        public void Draw(eWindowType window)
        {
            throw new NotImplementedException();
        }
    }
}