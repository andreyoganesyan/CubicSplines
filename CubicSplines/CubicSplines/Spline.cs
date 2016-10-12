using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CubicSplines
{
    class Spline
    {
        //static Spline[] splines;
        double a, b, c, d, x;
        //public static List<SplineNode> Nodes = new List<SplineNode>();
        
        public static List<Point> GetPoints(List<SplineNode> nodes)
        {
            nodes.Sort((a, b) => (int)(a.X - b.X));
            int n = nodes.Count;
            double[] x = new double[n];
            double[] y = new double[n];
            for (int i = 0; i < n; i++)
            {
                x[i] = nodes[i].X;
                y[i] = nodes[i].Y;
            }

            return Interpolate(BuildSplines(x, y, n));
        }
        
        static Spline[] BuildSplines(double[] x, double[] y, int n)
        {
            Spline[] splines = new Spline[n];

            for (int i = 0; i < n; ++i)
            {
                splines[i] = new Spline();
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = splines[n - 1].c = 0.0;

            double[] alpha = new double[n - 1];
            double[] beta = new double[n - 1];
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < n - 1; ++i)
            {
                double hi = x[i] - x[i - 1];
                double hi1 = x[i + 1] - x[i];
                double A = hi;
                double C = 2.0 * (hi + hi1);
                double B = hi1;
                double F = 6.0 * ((y[i + 1] - y[i]) / hi1 - (y[i] - y[i - 1]) / hi);
                double z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }

            
            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];
            }

            
            for (int i = n - 1; i > 0; --i)
            {
                double hi = x[i] - x[i - 1];
                splines[i].d = (splines[i].c - splines[i - 1].c) / hi;
                splines[i].b = hi * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (y[i] - y[i - 1]) / hi;
            }
            return splines;
        }

        static List<Point> Interpolate(Spline[] splines)
        {
            List<Point> result = new List<Point>();
            for (double x = SplineNode.listOfAllNodes[0].X; x <= SplineNode.listOfAllNodes[SplineNode.listOfAllNodes.Count - 1].X; x += (SplineNode.listOfAllNodes[SplineNode.listOfAllNodes.Count - 1].X - SplineNode.listOfAllNodes[0].X) / 500)
            {


                int n = splines.Length;
                Spline s;

                if (x <= splines[0].x) // Если x меньше точки сетки x[0] - пользуемся первым эл-тов массива
                {
                    s = splines[1];
                }
                else if (x >= splines[n - 1].x) // Если x больше точки сетки x[n - 1] - пользуемся последним эл-том массива
                {
                    s = splines[n - 1];
                }
                else // Иначе x лежит между граничными точками сетки - производим бинарный поиск нужного эл-та массива
                {
                    int i = 0;
                    int j = n - 1;
                    while (i + 1 < j)
                    {
                        int k = i + (j - i) / 2;
                        if (x <= splines[k].x)
                        {
                            j = k;
                        }
                        else
                        {
                            i = k;
                        }
                    }
                    s = splines[j];
                }

                double dx = x - s.x;
                // Вычисляем значение сплайна в заданной точке по схеме Горнера (в принципе, "умный" компилятор применил бы схему Горнера сам, но ведь не все так умны, как кажутся)
                result.Add(new Point(x, s.a + (s.b + (s.c / 2.0 + s.d * dx / 6.0) * dx) * dx));
            }
            return result;
        }
    }
}
