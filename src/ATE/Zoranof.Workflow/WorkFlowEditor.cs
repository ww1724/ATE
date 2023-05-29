using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Zoranof.Workflow.Common;
using Zoranof.WorkFlow;

namespace Zoranof.Workflow
{
    public enum EditorAction
    {
        BoxSelect, MoveViewer, MoveItem, Connect, None
    }

    public class WorkflowEditor : Control
    {
        public static double DefaultConnectDistance = 30;

        public static double NearItemDistance = 20;

        #region Propertys
        public FontFamily Font
        {
            get { return (FontFamily)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        public static readonly DependencyProperty FontProperty =
            DependencyProperty.Register("Font", typeof(FontFamily), typeof(WorkflowEditor), new FrameworkPropertyMetadata(new FontFamily("Simsun"), FrameworkPropertyMetadataOptions.AffectsRender));

        public ObservableCollection<WorkflowNode> Items
        {
            get { return (ObservableCollection<WorkflowNode>)GetValue(ItemsProperty) ?? new ObservableCollection<WorkflowNode>(); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<WorkflowNode>), typeof(WorkflowEditor), new FrameworkPropertyMetadata(null, OnItemsPropertyChanged));

        private static void OnItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as WorkflowEditor;

            if (e.OldValue != null)
            {
                var oldItems = e.OldValue as ObservableCollection<WorkflowNode>;
                oldItems.CollectionChanged -= view.OnItemsCollectionChanged;
            }

            if (e.NewValue != null)
            {
                var newItems = e.NewValue as ObservableCollection<WorkflowNode>;
                newItems.CollectionChanged += view.OnItemsCollectionChanged;
            }

        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in Items)
            {
                item.AttachedView = this;
            }
            Focus();
            InvalidateVisual();
        }
        #endregion

        public WorkflowEditor()
        {
            ViewerScale = 1;
            ClipToBounds = true;
            ViewerLocation = new Point(0, 0);
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEFCFB"));
            Links = new List<OptionLink>();
            IsEditable = true;
            AllowDrop = true;
            HoverColor = Brushes.Red;

            Items = new ObservableCollection<WorkflowNode>();
        }

        #region Fields
        Brush HoverColor { get; set; }

        public bool IsEditable { get; set; }

        public List<OptionLink> Links { get; set; }

        public OptionLink HoverLink { get; set; }

        public double NearOptionDistance { get; set; } = 15;

        public double ConnectLineDistance { get; set; } = DefaultConnectDistance;

        private Dictionary<string, Type> m_load_types = new Dictionary<string, Type>();
        #endregion

        #region Variables
        public EditorAction m_action = EditorAction.None;

        public bool IsDragging;

        public Point ViewerLocation { get; set; } = new Point(0, 0);
        public double ViewerScale { get; set; }

        private Point m_cursor_point = new Point(0, 0);
        private Point m_cursor_canvas_point => PointPixelMap2Viewer(m_cursor_point);

        private Point m_actionStartPoint = new Point(0, 0);
        private Point m_actionEndPoint = new Point(0, 0);

        private bool m_isReadyToBoxSelect;
        private Point m_boxSelectStartPoint;
        private Point m_boxSelectEndPoint;

        private bool m_isReadyToMoveViewer;
        private Point m_viewerMoveStartPoint;
        private Point m_viewerMoveEndPoint;

        private bool m_isReadyToMoveSelectedItems;
        private Point m_moveItemsStartPoint;
        private Point m_moveItemsEndPoint;

        private bool m_isReadyToConnectOption;
        private Point m_toConnectOptionStartPoint;
        private Point m_toConnectOptionEndPoint;
        private WorkflowOption m_fromOption = null;
        private WorkflowOption m_toOption = null;
        private List<Point> m_inlinePoints = new();
        private Polyline m_previewConnectLine = new();
        #endregion

        #region General Slots

        protected virtual void SetHoveredItem(WorkflowNode item)
        {

        }

