using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MPF.Core.Data;
using MPF.Core.Utilities;
using SabreTools.RedumpLib.Data;

namespace MPF.Core.Modules
{
    public abstract class BaseParameters
    {
        #region Event Handlers

        /// <summary>
        /// Geneeic way of reporting a message
        /// </summary>
        /// <param name="message">String value to report</param>
#if NET48
        public EventHandler<string> ReportStatus;
#else
        public EventHandler<string>? ReportStatus;
#endif

        #endregion

        #region Generic Dumping Information

        /// <summary>
        /// Base command to run
        /// </summary>
#if NET48
        public string BaseCommand { get; set; }
#else
        public string? BaseCommand { get; set; }
#endif

        /// <summary>
        /// Set of flags to pass to the executable
        /// </summary>
#if NET48
        protected Dictionary<string, bool?> flags = new Dictionary<string, bool?>();
#else
        protected Dictionary<string, bool?> flags = new();
#endif
        protected internal IEnumerable<string> Keys => flags.Keys;

        /// <summary>
        /// Safe access to currently set flags
        /// </summary>
        public bool? this[string key]
        {
            get
            {
                if (flags.ContainsKey(key))
                    return flags[key];

                return null;
            }
            set
            {
                flags[key] = value;
            }
        }

        /// <summary>
        /// Process to track external program
        /// </summary>
#if NET48
        private Process process;
#else
        private Process? process;
#endif

        #endregion

        #region Virtual Dumping Information

        /// <summary>
        /// Command to flag support mappings
        /// </summary>
#if NET48
        public Dictionary<string, List<string>> CommandSupport => GetCommandSupport();
#else
        public Dictionary<string, List<string>>? CommandSupport => GetCommandSupport();
#endif

        /// <summary>
        /// Input path for operations
        /// </summary>
#if NET48
        public virtual string InputPath => null;
#else
        public virtual string? InputPath => null;
#endif

        /// <summary>
        /// Output path for operations
        /// </summary>
        /// <returns>String representing the path, null on error</returns>
#if NET48
        public virtual string OutputPath => null;
#else
        public virtual string? OutputPath => null;
#endif

        /// <summary>
        /// Get the processing speed from the implementation
        /// </summary>
        public virtual int? Speed { get; set; } = null;

        #endregion

        #region Metadata

        /// <summary>
        /// Path to the executable
        /// </summary>
#if NET48
        public string ExecutablePath { get; set; }
#else
        public string? ExecutablePath { get; set; }
#endif

        /// <summary>
        /// Program that this set of parameters represents
        /// </summary>
        public virtual InternalProgram InternalProgram { get; }

        /// <summary>
        /// Currently represented system
        /// </summary>
        public RedumpSystem? System { get; set; }

        /// <summary>
        /// Currently represented media type
        /// </summary>
        public MediaType? Type { get; set; }

        #endregion

        /// <summary>
        /// Populate a Parameters object from a param string
        /// </summary>
        /// <param name="parameters">String possibly representing a set of parameters</param>
#if NET48
        public BaseParameters(string parameters)
#else
        public BaseParameters(string? parameters)
#endif
        {
            // If any parameters are not valid, wipe out everything
            if (!ValidateAndSetParameters(parameters))
                ResetValues();
        }

        /// <summary>
        /// Generate parameters based on a set of known inputs
        /// </summary>
        /// <param name="system">RedumpSystem value to use</param>
        /// <param name="type">MediaType value to use</param>
        /// <param name="drivePath">Drive path to use</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="driveSpeed">Drive speed to use</param>
        /// <param name="options">Options object containing all settings that may be used for setting parameters</param>
#if NET48
        public BaseParameters(RedumpSystem? system, MediaType? type, string drivePath, string filename, int? driveSpeed, Options options)
#else
        public BaseParameters(RedumpSystem? system, MediaType? type, string? drivePath, string filename, int? driveSpeed, Options options)
#endif
        {
            this.System = system;
            this.Type = type;
            SetDefaultParameters(drivePath, filename, driveSpeed, options);
        }

        #region Abstract Methods

        /// <summary>
        /// Validate if all required output files exist
        /// </summary>
        /// <param name="basePath">Base filename and path to use for checking</param>
        /// <param name="preCheck">True if this is a check done before a dump, false if done after</param>
        /// <returns>Tuple of true if all required files exist, false otherwise and a list representing missing files</returns>
        public abstract (bool, List<string>) CheckAllOutputFilesExist(string basePath, bool preCheck);

