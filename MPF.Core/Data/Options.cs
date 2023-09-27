﻿using System;
using System.Collections;
using System.Collections.Generic;
using MPF.Core.Converters;
using SabreTools.RedumpLib.Data;

namespace MPF.Core.Data
{
    public class Options : IDictionary<string, string>
    {
        private Dictionary<string, string> _settings;

        #region Internal Program

        /// <summary>
        /// Path to Aaru
        /// </summary>
        public string AaruPath
        {
            get { return GetStringSetting(_settings, "AaruPath", "Programs\\Aaru\\Aaru.exe"); }
            set { _settings["AaruPath"] = value; }
        }

        /// <summary>
        /// Path to DiscImageCreator
        /// </summary>
        public string DiscImageCreatorPath
        {
            get { return GetStringSetting(_settings, "DiscImageCreatorPath", "Programs\\Creator\\DiscImageCreator.exe"); }
            set { _settings["DiscImageCreatorPath"] = value; }
        }

        /// <summary>
        /// Path to Redumper
        /// </summary>
        public string RedumperPath
        {
            get { return GetStringSetting(_settings, "RedumperPath", "Programs\\Redumper\\redumper.exe"); }
            set { _settings["RedumperPath"] = value; }
        }

        /// <summary>
        /// Currently selected dumping program
        /// </summary>
        public InternalProgram InternalProgram
        {
            get
            {
                string valueString = GetStringSetting(_settings, "InternalProgram", InternalProgram.DiscImageCreator.ToString());
                var valueEnum = EnumConverter.ToInternalProgram(valueString);
                return valueEnum == InternalProgram.NONE ? InternalProgram.DiscImageCreator : valueEnum;
            }
            set
            {
                _settings["InternalProgram"] = value.ToString();
            }
        }

        #endregion

        #region UI Defaults

        /// <summary>
        /// Enable dark mode for UI elements
        /// </summary>
        public bool EnableDarkMode
        {
            get { return GetBooleanSetting(_settings, "EnableDarkMode", false); }
            set { _settings["EnableDarkMode"] = value.ToString(); }
        }

        /// <summary>
        /// Check for updates on startup
        /// </summary>
        public bool CheckForUpdatesOnStartup
        {
            get { return GetBooleanSetting(_settings, "CheckForUpdatesOnStartup", true); }
            set { _settings["CheckForUpdatesOnStartup"] = value.ToString(); }
        }

        /// <summary>
        /// Fast update label - Skips disc checks and updates path only
        /// </summary>
        public bool FastUpdateLabel
        {
            get { return GetBooleanSetting(_settings, "FastUpdateLabel", false); }
            set { _settings["FastUpdateLabel"] = value.ToString(); }
        }

        /// <summary>
        /// Default output path for dumps
        /// </summary>
        public string DefaultOutputPath
        {
            get { return GetStringSetting(_settings, "DefaultOutputPath", "ISO"); }
            set { _settings["DefaultOutputPath"] = value; }
        }

        /// <summary>
        /// Default system if none can be detected
        /// </summary>
        public RedumpSystem? DefaultSystem
        {
            get
            {
                string valueString = GetStringSetting(_settings, "DefaultSystem", null);
                var valueEnum = Extensions.ToRedumpSystem(valueString);
                return valueEnum;
            }
            set
            {
                _settings["DefaultSystem"] = value.LongName();
            }
        }

        /// <summary>
        /// Default output path for dumps
        /// </summary>
        /// <remarks>This is a hidden setting</remarks>
        public bool ShowDebugViewMenuItem
        {
            get { return GetBooleanSetting(_settings, "ShowDebugViewMenuItem", false); }
            set { _settings["ShowDebugViewMenuItem"] = value.ToString(); }
        }

        #endregion

        #region Dumping Speeds

        /// <summary>
        /// Default CD dumping speed
        /// </summary>
        public int PreferredDumpSpeedCD
        {
            get { return GetInt32Setting(_settings, "PreferredDumpSpeedCD", 24); }
            set { _settings["PreferredDumpSpeedCD"] = value.ToString(); }
        }

