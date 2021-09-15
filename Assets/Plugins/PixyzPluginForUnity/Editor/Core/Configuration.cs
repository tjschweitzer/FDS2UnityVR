using System;
using UnityEngine;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor.Profiling.Memory.Experimental;
using Pixyz.Plugin4Unity.Native;

namespace Pixyz.Plugin4Unity
{
    public enum Compatibility {
        COMPATIBLE,
        NOTCOMPATIBLE,
        UNTESTED
    }

    public class WebLicense {
        public DateTime startDate;
        public DateTime endDate;
        public string customerCompany;
        public string customerName;
        public string customerEmail;
    }

    public class LicenseServer {
        public string serverAddress = "127.0.0.1";
        public ushort serverPort = 64990;
        public bool useFlexLM = false;
    }

    public class Token {

        /// <summary>
        /// name of the token
        /// </summary>
        public readonly string name;

        /// <summary>
        /// is this token mandatory to use the plugin?
        /// </summary>
        public readonly bool mandatory;

        /// <summary>
        /// is this token owned by the user?
        /// </summary>
        public readonly bool valid;

        public Token(string name, bool mandatory, bool valid) {
            this.name = name;
            this.mandatory = mandatory;
            this.valid = valid;
        }
    }

    public static class Configuration
    {
        private static string _lastError;
        private static string lastError {
            get { return _lastError; }
            set {
                _lastError = value;
            }
        }

        private static double unityVersion;
        public static double UnityVersion
            {
            get {
                if (unityVersion == 0) {
                    string[] versionSplit = Application.unityVersion.Split('.');
                    unityVersion = double.Parse(versionSplit[0] + '.' + versionSplit[1], CultureInfo.InvariantCulture);
                }
                return unityVersion;
            }
        }

        private static string customVersionTag = "Unset";
        public static string CustomVersionTag {
            get {
                if (customVersionTag == "Unset") {
                    customVersionTag = NativeInterface.GetCustomVersionTag();
                    if (customVersionTag.ToLower() == "undefined") {
                        customVersionTag = "";
                    }
                }
                return customVersionTag;
            }
        }

        /// <summary>
        /// Get the lastError catch by this class during one of its function execution.
        /// </summary>
        /// <returns>A string containing the error message.</returns>
        public static string GetLastError()
        {
            return lastError;
        }

        /// <summary>
        /// Returns true if the current Unity version is compatible with this Pixyz for Unity plugin version
        /// </summary>
        /// <returns></returns>
        public static Compatibility IsPluginCompatible()
        {
            string message;
            return IsPluginCompatible(out message);
        }

        /// <summary>
        /// Returns true if the current Unity version is compatible with this Pixyz for Unity plugin version
        /// Also returns a message
        /// </summary>
        /// <param name="message">Compatibility message</param>
        /// <returns></returns>
        public static Compatibility IsPluginCompatible(out string message)
        {
            message = null;
            Compatibility compatibility = Compatibility.COMPATIBLE;

            if (UnityVersion > 2020.3) {
                compatibility = Compatibility.UNTESTED;
                message += $"- Pixyz Plugin for Unity {PixyzVersion} haven't been tested on Unity {UnityVersion}.\n";
            }

            if (UnityVersion < 2018.3) {
                compatibility = Compatibility.NOTCOMPATIBLE;
                message += $"- Pixyz Plugin for Unity {PixyzVersion} is not compatible with Unity {UnityVersion}.\n";
            }

            message = "Compatibility check result : " + compatibility + "\n" + message;

            return compatibility;
        }

        /// <summary>
        /// Check if the current license is valid.
        /// This function try to initialized the plugin if it's not initialized yet.
        /// </summary>
        /// <returns>true if the plugin is initialized and if a valid license was found.</returns>
        public static bool CheckLicense()
        {
            lastError = null;
            try {
                return Native.NativeInterface.CheckLicense();
            } catch (Exception exception) {
                // This is likely to be a mandatory token missing
                lastError = exception.Message;
            }
            return false;
        }

        private static string websiteLink;
        public static string WebsiteURL => !String.IsNullOrEmpty(websiteLink) ? websiteLink : (websiteLink = NativeInterface.GetPixyzWebsiteURL());

        private static string docLink;
        public static string DocumentationURL => !String.IsNullOrEmpty(docLink) ? docLink : (docLink = NativeInterface.GetProductDocumentationURL());

        private static string pixyzVersion;
        public static string PixyzVersion => !String.IsNullOrEmpty(pixyzVersion) ? pixyzVersion : (pixyzVersion = NativeInterface.GetVersion());

        private static Plugin4Unity.Native.NativeInterface.checkForUpdatesReturn? updateStatus;
        public static Plugin4Unity.Native.NativeInterface.checkForUpdatesReturn? UpdateStatus => updateStatus;