        /// <summary>
        /// Generate a SubmissionInfo for the output files
        /// </summary>
        /// <param name="submissionInfo">Base submission info to fill in specifics for</param>
        /// <param name="options">Options object representing user-defined options</param>
        /// <param name="basePath">Base filename and path to use for checking</param>
        /// <param name="drive">Drive representing the disc to get information from</param>
        /// <param name="includeArtifacts">True to include output files as encoded artifacts, false otherwise</param>
#if NET48
        public abstract void GenerateSubmissionInfo(SubmissionInfo submissionInfo, Options options, string basePath, Drive drive, bool includeArtifacts);
#else
        public abstract void GenerateSubmissionInfo(SubmissionInfo submissionInfo, Options options, string basePath, Drive? drive, bool includeArtifacts);
#endif

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Get all commands mapped to the supported flags
        /// </summary>
        /// <returns>Mappings from command to supported flags</returns>
#if NET48
        public virtual Dictionary<string, List<string>> GetCommandSupport() => null;
#else
        public virtual Dictionary<string, List<string>>? GetCommandSupport() => null;
#endif

        /// <summary>
        /// Blindly generate a parameter string based on the inputs
        /// </summary>
        /// <returns>Parameter string for invocation, null on error</returns>
#if NET48
        public virtual string GenerateParameters() => null;
#else
        public virtual string? GenerateParameters() => null;
#endif

        /// <summary>
        /// Get the default extension for a given media type
        /// </summary>
        /// <param name="mediaType">MediaType value to check</param>
        /// <returns>String representing the media type, null on error</returns>
#if NET48
        public virtual string GetDefaultExtension(MediaType? mediaType) => null;
#else
        public virtual string? GetDefaultExtension(MediaType? mediaType) => null;
#endif

        /// <summary>
        /// Generate a list of all deleteable files generated
        /// </summary>
        /// <param name="basePath">Base filename and path to use for checking</param>
        /// <returns>List of all deleteable file paths, empty otherwise</returns>
#if NET48
        public virtual List<string> GetDeleteableFilePaths(string basePath) => new List<string>();
#else
        public virtual List<string> GetDeleteableFilePaths(string basePath) => new();
#endif

        /// <summary>
        /// Generate a list of all log files generated
        /// </summary>
        /// <param name="basePath">Base filename and path to use for checking</param>
        /// <returns>List of all log file paths, empty otherwise</returns>
#if NET48
        public virtual List<string> GetLogFilePaths(string basePath) => new List<string>();
#else
        public virtual List<string> GetLogFilePaths(string basePath) => new();
#endif

        /// <summary>
        /// Get the MediaType from the current set of parameters
        /// </summary>
        /// <returns>MediaType value if successful, null on error</returns>
        public virtual MediaType? GetMediaType() => null;

        /// <summary>
        /// Gets if the current command is considered a dumping command or not
        /// </summary>
        /// <returns>True if it's a dumping command, false otherwise</returns>
        public virtual bool IsDumpingCommand() => true;

        /// <summary>
        /// Gets if the flag is supported by the current command
        /// </summary>
        /// <param name="flag">Flag value to check</param>
        /// <returns>True if the flag value is supported, false otherwise</returns>
        public virtual bool IsFlagSupported(string flag)
        {
            if (CommandSupport == null)
                return false;
            if (this.BaseCommand == null)
                return false;
            if (!CommandSupport.ContainsKey(this.BaseCommand))
                return false;
            return CommandSupport[this.BaseCommand].Contains(flag);
        }

        /// <summary>
        /// Returns if the current Parameter object is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid() => GenerateParameters() != null;

        /// <summary>
        /// Reset all special variables to have default values
        /// </summary>
        protected virtual void ResetValues() { }

        /// <summary>
        /// Set default parameters for a given system and media type
        /// </summary>
        /// <param name="drivePath">Drive path to use</param>
        /// <param name="filename">Filename to use</param>
        /// <param name="driveSpeed">Drive speed to use</param>
        /// <param name="options">Options object containing all settings that may be used for setting parameters</param>
#if NET48
        protected virtual void SetDefaultParameters(string drivePath, string filename, int? driveSpeed, Options options) { }
#else
        protected virtual void SetDefaultParameters(string? drivePath, string filename, int? driveSpeed, Options options) { }
#endif

