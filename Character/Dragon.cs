public partial class Game
{
    public class Dragon : Monster
    {
        private static int count;
        public Dragon(string name, int hp, int atk) : base(name, hp, atk)
        {
            ++count;
            Name = $"{Name} {count}";
            int rand = Random.Shared.Next(1, 11);
            if (rand > 5)
            {
                Items.Add(new HealthPotion("회복약", 5, 20 + Random.Shared.Next(-5, 6), eUsableType.HealthRestore));
                Items.Add(new StrengthPotion("강화약", 5, 20 + Random.Shared.Next(-5, 6)));
            }
            else if (rand > 3)
            {
                Items.Add(new Weapon("용살검", 50, 50));
            }
            else if (rand > 1)
            {
                Items.Add(new Armor("용비늘갑", 50, 50));
            }
            else
            {
                Items.Add(new Weapon("무한한 삼위일체", 400, 50, new EquipEffet("주문검", eEffectType.OnAttack, null, 20)));
                Items.Add(new Armor("무언의 기생갑", 550, 80, new EquipEffet("공허 태생의 저항력", eEffectType.OnTotalHealth, 20, null)));
            }
        }

        public override void Draw(eWindowType window)
        {
            Display.DrawImage(window, Display.eImageType.Dragon);
            base.Draw(window);
        }
    }
}