﻿using System;
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

        private AdornerLayer _adornerLayer;
        private double _leftOffset;
        private double _topOffset;
        Rectangle _r;


        public VisualBrushDragAdorner(FrameworkElement adornedElement, AdornerLayer adornerLayer ) : base(adornedElement)
        {
            _adornerLayer = adornerLayer;
            VisualBrush _brush = new VisualBrush(adornedElement);
            _brush.Opacity = .75;

            Rectangle r = new Rectangle();
            r.IsHitTestVisible = false;
            
            r.Width = adornedElement.DesiredSize.Width;
            r.Height = adornedElement.DesiredSize.Height;
            r.Width = adornedElement.ActualWidth;
            r.Height = adornedElement.ActualHeight;
            r.Name = "theAdornerLayer";
           
            // XCenter = adornedElement.DesiredSize.Width / 2;
            // YCenter = adornedElement.DesiredSize.Height / 2;
            r.Fill = _brush;
            _r = r;
            _r.IsHitTestVisible = false ;
            _adornerLayer.IsHitTestVisible = false;
            _adornerLayer.Add(this);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _r.Measure(constraint);
            return _r.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _r.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _r;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void UpdatePosition(double left, double top)
        {
            _leftOffset = left;
            _topOffset = top;
            if (_adornerLayer != null)
            {
                _adornerLayer.Update(this.AdornedElement);
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
            _adornerLayer.Remove(this);
           
           
        }

    }
}