using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class CancelTemporaryAccessConditionStringConverter : AbstractConverter<BaseTemporaryAccessCondition,string>
    {
        protected override string Convert(BaseTemporaryAccessCondition condition)
        {
            return ConvertCondition(condition);
        }

        public static string ConvertCondition(BaseTemporaryAccessCondition condition)
        {
            string originalString = condition.ToString();

            return originalString.Replace(". ", "." + Environment.NewLine);
        }
    }
}
