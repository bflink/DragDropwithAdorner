using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragObjectsAroundwithAdorner
{
    public class RectangleDragDecorator : BaseDecorator
    {
        private VisualBrushDragAdorner _visualBrushDragAdorner;
        private bool _isMouseDown;
        private object _data;
        private Point _dragStartPosition;
        private bool _isDragging;

        public RectangleDragDecorator() : base()
        {
            _isDragging = false;
            _isMouseDown = false;

            this.Loaded += new RoutedEventHandler(DraggableRectangle_Loaded);
        }

        private void DraggableRectangle_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(base.DecoratedUIElement is Rectangle))
            {
                throw new InvalidCastException(string.Format("ItemsControlDragDecorator cannot have child of type {0}", Child.GetType()));
            }
            Rectangle myRectangle = (Rectangle)DecoratedUIElement;
            myRectangle.AllowDrop = false;
            myRectangle.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(DraggableRectangle_PreviewMouseLeftButtonDown);
            myRectangle.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(DraggableRectangle_PreviewMouseMove);
            myRectangle.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(DraggableRectangle_PreviewMouseLeftButtonUp);
            myRectangle.PreviewDrop += new DragEventHandler(DraggableRectangle_PreviewDrop);
            myRectangle.PreviewQueryContinueDrag += new QueryContinueDragEventHandler(DraggableRectangle_PreviewQueryContinueDrag);
            myRectangle.PreviewDragEnter += new DragEventHandler(DraggableRectangle_PreviewDragEnter);
            myRectangle.PreviewDragOver += new DragEventHandler(DraggableRectangle_PreviewDragOver);
            myRectangle.DragLeave += new DragEventHandler(DraggableRectangle_DragLeave);
        }

       

        private void DraggableRectangle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle sourceRectangle = sender as Rectangle;
            if(sender != null)
            {
                Point p = e.GetPosition(sourceRectangle);
                _isMouseDown = true;
                _dragStartPosition = p;

            }
        }

        private void DraggableRectangle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            
            if (_isMouseDown && _isDragging == false)
            {
                Console.WriteLine("Mouse is down, dragging has not started");
                Rectangle sourceRectangle = sender as Rectangle ;
                if(sourceRectangle != null)
                {
                    Point currentPosition = e.GetPosition(sourceRectangle);
                    if( (_isDragging == false) && (Math.Abs(currentPosition.X - _dragStartPosition.X)> SystemParameters.MinimumHorizontalDragDistance)  ||
                        (Math.Abs(currentPosition.Y - _dragStartPosition.Y) > SystemParameters.MinimumVerticalDragDistance)  )
                    {
                        Console.WriteLine("Starting Drag");
                        sourceRectangle.CaptureMouse();
                        _isDragging = true;
                        InitializeDragAdorner(sourceRectangle, e.GetPosition(sourceRectangle));

                    }
                }

            }
            else if(_isMouseDown && _isDragging == true)
            {
                


                Console.Write(".");
               
                Rectangle sourceRectangle = sender as Rectangle;
                if(sourceRectangle != null)
                {
                    Point pt = e.GetPosition(sourceRectangle);
                    pt.X -= sourceRectangle.ActualWidth/2;
                    pt.Y -= sourceRectangle.ActualHeight / 2;
                    UpdateDragAdorner(pt);
                    //This is a drag operation  lets check what we are dragging over
                    hitResultsList.Clear();
                    //VisualTreeHelper.HitTest(Application.Current.MainWindow, MyHitTestFilter, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
                    VisualTreeHelper.HitTest(Application.Current.MainWindow, null, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(e.MouseDevice.GetPosition(Application.Current.MainWindow)));

                    if (hitResultsList.Count == 1)
                    {
                        string hits = "";
                        foreach(var hit in hitResultsList)
                        {
                           // hits = hits + " " + (hit as Shape).Name;
                        }
                        
                        Console.WriteLine("Mouse Moving : {0} " + hits,hitResultsList.Count);
                        //HitTestResult topElement = VisualTreeHelper.HitTest(Application.Current.MainWindow, e.MouseDevice.GetPosition(Application.Current.MainWindow));

                        Rectangle newRect = (hitResultsList[0]) as Rectangle;
                        Console.WriteLine(newRect.Name);
                    }

                }


            }
        }

       

        private void DraggableRectangle_DragLeave(object sender, DragEventArgs e)
        {
            
        }

        private void DraggableRectangle_PreviewDragOver(object sender, DragEventArgs e)
        {
            
        }

        private void DraggableRectangle_PreviewDragEnter(object sender, DragEventArgs e)
        {
            Console.WriteLine("PreviewDragEnter");
        }

        private void DraggableRectangle_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            
        }

        private void DraggableRectangle_PreviewDrop(object sender, DragEventArgs e)
        {
            
        }


        private List<DependencyObject> hitResultsList = new List<DependencyObject>();

        private void DraggableRectangle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Mouse Up... releaseing mouse");
            UIElement el = (UIElement)sender;
            

            Point pt = e.GetPosition(el);

            hitResultsList.Clear();

            //VisualTreeHelper.HitTest(Application.Current.MainWindow, MyHitTestFilter, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(pt));
            //VisualTreeHelper.HitTest(Application.Current.MainWindow, MyHitTestFilter, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(e.MouseDevice.GetPosition(Application.Current.MainWindow)));
            VisualTreeHelper.HitTest(Application.Current.MainWindow, null, new HitTestResultCallback(MyHitTestResult), new PointHitTestParameters(e.MouseDevice.GetPosition(Application.Current.MainWindow)));

            if (hitResultsList.Count > 1)
            {
                Rectangle theRect = hitResultsList[1] as Rectangle;
              //  Console.WriteLine("Underlying Rect: " + theRect.Name);
            }
            
            var underElement = e.MouseDevice.DirectlyOver as Rectangle;
            Console.WriteLine("Mouse is over element: {0}", underElement.Name);

            HitTestResult topElement = VisualTreeHelper.HitTest(Application.Current.MainWindow, e.MouseDevice.GetPosition(Application.Current.MainWindow));
            
            Rectangle newRect = (topElement.VisualHit) as Rectangle;
            Console.WriteLine(newRect.Name);

            el.ReleaseMouseCapture();
            ResetState((Rectangle)sender);
            DetachDragAdorner();
            e.Handled = true;
           
        }

        // Return the result of the hit test to the callback.
        public HitTestResultBehavior MyHitTestResult(HitTestResult result)
        {

            // Add the hit test result to the list that will be processed after the enumeration.
            if (result.VisualHit.GetType() == typeof(Rectangle))
                hitResultsList.Add(result.VisualHit);

            // Set the behavior to return visuals at all z-order levels.
            return HitTestResultBehavior.Continue;
        }

        // Filter the hit test values for each object in the enumeration.
        public HitTestFilterBehavior MyHitTestFilter(DependencyObject o)
        {
            // Test for the object value you want to filter.
            if (o.GetType() == typeof(Border))
            {
                // Visual object and descendants are NOT part of hit test results enumeration.
                return HitTestFilterBehavior.ContinueSkipSelf;
            }
            else
            {
                // Visual object is part of hit test results enumeration.
                return HitTestFilterBehavior.Continue;
            }
        }

        private void DragStarted(Rectangle sourceRectangle)
        {
            Console.WriteLine("In Drag Started");

            _isDragging = true;
      
        }


        private void InitializeDragAdorner(Rectangle sourceRectangle, Point point)
        {
            if(_visualBrushDragAdorner == null)
            {
                var adornerLayer = AdornerLayer.GetAdornerLayer(sourceRectangle);
                adornerLayer.IsHitTestVisible = false;
                _visualBrushDragAdorner = new VisualBrushDragAdorner(sourceRectangle, adornerLayer);
                _visualBrushDragAdorner.UpdatePosition(point.X, point.Y);
            
            }
            
        }

        private void UpdateDragAdorner(Point point)
        {
            if(_visualBrushDragAdorner != null)
            {
                _visualBrushDragAdorner.UpdatePosition(point.X, point.Y);
            }
        }


        private void ResetState(Rectangle sender)
        {
            _isDragging = false;
            _isMouseDown = false;

        }

        private void DetachDragAdorner()
        {
            if(_visualBrushDragAdorner != null)
            {
                _visualBrushDragAdorner.Destroy();
                _visualBrushDragAdorner = null;
            }
        }
    }
}
