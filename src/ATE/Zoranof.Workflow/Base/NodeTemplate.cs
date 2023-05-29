using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Zoranof.WorkFlow;

namespace Zoranof.Workflow.Base
{
    [NodeAttribute(title: "NodeTemplate", group: "Template")]
    public class NodeTemplate : WorkflowNode
    {
        public NodeTemplate() : base(false)
        {
            Width = 240;
            Pos = new Point(0, 0);
            Title = "Just A Template";
            BoundingRect = new Rect(Pos.X, Pos.Y, Width, Height);
            Options.Add(new WorkflowOption(this) { Type = OptionType.Input });
            Options.Add(new WorkflowOption(this) { Type = OptionType.Input });
            Options.Add(new WorkflowOption(this) { Type = OptionType.Input });
        }

        public Brush ThemeColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D3E5F4"));

        public Brush ActiveThemeColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A6B93"));

        public Brush OptionColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81B5E0"));

        public Brush OptionSelectedColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#81B5E0"));

        public Brush OptionErrorColor { get; set; } = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E66781"));

        public Thickness Margin { get; set; } = new Thickness(10);

        public double GapHeight = 5;

        public override double Height { get => TitleHeight + ContentHeight + OptionsHeight + OtherHeight + GapHeight; set => base.Height = value; }

        public double DefaultOptionHeight { get; set; } = 30;

        public double TitleHeight { get; set; } = 40;

        public double ContentHeight { get; set; } = 200;

        public double OptionsHeight => Options.Where(o => o.Type == OptionType.Output || o.Type == OptionType.Input).Count() * DefaultOptionHeight;

        public double OtherHeight { get; set; }

        #region Events

        //public override void OnDrawFramework(DrawingContext drawingContext)
        //{
        //    Brush bg = Brushes.White;
        //    Pen pen = new Pen(IsSelected || IsHovered ? Brushes.DeepSkyBlue : Brushes.LightGray, 2);
        //    drawingContext.DrawRoundedRectangle(ThemeColor, pen, SelfRect, 12, 12);

        //    pen.Thickness = 0;
        //    drawingContext.DrawRoundedRectangle(Brushes.White, pen, new Rect(4, TitleHeight, Width - 8, Height - TitleHeight- 4), 8, 8);
        //}

        //public override void OnDrawConnectOption(DrawingContext drawingContext)
        //{
        //    OnDrawInputOptions(drawingContext);
        //    OnDrawOutputOptions(drawingContext);
        //}

        //public virtual void OnDrawInputOptions(DrawingContext drawingContext) 
        //{
        //    var inputOptions = Options.Where(o => o.Type == OptionType.Input).ToList();
        //    var height = DefaultOptionHeight * inputOptions.Count;
        //    int i = 0;

        //    drawingContext.DrawRoundedRectangle(
        //        ThemeColor, new Pen(ThemeColor, 0),
        //        new Rect(Width - 12, Height - (inputOptions.Count() - 0.25) * DefaultOptionHeight - 50, 16, (inputOptions.Count() - 0.5) * DefaultOptionHeight),
        //        8, 8);

        //    drawingContext.DrawRoundedRectangle(
        //        ThemeColor, new Pen(ThemeColor, 0),
        //        new Rect(-4, Height - (inputOptions.Count() - 0.25) * DefaultOptionHeight - 50, 16, (inputOptions.Count() - 0.5) * DefaultOptionHeight),
        //        8, 8);

        //    foreach (var option in inputOptions)
        //    {
        //        Brush bg = option.IsConnecting ? Brushes.Green : Brushes.White;

        //        Pen pen = new Pen(Brushes.Orange, 2);

        //        drawingContext.DrawEllipse(bg, pen, new Point(4, Height - (i + 0.5) * DefaultOptionHeight - 50), 4, 4);
        //        drawingContext.DrawEllipse(bg, pen, new Point(Width - 4, Height - (i + 0.5) * DefaultOptionHeight - 50), 4, 4);
                
        //        i++;
        //    }
        //}

        public virtual void OnDrawOutputOptions(DrawingContext drawingContext) { }
        #endregion


        //[Obsolete]
        //public override void OnDrawFramework(DrawingContext drawingContext)
        //{
        //    base.OnDrawFramework(drawingContext);
        //    Brush bg = (IsSelected || IsActive) ? Brushes.White : Brushes.White;
        //    Pen borderPen = (IsSelected || IsActive) ?
        //        new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4153FE")), 2)
        //        : new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999")), 2);
        //    DropShadowBitmapEffect shadow = new DropShadowBitmapEffect();
        //    if (IsSelected || IsActive)
        //    {
        //        shadow = new DropShadowBitmapEffect() { Noise = 1, ShadowDepth = -4, Color = Colors.Black, Opacity = 0.2 };
        //    }
        //    drawingContext.PushEffect(shadow, null);
        //    drawingContext.DrawRoundedRectangle(bg, borderPen, SelfRect, 10, 10);
        //    drawingContext.Pop();
        //}

        //public override void OnDrawTitle(DrawingContext drawingContext)
        //{
        //    for (int i = 0; i < 6; i++)
        //    {
        //        drawingContext.DrawEllipse(
        //            new SolidColorBrush((Color)ColorConverter.ConvertFromString("#c2c2c2")),
        //            new Pen(Brushes.Transparent, 0),
        //            new Point((20 + (i % 2) * 10), ((i / 2) - 1) * 10 + Height / 2) , 2, 2);
        //    }
        //    FormattedText ft = new FormattedText(Title ?? "默认节点", CultureInfo.CurrentCulture,
        //        FlowDirection.LeftToRight,
        //        new Typeface("LXGW WenKai"),
        //        16,
        //        Brushes.Black, 100);
        //    drawingContext.DrawText(ft, new Point(50, 20));
        //}
    }
}
