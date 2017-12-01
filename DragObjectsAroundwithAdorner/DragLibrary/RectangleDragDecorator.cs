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
        private VisualBrushDragAdorner _adorner;
        private bool _isMouseDown;
        private object _data;
        private Point _dragStartPosition;
        private Point _adornerStartPosition;
        private bool _isDragging = false;
        private bool _dragHasLeftScope;
        public bool IsDragging { get; private set; }
        AdornerLayer _layer;
        FrameworkElement DragScope;

        public RectangleDragDecorator() : base()
        {
            _isDragging = false;
            _isMouseDown = false;
            IsDragging = false;

            this.Loaded += new RoutedEventHandler(DraggableRectangle_Loaded);
        }

        private void DraggableRectangle_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(base.DecoratedUIElement is Rectangle))
            {
                throw new InvalidCastException(string.Format("ItemsControlDragDecorator cannot have child of type {0}", Child.GetType()));
            }
            Rectangle myRectangle = (Rectangle)DecoratedUIElement;
            myRectangle.AllowDrop = true;
            myRectangle.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(DraggableRectangle_PreviewMouseLeftButtonDown);
            myRectangle.PreviewMouseMove += new System.Windows.Input.MouseEventHandler(DraggableRectangle_PreviewMouseMove);
            myRectangle.PreviewMouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(DraggableRectangle_PreviewMouseLeftButtonUp);
            myRectangle.PreviewDrop += new DragEventHandler(DraggableRectangle_PreviewDrop);
            myRectangle.PreviewQueryContinueDrag += new QueryContinueDragEventHandler(DraggableRectangle_PreviewQueryContinueDrag);
            myRectangle.PreviewDragEnter += new DragEventHandler(DraggableRectangle_PreviewDragEnter);
            myRectangle.PreviewDragOver += new DragEventHandler(DraggableRectangle_PreviewDragOver);
            myRectangle.DragLeave += new DragEventHandler(DraggableRectangle_DragLeave);

            _adornerStartPosition = myRectangle.PointToScreen(new Point());
            if (System.Windows.Application.Current.MainWindow != null)
            {
                   Rect rTemp = myRectangle.GetAbsolutePlacement(false);
                   _adornerStartPosition = rTemp.TopLeft;
            }

        }

       

        private void DraggableRectangle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMouseDown = true;
           
        }

        private void DraggableRectangle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (IsDragging == false && _isMouseDown == true)
            {
                Console.WriteLine("Calling Drag Method");
                StartDragInProcAdorner(e);
            }

        }

        private void StartDragInProcAdorner(MouseEventArgs e)
        {
            // Let's define our DragScope .. In this case it is every thing inside our main window ..

            DragScope = Application.Current.MainWindow.Content as FrameworkElement;


            // We enable Drag & Drop in our scope ...  We are not implementing Drop, so it is OK, but this allows us to get DragOver

            bool previousDrop = DragScope.AllowDrop;
             DragScope.AllowDrop = true;

            // The DragOver event for the window
            DragEventHandler draghandler = new DragEventHandler(Window1_DragOver);
            DragScope.PreviewDragOver += draghandler;

            // ThE Give Feedback handler for the window
            GiveFeedbackEventHandler givefeedbackhandler = new GiveFeedbackEventHandler(Window1_GiveFeedback);
            DragScope.GiveFeedback += givefeedbackhandler;

            DragEventHandler dragleavehandler = new DragEventHandler(DragScope_DragLeave);
            DragScope.DragLeave += dragleavehandler;


            // QueryContinue Drag goes with drag leave...
            QueryContinueDragEventHandler queryhandler = new QueryContinueDragEventHandler(DragScope_QueryContinueDrag);
            DragScope.QueryContinueDrag += queryhandler;

            DataObject data = new DataObject();
            data.SetData(DataFormats.StringFormat, Name.ToString());
            // data.SetData("Double", Name.Height);
            data.SetData("Object", this);

            //Here we create our adorner..

            _adorner = new VisualBrushDragAdorner(DragScope, (UIElement)this.Child, true, 1.0);
            _layer = AdornerLayer.GetAdornerLayer(DragScope as Visual);
            _layer.Add(_adorner);

            System.Windows.Point pt = e.GetPosition(Application.Current.MainWindow);
            _adorner.UpdatePosition(pt.X - _adornerStartPosition.X, pt.Y - _adornerStartPosition.Y, _layer);

            IsDragging = true;
            _dragHasLeftScope = false;

            // Inititate the drag-and-drop operation.
            Console.WriteLine("Starting Drag");
            data = new DataObject(System.Windows.DataFormats.Text.ToString(), "abcd");
            DragDropEffects de = DragDrop.DoDragDrop(this, data, DragDropEffects.Copy | DragDropEffects.Move);

            // Clean up our mess 🙂

            DragScope.AllowDrop = previousDrop;
            AdornerLayer.GetAdornerLayer(DragScope).Remove(_adorner);
            _adorner = null;


            //  DragSource.GiveFeedback -= feedbackhandler;

            DragScope.DragLeave -= dragleavehandler;
            DragScope.QueryContinueDrag -= queryhandler;
            DragScope.PreviewDragOver -= draghandler;
            IsDragging = false;

        }

        private void Window1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
           
        }

        private void DragScope_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }

        private void DragScope_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void Window1_DragOver(object sender, DragEventArgs e)
        {
            System.Windows.Point pt = e.GetPosition(DragScope);
            _adorner.UpdatePosition(pt.X - _adornerStartPosition.X, pt.Y - _adornerStartPosition.Y, _layer);
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
            _isMouseDown = false;
           
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
            Console.WriteLine("In DragStarted ()");

            _isDragging = true;
      
        }


        





        private void ResetState(Rectangle sender)
        {
            _isDragging = false;
            _isMouseDown = false;

        }

        private void DetachDragAdorner()
        {
            if(_adorner != null)
            {
                _adorner.Destroy();
                _adorner = null;
            }
        }
    }
}
