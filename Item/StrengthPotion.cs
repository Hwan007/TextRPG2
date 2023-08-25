public partial class Game
{
    public class StrengthPotion : IUsableItem
    {
        public string Name { get; private set; }
        public int Point { get; private set; }
        public BattleEffect BattleEffect { get; private set; }
        public eUsableType UsableType { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public StrengthPotion(string name)
        {
            Name = name;
        }
        public void UseItem(Warrior character)
        {
            throw new NotImplementedException();
        }

        public void Draw(WindowType window)
        {
            throw new NotImplementedException();
        }
    }
}