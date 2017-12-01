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

namespace DragObjectsAroundwithAdorner
{
    /// <summary>
    /// Interaction logic for RectangleControl.xaml
    /// </summary>
    public partial class RectangleControl : UserControl
    {
        public RectangleControl()
        {
            InitializeComponent();
        }

        public RectangleControl(RectangleControl r)
        {
            InitializeComponent();
            this.rectangleUI.Height = r.rectangleUI.Height;
            this.rectangleUI.Width = r.rectangleUI.Width;
            this.rectangleUI.Fill = r.rectangleUI.Fill;
        }
    }
}
