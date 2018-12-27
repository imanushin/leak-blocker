using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using LeakBlocker.AdminView.Desktop.Controls.Standard.Views;
using LeakBlocker.AdminView.Desktop.Resources;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class ManagedComputerViewsToStatistic : AbstractConverter<ItemCollection, Span>
    {
        protected override Span Convert(ItemCollection baseValue)
        {
            return GetStatusEntry(baseValue);
        }

        public static Span GetStatusEntry(ItemCollection baseValue)
        {
            ReadOnlySet<ManagedComputerView> computerViews = baseValue.Cast<ManagedComputerView>().ToReadOnlySet();

            var result = new Span();

            result.Inlines.Add(AdminViewResources.AgentsInScope.Combine(computerViews.Count));

            ILookup<ManagedComputerStatus, ManagedComputerView> actualStatuses = computerViews.ToLookup(computer => computer.Status);

            bool isFirst = true;

            foreach (IGrouping<ManagedComputerStatus, ManagedComputerView> managedComputerViews in actualStatuses)
            {
                if (isFirst)
                    result.Inlines.Add(":  ");
                else
                    result.Inlines.Add(";  ");
                
                FrameworkElement image = ManagedComputerStatusToImageConverter.GetStatusImage(managedComputerViews.Key);
                image.Height = 12;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.ToolTip = managedComputerViews.Key.GetValueDescription();
                int count = managedComputerViews.Count();

                result.Inlines.Add(image);
                result.Inlines.Add(AdminViewResources.ScopeCountTemplate.Combine(count));
                isFirst = false;
            }

            return result;
        }
    }
}
