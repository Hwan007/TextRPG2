using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

public partial class Game
{
    /// <summary>
    /// 유저가 사용하는 캐릭터
    /// </summary>
    public class Warrior : ICharacter
    {
        public string Name { get; private set; }
        private int mHealth;
        public int Health
        {
            get => mHealth;
            private set
            {
                if (value > TotalHealth) mHealth = TotalHealth;
                else mHealth = value;
            }
        }
        public int TotalHealth { get; private set; }
        public int Attack { get; private set; }
        public bool IsDead { get { return Health <= 0 ? true : false; } }
        public int Level { get; private set; }
        public int Exp { get; private set; }
        public int BaseHealth { get; private set; }
        public int BaseAttack { get; private set; }

        public LinkedList<IEffect> Effects { get; private set; }
        public LinkedList<IEquipment> Equips { get; private set; }

        /// <summary>
        /// 플래이어 캐릭터 클래스
        /// </summary>
        /// <param name="name">이름</param>
        /// <param name="basehp">초기 HP</param>
        /// <param name="baseatk">초기 ATK</param>
        public Warrior(string name, int basehp, int baseatk)
        {
            Name = name;
            BaseHealth = basehp;
            BaseAttack = baseatk;
            Effects = new LinkedList<IEffect>();
            Equips = new LinkedList<IEquipment>();
            ReStat();
            Health = BaseHealth;
        }

        public void TakeDamage(int damage)
        {
            if (damage >= 0)
                Health = Health - damage < 0 ? 0 : Health - damage;
        }

        /// <summary>
        /// 레벨이 올라가며 HP 100와 공격력 10이 상승하고 HP을 전부 회복한다.
        /// </summary>
        public void LevelUP()
        {
            ++Level;
            BaseHealth += 100;
            BaseAttack += 10;
            HPRestore(TotalHealth);
        }

        /// <summary>
        /// 가진 경험치값이 100을 넘어가면 레벨업
        /// </summary>
        /// <param name="ex"></param>
        public void GetEx(int ex)
        {
            if (ex >= 0)
            {
                Exp += ex;
                while (Exp >= 100)
                {
                    LevelUP();
                    Exp -= 100;
                }
            }
        }

        public void HPRestore(int restorePoint)
        {
            if (restorePoint >= 0)
                Health = Health + restorePoint > TotalHealth ? TotalHealth : Health;
        }

        private int GetTotalHealth()
        {
            int totalPlus = 0;
            int bh = BaseHealth;
            foreach (IEquipment eq in Equips)
            {
                bh += eq.Health;
            }
            foreach (IEffect ef in Effects)
            {
                if (ef.EffectType == eEffectType.OnTotalHealth)
                {
                    totalPlus += ef.CalEffectPoint(bh);
                }
            }
            return totalPlus + bh;
        }

        private int GetAttack()
        {
            int totalPlus = 0;
            int ba = BaseAttack;
            foreach (IEquipment eq in Equips)
            {
                ba += eq.Point;
            }
            foreach (IEffect ef in Effects)
            {
                if (ef.EffectType == eEffectType.OnAttack)
                {
                    totalPlus += ef.CalEffectPoint(ba);
                }
            }
            return totalPlus + ba;
        }

        public void ReStat()
        {
            TotalHealth = GetTotalHealth();
            Attack = GetAttack();
        }

        public void TurnEffect()
        {
            // 턴마다 돌아오는 효과를 적용
            var effects = Effects.Where((x) => x.EffectType >= eEffectType.OnBattleHealth);
            List<IEffect> removeList = new List<IEffect>();
            foreach (var ef in effects)
            {
                BattleEffect? battleEf = ef as BattleEffect;

                if (battleEf != null)
                {
                    switch (battleEf.EffectType)
                    {
                        case eEffectType.OnBattleAttack:
                            // 한번만 적용
                            Attack += battleEf.CalEffectPoint(BaseAttack);
                            break;
                        case eEffectType.OnBattleHealth:
                            // 턴마다 적용
                            Health += battleEf.CalEffectPoint(BaseHealth);
                            break;
                    }
                    //Thread.Sleep(1000);
                    if (battleEf.Time <= 0)
                    {
                        switch (battleEf.EffectType)
                        {
                            case eEffectType.OnBattleAttack:
                                // 원상복구
                                Attack -= battleEf.CalEffectPoint(BaseAttack);
                                break;
                        }
                        // 완료된 효과 제거
                        removeList.Add(ef);
                        //Thread.Sleep(1000);
                    }
                }
            }
            foreach (var ef in removeList)
                Effects.Remove(ef);
        }

        public void Draw(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            Display.DrawImage(window, Display.eImageType.Warrior);
            sb.Append(Display.SBWithCustomColor($"\n{Name}  (Lv.{Level})\n"));
            sb.Append(Display.SBWithCustomColor($"{Attack}", eColorType.Red));
            sb.Append(" | ");
            sb.Append(Display.SBWithCustomColor($"{Health}/{TotalHealth}\n", eColorType.Green));
            Display.AddSBToWindow(window, sb);
            foreach (IEffect ef in Effects)
            {
                ef.Draw(window);
            }
        }

        public void DrawStatus(eWindowType window)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Display.SBWithCustomColor($"\n{Name}  (Lv.{Level})\n"));
            sb.Append(Display.SBWithCustomColor($"공격력 : "));
            sb.Append(Display.SBWithCustomColor($"{Attack}\n", eColorType.Red));
            sb.Append(Display.SBWithCustomColor($"체  력 : "));
            sb.Append(Display.SBWithCustomColor($"{Health}/{TotalHealth}\n", eColorType.Green));
            sb.Append(Display.SBWithCustomColor($"경험치 : "));
            sb.Append(Display.SBWithCustomColor($"{Exp}\n", eColorType.Gold));
            sb.Append(Display.SBWithCustomColor($"장비\n"));
            foreach (IEquipment equip in Equips)
            {
                equip.Draw(window);
            }
            Display.AddSBToWindow(window, sb);
        }

        public void EquipItem(IEquipment equipment)
        {
            equipment.UseItem(this);
            ReStat();
        }

        public void UseItem(IUsableItem item)
        {
            item.UseItem(this);
            ReStat();
        }


    }
}