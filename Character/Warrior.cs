public partial class FourWeekHomework
{
    /// <summary>
    /// 유저가 사용하는 캐릭터
    /// </summary>
    public class Warrior : ICharacter
    {
        public string Name { get; private set; }
        public int Health { get; private set; }
        public int TotalHealth { get; private set; }
        public int Attack { get; private set; }
        public bool IsDead { get { return Health == 0 ? true : false; } }
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
        /// <param name="hp">초기 HP</param>
        /// <param name="atk">초기 ATK</param>
        public Warrior(string name, int hp, int atk)
        {
            Name = name;
            Health = BaseHealth = hp;
            Attack = BaseAttack = atk;
            Effects = new LinkedList<IEffect>();
            Equips = new LinkedList<IEquipment>();
        }

        public void TakeDamage(int damage)
        {
            if (damage >= 0)
                Health = Health - damage < 0 ? 0 : Health;
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
                ba += eq.Attack;
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
            var effects = from LinkedListNode<IEffect> ef in Effects
                          where ef.ValueRef.GetType() == typeof(BattleEffect)
                          select ef;
            foreach (var ef in effects)
            {
                BattleEffect? battleEf = ef.ValueRef as BattleEffect;

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
                    Thread.Sleep(200);
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
                        Effects.Remove(ef);
                        Thread.Sleep(200);
                    }
                }
            }
        }

        public void DrawStatus(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.WriteLine(Health);
            Console.WriteLine(Attack);
            foreach (IEquipment item in Equips)
            {
                item.Draw();
            }
            foreach (IEffect ef in Effects)
            {
                ef.Draw();
            }
        }
    }
}