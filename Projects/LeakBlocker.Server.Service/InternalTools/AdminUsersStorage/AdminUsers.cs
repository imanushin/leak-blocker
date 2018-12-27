using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Server.Service.InternalTools.AdminUsersStorage
{
    [DataContract(IsReference = true)]
    internal sealed class AdminUsers : BaseReadOnlyObject
    {
        private static readonly AdminUsers empty = new AdminUsers(new Dictionary<int, AdminUserData>());

        public static AdminUsers Empty
        {
            get
            {
                return empty;
            }
        }

        [DataMember]
        private readonly ReadOnlyDictionary<int, AdminUserData> users;

        public AdminUsers(IDictionary<int, AdminUserData> users)
        {
            Check.ObjectIsNotNull(users, "users");

            this.users = users.ToReadOnlyDictionary();
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return users;
        }

        public AdminUserData GetUser(int id)
        {
            return users[id];
        }

        public AdminUsers AddUser(AdminUserData user, out int newId)
        {
            newId = 1;

            if (users.Keys.Any())
                newId = users.Keys.Max() + 1;

            var dictionary = users.ToDictionary(pair => pair.Key, pair => pair.Value);

            dictionary.Add(newId, user);

            return new AdminUsers(dictionary);
        }
    }
}