        /// <summary>
        /// Default DVD dumping speed
        /// </summary>
        public int PreferredDumpSpeedDVD
        {
            get { return GetInt32Setting(_settings, "PreferredDumpSpeedDVD", 16); }
            set { _settings["PreferredDumpSpeedDVD"] = value.ToString(); }
        }

        /// <summary>
        /// Default HD-DVD dumping speed
        /// </summary>
        public int PreferredDumpSpeedHDDVD
        {
            get { return GetInt32Setting(_settings, "PreferredDumpSpeedHDDVD", 8); }
            set { _settings["PreferredDumpSpeedHDDVD"] = value.ToString(); }
        }

        /// <summary>
        /// Default BD dumping speed
        /// </summary>
        public int PreferredDumpSpeedBD
        {
            get { return GetInt32Setting(_settings, "PreferredDumpSpeedBD", 8); }
            set { _settings["PreferredDumpSpeedBD"] = value.ToString(); }
        }

        #endregion

        #region Aaru

        /// <summary>
        /// Enable debug output while dumping by default
        /// </summary>
        public bool AaruEnableDebug
        {
            get { return GetBooleanSetting(_settings, "AaruEnableDebug", false); }
            set { _settings["AaruEnableDebug"] = value.ToString(); }
        }

        /// <summary>
        /// Enable verbose output while dumping by default
        /// </summary>
        public bool AaruEnableVerbose
        {
            get { return GetBooleanSetting(_settings, "AaruEnableVerbose", false); }
            set { _settings["AaruEnableVerbose"] = value.ToString(); }
        }

        /// <summary>
        /// Enable force dumping of media by default
        /// </summary>
        public bool AaruForceDumping
        {
            get { return GetBooleanSetting(_settings, "AaruForceDumping", true); }
            set { _settings["AaruForceDumping"] = value.ToString(); }
        }

        /// <summary>
        /// Default number of sector/subchannel rereads
        /// </summary>
        public int AaruRereadCount
        {
            get { return GetInt32Setting(_settings, "AaruRereadCount", 5); }
            set { _settings["AaruRereadCount"] = value.ToString(); }
        }

        /// <summary>
        /// Strip personal data information from Aaru metadata by default
        /// </summary>
        public bool AaruStripPersonalData
        {
            get { return GetBooleanSetting(_settings, "AaruStripPersonalData", false); }
            set { _settings["AaruStripPersonalData"] = value.ToString(); }
        }

        #endregion

        #region DiscImageCreator

        /// <summary>
        /// Enable multi-sector read flag by default
        /// </summary>
        public bool DICMultiSectorRead
        {
            get { return GetBooleanSetting(_settings, "DICMultiSectorRead", false); }
            set { _settings["DICMultiSectorRead"] = value.ToString(); }
        }

        /// <summary>
        /// Include a default multi-sector read value
        /// </summary>
        public int DICMultiSectorReadValue
        {
            get { return GetInt32Setting(_settings, "DICMultiSectorReadValue", 0); }
            set { _settings["DICMultiSectorReadValue"] = value.ToString(); }
        }

        /// <summary>
        /// Enable overly-secure dumping flags by default
        /// </summary>
        /// <remarks>
        /// Split this into component parts later. Currently does:
        /// - Scan sector protection and set subchannel read level to 2 for CD
        /// - Set scan file protect flag for DVD
        /// </remarks>
        public bool DICParanoidMode
        {
            get { return GetBooleanSetting(_settings, "DICParanoidMode", false); }
            set { _settings["DICParanoidMode"] = value.ToString(); }
        }

        /// <summary>
        /// Enable the Quiet flag by default
        /// </summary>
        public bool DICQuietMode
        {
            get { return GetBooleanSetting(_settings, "DICQuietMode", false); }
            set { _settings["DICQuietMode"] = value.ToString(); }
        }

        /// <summary>
        /// Default number of C2 rereads
        /// </summary>
        public int DICRereadCount
        {
            get { return GetInt32Setting(_settings, "DICRereadCount", 20); }
            set { _settings["DICRereadCount"] = value.ToString(); }
        }

