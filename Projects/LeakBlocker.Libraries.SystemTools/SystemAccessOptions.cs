using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// Optional credentials for accessing the remote system.
    /// </summary>
    public struct SystemAccessOptions
    {
        /// <summary>
        /// Default instance. Means local machine without credentials.
        /// </summary>
        public static SystemAccessOptions Default
        {
            get
            {
                return default(SystemAccessOptions);
            }
        }

        /// <summary>
        /// Target system name.
        /// </summary>
        public string SystemName
        {
            get;
            private set;
        }

        /// <summary>
        /// Full user name (can be in any format: domain\username, or UPN or only username).
        /// </summary>
        public string UserName
        {
            get;
            private set;
        }

        /// <summary>
        /// Use account password.
        /// </summary>
        public string Password
        {
            get;
            private set;
        }

        /// <summary>
        /// User domain name or null if domain was not specified.
        /// </summary>
        public string DomainName
        {
            get
            {
                if (UserName == null)
                    return null;

                return NameConversion.GetUserDomainName(UserName);
            }
        }

        /// <summary>
        /// User name without domain.
        /// </summary>
        public string ShortUserName
        {
            get
            {
                if (UserName == null)
                    return null;

                return NameConversion.SimplifyUserName(UserName);
            }
        }

        /// <summary>
        /// Creates an empty credentials set for the specified system.
        /// </summary>
        /// <param name="systemName">Target system name.</param>
        public SystemAccessOptions(string systemName = null)
            : this()
        {
            SystemName = systemName;
        }

        /// <summary>
        /// Creates a crredential set. Both user name and password should be either null or not null.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="systemName">Target system name (null means local machine).</param>
        public SystemAccessOptions(string userName, string password, string systemName = null)
            : this(systemName)
        {
            if ((userName == null) && (password == null))
                return;

            Check.StringIsMeaningful(userName, "userName");
            Check.StringIsMeaningful(password, "password");

            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Converts the specified Credentials instance to the SystemAccessOptions instance.
        /// </summary>
        public static implicit operator SystemAccessOptions(Credentials value)
        {
            return (value == null) ? new SystemAccessOptions(null) : 
                new SystemAccessOptions(value.User, value.Password, value.Domain.FullName);
        }

        /// <summary>
        /// Converts the specified Credentials instance to the SystemAccessOptions instance.
        /// </summary>
        public static SystemAccessOptions FromCredentials(Credentials value)
        {
            return value;
        }

        /// <summary>
        /// Returns true if objects are equal.
        /// </summary>
        public static bool operator ==(SystemAccessOptions left, SystemAccessOptions right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Returns true if objects are not equal.
        /// </summary>
        public static bool operator !=(SystemAccessOptions left, SystemAccessOptions right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is SystemAccessOptions))
                return false;

            var systemAccessOptions = (SystemAccessOptions)obj;

            return string.Equals(SystemName, systemAccessOptions.SystemName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(UserName, systemAccessOptions.UserName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(Password, systemAccessOptions.Password, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        // ReSharper disable NonReadonlyFieldInGetHashCode
        public override int GetHashCode()
        {
            return (SystemName ?? string.Empty).GetHashCode() ^ (UserName ?? string.Empty).GetHashCode() ^ (Password ?? string.Empty).GetHashCode();
        }

        /// <summary>
        /// Returns string description.
        /// </summary>
        public override string ToString()
        {
            return "Target system: " + (SystemName ?? "<LOCAL>") + " User name: " + (UserName ?? "<CURRENT>");
        }
    }
}