        /// <summary>
        /// Scan a possible parameter string and populate whatever possible
        /// </summary>
        /// <param name="parameters">String possibly representing parameters</param>
        /// <returns>True if the parameters were set correctly, false otherwise</returns>
#if NET48
        protected virtual bool ValidateAndSetParameters(string parameters) => !string.IsNullOrWhiteSpace(parameters);
#else
        protected virtual bool ValidateAndSetParameters(string? parameters) => !string.IsNullOrWhiteSpace(parameters);
#endif

        #endregion

        #region Execution

        /// <summary>
        /// Run internal program
        /// </summary>
        /// <param name="separateWindow">True to show in separate window, false otherwise</param>
        public void ExecuteInternalProgram(bool separateWindow)
        {
            // Create the start info
            var startInfo = new ProcessStartInfo()
            {
                FileName = ExecutablePath,
                Arguments = GenerateParameters() ?? "",
                CreateNoWindow = !separateWindow,
                UseShellExecute = separateWindow,
                RedirectStandardOutput = !separateWindow,
                RedirectStandardError = !separateWindow,
            };

            // Create the new process
            process = new Process() { StartInfo = startInfo };

            // Start the process
            process.Start();

            // Start processing tasks, if necessary
            if (!separateWindow)
            {
                _ = Logging.OutputToLog(process.StandardOutput, this, ReportStatus);
                _ = Logging.OutputToLog(process.StandardError, this, ReportStatus);
            }

            process.WaitForExit();
            process.Close();
        }

        /// <summary>
        /// Cancel an in-progress dumping process
        /// </summary>
        public void KillInternalProgram()
        {
            try
            {
                while (process != null && !process.HasExited)
                {
                    process.Kill();
                }
            }
            catch
            { }
        }

        #endregion

        #region Parameter Parsing

        /// <summary>
        /// Returns whether or not the selected item exists
        /// </summary>
        /// <param name="parameters">List of parameters to check against</param>
        /// <param name="index">Current index</param>
        /// <returns>True if the next item exists, false otherwise</returns>
        protected static bool DoesExist(List<string> parameters, int index)
            => index < parameters.Count;

        /// <summary>
        /// Get the Base64 representation of a string
        /// </summary>
        /// <param name="content">String content to encode</param>
        /// <returns>Base64-encoded contents, if possible</returns>
#if NET48
        protected static string GetBase64(string content)
#else
        protected static string? GetBase64(string? content)
#endif
        {
            if (string.IsNullOrEmpty(content))
                return null;

            byte[] temp = Encoding.UTF8.GetBytes(content);
            return Convert.ToBase64String(temp);
        }

        /// <summary>
        /// Get the full lines from the input file, if possible
        /// </summary>
        /// <param name="filename">file location</param>
        /// <param name="binary">True if should read as binary, false otherwise (default)</param>
        /// <returns>Full text of the file, null on error</returns>
#if NET48
        protected static string GetFullFile(string filename, bool binary = false)
#else
        protected static string? GetFullFile(string filename, bool binary = false)
#endif
        {
            // If the file doesn't exist, we can't get info from it
            if (!File.Exists(filename))
                return null;

            // If we're reading as binary
            if (binary)
            {
                byte[] bytes = File.ReadAllBytes(filename);
                return BitConverter.ToString(bytes).Replace("-", string.Empty);
            }

            return File.ReadAllText(filename);
        }

        /// <summary>
        /// Returns whether a string is a valid drive letter
        /// </summary>
        /// <param name="parameter">String value to check</param>
        /// <returns>True if it's a valid drive letter, false otherwise</W>
        protected static bool IsValidDriveLetter(string parameter)
            => Regex.IsMatch(parameter, @"^[A-Z]:?\\?$");

        /// <summary>
        /// Returns whether a string is a valid bool
        /// </summary>
        /// <param name="parameter">String value to check</param>
        /// <returns>True if it's a valid bool, false otherwise</returns>
        protected static bool IsValidBool(string parameter)
            => bool.TryParse(parameter, out bool _);

        /// <summary>
        /// Returns whether a string is a valid byte
        /// </summary>
        /// <param name="parameter">String value to check</param>
        /// <param name="lowerBound">Lower bound (>=)</param>
        /// <param name="upperBound">Upper bound (<=)</param>
        /// <returns>True if it's a valid byte, false otherwise</returns>
        protected static bool IsValidInt8(string parameter, sbyte lowerBound = -1, sbyte upperBound = -1)
        {
            (string value, long _) = ExtractFactorFromValue(parameter);
            if (!sbyte.TryParse(value, out sbyte temp))
                return false;
            else if (lowerBound != -1 && temp < lowerBound)
                return false;
            else if (upperBound != -1 && temp > upperBound)
                return false;

            return true;
        }

