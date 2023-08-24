public partial class FourWeekHomework
{
    public class Monster : ICharacter
    {
        private int mHealth;
        private int mAttack;
        public string Name { get; protected set; }
        public int Health { get => mHealth; protected set => mHealth = value; }
        public int Attack { get => mAttack; protected set => mAttack = value; }
        public bool IsDead { get; protected set; }
        public LinkedList<IEffect> Effects { get; private set; }

        public void TakeDamage(int damage)
        {
            Health = Health - damage < 0 ? 0 : Health;
            if (Health == 0) IsDead = true;
        }

        public void DrawStatus(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(Name);
            Console.WriteLine(Health);
            Console.WriteLine(Attack);
        }

        public Monster(string name, int hp, int atk)
        {
            Name = name;
            Health = hp;
            Attack = atk;
            IsDead = false;
            Effects = new LinkedList<IEffect>();
        }

    }
}