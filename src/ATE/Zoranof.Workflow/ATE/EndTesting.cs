using WorkflowCore.Interface;
using WorkflowCore.Models;

namespace Zoranof.Workflow.ATE
{
    [NodeAttribute(title: "结束测试", group: "ATE")]
    public class EndTesting : TitleOnlyTemplate
    {
        public EndTesting()
        {
            Width = 100;
            Height = 30;
            Title = "结束测试";
        }

        //public override void OnDrawFramework(DrawingContext drawingContext)
        //{
        //    drawingContext.DrawRoundedRectangle(
        //        new SolidColorBrush((Color)ColorConverter.ConvertFromString("#782787")),
        //        new Pen(Brushes.Transparent, 0),
        //        SelfRect, 15, 15);

        //    if (IsHovered || IsSelected)
        //    {
        //        drawingContext.DrawRoundedRectangle(
        //            Brushes.Transparent,
        //            new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205363")), 2),
        //            SelfRect, 15, 15);
        //    }
        //}

        //public override void OnDrawTitle(DrawingContext drawingContext)
        //{
        //    base.OnDrawTitle(drawingContext);
        //    var ft = new FormattedText("开始测试", 
        //        System.Globalization.CultureInfo.CurrentCulture, 
        //        System.Windows.FlowDirection.LeftToRight,                               
        //        new Typeface("LXGW WenKai"), 12, Brushes.White, 100);
            
        //    drawingContext.DrawText(ft, new Point(Width / 2 - ft.Width / 2, Height / 2 - ft.Height / 2));
        //}
    }


    public class EndTestingStep : StepBody
    {
        public override ExecutionResult Run(IStepExecutionContext context)
        {
            return ExecutionResult.Next();
        }
    }
}