        /// <summary>
        /// Default number of DVD/HD-DVD/BD rereads
        /// </summary>
        public int DICDVDRereadCount
        {
            get { return GetInt32Setting(_settings, "DICDVDRereadCount", 10); }
            set { _settings["DICDVDRereadCount"] = value.ToString(); }
        }

        /// <summary>
        /// Reset drive after dumping (useful for older drives)
        /// </summary>
        public bool DICResetDriveAfterDump
        {
            get { return GetBooleanSetting(_settings, "DICResetDriveAfterDump", false); }
            set { _settings["DICResetDriveAfterDump"] = value.ToString(); }
        }

        /// <summary>
        /// Use the CMI flag for supported disc types
        /// </summary>
        public bool DICUseCMIFlag
        {
            get { return GetBooleanSetting(_settings, "DICUseCMIFlag", false); }
            set { _settings["DICUseCMIFlag"] = value.ToString(); }
        }

        #endregion

        #region Redumper

        /// <summary>
        /// Enable debug output while dumping by default
        /// </summary>
        public bool RedumperEnableDebug
        {
            get { return GetBooleanSetting(_settings, "RedumperEnableDebug", false); }
            set { _settings["RedumperEnableDebug"] = value.ToString(); }
        }

        /// <summary>
        /// Enable verbose output while dumping by default
        /// </summary>
        public bool RedumperEnableVerbose
        {
            get { return GetBooleanSetting(_settings, "RedumperEnableVerbose", false); }
            set { _settings["RedumperEnableVerbose"] = value.ToString(); }
        }

        /// <summary>
        /// Default number of rereads
        /// </summary>
        public int RedumperRereadCount
        {
            get { return GetInt32Setting(_settings, "RedumperRereadCount", 20); }
            set { _settings["RedumperRereadCount"] = value.ToString(); }
        }

        #endregion

        #region Extra Dumping Options

        /// <summary>
        /// Scan the disc for protection after dumping
        /// </summary>
        public bool ScanForProtection
        {
            get { return GetBooleanSetting(_settings, "ScanForProtection", true); }
            set { _settings["ScanForProtection"] = value.ToString(); }
        }

        /// <summary>
        /// Output all found protections to a separate file in the directory
        /// </summary>
        public bool OutputSeparateProtectionFile
        {
            get { return GetBooleanSetting(_settings, "OutputSeparateProtectionFile", true); }
            set { _settings["OutputSeparateProtectionFile"] = value.ToString(); }
        }

        /// <summary>
        /// Add placeholder values in the submission info
        /// </summary>
        public bool AddPlaceholders
        {
            get { return GetBooleanSetting(_settings, "AddPlaceholders", true); }
            set { _settings["AddPlaceholders"] = value.ToString(); }
        }

        /// <summary>
        /// Show the disc information window after dumping
        /// </summary>
        public bool PromptForDiscInformation
        {
            get { return GetBooleanSetting(_settings, "PromptForDiscInformation", true); }
            set { _settings["PromptForDiscInformation"] = value.ToString(); }
        }

        /// <summary>
        /// Enable tabs in all input fields
        /// </summary>
        public bool EnableTabsInInputFields
        {
            get { return GetBooleanSetting(_settings, "EnableTabsInInputFields", false); }
            set { _settings["EnableTabsInInputFields"] = value.ToString(); }
        }

        /// <summary>
        /// Limit outputs to Redump-supported values only
        /// </summary>
        public bool EnableRedumpCompatibility
        {
            get { return GetBooleanSetting(_settings, "EnableRedumpCompatibility", true); }
            set { _settings["EnableRedumpCompatibility"] = value.ToString(); }
        }

        /// <summary>
        /// Show disc eject reminder before the disc information window is shown
        /// </summary>
        public bool ShowDiscEjectReminder
        {
            get { return GetBooleanSetting(_settings, "ShowDiscEjectReminder", true); }
            set { _settings["ShowDiscEjectReminder"] = value.ToString(); }
        }

        /// <summary>
        /// Eject the disc after dumping
        /// </summary>
        public bool EjectAfterDump
        {
            get { return GetBooleanSetting(_settings, "EjectAfterDump", false); }
            set { _settings["EjectAfterDump"] = value.ToString(); }
        }

