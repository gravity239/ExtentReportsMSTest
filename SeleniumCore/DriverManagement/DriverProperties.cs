using Newtonsoft.Json.Linq;
using SeleniumCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumCore.DriverManagement
{
    /// <summary>
    ///     Contain properties for web driver.
    /// </summary>
    public class DriverProperties
    {
        private List<string> arguments;
        private Dictionary<string, object> capabilities;
        private Dictionary<string, object> userProfilePreference;
        private DriverType driverType;
        private string proKey;
        private string remoteUrl;
        private RunningMode runningMode;
        private int timeSpan;

        /// <summary>
        ///     Load driver config from json file.
        /// </summary>
        public DriverProperties(string filePath, string driver)
        {
            SetPropertyKey(driver);
            var json = JsonParser.CreateJsonObjectFromFile(filePath);
            var tokens = json.SelectToken(driver);
            SetDriverType(tokens.SelectToken("driverType").ToString());
            SetRunningMode(tokens.SelectToken("runMode").ToString());
            SetTimeSpan(Convert.ToInt32(tokens.SelectToken("timeSpan")));
            if (tokens.SelectToken("arguments") != null)
                SetArguments(tokens.SelectToken("arguments").ToObject<List<string>>());
            if (tokens.SelectToken("capabilities") != null) SetCapabilities(tokens.SelectToken("capabilities"));
            if(tokens.SelectToken("userProfilePreference") != null)

            if (IsRemoteMode())
                SetRemoteUrl(tokens.SelectToken("remoteUrl").ToString());
        }

        /// <summary>
        ///     Set web driver Capabilities.
        /// </summary>
        public void SetCapabilities(JToken caps)
        {
            if (caps != null) capabilities = caps.ToObject<Dictionary<string, object>>();
        }

        /// <summary>
        ///     Get web driver Capabilities.
        /// </summary>
        public Dictionary<string, object> GetCapabilities()
        {
            return capabilities;
        }

        /// <summary>
        ///     Get running mode.
        /// </summary>
        public RunningMode GetRunningMode()
        {
            return runningMode;
        }

        /// <summary>
        ///     Set running mode.
        /// </summary>
        public void SetRunningMode(string runningMode)
        {
            switch (runningMode.ToLower())
            {
                case "remote":
                    this.runningMode = RunningMode.Remote;
                    break;
                case "local":
                    this.runningMode = RunningMode.Local;
                    break;
            }
        }

        /// <summary>
        ///     Check if this is remote running.
        /// </summary>
        public bool IsRemoteMode()
        {
            return GetRunningMode() == RunningMode.Remote;
        }

        /// <summary>
        ///     Get selenium grid server address.
        /// </summary>
        public string GetRemoteUrl()
        {
            return remoteUrl;
        }

        /// <summary>
        ///     Set selenium grid server address.
        /// </summary>
        public void SetRemoteUrl(string remoteUrl)
        {
            this.remoteUrl = remoteUrl;
        }

        /// <summary>
        ///     get driver type.
        /// </summary>
        public DriverType GetDriverType()
        {
            return driverType;
        }

        /// <summary>
        ///     set driver type.
        /// </summary>
        public void SetDriverType(string driverType)
        {
            switch (driverType.ToLower())
            {
                case "firefox":
                    this.driverType = DriverType.Firefox;
                    break;
                case "chrome":
                    this.driverType = DriverType.Chrome;
                    break;
                case "edge":
                    this.driverType = DriverType.Edge;
                    break;
                case "ie":
                    this.driverType = DriverType.IE;
                    break;
                case "safari":
                    this.driverType = DriverType.Safari;
                    break;
                case "androidweb":
                    this.driverType = DriverType.AndroidWeb;
                    break;
                case "iosweb":
                    this.driverType = DriverType.IOSWeb;
                    break;
            }
        }

        /**
         * @return the arguments
         */
        public List<string> GetArguments()
        {
            return arguments;
        }

        /**
         * @param arguments the arguments to set
         */
        public void SetArguments(List<string> arguments)
        {
            this.arguments = arguments;
        }

        /**
        * @return the userProfilePreference
        */
        public Dictionary<string, object> GetUserProfilePreference()
        {
           return userProfilePreference; ;
        }

        /**
         * @param arguments the arguments to set
         */
        public void SetUserProfilePreferences(JToken profiles)
        {
            if (profiles != null) userProfilePreference = profiles.ToObject<Dictionary<string, object>>();
        }

        /// <summary>
        ///     Set current key for property
        /// </summary>
        /// <param name="key"></param>
        public void SetPropertyKey(string key)
        {
            proKey = key;
        }

        /// <summary>
        ///     Get current property key
        /// </summary>
        /// <returns></returns>
        public string GetPropertyKey()
        {
            return proKey;
        }

        /// <summary>
        ///     Set time span in second for property
        /// </summary>
        /// <param name="timeSpan"></param>
        public void SetTimeSpan(int timeSpan)
        {
            this.timeSpan = timeSpan;
        }

        /// <summary>
        ///     Get current timeSpan
        /// </summary>
        /// <returns></returns>
        public int GetTimeSpan()
        {
            return timeSpan;
        }
    }
}
