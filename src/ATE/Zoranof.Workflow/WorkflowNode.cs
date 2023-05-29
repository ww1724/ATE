using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WorkflowCore.Models;
using Zoranof.Workflow;
using Zoranof.Workflow.Common;

namespace Zoranof.WorkFlow
{
    public class WorkflowNode 
    {
        public WorkflowNode(bool applyDefaultOptions = true)
        {
            AcceptDrops = false;
            AcceptHoverEvents = true;
            AcceptTouchEvents = false;
            Width = 300;
            Height = 60;
            Pos = new Point(0, 0);
            Id = Guid.NewGuid().ToString();

            Options = new List<WorkflowOption>();
            Background = Brushes.Transparent;
            NearOptionDistance = 30;

            if (applyDefaultOptions) ApplyDefaultOptions();

            // 事件初始化

            ItemResized += (o, e) =>
            {
                BoundingRect = new Rect(pos.X, pos.Y, Width, Height);
            };
            ItemMoved += (o, e) =>
            {
                BoundingRect = new Rect(pos.X, pos.Y, Width, Height);
            };

        }

        #region Data
        public string Id { get; set; }

        public object Data { get { return data; } set { data = value; } }

        public Type StepBodyType { get; set; }
        #endregion

        #region Private Member
        private Point pos;
        private double width;
        private double height;
        private object data;
        #endregion

        #region Fields
        public string Title { get; set; }

        public char Icon { get; set; }

        public string Description { get; set; }

        public virtual List<WorkflowOption> Options { get; set; }

        public WorkflowEditor AttachedView { get; set; }

        public double NearOptionDistance { get; set; }

        public Brush Background { get; set; }

        /// <summary>
        /// 是否接收Drop事件
        /// </summary>
        public bool AcceptDrops { get; set; }

        /// <summary>
        /// 是否接收悬浮事件
        /// </summary>
        public bool AcceptHoverEvents { get; set; }

        /// <summary>
        /// 是否接收触摸事件
        /// </summary>
        public bool AcceptTouchEvents { get; set; }

        /// <summary>
        /// 抓取所有鼠标事件
        /// </summary>
        public bool GrabMouse { get; set; }

        /// <summary>
        /// 抓取所有键盘事件
        /// </summary>
        public bool GrabKeyboard { get; set; }

        /// <summary>
        /// 鼠标形状
        /// </summary>
        public Cursor Cursor { get; set; }

        public double Opacity { get; set; }

        public int Rotation { get; set; }

        public double Scale { get; set; }

        public double Width { get => width; set { width = value; OnItemResized(null); } }


        public virtual double Height { get => height; set { height = value; OnItemResized(null); } }
        #endregion

        #region Variables
        public WorkflowOption ActiveOption { get; set; }
        public WorkflowOption HoverOprion { get; set; }

        /// <summary>
        /// 当前位置
        /// </summary>
        public Point Pos
        {
            get => pos;
            set { pos = value; OnItemMoved(null); }
        }
        /// <summary>
        /// 父项目
        /// </summary>
        public WorkflowNode ParentItem { get; set; }

        /// <summary>
        /// 子项目
        /// </summary>
        public List<WorkflowNode> ChildItems { get; set; }

        /// <summary>
        /// Item边框
        /// </summary>
        public Rect BoundingRect { get; set; }

        public Rect SelfRect { get => new Rect(0, 0, width, Height); }

        /// <summary>
        /// 元数据
        /// </summary>
        public Dictionary<string, object> MetaData { get; set; }

        /// <summary>
        /// 是否被选择
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 是否鼠标悬停
        /// </summary>
        public bool IsHovered { get; set; }

        /// <summary>
        /// 是否按下鼠标
        /// </summary>
        public bool IsMousePressed { get; set; }

        public bool IsDragging { get; set; }

        /// <summary>
        /// 缩放
        /// </summary>
        public int Scalage { get; set; }

        public int ZIndex { get; set; }
        #endregion

        #region public slots
        public virtual string ExportToJson() {
            var a = new
            {
                Id,
                Width,
                Height,
                Pos,
            };

            return ""; 
        }

        public virtual void UpdateOptions()
        {

            SetDefaultOptions();
        }

        /// <summary>
        /// 应用默认选项点
        /// </summary>
        public void ApplyDefaultOptions()
        {
            ItemResized += (sender, e) =>
            {
                SetDefaultOptions();
            };
            OnItemResized(null);
        }

        /// <summary>
        /// 更新连接
        /// </summary>
        public virtual void UpdateOptionsPosition() { }

        /// <summary>
        /// 获取Item边界
        /// </summary>
        /// <returns></returns>
        public virtual Rect GetBoundingRect() => new(pos.X, pos.Y, Width, Height);

        public void Update()
        {
            AttachedView.InvalidateVisual();
        }
        #endregion

