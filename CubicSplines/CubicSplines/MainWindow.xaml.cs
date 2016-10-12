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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Polyline polyline = new Polyline();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SplineNode.DraggedNode == null)
            {
                SplineNode newNode = new SplineNode(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
                canvas.Children.Add(newNode);
                newNode.NodeRemove += removeNode;
                newNode.NodeMoved += NodeMoved;
                Canvas.SetLeft(newNode, newNode.X - newNode.Width / 2);
                Canvas.SetTop(newNode, newNode.Y - newNode.Height / 2);
                if (SplineNode.listOfAllNodes.Count >= 2)
                {
                    DrawLine();
                }

            }
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (SplineNode.DraggedNode != null)
            {
                
                SplineNode.DraggedNode.MoveTo(e.GetPosition(canvas).X, e.GetPosition(canvas).Y);
                Canvas.SetLeft(SplineNode.DraggedNode, SplineNode.DraggedNode.X - SplineNode.DraggedNode.Width / 2);
                Canvas.SetTop(SplineNode.DraggedNode, SplineNode.DraggedNode.Y - SplineNode.DraggedNode.Height / 2);
            }
        }

        private void canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            SplineNode.MainWindow_canvas_MouseLeave(sender, e);
        }

        public void removeNode (object sender, EventArgs e)
        {
            canvas.Children.Remove(sender as UIElement);
        }

        void DrawLine()
        {
            polyline.Points = new PointCollection(Spline.GetPoints(SplineNode.listOfAllNodes));
           
            if (!canvas.Children.Contains(polyline))
            {
                canvas.Children.Add(polyline);
                polyline.Stroke = System.Windows.Media.Brushes.SlateGray;
                polyline.StrokeThickness = 2;
                Canvas.SetZIndex(polyline, -1);
            }
            
        }

        private void NodeMoved(object sender, EventArgs e)
        {
            if (SplineNode.listOfAllNodes.Count >= 2)
            {
                DrawLine();
            }
        }

        private void mainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            rect.Width = mainWindow.ActualWidth;
            rect.Height = mainWindow.ActualHeight;
        }
    }
}
