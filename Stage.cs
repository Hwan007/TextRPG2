public partial class Game
{
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