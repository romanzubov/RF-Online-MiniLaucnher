using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MiniLauncherStyle.Helper
{
    /// <summary>
    /// Canvas с поддержкой перетаскивания дочерних элементов.
    /// </summary>
    public class DragCanvas : Canvas
    {
        #region Fields

        private UIElement elementBeingDragged;
        private Point origCursorLocation;
        private double origHorizOffset, origVertOffset;
        private bool modifyLeftOffset, modifyTopOffset;
        private bool isDragInProgress;
        
        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty AllowDraggingProperty;
        public static readonly DependencyProperty AllowDragOutOfViewProperty;
        public static readonly DependencyProperty CanBeDraggedProperty;

        static DragCanvas()
        {
            AllowDraggingProperty = DependencyProperty.Register(
                "AllowDragging",
                typeof(bool),
                typeof(DragCanvas),
                new PropertyMetadata(true));

            AllowDragOutOfViewProperty = DependencyProperty.Register(
                "AllowDragOutOfView",
                typeof(bool),
                typeof(DragCanvas),
                new UIPropertyMetadata(false));

            CanBeDraggedProperty = DependencyProperty.RegisterAttached(
                "CanBeDragged",
                typeof(bool),
                typeof(DragCanvas),
                new UIPropertyMetadata(true));
        }

        #endregion

        #region Properties

        public bool AllowDragging
        {
            get { return (bool)GetValue(AllowDraggingProperty); }
            set { SetValue(AllowDraggingProperty, value); }
        }

        public bool AllowDragOutOfView
        {
            get { return (bool)GetValue(AllowDragOutOfViewProperty); }
            set { SetValue(AllowDragOutOfViewProperty, value); }
        }

        public UIElement ElementBeingDragged
        {
            get
            {
                if (!AllowDragging)
                    return null;
                
                return elementBeingDragged;
            }
            protected set
            {
                if (elementBeingDragged != null)
                    elementBeingDragged.ReleaseMouseCapture();

                if (!AllowDragging)
                {
                    elementBeingDragged = null;
                }
                else
                {
                    if (GetCanBeDragged(value))
                    {
                        elementBeingDragged = value;
                        elementBeingDragged.CaptureMouse();
                    }
                    else
                    {
                        elementBeingDragged = null;
                    }
                }
            }
        }

        #endregion

        #region Attached Property Accessors

        public static bool GetCanBeDragged(UIElement uiElement)
        {
            if (uiElement == null)
                return false;

            return (bool)uiElement.GetValue(CanBeDraggedProperty);
        }

        public static void SetCanBeDragged(UIElement uiElement, bool value)
        {
            if (uiElement != null)
                uiElement.SetValue(CanBeDraggedProperty, value);
        }

        #endregion

        #region Public Methods

        public void BringToFront(UIElement element)
        {
            UpdateZOrder(element, true);
        }

        public void SendToBack(UIElement element)
        {
            UpdateZOrder(element, false);
        }

        public UIElement FindCanvasChild(DependencyObject depObj)
        {
            while (depObj != null)
            {
                UIElement elem = depObj as UIElement;
                if (elem != null && Children.Contains(elem))
                    break;
                    
                if (depObj is Visual || depObj is Visual3D)
                    depObj = VisualTreeHelper.GetParent(depObj);
                else
                    depObj = LogicalTreeHelper.GetParent(depObj);
            }
            return depObj as UIElement;
        }

        #endregion

        #region Protected Overrides

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            
            isDragInProgress = false;
            origCursorLocation = e.GetPosition(this);
            ElementBeingDragged = FindCanvasChild(e.Source as DependencyObject);
            
            if (ElementBeingDragged == null)
                return;

            double left = Canvas.GetLeft(ElementBeingDragged);
            double right = Canvas.GetRight(ElementBeingDragged);
            double top = Canvas.GetTop(ElementBeingDragged);
            double bottom = Canvas.GetBottom(ElementBeingDragged);
            
            origHorizOffset = ResolveOffset(left, right, out modifyLeftOffset);
            origVertOffset = ResolveOffset(top, bottom, out modifyTopOffset);
            
            e.Handled = true;
            isDragInProgress = true;
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            
            if (ElementBeingDragged == null || !isDragInProgress)
                return;
                
            Point cursorLocation = e.GetPosition(this);
            double newHorizontalOffset, newVerticalOffset;

            #region Calculate Offsets

            if (modifyLeftOffset)
                newHorizontalOffset = origHorizOffset + (cursorLocation.X - origCursorLocation.X);
            else
                newHorizontalOffset = this.origHorizOffset - (cursorLocation.X - this.origCursorLocation.X);
            if (this.modifyTopOffset)
                newVerticalOffset = this.origVertOffset + (cursorLocation.Y - this.origCursorLocation.Y);
            else
                newVerticalOffset = this.origVertOffset - (cursorLocation.Y - this.origCursorLocation.Y);

            #endregion

            if (!this.AllowDragOutOfView)
            {
                #region Verify Drag Element Location 

                Rect elemRect = this.CalculateDragElementRect(newHorizontalOffset, newVerticalOffset);
                bool leftAlign = elemRect.Left < 0;
                bool rightAlign = elemRect.Right > this.ActualWidth;

                if (leftAlign)
                    newHorizontalOffset = modifyLeftOffset ? 0 : this.ActualWidth - elemRect.Width;
                else if (rightAlign)
                    newHorizontalOffset = modifyLeftOffset ? this.ActualWidth - elemRect.Width : 0;

                bool topAlign = elemRect.Top < 0;
                bool bottomAlign = elemRect.Bottom > this.ActualHeight;

                if (topAlign)
                    newVerticalOffset = modifyTopOffset ? 0 : this.ActualHeight - elemRect.Height;
                else if (bottomAlign)
                    newVerticalOffset = modifyTopOffset ? this.ActualHeight - elemRect.Height : 0;

                #endregion
            }

            #region Move Drag Element 

            if (this.modifyLeftOffset)
                Canvas.SetLeft(this.ElementBeingDragged, newHorizontalOffset);
            else
                Canvas.SetRight(this.ElementBeingDragged, newHorizontalOffset);

            if (this.modifyTopOffset)
                Canvas.SetTop(this.ElementBeingDragged, newVerticalOffset);
            else
                Canvas.SetBottom(this.ElementBeingDragged, newVerticalOffset);

            #endregion
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            this.ElementBeingDragged = null;
        }

        #endregion

        #region Private Helpers 
        #region CalculateDragElementRect 
        private Rect CalculateDragElementRect(double newHorizOffset, double newVertOffset)
        {
            if (this.ElementBeingDragged == null)
                throw new InvalidOperationException("ElementBeingDragged is null.");
            Size elemSize = this.ElementBeingDragged.RenderSize;
            double x, y;
            if (this.modifyLeftOffset)
                x = newHorizOffset;
            else
                x = this.ActualWidth - newHorizOffset - elemSize.Width;
            if (this.modifyTopOffset)
                y = newVertOffset;
            else
                y = this.ActualHeight - newVertOffset - elemSize.Height;
            Point elemLoc = new Point(x, y);
            return new Rect(elemLoc, elemSize);
        }
        #endregion
        #region ResolveOffset      
        private static double ResolveOffset(double side1, double side2, out bool useSide1)
        {

            useSide1 = true;
            double result;
            if (Double.IsNaN(side1))
            {
                if (Double.IsNaN(side2))
                {
                    result = 0;
                }
                else
                {
                    result = side2;
                    useSide1 = false;
                }
            }
            else
            {
                result = side1;
            }
            return result;
        }
        #endregion
        #region UpdateZOrder 
        private void UpdateZOrder(UIElement element, bool bringToFront)
        {
            #region Safety Check 

            if (element == null)
                throw new ArgumentNullException("element");

            if (!base.Children.Contains(element))
                throw new ArgumentException("Must be a child element of the Canvas.", "element");

            #endregion

            #region Calculate Z-Indici And Offset 
            int elementNewZIndex = -1;
            if (bringToFront)
            {
                foreach (UIElement elem in base.Children)
                    if (elem.Visibility != Visibility.Collapsed)
                        ++elementNewZIndex;
            }
            else
            {
                elementNewZIndex = 0;
            }
            int offset = (elementNewZIndex == 0) ? +1 : -1;

            int elementCurrentZIndex = Canvas.GetZIndex(element);
            #endregion
            #region Update Z-Indici 
            foreach (UIElement childElement in base.Children)
            {
                if (childElement == element)
                    Canvas.SetZIndex(element, elementNewZIndex);
                else
                {
                    int zIndex = Canvas.GetZIndex(childElement);
                    if (bringToFront && elementCurrentZIndex < zIndex ||
                        !bringToFront && zIndex < elementCurrentZIndex)
                    {
                        Canvas.SetZIndex(childElement, zIndex + offset);
                    }
                }
            }
            #endregion
        }
        #endregion
        #endregion
    }
}