        protected virtual void DeleteSelectedItems(ICollection<WorkflowNode> items)
        {
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].IsSelected)
                {
                    for (int j = Links.Count - 1; j >= 0; j--)
                    {
                        if (Links[j].From.Owner == Items[i] || Links[j].To.Owner == Items[i])
                        {
                            Links.RemoveAt(j);
                        }
                    }

                    Items.RemoveAt(i);

                    OnItemRemoved(new WorkflowEventArgs { Item = Items[i] });
                }
            }

            Collection<int> toDeleteLinks = new();
            int index = 0;
            foreach (var link in Links)
            {
                if (link.IsSelected) { toDeleteLinks.Add(index); }
                index++;
            }
            for (int i = Links.Count - 1; i >= 0; --i)
            {
                if (toDeleteLinks.Contains(i))
                {
                    Links.RemoveAt(i);
                }
            }
            InvalidateVisual();
        }

        protected virtual void DeleteItems(ICollection<WorkflowNode> items)
        {
            foreach (var item in items)
            {
                Items.Remove(item);
            }
            InvalidateVisual();
        }

        protected virtual void SetActiveItem(WorkflowNode item)
        {
            foreach (var i in Items)
            {
                i.IsActive = false;
                i.ZIndex = 0;
            }
            item.IsActive = true;
            item.ZIndex = 1;
        }

        protected virtual void SelectItemAS(WorkflowNode item)
        {
            foreach (WorkflowNode item1 in Items)
            {
                item1.IsSelected = false;
            }

            item.IsSelected = true;
            InvalidateVisual();
            OnSelectedChanged(new WorkflowEventArgs(item));

        }

        protected virtual void SelectItemIS(WorkflowNode item)
        {
            item.IsSelected = true;
            InvalidateVisual();
            OnSelectedChanged(new WorkflowEventArgs(item));
        }

        /// <summary>
        /// 选择给定元素  绝对式
        /// </summary>
        /// <param name="items"></param>
        protected virtual void SelectItemsAS(ICollection<WorkflowNode> items)
        {
            foreach (WorkflowNode item in Items)
            {
                item.IsSelected = false;
            }

            if (items.Count == 0)
                return;

            foreach (WorkflowNode item in items)
            {
                item.IsSelected = true;
            }

            OnSelectedChanged(new WorkflowEventArgs(items.FirstOrDefault()));
            InvalidateVisual();
        }

        /// <summary>
        /// 选择给定元素  增量式
        /// </summary>
        /// <param name="items"></param>
        protected virtual void SelectItemsIS(ICollection<WorkflowNode> items)
        {

            if (items.Count == 0)
                return;

            foreach (WorkflowNode item in items)
            {
                item.IsSelected = true;
            }

            OnSelectedChanged(new WorkflowEventArgs(items.FirstOrDefault()));
            InvalidateVisual();
        }

        protected virtual void SelectLinksAS(ICollection<OptionLink> links)
        {
            foreach (OptionLink link in Links)
            {
                link.IsSelected = false;
            }

            foreach (OptionLink link in links)
            {
                link.IsSelected = true;
            }

            InvalidateVisual();
        }

        /// <summary>
        /// 选择给定元素  增量式
        /// </summary>
        /// <param name="items"></param>
        protected virtual void SelectLinksIS(ICollection<OptionLink> links)
        {

            foreach (OptionLink link in links)
            {
                link.IsSelected = true;
            }
            InvalidateVisual();
        }


        /// <summary>
        /// 选择全部元素
        /// </summary>
        protected virtual void SelectAllItems()
        {
            foreach (WorkflowNode item in Items)
            {
                item.IsSelected = true;
            }
            InvalidateVisual();
        }

        /// <summary>
        /// 取消选择全部元素
        /// </summary>
        protected virtual void UnSeselectAllItems()
        {
            foreach (WorkflowNode item in Items)
            {
                item.IsSelected = false;
                item.IsActive = false;
            }
            InvalidateVisual();
        }

        /// <summary> 框选动作 </summary>
        /// <description> 取消之前的选中, 选中当前框选 </description>
        /// <param name="selectedBoxRect">当前选框的rect</param>
        protected virtual void ToBoxSelect(Rect selectedBoxRect)
        {
            // 元素框选
            ICollection<WorkflowNode> toSelectItems = new Collection<WorkflowNode>();
            foreach (var item in Items)
            {
                Rect itemRectInViewer = RectMapToViewer(item.BoundingRect);

                if (itemRectInViewer.IntersectsWith(selectedBoxRect))
                    toSelectItems.Add(item);
            }

            if (toSelectItems.Count > 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    SelectItemsIS(toSelectItems);
                }
                else
                {
                    SelectItemsAS(toSelectItems);

                }
            }

            // 线段框选
            ICollection<OptionLink> toSelectLinks = new Collection<OptionLink>();
            foreach (var link in Links)
            {
                bool isNear = (new LineGeometry(link.StartPoint, link.EndPoint)).Bounds.IntersectsWith(selectedBoxRect);
                if (isNear) toSelectLinks.Add(link);
            }

            if (toSelectLinks.Count > 0)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    SelectLinksIS(toSelectLinks);
                }
                else
                {
                    SelectLinksAS(toSelectLinks);
                }
            }


            InvalidateVisual();
        }

        /// <summary>
        /// 偏移选中元素
        /// </summary>
        /// <param name="offset"></param>
        protected virtual void MoveSelectedItemsWithOffset(Vector offset)
        {
            var toMoveItems = (ICollection<WorkflowNode>)Items.Where(x => x.IsSelected == true).ToList();
            MoveItemsWithOffset(toMoveItems, offset);
        }


        /// <summary>
        /// 偏移元素
        /// </summary>
        /// <param name="items"></param>
        /// <param name="offset"></param>
        protected virtual void MoveItemsWithOffset(ICollection<WorkflowNode> items, Vector offset)
        {
            if (items.Count == 0) return;
            foreach (var item in items)
            {
                item.Pos = new Point(
                    item.Pos.X + offset.X,
                    item.Pos.Y + offset.Y
                    );
                //item.MoveWithOssfet(offset);
                item.OnItemMoved(null);
            }

            InvalidateVisual();
        }

        /// <summary>
        /// 像素坐标转当前视图坐标
        /// </summary>
        /// <param name="point">通常为鼠标坐标</param>
        /// <returns>视图坐标</returns>
        protected virtual Point PointPixelMap2Viewer(Point point) => new Point(
            point.X * ViewerScale + ViewerLocation.X, 
            point.Y * ViewerScale + ViewerLocation.Y);
            //(point.X - ViewerLocation.X) * ViewerScale,
            //(point.X - ViewerLocation.X) * ViewerScale);

        protected virtual Point PointViewerMap2Pixel(Point point) => new(
            point.X / ViewerScale + ViewerLocation.X,
            point.Y / ViewerScale + ViewerLocation.Y);

        //protected virtual Point PointItemMap2Viewer(Point point, WorkflowNode item) => new Point(
        //    (point.X + item.Pos.X + ViewerLocation.X)
        //    );

        protected virtual Rect RectPixelMap2Viewer(Rect rect) => new(
            rect.Left * ViewerScale + ViewerLocation.X,
            rect.Top * ViewerScale + ViewerLocation.Y,
            rect.Width * ViewerScale,
            rect.Height * ViewerScale);

        protected virtual Rect RectViewerMap2Pixel(Rect rect) => new(
            (rect.Left - ViewerLocation.X) / ViewerScale,
            (rect.Top - ViewerLocation.Y) / ViewerScale,
            rect.Width / ViewerScale,
            rect.Height / ViewerScale);

        /// <summary>
        /// Rect 映射到viewer
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        protected virtual Rect RectMapToViewer(Rect rect) => new(
                    rect.X * ViewerScale + ViewerLocation.X,
                    rect.Y * ViewerScale + ViewerLocation.Y,
                    rect.Width * ViewerScale,
                    rect.Height * ViewerScale);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="offset"></param>
        ///// <returns></returns>
        //protected virtual Vector OffsetMapToControl(Vector offset) => new Vector(
        //            offset.X / ViewerScale + ViewerLocation.X,
        //            offset.Y / ViewerScale + ViewerLocation.Y);

        /// <summary>
        /// 移动画布
        /// </summary>
        /// <param name="point"></param>
        protected virtual void MoveViewerTo(Point point)
        {
            ViewerLocation = point;
            this.InvalidateVisual();
        }

        /// <summary>
        /// 通过偏移移动画布
        /// </summary>
        /// <param name="offset"></param>
        protected virtual void MoveViewerWithOffset(Vector offset)
        {
            ViewerLocation = new Point(ViewerLocation.X + offset.X, ViewerLocation.Y + offset.Y);
            this.InvalidateVisual();
        }
        #endregion

        #region Public Slots
        public bool LoadAssembly(string assemblyFile)
        {
            bool flag = false;



            return flag;
        }

        public virtual string ExportCanvas() { return ""; }
        public virtual string ExportNodes() { return ""; }
        public virtual string ExportWorkflowStep() { return ""; }
        public virtual void ImportToCanvasFromFile(string file) { }
        public virtual void ImportToCanvas(string text) { }

        public WorkflowOption GetNearOption(Point point)
        {

            point = new Point(point.X - ViewerLocation.X, point.Y - ViewerLocation.Y);
            WorkflowOption roption = null;
            foreach (var item in Items)
            {
                foreach (var option in item.Options)
                {
                    Point optionPoint = new Point(option.CenterPos.X + item.Pos.X, option.CenterPos.Y + item.Pos.Y);
                    Rect optionRect = new Rect(optionPoint.X - NearItemDistance / 2, optionPoint.Y - NearItemDistance / 2, NearItemDistance, NearItemDistance);
                    if (optionRect.Contains(point))
                        roption = option;
                }
            }
            return roption;
        }

        /// <summary>
        /// 是否碰撞
        /// </summary>
        /// <param name="item"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public virtual bool IsCollidesWithItem(WorkflowNode a, WorkflowNode b, ItemSelectionMode mode = ItemSelectionMode.IntersectsItemShape) { return true; }


        /// <summary>
        /// 确保Item可见
        /// </summary>
        public virtual void EnsureVisible(WorkflowNode item) { }

        /// <summary>
        /// 确保Items可见
        /// </summary>
        public virtual void EnsureVisible(ICollection<WorkflowNode> items) { }
        #endregion

        #region Events
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            // 容器框架绘制
            OnDrawFramework(drawingContext);

            // 绘制元素
            OnDrawItems(drawingContext);

            // 各事件绘制
            OnDrawActionIndicator(drawingContext);

            // 绘制信息
            OnDrawAlert(drawingContext);

            // 绘制标志
            OnDrawMask(drawingContext);

            // 绘制连接线
            OnDrawLinks(drawingContext);
        }

        protected virtual void OnDrawFramework(DrawingContext drawingContext)
        {

            // 是否获取焦点绘制不同边框
            Pen borderPen = new(
                IsFocused ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CBCFF2")) : Brushes.Transparent,
                3);

            Brush bgBrush = Background;

            drawingContext.DrawRectangle(
                bgBrush,
                borderPen,
                new Rect(new Point(4, 4), new Point(RenderSize.Width - 4, RenderSize.Height - 4)));
        }

        protected virtual void OnDrawItems(DrawingContext drawingContext)
        {

            if (Items == null) return;
            //启动偏移
            drawingContext.PushTransform(new TranslateTransform(ViewerLocation.X, ViewerLocation.Y));
            drawingContext.PushTransform(new ScaleTransform(ViewerScale, ViewerScale));
            // 绘制节点
            foreach (var item in Items.OrderBy(x => x.ZIndex).ToList())
            {
                item.OnRender(drawingContext);
            }
            // 关闭偏移
            drawingContext.Pop();
            drawingContext.Pop();
        }

        protected virtual void OnDrawActionIndicator(DrawingContext drawingContext)
        {
            if (m_action == EditorAction.BoxSelect)
            {
                drawingContext.DrawRectangle(
                    new SolidColorBrush() { Opacity = 0.5, Color = Color.FromArgb(100, 0, 0, 0) },
                    new Pen(),
                    new Rect(m_actionStartPoint, m_actionEndPoint));
            }
        }

        protected virtual void OnDrawAlert(DrawingContext drawingContext) { }

        protected virtual void OnDrawMask(DrawingContext drawingContext) { }

        protected internal void OnDrawLinks(DrawingContext drawingContext)
        {
            
            if (m_action == EditorAction.Connect)
            {
                drawingContext.DrawLine(new Pen(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0BBABA")), 2), 
                    m_actionStartPoint, 
                    m_actionEndPoint);
            }

            drawingContext.PushTransform(new TranslateTransform(ViewerLocation.X, ViewerLocation.Y));

            foreach (var link in Links)
            {
                if (link.IsHovered || link.IsSelected || link.IsDragging) drawingContext.DrawLine(new Pen(Brushes.Red, 2), link.StartPoint, link.EndPoint);
                else drawingContext.DrawLine(new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0BBABA")), 2), link.StartPoint, link.EndPoint);
            }

            drawingContext.Pop();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {

            base.OnRenderSizeChanged(sizeInfo);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Focus();
            m_cursor_point = e.GetPosition(this);

            // 左键按下 => 选择元素 || 准备移动元素 || 准备框选元素
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                foreach (var link in Links)
                {
                    bool isNear = e.GetPosition(this).IsPointNearToLine(link.StartPoint, link.EndPoint, 10);
                    if (link.IsSelected != isNear)
                    {
                        link.IsSelected = isNear;
                        InvalidateVisual();
                    }
                }

                var _activeItem = Items
                    .Where(x => RectMapToViewer(new Rect(
                        x.BoundingRect.Left - NearItemDistance,
                        x.BoundingRect.Top - NearItemDistance,
                        x.BoundingRect.Width + 2 * NearItemDistance,
                        x.BoundingRect.Height + 2 * NearItemDistance))
                    .Contains(e.GetPosition(this)))
                    .OrderByDescending(x => x.ZIndex)
                    .FirstOrDefault();


                // 按下鼠标时鼠标下有元素 => 选择Items
                if (_activeItem != null)
                {
                    WorkflowOption nearOption = GetNearOption(e.GetPosition(this));
                    // 1 => 进入连线状态
                    if (nearOption != null)
                    {
                        m_fromOption = nearOption;
                        nearOption.IsConnecting = true;

                        m_action = EditorAction.Connect;
                        m_actionStartPoint = e.GetPosition(this);
                        m_actionEndPoint = e.GetPosition (this);

                        nearOption.IsConnecting = true;
                        m_inlinePoints.Add(m_toConnectOptionStartPoint);
                        Cursor = Cursors.Cross;
                        return;
                    }

                    // 2 => 选择并准备移动元素
                    else
                    {
                        SetActiveItem(_activeItem);
                        if (!_activeItem.IsSelected)
                        {
                            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                                SelectItemIS(_activeItem);
                            else SelectItemAS(_activeItem);
                        }
                        // 准备移动元素
                        m_action = EditorAction.MoveItem;
                        m_actionStartPoint = e.GetPosition(this);
                        m_actionEndPoint = e.GetPosition(this);
                        Cursor = Cursors.SizeAll;
                    }
                }
                else
                {
                    // 按下鼠标时鼠标下没元素 => 取消全部选择 - 开始框选动作
                    if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                        UnSeselectAllItems();

                    m_action = EditorAction.BoxSelect;
                    m_actionStartPoint = m_cursor_point;
                    m_actionEndPoint = m_cursor_point;
                    return;
                }
            }

            // 滚轮按下
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                m_action = EditorAction.MoveViewer;
                m_actionStartPoint = e.GetPosition(this);
                m_actionEndPoint = e.GetPosition(this);

                //m_isReadyToMoveViewer = true;
                //m_viewerMoveStartPoint = e.GetPosition(this);
                //m_viewerMoveEndPoint = e.GetPosition(this);
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {

            switch(m_action)
            {
                case EditorAction.BoxSelect:
                    m_action = EditorAction.None;
                    m_actionStartPoint = new Point(0, 0);
                    m_actionEndPoint = new Point(0, 0);
                    m_action = EditorAction.None;
                    InvalidateVisual();
                    break;
                case EditorAction.MoveItem:
                    m_action = EditorAction.None;
                    m_actionStartPoint = new Point(0, 0);
                    m_actionEndPoint = new Point(0, 0);
                    m_action = EditorAction.None;
                    InvalidateVisual();
                    break;
                case EditorAction.MoveViewer:
                    m_action = EditorAction.None;
                    m_actionStartPoint = new Point(0, 0);
                    m_actionEndPoint = new Point(0, 0);
                    m_action = EditorAction.None;
                    InvalidateVisual();
                    break;
                case EditorAction.Connect:
                    m_toConnectOptionEndPoint = e.GetPosition(this);
                    WorkflowOption nearOption = GetNearOption(m_toConnectOptionEndPoint);
                    if (nearOption != null)
                    {
                        m_toOption = nearOption;
                        if (m_toOption.Owner != m_fromOption.Owner)
                        {
                            Links.Add(new OptionLink { From = m_fromOption, To = m_toOption });
                        }
                    }
                    m_fromOption.IsConnecting = false;
                    m_action = EditorAction.None;

                    InvalidateVisual();
                    break;
                default:
                    UnSeselectAllItems();
                    break;
            }

            Cursor = Cursors.Arrow;


            //if (m_isReadyToBoxSelect)
            //{
            //    m_isReadyToBoxSelect = false;
            //    m_boxSelectStartPoint = new Point(0, 0);
            //    m_boxSelectEndPoint = new Point(0, 0);
            //    InvalidateVisual();
            //}
            //else if (m_isReadyToConnectOption || m_action == EditorAction.Connect)
            //{

            //    m_toConnectOptionEndPoint = e.GetPosition(this);
            //    WorkflowOption nearOption = GetNearOption(e.GetPosition(this));
            //    if (nearOption != null)
            //    {
            //        m_toOption = nearOption;
            //        if (m_toOption.Owner != m_fromOption.Owner)
            //        {
            //            Links.Add(new OptionLink { From = m_fromOption, To = m_toOption });
            //        }       
            //    }
            //    m_isReadyToConnectOption = false;
            //    m_toConnectOptionStartPoint = new Point(0, 0);
            //    m_toConnectOptionEndPoint = new Point(0, 0);
            //    m_fromOption.IsConnecting = false;
            //    InvalidateVisual();
            //}
            //else
            //{
            //    UnSeselectAllItems();
            //}

            Cursor = Cursors.Arrow;

            switch (m_action)
            {
                case EditorAction.Connect:

                    break;
            }




            base.OnMouseUp(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {

            switch (m_action)
            {
                case EditorAction.BoxSelect:
                    m_action = EditorAction.None;
                    m_actionStartPoint = new Point(0, 0);
                    m_actionEndPoint = new Point(0, 0);
                    InvalidateVisual();
                    break;
                case EditorAction.MoveItem:
                    m_action = EditorAction.None;
                    m_actionStartPoint = new Point(0, 0);
                    m_actionEndPoint = new Point(0, 0);
                    break;
            }
            //if (m_isReadyToBoxSelect)
            //{
            //    m_isReadyToBoxSelect = false;
            //    m_boxSelectStartPoint = new Point(0, 0);
            //    m_boxSelectEndPoint = new Point(0, 0);
            //    InvalidateVisual();
            //}
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var mouseInItem = Items
            .Where(x => RectMapToViewer(new Rect(
                x.BoundingRect.Left - NearItemDistance,
                x.BoundingRect.Top - NearItemDistance,
                x.BoundingRect.Width + 2 * NearItemDistance,
                x.BoundingRect.Height + 2 * NearItemDistance))
            .Contains(e.GetPosition(this)))
            .OrderByDescending(x => x.ZIndex)
            .FirstOrDefault();

            foreach (var link in Links)
            {
                bool isNear = (PointPixelMap2Viewer(e.GetPosition(this))).IsPointNearToLine(link.StartPoint, link.EndPoint, 10);
                if (link.IsHovered != isNear)
                {
                    link.IsHovered = isNear;
                    InvalidateVisual();
                }
            }


            switch (m_action)
            {
                case EditorAction.Connect:
                    m_actionEndPoint = e.GetPosition(this);
                    InvalidateVisual();
                    break;
                case EditorAction.MoveViewer:
                    m_actionEndPoint = e.GetPosition(this);
                    MoveViewerWithOffset(new Vector(
                        m_actionEndPoint.X - m_actionStartPoint.X,
                        m_actionEndPoint.Y - m_actionStartPoint.Y));
                    m_actionStartPoint = m_actionEndPoint;
                    InvalidateVisual();
                    break;
                case EditorAction.MoveItem:
                    m_actionEndPoint = e.GetPosition(this);
                    var offset = new Vector(
                        (m_actionEndPoint.X - m_actionStartPoint.X) / ViewerScale,
                        (m_actionEndPoint.Y - m_actionStartPoint.Y) / ViewerScale);
                    Console.WriteLine(offset);
                    MoveSelectedItemsWithOffset(offset);
                    m_actionStartPoint = m_actionEndPoint;
                    InvalidateVisual();
                    break;
                case EditorAction.BoxSelect:
                    m_actionEndPoint = e.GetPosition(this);
                    Rect selectedBoxRect = new(m_actionStartPoint, m_actionEndPoint);
                    ToBoxSelect(selectedBoxRect);
                    break;
            }


            m_cursor_point = e.GetPosition(this);

            #region 事件分发

            #endregion

            #region Hover
            bool isNeedToRefreshHover = false;
            //鼠标下有元素
            if (mouseInItem != null)
            {
                isNeedToRefreshHover = true;
                foreach (var item in Items)
                {
                    item.IsHovered = item == mouseInItem;
                }
                WorkflowOption nearOption = GetNearOption(e.GetPosition(this));
                if (nearOption != null)
                {
                    nearOption.IsHovered = true;
                    Cursor = Cursors.Cross;
                }
                else
                {
                    Cursor = Cursors.Arrow;
                    foreach (var item in Items)
                    {
                        foreach (var option in item.Options)
                        {
                            option.IsHovered = false;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in Items)
                {
                    isNeedToRefreshHover = isNeedToRefreshHover || item.IsHovered;
                    item.IsHovered = false;
                    foreach (var option in item.Options)
                    {
                        option.IsHovered = false;
                    }
                }
                Cursor = Cursors.Arrow;
            }
            if (isNeedToRefreshHover) InvalidateVisual();

            #endregion
        }

        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            foreach (var item in Items)
            {
                item.OnTextInputed(e);
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                //return;
                double zoom = e.Delta < 0 ? 1.05 : 0.95;
                ViewerScale = zoom * ViewerScale;
                Console.Write(ViewerScale);
                InvalidateVisual();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DeleteSelectedItems(Items);
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                DeleteSelectedItems(Items);
                e.Handled = true;
            }
            else if (e.Key == Key.A && e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                SelectAllItems();
            }

            base.OnKeyDown(e);
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);

            if (e.Data.GetDataPresent(DataTypeExtension.DragDataModelFormat))
            {
                object data = e.Data.GetData(DataTypeExtension.DragDataModelFormat);
                if (!(data is Type)) return;
                var type = (Type)data;
                if (!type.IsSubclassOf(typeof(WorkflowNode))) return;
                WorkflowNode workflowNode= (WorkflowNode)Activator.CreateInstance(type);
                workflowNode.Pos = e.GetPosition(this);
                this.Items.Add(workflowNode);
            }
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            if (e.Data.GetDataPresent(DataTypeExtension.DragDataModelFormat))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        //protected override void OnDragOver(DragEventArgs e)
        //{
        //    base.OnDragOver(e);
        //}

        //protected override void OnDragLeave(DragEventArgs e)
        //{
        //    //e.Data
        //    base.OnDragLeave(e);
        //}
        #region Custom Events
        public event EventHandler ViewScaled;
        public event EventHandler SelectedChanged;
        public event EventHandler HoveredChanged;
        public event EventHandler ActiveChanged;
        public event EventHandler ItemAdded;
        public event EventHandler ItemRemoved;
        public event EventHandler ViewerMoved;
        public event EventHandler BoxSelectChanged;

        protected virtual void OnViewScaled(WorkflowEventArgs e) => ViewScaled?.Invoke(this, e);

        protected virtual void OnSelectedChanged(WorkflowEventArgs e) => SelectedChanged?.Invoke(this, e);

        protected virtual void OnHoveredChanged(WorkflowEventArgs e) => HoveredChanged?.Invoke(this, e);

        protected virtual void OnActiveChanged(WorkflowEventArgs e) => ActiveChanged?.Invoke(this, e);

        protected virtual void OnItemAdded(WorkflowEventArgs e) => ItemAdded?.Invoke(this, e);

        protected virtual void OnItemRemoved(WorkflowEventArgs e) => ItemRemoved?.Invoke(this, e);

        protected virtual void OnViewerMoved(WorkflowEventArgs e) => ViewerMoved?.Invoke(this, e);

        protected virtual void OnBoxSelectChanged(WorkflowEventArgs e) => BoxSelectChanged?.Invoke(this, e);
        #endregion

    }
    #endregion
}
