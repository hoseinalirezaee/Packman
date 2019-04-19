using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Logic;
namespace AI_P1.gui_elements
{
    public class Packman
    {
        private EnvBlock[,] env;
        public Packman(EnvBlock[,] env, int row, int col)
        {
            this.env = env;
            Row = row;
            Column = col;
        }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public void Left()
        {
            if (Column - 1 >= 0
                    &&
                    env[Row, Column - 1].Type != EnvType.Wall)
            {
                env[Row, Column].Type = EnvType.Empty;
                env[Row, --Column].Type = EnvType.Packman;
            }
            else
            {
                throw new Exception("Packman can not go left.");
            }
        }
        public void Right()
        {
            if (Column + 1 < env.GetLength(1)
                    &&
                    env[Row, Column + 1].Type != EnvType.Wall)
            {
                env[Row, Column].Type = EnvType.Empty;
                env[Row, ++Column].Type = EnvType.Packman;
            }
            else
            {
                throw new Exception("Packman can not go right.");
            }
        }
        public void Up()
        {
            if (Row - 1 >= 0
                    &&
                    env[Row - 1, Column].Type != EnvType.Wall)
            {
                env[Row, Column].Type = EnvType.Empty;
                env[--Row, Column].Type = EnvType.Packman;
            }
            else
            {
                throw new Exception("Packman can not go up.");
            }
        }
        public void Down()
        {
            if (Row + 1 < env.GetLength(0)
                    &&
                    env[Row + 1, Column].Type != EnvType.Wall)
            {
                env[Row, Column].Type = EnvType.Empty;
                env[++Row, Column].Type = EnvType.Packman;
            }
            else
            {
                throw new Exception("Packman can not go down.");
            }
        }
    }

    public partial class GameEnviroment : UserControl
    {
        #region Private Fields

        private EnvBlock[,] env;

        private int rowCount;
        private int columnCount;

        #endregion

        #region Properties

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
                }
            }
        }

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
                }
            }
        }

        public EnvType? SelectedTool { get; set; } = null;

        public Packman Packman { get; private set; }

        #endregion

        #region Public Functions

        public GameEnviroment()
        {
            InitializeComponent();
            RowCount = 1;
            ColumnCount = 1;
        }

        public EnvType this[int i, int j]
        {
            get
            {
                return env[i, j].Type;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    env[i, j].Type = EnvType.Empty;
                }
            }
        }

        #endregion

        #region Private Functions

        private void GameEnviroment_RowChanged(object sender, int newValue)
        {
            GameEnviroment env = sender as GameEnviroment;
            env.grid.RowDefinitions.Clear();
            for (int i = 0; i < newValue; i++)
            {
                env.grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            AddButon();
        }

        private void GameEnviroment_ColumnChanged(object sender, int newValue)
        {
            GameEnviroment env = sender as GameEnviroment;
            env.grid.ColumnDefinitions.Clear();
            for (int i = 0; i < newValue; i++)
            {
                env.grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            AddButon();

        }

        private void AddButon()
        {
            env = new EnvBlock[rowCount, columnCount];
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    env[i, j] = new EnvBlock { Type = EnvType.Empty };
                    env[i, j].MouseUp += GameEnviroment_MouseUp;
                    grid.Children.Add(env[i, j]);
                    Grid.SetRow(env[i, j], i);
                    Grid.SetColumn(env[i, j], j);
                }
            }
        }

        private void GameEnviroment_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SelectedTool != null)
            {
                var block = (sender as EnvBlock);
                block.Type = (EnvType)SelectedTool;
                
                if (block.Type == EnvType.Packman)
                {
                    int newPackmanRow = Grid.GetRow(block);
                    int newPackmanCol = Grid.GetColumn(block);

                    if (Packman != null)
                    {
                        if (Packman.Row != newPackmanRow || Packman.Column != newPackmanCol)
                        {
                            env[Packman.Row, Packman.Column].Type = EnvType.Empty;
                            Packman = new Packman(env, newPackmanRow, newPackmanCol);
                        }
                    }
                    else
                    {
                        Packman = new Packman(env, newPackmanRow, newPackmanCol);
                    }
                }
            }
        }

        #endregion

        #region Relation functions to other objects

        public void Show(State state)
        {
            if (env.GetLength(0) != state.RowSize
                ||
                env.GetLength(1) != state.ColumnSize)
            {
                this.RowCount = state.RowSize;
                this.ColumnCount = state.ColumnSize;
            }

            int numberOfPackman = 0;
            int packmanRow = -1;
            int packmanCol = -1;


            for (int i = 0; i < state.RowSize; i++)
            {
                for (int j = 0; j < state.ColumnSize; j++)
                {
                    env[i, j].Type = (EnvType)state[i, j];
                    if (env[i, j].Type == EnvType.Packman)
                    {
                        numberOfPackman++;
                        packmanRow = i;
                        packmanCol = j;
                    }
                }
            }

            if (numberOfPackman != 1)
                throw new Exception("Number of packman must be exactly one.");
            else
                Packman = new Packman(env, packmanRow, packmanCol);
        }

        public State GetState()
        {
            State state = new State(env.GetLength(0), env.GetLength(1));
            for (int i = 0; i < env.GetLength(0); i++)
            {
                for (int j = 0; j < env.GetLength(1); j++)
                {
                    state[i, j] = env[i, j].Type;
                }
            }
            return state;
        }

        #endregion
    }
}