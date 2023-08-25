using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

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
    public interface IDraw
    {
        public void Draw(WindowType window);
    }
    public interface ICharacter : IDraw
    {
        public string Name { get; }
        public int Health { get; }
        public int Attack { get; }
        public bool IsDead { get; }
        public void TakeDamage(int damage);
        public LinkedList<IEffect> Effects { get; }
    }

    public interface IItem : IDraw
    {
        public string Name { get; }
        public void UseItem(Warrior character);
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

    public interface IEffect : IDraw
    {
        public string Name { get; }
        public eEffectType EffectType { get; }
        public void AddEffect(ICharacter character);
        public int CalEffectPoint(int basePoint);
    }

    public struct Vector2
    {
        public int x, y;
    }
    public enum WindowType
    {
        Full,
        Top,
        Bottom,
        Left,
        Right,
        Center,
        End
    }

    public enum ColorType
    {
        Gold = 178,
        Red = 160,
        Gray = 7,
        White = 15,
        Black = 16,
        Orange = 208,
        PurpleBlue = 75,
        Green = 76,
        Yellow = 226,
        Mint = 121
    }


}