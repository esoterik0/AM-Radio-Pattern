using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Tower.xaml
    /// </summary>
    public partial class Tower : UserControl
    {
        private double _ratio = 1.0;
        private double _phase = 0;
        private double _spacing = 0;
        private double _orient = 0;

        public double ratio
        {
            get { return _ratio; }
            set
            {
                if (_ratio == value) // avoid udate recursion when we set slider
                    return;

                _ratio = value;
                tRatio.Text = String.Format("{0:0.000}", value);
                sRatio.Value = value;
                CallChanged();
            }
        }

        public double phase
        {
            get { return _phase; }
            set
            {
                if (_phase == value) // avoid udate recursion when we set slider
                    return;

                _phase = value;
                tPhase.Text = String.Format("{0:0.0}", value);
                sPhase.Value = value;
                CallChanged();
            }
        }

        public double spacing
        {
            get { return _spacing; }
            set
            {
                if (_spacing == value) // avoid udate recursion when we set slider
                    return;

                _spacing = value;
                tSpace.Text = String.Format("{0:0.0}", value);
                sSpace.Value = value;
                CallChanged();
            }
        }

        public double orient
        {
            get { return _orient; }
            set
            {
                if (_orient == value) // avoid udate recursion when we set slider
                    return;

                _orient = value;
                tOrient.Text = String.Format("{0:0.0}", value);
                sOrient.Value = value;
                CallChanged();
            }
        }
        
        public Action changed { get; set; }

        public Tower()
        {
            InitializeComponent();

            var textParse = new Func<TextBox, double>(s => { double v; return Double.TryParse(s.Text, out v) ? v : Double.NaN; });

            ratio = 1.0;

            // our textboxen update the sliders which update the properties.
            // we don't want to use NewText because the user could still be typing.
            tRatio.LostFocus += (o, e) => sRatio.Value = textParse((TextBox)o);
            tPhase.LostFocus += (o, e) => sPhase.Value = textParse((TextBox)o);
            tSpace.LostFocus += (o, e) => sSpace.Value = textParse((TextBox)o);
            tOrient.LostFocus += (o, e) => sOrient.Value = textParse((TextBox)o);
            
            Action<Key, Action> ifEnter = (k, a) => { if (k == Key.Return) a(); };

            // if the user hits enter we want to update the values.
            tRatio.KeyDown += (o, k) => ifEnter(k.Key, () => sRatio.Value = textParse((TextBox)o));
            tPhase.KeyDown += (o, k) => ifEnter(k.Key, () => sPhase.Value = textParse((TextBox)o));
            tSpace.KeyDown += (o, k) => ifEnter(k.Key, () => sSpace.Value = textParse((TextBox)o));
            tOrient.KeyDown += (o, k) => ifEnter(k.Key, () => sOrient.Value = textParse((TextBox)o));

            sRatio.ValueChanged += (o, e) =>  ratio = sRatio.Value;
            sPhase.ValueChanged += (o, e) => phase = sPhase.Value;
            sSpace.ValueChanged += (o, e) => spacing = sSpace.Value;
            sOrient.ValueChanged += (o, e) => orient = sOrient.Value;

            // our sliders need a data context to bind to this.Background
            sRatio.DataContext = this;
            sPhase.DataContext = this;
            sSpace.DataContext = this;
            sOrient.DataContext = this;
        }

        // do the math for each antennas contribution in that direction
        public Point GetCart(double deg)
        {
            // convert to radians or the math won't work
            double ang = deg * (Math.PI / 180.0); 
            double rPhase = phase * (Math.PI / 180.0);
            double rSpace = spacing * (Math.PI / 180.0);
            double rOrient = orient * (Math.PI / 180.0);

            // antenna contribution for angle
            double beta = rPhase + rSpace*Math.Cos(rOrient - ang);

            // return total contribution
            return new Point(ratio*Math.Cos(beta), ratio*Math.Sin(beta));
        }
        
        private void CallChanged()
        {
            if (changed != null)
                changed();
        }
    }
}
