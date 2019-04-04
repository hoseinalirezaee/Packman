using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace AI_P1.gui_elements
{
    
    public partial class GameEnviroment : UserControl/*, INotifyPropertyChanged*/
    {
        #region Extra
        //public event PropertyChangedEventHandler PropertyChanged;

        //private void OnPropertyChanged(string propName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        //}

        //private delegate void ColumnChangedEventHandler(object sender, int newValue);
        //private event ColumnChangedEventHandler ColumnChanged;
        private int columnCount;
        public int ColumnCount
        {
            get
            {
                return columnCount;
            }
            set
            {
                if (columnCount != value)
                {
                    columnCount = value;
                    GameEnviroment_ColumnChanged(this, value);
                    //ColumnChanged?.Invoke(this, value);
                }
            }
        }
        //public static readonly DependencyProperty ColumnCountProperty 
        //    = DependencyProperty.Register(
        //        "ColumnCount",
        //        typeof(int),
        //        typeof(UserControl),
        //        new PropertyMetadata(new PropertyChangedCallback(OnColumnCountChanged)));

        //private static void OnColumnCountChanged(
        //    DependencyObject d,
        //    DependencyPropertyChangedEventArgs e)
        //{
        //    (d as GameEnviroment).ColumnCount = (int)e.NewValue;
        //}


        //private delegate void RowChangedEventHandler(object sender, int newValue);
        //private event RowChangedEventHandler RowChanged;
        private int rowCount;
        public int RowCount
        {
            get
            {
                return rowCount;
            }
            set
            {
                if (rowCount != value)
                {
                    rowCount = value;
                    GameEnviroment_RowChanged(this, value);
                    //RowChanged?.Invoke(this, value);
                }
            }
        }
        //public static readonly DependencyProperty RowCountProperty
        //    = DependencyProperty.Register(
        //        "RowCount",
        //        typeof(int),
        //        typeof(UserControl),
        //        new PropertyMetadata(new PropertyChangedCallback(OnRowCountChanged)));

        //private static void OnRowCountChanged(
        //    DependencyObject d,
        //    DependencyPropertyChangedEventArgs e)
        //{
        //    (d as GameEnviroment).RowCount = (int)e.NewValue;
        //}

        

        
        #endregion Extra

        #region Relations

        public void Show(Logic.State state)
        {
            if (gridElements.GetLength(0) != state.RowSize
                ||
                gridElements.GetLength(1) != state.ColumnSize)
            {
                this.RowCount = state.RowSize;
                this.ColumnCount = state.ColumnSize;
            }

            for (int i = 0; i < state.RowSize; i++)
            {
                for (int j = 0; j < state.ColumnSize; j++)
                {
                    gridElements[i, j].Type = (EnvBlockType)state[i, j];
                }
            }
        }

        #endregion

        private EnvBlock[,] gridElements;

        public EnvBlockType this[int i, int j]
        {
            get
            {
                return gridElements[i, j].Type;
            }
        }

        public EnvBlockType? SelectedTool { get; set; } = null;

        public GameEnviroment()
        {
            InitializeComponent();
            //ColumnChanged += GameEnviroment_ColumnChanged;
            //RowChanged += GameEnviroment_RowChanged;

            RowCount = 1;
            ColumnCount = 1;
            
        }

        private void GameEnviroment_RowChanged(object sender, int newValue)
        {
            GameEnviroment env = sender as GameEnviroment;
            env.grid.RowDefinitions.Clear();
            for (int i = 0; i < newValue; i++)
            {
                env.grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star)});
            }
            AddButon();
        }

        private void GameEnviroment_ColumnChanged(object sender, int newValue)
        {
            GameEnviroment env = sender as GameEnviroment;
            env.grid.ColumnDefinitions.Clear();
            for (int i = 0; i < newValue; i++)
            {
                env.grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)});
            }
            AddButon();
            
        }

        private void AddButon()
        {
            gridElements = new EnvBlock[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    gridElements[i, j] = new EnvBlock { Type = EnvBlockType.Empty};
                    gridElements[i, j].MouseUp += GameEnviroment_MouseUp;
                    grid.Children.Add(gridElements[i, j]);
                    Grid.SetRow(gridElements[i, j], i);
                    Grid.SetColumn(gridElements[i, j], j);
                }
            }
        }

        private void GameEnviroment_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedTool != null)
            {
                (sender as EnvBlock).Type = (EnvBlockType)SelectedTool;
            }
        }
    }
}
