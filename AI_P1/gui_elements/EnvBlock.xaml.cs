using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AI_P1.gui_elements
{
    public enum EnvBlockType { Wall = 1, Empty = 2, Food = 3, Packman = 4 };

    public partial class EnvBlock : UserControl
    {
        private EnvBlockType type;
        public EnvBlockType Type
        {
            get
            {
                return type;
            }
            set
            {
                if (type != value)
                {
                    type = value;
                    TypeChanged?.Invoke(this, value);
                }
            }
        }

        public delegate void TypeChangeEventHandler(object sender, EnvBlockType newType);
        public event TypeChangeEventHandler TypeChanged;

        public EnvBlock()
        {
            InitializeComponent();
            TypeChanged += EnvBlock_TypeChanged;
        }

        private void EnvBlock_TypeChanged(object sender, EnvBlockType newType)
        {
            var block = sender as EnvBlock;
            block.grid.Children.Clear();
            switch (newType)
            {
                case EnvBlockType.Wall:
                    block.grid.Background = new SolidColorBrush(Colors.Brown);
                    break;
                case EnvBlockType.Empty:
                    block.grid.Background = new SolidColorBrush(Colors.White);
                    break;
                case EnvBlockType.Food:
                    block.grid.Children.Add(new Ellipse { Fill = new SolidColorBrush(Colors.Black) });
                    break;
                case EnvBlockType.Packman:
                    block.grid.Children.Add(new Ellipse { Fill = new SolidColorBrush(Colors.Yellow) });
                    break;
                default:
                    throw new Exception("Type not defined.");
            }
        }
    }
}