        /// <summary>
        /// Check if there is a newer version of the plugin on pixyz website.
        /// Freezes until the check is finished.
        /// It can take several seconds if the network is busy or weak.
        /// Result is cached so that further calls are fast.
        /// </summary>
        public static Plugin4Unity.Native.NativeInterface.checkForUpdatesReturn CheckForUpdate()
        {
            return (Plugin4Unity.Native.NativeInterface.checkForUpdatesReturn)(updateStatus = Plugin4Unity.Native.NativeInterface.CheckForUpdates());
        }

        #region Web License

        private static string username;
        private static string password;
        private static Native.Core.WebLicenseInfoList licenses;
        private static Native.Core.LicenseInfos currentLicense = null;

        public static string Username => username;

        /// <summary>
        /// License currently used by the plugin.
        /// Need to be connected to a web server.
        /// </summary>
        public static Native.Core.LicenseInfos CurrentLicense {
            get {
                if (currentLicense == null)
                    RetrieveCurrentLicense();
                return currentLicense;
            }
            set {
                currentLicense = value;
            }
        }

        private static bool isConnectedToWebServer = false;
        public static bool IsConnectedToWebServer() => isConnectedToWebServer;

        /// <summary>
        /// The licenses available on the web server.
        /// This list is not updated in realtime. \see @ref Configuration::RefreshAvailableLicenses.
        /// </summary>
        public static Native.Core.WebLicenseInfoList Licenses {
            get {
                if (licenses == null || licenses.list.Length == 0)
                    RefreshAvailableLicenses();
                return licenses;
            }
        }

        /// <summary>
        /// Check on the web server the available licenses.
        /// \see @ref Configuration::Licenses
        /// </summary>
        public static void RefreshAvailableLicenses()
        {
            lastError = null;
            try {
                lastError = null;
                licenses = Native.NativeInterface.RetrieveAvailableLicenses(username, password);
            } catch (Exception exception) {
                lastError = exception.Message;
            }
        }

        /// <summary>
        /// Connect to Pixyz web server using username and password.
        /// This function automatically refresh the available licenses list. \see @ref Configuration::Licenses
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>true if the connection succeed</returns>
        public static bool ConnectToWebServer(string username, string password)
        {
            lastError = null;
            Configuration.username = (username != null) ? username : "";
            Configuration.password = (password != null) ? password : "";

            RefreshAvailableLicenses();
            isConnectedToWebServer = string.IsNullOrEmpty(lastError);
            return isConnectedToWebServer;
        }

