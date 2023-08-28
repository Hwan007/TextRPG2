using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

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

        Console.SetWindowSize(Game.Display.ConsoleWidth, Game.Display.ConsoleHeight);

        //Game.Display.DrawImage(Game.eWindowType.Left, Game.Display.eImageType.Warrior);
        //Game.Display.DrawImage(Game.eWindowType.Right, Game.Display.eImageType.Dragon);
        //Game.Display.DrawImage(Game.eWindowType.Center, Game.Display.eImageType.Goblin);

        //Game.Display.OpenWindow(Game.eWindowType.Center);
        //Console.ReadLine();
        //Game.Display.OpenWindow(Game.eWindowType.Right);
        //Console.ReadLine();
        //Game.Display.OpenWindow(Game.eWindowType.Left);
        //Console.ReadLine();
        //Game.Display.CloseCurrentWindow();
        //Console.ReadLine();
        //Game.Display.CloseCurrentWindow();
        //Console.ReadLine();
        //Game.Display.OpenWindow(Game.eWindowType.Center);
        //Console.ReadLine();
        //Game.Display.OpenWindow(Game.eWindowType.Right);
        Game game = new Game();
        game.GameStart();
        Console.ReadLine();
    }
}

public partial class Game
{
    private Warrior? mPlayer;
    private Stage? mStage;
    public void GameStart()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("이름을 입력해주세요.");
        Display.AddSBToWindow(eWindowType.Center, sb);
        Display.OpenWindow(eWindowType.Center);
        Display.UpdateCurrentWindow();
        sb.Clear();
        sb.Append(">> ");
        Display.AddSBToWindow(eWindowType.Bottom, sb);
        Display.OpenWindow(eWindowType.Bottom);
        Display.UpdateCurrentWindow();
        var name = Console.ReadLine();
        mPlayer = new Warrior(name ?? "철수", 200, 20);
        Display.ClearSBWindow(eWindowType.Center);
        Display.ClearSBWindow(eWindowType.Bottom);
        Display.CloseCurrentWindow();
        Display.CloseCurrentWindow();
        sb.Clear();
        mPlayer.Draw(eWindowType.Left);
        Display.OpenWindow(eWindowType.Left);
        Display.UpdateCurrentWindow();
        Thread.Sleep(1000);
        Display.ClearSBWindow(eWindowType.Right);
        Display.CloseCurrentWindow();
        Thread.Sleep(1000);
        int level = 1;
        while (mPlayer.IsDead == false)
        {
            if (level <= 2)
                mStage = new Stage("너른 초원", level, mPlayer);
            else if (level <= 4)
                mStage = new Stage("깊은 숲속", level, mPlayer);
            else if (level <= 8)
                mStage = new Stage("지하 동굴", level, mPlayer);
            else
                mStage = new Stage("용의 둥지", level, mPlayer);
            level++;
            mStage.Start();
        }
    }
    public interface IDraw
    {
        public virtual void Draw(eWindowType window) { }
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
        public BattleEffect Effect { get; }
    }

    public interface IEquipment : IItem
    {
        public int Health { get; }
        public int Point { get; }
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
    public enum eWindowType
    {
        Full,
        Top,
        Bottom,
        Left,
        Right,
        Center,
        End
    }

    public enum eColorType
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