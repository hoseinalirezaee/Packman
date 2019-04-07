using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Logic;
namespace AI_P1.gui_elements
{
    //public enum EnvType { Wall = 1, Empty = 2, Food = 3, Packman = 4 };

    public partial class EnvBlock : UserControl
    {
        private EnvType type;
        public EnvType Type
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

        public delegate void TypeChangeEventHandler(object sender, EnvType newType);
        public event TypeChangeEventHandler TypeChanged;

        public EnvBlock()
        {
            InitializeComponent();
            TypeChanged += EnvBlock_TypeChanged;
        }

        private void EnvBlock_TypeChanged(object sender, EnvType newType)
        {
            var block = sender as EnvBlock;
            block.grid.Children.Clear();
            switch (newType)
            {
                case EnvType.Wall:
                    block.grid.Children.Add(new Rectangle { Fill = new SolidColorBrush(Colors.Brown) });// = new SolidColorBrush(Colors.Brown);
                    break;
                case EnvType.Empty:
                    block.grid.Children.Add(new Rectangle { Fill = new SolidColorBrush(Colors.White) });
                    break;
                case EnvType.Food:
                    block.grid.Children.Add(new Ellipse { Fill = new SolidColorBrush(Colors.Black) });
                    break;
                case EnvType.Packman:
                    block.grid.Children.Add(new Ellipse { Fill = new SolidColorBrush(Colors.Yellow) });
                    break;
                default:
                    throw new Exception("Type not defined.");
            }
        }
    }
}
