using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.ServerShared.AgentCommunication
{
    /// <summary>
    /// Agent installer return code.
    /// </summary>
    [EnumerationDescriptionProvider(typeof(AgentInstallerStrings))]
    public enum AgentInstallerStatus : int
    {
        /// <summary>
        /// Installer completed successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// Previous version was not correctly installed. Missing data from private storage.
        /// </summary>
        PreviousVersionWasNotCorrectlyInstalled = 1,

        /// <summary>
        /// Agent is not installed.
        /// </summary>
        AgentIsNotInstalled = 3,

        /// <summary>
        /// Same version is already installed.
        /// </summary>
        SameVersionAlreadyInstalled = 4,

        /// <summary>
        /// Current version is not installed.
        /// </summary>
        CurrentVersionIsNotInstalled = 5,

        /// <summary>
        /// Wrong password.
        /// </summary>
        IncorrectPassword = 6,

        /// <summary>
        /// Agent is already installed (same or another version).
        /// </summary>
        AgentAlreadyInstalled = 7,

        /// <summary>
        /// Verification key is incorrect.
        /// </summary>
        IncorrectVerificationKey = 8,

        /// <summary>
        /// Secret key is incorrect.
        /// </summary>
        SecretKeyIsIncorrect = 9,

        /// <summary>
        /// Wrong arguments.
        /// </summary>
        WrongArguments = 10,

        /// <summary>
        /// Unknown error.
        /// </summary>
        InternalFailure = 11
    }
}
