public partial class FourWeekHomework
{
    public class Goblin : Monster
    {
        private static int count;
        public Goblin(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
        }
    }
}