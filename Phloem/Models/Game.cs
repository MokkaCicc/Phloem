namespace Phloem.Models
{
    public class Game
    {
        public int GenerationNumber;
        public readonly Generation FirstGeneration;
        public Generation LastGeneration { get; private set; }

        public Game()
        {
            GenerationNumber = 0;
            FirstGeneration = new Generation(10000, 10000);
            LastGeneration = FirstGeneration;
        }

        public void FastForward()
        {
            GenerationNumber++;
            LastGeneration = LastGeneration.AsyncGetNextGeneration();
        }
    }
}
