using System.Reflection.Metadata.Ecma335;

public class Program
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetConsoleMode(IntPtr handle, out int mode);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int handle);

    public static void Main(string[] args)
    {
        var handle = GetStdHandle(-11);
        int mode;
        GetConsoleMode(handle, out mode);
        // You need set flag ENABLE_VIRTUAL_TERMINAL_PROCESSING(0x4) by SetConsoleMode
        SetConsoleMode(handle, mode | 0x4);


        Console.ReadLine();
    }
}

public partial class FourWeekHomework
{
    public interface ICharacter
    {
        public string Name { get; }
        public int Health { get; }
        public int Attack { get; }
        public bool IsDead { get; }
        public void TakeDamage(int damage);
        public void DrawStatus(int x, int y);

        public LinkedList<IEffect> Effects { get; }
    }

    public interface IItem
    {
        public string Name { get; }
        public void UseItem(Warrior character);
        public void Draw();
    }

    public interface IUsableItem : IItem
    {
        public int Point { get; }
        public eUsableType UsableType { get; }
        public BattleEffect BattleEffect { get; }
        public LinkedListNode<IEffect> EffectNode { get; }
    }

    public interface IEquipment : IItem
    {
        public int Health { get; }
        public int Attack { get; }
        public void UnuseItem(Warrior character);
        public EquipEffet? EquipEffect { get; }
        public LinkedListNode<IEffect> EffectNode { get; }
    }

    public enum eUsableType
    {
        HealthRestore,
        AttackStrength
    }

    public enum eEffectType
    {
        OnTotalHealth,
        OnAttack,
        OnBattleHealth,
        OnBattleAttack
    }

    public interface IEffect
    {
        public string Name { get; }
        public eEffectType EffectType { get; }
        public void AddEffect(ICharacter character);
        public int CalEffectPoint(int basePoint);
        public void Draw();
    }

    public struct Vector2
    {
        public int x, y;
    }

}