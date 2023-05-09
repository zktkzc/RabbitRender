namespace Rabbit_Sandbox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (Window window = new Window(800, 600, "LearnOpenTK"))
            {
                window.RenderFrequency = 60;
                window.Run();
            }
        }
    }
}