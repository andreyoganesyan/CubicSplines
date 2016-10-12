using System;
using System.Collections.Generic;
using System.Linq;
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

namespace CubicSplines
{
    /// <summary>
    /// Interaction logic for SplineNode.xaml
    /// </summary>
    public partial class SplineNode : UserControl
    {
        public static List<SplineNode> listOfAllNodes = new List<SplineNode>();
        public event EventHandler NodeRemove;
        public event EventHandler NodeMoved;
        public static SplineNode DraggedNode { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public SplineNode(double X, double Y)
        {
            InitializeComponent();
            this.X = X;
            this.Y = Y;
            listOfAllNodes.Add(this);
            
        }

        public void MoveTo(double x, double y)
        {
            this.X = x;
            this.Y = y;
            NodeMoved(this, null);
        }
        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                listOfAllNodes.Remove(this);
                NodeRemove(this, null);
                NodeMoved(this, null);
            }
            else {
                DraggedNode = this;
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DraggedNode = null;

        }
        
        public static void MainWindow_canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            DraggedNode = null;
        }
    }
}