        /// <summary>
        /// Release installed license, this lets you install it on another computer.
        /// \warning This action is available only once per license.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true if the releasing succeed, else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool ReleaseWebLicense(Native.Core.WebLicenseInfo license)
        {
            lastError = null;
            try {
                Native.NativeInterface.ReleaseWebLicense(username, password, license.id);
            } catch (Exception exception) {
                lastError = exception.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Request license, this install the license on your computer.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>true if the installation succeed, else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool RequestWebLicense(Native.Core.WebLicenseInfo license)
        {
            lastError = null;
            try {
                lastError = null;
                Native.NativeInterface.RequestWebLicense(username, password, license.id);
                CurrentLicense = null;
                RetrieveTokens();
            } catch (Exception exception) {
                lastError = exception.Message;
                return false;
            }
            return true;
        }

        private static DateTime GetD(Native.Core.Date date)
        {
            return new DateTime(date.year, date.month, date.day);
        }

        /// <summary>
        /// Retrieve current valid license.
        /// </summary>
        /// <returns>true if a valid license is found, else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool RetrieveCurrentLicense()
        {
            lastError = null;
            try {
                if (Native.NativeInterface.CheckLicense()) {
                    CurrentLicense = Native.NativeInterface.GetCurrentLicenseInfos();
                } else {
                    CurrentLicense = null;
                    if (String.IsNullOrEmpty(lastError))
                        lastError = "Can't retrieve a valid license.";
                    return false;
                }
            } catch (Exception exception) {
                lastError = exception.Message;
                return false;
            }

            return true;
        }
        #endregion

        #region Offline License
        /// @name Offline License
        /// Functions and attributes needed to deal with offline licenses.
        /// @{
        /// <summary>
        /// Generate activation code to the file 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if the generation succeed else else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool GenerateActivationCode(string path)
        {
            lastError = null;
            if (path.Length == 0)
                return false;
            try {
                Plugin4Unity.Native.NativeInterface.GenerateActivationCode(path);
                return true;
            } catch (Exception exception) {
                lastError = exception.Message;
            }
            return false;
        }

        /// <summary>
        /// GenerateDeactivationCode
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if the generation succeed else else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool GenerateDeactivationCode(string path)
        {
            lastError = null;
            if (path.Length == 0)
                return false;
            try {
                Plugin4Unity.Native.NativeInterface.GenerateDeactivationCode(path);
                return true;
            } catch (Exception exception) {
                lastError = exception.Message;
            }
            return false;
        }

        /// <summary>
        /// InstallActivationCode
        /// </summary>
        /// <param name="path"></param>
        /// <returns>true if the installation succeed else else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool InstallActivationCode(string path)
        {
            lastError = null;
            if (String.IsNullOrEmpty(path))
                return false;
            if (!path.ToLower().EndsWith(".bin") && !path.ToLower().EndsWith(".lic")) {
                lastError = "The file must be an installation code (bin file) or a license file (lic file)";
                return false;
            }

            try {
                //finish install

                Plugin4Unity.Native.NativeInterface.InstallLicense(path);
                return true;
            } catch (Exception exception) {
                lastError = exception.Message;
            }
            return false;
        }
        #endregion

        #region Floating Licenses

        private static LicenseServer currentLicenseServer = null;

        /// <summary>
        /// Current floating license server settings.
        /// </summary>
        public static LicenseServer CurrentLicenseServer {
            get { if (currentLicenseServer == null) RetrieveFloatingLicensingSetup(); return currentLicenseServer; }
        }

        /// <summary>
        /// Check if the current license is a floating.
        /// </summary>
        /// <returns>true if the license is floating else return false</returns>
        public static bool IsFloatingLicense()
        {
            lastError = null;
            try {
                return NativeInterface.IsFloatingLicense();
            } catch (Exception exception) {
                lastError = exception.Message;
                return false;
            }
        }

        /// <summary>
        /// Retrieve last floating license server settings used.
        /// </summary>
        /// <returns>true if CurrentLicenseServer is updated, else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool RetrieveFloatingLicensingSetup() {
            if (!CheckLicense()) {
                return false;
            }
            lastError = null;
            //try {
            //    NativeInterface.getLicenseServerReturn ret = NativeInterface.GetLicenseServer();
            //    if (String.IsNullOrEmpty(ret.serverHost)) {
            //        if (currentLicenseServer == null)
            //            currentLicenseServer = new LicenseServer();
            //    } else {
            //        currentLicenseServer = new LicenseServer();
            //        currentLicenseServer.serverAddress = ret.serverHost;
            //        currentLicenseServer.serverPort = ret.serverPort;
            //        currentLicenseServer.useFlexLM = ret.useFlexLM;
            //    }
            //} catch (Exception exception) {
            //    lastError = exception.Message;
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// Try to connect to floating license server.
        /// </summary>
        /// <param name="address">License server IP address</param>
        /// <param name="port">License server port</param>
        /// <param name="useFlexLM">If true, use FlexLM license server</param>
        /// <returns>Returns true if connection succeed, else return false and store details (@ref Configuration::GetLastError)</returns>
        public static bool ConfigureLicenseServer(string address, ushort port, bool useFlexLM) {
            lastError = null;
            try {
                lastError = null;
                tokens = null;
                ImportSDK.Native.NativeInterface.ConfigureLicenseServer(address, port, useFlexLM);
            } catch (Exception exception) {
                lastError = exception.Message;
                return false;
            }
            RetrieveFloatingLicensingSetup();
            DisconnectFromWebServer();
            RetrieveTokens();
            return true;
        }

        public static bool DisconnectFromWebServer() {
            if (!isConnectedToWebServer) {
                return false;
            }
            isConnectedToWebServer = false;
            username = null;
            password = null;
            licenses = null;
            return true;
        }
        #endregion

        #region Tokens

        private static Token[] tokens;
        /// <summary>
        /// list of tokens known by the plugin
        /// </summary>
        public static Token[] Tokens {
            get {
                if (tokens == null)
                    RetrieveTokens();
                return tokens;
            }
        }

        /// <summary>
        /// update the Tokens list \see @ref Configuration::Tokens
        /// </summary>
        private static void RetrieveTokens()
        {
            lastError = null;
            try {
                lastError = null;
                var allTokenNames = Native.NativeInterface.GetTokens(false);
                var allTokenPossessions = new ImportSDK.Native.Core.IntList(allTokenNames.length);
                var mandatoryTokenNames = new HashSet<string>(Native.NativeInterface.GetTokens(true).list);
                try {
                    //check tokens validity
                    for (int i = 0; i < allTokenNames.list.Length; i++)
                    {
                        allTokenPossessions[i] = Native.NativeInterface.IsTokenValid(allTokenNames[i]) ? 1 : 0;
                    }
                } catch (Exception exception) {
                    lastError = exception.Message;
                }
                tokens = new Token[allTokenNames.length];
                for (int i = 0; i < allTokenNames.length; ++i) {
                    tokens[i] = new Token(allTokenNames[i], mandatoryTokenNames.Contains(allTokenNames[i]), allTokenPossessions[i] == 1);
                }
            } catch (Exception exception) {
                lastError = exception.Message;
            }
        }

        #endregion
    }
}