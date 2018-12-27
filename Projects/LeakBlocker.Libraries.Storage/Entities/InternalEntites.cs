using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using LeakBlocker.Libraries.Common.Collections;

namespace LeakBlocker.Libraries.Storage.Entities
{
// ReSharper disable UnusedMember.Global
// ReSharper disable AnnotationRedundanceAtValueType
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedParameter.Global
// ReSharper disable MemberCanBeInternal
// ReSharper disable RedundantCast
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable EmptyConstructor.Global
// ReSharper disable PublicConstructorInAbstractClass.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable ClassCanBeSealed.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable VirtualMemberNeverOverriden.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbBaseRuleCondition : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateBaseRuleCondition(BaseRuleCondition entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            Not = entity.Not;

        }

        private Boolean innerNot;

        /// <summary>
        /// Property from <see cref="BaseRuleCondition"/>
        /// </summary>
        [Required]
        public virtual Boolean Not
        {
            get
            {
                return innerNot;
            }
            set
            {
                innerNot = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="BaseRuleCondition"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="BaseRuleCondition"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }
        internal abstract BaseRuleCondition ForceGetBaseRuleCondition();


        internal static DbBaseRuleCondition ConvertFromBaseRuleCondition(BaseRuleCondition entity, IDatabaseModel model)
        {
            var entityAsDeviceTypeRuleCondition = entity as DeviceTypeRuleCondition;

            if (entityAsDeviceTypeRuleCondition != null)
                return DbDeviceTypeRuleCondition.ConvertFromDeviceTypeRuleCondition(entityAsDeviceTypeRuleCondition, model);

            var entityAsCompositeRuleCondition = entity as CompositeRuleCondition;

            if (entityAsCompositeRuleCondition != null)
                return DbCompositeRuleCondition.ConvertFromCompositeRuleCondition(entityAsCompositeRuleCondition, model);

            var entityAsComputerListRuleCondition = entity as ComputerListRuleCondition;

            if (entityAsComputerListRuleCondition != null)
                return DbComputerListRuleCondition.ConvertFromComputerListRuleCondition(entityAsComputerListRuleCondition, model);

            var entityAsDeviceListRuleCondition = entity as DeviceListRuleCondition;

            if (entityAsDeviceListRuleCondition != null)
                return DbDeviceListRuleCondition.ConvertFromDeviceListRuleCondition(entityAsDeviceListRuleCondition, model);

            var entityAsUserListRuleCondition = entity as UserListRuleCondition;

            if (entityAsUserListRuleCondition != null)
                return DbUserListRuleCondition.ConvertFromUserListRuleCondition(entityAsUserListRuleCondition, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }


        internal static IQueryable<DbBaseRuleCondition> OfRuntimeType(IQueryable<DbBaseRuleCondition> baseQuery, BaseRuleCondition entity)
        {
            if(entity is DeviceTypeRuleCondition)
                return baseQuery.OfType<DbDeviceTypeRuleCondition>();

            if(entity is CompositeRuleCondition)
                return baseQuery.OfType<DbCompositeRuleCondition>();

            if(entity is ComputerListRuleCondition)
                return baseQuery.OfType<DbComputerListRuleCondition>();

            if(entity is DeviceListRuleCondition)
                return baseQuery.OfType<DbDeviceListRuleCondition>();

            if(entity is UserListRuleCondition)
                return baseQuery.OfType<DbUserListRuleCondition>();


            throw new InvalidOperationException( "Type {0} isn't supported".Combine(entity.GetType()));
        }

        internal BaseRuleCondition GetBaseRuleCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetBaseRuleCondition();

                CachedItem = createdItem;
            }

            return (BaseRuleCondition)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbBaseRuleCondition"/> to  <see cref="BaseRuleCondition"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected BaseRuleCondition CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDeviceTypeRuleCondition : DbBaseRuleCondition
    {
        internal void UpdateDeviceTypeRuleCondition(BaseRuleCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (DeviceTypeRuleCondition)baseEntity;
            UpdateBaseRuleCondition(entity, model);
            DeviceType = entity.DeviceType;

        }


        /// <summary>
        /// Inner property for database
        /// </summary>

        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public virtual int DeviceTypeIntValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="DeviceTypeRuleCondition"/>
        /// </summary>
        internal DeviceCategory DeviceType
        {
            get
            {
                var result = (DeviceCategory)DeviceTypeIntValue;

                Check.EnumerationValueIsDefined((DeviceCategory)DeviceTypeIntValue);

                return result;
            }
            set
            {
                Check.EnumerationValueIsDefined(value, "value");

                DeviceTypeIntValue = (int)value;
           
                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseRuleCondition ForceGetBaseRuleCondition()
        {
            return ForceGetDeviceTypeRuleCondition();
        }


        internal static DbDeviceTypeRuleCondition ConvertFromDeviceTypeRuleCondition(DeviceTypeRuleCondition entity, IDatabaseModel model)
        {
            var result = model.BaseRuleConditionSet.Create<DbDeviceTypeRuleCondition>();
            result.UpdateDeviceTypeRuleCondition(entity,model);
            model.BaseRuleConditionSet.Add(result);

            return result;
        }




        internal DeviceTypeRuleCondition GetDeviceTypeRuleCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDeviceTypeRuleCondition();

                CachedItem = createdItem;
            }

            return (DeviceTypeRuleCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbAuditItem : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateAuditItem(AuditItem entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            EventType = entity.EventType;
            Time = entity.Time;
            Computer = DbBaseComputerAccount.ConvertFromBaseComputerAccount(entity.Computer, model);
            User = entity.User == null ? null : DbBaseUserAccount.ConvertFromBaseUserAccount(entity.User, model);
            TextData = entity.TextData;
            AdditionalTextData = entity.AdditionalTextData;
            Device = entity.Device == null ? null : DbDeviceDescription.ConvertFromDeviceDescription(entity.Device, model);
            Configuration = entity.Configuration;
            Number = entity.Number;

        }


        /// <summary>
        /// Inner property for database
        /// </summary>
        [Required]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public virtual int EventTypeIntValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        internal AuditItemType EventType
        {
            get
            {
                var result = (AuditItemType)EventTypeIntValue;

                Check.EnumerationValueIsDefined((AuditItemType)EventTypeIntValue);

                return result;
            }
            set
            {
                Check.EnumerationValueIsDefined(value, "value");

                EventTypeIntValue = (int)value;
           
                CachedItem = null;
            }
        }

        /// <summary>
        /// Inner property for time representation in database
        /// </summary>
        [NotNull]
        [Required]
        public virtual long TimeTicks
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [NotMapped]
        internal Time Time
        {
            get
            {
                return new Time(TimeTicks);
            }
            set
            {
                Check.ObjectIsNotNull(value, "value");

                TimeTicks = value.Ticks;
           
                CachedItem = null;
            }
        }
        private DbBaseComputerAccount innerComputer;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual DbBaseComputerAccount Computer
        {
            get
            {
                return innerComputer;
            }
            set
            {
                innerComputer = value;

                CachedItem = null;
            }
        }
        private DbBaseUserAccount innerUser;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [CanBeNull]
        public virtual DbBaseUserAccount User
        {
            get
            {
                return innerUser;
            }
            set
            {
                innerUser = value;

                CachedItem = null;
            }
        }
        private String innerTextData;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [CanBeNull]
        public virtual String TextData
        {
            get
            {
                return innerTextData;
            }
            set
            {
                innerTextData = value;

                CachedItem = null;
            }
        }
        private String innerAdditionalTextData;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [CanBeNull]
        public virtual String AdditionalTextData
        {
            get
            {
                return innerAdditionalTextData;
            }
            set
            {
                innerAdditionalTextData = value;

                CachedItem = null;
            }
        }
        private DbDeviceDescription innerDevice;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [CanBeNull]
        public virtual DbDeviceDescription Device
        {
            get
            {
                return innerDevice;
            }
            set
            {
                innerDevice = value;

                CachedItem = null;
            }
        }
        private Int32 innerConfiguration;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>

        public virtual Int32 Configuration
        {
            get
            {
                return innerConfiguration;
            }
            set
            {
                innerConfiguration = value;

                CachedItem = null;
            }
        }
        private Int32 innerNumber;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>

        public virtual Int32 Number
        {
            get
            {
                return innerNumber;
            }
            set
            {
                innerNumber = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="AuditItem"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbAuditItem ConvertFromAuditItem(AuditItem entity, IDatabaseModel model)
        {
            var result = model.AuditItemSet.Create<DbAuditItem>();
            result.UpdateAuditItem(entity,model);
            model.AuditItemSet.Add(result);

            return result;
        }




        internal AuditItem GetAuditItem()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetAuditItem();

                CachedItem = createdItem;
            }

            return (AuditItem)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbAuditItem"/> to  <see cref="AuditItem"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected AuditItem CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDeviceDescription : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateDeviceDescription(DeviceDescription entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            FriendlyName = entity.FriendlyName;
            Identifier = entity.Identifier;
            Category = entity.Category;

        }

        private String innerFriendlyName;

        /// <summary>
        /// Property from <see cref="DeviceDescription"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String FriendlyName
        {
            get
            {
                return innerFriendlyName;
            }
            set
            {
                innerFriendlyName = value;

                CachedItem = null;
            }
        }
        private String innerIdentifier;

        /// <summary>
        /// Property from <see cref="DeviceDescription"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String Identifier
        {
            get
            {
                return innerIdentifier;
            }
            set
            {
                innerIdentifier = value;

                CachedItem = null;
            }
        }

        /// <summary>
        /// Inner property for database
        /// </summary>
        [Required]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public virtual int CategoryIntValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="DeviceDescription"/>
        /// </summary>
        internal DeviceCategory Category
        {
            get
            {
                var result = (DeviceCategory)CategoryIntValue;

                Check.EnumerationValueIsDefined((DeviceCategory)CategoryIntValue);

                return result;
            }
            set
            {
                Check.EnumerationValueIsDefined(value, "value");

                CategoryIntValue = (int)value;
           
                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="DeviceDescription"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="DeviceDescription"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbDeviceDescription ConvertFromDeviceDescription(DeviceDescription entity, IDatabaseModel model)
        {
            DbDeviceDescription cachedResult = model.DeviceDescriptionCache.Get<DbDeviceDescription, DeviceDescription>(entity, model.DeviceDescriptionSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDeviceDescription(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.DeviceDescriptionSet.Create<DbDeviceDescription>();
            result.UpdateDeviceDescription(entity,model);
            model.DeviceDescriptionSet.Add(result);
            model.DeviceDescriptionCache.Add(entity, result);
            return result;
        }




        internal DeviceDescription GetDeviceDescription()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDeviceDescription();

                CachedItem = createdItem;
            }

            return (DeviceDescription)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbDeviceDescription"/> to  <see cref="DeviceDescription"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected DeviceDescription CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbAccount : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateAccount(Account entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            FullName = entity.FullName;
            ShortName = entity.ShortName;
            CanonicalName = entity.CanonicalName;
            Identifier = DbAccountSecurityIdentifier.ConvertFromAccountSecurityIdentifier(entity.Identifier, model);

        }

        private String innerFullName;

        /// <summary>
        /// Property from <see cref="Account"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String FullName
        {
            get
            {
                return innerFullName;
            }
            set
            {
                innerFullName = value;

                CachedItem = null;
            }
        }
        private String innerShortName;

        /// <summary>
        /// Property from <see cref="Account"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String ShortName
        {
            get
            {
                return innerShortName;
            }
            set
            {
                innerShortName = value;

                CachedItem = null;
            }
        }
        private String innerCanonicalName;

        /// <summary>
        /// Property from <see cref="Account"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String CanonicalName
        {
            get
            {
                return innerCanonicalName;
            }
            set
            {
                innerCanonicalName = value;

                CachedItem = null;
            }
        }
        private DbAccountSecurityIdentifier innerIdentifier;

        /// <summary>
        /// Property from <see cref="Account"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual DbAccountSecurityIdentifier Identifier
        {
            get
            {
                return innerIdentifier;
            }
            set
            {
                innerIdentifier = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="Account"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="Account"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }
        internal abstract Account ForceGetAccount();


        internal static DbAccount ConvertFromAccount(Account entity, IDatabaseModel model)
        {
            DbAccount cachedResult = model.AccountCache.Get<DbAccount, Account>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var entityAsDomainAccount = entity as DomainAccount;

            if (entityAsDomainAccount != null)
                return DbDomainAccount.ConvertFromDomainAccount(entityAsDomainAccount, model);

            var entityAsDomainComputerAccount = entity as DomainComputerAccount;

            if (entityAsDomainComputerAccount != null)
                return DbDomainComputerAccount.ConvertFromDomainComputerAccount(entityAsDomainComputerAccount, model);

            var entityAsDomainComputerGroupAccount = entity as DomainComputerGroupAccount;

            if (entityAsDomainComputerGroupAccount != null)
                return DbDomainComputerGroupAccount.ConvertFromDomainComputerGroupAccount(entityAsDomainComputerGroupAccount, model);

            var entityAsDomainComputerUserAccount = entity as DomainComputerUserAccount;

            if (entityAsDomainComputerUserAccount != null)
                return DbDomainComputerUserAccount.ConvertFromDomainComputerUserAccount(entityAsDomainComputerUserAccount, model);

            var entityAsDomainGroupAccount = entity as DomainGroupAccount;

            if (entityAsDomainGroupAccount != null)
                return DbDomainGroupAccount.ConvertFromDomainGroupAccount(entityAsDomainGroupAccount, model);

            var entityAsDomainUserAccount = entity as DomainUserAccount;

            if (entityAsDomainUserAccount != null)
                return DbDomainUserAccount.ConvertFromDomainUserAccount(entityAsDomainUserAccount, model);

            var entityAsLocalComputerAccount = entity as LocalComputerAccount;

            if (entityAsLocalComputerAccount != null)
                return DbLocalComputerAccount.ConvertFromLocalComputerAccount(entityAsLocalComputerAccount, model);

            var entityAsLocalGroupAccount = entity as LocalGroupAccount;

            if (entityAsLocalGroupAccount != null)
                return DbLocalGroupAccount.ConvertFromLocalGroupAccount(entityAsLocalGroupAccount, model);

            var entityAsLocalUserAccount = entity as LocalUserAccount;

            if (entityAsLocalUserAccount != null)
                return DbLocalUserAccount.ConvertFromLocalUserAccount(entityAsLocalUserAccount, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }


        internal static IQueryable<DbAccount> OfRuntimeType(IQueryable<DbAccount> baseQuery, Account entity)
        {
            if(entity is DomainAccount)
                return baseQuery.OfType<DbDomainAccount>();

            if(entity is DomainComputerAccount)
                return baseQuery.OfType<DbDomainComputerAccount>();

            if(entity is DomainComputerGroupAccount)
                return baseQuery.OfType<DbDomainComputerGroupAccount>();

            if(entity is DomainComputerUserAccount)
                return baseQuery.OfType<DbDomainComputerUserAccount>();

            if(entity is DomainGroupAccount)
                return baseQuery.OfType<DbDomainGroupAccount>();

            if(entity is DomainUserAccount)
                return baseQuery.OfType<DbDomainUserAccount>();

            if(entity is LocalComputerAccount)
                return baseQuery.OfType<DbLocalComputerAccount>();

            if(entity is LocalGroupAccount)
                return baseQuery.OfType<DbLocalGroupAccount>();

            if(entity is LocalUserAccount)
                return baseQuery.OfType<DbLocalUserAccount>();


            throw new InvalidOperationException( "Type {0} isn't supported".Combine(entity.GetType()));
        }

        internal Account GetAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetAccount();

                CachedItem = createdItem;
            }

            return (Account)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbAccount"/> to  <see cref="Account"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected Account CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbAccountSecurityIdentifier : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateAccountSecurityIdentifier(AccountSecurityIdentifier entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            Value = entity.Value;

        }

        private String innerValue;

        /// <summary>
        /// Property from <see cref="AccountSecurityIdentifier"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String Value
        {
            get
            {
                return innerValue;
            }
            set
            {
                innerValue = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="AccountSecurityIdentifier"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="AccountSecurityIdentifier"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbAccountSecurityIdentifier ConvertFromAccountSecurityIdentifier(AccountSecurityIdentifier entity, IDatabaseModel model)
        {
            DbAccountSecurityIdentifier cachedResult = model.AccountSecurityIdentifierCache.Get<DbAccountSecurityIdentifier, AccountSecurityIdentifier>(entity, model.AccountSecurityIdentifierSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateAccountSecurityIdentifier(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSecurityIdentifierSet.Create<DbAccountSecurityIdentifier>();
            result.UpdateAccountSecurityIdentifier(entity,model);
            model.AccountSecurityIdentifierSet.Add(result);
            model.AccountSecurityIdentifierCache.Add(entity, result);
            return result;
        }




        internal AccountSecurityIdentifier GetAccountSecurityIdentifier()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetAccountSecurityIdentifier();

                CachedItem = createdItem;
            }

            return (AccountSecurityIdentifier)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbAccountSecurityIdentifier"/> to  <see cref="AccountSecurityIdentifier"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected AccountSecurityIdentifier CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbAgentEncryptionData : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateAgentEncryptionData(AgentEncryptionData entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            Computer = DbBaseComputerAccount.ConvertFromBaseComputerAccount(entity.Computer, model);
            Key = entity.Key;

        }

        private DbBaseComputerAccount innerComputer;

        /// <summary>
        /// Property from <see cref="AgentEncryptionData"/>
        /// </summary>

        public virtual DbBaseComputerAccount Computer
        {
            get
            {
                return innerComputer;
            }
            set
            {
                innerComputer = value;

                CachedItem = null;
            }
        }
        private String innerKey;

        /// <summary>
        /// Property from <see cref="AgentEncryptionData"/>
        /// </summary>

        public virtual String Key
        {
            get
            {
                return innerKey;
            }
            set
            {
                innerKey = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="AgentEncryptionData"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="AgentEncryptionData"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbAgentEncryptionData ConvertFromAgentEncryptionData(AgentEncryptionData entity, IDatabaseModel model)
        {
            DbAgentEncryptionData cachedResult = model.AgentEncryptionDataCache.Get<DbAgentEncryptionData, AgentEncryptionData>(entity, model.AgentEncryptionDataSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateAgentEncryptionData(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AgentEncryptionDataSet.Create<DbAgentEncryptionData>();
            result.UpdateAgentEncryptionData(entity,model);
            model.AgentEncryptionDataSet.Add(result);
            model.AgentEncryptionDataCache.Add(entity, result);
            return result;
        }




        internal AgentEncryptionData GetAgentEncryptionData()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetAgentEncryptionData();

                CachedItem = createdItem;
            }

            return (AgentEncryptionData)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbAgentEncryptionData"/> to  <see cref="AgentEncryptionData"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected AgentEncryptionData CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbBaseDomainAccount : DbAccount
    {
        internal void UpdateBaseDomainAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (BaseDomainAccount)baseEntity;
            UpdateAccount(entity, model);

        }

        internal abstract BaseDomainAccount ForceGetBaseDomainAccount();
        [NotNull]
        internal override Account ForceGetAccount()
        {
            return ForceGetBaseDomainAccount();
        }


        internal static DbBaseDomainAccount ConvertFromBaseDomainAccount(BaseDomainAccount entity, IDatabaseModel model)
        {
            DbBaseDomainAccount cachedResult = model.AccountCache.Get<DbBaseDomainAccount, BaseDomainAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateBaseDomainAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var entityAsDomainAccount = entity as DomainAccount;

            if (entityAsDomainAccount != null)
                return DbDomainAccount.ConvertFromDomainAccount(entityAsDomainAccount, model);

            var entityAsDomainComputerAccount = entity as DomainComputerAccount;

            if (entityAsDomainComputerAccount != null)
                return DbDomainComputerAccount.ConvertFromDomainComputerAccount(entityAsDomainComputerAccount, model);

            var entityAsLocalComputerAccount = entity as LocalComputerAccount;

            if (entityAsLocalComputerAccount != null)
                return DbLocalComputerAccount.ConvertFromLocalComputerAccount(entityAsLocalComputerAccount, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }




        internal BaseDomainAccount GetBaseDomainAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetBaseDomainAccount();

                CachedItem = createdItem;
            }

            return (BaseDomainAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbBaseComputerAccount : DbBaseDomainAccount
    {
        internal void UpdateBaseComputerAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (BaseComputerAccount)baseEntity;
            UpdateBaseDomainAccount(entity, model);

        }

        internal abstract BaseComputerAccount ForceGetBaseComputerAccount();
        [NotNull]
        internal override BaseDomainAccount ForceGetBaseDomainAccount()
        {
            return ForceGetBaseComputerAccount();
        }


        internal static DbBaseComputerAccount ConvertFromBaseComputerAccount(BaseComputerAccount entity, IDatabaseModel model)
        {
            DbBaseComputerAccount cachedResult = model.AccountCache.Get<DbBaseComputerAccount, BaseComputerAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateBaseComputerAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var entityAsDomainComputerAccount = entity as DomainComputerAccount;

            if (entityAsDomainComputerAccount != null)
                return DbDomainComputerAccount.ConvertFromDomainComputerAccount(entityAsDomainComputerAccount, model);

            var entityAsLocalComputerAccount = entity as LocalComputerAccount;

            if (entityAsLocalComputerAccount != null)
                return DbLocalComputerAccount.ConvertFromLocalComputerAccount(entityAsLocalComputerAccount, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }




        internal BaseComputerAccount GetBaseComputerAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetBaseComputerAccount();

                CachedItem = createdItem;
            }

            return (BaseComputerAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbBaseGroupAccount : DbAccount
    {
        internal void UpdateBaseGroupAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (BaseGroupAccount)baseEntity;
            UpdateAccount(entity, model);
            Parent = DbBaseDomainAccount.ConvertFromBaseDomainAccount(entity.Parent, model);

        }

        private DbBaseDomainAccount innerParent;

        /// <summary>
        /// Property from <see cref="BaseGroupAccount"/>
        /// </summary>
        [NotNull]
        public virtual DbBaseDomainAccount Parent
        {
            get
            {
                return innerParent;
            }
            set
            {
                innerParent = value;

                CachedItem = null;
            }
        }
        internal abstract BaseGroupAccount ForceGetBaseGroupAccount();
        [NotNull]
        internal override Account ForceGetAccount()
        {
            return ForceGetBaseGroupAccount();
        }


        internal static DbBaseGroupAccount ConvertFromBaseGroupAccount(BaseGroupAccount entity, IDatabaseModel model)
        {
            DbBaseGroupAccount cachedResult = model.AccountCache.Get<DbBaseGroupAccount, BaseGroupAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateBaseGroupAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var entityAsDomainComputerGroupAccount = entity as DomainComputerGroupAccount;

            if (entityAsDomainComputerGroupAccount != null)
                return DbDomainComputerGroupAccount.ConvertFromDomainComputerGroupAccount(entityAsDomainComputerGroupAccount, model);

            var entityAsDomainGroupAccount = entity as DomainGroupAccount;

            if (entityAsDomainGroupAccount != null)
                return DbDomainGroupAccount.ConvertFromDomainGroupAccount(entityAsDomainGroupAccount, model);

            var entityAsLocalGroupAccount = entity as LocalGroupAccount;

            if (entityAsLocalGroupAccount != null)
                return DbLocalGroupAccount.ConvertFromLocalGroupAccount(entityAsLocalGroupAccount, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }




        internal BaseGroupAccount GetBaseGroupAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetBaseGroupAccount();

                CachedItem = createdItem;
            }

            return (BaseGroupAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbBaseUserAccount : DbAccount
    {
        internal void UpdateBaseUserAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (BaseUserAccount)baseEntity;
            UpdateAccount(entity, model);
            Parent = DbBaseDomainAccount.ConvertFromBaseDomainAccount(entity.Parent, model);

        }

        private DbBaseDomainAccount innerParent;

        /// <summary>
        /// Property from <see cref="BaseUserAccount"/>
        /// </summary>
        [NotNull]
        public virtual DbBaseDomainAccount Parent
        {
            get
            {
                return innerParent;
            }
            set
            {
                innerParent = value;

                CachedItem = null;
            }
        }
        internal abstract BaseUserAccount ForceGetBaseUserAccount();
        [NotNull]
        internal override Account ForceGetAccount()
        {
            return ForceGetBaseUserAccount();
        }


        internal static DbBaseUserAccount ConvertFromBaseUserAccount(BaseUserAccount entity, IDatabaseModel model)
        {
            DbBaseUserAccount cachedResult = model.AccountCache.Get<DbBaseUserAccount, BaseUserAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateBaseUserAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var entityAsDomainComputerUserAccount = entity as DomainComputerUserAccount;

            if (entityAsDomainComputerUserAccount != null)
                return DbDomainComputerUserAccount.ConvertFromDomainComputerUserAccount(entityAsDomainComputerUserAccount, model);

            var entityAsDomainUserAccount = entity as DomainUserAccount;

            if (entityAsDomainUserAccount != null)
                return DbDomainUserAccount.ConvertFromDomainUserAccount(entityAsDomainUserAccount, model);

            var entityAsLocalUserAccount = entity as LocalUserAccount;

            if (entityAsLocalUserAccount != null)
                return DbLocalUserAccount.ConvertFromLocalUserAccount(entityAsLocalUserAccount, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }




        internal BaseUserAccount GetBaseUserAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetBaseUserAccount();

                CachedItem = createdItem;
            }

            return (BaseUserAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbCredentials : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateCredentials(Credentials entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            Domain = DbBaseDomainAccount.ConvertFromBaseDomainAccount(entity.Domain, model);
            User = entity.User;
            Password = entity.Password;

        }

        private DbBaseDomainAccount innerDomain;

        /// <summary>
        /// Property from <see cref="Credentials"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual DbBaseDomainAccount Domain
        {
            get
            {
                return innerDomain;
            }
            set
            {
                innerDomain = value;

                CachedItem = null;
            }
        }
        private String innerUser;

        /// <summary>
        /// Property from <see cref="Credentials"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String User
        {
            get
            {
                return innerUser;
            }
            set
            {
                innerUser = value;

                CachedItem = null;
            }
        }
        private String innerPassword;

        /// <summary>
        /// Property from <see cref="Credentials"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String Password
        {
            get
            {
                return innerPassword;
            }
            set
            {
                innerPassword = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="Credentials"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="Credentials"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbCredentials ConvertFromCredentials(Credentials entity, IDatabaseModel model)
        {
            DbCredentials cachedResult = model.CredentialsCache.Get<DbCredentials, Credentials>(entity, model.CredentialsSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateCredentials(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.CredentialsSet.Create<DbCredentials>();
            result.UpdateCredentials(entity,model);
            model.CredentialsSet.Add(result);
            model.CredentialsCache.Add(entity, result);
            return result;
        }




        internal Credentials GetCredentials()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetCredentials();

                CachedItem = createdItem;
            }

            return (Credentials)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbCredentials"/> to  <see cref="Credentials"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected Credentials CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDomainAccount : DbBaseDomainAccount
    {
        internal void UpdateDomainAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (DomainAccount)baseEntity;
            UpdateBaseDomainAccount(entity, model);

        }

        [NotNull]
        internal override BaseDomainAccount ForceGetBaseDomainAccount()
        {
            return ForceGetDomainAccount();
        }


        internal static DbDomainAccount ConvertFromDomainAccount(DomainAccount entity, IDatabaseModel model)
        {
            DbDomainAccount cachedResult = model.AccountCache.Get<DbDomainAccount, DomainAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDomainAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbDomainAccount>();
            result.UpdateDomainAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal DomainAccount GetDomainAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDomainAccount();

                CachedItem = createdItem;
            }

            return (DomainAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDomainComputerAccount : DbBaseComputerAccount
    {
        internal void UpdateDomainComputerAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (DomainComputerAccount)baseEntity;
            UpdateBaseComputerAccount(entity, model);
            Parent = DbDomainAccount.ConvertFromDomainAccount(entity.Parent, model);

        }

        private DbDomainAccount innerParent;

        /// <summary>
        /// Property from <see cref="DomainComputerAccount"/>
        /// </summary>
        [NotNull]
        public virtual DbDomainAccount Parent
        {
            get
            {
                return innerParent;
            }
            set
            {
                innerParent = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseComputerAccount ForceGetBaseComputerAccount()
        {
            return ForceGetDomainComputerAccount();
        }


        internal static DbDomainComputerAccount ConvertFromDomainComputerAccount(DomainComputerAccount entity, IDatabaseModel model)
        {
            DbDomainComputerAccount cachedResult = model.AccountCache.Get<DbDomainComputerAccount, DomainComputerAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDomainComputerAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbDomainComputerAccount>();
            result.UpdateDomainComputerAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal DomainComputerAccount GetDomainComputerAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDomainComputerAccount();

                CachedItem = createdItem;
            }

            return (DomainComputerAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDomainComputerGroupAccount : DbBaseGroupAccount
    {
        internal void UpdateDomainComputerGroupAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (DomainComputerGroupAccount)baseEntity;
            UpdateBaseGroupAccount(entity, model);

        }

        [NotNull]
        internal override BaseGroupAccount ForceGetBaseGroupAccount()
        {
            return ForceGetDomainComputerGroupAccount();
        }


        internal static DbDomainComputerGroupAccount ConvertFromDomainComputerGroupAccount(DomainComputerGroupAccount entity, IDatabaseModel model)
        {
            DbDomainComputerGroupAccount cachedResult = model.AccountCache.Get<DbDomainComputerGroupAccount, DomainComputerGroupAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDomainComputerGroupAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbDomainComputerGroupAccount>();
            result.UpdateDomainComputerGroupAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal DomainComputerGroupAccount GetDomainComputerGroupAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDomainComputerGroupAccount();

                CachedItem = createdItem;
            }

            return (DomainComputerGroupAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDomainComputerUserAccount : DbBaseUserAccount
    {
        internal void UpdateDomainComputerUserAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (DomainComputerUserAccount)baseEntity;
            UpdateBaseUserAccount(entity, model);

        }

        [NotNull]
        internal override BaseUserAccount ForceGetBaseUserAccount()
        {
            return ForceGetDomainComputerUserAccount();
        }


        internal static DbDomainComputerUserAccount ConvertFromDomainComputerUserAccount(DomainComputerUserAccount entity, IDatabaseModel model)
        {
            DbDomainComputerUserAccount cachedResult = model.AccountCache.Get<DbDomainComputerUserAccount, DomainComputerUserAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDomainComputerUserAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbDomainComputerUserAccount>();
            result.UpdateDomainComputerUserAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal DomainComputerUserAccount GetDomainComputerUserAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDomainComputerUserAccount();

                CachedItem = createdItem;
            }

            return (DomainComputerUserAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDomainGroupAccount : DbBaseGroupAccount
    {
        internal void UpdateDomainGroupAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (DomainGroupAccount)baseEntity;
            UpdateBaseGroupAccount(entity, model);

        }

        [NotNull]
        internal override BaseGroupAccount ForceGetBaseGroupAccount()
        {
            return ForceGetDomainGroupAccount();
        }


        internal static DbDomainGroupAccount ConvertFromDomainGroupAccount(DomainGroupAccount entity, IDatabaseModel model)
        {
            DbDomainGroupAccount cachedResult = model.AccountCache.Get<DbDomainGroupAccount, DomainGroupAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDomainGroupAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbDomainGroupAccount>();
            result.UpdateDomainGroupAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal DomainGroupAccount GetDomainGroupAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDomainGroupAccount();

                CachedItem = createdItem;
            }

            return (DomainGroupAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDomainUserAccount : DbBaseUserAccount
    {
        internal void UpdateDomainUserAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (DomainUserAccount)baseEntity;
            UpdateBaseUserAccount(entity, model);

        }

        [NotNull]
        internal override BaseUserAccount ForceGetBaseUserAccount()
        {
            return ForceGetDomainUserAccount();
        }


        internal static DbDomainUserAccount ConvertFromDomainUserAccount(DomainUserAccount entity, IDatabaseModel model)
        {
            DbDomainUserAccount cachedResult = model.AccountCache.Get<DbDomainUserAccount, DomainUserAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateDomainUserAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbDomainUserAccount>();
            result.UpdateDomainUserAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal DomainUserAccount GetDomainUserAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDomainUserAccount();

                CachedItem = createdItem;
            }

            return (DomainUserAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbLocalComputerAccount : DbBaseComputerAccount
    {
        internal void UpdateLocalComputerAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (LocalComputerAccount)baseEntity;
            UpdateBaseComputerAccount(entity, model);

        }

        [NotNull]
        internal override BaseComputerAccount ForceGetBaseComputerAccount()
        {
            return ForceGetLocalComputerAccount();
        }


        internal static DbLocalComputerAccount ConvertFromLocalComputerAccount(LocalComputerAccount entity, IDatabaseModel model)
        {
            DbLocalComputerAccount cachedResult = model.AccountCache.Get<DbLocalComputerAccount, LocalComputerAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateLocalComputerAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbLocalComputerAccount>();
            result.UpdateLocalComputerAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal LocalComputerAccount GetLocalComputerAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetLocalComputerAccount();

                CachedItem = createdItem;
            }

            return (LocalComputerAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbLocalGroupAccount : DbBaseGroupAccount
    {
        internal void UpdateLocalGroupAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (LocalGroupAccount)baseEntity;
            UpdateBaseGroupAccount(entity, model);

        }

        [NotNull]
        internal override BaseGroupAccount ForceGetBaseGroupAccount()
        {
            return ForceGetLocalGroupAccount();
        }


        internal static DbLocalGroupAccount ConvertFromLocalGroupAccount(LocalGroupAccount entity, IDatabaseModel model)
        {
            DbLocalGroupAccount cachedResult = model.AccountCache.Get<DbLocalGroupAccount, LocalGroupAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateLocalGroupAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbLocalGroupAccount>();
            result.UpdateLocalGroupAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal LocalGroupAccount GetLocalGroupAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetLocalGroupAccount();

                CachedItem = createdItem;
            }

            return (LocalGroupAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbLocalUserAccount : DbBaseUserAccount
    {
        internal void UpdateLocalUserAccount(Account baseEntity, IDatabaseModel model)
        {            
            var entity = (LocalUserAccount)baseEntity;
            UpdateBaseUserAccount(entity, model);

        }

        [NotNull]
        internal override BaseUserAccount ForceGetBaseUserAccount()
        {
            return ForceGetLocalUserAccount();
        }


        internal static DbLocalUserAccount ConvertFromLocalUserAccount(LocalUserAccount entity, IDatabaseModel model)
        {
            DbLocalUserAccount cachedResult = model.AccountCache.Get<DbLocalUserAccount, LocalUserAccount>(entity, model.AccountSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateLocalUserAccount(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.AccountSet.Create<DbLocalUserAccount>();
            result.UpdateLocalUserAccount(entity,model);
            model.AccountSet.Add(result);
            model.AccountCache.Add(entity, result);
            return result;
        }




        internal LocalUserAccount GetLocalUserAccount()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetLocalUserAccount();

                CachedItem = createdItem;
            }

            return (LocalUserAccount)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbOrganizationalUnit : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateOrganizationalUnit(OrganizationalUnit entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            CanonicalName = entity.CanonicalName;
            Parent = DbDomainAccount.ConvertFromDomainAccount(entity.Parent, model);
            ShortName = entity.ShortName;

        }

        private String innerCanonicalName;

        /// <summary>
        /// Property from <see cref="OrganizationalUnit"/>
        /// </summary>
        [Required]
        public virtual String CanonicalName
        {
            get
            {
                return innerCanonicalName;
            }
            set
            {
                innerCanonicalName = value;

                CachedItem = null;
            }
        }
        private DbDomainAccount innerParent;

        /// <summary>
        /// Property from <see cref="OrganizationalUnit"/>
        /// </summary>
        [Required]
        public virtual DbDomainAccount Parent
        {
            get
            {
                return innerParent;
            }
            set
            {
                innerParent = value;

                CachedItem = null;
            }
        }
        private String innerShortName;

        /// <summary>
        /// Property from <see cref="OrganizationalUnit"/>
        /// </summary>
        [Required]
        public virtual String ShortName
        {
            get
            {
                return innerShortName;
            }
            set
            {
                innerShortName = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="OrganizationalUnit"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="OrganizationalUnit"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbOrganizationalUnit ConvertFromOrganizationalUnit(OrganizationalUnit entity, IDatabaseModel model)
        {
            DbOrganizationalUnit cachedResult = model.OrganizationalUnitCache.Get<DbOrganizationalUnit, OrganizationalUnit>(entity, model.OrganizationalUnitSet);
            if( cachedResult != null )
            {
                if( !cachedResult.WasUpdated )
                {
                    cachedResult.UpdateOrganizationalUnit(entity, model);

                    var entityWrapper = model.Entry(cachedResult);

                    if (entityWrapper.State != System.Data.EntityState.Added)
                        entityWrapper.State = System.Data.EntityState.Modified;
                }

                return cachedResult;
            }

            var result = model.OrganizationalUnitSet.Create<DbOrganizationalUnit>();
            result.UpdateOrganizationalUnit(entity,model);
            model.OrganizationalUnitSet.Add(result);
            model.OrganizationalUnitCache.Add(entity, result);
            return result;
        }




        internal OrganizationalUnit GetOrganizationalUnit()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetOrganizationalUnit();

                CachedItem = createdItem;
            }

            return (OrganizationalUnit)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbOrganizationalUnit"/> to  <see cref="OrganizationalUnit"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected OrganizationalUnit CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbProgramConfiguration : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateProgramConfiguration(ProgramConfiguration entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            ConfigurationVersion = entity.ConfigurationVersion;
            Rules = entity.Rules.Select( child=> DbRule.ConvertFromRule(child, model)).ToList();
            TemporaryAccess = entity.TemporaryAccess.Select( child=> DbBaseTemporaryAccessCondition.ConvertFromBaseTemporaryAccessCondition(child, model)).ToList();

        }

        private Int32 innerConfigurationVersion;

        /// <summary>
        /// Property from <see cref="ProgramConfiguration"/>
        /// </summary>
        [Required]
        public virtual Int32 ConfigurationVersion
        {
            get
            {
                return innerConfigurationVersion;
            }
            set
            {
                innerConfigurationVersion = value;

                CachedItem = null;
            }
        }
        private List<DbRule> innerRules;

        /// <summary>
        /// Property from <see cref="ProgramConfiguration"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbRule> Rules
        {
            get
            {
                return innerRules;
            }
            set
            {
                innerRules = value;

                CachedItem = null;
            }
        }
        private List<DbBaseTemporaryAccessCondition> innerTemporaryAccess;

        /// <summary>
        /// Property from <see cref="ProgramConfiguration"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbBaseTemporaryAccessCondition> TemporaryAccess
        {
            get
            {
                return innerTemporaryAccess;
            }
            set
            {
                innerTemporaryAccess = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="ProgramConfiguration"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="ProgramConfiguration"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbProgramConfiguration ConvertFromProgramConfiguration(ProgramConfiguration entity, IDatabaseModel model)
        {
            var result = model.ProgramConfigurationSet.Create<DbProgramConfiguration>();
            result.UpdateProgramConfiguration(entity,model);
            model.ProgramConfigurationSet.Add(result);

            return result;
        }




        internal ProgramConfiguration GetProgramConfiguration()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetProgramConfiguration();

                CachedItem = createdItem;
            }

            return (ProgramConfiguration)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbProgramConfiguration"/> to  <see cref="ProgramConfiguration"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected ProgramConfiguration CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbActionData : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateActionData(ActionData entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            BlockAction = entity.BlockAction;
            AuditAction = entity.AuditAction;

        }


        /// <summary>
        /// Inner property for database
        /// </summary>
        [Required]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public virtual int BlockActionIntValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="ActionData"/>
        /// </summary>
        internal BlockActionType BlockAction
        {
            get
            {
                var result = (BlockActionType)BlockActionIntValue;

                Check.EnumerationValueIsDefined((BlockActionType)BlockActionIntValue);

                return result;
            }
            set
            {
                Check.EnumerationValueIsDefined(value, "value");

                BlockActionIntValue = (int)value;
           
                CachedItem = null;
            }
        }

        /// <summary>
        /// Inner property for database
        /// </summary>
        [Required]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public virtual int AuditActionIntValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="ActionData"/>
        /// </summary>
        internal AuditActionType AuditAction
        {
            get
            {
                var result = (AuditActionType)AuditActionIntValue;

                Check.EnumerationValueIsDefined((AuditActionType)AuditActionIntValue);

                return result;
            }
            set
            {
                Check.EnumerationValueIsDefined(value, "value");

                AuditActionIntValue = (int)value;
           
                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="ActionData"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="ActionData"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbActionData ConvertFromActionData(ActionData entity, IDatabaseModel model)
        {
            var result = model.ActionDataSet.Create<DbActionData>();
            result.UpdateActionData(entity,model);
            model.ActionDataSet.Add(result);

            return result;
        }




        internal ActionData GetActionData()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetActionData();

                CachedItem = createdItem;
            }

            return (ActionData)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbActionData"/> to  <see cref="ActionData"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected ActionData CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbCompositeRuleCondition : DbBaseRuleCondition
    {
        internal void UpdateCompositeRuleCondition(BaseRuleCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (CompositeRuleCondition)baseEntity;
            UpdateBaseRuleCondition(entity, model);
            OperationType = entity.OperationType;
            Conditions = entity.Conditions.Select( child=> DbBaseRuleCondition.ConvertFromBaseRuleCondition(child, model)).ToList();

        }


        /// <summary>
        /// Inner property for database
        /// </summary>
        [Required]
        [SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int")]
        public virtual int OperationTypeIntValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="CompositeRuleCondition"/>
        /// </summary>
        internal CompositeRuleConditionType OperationType
        {
            get
            {
                var result = (CompositeRuleConditionType)OperationTypeIntValue;

                Check.EnumerationValueIsDefined((CompositeRuleConditionType)OperationTypeIntValue);

                return result;
            }
            set
            {
                Check.EnumerationValueIsDefined(value, "value");

                OperationTypeIntValue = (int)value;
           
                CachedItem = null;
            }
        }
        private List<DbBaseRuleCondition> innerConditions;

        /// <summary>
        /// Property from <see cref="CompositeRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbBaseRuleCondition> Conditions
        {
            get
            {
                return innerConditions;
            }
            set
            {
                innerConditions = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseRuleCondition ForceGetBaseRuleCondition()
        {
            return ForceGetCompositeRuleCondition();
        }


        internal static DbCompositeRuleCondition ConvertFromCompositeRuleCondition(CompositeRuleCondition entity, IDatabaseModel model)
        {
            var result = model.BaseRuleConditionSet.Create<DbCompositeRuleCondition>();
            result.UpdateCompositeRuleCondition(entity,model);
            model.BaseRuleConditionSet.Add(result);

            return result;
        }




        internal CompositeRuleCondition GetCompositeRuleCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetCompositeRuleCondition();

                CachedItem = createdItem;
            }

            return (CompositeRuleCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbComputerListRuleCondition : DbBaseRuleCondition
    {
        internal void UpdateComputerListRuleCondition(BaseRuleCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (ComputerListRuleCondition)baseEntity;
            UpdateBaseRuleCondition(entity, model);
            Domains = entity.Domains.Select( child=> DbDomainAccount.ConvertFromDomainAccount(child, model)).ToList();
            OrganizationalUnits = entity.OrganizationalUnits.Select( child=> DbOrganizationalUnit.ConvertFromOrganizationalUnit(child, model)).ToList();
            Groups = entity.Groups.Select( child=> DbDomainGroupAccount.ConvertFromDomainGroupAccount(child, model)).ToList();
            Computers = entity.Computers.Select( child=> DbBaseComputerAccount.ConvertFromBaseComputerAccount(child, model)).ToList();

        }

        private List<DbDomainAccount> innerDomains;

        /// <summary>
        /// Property from <see cref="ComputerListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbDomainAccount> Domains
        {
            get
            {
                return innerDomains;
            }
            set
            {
                innerDomains = value;

                CachedItem = null;
            }
        }
        private List<DbOrganizationalUnit> innerOrganizationalUnits;

        /// <summary>
        /// Property from <see cref="ComputerListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbOrganizationalUnit> OrganizationalUnits
        {
            get
            {
                return innerOrganizationalUnits;
            }
            set
            {
                innerOrganizationalUnits = value;

                CachedItem = null;
            }
        }
        private List<DbDomainGroupAccount> innerGroups;

        /// <summary>
        /// Property from <see cref="ComputerListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbDomainGroupAccount> Groups
        {
            get
            {
                return innerGroups;
            }
            set
            {
                innerGroups = value;

                CachedItem = null;
            }
        }
        private List<DbBaseComputerAccount> innerComputers;

        /// <summary>
        /// Property from <see cref="ComputerListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbBaseComputerAccount> Computers
        {
            get
            {
                return innerComputers;
            }
            set
            {
                innerComputers = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseRuleCondition ForceGetBaseRuleCondition()
        {
            return ForceGetComputerListRuleCondition();
        }


        internal static DbComputerListRuleCondition ConvertFromComputerListRuleCondition(ComputerListRuleCondition entity, IDatabaseModel model)
        {
            var result = model.BaseRuleConditionSet.Create<DbComputerListRuleCondition>();
            result.UpdateComputerListRuleCondition(entity,model);
            model.BaseRuleConditionSet.Add(result);

            return result;
        }




        internal ComputerListRuleCondition GetComputerListRuleCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetComputerListRuleCondition();

                CachedItem = createdItem;
            }

            return (ComputerListRuleCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDeviceListRuleCondition : DbBaseRuleCondition
    {
        internal void UpdateDeviceListRuleCondition(BaseRuleCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (DeviceListRuleCondition)baseEntity;
            UpdateBaseRuleCondition(entity, model);
            Devices = entity.Devices.Select( child=> DbDeviceDescription.ConvertFromDeviceDescription(child, model)).ToList();

        }

        private List<DbDeviceDescription> innerDevices;

        /// <summary>
        /// Property from <see cref="DeviceListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbDeviceDescription> Devices
        {
            get
            {
                return innerDevices;
            }
            set
            {
                innerDevices = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseRuleCondition ForceGetBaseRuleCondition()
        {
            return ForceGetDeviceListRuleCondition();
        }


        internal static DbDeviceListRuleCondition ConvertFromDeviceListRuleCondition(DeviceListRuleCondition entity, IDatabaseModel model)
        {
            var result = model.BaseRuleConditionSet.Create<DbDeviceListRuleCondition>();
            result.UpdateDeviceListRuleCondition(entity,model);
            model.BaseRuleConditionSet.Add(result);

            return result;
        }




        internal DeviceListRuleCondition GetDeviceListRuleCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDeviceListRuleCondition();

                CachedItem = createdItem;
            }

            return (DeviceListRuleCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public abstract partial class DbBaseTemporaryAccessCondition : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateBaseTemporaryAccessCondition(BaseTemporaryAccessCondition entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            EndTime = entity.EndTime;
            ReadOnlyAccess = entity.ReadOnlyAccess;

        }


        /// <summary>
        /// Inner property for time representation in database
        /// </summary>
        [Required]
        public virtual long EndTimeTicks
        {
            get;
            set;
        }
        
        /// <summary>
        /// Property from <see cref="BaseTemporaryAccessCondition"/>
        /// </summary>
        [NotMapped]
        internal Time EndTime
        {
            get
            {
                return new Time(EndTimeTicks);
            }
            set
            {
                Check.ObjectIsNotNull(value, "value");

                EndTimeTicks = value.Ticks;
           
                CachedItem = null;
            }
        }
        private Boolean innerReadOnlyAccess;

        /// <summary>
        /// Property from <see cref="BaseTemporaryAccessCondition"/>
        /// </summary>
        [Required]
        public virtual Boolean ReadOnlyAccess
        {
            get
            {
                return innerReadOnlyAccess;
            }
            set
            {
                innerReadOnlyAccess = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="BaseTemporaryAccessCondition"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="BaseTemporaryAccessCondition"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }
        internal abstract BaseTemporaryAccessCondition ForceGetBaseTemporaryAccessCondition();


        internal static DbBaseTemporaryAccessCondition ConvertFromBaseTemporaryAccessCondition(BaseTemporaryAccessCondition entity, IDatabaseModel model)
        {
            var entityAsComputerTemporaryAccessCondition = entity as ComputerTemporaryAccessCondition;

            if (entityAsComputerTemporaryAccessCondition != null)
                return DbComputerTemporaryAccessCondition.ConvertFromComputerTemporaryAccessCondition(entityAsComputerTemporaryAccessCondition, model);

            var entityAsDeviceTemporaryAccessCondition = entity as DeviceTemporaryAccessCondition;

            if (entityAsDeviceTemporaryAccessCondition != null)
                return DbDeviceTemporaryAccessCondition.ConvertFromDeviceTemporaryAccessCondition(entityAsDeviceTemporaryAccessCondition, model);

            var entityAsUserTemporaryAccessCondition = entity as UserTemporaryAccessCondition;

            if (entityAsUserTemporaryAccessCondition != null)
                return DbUserTemporaryAccessCondition.ConvertFromUserTemporaryAccessCondition(entityAsUserTemporaryAccessCondition, model);

            throw new InvalidOperationException("Unable to convert entity {0}".Combine(entity));
        }


        internal static IQueryable<DbBaseTemporaryAccessCondition> OfRuntimeType(IQueryable<DbBaseTemporaryAccessCondition> baseQuery, BaseTemporaryAccessCondition entity)
        {
            if(entity is ComputerTemporaryAccessCondition)
                return baseQuery.OfType<DbComputerTemporaryAccessCondition>();

            if(entity is DeviceTemporaryAccessCondition)
                return baseQuery.OfType<DbDeviceTemporaryAccessCondition>();

            if(entity is UserTemporaryAccessCondition)
                return baseQuery.OfType<DbUserTemporaryAccessCondition>();


            throw new InvalidOperationException( "Type {0} isn't supported".Combine(entity.GetType()));
        }

        internal BaseTemporaryAccessCondition GetBaseTemporaryAccessCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetBaseTemporaryAccessCondition();

                CachedItem = createdItem;
            }

            return (BaseTemporaryAccessCondition)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbBaseTemporaryAccessCondition"/> to  <see cref="BaseTemporaryAccessCondition"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected BaseTemporaryAccessCondition CachedItem
        {
            get;
            set;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbComputerTemporaryAccessCondition : DbBaseTemporaryAccessCondition
    {
        internal void UpdateComputerTemporaryAccessCondition(BaseTemporaryAccessCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (ComputerTemporaryAccessCondition)baseEntity;
            UpdateBaseTemporaryAccessCondition(entity, model);
            Computer = DbBaseComputerAccount.ConvertFromBaseComputerAccount(entity.Computer, model);

        }

        private DbBaseComputerAccount innerComputer;

        /// <summary>
        /// Property from <see cref="ComputerTemporaryAccessCondition"/>
        /// </summary>
        [Required]
        public virtual DbBaseComputerAccount Computer
        {
            get
            {
                return innerComputer;
            }
            set
            {
                innerComputer = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseTemporaryAccessCondition ForceGetBaseTemporaryAccessCondition()
        {
            return ForceGetComputerTemporaryAccessCondition();
        }


        internal static DbComputerTemporaryAccessCondition ConvertFromComputerTemporaryAccessCondition(ComputerTemporaryAccessCondition entity, IDatabaseModel model)
        {
            var result = model.BaseTemporaryAccessConditionSet.Create<DbComputerTemporaryAccessCondition>();
            result.UpdateComputerTemporaryAccessCondition(entity,model);
            model.BaseTemporaryAccessConditionSet.Add(result);

            return result;
        }




        internal ComputerTemporaryAccessCondition GetComputerTemporaryAccessCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetComputerTemporaryAccessCondition();

                CachedItem = createdItem;
            }

            return (ComputerTemporaryAccessCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbDeviceTemporaryAccessCondition : DbBaseTemporaryAccessCondition
    {
        internal void UpdateDeviceTemporaryAccessCondition(BaseTemporaryAccessCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (DeviceTemporaryAccessCondition)baseEntity;
            UpdateBaseTemporaryAccessCondition(entity, model);
            Device = DbDeviceDescription.ConvertFromDeviceDescription(entity.Device, model);

        }

        private DbDeviceDescription innerDevice;

        /// <summary>
        /// Property from <see cref="DeviceTemporaryAccessCondition"/>
        /// </summary>
        [Required]
        public virtual DbDeviceDescription Device
        {
            get
            {
                return innerDevice;
            }
            set
            {
                innerDevice = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseTemporaryAccessCondition ForceGetBaseTemporaryAccessCondition()
        {
            return ForceGetDeviceTemporaryAccessCondition();
        }


        internal static DbDeviceTemporaryAccessCondition ConvertFromDeviceTemporaryAccessCondition(DeviceTemporaryAccessCondition entity, IDatabaseModel model)
        {
            var result = model.BaseTemporaryAccessConditionSet.Create<DbDeviceTemporaryAccessCondition>();
            result.UpdateDeviceTemporaryAccessCondition(entity,model);
            model.BaseTemporaryAccessConditionSet.Add(result);

            return result;
        }




        internal DeviceTemporaryAccessCondition GetDeviceTemporaryAccessCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetDeviceTemporaryAccessCondition();

                CachedItem = createdItem;
            }

            return (DeviceTemporaryAccessCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbUserTemporaryAccessCondition : DbBaseTemporaryAccessCondition
    {
        internal void UpdateUserTemporaryAccessCondition(BaseTemporaryAccessCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (UserTemporaryAccessCondition)baseEntity;
            UpdateBaseTemporaryAccessCondition(entity, model);
            User = DbBaseUserAccount.ConvertFromBaseUserAccount(entity.User, model);

        }

        private DbBaseUserAccount innerUser;

        /// <summary>
        /// Property from <see cref="UserTemporaryAccessCondition"/>
        /// </summary>
        [Required]
        public virtual DbBaseUserAccount User
        {
            get
            {
                return innerUser;
            }
            set
            {
                innerUser = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseTemporaryAccessCondition ForceGetBaseTemporaryAccessCondition()
        {
            return ForceGetUserTemporaryAccessCondition();
        }


        internal static DbUserTemporaryAccessCondition ConvertFromUserTemporaryAccessCondition(UserTemporaryAccessCondition entity, IDatabaseModel model)
        {
            var result = model.BaseTemporaryAccessConditionSet.Create<DbUserTemporaryAccessCondition>();
            result.UpdateUserTemporaryAccessCondition(entity,model);
            model.BaseTemporaryAccessConditionSet.Add(result);

            return result;
        }




        internal UserTemporaryAccessCondition GetUserTemporaryAccessCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetUserTemporaryAccessCondition();

                CachedItem = createdItem;
            }

            return (UserTemporaryAccessCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbUserListRuleCondition : DbBaseRuleCondition
    {
        internal void UpdateUserListRuleCondition(BaseRuleCondition baseEntity, IDatabaseModel model)
        {            
            var entity = (UserListRuleCondition)baseEntity;
            UpdateBaseRuleCondition(entity, model);
            Domains = entity.Domains.Select( child=> DbDomainAccount.ConvertFromDomainAccount(child, model)).ToList();
            OrganizationalUnits = entity.OrganizationalUnits.Select( child=> DbOrganizationalUnit.ConvertFromOrganizationalUnit(child, model)).ToList();
            Groups = entity.Groups.Select( child=> DbBaseGroupAccount.ConvertFromBaseGroupAccount(child, model)).ToList();
            Users = entity.Users.Select( child=> DbBaseUserAccount.ConvertFromBaseUserAccount(child, model)).ToList();

        }

        private List<DbDomainAccount> innerDomains;

        /// <summary>
        /// Property from <see cref="UserListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbDomainAccount> Domains
        {
            get
            {
                return innerDomains;
            }
            set
            {
                innerDomains = value;

                CachedItem = null;
            }
        }
        private List<DbOrganizationalUnit> innerOrganizationalUnits;

        /// <summary>
        /// Property from <see cref="UserListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbOrganizationalUnit> OrganizationalUnits
        {
            get
            {
                return innerOrganizationalUnits;
            }
            set
            {
                innerOrganizationalUnits = value;

                CachedItem = null;
            }
        }
        private List<DbBaseGroupAccount> innerGroups;

        /// <summary>
        /// Property from <see cref="UserListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbBaseGroupAccount> Groups
        {
            get
            {
                return innerGroups;
            }
            set
            {
                innerGroups = value;

                CachedItem = null;
            }
        }
        private List<DbBaseUserAccount> innerUsers;

        /// <summary>
        /// Property from <see cref="UserListRuleCondition"/>
        /// </summary>
        [NotNull]
        [Required]
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual List<DbBaseUserAccount> Users
        {
            get
            {
                return innerUsers;
            }
            set
            {
                innerUsers = value;

                CachedItem = null;
            }
        }
        [NotNull]
        internal override BaseRuleCondition ForceGetBaseRuleCondition()
        {
            return ForceGetUserListRuleCondition();
        }


        internal static DbUserListRuleCondition ConvertFromUserListRuleCondition(UserListRuleCondition entity, IDatabaseModel model)
        {
            var result = model.BaseRuleConditionSet.Create<DbUserListRuleCondition>();
            result.UpdateUserListRuleCondition(entity,model);
            model.BaseRuleConditionSet.Add(result);

            return result;
        }




        internal UserListRuleCondition GetUserListRuleCondition()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetUserListRuleCondition();

                CachedItem = createdItem;
            }

            return (UserListRuleCondition)CachedItem;
        }

    }

    /// <summary>
    /// Type for properly database working
    /// </summary>
    public partial class DbRule : IKeyedItem
    {
         [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "model")]
        internal void UpdateRule(Rule entity, IDatabaseModel model)
        {
            Check.ObjectIsNotNull(entity, "entity");

            WasUpdated = true;
            Actions = DbActionData.ConvertFromActionData(entity.Actions, model);
            Name = entity.Name;
            RootCondition = DbBaseRuleCondition.ConvertFromBaseRuleCondition(entity.RootCondition, model);
            Priority = entity.Priority;

        }

        private DbActionData innerActions;

        /// <summary>
        /// Property from <see cref="Rule"/>
        /// </summary>
        [Required]
        public virtual DbActionData Actions
        {
            get
            {
                return innerActions;
            }
            set
            {
                innerActions = value;

                CachedItem = null;
            }
        }
        private String innerName;

        /// <summary>
        /// Property from <see cref="Rule"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual String Name
        {
            get
            {
                return innerName;
            }
            set
            {
                innerName = value;

                CachedItem = null;
            }
        }
        private DbBaseRuleCondition innerRootCondition;

        /// <summary>
        /// Property from <see cref="Rule"/>
        /// </summary>
        [NotNull]
        [Required]
        public virtual DbBaseRuleCondition RootCondition
        {
            get
            {
                return innerRootCondition;
            }
            set
            {
                innerRootCondition = value;

                CachedItem = null;
            }
        }
        private Int32 innerPriority;

        /// <summary>
        /// Property from <see cref="Rule"/>
        /// </summary>
        [Required]
        public virtual Int32 Priority
        {
            get
            {
                return innerPriority;
            }
            set
            {
                innerPriority = value;

                CachedItem = null;
            }
        }
        private int innerId;

        /// <summary>
        /// Property from <see cref="Rule"/>
        /// </summary>
        [Key,Required,SuppressMessage("Microsoft.Security", "CA2119:SealMethodsThatSatisfyPrivateInterfaces")]
        public virtual int Id
        {
            get
            {
                return innerId;
            }
            set
            {
                innerId = value;

                CachedItem = null;
            }
        }
        /// <summary>
        /// Property from <see cref="Rule"/>
        /// </summary>
        [NotMapped]
        protected bool WasUpdated
        {
            get;
            set;
        }


        internal static DbRule ConvertFromRule(Rule entity, IDatabaseModel model)
        {
            var result = model.RuleSet.Create<DbRule>();
            result.UpdateRule(entity,model);
            model.RuleSet.Add(result);

            return result;
        }




        internal Rule GetRule()
        {
            if( CachedItem == null )
            {
                var createdItem = ForceGetRule();

                CachedItem = createdItem;
            }

            return (Rule)CachedItem;
        }
        /// <summary>
        /// Object using to cache items converted from <see cref="DbRule"/> to  <see cref="Rule"/>
        /// </summary>
        [NotMapped]
        [CanBeNull]
        protected Rule CachedItem
        {
            get;
            set;
        }

    }

// ReSharper restore ClassNeverInstantiated.Global
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore VirtualMemberNeverOverriden.Global
// ReSharper restore MemberCanBeProtected.Global
// ReSharper restore ClassCanBeSealed.Global
// ReSharper restore ClassWithVirtualMembersNeverInherited.Global
// ReSharper restore PublicConstructorInAbstractClass.Global
// ReSharper restore EmptyConstructor.Global
// ReSharper restore PartialTypeWithSinglePart
// ReSharper restore UnusedAutoPropertyAccessor.Global
// ReSharper restore RedundantCast
// ReSharper restore MemberCanBeInternal
// ReSharper restore UnusedParameter.Global
// ReSharper restore RedundantNameQualifier
// ReSharper restore AnnotationRedundanceAtValueType
// ReSharper restore UnusedMember.Global
}
