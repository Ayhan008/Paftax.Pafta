using Paftax.Pafta.Drawing.Annotations;
using Paftax.Pafta.Drawing.Elements;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Paftax.Pafta.Drawing
{
    public partial class DrawingCanvas : FrameworkElement
    {
        // Collection of visual elements
        private readonly VisualCollection _visuals;

        // Elements and Annotations
        private readonly List<Element> _elements = [];
        private readonly List<Annotation> _annotations = [];

        // Pan and Zoom state
        private Vector _panOffset = new(0, 0);
        private double _zoomScale = 1.0;

        // Panning state
        private bool _isPanning = false;
        private Point _lastMousePos;

        // Keyboard state
        private bool _isZPressed = false;

        public DrawingCanvas()
        {
            _visuals = new VisualCollection(this);
            Background = Brushes.White;

            Focusable = true;
            Loaded += (s, e) => Focus();
            MouseEnter += (s, e) => Focus();
        }

        #region Dependency Properties
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(DrawingCanvas),
                new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(DrawingCanvas),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));
        
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register(nameof(Scale), typeof(double), typeof(DrawingCanvas),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender, OnScaleChanged));  
        
        public static readonly DependencyProperty SelectedGeometryColorProperty =
            DependencyProperty.Register(nameof(SelectedGeometryColor), typeof(Brush), typeof(DrawingCanvas),
                new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush SelectedGeometryColor
        {
            get => (Brush)GetValue(SelectedGeometryColorProperty);
            set => SetValue(SelectedGeometryColorProperty, value);
        }

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        public Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public double Scale
        {
            get => (double)GetValue(ScaleProperty);
            set => SetValue(ScaleProperty, value);
        }

        public static void OnScaleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DrawingCanvas canvas)
            {
                canvas.ApplyTransformToAllVisuals();
            }
        }
        #endregion

        #region Visual Management
        protected override int VisualChildrenCount => _visuals.Count;
        protected override Visual GetVisualChild(int index) => _visuals[index];
        #endregion

        #region Annotations
        public void AddElevation(Point point)
        {
            Elevation elevation = new()
            {
                Center = point,
                Rotation = 90,
                SheetNumber = "A101",
                DetailNumber = "1",
                DrawingVisual = new DrawingVisual()
            };
            using (var dc = elevation.DrawingVisual.RenderOpen())
            {
                elevation.Draw(dc, Foreground);
            }

            _annotations.Add(elevation);
            _visuals.Add(elevation.DrawingVisual);

            ApplyTransformToVisual(elevation.DrawingVisual, isAnnotation: true);
            InvalidateVisual();
        }
        #endregion

        #region Element Management
        public void AddElement(Element element)
        {
            if (element == null) return;

            element.DrawingVisual = new DrawingVisual();
            using (var dc = element.DrawingVisual.RenderOpen())
                element.Draw(dc, Foreground);

            _elements.Add(element);
            _visuals.Add(element.DrawingVisual);

            ApplyTransformToVisual(element.DrawingVisual, isAnnotation: false);
            InvalidateVisual();
        }

        public void RemoveElement(Element element)
        {
            if (element?.DrawingVisual != null)
            {
                _visuals.Remove(element.DrawingVisual);
                InvalidateVisual();
            }
        }
        #endregion

        #region Transform Helpers
        private void ApplyTransformToAllVisuals()
        {
            foreach (var el in _elements)
                ApplyTransformToVisual(el.DrawingVisual, false);

            foreach (var ann in _annotations)
                ApplyTransformToVisual(ann.DrawingVisual, true);
        }

        private void ApplyTransformToVisual(DrawingVisual visual, bool isAnnotation)
        {
            // Calculate the transformation matrix
            double scale = isAnnotation ? (_zoomScale/Scale)/90 : _zoomScale;

            // Flip Y axis and apply scalingze
            Matrix matrix = new ScaleTransform(scale, -scale).Value;

            // Translate to center and apply pan offset
            Matrix translateCenter = new TranslateTransform(ActualWidth / 2, ActualHeight / 2).Value;

            // Apply panning
            Matrix pan = new TranslateTransform(_panOffset.X, _panOffset.Y).Value;

            matrix.Append(pan);
            matrix.Append(translateCenter);

            visual.Transform = new MatrixTransform(matrix);
        }
        #endregion

        #region Mouse Wheel Zoom
        /// <summary>
        /// Handles mouse wheel events to zoom the view in or out, centering the zoom operation on the mouse pointer
        /// position.
        /// </summary>
        /// <remarks>Zooming is constrained to a minimum scale of 0.01 and a maximum scale of 100. The
        /// zoom operation is centered on the current mouse position, and the pan offset is adjusted to maintain the
        /// focus point under the cursor.</remarks>
        /// <param name="e">The event data containing information about the mouse wheel movement.</param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;

            Point mousePos = e.GetPosition(this);

            double newZoom = _zoomScale * zoomFactor;
            if (newZoom < 0.01 || newZoom > 100) return;

            Vector canvasCenter = new(ActualWidth / 2, ActualHeight / 2);

            Vector worldPos = (Vector)(mousePos - canvasCenter - _panOffset) / _zoomScale;

            _panOffset -= worldPos * (_zoomScale * zoomFactor - _zoomScale);

            _zoomScale = newZoom;

            ApplyTransformToAllVisuals();
        }
        #endregion

        #region Keyboard Shortcuts
        /// <summary>
        /// Handles keyboard input to support custom keyboard shortcuts, including a shortcut for zooming to fit all
        /// content.
        /// </summary>
        /// <remarks>Pressing and releasing the 'Z' key in combination with the 'E' key triggers the
        /// zoom-to-fit-all functionality. Other key combinations are handled by the base implementation.</remarks>
        /// <param name="e">A <see cref="KeyEventArgs"/> that contains the event data for the key press.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Z)
            {
                _isZPressed = true;
                return;
            }

            if (_isZPressed && e.Key >= Key.A && e.Key <= Key.Z)
            {
                if (e.Key == Key.E)
                {
                    ZoomToFitAll();
                }
                _isZPressed = false;
            }
        }

        /// <summary>
        /// Handles the KeyUp event by invoking the base class implementation.
        /// </summary>
        /// <param name="e">A KeyEventArgs that contains the event data associated with the key release.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }
        #endregion

        #region Pan / Mouse Events
        /// <summary>
        /// Handles mouse down events to initiate panning when the Shift key and right mouse button are pressed.
        /// </summary>
        /// <remarks>Panning mode is activated only when either Shift key is held and the right mouse
        /// button is pressed. The control also receives focus when this event is handled.</remarks>
        /// <param name="e">The event data associated with the mouse button event.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            this.Focus();

            Point mousePos = e.GetPosition(this);

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                && e.RightButton == MouseButtonState.Pressed)
            {
                _isPanning = true;
                _lastMousePos = mousePos;
                CaptureMouse();

                Cursor = Cursors.SizeAll;
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                bool changed = false;
                Point world = DeviceToAnnotation(mousePos);

                foreach (var elevation in _annotations.OfType<Elevation>())
                {
                    bool hit = elevation.HitTriangle(world);
                    if (hit != elevation.IsViewBoxVisible)
                    {
                        elevation.IsViewBoxVisible = hit;
                        changed = true;
                    }
                    else if (!hit && elevation.IsViewBoxVisible)
                    {
                        // ensure only one active
                        elevation.IsViewBoxVisible = false;
                    }
                }

                if (changed)
                    Redraw();
            }
        }

        /// <summary>
        /// Handles mouse movement events to update the panning state of the control when panning is active.
        /// </summary>
        /// <remarks>This method is typically called by the WPF framework when the mouse moves over the
        /// control. If panning is in progress, the control's visual content is shifted according to the mouse movement.
        /// Derived classes can override this method to provide custom mouse movement handling, but should call the base
        /// implementation to preserve panning behavior.</remarks>
        /// <param name="e">The event data associated with the mouse movement.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isPanning)
            {
                Point currentPos = e.GetPosition(this);
                Vector delta = currentPos - _lastMousePos;

                _panOffset += delta;
                ApplyTransformToAllVisuals();

                _lastMousePos = currentPos;
            }
        }

        /// <summary>
        /// Handles the MouseUp event to end a panning operation when the right mouse button is released.
        /// </summary>
        /// <remarks>This method is typically called by the WPF event system and should not be called
        /// directly. When the right mouse button is released during a panning operation, this method releases mouse
        /// capture and resets the cursor to the default arrow.</remarks>
        /// <param name="e">The event data associated with the mouse button release.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isPanning && e.ChangedButton == MouseButton.Right)
            {
                _isPanning = false;
                ReleaseMouseCapture();

                Cursor = Cursors.Arrow;
            }
        }

        private Point DeviceToAnnotation(Point devicePoint)
        {
            double scale = (_zoomScale / Scale) / 90.0;
            Vector canvasCenter = new(ActualWidth / 2, ActualHeight / 2);
            Vector v = (Vector)(devicePoint - canvasCenter - _panOffset);
            double worldX = v.X / scale;
            double worldY = v.Y / -scale;
            return new Point(worldX, worldY);
        }
        #endregion

        #region ZoomToFitAll (visuals)
        /// <summary>
        /// Adjusts the zoom and pan so that all visual elements are scaled and positioned to fit within the visible
        /// area.
        /// </summary>
        /// <remarks>This method automatically calculates the optimal zoom level and pan offset to ensure
        /// that all visuals are visible within the current viewport. If there are no visuals to display, the method has
        /// no effect.</remarks>
        public void ZoomToFitAll()
        {
            // Get the bounding box of all visuals
            Rect bbox = GetVisualsBoundingBox();
            if (bbox.IsEmpty) return;

            // Calculate the required scale to fit the bounding box within the control's dimensions
            double scaleX = ActualWidth / bbox.Width;
            double scaleY = ActualHeight / bbox.Height;

            // Use the smaller scale to ensure the entire bounding box fits, with a margin
            _zoomScale = Math.Min(scaleX, scaleY) * 0.9;

            // Center the bounding box in the control
            _panOffset = new Vector(
                -(bbox.X + bbox.Width / 2) * _zoomScale,
                (bbox.Y + bbox.Height / 2) * _zoomScale
            );

            ApplyTransformToAllVisuals();
        }

        /// <summary>
        /// Calculates the smallest axis-aligned rectangle that contains the content of all visuals in the collection.
        /// </summary>
        /// <returns>A <see cref="Rect"/> representing the bounding box that encompasses the content of all visuals. Returns <see
        /// cref="Rect.Empty"/> if the collection contains no visuals or all visuals have empty content bounds.</returns>
        private Rect GetVisualsBoundingBox()
        {
            Rect bounds = Rect.Empty;

            foreach (DrawingVisual v in _visuals.Cast<DrawingVisual>())
            {
                Rect vb = v.ContentBounds;

                if (!vb.IsEmpty)
                {
                    if (bounds.IsEmpty)
                        bounds = vb;
                    else
                        bounds.Union(vb);
                }
            }

            return bounds;
        }
        #endregion

        #region Render
        /// <summary>
        /// Renders the background of the control using the specified drawing context.
        /// </summary>
        /// <remarks>This method is typically called by the layout system and should not be called
        /// directly. Overrides should call the base implementation to ensure proper rendering behavior.</remarks>
        /// <param name="drawingContext">The drawing context to use for rendering the control's visual content. Cannot be null.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(Background, null, new Rect(0, 0, ActualWidth, ActualHeight));
        }

        /// <summary>
        /// Responds to changes in the rendered size of the element.
        /// </summary>
        /// <remarks>Overrides this method to update visual transforms when the element's size changes.
        /// Call the base implementation to ensure standard layout behavior is preserved.</remarks>
        /// <param name="sizeInfo">Information about the size changes, including the previous and new size values.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            ApplyTransformToAllVisuals();
        }
        #endregion

        public void Redraw()
        {
            foreach (DrawingVisual visual in _visuals.Cast<DrawingVisual>())
            {
                using var dc = visual.RenderOpen();

                Element? element = _elements.FirstOrDefault(el => el.DrawingVisual == visual);
                if (element != null)
                {
                    element.Draw(dc, Foreground);
                    continue;
                }

                Annotation? annotation = _annotations.FirstOrDefault(ann => ann.DrawingVisual == visual);
                annotation?.Draw(dc, Foreground);
            }
        }
    }
}
