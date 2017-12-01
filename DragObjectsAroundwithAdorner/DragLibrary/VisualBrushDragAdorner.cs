using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragObjectsAroundwithAdorner
{
    public class VisualBrushDragAdorner : Adorner
    {

        private double _leftOffset;
        private double _topOffset;
        
        UIElement _child;
        UIElement _owner;


        public VisualBrushDragAdorner(UIElement owner, UIElement adornedElement, bool useVisualBrush, double opacity) : base(adornedElement)
        {
            _owner = owner;
            FrameworkElement theElement = adornedElement as FrameworkElement;


            if (useVisualBrush)

            {

                VisualBrush _brush = new VisualBrush(adornedElement);

                _brush.Opacity = opacity;

                Rectangle r = new Rectangle();

                r.RadiusX = 3;

                r.RadiusY = 3;

                r.Width = adornedElement.DesiredSize.Width;

                r.Height = adornedElement.DesiredSize.Height;

                r.Width = theElement.ActualWidth ;

                r.Height = theElement.ActualHeight ;

                //XCenter = adornedElement.DesiredSize.Width / 2;

                //YCenter = adornedElement.DesiredSize.Height / 2;


                r.Fill = _brush;

                _child = r;


            }

            else

                _child = adornedElement;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _child.Measure(constraint);
            return _child.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _child.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _child;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void UpdatePosition(double left, double top, AdornerLayer _theAdornerLayer)
        {
            _leftOffset = left;
            _topOffset = top;
            if (_theAdornerLayer != null)
            {
                _theAdornerLayer.Update(this.AdornedElement);
            }
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            GeneralTransformGroup result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
            return result;
        }

        public void Destroy()
        {
          //  _adornerLayer.Remove(this);
           
           
        }

    }
}
