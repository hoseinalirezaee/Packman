using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic;

namespace AI_P1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //{   1, 1, 1, 1, 1, 1,
            //    1, 3, 1, 3, 1, 1,
            //    1, 1, 1, 1, 1, 1,
            //    1, 3, 1, 3, 1, 1,
            //    1, 4, 1, 1, 1, 1,
            //    1, 1, 1, 1, 1, 1,   }
           
            FileStream inputStream = new FileStream(@"E:\Daneshgah\Term 6\Artificial Intelligence and Expert Systems\Project\3.txt", FileMode.Open, FileAccess.Read);

            StreamReader reader = new StreamReader(inputStream);

            string data = reader.ReadToEnd();

            inputStream.Close();

            Problem problem = new Problem(data);

            var actionSeq = ProblemSolver.Solve(problem, Algorithms.BFS);

            String str = String.Empty;

            for (int i = 0; i < actionSeq.Size; i++)
            {
                str += ((int)actionSeq[i]).ToString();
            }

        }
    }
}

