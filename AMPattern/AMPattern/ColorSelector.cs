using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace AMPattern
{
    public class ColorSelector
    {
        private Brush[] colors = new Brush[]
                             {
                                 Brushes.Black,
                                 Brushes.SaddleBrown,
                                 Brushes.Red,
                                 Brushes.Orange,
                                 Brushes.Yellow,
                                 Brushes.Green,
                                 Brushes.Blue,
                                 Brushes.Violet,
                                 Brushes.Gray,
                                 Brushes.White,
                             };

        public Brush GetColor(int i)
        {
            return colors[i%10];
        }
    }
}