        /// <summary>
        /// Returns whether a string is a valid Int16
        /// </summary>
        /// <param name="parameter">String value to check</param>
        /// <param name="lowerBound">Lower bound (>=)</param>
        /// <param name="upperBound">Upper bound (<=)</param>
        /// <returns>True if it's a valid Int16, false otherwise</returns>
        protected static bool IsValidInt16(string parameter, short lowerBound = -1, short upperBound = -1)
        {
            (string value, long _) = ExtractFactorFromValue(parameter);
            if (!short.TryParse(value, out short temp))
                return false;
            else if (lowerBound != -1 && temp < lowerBound)
                return false;
            else if (upperBound != -1 && temp > upperBound)
                return false;

            return true;
        }

        /// <summary>
        /// Returns whether a string is a valid Int32
        /// </summary>
        /// <param name="parameter">String value to check</param>
        /// <param name="lowerBound">Lower bound (>=)</param>
        /// <param name="upperBound">Upper bound (<=)</param>
        /// <returns>True if it's a valid Int32, false otherwise</returns>
        protected static bool IsValidInt32(string parameter, int lowerBound = -1, int upperBound = -1)
        {
            (string value, long _) = ExtractFactorFromValue(parameter);
            if (!int.TryParse(value, out int temp))
                return false;
            else if (lowerBound != -1 && temp < lowerBound)
                return false;
            else if (upperBound != -1 && temp > upperBound)
                return false;

            return true;
        }

        /// <summary>
        /// Returns whether a string is a valid Int64
        /// </summary>
        /// <param name="parameter">String value to check</param>
        /// <param name="lowerBound">Lower bound (>=)</param>
        /// <param name="upperBound">Upper bound (<=)</param>
        /// <returns>True if it's a valid Int64, false otherwise</returns>
        protected static bool IsValidInt64(string parameter, long lowerBound = -1, long upperBound = -1)
        {
            (string value, long _) = ExtractFactorFromValue(parameter);
            if (!long.TryParse(value, out long temp))
                return false;
            else if (lowerBound != -1 && temp < lowerBound)
                return false;
            else if (upperBound != -1 && temp > upperBound)
                return false;

            return true;
        }

        /// <summary>
        /// Process a flag parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <returns>True if the parameter was processed successfully or skipped, false otherwise</returns>
        protected bool ProcessFlagParameter(List<string> parts, string flagString, ref int i)
            => ProcessFlagParameter(parts, null, flagString, ref i);

        /// <summary>
        /// Process a flag parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <returns>True if the parameter was processed successfully or skipped, false otherwise</returns>
#if NET48
        protected bool ProcessFlagParameter(List<string> parts, string shortFlagString, string longFlagString, ref int i)
#else
        protected bool ProcessFlagParameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i)
#endif
        {
            if (parts == null)
                return false;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                    return false;

                this[longFlagString] = true;
            }

            return true;
        }

        /// <summary>
        /// Process a boolean parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>True if the parameter was processed successfully or skipped, false otherwise</returns>
        protected bool ProcessBooleanParameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
            => ProcessBooleanParameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process a boolean parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>True if the parameter was processed successfully or skipped, false otherwise</returns>
