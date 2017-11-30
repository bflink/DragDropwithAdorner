using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DragObjectsAroundwithAdorner
{
    public class BaseDecorator : Decorator
    {
        protected UIElement DecoratedUIElement
        {
            get
            {
                if (this.Child is BaseDecorator)
                {
                    return ((BaseDecorator)this.Child).DecoratedUIElement;
                }
                return this.Child;
            }
        }
    }
}
