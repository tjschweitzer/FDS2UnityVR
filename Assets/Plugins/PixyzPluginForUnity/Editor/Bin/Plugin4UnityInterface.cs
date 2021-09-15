#pragma warning disable CA2101

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Security;

namespace Pixyz.Plugin4Unity.Native {

#region Types
namespace Core {

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class String {
		System.String value { get; set; }
		String(System.String v) { value = v; }
		public static implicit operator System.String(String self) { return self.value; }
		public static implicit operator String(System.String v) { return new String(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Bool {
		System.Boolean value { get; set; }
		Bool(System.Boolean v) { value = v; }
		public static implicit operator System.Boolean(Bool self) { return self.value; }
		public static implicit operator Bool(System.Boolean v) { return new Bool(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Int {
		System.Int32 value { get; set; }
		Int(System.Int32 v) { value = v; }
		public static implicit operator System.Int32(Int self) { return self.value; }
		public static implicit operator Int(System.Int32 v) { return new Int(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class UShort {
		System.UInt16 value { get; set; }
		UShort(System.UInt16 v) { value = v; }
		public static implicit operator System.UInt16(UShort self) { return self.value; }
		public static implicit operator UShort(System.UInt16 v) { return new UShort(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Ident {
		System.UInt32 value { get; set; }
		Ident(System.UInt32 v) { value = v; }
		public static implicit operator System.UInt32(Ident self) { return self.value; }
		public static implicit operator Ident(System.UInt32 v) { return new Ident(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Date
	{
		public Date(Date o) {
			this.year = o.year;
			this.month = o.month;
			this.day = o.day;
		}
		public System.Int32 year;
		public System.Int32 month;
		public System.Int32 day;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Date_c
	{
		internal Int32 year;
		internal Int32 month;
		internal Int32 day;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class WebLicenseInfo
	{
		public WebLicenseInfo() {}
		public WebLicenseInfo(WebLicenseInfo o) {
			this.id = o.id;
			this.product = o.product;
			this.validity = o.validity;
			this.count = o.count;
			this.inUse = o.inUse;
			this.onMachine = o.onMachine;
			this.current = o.current;
		}
		public System.UInt32 id;
		public System.String product;
		public Date validity;
		public System.Int32 count;
		public System.Int32 inUse;
		public System.Boolean onMachine;
		public System.Boolean current;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WebLicenseInfo_c
	{
		internal System.UInt32 id;
		internal IntPtr product;
		internal Date_c validity;
		internal Int32 count;
		internal Int32 inUse;
		internal Int32 onMachine;
		internal Int32 current;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class LicenseInfos
	{
		public LicenseInfos() {}
		public LicenseInfos(LicenseInfos o) {
			this.version = o.version;
			this.customerName = o.customerName;
			this.customerCompany = o.customerCompany;
			this.customerEmail = o.customerEmail;
			this.startDate = o.startDate;
			this.endDate = o.endDate;
		}
		public System.String version;
		public System.String customerName;
		public System.String customerCompany;
		public System.String customerEmail;
		public Date startDate;
		public Date endDate;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct LicenseInfos_c
	{
		internal IntPtr version;
		internal IntPtr customerName;
		internal IntPtr customerCompany;
		internal IntPtr customerEmail;
		internal Date_c startDate;
		internal Date_c endDate;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StringList {
		public System.String[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public StringList() {}
		public StringList(System.String[] tab) { list = tab; }
		public static implicit operator System.String[](StringList o) { return o.list; }
		public System.String this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public StringList(int size) { list = new System.String[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StringList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class WebLicenseInfoList {
		public WebLicenseInfo[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public WebLicenseInfoList(WebLicenseInfo[] tab) { list = tab; }
		public static implicit operator WebLicenseInfo[](WebLicenseInfoList o) { return o.list; }
		public WebLicenseInfo this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public WebLicenseInfoList(int size) { list = new WebLicenseInfo[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WebLicenseInfoList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

#endregion

	public static partial class NativeInterface {

#if !PXZ_CUSTOM_DLL_PATH
	#if PXZ_OS_LINUX
		private const string PiXYZPlugin4Unity_dll = "libPiXYZPlugin4Unity";
		private const string memcpy_dll = "libc.so.6";
	#elif PXZ_OS_WIN32
		private const string PiXYZPlugin4Unity_dll = "PiXYZPlugin4Unity";
		private const string memcpy_dll = "msvcrt.dll";
	#else
		private const string PiXYZPlugin4Unity_dll = "PiXYZPlugin4Unity_undefined_platform";
		private const string memcpy_dll = "msvcrt.dll_undefined_platform";
	#endif

#endif

		#region Conversion

	[DllImport(PiXYZPlugin4Unity_dll)]
	private static extern void Core_Date_init(ref Core.Date_c str);
	[DllImport(PiXYZPlugin4Unity_dll)]
	private static extern void Core_Date_free(ref Core.Date_c str);

	private static Core.Date ConvertValue(Core.Date_c s) {
		Core.Date ss = new Core.Date();
		ss.year = (System.Int32)s.year;
		ss.month = (System.Int32)s.month;
		ss.day = (System.Int32)s.day;
		return ss;
	}

	private static Core.Date_c ConvertValue(Core.Date s) {
		Core.Date_c ss = new Core.Date_c();
		Core_Date_init(ref ss);
		ss.year = (Int32)s.year;
		ss.month = (Int32)s.month;
		ss.day = (Int32)s.day;
		return ss;
	}

	[DllImport(PiXYZPlugin4Unity_dll)]
	private static extern void Core_WebLicenseInfo_init(ref Core.WebLicenseInfo_c str);
	[DllImport(PiXYZPlugin4Unity_dll)]
	private static extern void Core_WebLicenseInfo_free(ref Core.WebLicenseInfo_c str);

	private static Core.WebLicenseInfo ConvertValue(Core.WebLicenseInfo_c s) {
		Core.WebLicenseInfo ss = new Core.WebLicenseInfo();
		ss.id = (System.UInt32)s.id;
		ss.product = ConvertValue(s.product);
		ss.validity = ConvertValue(s.validity);
		ss.count = (System.Int32)s.count;
		ss.inUse = (System.Int32)s.inUse;
		ss.onMachine = ConvertValue(s.onMachine);
		ss.current = ConvertValue(s.current);
		return ss;
	}

	private static Core.WebLicenseInfo_c ConvertValue(Core.WebLicenseInfo s) {
		Core.WebLicenseInfo_c ss = new Core.WebLicenseInfo_c();
		Core_WebLicenseInfo_init(ref ss);
		ss.id = (System.UInt32)s.id;
		ss.product = ConvertValue(s.product);
		ss.validity = ConvertValue(s.validity);
		ss.count = (Int32)s.count;
		ss.inUse = (Int32)s.inUse;
		ss.onMachine = ConvertValue(s.onMachine);
		ss.current = ConvertValue(s.current);
		return ss;
	}

	[DllImport(PiXYZPlugin4Unity_dll)]
	private static extern void Core_LicenseInfos_init(ref Core.LicenseInfos_c str);
	[DllImport(PiXYZPlugin4Unity_dll)]
	private static extern void Core_LicenseInfos_free(ref Core.LicenseInfos_c str);

	private static Core.LicenseInfos ConvertValue(Core.LicenseInfos_c s) {
		Core.LicenseInfos ss = new Core.LicenseInfos();
		ss.version = ConvertValue(s.version);
		ss.customerName = ConvertValue(s.customerName);
		ss.customerCompany = ConvertValue(s.customerCompany);
		ss.customerEmail = ConvertValue(s.customerEmail);
		ss.startDate = ConvertValue(s.startDate);
		ss.endDate = ConvertValue(s.endDate);
		return ss;
	}

	private static Core.LicenseInfos_c ConvertValue(Core.LicenseInfos s) {
		Core.LicenseInfos_c ss = new Core.LicenseInfos_c();
		Core_LicenseInfos_init(ref ss);
		ss.version = ConvertValue(s.version);
		ss.customerName = ConvertValue(s.customerName);
		ss.customerCompany = ConvertValue(s.customerCompany);
		ss.customerEmail = ConvertValue(s.customerEmail);
		ss.startDate = ConvertValue(s.startDate);
		ss.endDate = ConvertValue(s.endDate);
		return ss;
	}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Core_StringList_init(ref Core.StringList_c list, UInt64 size);
		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Core_StringList_free(ref Core.StringList_c list);

		private static Core.StringList ConvertValue(Core.StringList_c s) {
			Core.StringList list = new Core.StringList((int)s.size);
			if (s.size==0) return list;
			IntPtr[] tab = new IntPtr[s.size];
			Marshal.Copy(s.ptr, tab, 0, (int)s.size);
			for (int i = 0; i < (int)s.size; ++i) {
				list.list[i] = ConvertValue(tab[i]);
			}
			return list;
		}

		private static Core.StringList_c ConvertValue(Core.StringList s) {
			Core.StringList_c list =  new Core.StringList_c();
			Core_StringList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			IntPtr[] tab = new IntPtr[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = ConvertValue(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Core_WebLicenseInfoList_init(ref Core.WebLicenseInfoList_c list, UInt64 size);
		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Core_WebLicenseInfoList_free(ref Core.WebLicenseInfoList_c list);

		private static Core.WebLicenseInfoList ConvertValue(Core.WebLicenseInfoList_c s) {
			Core.WebLicenseInfoList list = new Core.WebLicenseInfoList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.WebLicenseInfo_c)));
				list.list[i] = ConvertValue((Core.WebLicenseInfo_c)Marshal.PtrToStructure(p, typeof(Core.WebLicenseInfo_c)));
			}
			return list;
		}

		private static Core.WebLicenseInfoList_c ConvertValue(Core.WebLicenseInfoList s) {
			Core.WebLicenseInfoList_c list =  new Core.WebLicenseInfoList_c();
			Core_WebLicenseInfoList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Core.WebLicenseInfo_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.WebLicenseInfo_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		#endregion

		#region Base

		[DllImport(memcpy_dll, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
		private static unsafe extern void* memcpy(void* dest, void* src, ulong count);

		public static unsafe T[] CopyMemory<T>(IntPtr pointer, int length) {
			T[] managedArray = new T[length];
			GCHandle handle = GCHandle.Alloc(managedArray, GCHandleType.Pinned);
			IntPtr ptr = handle.AddrOfPinnedObject();
			void* nativePtr = pointer.ToPointer();
			memcpy(ptr.ToPointer(), nativePtr, (ulong)(length * Marshal.SizeOf(typeof(T))));
			handle.Free();
			return managedArray;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern IntPtr Plugin4Unity_getLastError();

		private static unsafe String ConvertValue(IntPtr s) {
			return new string((sbyte*)s);
		}

		private static IntPtr ConvertValue(string s) {
			return Marshal.StringToHGlobalAnsi(s);
		}

		private static bool ConvertValue(int b) {
			return (b != 0);
		}

		private static int ConvertValue(bool b) {
			return b ? 1 : 0;
		}

		#endregion

		#region 

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern Int32 Plugin4Unity_checkLicense();
		/// <summary>
		/// check the current license
		/// </summary>
		public static Core.Bool CheckLicense() {
			var ret = Plugin4Unity_checkLicense();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_configureLicenseServer(string address, System.UInt16 port, Int32 flexLM);
		/// <summary>
		/// Configure the license server to use to get floating licenses
		/// </summary>
		/// <param name="address">Server address</param>
		/// <param name="port">Server port</param>
		/// <param name="flexLM">Enable FlexLM license server</param>
		public static void ConfigureLicenseServer(Core.String address, Core.UShort port, Core.Bool flexLM) {
			Plugin4Unity_configureLicenseServer(address, port, flexLM ? 1 : 0);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_generateActivationCode(string filePath);
		/// <summary>
		/// Create an activation code to generate an offline license
		/// </summary>
		/// <param name="filePath">Path to write the activation code</param>
		public static void GenerateActivationCode(Core.String filePath) {
			Plugin4Unity_generateActivationCode(filePath);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_generateDeactivationCode(string filePath);
		/// <summary>
		/// Create an deactivation code to release the license from this machine
		/// </summary>
		/// <param name="filePath">Path to write the deactivation code</param>
		public static void GenerateDeactivationCode(Core.String filePath) {
			Plugin4Unity_generateDeactivationCode(filePath);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern Core.LicenseInfos_c Plugin4Unity_getCurrentLicenseInfos();
		/// <summary>
		/// get informations on current installed license
		/// </summary>
		public static Core.LicenseInfos GetCurrentLicenseInfos() {
			var ret = Plugin4Unity_getCurrentLicenseInfos();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_LicenseInfos_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getLicenseServerReturn_c
		{
			internal IntPtr serverHost;
			internal System.UInt16 serverPort;
			internal Int32 useFlexLM;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern getLicenseServerReturn_c Plugin4Unity_getLicenseServer();
		/// <summary>
		/// get informations on current configured license server
		/// </summary>
		public struct getLicenseServerReturn
		{
			public System.String serverHost;
			public System.UInt16 serverPort;
			public System.Boolean useFlexLM;
		}

		public static getLicenseServerReturn GetLicenseServer() {
			var ret = Plugin4Unity_getLicenseServer();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getLicenseServerReturn retStruct = new getLicenseServerReturn();
			retStruct.serverHost = ConvertValue(ret.serverHost);
			retStruct.serverPort = (Core.UShort)ret.serverPort;
			retStruct.useFlexLM = ConvertValue(ret.useFlexLM);
			return retStruct;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern Core.StringList_c Plugin4Unity_getTokens(Int32 onlyMandatory);
		/// <summary>
		/// Get the list of license tokens for this product
		/// </summary>
		/// <param name="onlyMandatory">If True, optional tokens will not be returned</param>
		public static Core.StringList GetTokens(Core.Bool onlyMandatory) {
			var ret = Plugin4Unity_getTokens(onlyMandatory ? 1 : 0);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_StringList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_initialize(Int32 isEditor);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="isEditor"></param>
		public static void Initialize(Core.Bool isEditor) {
			Plugin4Unity_initialize(isEditor ? 1 : 0);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_installLicense(string licensePath);
		/// <summary>
		/// install a new license
		/// </summary>
		/// <param name="licensePath">Path of the license file</param>
		public static void InstallLicense(Core.String licensePath) {
			Plugin4Unity_installLicense(licensePath);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern Int32 Plugin4Unity_isFloatingLicense();
		/// <summary>
		/// Tells if license is floating
		/// </summary>
		public static Core.Bool IsFloatingLicense() {
			var ret = Plugin4Unity_isFloatingLicense();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern Int32 Plugin4Unity_isTokenValid(string tokenName);
		/// <summary>
		/// Returns True if a token is owned by the product
		/// </summary>
		/// <param name="tokenName">Token name</param>
		public static Core.Bool IsTokenValid(Core.String tokenName) {
			var ret = Plugin4Unity_isTokenValid(tokenName);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_releaseWebLicense(string login, string password, System.UInt32 id);
		/// <summary>
		/// release License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		/// <param name="id">WEB license id</param>
		public static void ReleaseWebLicense(Core.String login, Core.String password, Core.Ident id) {
			Plugin4Unity_releaseWebLicense(login, password, id);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern void Plugin4Unity_requestWebLicense(string login, string password, System.UInt32 id);
		/// <summary>
		/// request License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		/// <param name="id">WEB license id</param>
		public static void RequestWebLicense(Core.String login, Core.String password, Core.Ident id) {
			Plugin4Unity_requestWebLicense(login, password, id);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern Core.WebLicenseInfoList_c Plugin4Unity_retrieveAvailableLicenses(string login, string password);
		/// <summary>
		/// Retrieves License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		public static Core.WebLicenseInfoList RetrieveAvailableLicenses(Core.String login, Core.String password) {
			var ret = Plugin4Unity_retrieveAvailableLicenses(login, password);
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_WebLicenseInfoList_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct checkForUpdatesReturn_c
		{
			internal Int32 newVersionAvailable;
			internal IntPtr newVersion;
			internal IntPtr newVersionLink;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern checkForUpdatesReturn_c Plugin4Unity_checkForUpdates();
		/// <summary>
		/// check for software update
		/// </summary>
		public struct checkForUpdatesReturn
		{
			public System.Boolean newVersionAvailable;
			public System.String newVersion;
			public System.String newVersionLink;
		}

		public static checkForUpdatesReturn CheckForUpdates() {
			var ret = Plugin4Unity_checkForUpdates();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			checkForUpdatesReturn retStruct = new checkForUpdatesReturn();
			retStruct.newVersionAvailable = ConvertValue(ret.newVersionAvailable);
			retStruct.newVersion = ConvertValue(ret.newVersion);
			retStruct.newVersionLink = ConvertValue(ret.newVersionLink);
			return retStruct;
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern IntPtr Plugin4Unity_getCustomVersionTag();
		/// <summary>
		/// get the Pixyz custom version tag
		/// </summary>
		public static Core.String GetCustomVersionTag() {
			var ret = Plugin4Unity_getCustomVersionTag();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern IntPtr Plugin4Unity_getPixyzWebsiteURL();
		/// <summary>
		/// get the Pixyz website URL
		/// </summary>
		public static Core.String GetPixyzWebsiteURL() {
			var ret = Plugin4Unity_getPixyzWebsiteURL();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern IntPtr Plugin4Unity_getProductDocumentationURL();
		/// <summary>
		/// get the product documentation URL
		/// </summary>
		public static Core.String GetProductDocumentationURL() {
			var ret = Plugin4Unity_getProductDocumentationURL();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZPlugin4Unity_dll)]
		private static extern IntPtr Plugin4Unity_getVersion();
		/// <summary>
		/// get the Pixyz product version
		/// </summary>
		public static Core.String GetVersion() {
			var ret = Plugin4Unity_getVersion();
			System.String err = ConvertValue(Plugin4Unity_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		#endregion

	}
}
