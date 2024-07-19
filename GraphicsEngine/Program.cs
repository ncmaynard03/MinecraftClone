namespace GraphicsEngine
{
    internal class Program
    {

        [STAThread]
        static void Main()
        {
            using (var game = new Game(1500, 1000, "OpenTK Triangle"))
            {
                game.Run();
            }
        }
    }
}