using System;

namespace Pixyz
{
    public class PixyzException : Exception
    {
        public PixyzException(string message) : base(message) { }
    }

    public class NoValidLicenseException : PixyzException
    {
        public NoValidLicenseException() : base("The Pixyz Plugin for Unity requires a valid license.\nPlease install yours via the License Manager or visit www.pixyz-software.com to get one") { }
    }

    public class OutOfTermsException : PixyzException
    {
        public OutOfTermsException() : base("This call doesn't respect the term and conditions of the license.\nhttps://www.pixyz-software.com/documentations/html/2020.2/plugin4unity/LicensingPolicy.html") { }
    }

    public class CoreLoadingException : PixyzException
    {
        public CoreLoadingException() : base("Impossible to load Pixyz Core. Please close your project, delete the Plugins/Pixyz folder and reinstall. Please contact the support if the issue persists.") { }
    }
}