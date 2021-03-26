using System.Collections.Generic;
using System.IO;
using System.Linq;
using Phloem.Models;

namespace Phloem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        public MainWindow()
        {
            List<float> times = new();
            InitializeComponent();
            Game game = new();
            StreamWriter sw = new("latest.log");
            for (int i = 0; i < 1000; i++)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                game.FastForward();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                times.Add(elapsedMs);
                sw.WriteLine($"number generations: {game.GenerationNumber.ToString()}");
                sw.WriteLine($"execution time: {elapsedMs.ToString()}ms");
                sw.WriteLine($"mean execution time: {((int) times.Aggregate((x, y) => x + y) / times.Count).ToString()}ms");
                sw.WriteLine();
                sw.Flush();
            }
        }
    }
}
