public partial class FourWeekHomework
{
    public class Dragon : Monster
    {
        private static int count;
        public Dragon(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
        }
    }
}