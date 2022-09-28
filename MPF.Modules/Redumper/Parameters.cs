﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MPF.Core.Data;
using RedumpLib.Data;

namespace MPF.Modules.Redumper
{
    /// <summary>
    /// Represents a generic set of Redumper parameters
    /// </summary>
    public class Parameters : BaseParameters
    {
        #region Generic Dumping Information

        /// <inheritdoc/>
        public override string InputPath => DriveValue;

        /// <inheritdoc/>
        public override string OutputPath => Path.Combine(ImagePathValue ?? string.Empty, ImageNameValue ?? string.Empty);

        /// <inheritdoc/>
        public override int? Speed => SpeedValue;

        #endregion

        #region Metadata

        /// <inheritdoc/>
        public override InternalProgram InternalProgram => InternalProgram.Redumper;

        #endregion

        #region Flag Values

        /// <summary>
        /// Maximum absolute sample value to treat it as silence (default: 32)
        /// </summary>
        public int? AudioSilenceThresholdValue { get; set; }

        /// <summary>
        /// Drive to use, first available drive with disc, if not provided
        /// </summary>
        public string DriveValue { get; set; }

        /// <summary>
        /// Override offset autodetection and use supplied value
        /// </summary>
        public int? ForceFoffsetValue { get; set; }

        /// <summary>
        /// Dump files prefix, autogenerated in dump mode, if not provided
        /// </summary>
        public string ImageNameValue { get; set; }

        /// <summary>
        /// Dump files base directory
        /// </summary>
        public string ImagePathValue { get; set; }

        /// <summary>
        /// Number of sector retries in case of SCSI/C2 error (default: 0)
        /// </summary>
        public int? RetriesValue { get; set; }

        /// <summary>
        /// Rings mode, maximum ring size to stop subdivision (rings, default: 1024)
        /// </summary>
        public int? RingSizeValue { get; set; }

        /// <summary>
        /// LBA ranges of sectors to skip
        /// </summary>
        public string SkipValue { get; set; }

        /// <summary>
        /// Fill byte value for skipped sectors (default: 0x55)
        /// </summary>
        public byte? SkipFillValue { get; set; }

        /// <summary>
        /// Rings mode, number of sectors to skip on SCSI error (default: 4096)
        /// </summary>
        public int? SkipSizeValue { get; set; }

        /// <summary>
        /// Drive read speed, optimal drive speed will be used if not provided
        /// </summary>
        public int? SpeedValue { get; set; }

        /// <summary>
        /// LBA to stop dumping at (everything before the value, useful for discs with fake TOC
        /// </summary>
        public int? StopLBAValue { get; set; }

        #endregion

        /// <inheritdoc/>
        public Parameters(string parameters) : base(parameters) { }

        /// <inheritdoc/>
        public Parameters(RedumpSystem? system, MediaType? type, char driveLetter, string filename, int? driveSpeed, Options options)
            : base(system, type, driveLetter, filename, driveSpeed, options)
        {
        }

        #region BaseParameters Implementations

        /// <inheritdoc/>
        public override (bool, List<string>) CheckAllOutputFilesExist(string basePath, bool preCheck)
        {
            // TODO: Fill out
            return (true, new List<string>());
        }

        /// <inheritdoc/>
        public override void GenerateSubmissionInfo(SubmissionInfo info, string basePath, Drive drive, bool includeArtifacts)
        {
            // TODO: Fill in submission info specifics for Redumper
            string outputDirectory = Path.GetDirectoryName(basePath);

            switch (this.Type)
            {
                // Determine type-specific differences
            }

            switch (this.System)
            {
                case RedumpSystem.KonamiPython2:
                    if (GetPlayStationExecutableInfo(drive?.Letter, out string pythonTwoSerial, out Region? pythonTwoRegion, out string pythonTwoDate))
                    {
                        // Ensure internal serial is pulled from local data
                        info.CommonDiscInfo.CommentsSpecialFields[SiteCode.InternalSerialName] = pythonTwoSerial ?? string.Empty;
                        info.CommonDiscInfo.Region = info.CommonDiscInfo.Region ?? pythonTwoRegion;
                        info.CommonDiscInfo.EXEDateBuildDate = pythonTwoDate;
                    }

                    info.VersionAndEditions.Version = GetPlayStation2Version(drive?.Letter) ?? "";
                    break;

                case RedumpSystem.SonyPlayStation:
                    if (GetPlayStationExecutableInfo(drive?.Letter, out string playstationSerial, out Region? playstationRegion, out string playstationDate))
                    {
                        // Ensure internal serial is pulled from local data
                        info.CommonDiscInfo.CommentsSpecialFields[SiteCode.InternalSerialName] = playstationSerial ?? string.Empty;
                        info.CommonDiscInfo.Region = info.CommonDiscInfo.Region ?? playstationRegion;
                        info.CommonDiscInfo.EXEDateBuildDate = playstationDate;
                    }

                    break;

                case RedumpSystem.SonyPlayStation2:
                    if (GetPlayStationExecutableInfo(drive?.Letter, out string playstationTwoSerial, out Region? playstationTwoRegion, out string playstationTwoDate))
                    {
                        // Ensure internal serial is pulled from local data
                        info.CommonDiscInfo.CommentsSpecialFields[SiteCode.InternalSerialName] = playstationTwoSerial ?? string.Empty;
                        info.CommonDiscInfo.Region = info.CommonDiscInfo.Region ?? playstationTwoRegion;
                        info.CommonDiscInfo.EXEDateBuildDate = playstationTwoDate;
                    }

                    info.VersionAndEditions.Version = GetPlayStation2Version(drive?.Letter) ?? "";
                    break;

                case RedumpSystem.SonyPlayStation4:
                    info.VersionAndEditions.Version = GetPlayStation4Version(drive?.Letter) ?? "";
                    break;

                case RedumpSystem.SonyPlayStation5:
                    info.VersionAndEditions.Version = GetPlayStation5Version(drive?.Letter) ?? "";
                    break;
            }
        }

