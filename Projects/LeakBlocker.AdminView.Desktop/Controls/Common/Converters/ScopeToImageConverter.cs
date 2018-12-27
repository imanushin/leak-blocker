using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LeakBlocker.AdminView.Desktop.Generated;
using LeakBlocker.Libraries.Common;
using LeakBlocker.ServerShared.AdminViewCommunication;

namespace LeakBlocker.AdminView.Desktop.Controls.Common.Converters
{
    internal sealed class ScopeToImageConverter : AbstractConverter<Scope,FrameworkElement>
    {
        protected override FrameworkElement Convert(Scope scope)
        {
            return ImageForScope(scope);
        }

        public static FrameworkElement ImageForScope(object input)
        {
            return ImageForScope((Scope)input);
        }

        private static FrameworkElement ImageForScope(Scope scope)
        {
            switch (scope.ScopeType)
            {
                case ScopeType.Domain:
                    return new ObjectTypeDomain();
                case ScopeType.OU:
                    return new ObjectTypeOrganizaitionalUnit();
                case ScopeType.Computer:
                    return new ObjectTypeComputer();
                case ScopeType.Group:
                    return new ObjectTypeGroupUsers();
                case ScopeType.User:
                    return new ObjectTypeUser();
                    
                default:
                    throw new ArgumentOutOfRangeException("scope");
            }
        }
    }
}
