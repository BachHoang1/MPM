using System;

namespace RoundLAB.eddi.Classes.Enums
{
    [Flags]
    public enum MSASurveyAdminState
    {
        Unchanged = -1, 
        /// <summary>
        /// the Survey has been submitted but has not yet been approved or corrected
        /// </summary>
        Pending = 0,

        /// <summary>
        /// A rejected Survey. This survey station will NOT be used for trajectory, AND will not be taken into
        /// account by the characterization algorithm
        /// </summary>
        Rejected = 1,

        /// <summary>
        /// An active survey which has not been corrected for some reason. The survey WILL be used for trajectory calculations
        /// but WILL NOT be used by the characterization algorithm.
        /// </summary>
        ActiveUncorrected = 2,

        /// <summary>
        /// An Active survey which WILL be corrected once a characterization has been determined. This is typically reserved for
        /// the first surveys in a new BHA/Run when we are waiting for enough data to calculate the initial run characterization. Normally:
        /// once a characterization has been generated, all ActivePendingCharacterization classified surveys recorded earlier than the initial characterization
        /// will be 'back corrected'
        /// </summary>
        ActivePendingCharacterization = 4,

        /// <summary>
        /// An active survey that HAS BEEN corrected, but was corrected using a previous characterization (aka The 'running' characterization was not
        /// updated to include/compensate for this survey). This survey data IS used in trajectory
        /// </summary>
        ActiveCorrected = 8,

        /// <summary>
        /// Represents an Active survey that has been re-characterized using this survey station's data as well as all the data from previous
        /// ActiveCorrected surveys. These stations ARE used in trajectory calculations are ARE used in upcoming re-characterization calculations
        /// </summary>
        ActiveCorrectedRecharacterized = 16,

        /// <summary>
        /// A check-shot survey station. This Survey station IS NOT used for trajectory calculations, but IS used by the characterization algorithm,]
        /// </summary>
        ActiveCheckShot = 32,

        /// <summary>
        /// Survey is active and WILL be corrected, the data from this survey will however NOT be used by the characterization algorithm
        /// </summary>
        ActiveCorrectedIgnoreForCharacterization = 64,

        /// <summary>
        /// Survey is Pending characterization, but will not be used by the characterization algorithm
        /// </summary>
        PendingIgnoreForCharacterization = 128,

        /// <summary>
        /// Short/Conventional Survey. Inc/Azm/Depth
        /// </summary>
        ShortSurvey = 256,

        /// <summary>
        /// 'ActiveNotCharacterized' Surveys will NOT be corrected, but WILL participate as input into the characterization algorithm.
        /// </summary>
        ActiveNotCharacterized = 512
    }
}
