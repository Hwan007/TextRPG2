public partial class FourWeekHomework
{
    /// <summary>
    /// 유저가 사용하는 캐릭터
    /// </summary>
    public class Warrior : ICharacter
    {
        private int mBaseHealth;
        private int mTotalHealth;
        private int mHealth;
        private int mBaseAttack;
        private int mAttack;
        private int mExperiance;

        public LinkedList<IEffect> Effects { get; private set; }
        public LinkedList<IEquipment> Equips { get; private set; }

        public string Name { get; private set; }
        public int Health { get => mHealth; private set => mHealth = value; }
        public int Attack { get => mAttack; private set => mAttack = value; }
        public bool IsDead { get; private set; }
        public int Level { get; private set; }
        //효과 적용은 Base Stat에만 적용하고 이를 적용하면 다른 값도 연동해서 바뀌도록.
        public int BaseHealth
        {
            get => mBaseHealth;
            private set
            {
                mBaseHealth = value;
                // TotalHealth를 재계산 하도록 한다.
                mTotalHealth = SetTotalHealth();
            }
        }
        public int BaseAttack
        {
            get => mBaseAttack;
            private set
            {
                mBaseAttack = value;
                // Attack을 재계산 하도록 한다.
                mAttack = SetAttack();
            }
        }
        public int TotalHealth { get => mTotalHealth; private set => mTotalHealth = value; }

        public void TakeDamage(int damage)
        {
            Health = Health - damage < 0 ? 0 : Health;
            if (Health == 0) IsDead = true;
        }

        /// <summary>
        /// 초기 스탯 HP=100, ATK=10
        /// </summary>
        /// <param name="name">이름</param>
        public Warrior(string name)
        {
            Name = name;
            IsDead = false;
            Health = BaseHealth = 100;
            Attack = BaseAttack = 10;
            Effects = new LinkedList<IEffect>();
            Equips = new LinkedList<IEquipment>();
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
            mExperiance += ex;
            if (mExperiance >= 100)
            {
                for (int i = 0; i < mExperiance / 100; ++i)
                {
                    LevelUP();
                }
                mExperiance %= 100;
            }
        }

        public void HPRestore(int restorePoint)
        {
            Health = Health + restorePoint > TotalHealth ? TotalHealth : Health;
        }

        private int SetTotalHealth()
        {
            int total = 0;
            int bh = BaseHealth;
            foreach (IEquipment eq in Equips)
            {
                bh += eq.Health;
            }
            foreach (IEffect ef in Effects)
            {
                if (ef.EffectType == eEffectType.OnTotalHealth)
                {
                    // 효과 적용 시키기
                    total += ef.CalEffectPoint(bh);
                }
            }
            return total + bh;
        }

        private int SetAttack()
        {
            int total = 0;
            int ba = BaseAttack;
            foreach (IEquipment eq in Equips)
            {
                ba += eq.Attack;
            }
            foreach (IEffect ef in Effects)
            {
                if (ef.EffectType == eEffectType.OnAttack)
                {
                    // 효과 적용 시키기
                    total += ef.CalEffectPoint(ba);
                }
            }
            return total + ba;
        }

        public void ReStat()
        {
            TotalHealth = SetTotalHealth();
            Attack = SetAttack();
        }

        public void TurnLoop()
        {
            // 턴마다 돌아오는 효과를 적용
            var effects = from LinkedListNode<IEffect> ef in Effects
                          where ef.ValueRef.GetType() == typeof(BattleEffect)
                          select ef;
            foreach (var ef in effects)
            {
                BattleEffect? eff = ef.ValueRef as BattleEffect;

                if (eff is BattleEffect valueOfEff)
                {
                    switch (eff.EffectType)
                    {
                        case eEffectType.OnBattleAttack:
                            // 한번만 적용
                            Attack += eff.CalEffectPoint(BaseAttack);
                            break;
                        case eEffectType.OnBattleHealth:
                            // 턴마다 적용
                            Health += eff.CalEffectPoint(BaseHealth);
                            break;
                    }

                    if (eff.Time <= 0)
                    {
                        switch (eff.EffectType)
                        {
                            case eEffectType.OnBattleAttack:
                                // 원상복구
                                Attack -= eff.CalEffectPoint(BaseAttack);
                                break;
                        }
                        // 완료된 효과 제거
                        Effects.Remove(ef);
                    }
                }
            }
            // 행동을 선택

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