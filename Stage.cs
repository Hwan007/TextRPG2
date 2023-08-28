using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using static Game;

public partial class Game
{
    public class Stage
    {
        public string Name { get; private set; }
        public int Level { get; private set; }
        public Queue<Monster> monsters { get; private set; }
        private Warrior mPlayer;
        private Monster mCurrentMonster;

        public Stage(string name, int level, Warrior player)
        {
            monsters = new Queue<Monster>();
            Name = name;
            Level = level;
            mPlayer = player;
            // level 만큼 나온다.
            for (int i = 0; i < level; ++i)
            {
                int rand = Random.Shared.Next(1, 11);
                // 0 level 일 때 3 ~ 9 는 고블린. 점점 감소
                if (rand / level > 1)
                {
                    monsters.Enqueue(new Goblin("고블린", 15 + Random.Shared.Next(-10, 11), 5 + Random.Shared.Next(-2, 3)));
                }
                // 0 level 일 때 0 은 용. 점점 증가
                else
                {
                    monsters.Enqueue(new Dragon("드래곤", 50 + Random.Shared.Next(-20, 21), 50 + Random.Shared.Next(-20, 21)));
                }
            }
        }
        public void Start()
        {
            mCurrentMonster = NextMonster();
            Display.ClearSBWindow(eWindowType.Left);
            Display.ClearSBWindow(eWindowType.Right);
            Display.ClearSBWindow(eWindowType.Bottom);
            mPlayer.Draw(eWindowType.Left);
            mCurrentMonster.Draw(eWindowType.Right);
            Display.OpenWindow(eWindowType.Left);
            Display.OpenWindow(eWindowType.Right);
            Display.OpenWindow(eWindowType.Bottom);
            Display.UpdateAllBackWindow();
            Loop();
        }
        public void Loop()
        {
            StringBuilder sb = new StringBuilder();
            while (mPlayer.IsDead == false && (monsters.Count != 0 || mCurrentMonster.IsDead == false))
            {
                sb.Clear();
                if (mCurrentMonster.IsDead)
                {
                    ChooseReward();
                    
                    Display.ClearSBWindow(eWindowType.Left);
                    Display.ClearSBWindow(eWindowType.Right);
                    mPlayer.Draw(eWindowType.Left);
                    mPlayer.DrawStatus(eWindowType.Right);
                    Display.UpdateAllBackWindow();
                    Thread.Sleep(3000);
                    mCurrentMonster = NextMonster();
                }
                Display.ClearSBWindow(eWindowType.Left);
                Display.ClearSBWindow(eWindowType.Right);
                mPlayer.Draw(eWindowType.Left);
                mCurrentMonster.Draw(eWindowType.Right);
                Display.OpenWindow(eWindowType.Left);
                Display.OpenWindow(eWindowType.Right);
                Display.UpdateAllBackWindow();

                mPlayer.TurnEffect();
                Display.ClearSBWindow(eWindowType.Left);
                mPlayer.Draw(eWindowType.Left);
                Display.UpdateAllBackWindow();

                Thread.Sleep(1000);
                mCurrentMonster.TakeDamage(mPlayer.Attack);
                sb.Append($"{mCurrentMonster.Name}에게 {mPlayer.Attack} 데미지를 입혔습니다.");
                Display.AddSBToWindow(eWindowType.Bottom, sb);

                Display.ClearSBWindow(eWindowType.Left);
                Display.ClearSBWindow(eWindowType.Right);
                mPlayer.Draw(eWindowType.Left);
                mCurrentMonster.Draw(eWindowType.Right);
                Display.OpenWindow(eWindowType.Left);
                Display.OpenWindow(eWindowType.Right);
                Display.UpdateAllBackWindow();
                Display.UpdateCurrentWindow();
                sb.Clear();
                Thread.Sleep(1000);
                mPlayer.TakeDamage(mCurrentMonster.Attack);
                sb.Append($"{mCurrentMonster.Attack} 데미지를 입었습니다.");
                Display.AddSBToWindow(eWindowType.Bottom, sb);

                Display.ClearSBWindow(eWindowType.Left);
                Display.ClearSBWindow(eWindowType.Right);
                mPlayer.Draw(eWindowType.Left);
                mCurrentMonster.Draw(eWindowType.Right);
                Display.OpenWindow(eWindowType.Left);
                Display.OpenWindow(eWindowType.Right);
                Display.UpdateAllBackWindow();
                Display.UpdateCurrentWindow();
                Thread.Sleep(1000);
                Display.ClearSBWindow(eWindowType.Bottom);
            }
            if (mPlayer.IsDead)
                EndGame();
        }
        public void Draw()
        {
            Display.UpdateCurrentWindow();
        }
        private void EndGame()
        {
            Display.ClearSBWindow(eWindowType.Center);
            StringBuilder sb = new StringBuilder();
            sb.Append("사망하셨습니다.");
            Display.AddSBToWindow(eWindowType.Center, sb);
            sb.Clear();
            sb.Append("Game Over");
            Display.AddSBToWindow(eWindowType.Center, sb);
            Display.OpenWindow(eWindowType.Center);
            Display.UpdateCurrentWindow();
            Console.ReadLine();
        }
        private Monster NextMonster()
        {
            Monster monster = monsters.Dequeue();
            Display.ClearSBWindow(eWindowType.Center);
            StringBuilder sb = new StringBuilder();
            sb.Append($"야생의 {monster.Name}이 나타났다!");
            Display.AddSBToWindow(eWindowType.Center, sb);
            Display.OpenWindow(eWindowType.Center);
            Display.UpdateCurrentWindow();
            Thread.Sleep(1000);
            Display.CloseCurrentWindow();
            return monster;
        }
        private void ChooseReward()
        {
            Display.ClearSBWindow(eWindowType.Center);
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (IItem item in mCurrentMonster.Items)
            {
                sb.Append($"[{i}]" + item.ToString());
                ++i;
            }
            Display.AddSBToWindow(eWindowType.Center, sb);
            Display.OpenWindow(eWindowType.Center);
            Display.UpdateCurrentWindow();

            Display.ClearSBWindow(eWindowType.Bottom);
            sb.Clear();
            sb.Append("보상을 골라주세요.");
            Display.AddSBToWindow(eWindowType.Bottom, sb);
            Display.OpenWindow(eWindowType.Bottom);
            Display.UpdateCurrentWindow();

            string key = Console.ReadLine() ?? "";
            if (IsValidKey(key, i-1, out int result))
            {
                mCurrentMonster.Items[result].UseItem(mPlayer);
            }
            Display.ClearSBWindow(eWindowType.Bottom);
            Display.ClearSBWindow(eWindowType.Center);
            Display.CloseCurrentWindow();
            Display.CloseCurrentWindow();
        }
        private bool IsValidKey(string key, int max, out int ret)
        {
            ret = -1;
            if (int.TryParse(key, out var value))
            {
                if (value > max)
                    return false;
                else
                {
                    ret = value;
                    return true;
                }
            }
            else
                return false;
        }
    }
}