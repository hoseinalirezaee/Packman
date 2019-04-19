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
using Microsoft.Win32;
using AI_P1.io;

namespace AI_P1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ToggleButton[] tools = new ToggleButton[4];
        private ToggleButton selectedTool = null;

        private State initialState = null;
        private ActionSecuence actionSeq = null;

        public MainWindow()
        {
            InitializeComponent();

            wallTButton.Tag = EnvType.Wall;
            emptyTButton.Tag = EnvType.Empty;
            foodTButton.Tag = EnvType.Food;
            packmanTButton.Tag = EnvType.Packman;

            tools[0] = wallTButton;
            tools[1] = emptyTButton;
            tools[2] = foodTButton;
            tools[3] = packmanTButton;

            env.RowCount = 4;
            env.ColumnCount = 6;

            
        }

        private void SolveButtonClicked(object sender, RoutedEventArgs e)
        {
            initialState = env.GetState();

            if (initialState.Packman != null)
            {
                Algorithms algo = (Algorithms)int.Parse((algorithm.SelectedItem as ComboBoxItem).Tag.ToString());

                actionSeq = ProblemSolver.Solve(initialState, algo);

                MessageBox.Show("Done.");

                resetToInitStateBtn.IsEnabled = true;
                showPathBtn.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Packman not set.");
            }
        }

        #region Animating Path
        private void ShowButtonClicked(object sender, RoutedEventArgs e)
        {
            if (actionSeq.Size != 0)
            {
                controlPanel.IsEnabled = false;
                env.Show(initialState);

                BackgroundWorker worker = new BackgroundWorker
                {
                    WorkerReportsProgress = true
                };
                worker.ProgressChanged += StateChanged;
                worker.DoWork += ShowRoute;
                worker.RunWorkerCompleted += RunWorkerCompleted;

                worker.RunWorkerAsync(new WorkerArgs { ActionSecuence = actionSeq });
            }
            else
            {
                MessageBox.Show(this, "No path found");
            }
        }

        private void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            env.Show(e.UserState as State);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(this, "Routing Completed");
            controlPanel.IsEnabled = true;
        }

        private class WorkerArgs
        {
            public ActionSecuence ActionSecuence { get; set; }
        }

        private void ShowRoute(object sender, DoWorkEventArgs e)
        {
            ActionSecuence actions = (e.Argument as WorkerArgs).ActionSecuence;
            var worker = sender as BackgroundWorker;

            Thread.Sleep(500);
            for (int i = 0; i < actions.Size; i++)
            {
                worker.ReportProgress(i, actions[i]);
                Thread.Sleep(500);
            }
        }

        private new void StateChanged(object sender, ProgressChangedEventArgs e)
        {
            Actions action = (Actions)e.UserState;

            switch (action)
            {
                case Actions.LEFT:
                    env.Packman.Left();
                    break;
                case Actions.UP:
                    env.Packman.Up();
                    break;
                case Actions.RIGHT:
                    env.Packman.Right();
                    break;
                case Actions.DOWN:
                    env.Packman.Down();
                    break;
            }
        }
        #endregion Animating Path
        private void ToggleButtonChecked(object sender, RoutedEventArgs e)
        {
            env.SelectedTool = (sender as ToggleButton).Tag as EnvType?;
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

        private void LoadButtonClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            var status = openFile.ShowDialog(this);
            if (status == true)
            {
                string path = openFile.FileName;
                string initialState = IOHandler.LoadInitialState(path);
                State s = new State(initialState);
                env.Show(s);
            }
        }

        private void SaveButtonClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            var status = saveFile.ShowDialog(this);
            if (status == true)
            {
                string path = saveFile.FileName;
                StringWriter strWriter = new StringWriter();

                for (int i = 0; i < env.RowCount; i++)
                {
                    for (int j = 0; j < env.ColumnCount; j++)
                    {
                        switch (env[i, j])
                        {
                            case EnvType.Wall:
                                strWriter.Write('%');
                                break;
                            case EnvType.Empty:
                                strWriter.Write(' ');
                                break;
                            case EnvType.Food:
                                strWriter.Write('.');
                                break;
                            case EnvType.Packman:
                                strWriter.Write('p');
                                break;
                        }
                    }
                    strWriter.Write(strWriter.NewLine);
                }

                IOHandler.SaveInitialState(path, strWriter.ToString());
                MessageBox.Show("Problem saved successfully");
            }
        }

        private void ResetToInitState(object sender, RoutedEventArgs e)
        {
            if (initialState != null)
                env.Show(initialState);
        }

        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            initialState = null;
            actionSeq = null;
            showPathBtn.IsEnabled = false;
            resetToInitStateBtn.IsEnabled = false;

            env.Reset();
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

