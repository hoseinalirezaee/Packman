using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Logic;
using AI_P1.gui_elements;
using System.Globalization;
using System.Windows.Controls.Primitives;

namespace AI_P1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ToggleButton[] tools = new ToggleButton[4];
        private ToggleButton selectedTool = null;

        public MainWindow()
        {
            InitializeComponent();

            wallTButton.Tag = EnvBlockType.Wall;
            emptyTButton.Tag = EnvBlockType.Empty;
            foodTButton.Tag = EnvBlockType.Food;
            packmanTButton.Tag = EnvBlockType.Packman;

            tools[0] = wallTButton;
            tools[1] = emptyTButton;
            tools[2] = foodTButton;
            tools[3] = packmanTButton;

            env.RowCount = 4;
            env.ColumnCount = 6;
            


        }


        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            env.Show(e.UserState as State);
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            FileStream inputStream = new FileStream(@"E:\Daneshgah\Term 6\Artificial Intelligence and Expert Systems\Project\3.txt", FileMode.Open, FileAccess.Read);

            StreamReader reader = new StreamReader(inputStream);

            string data = reader.ReadToEnd();

            inputStream.Close();

            Problem problem = new Problem(data);

            var actionSeq = ProblemSolver.Solve(problem, Algorithms.BFS);

            State currentState = problem.InitialState;

            worker.ReportProgress(0, currentState);
            Thread.Sleep(2000);

            for (int i = 0; i < actionSeq.Size; i++)
            {
                currentState = problem.Result(currentState, actionSeq[i]);
                worker.ReportProgress(i, currentState);
                Thread.Sleep(500);
            }
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

        private void SolveButtonClicked(object sender, RoutedEventArgs e)
        {
            StringWriter strWriter = new StringWriter();
            int packCount = 0;

            for (int i = 0; i < env.RowCount; i++)
            {
                for (int j = 0; j < env.ColumnCount; j++)
                {
                    switch(env[i, j])
                    {
                        case EnvBlockType.Wall:
                            strWriter.Write('%');
                            break;
                        case EnvBlockType.Empty:
                            strWriter.Write(' ');
                            break;
                        case EnvBlockType.Food:
                            strWriter.Write('.');
                            break;
                        case EnvBlockType.Packman:
                            strWriter.Write('p');
                            packCount++;
                            break;
                    }
                }
                strWriter.Write(strWriter.NewLine);
            }

            if (packCount > 1)
            {
                MessageBox.Show("Number of packman can't be more than one.");
            }
            else
            {
                strWriter.Flush();
                Problem problem = new Problem(strWriter.ToString());
                ActionSecuence actionSeq = ProblemSolver.Solve(problem, Algorithms.BFS);
                env.Show(problem.InitialState);

                if (actionSeq.Size != 0)
                {
                    MessageBox.Show("Route found.");

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.WorkerReportsProgress = true;
                    worker.ProgressChanged += StateChanged;
                    worker.DoWork += ShowRoute;
                    worker.RunWorkerCompleted += RunWorkerCompleted;

                    worker.RunWorkerAsync(new WorkerArgs { Problem = problem, ActionSecuence = actionSeq });

                }
                else
                {
                    MessageBox.Show("No solution found.");
                }
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Routing Completed");
        }

        private class WorkerArgs
        {
            public Problem Problem { get; set; }
            public ActionSecuence ActionSecuence { get; set; }
        }

        private void ShowRoute(object sender, DoWorkEventArgs e)
        {
            Problem problem = (e.Argument as WorkerArgs).Problem;
            ActionSecuence actions = (e.Argument as WorkerArgs).ActionSecuence;
            State current = problem.InitialState;
            var worker = sender as BackgroundWorker;

            Thread.Sleep(500);
            for (int i = 0; i < actions.Size; i++)
            {
                current = problem.Result(current, actions[i]);
                worker.ReportProgress(i, current);
                Thread.Sleep(500);
            }
        }

        private void StateChanged(object sender, ProgressChangedEventArgs e)
        {
            env.Show(e.UserState as State);
        }

        private void ToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            env.SelectedTool = (sender as ToggleButton).Tag as EnvBlockType?;
            selectedTool = sender as ToggleButton;
            for (int i = 0; i < 4; i++)
            {
                if (sender as ToggleButton != tools[i])
                {
                    tools[i].IsChecked = false;
                }
            }
        }

        private void ToggleButtonUnchecked(object sender, RoutedEventArgs e)
        {
            if (selectedTool == sender as ToggleButton)
                env.SelectedTool = null;
        }
    }


    public class IntToTextConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null
                ||
                value.ToString() == String.Empty)
                return 0;
            return int.Parse(value as string);
        }
    }

    public class CountToSize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) * 100;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