        #region Private Slots
        void SetDefaultOptions()
        {
            Options = new List<WorkflowOption>()
                {
                    new WorkflowOption(this) { CenterPos = new Point(Width / 2, 0), Location=OptionLocation.Top,  },
                    new WorkflowOption(this) { CenterPos = new Point(Width / 2, Height), Location=OptionLocation.Bottom },
                    new WorkflowOption(this) { CenterPos = new Point(0, Height / 2), Location=OptionLocation.Left },
                    new WorkflowOption(this) { CenterPos = new Point(Width, Height / 2), Location=OptionLocation.Right },
                };
        }
        #endregion

        #region Drawing Events
        /// <summary>
        /// 自绘
        /// </summary>
        /// <param name="drawingContext"></param>
        public  virtual void OnRender(DrawingContext drawingContext)
        {
            drawingContext.PushTransform(new TranslateTransform(Pos.X, Pos.Y));

            OnDrawFramework(drawingContext);

            OnDrawTitle(drawingContext);

            OnDrawContent(drawingContext);

            OnDrawConnectOption(drawingContext);

            OnDrawConnectLine(drawingContext);

            OnDrawBeforeMark(drawingContext);

            OnDrawMark(drawingContext);

            //OnDrawResizeBorder(drawingContext);

            drawingContext.Pop();
        }

        /// <summary>
        /// 绘制框架 背景+边框
        /// </summary>
        /// <param name="drawingContext"></param>
        public  virtual void OnDrawFramework(DrawingContext drawingContext) {

            drawingContext.DrawRoundedRectangle(Brushes.Transparent,
                new Pen(Brushes.LightGray, 2),
                SelfRect, 4, 4);
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="drawingContext"></param>
        public  virtual void OnDrawTitle(DrawingContext drawingContext) { }

        /// <summary>
        /// 绘制内容
        /// </summary>
        /// <param name="drawingContext"></param>
        public  virtual void OnDrawContent(DrawingContext drawingContext) { }

        /// <summary>
        /// 绘制连接点
        /// </summary>
        /// <param name="drawingContext"></param>
        public  virtual void OnDrawConnectOption(DrawingContext drawingContext)
        {
            foreach (var option in Options)
            {
                if (!(IsHovered || option.IsConnecting || IsSelected || IsActive)) continue;
                Pen dotPen = option.IsHovered ? new Pen(Brushes.Blue, 1) : new Pen(Brushes.Black, 1);
                Brush bg = option.IsHovered ? Brushes.Red : Brushes.White;
                drawingContext.DrawEllipse(bg, dotPen, option.CenterPos, 4, 4);
            }
        }

        public  virtual void OnDrawBeforeMark(DrawingContext drawingContext) { }

        /// <summary>
        /// 绘制Resize边框
        /// </summary>
        /// <param name="drawingContext"></param>
        public virtual void OnDrawResizeBorder(DrawingContext drawingContext)
        {

            if (IsActive)
            {
                ICollection<Rect> rects = new Collection<Rect>()
                {
                    new Rect(0 - 3, 0 - 3, 6, 6),
                    new Rect(Width / 2 - 3, 0 - 3, 6, 6),
                    new Rect(Width -3, 0 - 3, 6, 6),
                    new Rect(0 - 3, Height / 2 - 3, 6, 6),
                    new Rect(Width -3, Height / 2 - 3, 6, 6),
                    new Rect(0 - 3, Height - 3, 6, 6),
                    new Rect(Width / 2 - 3, Height - 3, 6, 6),
                    new Rect(Width -3, Height - 3, 6, 6)
                };
                drawingContext.DrawRectangle(Brushes.Transparent, new Pen(Brushes.Black, 1), BoundingRect);
                drawingContext.PushTransform(new TranslateTransform(Pos.X, Pos.Y));

                foreach (Rect rect in rects)
                {
                    drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.CadetBlue, 1), rect);
                }
                drawingContext.Pop();
            }

        }

        /// <summary>
        /// 绘制连接线
        /// </summary>
        /// <param name="drawingContext"></param>
        public virtual void OnDrawConnectLine(DrawingContext drawingContext)
        {

        }

        /// <summary>
        /// 绘制标记
        /// </summary>
        /// <param name="drawingContext"></param>
        public  virtual void OnDrawMark(DrawingContext drawingContext)
        {

        }

        #endregion

        #region Custom Events
        public event EventHandler ItemResized;

        public event EventHandler ItemMoved;

        public event EventHandler SizeChanged;

        public event EventHandler OptionChanged;

        public event EventHandler TextInputed;

        public event EventHandler ContentExported;

        public virtual void OnSizeChanged(EventArgs e) => SizeChanged?.Invoke(this, e);

        public virtual void OnOptionChanged(EventArgs e) => OptionChanged?.Invoke(this, e);

        public virtual void OnItemResized(EventArgs e) => ItemResized?.Invoke(this, e);

        public virtual void OnItemMoved(EventArgs e) => ItemMoved?.Invoke(this, e);

        public virtual void OnTextInputed(TextCompositionEventArgs e) => TextInputed?.Invoke(this, e);

        public virtual void OnContentExported(EventArgs e) => ContentExported?.Invoke(this, e);
        #endregion
    }



}
