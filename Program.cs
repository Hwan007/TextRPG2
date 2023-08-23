using System.Reflection.Metadata.Ecma335;

public class Program
{
    public static void Main(string[] args)
    {

    }
}

public class FourWeekHomework
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

    public class Goblin : Monster
    {
        private static int count;
        public Goblin(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
        }
    }

    public class Dragon : Monster
    {
        private static int count;
        public Dragon(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
        }
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

    public class HealthPotion : IUsableItem
    {
        public string Name { get; private set; }
        public int Point { get; private set; }
        public BattleEffect BattleEffect { get; private set; }
        public eUsableType UsableType { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public HealthPotion(string name, int point, eUsableType type, BattleEffect ef)
        {
            Name = name;
            Point = point;
            UsableType = type;
            BattleEffect = ef;
            EffectNode = new LinkedListNode<IEffect>(ef);
        }

        public void UseItem(Warrior character)
        {
            character.Effects.AddLast(EffectNode);
        }
        public void Draw()
        {

        }
    }

    public class StrengthPotion : IUsableItem
    {
        public string Name { get; private set; }
        public int Point { get; private set; }
        public BattleEffect BattleEffect { get; private set; }
        public eUsableType UsableType { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public StrengthPotion(string name)
        {
            Name = name;
        }
        public void UseItem(Warrior character)
        {
            throw new NotImplementedException();
        }
        public void Draw()
        {

        }
    }

    public class Weapon : IEquipment
    {
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public string Name { get; private set; }
        public EquipEffet? EquipEffect { get; private set; }
        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public Weapon(string name, int hp, int atk)
        {
            Name = name;
            Health = hp;
            Attack = atk;
        }
        public Weapon(string name, int hp, int atk, EquipEffet effect)
        {
            Name = name;
            Health = hp;
            Attack = atk;
            EquipEffect = effect;
            EffectNode = new LinkedListNode<IEffect>(effect);
        }
        public void Draw()
        {
            Console.WriteLine($"{Name} => HP({Health}) ATK({Attack})");
        }

        public void UnuseItem(Warrior character)
        {
            character.Equips.Remove(this);
            if (EquipEffect != null && EffectNode != null)
                character.Effects.Remove(EffectNode);
            character.ReStat();
        }

        public void UseItem(Warrior character)
        {
            character.Equips.AddLast(this);
            if (EquipEffect != null && EffectNode != null)
                character.Effects.AddLast(EffectNode);
            character.ReStat();
        }
    }

    public class Armor : IEquipment
    {
        public int Health { get; private set; }
        public int Attack { get; private set; }
        public string Name { get; private set; }
        public EquipEffet? EquipEffect { get; private set; }

        public LinkedListNode<IEffect> EffectNode { get; private set; }

        public Armor(string name, int hp, int atk)
        {
            Name = name;
            Health = hp;
            Attack = atk;
        }
        public Armor(string name, int hp, int atk, EquipEffet effect)
        {
            Name = name;
            Health = hp;
            Attack = atk;
            EquipEffect = effect;
            EffectNode = new LinkedListNode<IEffect>(effect);
        }

        public void Draw()
        {
            Console.WriteLine($"{Name} => HP({Health}) ATK({Attack})");
        }

        public void UnuseItem(Warrior character)
        {
            character.Equips.Remove(this);
            if (EquipEffect != null && EffectNode != null)
                character.Effects.Remove(EffectNode);
            character.ReStat();
        }

        public void UseItem(Warrior character)
        {
            character.Equips.AddLast(this);
            if (EquipEffect != null && EffectNode != null)
                character.Effects.AddLast(EffectNode);
            character.ReStat();
        }
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

    public class EquipEffet : IEffect
    {
        public string Name { get; private set; }
        public int ATK { get; private set; }
        public int HP { get; private set; }
        public eEffectType EffectType { get; private set; }

        public EquipEffet(string name, eEffectType effectType)
        {
            Name = name;
            EffectType = effectType;
        }

        public void AddEffect(ICharacter character)
        {
            character.Effects.AddLast(this);
        }

        public int CalEffectPoint(int basePoint)
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            Console.WriteLine("{0} => {1}{2}", Name, ATK == 0 ? "" : $"ATK {ATK} ", HP == 0 ? "" : $"HP {HP}");
        }
    }

    public class BattleEffect : IEffect
    {
        public string Name { get; private set; }
        public int Time { get; private set; }
        public int? ATK { get; private set; }
        public int? HP { get; private set; }
        public eEffectType EffectType { get; private set; }

        public BattleEffect(string name, eEffectType ef, int time, int? atk, int? hp)
        {
            Name = name;
            EffectType = ef;
            Time = time;

            if ((ef == eEffectType.OnBattleAttack || ef == eEffectType.OnAttack) && atk.HasValue) { ATK = atk.Value; }
            else throw new Exception("Effect type and stat is not match.");
            if ((ef == eEffectType.OnBattleHealth || ef == eEffectType.OnTotalHealth) && hp.HasValue) { HP = hp.Value; }
            else throw new Exception("Effect type and stat is not match.");
        }

        public void Draw()
        {
            Console.WriteLine("{0} => {1}{2}가 {3}턴 동안 적용", Name, ATK == 0 ? "" : $"ATK {ATK} ", HP == 0 ? "" : $"HP {HP}", Time);
        }

        public int CalEffectPoint(int basePoint)
        {
            int point = 0;
            if (Time > 0) --Time;
            switch (EffectType)
            {
                case eEffectType.OnTotalHealth:

                    break;
                case eEffectType.OnAttack:

                    break;
                case eEffectType.OnBattleHealth:

                    break;
                case eEffectType.OnBattleAttack:

                    break;
            }
            return point;
        }

        public void AddEffect(ICharacter character)
        {
            character.Effects.AddLast(this);
        }
    }

    public struct Vector2
    {
        public int x, y;
    }

    public class Stage
    {
        public string Name { get; private set; }
        public int Difficulty { get; private set; }

        public Stage(string name, Vector2 size, int difficulty)
        {
            Name = name;
            Difficulty = difficulty;
        }
        public void Start()
        {

        }
        public void Draw()
        {

        }

    }
}