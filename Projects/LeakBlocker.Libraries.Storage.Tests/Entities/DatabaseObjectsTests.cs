using Microsoft.VisualStudio.TestTools.UnitTesting;
using LeakBlocker.Libraries.Storage.Entities;
using LeakBlocker.Libraries.Storage.InternalTools;

using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Audit;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities;

namespace LeakBlocker.Libraries.Storage.Tests.Entities
{

    [TestClass]
    public sealed class DatabaseObjectsTests : BaseDatabaseObjectsTests
    {
        // ReSharper disable RedundantBaseQualifier

        #region Database insert/select tests

        
        [TestMethod]
        public void BaseRuleConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseRuleConditionSet,
                    BaseRuleConditionTest.objects,
                    DbBaseRuleCondition.ConvertFromBaseRuleCondition,
                    dbEntity => dbEntity.GetBaseRuleCondition());
        }

        [TestMethod]
        public void DeviceTypeRuleConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseRuleConditionSet,
                    DeviceTypeRuleConditionTest.objects,
                    DbDeviceTypeRuleCondition.ConvertFromDeviceTypeRuleCondition,
                    dbEntity => dbEntity.GetDeviceTypeRuleCondition());
        }

        [TestMethod]
        public void AuditItemInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AuditItemSet,
                    AuditItemTest.objects,
                    DbAuditItem.ConvertFromAuditItem,
                    dbEntity => dbEntity.GetAuditItem());
        }

        [TestMethod]
        public void DeviceDescriptionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.DeviceDescriptionSet,
                    DeviceDescriptionTest.objects,
                    DbDeviceDescription.ConvertFromDeviceDescription,
                    dbEntity => dbEntity.GetDeviceDescription());
        }

        [TestMethod]
        public void AccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    AccountTest.objects,
                    DbAccount.ConvertFromAccount,
                    dbEntity => dbEntity.GetAccount());
        }

        [TestMethod]
        public void AccountSecurityIdentifierInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSecurityIdentifierSet,
                    AccountSecurityIdentifierTest.objects,
                    DbAccountSecurityIdentifier.ConvertFromAccountSecurityIdentifier,
                    dbEntity => dbEntity.GetAccountSecurityIdentifier());
        }

        [TestMethod]
        public void AgentEncryptionDataInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AgentEncryptionDataSet,
                    AgentEncryptionDataTest.objects,
                    DbAgentEncryptionData.ConvertFromAgentEncryptionData,
                    dbEntity => dbEntity.GetAgentEncryptionData());
        }

        [TestMethod]
        public void BaseDomainAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    BaseDomainAccountTest.objects,
                    DbBaseDomainAccount.ConvertFromBaseDomainAccount,
                    dbEntity => dbEntity.GetBaseDomainAccount());
        }

        [TestMethod]
        public void BaseComputerAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    BaseComputerAccountTest.objects,
                    DbBaseComputerAccount.ConvertFromBaseComputerAccount,
                    dbEntity => dbEntity.GetBaseComputerAccount());
        }

        [TestMethod]
        public void BaseGroupAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    BaseGroupAccountTest.objects,
                    DbBaseGroupAccount.ConvertFromBaseGroupAccount,
                    dbEntity => dbEntity.GetBaseGroupAccount());
        }

        [TestMethod]
        public void BaseUserAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    BaseUserAccountTest.objects,
                    DbBaseUserAccount.ConvertFromBaseUserAccount,
                    dbEntity => dbEntity.GetBaseUserAccount());
        }

        [TestMethod]
        public void CredentialsInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.CredentialsSet,
                    CredentialsTest.objects,
                    DbCredentials.ConvertFromCredentials,
                    dbEntity => dbEntity.GetCredentials());
        }

        [TestMethod]
        public void DomainAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    DomainAccountTest.objects,
                    DbDomainAccount.ConvertFromDomainAccount,
                    dbEntity => dbEntity.GetDomainAccount());
        }

        [TestMethod]
        public void DomainComputerAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    DomainComputerAccountTest.objects,
                    DbDomainComputerAccount.ConvertFromDomainComputerAccount,
                    dbEntity => dbEntity.GetDomainComputerAccount());
        }

        [TestMethod]
        public void DomainComputerGroupAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    DomainComputerGroupAccountTest.objects,
                    DbDomainComputerGroupAccount.ConvertFromDomainComputerGroupAccount,
                    dbEntity => dbEntity.GetDomainComputerGroupAccount());
        }

        [TestMethod]
        public void DomainComputerUserAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    DomainComputerUserAccountTest.objects,
                    DbDomainComputerUserAccount.ConvertFromDomainComputerUserAccount,
                    dbEntity => dbEntity.GetDomainComputerUserAccount());
        }

        [TestMethod]
        public void DomainGroupAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    DomainGroupAccountTest.objects,
                    DbDomainGroupAccount.ConvertFromDomainGroupAccount,
                    dbEntity => dbEntity.GetDomainGroupAccount());
        }

        [TestMethod]
        public void DomainUserAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    DomainUserAccountTest.objects,
                    DbDomainUserAccount.ConvertFromDomainUserAccount,
                    dbEntity => dbEntity.GetDomainUserAccount());
        }

        [TestMethod]
        public void LocalComputerAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    LocalComputerAccountTest.objects,
                    DbLocalComputerAccount.ConvertFromLocalComputerAccount,
                    dbEntity => dbEntity.GetLocalComputerAccount());
        }

        [TestMethod]
        public void LocalGroupAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    LocalGroupAccountTest.objects,
                    DbLocalGroupAccount.ConvertFromLocalGroupAccount,
                    dbEntity => dbEntity.GetLocalGroupAccount());
        }

        [TestMethod]
        public void LocalUserAccountInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.AccountSet,
                    LocalUserAccountTest.objects,
                    DbLocalUserAccount.ConvertFromLocalUserAccount,
                    dbEntity => dbEntity.GetLocalUserAccount());
        }

        [TestMethod]
        public void OrganizationalUnitInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.OrganizationalUnitSet,
                    OrganizationalUnitTest.objects,
                    DbOrganizationalUnit.ConvertFromOrganizationalUnit,
                    dbEntity => dbEntity.GetOrganizationalUnit());
        }

        [TestMethod]
        public void ProgramConfigurationInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.ProgramConfigurationSet,
                    ProgramConfigurationTest.objects,
                    DbProgramConfiguration.ConvertFromProgramConfiguration,
                    dbEntity => dbEntity.GetProgramConfiguration());
        }

        [TestMethod]
        public void ActionDataInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.ActionDataSet,
                    ActionDataTest.objects,
                    DbActionData.ConvertFromActionData,
                    dbEntity => dbEntity.GetActionData());
        }

        [TestMethod]
        public void CompositeRuleConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseRuleConditionSet,
                    CompositeRuleConditionTest.objects,
                    DbCompositeRuleCondition.ConvertFromCompositeRuleCondition,
                    dbEntity => dbEntity.GetCompositeRuleCondition());
        }

        [TestMethod]
        public void ComputerListRuleConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseRuleConditionSet,
                    ComputerListRuleConditionTest.objects,
                    DbComputerListRuleCondition.ConvertFromComputerListRuleCondition,
                    dbEntity => dbEntity.GetComputerListRuleCondition());
        }

        [TestMethod]
        public void DeviceListRuleConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseRuleConditionSet,
                    DeviceListRuleConditionTest.objects,
                    DbDeviceListRuleCondition.ConvertFromDeviceListRuleCondition,
                    dbEntity => dbEntity.GetDeviceListRuleCondition());
        }

        [TestMethod]
        public void BaseTemporaryAccessConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseTemporaryAccessConditionSet,
                    BaseTemporaryAccessConditionTest.objects,
                    DbBaseTemporaryAccessCondition.ConvertFromBaseTemporaryAccessCondition,
                    dbEntity => dbEntity.GetBaseTemporaryAccessCondition());
        }

        [TestMethod]
        public void ComputerTemporaryAccessConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseTemporaryAccessConditionSet,
                    ComputerTemporaryAccessConditionTest.objects,
                    DbComputerTemporaryAccessCondition.ConvertFromComputerTemporaryAccessCondition,
                    dbEntity => dbEntity.GetComputerTemporaryAccessCondition());
        }

        [TestMethod]
        public void DeviceTemporaryAccessConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseTemporaryAccessConditionSet,
                    DeviceTemporaryAccessConditionTest.objects,
                    DbDeviceTemporaryAccessCondition.ConvertFromDeviceTemporaryAccessCondition,
                    dbEntity => dbEntity.GetDeviceTemporaryAccessCondition());
        }

        [TestMethod]
        public void UserTemporaryAccessConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseTemporaryAccessConditionSet,
                    UserTemporaryAccessConditionTest.objects,
                    DbUserTemporaryAccessCondition.ConvertFromUserTemporaryAccessCondition,
                    dbEntity => dbEntity.GetUserTemporaryAccessCondition());
        }

        [TestMethod]
        public void UserListRuleConditionInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.BaseRuleConditionSet,
                    UserListRuleConditionTest.objects,
                    DbUserListRuleCondition.ConvertFromUserListRuleCondition,
                    dbEntity => dbEntity.GetUserListRuleCondition());
        }

        [TestMethod]
        public void RuleInsertSelect()
        {
            base.TestForInsertAndSelect(
                    model => model.RuleSet,
                    RuleTest.objects,
                    DbRule.ConvertFromRule,
                    dbEntity => dbEntity.GetRule());
        }


        #endregion Database insert/select tests

        // ReSharper restore RedundantBaseQualifier
    }
}