#if NET48
        protected bool ProcessBooleanParameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected bool ProcessBooleanParameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return false;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return false;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                    {
                        this[longFlagString] = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                    {
                        this[longFlagString] = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (!IsValidBool(parts[i + 1]))
                {
                    if (missingAllowed)
                    {
                        this[longFlagString] = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                this[longFlagString] = bool.Parse(parts[i + 1]);
                i++;
            }

            return true;
        }

        /// <summary>
        /// Process a sbyte parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>SByte value if success, SByte.MinValue if skipped, null on error/returns>
        protected sbyte? ProcessInt8Parameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
            => ProcessInt8Parameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process an sbyte parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>SByte value if success, SByte.MinValue if skipped, null on error/returns>
#if NET48
        protected sbyte? ProcessInt8Parameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected sbyte? ProcessInt8Parameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return null;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return null;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (!IsValidInt8(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }

                this[longFlagString] = true;
                i++;

                (string value, long factor) = ExtractFactorFromValue(parts[i]);
                return (sbyte)(sbyte.Parse(value) * factor);
            }
            else if (parts[i].StartsWith(shortFlagString + "=") || parts[i].StartsWith(longFlagString + "="))
            {
                if (!IsFlagSupported(longFlagString))
                    return null;

                string[] commandParts = parts[i].Split('=');
                if (commandParts.Length != 2)
                    return null;

                string valuePart = commandParts[1];

                this[longFlagString] = true;
                (string value, long factor) = ExtractFactorFromValue(valuePart);
                return (sbyte)(sbyte.Parse(value) * factor);
            }

            return SByte.MinValue;
        }

        /// <summary>
        /// Process an Int16 parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Int16 value if success, Int16.MinValue if skipped, null on error/returns>
        protected short? ProcessInt16Parameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
            => ProcessInt16Parameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process an Int16 parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Int16 value if success, Int16.MinValue if skipped, null on error/returns>
#if NET48
        protected short? ProcessInt16Parameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected short? ProcessInt16Parameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return null;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return null;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (!IsValidInt16(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }

                this[longFlagString] = true;
                i++;
                (string value, long factor) = ExtractFactorFromValue(parts[i]);
                return (short)(short.Parse(value) * factor);
            }
            else if (parts[i].StartsWith(shortFlagString + "=") || parts[i].StartsWith(longFlagString + "="))
            {
                if (!IsFlagSupported(longFlagString))
                    return null;

                string[] commandParts = parts[i].Split('=');
                if (commandParts.Length != 2)
                    return null;

                string valuePart = commandParts[1];

                this[longFlagString] = true;
                (string value, long factor) = ExtractFactorFromValue(valuePart);
                return (short)(short.Parse(value) * factor);
            }

            return Int16.MinValue;
        }

        /// <summary>
        /// Process an Int32 parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Int32 value if success, Int32.MinValue if skipped, null on error/returns>
        protected int? ProcessInt32Parameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
            => ProcessInt32Parameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process an Int32 parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Int32 value if success, Int32.MinValue if skipped, null on error/returns>
#if NET48
        protected int? ProcessInt32Parameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected int? ProcessInt32Parameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return null;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return null;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (!IsValidInt32(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }

                this[longFlagString] = true;
                i++;
                (string value, long factor) = ExtractFactorFromValue(parts[i]);
                return (int)(int.Parse(value) * factor);
            }
            else if (parts[i].StartsWith(shortFlagString + "=") || parts[i].StartsWith(longFlagString + "="))
            {
                if (!IsFlagSupported(longFlagString))
                    return null;

                string[] commandParts = parts[i].Split('=');
                if (commandParts.Length != 2)
                    return null;

                string valuePart = commandParts[1];

                this[longFlagString] = true;
                (string value, long factor) = ExtractFactorFromValue(valuePart);
                return (int)(int.Parse(value) * factor);
            }

            return Int32.MinValue;
        }

        /// <summary>
        /// Process an Int64 parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Int64 value if success, Int64.MinValue if skipped, null on error/returns>
        protected long? ProcessInt64Parameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
            => ProcessInt64Parameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process an Int64 parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Int64 value if success, Int64.MinValue if skipped, null on error/returns>
#if NET48
        protected long? ProcessInt64Parameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected long? ProcessInt64Parameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return null;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return null;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (!IsValidInt64(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }

                this[longFlagString] = true;
                i++;
                (string value, long factor) = ExtractFactorFromValue(parts[i]);
                return long.Parse(value) * factor;
            }
            else if (parts[i].StartsWith(shortFlagString + "=") || parts[i].StartsWith(longFlagString + "="))
            {
                if (!IsFlagSupported(longFlagString))
                    return null;

                string[] commandParts = parts[i].Split('=');
                if (commandParts.Length != 2)
                    return null;

                string valuePart = commandParts[1];

                this[longFlagString] = true;
                (string value, long factor) = ExtractFactorFromValue(valuePart);
                return long.Parse(value) * factor;
            }

            return Int64.MinValue;
        }

        /// <summary>
        /// Process an string parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>String value if possible, string.Empty on missing, null on error</returns>
#if NET48
        protected string ProcessStringParameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
#else
        protected string? ProcessStringParameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
#endif
        => ProcessStringParameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process a string parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>String value if possible, string.Empty on missing, null on error</returns>
