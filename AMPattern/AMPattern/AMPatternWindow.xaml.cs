using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMPattern
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AMPatternWindow : Window
    {
        private GeometryDrawing bkDrawing;
        private ColorSelector colors = new ColorSelector();
        private double strokeThickness = .005;
        private double towerRadius = .025;
        
        public AMPatternWindow()
        {
            InitializeComponent();

            TransformGroup xGroup = new TransformGroup();
            xGroup.Children.Add(new RotateTransform(-90));
            //xGroup.Children.Add(new ScaleTransform(-1, 1));
            pattern.LayoutTransform = xGroup;

            InitBackground(12);
            
            AddTower();
            AddTower(new Tower { phase = 106, spacing = 90, orient = 315 });
            AddTower(new Tower { phase = 90, spacing = 180, orient = 45 });
            AddTower(new Tower { phase = 196, spacing = 201, orient = 18 });
            TowerChanged();
        }

        private void InitBackground(int radials)
        {
            var path = new PathFigureCollection();

            for (int i = 1; i <= 10; i++)
            {
                double[] mag = Enumerable.Range(0, 60).Select(m => i/10.0).ToArray();
                path.Add(ComputePath(mag));
            }

            PathSegmentCollection radial = new PathSegmentCollection();
            Point zero = new Point(0,0);
            
            for (int i = 0; i < 360; i+=(360/radials))
            {
                double rad = (Math.PI/180)*i;
                radial.Add(new LineSegment(new Point(Math.Cos(rad), Math.Sin(rad)), true));
                radial.Add(new LineSegment(zero, false ));
            }

            path.Add(new PathFigure(zero, radial, false ));

            bkDrawing = new GeometryDrawing
                            {
                                Geometry = new PathGeometry(path),
                                Pen = new Pen(Brushes.Gray, strokeThickness),
                                Brush = Brushes.LightGray
                            };
        }

        private void SetPattern(double[] input)
        {
            var geo = new GeometryDrawing
                      {
                          Geometry = new PathGeometry(new PathFigureCollection {ComputePath(input)}),
                          Pen = new Pen(Brushes.Black, strokeThickness),
                          Brush = new SolidColorBrush(Colors.LightBlue) {Opacity = 0.5}
                      };

            var grp = new DrawingGroup();
            grp.Children.Add(bkDrawing);
            grp.Children.Add(geo);

            if(ShowTowers.IsChecked == true)
                grp.Children.Add(DrawTowers());
            
            pattern.Source = new DrawingImage(grp);
        }

        private Drawing DrawTowers()
        {
            Func<Tower, Point> map = t =>
                                     {
                                         double mag = t.spacing/360.0;
                                         double rad = t.orient*(Math.PI/180.0);
                                         return new Point(mag*Math.Cos(rad), mag*Math.Sin(rad));
                                     };

            var pen = new Pen(Brushes.Black, strokeThickness);
            var dg = new DrawingGroup
                     {
                         Children = new DrawingCollection(aStack.Children.Cast<Tower>().Select(map) // maps towers to points
                                    .Select((p, i) => new GeometryDrawing(colors.GetColor(i), pen, new EllipseGeometry(p, towerRadius, towerRadius)))) // maps points to geometry
                     };

            return dg;
        }

        /// <summary>
        /// mag is assumed to be the magnatude of evenly spaced polar vectors starting at 0 radians
        /// 
        /// Comptues the path figure to display the polar graph.
        /// </summary>
        /// <param name="mag">vector magnatudes</param>
        /// <returns>path figure of the polar graph</returns>
        private PathFigure ComputePath(double[] mag)
        {
            double radStep = 2.0*Math.PI/mag.Length;
            
            // project our vectors into points; 
            Point[] points = mag.Select((m, r) => new Point(m*Math.Cos(r*radStep), m*Math.Sin(r*radStep))).ToArray();

            // create and return our path figure
            return new PathFigure(points[0], new PathSegmentCollection(points.Skip(1).Select(p => new LineSegment(p, true))), true);
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            aStack.Children.Clear();
            AddTower();
            TowerChanged();
        }

        private void AddTower()
        {
            Tower t = new Tower();
            AddTower(t);
        }

        private void AddTower(Tower t)
        {
            t.Background = colors.GetColor(aStack.Children.Count);
            t.changed += TowerChanged;
            aStack.Children.Add(t);
        }

        private void TowerChanged()
        {
            int slices = 400; 

            double[] magn = new double[slices];
            
            for (int i=0; i<slices; i++)
            {
                Point p = new Point(0,0);

                foreach (Point np in aStack.Children.Cast<Tower>().Select(t => t.GetCart(i*360.0/slices)))
                {
                    p.X += np.X;
                    p.Y += np.Y;
                }

                magn[i] = Math.Sqrt(p.X*p.X + p.Y*p.Y);
            }

            double max = magn.Max(d => Math.Abs(d));

            SetPattern(magn.Select(d => d/max).ToArray());
        }

        private void NewTower(object sender, RoutedEventArgs e)
        {
            AddTower();
        }

        private void ToggleTowers(object sender, RoutedEventArgs e)
        {
            TowerChanged();
        }
    }
}