        /// <summary>
        /// Ignore fixed drives when populating the list
        /// </summary>
        public bool IgnoreFixedDrives
        {
            get { return GetBooleanSetting(_settings, "IgnoreFixedDrives", true); }
            set { _settings["IgnoreFixedDrives"] = value.ToString(); }
        }

        /// <summary>
        /// Show dumping tools in their own window instead of in the log
        /// </summary>
        public bool ToolsInSeparateWindow
        {
            get { return GetBooleanSetting(_settings, "ToolsInSeparateWindow", true); }
            set { _settings["ToolsInSeparateWindow"] = value.ToString(); }
        }

        /// <summary>
        /// Output the compressed JSON version of the submission info
        /// </summary>
        public bool OutputSubmissionJSON
        {
            get { return GetBooleanSetting(_settings, "OutputSubmissionJSON", false); }
            set { _settings["OutputSubmissionJSON"] = value.ToString(); }
        }

        /// <summary>
        /// Include log files in serialized JSON data
        /// </summary>
        public bool IncludeArtifacts
        {
            get { return GetBooleanSetting(_settings, "IncludeArtifacts", false); }
            set { _settings["IncludeArtifacts"] = value.ToString(); }
        }

        /// <summary>
        /// Compress output log files to reduce space
        /// </summary>
        public bool CompressLogFiles
        {
            get { return GetBooleanSetting(_settings, "CompressLogFiles", true); }
            set { _settings["CompressLogFiles"] = value.ToString(); }
        }

        #endregion

        #region Skip Options

        /// <summary>
        /// Skip detecting media type on disc scan
        /// </summary>
        public bool SkipMediaTypeDetection
        {
            get { return GetBooleanSetting(_settings, "SkipMediaTypeDetection", false); }
            set { _settings["SkipMediaTypeDetection"] = value.ToString(); }
        }

        /// <summary>
        /// Skip detecting known system on disc scan
        /// </summary>
        public bool SkipSystemDetection
        {
            get { return GetBooleanSetting(_settings, "SkipSystemDetection", false); }
            set { _settings["SkipSystemDetection"] = value.ToString(); }
        }

        #endregion

        #region Protection Scanning Options

        /// <summary>
        /// Scan archive contents during protection scanning
        /// </summary>
        public bool ScanArchivesForProtection
        {
            get { return GetBooleanSetting(_settings, "ScanArchivesForProtection", true); }
            set { _settings["ScanArchivesForProtection"] = value.ToString(); }
        }

        /// <summary>
        /// Scan for executable packers during protection scanning
        /// </summary>
        public bool ScanPackersForProtection
        {
            get { return GetBooleanSetting(_settings, "ScanPackersForProtection", false); }
            set { _settings["ScanPackersForProtection"] = value.ToString(); }
        }

        /// <summary>
        /// Include debug information with scan results
        /// </summary>
        public bool IncludeDebugProtectionInformation
        {
            get { return GetBooleanSetting(_settings, "IncludeDebugProtectionInformation", false); }
            set { _settings["IncludeDebugProtectionInformation"] = value.ToString(); }
        }

        #endregion

        #region Logging Options

        /// <summary>
        /// Enable verbose and debug logs to be written
        /// </summary>
        public bool VerboseLogging
        {
            get { return GetBooleanSetting(_settings, "VerboseLogging", true); }
            set { _settings["VerboseLogging"] = value.ToString(); }
        }

        /// <summary>
        /// Have the log panel expanded by default on startup
        /// </summary>
        public bool OpenLogWindowAtStartup
        {
            get { return GetBooleanSetting(_settings, "OpenLogWindowAtStartup", true); }
            set { _settings["OpenLogWindowAtStartup"] = value.ToString(); }
        }

        #endregion

        #region Redump Login Information

        public string RedumpUsername
        {
            get { return GetStringSetting(_settings, "RedumpUsername", ""); }
            set { _settings["RedumpUsername"] = value; }
        }