#if NET48
        protected string ProcessStringParameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected string? ProcessStringParameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return null;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return null;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (string.IsNullOrWhiteSpace(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }

                this[longFlagString] = true;
                i++;
                return parts[i].Trim('"');
            }
            else if (parts[i].StartsWith(shortFlagString + "=") || parts[i].StartsWith(longFlagString + "="))
            {
                if (!IsFlagSupported(longFlagString))
                    return null;

                string[] commandParts = parts[i].Split('=');
                if (commandParts.Length != 2)
                    return null;

                string valuePart = commandParts[1];

                this[longFlagString] = true;
                return valuePart.Trim('"');
            }

            return string.Empty;
        }

        /// <summary>
        /// Process a byte parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="flagString">Flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Byte value if success, Byte.MinValue if skipped, null on error/returns>
        protected byte? ProcessUInt8Parameter(List<string> parts, string flagString, ref int i, bool missingAllowed = false)
            => ProcessUInt8Parameter(parts, null, flagString, ref i, missingAllowed);

        /// <summary>
        /// Process a byte parameter
        /// </summary>
        /// <param name="parts">List of parts to be referenced</param>
        /// <param name="shortFlagString">Short flag string, if available</param>
        /// <param name="longFlagString">Long flag string, if available</param>
        /// <param name="i">Reference to the position in the parts</param>
        /// <param name="missingAllowed">True if missing values are allowed, false otherwise</param>
        /// <returns>Byte value if success, Byte.MinValue if skipped, null on error/returns>
#if NET48
        protected byte? ProcessUInt8Parameter(List<string> parts, string shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#else
        protected byte? ProcessUInt8Parameter(List<string> parts, string? shortFlagString, string longFlagString, ref int i, bool missingAllowed = false)
#endif
        {
            if (parts == null)
                return null;

            if (parts[i] == shortFlagString || parts[i] == longFlagString)
            {
                if (!IsFlagSupported(longFlagString))
                {
                    return null;
                }
                else if (!DoesExist(parts, i + 1))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (IsFlagSupported(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }
                else if (!IsValidInt8(parts[i + 1]))
                {
                    if (missingAllowed)
                        this[longFlagString] = true;

                    return null;
                }

                this[longFlagString] = true;
                i++;

                (string value, long factor) = ExtractFactorFromValue(parts[i]);
                return (byte)(byte.Parse(value) * factor);
            }
            else if (parts[i].StartsWith(shortFlagString + "=") || parts[i].StartsWith(longFlagString + "="))
            {
                if (!IsFlagSupported(longFlagString))
                    return null;

                string[] commandParts = parts[i].Split('=');
                if (commandParts.Length != 2)
                    return null;

                string valuePart = commandParts[1];

                this[longFlagString] = true;
                (string value, long factor) = ExtractFactorFromValue(valuePart);
                return (byte)(byte.Parse(value) * factor);
            }

            return Byte.MinValue;
        }

        /// <summary>
        /// Get yhe trimmed value and multiplication factor from a value
        /// </summary>
        /// <param name="value">String value to treat as suffixed number</param>
        /// <returns>Trimmed value and multiplication factor</returns>
        private static (string trimmed, long factor) ExtractFactorFromValue(string value)
        {
            value = value.Trim('"');
            long factor = 1;

            // Characters
            if (value.EndsWith("c", StringComparison.Ordinal))
            {
                factor = 1;
                value = value.TrimEnd('c');
            }

            // Words
            else if (value.EndsWith("w", StringComparison.Ordinal))
            {
                factor = 2;
                value = value.TrimEnd('w');
            }

            // Double Words
            else if (value.EndsWith("d", StringComparison.Ordinal))
            {
                factor = 4;
                value = value.TrimEnd('d');
            }

            // Quad Words
            else if (value.EndsWith("q", StringComparison.Ordinal))
            {
                factor = 8;
                value = value.TrimEnd('q');
            }

            // Kilobytes
            else if (value.EndsWith("k", StringComparison.Ordinal))
            {
                factor = 1024;
                value = value.TrimEnd('k');
            }

            // Megabytes
            else if (value.EndsWith("M", StringComparison.Ordinal))
            {
                factor = 1024 * 1024;
                value = value.TrimEnd('M');
            }

            // Gigabytes
            else if (value.EndsWith("G", StringComparison.Ordinal))
            {
                factor = 1024 * 1024 * 1024;
                value = value.TrimEnd('G');
            }

            return (value, factor);
        }

        #endregion
    }
}