        /// <inheritdoc/>
        public override string GenerateParameters()
        {
            List<string> parameters = new List<string>();

            // TODO: Fill out

            return string.Join(" ", parameters);
        }

        /// <inheritdoc/>
        public override Dictionary<string, List<string>> GetCommandSupport()
        {
            // TODO: Figure out actual support for each flag
            return new Dictionary<string, List<string>>()
            {
                [CommandStrings.NONE] = new List<string>()
                {
                    FlagStrings.HelpLong,
                    FlagStrings.HelpShort,
                },
                [CommandStrings.CD] = new List<string>()
                {
                    FlagStrings.AudioSilenceThreshold,
                    FlagStrings.CDiCorrectOffset,
                    FlagStrings.CDiReadyNormalize,
                    FlagStrings.DescrambleNew,
                    FlagStrings.Drive,
                    FlagStrings.ForceOffset,
                    FlagStrings.ForceQTOC,
                    FlagStrings.ForceSplit,
                    FlagStrings.ForceTOC,
                    FlagStrings.ISO9660Trim,
                    FlagStrings.ImageName,
                    FlagStrings.ImagePath,
                    FlagStrings.LeaveUnchanged,
                    FlagStrings.Overwrite,
                    FlagStrings.RefineSubchannel,
                    FlagStrings.Retries,
                    FlagStrings.RingSize,
                    FlagStrings.Skip,
                    FlagStrings.SkipFill,
                    FlagStrings.SkipLeadIn,
                    FlagStrings.SkipSize,
                    FlagStrings.Speed,
                    FlagStrings.StopLBA,
                    FlagStrings.Unsupported,
                    FlagStrings.Verbose,
                },
                [CommandStrings.Dump] = new List<string>()
                {
                    FlagStrings.AudioSilenceThreshold,
                    FlagStrings.CDiCorrectOffset,
                    FlagStrings.CDiReadyNormalize,
                    FlagStrings.DescrambleNew,
                    FlagStrings.Drive,
                    FlagStrings.ForceOffset,
                    FlagStrings.ForceQTOC,
                    FlagStrings.ForceSplit,
                    FlagStrings.ForceTOC,
                    FlagStrings.ISO9660Trim,
                    FlagStrings.ImageName,
                    FlagStrings.ImagePath,
                    FlagStrings.LeaveUnchanged,
                    FlagStrings.Overwrite,
                    FlagStrings.RefineSubchannel,
                    FlagStrings.Retries,
                    FlagStrings.RingSize,
                    FlagStrings.Skip,
                    FlagStrings.SkipFill,
                    FlagStrings.SkipLeadIn,
                    FlagStrings.SkipSize,
                    FlagStrings.Speed,
                    FlagStrings.StopLBA,
                    FlagStrings.Unsupported,
                    FlagStrings.Verbose,
                },
                [CommandStrings.Info] = new List<string>()
                {
                    FlagStrings.AudioSilenceThreshold,
                    FlagStrings.CDiCorrectOffset,
                    FlagStrings.CDiReadyNormalize,
                    FlagStrings.DescrambleNew,
                    FlagStrings.Drive,
                    FlagStrings.ForceOffset,
                    FlagStrings.ForceQTOC,
                    FlagStrings.ForceSplit,
                    FlagStrings.ForceTOC,
                    FlagStrings.ISO9660Trim,
                    FlagStrings.ImageName,
                    FlagStrings.ImagePath,
                    FlagStrings.LeaveUnchanged,
                    FlagStrings.Overwrite,
                    FlagStrings.RefineSubchannel,
                    FlagStrings.Retries,
                    FlagStrings.RingSize,
                    FlagStrings.Skip,
                    FlagStrings.SkipFill,
                    FlagStrings.SkipLeadIn,
                    FlagStrings.SkipSize,
                    FlagStrings.Speed,
                    FlagStrings.StopLBA,
                    FlagStrings.Unsupported,
                    FlagStrings.Verbose,
                },
                [CommandStrings.Protection] = new List<string>()
                {
                    FlagStrings.AudioSilenceThreshold,
                    FlagStrings.CDiCorrectOffset,
                    FlagStrings.CDiReadyNormalize,
                    FlagStrings.DescrambleNew,
                    FlagStrings.Drive,
                    FlagStrings.ForceOffset,
                    FlagStrings.ForceQTOC,
                    FlagStrings.ForceSplit,
                    FlagStrings.ForceTOC,
                    FlagStrings.ISO9660Trim,
                    FlagStrings.ImageName,
                    FlagStrings.ImagePath,
                    FlagStrings.LeaveUnchanged,
                    FlagStrings.Overwrite,
                    FlagStrings.RefineSubchannel,
                    FlagStrings.Retries,
                    FlagStrings.RingSize,
                    FlagStrings.Skip,
                    FlagStrings.SkipFill,
                    FlagStrings.SkipLeadIn,
                    FlagStrings.SkipSize,
                    FlagStrings.Speed,
                    FlagStrings.StopLBA,
                    FlagStrings.Unsupported,
                    FlagStrings.Verbose,
                },
                [CommandStrings.Refine] = new List<string>()
                {
                    FlagStrings.AudioSilenceThreshold,
                    FlagStrings.CDiCorrectOffset,
                    FlagStrings.CDiReadyNormalize,
                    FlagStrings.DescrambleNew,
                    FlagStrings.Drive,
                    FlagStrings.ForceOffset,
                    FlagStrings.ForceQTOC,
                    FlagStrings.ForceSplit,
                    FlagStrings.ForceTOC,
                    FlagStrings.ISO9660Trim,
                    FlagStrings.ImageName,
                    FlagStrings.ImagePath,
                    FlagStrings.LeaveUnchanged,
                    FlagStrings.Overwrite,
                    FlagStrings.RefineSubchannel,
                    FlagStrings.Retries,
                    FlagStrings.RingSize,
                    FlagStrings.Skip,
                    FlagStrings.SkipFill,
                    FlagStrings.SkipLeadIn,
                    FlagStrings.SkipSize,
                    FlagStrings.Speed,
                    FlagStrings.StopLBA,
                    FlagStrings.Unsupported,
                    FlagStrings.Verbose,
                },
                [CommandStrings.Split] = new List<string>()
                {
                    FlagStrings.AudioSilenceThreshold,
                    FlagStrings.CDiCorrectOffset,
                    FlagStrings.CDiReadyNormalize,
                    FlagStrings.DescrambleNew,
                    FlagStrings.Drive,
                    FlagStrings.ForceOffset,
                    FlagStrings.ForceQTOC,
                    FlagStrings.ForceSplit,
                    FlagStrings.ForceTOC,
                    FlagStrings.ISO9660Trim,
                    FlagStrings.ImageName,
                    FlagStrings.ImagePath,
                    FlagStrings.LeaveUnchanged,
                    FlagStrings.Overwrite,
                    FlagStrings.RefineSubchannel,
                    FlagStrings.Retries,
                    FlagStrings.RingSize,
                    FlagStrings.Skip,
                    FlagStrings.SkipFill,
                    FlagStrings.SkipLeadIn,
                    FlagStrings.SkipSize,
                    FlagStrings.Speed,
                    FlagStrings.StopLBA,
                    FlagStrings.Unsupported,
                    FlagStrings.Verbose,
                },
            };
        }

        /// <inheritdoc/>
        public override string GetDefaultExtension(MediaType? mediaType) => ".bin"; // TODO: Fill out

        /// <inheritdoc/>
        public override bool IsDumpingCommand()
        {
            switch (this.BaseCommand)
            {
                case CommandStrings.CD:
                case CommandStrings.Dump:
                    return true;
                default:
                    return false;
            }
        }

        /// <inheritdoc/>
        protected override void ResetValues()
        {
            BaseCommand = CommandStrings.NONE;

            flags = new Dictionary<string, bool?>();

            AudioSilenceThresholdValue = null;
            DriveValue = null;
            ForceFoffsetValue = null;
            ImageNameValue = null;
            ImagePathValue = null;
            RetriesValue = null;
            RingSizeValue = null;
            SkipValue = null;
            SkipFillValue = null;
            SkipSizeValue = null;
            SpeedValue = null;
            StopLBAValue = null;
        }

        /// <inheritdoc/>
        protected override void SetDefaultParameters(char driveLetter, string filename, int? driveSpeed, Options options)
        {
            // TODO: Fill out
        }

        /// <inheritdoc/>
        protected override bool ValidateAndSetParameters(string parameters)
        {
            // TODO: Fill out
            return true;
        }

        #endregion
    }
}