        // TODO: Figure out a way to keep this encrypted in some way, BASE64 to start?
        public string RedumpPassword
        {
            get { return GetStringSetting(_settings, "RedumpPassword", ""); }
            set { _settings["RedumpPassword"] = value; }
        }

        /// <summary>
        /// Determine if a complete set of Redump credentials might exist
        /// </summary>
        public bool HasRedumpLogin { get => !string.IsNullOrWhiteSpace(RedumpUsername) && !string.IsNullOrWhiteSpace(RedumpPassword); }

        #endregion

        /// <summary>
        /// Constructor taking a dictionary for settings
        /// </summary>
        /// <param name="settings"></param>
        public Options(Dictionary<string, string> settings = null)
        {
            this._settings = settings ?? new Dictionary<string, string>();
        }

        /// <summary>
        /// Constructor taking an existing Options object
        /// </summary>
        /// <param name="source"></param>
        public Options(Options source)
        {
            _settings = new Dictionary<string, string>(source._settings);
        }

        /// <summary>
        /// Set all fields from an existing Options object
        /// </summary>
        /// <param name="source"></param>
        public void SetFromExisting(Options source)
        {
            _settings = new Dictionary<string, string>(source._settings);
        }

        #region Helpers

        /// <summary>
        /// Get a Boolean setting from a settings, dictionary
        /// </summary>
        /// <param name="settings">Dictionary representing the settings</param>
        /// <param name="key">Setting key to get a value for</param>
        /// <param name="defaultValue">Default value to return if no value is found</param>
        /// <returns>Setting value if possible, default value otherwise</returns>
        private bool GetBooleanSetting(Dictionary<string, string> settings, string key, bool defaultValue)
        {
            if (settings.ContainsKey(key))
            {
                if (Boolean.TryParse(settings[key], out bool value))
                    return value;
                else
                    return defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get an Int32 setting from a settings, dictionary
        /// </summary>
        /// <param name="settings">Dictionary representing the settings</param>
        /// <param name="key">Setting key to get a value for</param>
        /// <param name="defaultValue">Default value to return if no value is found</param>
        /// <returns>Setting value if possible, default value otherwise</returns>
        private int GetInt32Setting(Dictionary<string, string> settings, string key, int defaultValue)
        {
            if (settings.ContainsKey(key))
            {
                if (Int32.TryParse(settings[key], out int value))
                    return value;
                else
                    return defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Get a String setting from a settings, dictionary
        /// </summary>
        /// <param name="settings">Dictionary representing the settings</param>
        /// <param name="key">Setting key to get a value for</param>
        /// <param name="defaultValue">Default value to return if no value is found</param>
        /// <returns>Setting value if possible, default value otherwise</returns>
        private string GetStringSetting(Dictionary<string, string> settings, string key, string defaultValue)
        {
            if (settings.ContainsKey(key))
                return settings[key];
            else
                return defaultValue;
        }

        #endregion

        #region IDictionary implementations

        public ICollection<string> Keys => _settings.Keys;

        public ICollection<string> Values => _settings.Values;

        public int Count => _settings.Count;

        public bool IsReadOnly => ((IDictionary<string, string>)_settings).IsReadOnly;

        public string this[string key]
        {
            get { return (_settings.ContainsKey(key) ? _settings[key] : null); }
            set { _settings[key] = value; }
        }

        public bool ContainsKey(string key) => _settings.ContainsKey(key);

        public void Add(string key, string value) => _settings.Add(key, value);

        public bool Remove(string key) => _settings.Remove(key);

        public bool TryGetValue(string key, out string value) => _settings.TryGetValue(key, out value);

        public void Add(KeyValuePair<string, string> item) => _settings.Add(item.Key, item.Value);

        public void Clear() => _settings.Clear();

        public bool Contains(KeyValuePair<string, string> item) => ((IDictionary<string, string>)_settings).Contains(item);

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => ((IDictionary<string, string>)_settings).CopyTo(array, arrayIndex);

        public bool Remove(KeyValuePair<string, string> item) => ((IDictionary<string, string>)_settings).Remove(item);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _settings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _settings.GetEnumerator();

        #endregion
    }
}
