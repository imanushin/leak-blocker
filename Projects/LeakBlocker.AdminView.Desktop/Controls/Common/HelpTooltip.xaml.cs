using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common
{
    internal sealed partial class HelpTooltip
    {
        private readonly ToolTip tooltip;

        public HelpTooltip()
        {
            InitializeComponent();

            text.Inlines.Add(CreateHelpInjection(out tooltip));
        }

        public string HelpText
        {
            get
            {
                return (string)tooltip.Content;
            }
            set
            {
                tooltip.Content = value;
            }
        }

        internal static Span CreateHelpInjection(string text)
        {
            var result = new Span();

            ToolTip toolTip;

            var hyperlink = CreateHelpInjection(out toolTip);

            toolTip.Content = text;

            result.Inlines.Add(new Run("  "));
            result.Inlines.Add(hyperlink);

            return result;
        }

        private static Hyperlink CreateHelpInjection(out ToolTip externalToolTip)
        {
            var result = new Hyperlink(new Run("?"));

            ToolTipService.SetShowDuration(result, int.MaxValue);

            var tooltip = new ToolTip();

            externalToolTip = tooltip;
            result.ToolTip = externalToolTip;

            result.MouseEnter += (source, arg) => tooltip.IsOpen = true;
            result.MouseLeave += (source, arg) => tooltip.IsOpen = false;

            return result;
        }
    }
}
