#pragma warning disable CA2101

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Security;

namespace Pixyz.ImportSDK.Native {

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
	public class Ident {
		System.UInt32 value { get; set; }
		Ident(System.UInt32 v) { value = v; }
		public static implicit operator System.UInt32(Ident self) { return self.value; }
		public static implicit operator Ident(System.UInt32 v) { return new Ident(v); }
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
	public class UShort {
		System.UInt16 value { get; set; }
		UShort(System.UInt16 v) { value = v; }
		public static implicit operator System.UInt16(UShort self) { return self.value; }
		public static implicit operator UShort(System.UInt16 v) { return new UShort(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Byte {
		System.Byte value { get; set; }
		Byte(System.Byte v) { value = v; }
		public static implicit operator System.Byte(Byte self) { return self.value; }
		public static implicit operator Byte(System.Byte v) { return new Byte(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Double {
		System.Double value { get; set; }
		Double(System.Double v) { value = v; }
		public static implicit operator System.Double(Double self) { return self.value; }
		public static implicit operator Double(System.Double v) { return new Double(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorAlpha
	{
		public ColorAlpha(ColorAlpha o) {
			this.r = o.r;
			this.g = o.g;
			this.b = o.b;
			this.a = o.a;
		}
		public System.Double r;
		public System.Double g;
		public System.Double b;
		public System.Double a;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ColorAlpha_c
	{
		internal System.Double r;
		internal System.Double g;
		internal System.Double b;
		internal System.Double a;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ByteList {
		public System.Byte[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ByteList() {}
		public ByteList(System.Byte[] tab) { list = tab; }
		public static implicit operator System.Byte[](ByteList o) { return o.list; }
		public System.Byte this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ByteList(int size) { list = new System.Byte[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ByteList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Color
	{
		public Color(Color o) {
			this.r = o.r;
			this.g = o.g;
			this.b = o.b;
		}
		public System.Double r;
		public System.Double g;
		public System.Double b;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Color_c
	{
		internal System.Double r;
		internal System.Double g;
		internal System.Double b;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class DoubleList {
		public System.Double[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public DoubleList() {}
		public DoubleList(System.Double[] tab) { list = tab; }
		public static implicit operator System.Double[](DoubleList o) { return o.list; }
		public System.Double this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public DoubleList(int size) { list = new System.Double[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DoubleList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class DoubleListList {
		public DoubleList[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public DoubleListList(DoubleList[] tab) { list = tab; }
		public static implicit operator DoubleList[](DoubleListList o) { return o.list; }
		public DoubleList this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public DoubleListList(int size) { list = new DoubleList[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DoubleListList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class IntList {
		public System.Int32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public IntList() {}
		public IntList(System.Int32[] tab) { list = tab; }
		public static implicit operator System.Int32[](IntList o) { return o.list; }
		public System.Int32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public IntList(int size) { list = new System.Int32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct IntList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BoolList {
		public System.Boolean[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public BoolList() {}
		public BoolList(System.Boolean[] tab) { list = tab; }
		public static implicit operator System.Boolean[](BoolList o) { return o.list; }
		public System.Boolean this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public BoolList(int size) { list = new System.Boolean[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BoolList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	public enum InheritableBool
	{
		False = 0,
		True = 1,
		Inherited = 2,
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct StringPair
	{
		public StringPair(StringPair o) {
			this.key = o.key;
			this.value = o.value;
		}
		public System.String key;
		public System.String value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StringPair_c
	{
		internal IntPtr key;
		internal IntPtr value;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StringPairList {
		public StringPair[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public StringPairList(StringPair[] tab) { list = tab; }
		public static implicit operator StringPair[](StringPairList o) { return o.list; }
		public StringPair this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public StringPairList(int size) { list = new StringPair[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StringPairList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StringPairListList {
		public StringPairList[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public StringPairListList(StringPairList[] tab) { list = tab; }
		public static implicit operator StringPairList[](StringPairListList o) { return o.list; }
		public StringPairList this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public StringPairListList(int size) { list = new StringPairList[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StringPairListList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class InheritableBoolList {
		public InheritableBool[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public InheritableBoolList(InheritableBool[] tab) { list = tab; }
		public static implicit operator InheritableBool[](InheritableBoolList o) { return o.list; }
		public InheritableBool this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public InheritableBoolList(int size) { list = new InheritableBool[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct InheritableBoolList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ColorAlphaList {
		public ColorAlpha[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ColorAlphaList(ColorAlpha[] tab) { list = tab; }
		public static implicit operator ColorAlpha[](ColorAlphaList o) { return o.list; }
		public ColorAlpha this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ColorAlphaList(int size) { list = new ColorAlpha[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ColorAlphaList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

namespace Geom {

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Array4
	{
		public Double[] tab = new Double[4];
		public Array4() {}
		public Array4(Double[] t) { tab = t; }
		public static implicit operator Double[](Array4 o) { return o.tab; }
		public Double this[int index] {
			get{ return tab[index]; }
			set{ tab[index] = value; }
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Array4_c
	{
		internal IntPtr tab;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Point2
	{
		public Point2(Point2 o) {
			this.x = o.x;
			this.y = o.y;
		}
		public System.Double x;
		public System.Double y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point2_c
	{
		internal System.Double x;
		internal System.Double y;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Point3
	{
		public Point3(Point3 o) {
			this.x = o.x;
			this.y = o.y;
			this.z = o.z;
		}
		public System.Double x;
		public System.Double y;
		public System.Double z;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point3_c
	{
		internal System.Double x;
		internal System.Double y;
		internal System.Double z;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3 {
		public Vector3(Point3 value) { this._base = value; }
		public static implicit operator Point3(Vector3 v) { return v._base; }
		public static implicit operator Vector3(Point3 v) { return new Vector3(v); }
		public Point3 _base;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Curvatures
	{
		public Curvatures() {}
		public Curvatures(Curvatures o) {
			this.k1 = o.k1;
			this.k2 = o.k2;
			this.v1 = o.v1;
			this.v2 = o.v2;
		}
		public System.Double k1;
		public System.Double k2;
		public Point3 v1;
		public Point3 v2;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Curvatures_c
	{
		internal System.Double k1;
		internal System.Double k2;
		internal Point3_c v1;
		internal Point3_c v2;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Point3List {
		public Point3[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Point3List(Point3[] tab) { list = tab; }
		public static implicit operator Point3[](Point3List o) { return o.list; }
		public Point3 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Point3List(int size) { list = new Point3[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point3List_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Point3ListList {
		public Point3List[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Point3ListList(Point3List[] tab) { list = tab; }
		public static implicit operator Point3List[](Point3ListList o) { return o.list; }
		public Point3List this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Point3ListList(int size) { list = new Point3List[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point3ListList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Point2List {
		public Point2[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Point2List(Point2[] tab) { list = tab; }
		public static implicit operator Point2[](Point2List o) { return o.list; }
		public Point2 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Point2List(int size) { list = new Point2[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point2List_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Point2ListList {
		public Point2List[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Point2ListList(Point2List[] tab) { list = tab; }
		public static implicit operator Point2List[](Point2ListList o) { return o.list; }
		public Point2List this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Point2ListList(int size) { list = new Point2List[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point2ListList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Matrix4
	{
		public Array4[] tab = new Array4[4];
		public Matrix4() {}
		public Matrix4(Array4[] t) { tab = t; }
		public static implicit operator Array4[](Matrix4 o) { return o.tab; }
		public Array4 this[int index] {
			get{ return tab[index]; }
			set{ tab[index] = value; }
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix4_c
	{
		internal IntPtr tab;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class AABB
	{
		public AABB() {}
		public AABB(AABB o) {
			this.low = o.low;
			this.high = o.high;
		}
		public Point3 low;
		public Point3 high;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct AABB_c
	{
		internal Point3_c low;
		internal Point3_c high;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Matrix4List {
		public Matrix4[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Matrix4List(Matrix4[] tab) { list = tab; }
		public static implicit operator Matrix4[](Matrix4List o) { return o.list; }
		public Matrix4 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Matrix4List(int size) { list = new Matrix4[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Matrix4List_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Point4
	{
		public Point4(Point4 o) {
			this.x = o.x;
			this.y = o.y;
			this.z = o.z;
			this.w = o.w;
		}
		public System.Double x;
		public System.Double y;
		public System.Double z;
		public System.Double w;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Point4_c
	{
		internal System.Double x;
		internal System.Double y;
		internal System.Double z;
		internal System.Double w;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4 {
		public Vector4(Point4 value) { this._base = value; }
		public static implicit operator Point4(Vector4 v) { return v._base; }
		public static implicit operator Vector4(Point4 v) { return new Vector4(v); }
		public Point4 _base;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Vector4List {
		public Vector4[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Vector4List(Vector4[] tab) { list = tab; }
		public static implicit operator Vector4[](Vector4List o) { return o.list; }
		public Vector4 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Vector4List(int size) { list = new Vector4[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4List_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4I
	{
		public Vector4I(Vector4I o) {
			this.x = o.x;
			this.y = o.y;
			this.z = o.z;
			this.w = o.w;
		}
		public System.Int32 x;
		public System.Int32 y;
		public System.Int32 z;
		public System.Int32 w;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4I_c
	{
		internal Int32 x;
		internal Int32 y;
		internal Int32 z;
		internal Int32 w;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Vector4IList {
		public Vector4I[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Vector4IList(Vector4I[] tab) { list = tab; }
		public static implicit operator Vector4I[](Vector4IList o) { return o.list; }
		public Vector4I this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Vector4IList(int size) { list = new Vector4I[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector4IList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Vector3List {
		public Vector3[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public Vector3List(Vector3[] tab) { list = tab; }
		public static implicit operator Vector3[](Vector3List o) { return o.list; }
		public Vector3 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public Vector3List(int size) { list = new Vector3[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vector3List_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class CurvaturesList {
		public Curvatures[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public CurvaturesList(Curvatures[] tab) { list = tab; }
		public static implicit operator Curvatures[](CurvaturesList o) { return o.list; }
		public Curvatures this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public CurvaturesList(int size) { list = new Curvatures[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CurvaturesList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

namespace Material {

	public enum MaterialPatternType
	{
		CUSTOM = 0,
		COLOR = 1,
		STANDARD = 2,
		UNLIT_TEXTURE = 3,
		PBR = 4,
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Texture
	{
		public Texture() {}
		public Texture(Texture o) {
			this.image = o.image;
			this.channel = o.channel;
			this.offset = o.offset;
			this.tilling = o.tilling;
		}
		public System.UInt32 image;
		public System.Int32 channel;
		public Geom.Point2 offset;
		public Geom.Point2 tilling;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Texture_c
	{
		internal System.UInt32 image;
		internal Int32 channel;
		internal Geom.Point2_c offset;
		internal Geom.Point2_c tilling;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct CoeffOrTexture
	{
		public enum Type
		{
			UNKNOWN = 0,
			COEFF = 1,
			TEXTURE = 2,
		}
		public System.Double coeff;
		public Texture texture;
		public Type _type;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CoeffOrTexture_c
	{
		internal System.Double coeff;
		internal Texture_c texture;
		internal int _type;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct ColorOrTexture
	{
		public enum Type
		{
			UNKNOWN = 0,
			COLOR = 1,
			TEXTURE = 2,
		}
		public Core.Color color;
		public Texture texture;
		public Type _type;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ColorOrTexture_c
	{
		internal Core.Color_c color;
		internal Texture_c texture;
		internal int _type;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StandardMaterialInfos
	{
		public StandardMaterialInfos() {}
		public StandardMaterialInfos(StandardMaterialInfos o) {
			this.name = o.name;
			this.diffuse = o.diffuse;
			this.specular = o.specular;
			this.ambient = o.ambient;
			this.emissive = o.emissive;
			this.shininess = o.shininess;
			this.transparency = o.transparency;
		}
		public System.String name;
		public ColorOrTexture diffuse;
		public ColorOrTexture specular;
		public ColorOrTexture ambient;
		public ColorOrTexture emissive;
		public System.Double shininess;
		public System.Double transparency;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StandardMaterialInfos_c
	{
		internal IntPtr name;
		internal ColorOrTexture_c diffuse;
		internal ColorOrTexture_c specular;
		internal ColorOrTexture_c ambient;
		internal ColorOrTexture_c emissive;
		internal System.Double shininess;
		internal System.Double transparency;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ColorMaterialInfos
	{
		public ColorMaterialInfos() {}
		public ColorMaterialInfos(ColorMaterialInfos o) {
			this.name = o.name;
			this.color = o.color;
		}
		public System.String name;
		public Core.ColorAlpha color;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ColorMaterialInfos_c
	{
		internal IntPtr name;
		internal Core.ColorAlpha_c color;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ImageDefinition
	{
		public ImageDefinition() {}
		public ImageDefinition(ImageDefinition o) {
			this.id = o.id;
			this.name = o.name;
			this.width = o.width;
			this.height = o.height;
			this.bitsPerComponent = o.bitsPerComponent;
			this.componentsCount = o.componentsCount;
			this.data = o.data;
		}
		public System.UInt32 id;
		public System.String name;
		public System.Int32 width;
		public System.Int32 height;
		public System.Int32 bitsPerComponent;
		public System.Int32 componentsCount;
		public Core.ByteList data;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImageDefinition_c
	{
		internal System.UInt32 id;
		internal IntPtr name;
		internal Int32 width;
		internal Int32 height;
		internal Int32 bitsPerComponent;
		internal Int32 componentsCount;
		internal Core.ByteList_c data;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ImageDefinitionList {
		public ImageDefinition[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ImageDefinitionList(ImageDefinition[] tab) { list = tab; }
		public static implicit operator ImageDefinition[](ImageDefinitionList o) { return o.list; }
		public ImageDefinition this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ImageDefinitionList(int size) { list = new ImageDefinition[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImageDefinitionList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MaterialList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MaterialList() {}
		public MaterialList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](MaterialList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MaterialList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MaterialList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ImageList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ImageList() {}
		public ImageList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](ImageList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ImageList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ImageList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class PBRMaterialInfos
	{
		public PBRMaterialInfos() {}
		public PBRMaterialInfos(PBRMaterialInfos o) {
			this.name = o.name;
			this.albedo = o.albedo;
			this.normal = o.normal;
			this.metallic = o.metallic;
			this.roughness = o.roughness;
			this.ao = o.ao;
			this.opacity = o.opacity;
		}
		public System.String name;
		public ColorOrTexture albedo;
		public ColorOrTexture normal;
		public CoeffOrTexture metallic;
		public CoeffOrTexture roughness;
		public CoeffOrTexture ao;
		public CoeffOrTexture opacity;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PBRMaterialInfos_c
	{
		internal IntPtr name;
		internal ColorOrTexture_c albedo;
		internal ColorOrTexture_c normal;
		internal CoeffOrTexture_c metallic;
		internal CoeffOrTexture_c roughness;
		internal CoeffOrTexture_c ao;
		internal CoeffOrTexture_c opacity;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class UnlitTextureMaterialInfos
	{
		public UnlitTextureMaterialInfos() {}
		public UnlitTextureMaterialInfos(UnlitTextureMaterialInfos o) {
			this.name = o.name;
			this.texture = o.texture;
		}
		public System.String name;
		public Texture texture;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct UnlitTextureMaterialInfos_c
	{
		internal IntPtr name;
		internal Texture_c texture;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MaterialDefinition
	{
		public MaterialDefinition() {}
		public MaterialDefinition(MaterialDefinition o) {
			this.name = o.name;
			this.id = o.id;
			this.albedo = o.albedo;
			this.normal = o.normal;
			this.metallic = o.metallic;
			this.roughness = o.roughness;
			this.ao = o.ao;
			this.opacity = o.opacity;
		}
		public System.String name;
		public System.UInt32 id;
		public ColorOrTexture albedo;
		public ColorOrTexture normal;
		public CoeffOrTexture metallic;
		public CoeffOrTexture roughness;
		public CoeffOrTexture ao;
		public CoeffOrTexture opacity;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MaterialDefinition_c
	{
		internal IntPtr name;
		internal System.UInt32 id;
		internal ColorOrTexture_c albedo;
		internal ColorOrTexture_c normal;
		internal CoeffOrTexture_c metallic;
		internal CoeffOrTexture_c roughness;
		internal CoeffOrTexture_c ao;
		internal CoeffOrTexture_c opacity;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MaterialDefinitionList {
		public MaterialDefinition[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MaterialDefinitionList(MaterialDefinition[] tab) { list = tab; }
		public static implicit operator MaterialDefinition[](MaterialDefinitionList o) { return o.list; }
		public MaterialDefinition this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MaterialDefinitionList(int size) { list = new MaterialDefinition[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MaterialDefinitionList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

namespace Polygonal {

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MeshList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MeshList() {}
		public MeshList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](MeshList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MeshList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MeshList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	public enum StyleType
	{
		NORMAL = 0,
		STIPPLE = 1,
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class JointList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public JointList() {}
		public JointList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](JointList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public JointList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct JointList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct DressedPoly
	{
		public DressedPoly(DressedPoly o) {
			this.material = o.material;
			this.firstTri = o.firstTri;
			this.triCount = o.triCount;
			this.firstQuad = o.firstQuad;
			this.quadCount = o.quadCount;
			this.externalId = o.externalId;
		}
		public System.UInt32 material;
		public System.Int32 firstTri;
		public System.Int32 triCount;
		public System.Int32 firstQuad;
		public System.Int32 quadCount;
		public System.UInt32 externalId;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DressedPoly_c
	{
		internal System.UInt32 material;
		internal Int32 firstTri;
		internal Int32 triCount;
		internal Int32 firstQuad;
		internal Int32 quadCount;
		internal System.UInt32 externalId;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class DressedPolyList {
		public DressedPoly[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public DressedPolyList(DressedPoly[] tab) { list = tab; }
		public static implicit operator DressedPoly[](DressedPolyList o) { return o.list; }
		public DressedPoly this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public DressedPolyList(int size) { list = new DressedPoly[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DressedPolyList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StylizedLine
	{
		public StylizedLine() {}
		public StylizedLine(StylizedLine o) {
			this.lines = o.lines;
			this.width = o.width;
			this.type = o.type;
			this.pattern = o.pattern;
			this.color = o.color;
			this.externalId = o.externalId;
		}
		public Core.IntList lines;
		public System.Double width;
		public StyleType type;
		public System.Int32 pattern;
		public Core.ColorAlpha color;
		public System.UInt32 externalId;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StylizedLine_c
	{
		internal Core.IntList_c lines;
		internal System.Double width;
		internal Int32 type;
		internal Int32 pattern;
		internal Core.ColorAlpha_c color;
		internal System.UInt32 externalId;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class StylizedLineList {
		public StylizedLine[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public StylizedLineList(StylizedLine[] tab) { list = tab; }
		public static implicit operator StylizedLine[](StylizedLineList o) { return o.list; }
		public StylizedLine this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public StylizedLineList(int size) { list = new StylizedLine[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct StylizedLineList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MeshDefinition
	{
		public MeshDefinition() {}
		public MeshDefinition(MeshDefinition o) {
			this.id = o.id;
			this.vertices = o.vertices;
			this.normals = o.normals;
			this.tangents = o.tangents;
			this.uvChannels = o.uvChannels;
			this.uvs = o.uvs;
			this.vertexColors = o.vertexColors;
			this.curvatures = o.curvatures;
			this.triangles = o.triangles;
			this.quadrangles = o.quadrangles;
			this.vertexMerged = o.vertexMerged;
			this.dressedPolys = o.dressedPolys;
			this.linesVertices = o.linesVertices;
			this.lines = o.lines;
			this.points = o.points;
			this.pointsColors = o.pointsColors;
			this.joints = o.joints;
			this.inverseBindMatrices = o.inverseBindMatrices;
			this.jointWeights = o.jointWeights;
			this.jointIndices = o.jointIndices;
		}
		public System.UInt32 id;
		public Geom.Point3List vertices;
		public Geom.Vector3List normals;
		public Geom.Vector4List tangents;
		public Core.IntList uvChannels;
		public Geom.Point2ListList uvs;
		public Core.ColorAlphaList vertexColors;
		public Geom.CurvaturesList curvatures;
		public Core.IntList triangles;
		public Core.IntList quadrangles;
		public Core.IntList vertexMerged;
		public DressedPolyList dressedPolys;
		public Geom.Point3List linesVertices;
		public StylizedLineList lines;
		public Geom.Point3List points;
		public Geom.Vector3List pointsColors;
		public JointList joints;
		public Geom.Matrix4List inverseBindMatrices;
		public Geom.Vector4List jointWeights;
		public Geom.Vector4IList jointIndices;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MeshDefinition_c
	{
		internal System.UInt32 id;
		internal Geom.Point3List_c vertices;
		internal Geom.Vector3List_c normals;
		internal Geom.Vector4List_c tangents;
		internal Core.IntList_c uvChannels;
		internal Geom.Point2ListList_c uvs;
		internal Core.ColorAlphaList_c vertexColors;
		internal Geom.CurvaturesList_c curvatures;
		internal Core.IntList_c triangles;
		internal Core.IntList_c quadrangles;
		internal Core.IntList_c vertexMerged;
		internal DressedPolyList_c dressedPolys;
		internal Geom.Point3List_c linesVertices;
		internal StylizedLineList_c lines;
		internal Geom.Point3List_c points;
		internal Geom.Vector3List_c pointsColors;
		internal JointList_c joints;
		internal Geom.Matrix4List_c inverseBindMatrices;
		internal Geom.Vector4List_c jointWeights;
		internal Geom.Vector4IList_c jointIndices;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MeshDefinitionList {
		public MeshDefinition[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MeshDefinitionList(MeshDefinition[] tab) { list = tab; }
		public static implicit operator MeshDefinition[](MeshDefinitionList o) { return o.list; }
		public MeshDefinition this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MeshDefinitionList(int size) { list = new MeshDefinition[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MeshDefinitionList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

namespace CAD {

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct OrientedDomain
	{
		public OrientedDomain(OrientedDomain o) {
			this.domain = o.domain;
			this.orientation = o.orientation;
		}
		public System.UInt32 domain;
		public System.Boolean orientation;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct OrientedDomain_c
	{
		internal System.UInt32 domain;
		internal Int32 orientation;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class OrientedDomainList {
		public OrientedDomain[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public OrientedDomainList(OrientedDomain[] tab) { list = tab; }
		public static implicit operator OrientedDomain[](OrientedDomainList o) { return o.list; }
		public OrientedDomain this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public OrientedDomainList(int size) { list = new OrientedDomain[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct OrientedDomainList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ClosedShellList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ClosedShellList() {}
		public ClosedShellList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](ClosedShellList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ClosedShellList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ClosedShellList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class DomainList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public DomainList() {}
		public DomainList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](DomainList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public DomainList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct DomainList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class FaceList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public FaceList() {}
		public FaceList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](FaceList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public FaceList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FaceList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class EdgeList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public EdgeList() {}
		public EdgeList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](EdgeList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public EdgeList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EdgeList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class EdgeListList {
		public EdgeList[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public EdgeListList(EdgeList[] tab) { list = tab; }
		public static implicit operator EdgeList[](EdgeListList o) { return o.list; }
		public EdgeList this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public EdgeListList(int size) { list = new EdgeList[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EdgeListList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class CoEdgeList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public CoEdgeList() {}
		public CoEdgeList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](CoEdgeList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public CoEdgeList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct CoEdgeList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct Bounds1D
	{
		public Bounds1D(Bounds1D o) {
			this.min = o.min;
			this.max = o.max;
		}
		public System.Double min;
		public System.Double max;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Bounds1D_c
	{
		internal System.Double min;
		internal System.Double max;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class BodyList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public BodyList() {}
		public BodyList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](BodyList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public BodyList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct BodyList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Bounds2D
	{
		public Bounds2D() {}
		public Bounds2D(Bounds2D o) {
			this.u = o.u;
			this.v = o.v;
		}
		public Bounds1D u;
		public Bounds1D v;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Bounds2D_c
	{
		internal Bounds1D_c u;
		internal Bounds1D_c v;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class LoopList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public LoopList() {}
		public LoopList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](LoopList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public LoopList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct LoopList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class LimitedCurveList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public LimitedCurveList() {}
		public LimitedCurveList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](LimitedCurveList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public LimitedCurveList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct LimitedCurveList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ModelList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ModelList() {}
		public ModelList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](ModelList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ModelList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ModelList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

namespace Scene {

	public enum MergeHiddenPartsMode
	{
		Destroy = 0,
		MakeVisible = 1,
		MergeSeparately = 2,
	}

	public enum ComponentType
	{
		Part = 0,
		PMI = 1,
		Light = 2,
		VisualBehavior = 3,
		InteractionBehavior = 4,
		Metadata = 5,
		Variant = 6,
		Animation = 7,
		Joint = 8,
		Widget = 9,
	}

	public enum VisibilityMode
	{
		Inherited = 0,
		Hide = 1,
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class OccurrenceList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public OccurrenceList() {}
		public OccurrenceList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](OccurrenceList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public OccurrenceList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct OccurrenceList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct PropertyValue
	{
		public PropertyValue(PropertyValue o) {
			this.name = o.name;
			this.value = o.value;
		}
		public System.String name;
		public System.String value;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PropertyValue_c
	{
		internal IntPtr name;
		internal IntPtr value;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ComponentList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ComponentList() {}
		public ComponentList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](ComponentList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ComponentList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ComponentList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class PartList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public PartList() {}
		public PartList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](PartList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public PartList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PartList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class PackedTree
	{
		public PackedTree() {}
		public PackedTree(PackedTree o) {
			this.occurrences = o.occurrences;
			this.parents = o.parents;
			this.names = o.names;
			this.visibles = o.visibles;
			this.materials = o.materials;
			this.transformIndices = o.transformIndices;
			this.transformMatrices = o.transformMatrices;
			this.customProperties = o.customProperties;
		}
		public OccurrenceList occurrences;
		public Core.IntList parents;
		public Core.StringList names;
		public Core.InheritableBoolList visibles;
		public Material.MaterialList materials;
		public Core.IntList transformIndices;
		public Geom.Matrix4List transformMatrices;
		public Core.StringPairListList customProperties;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PackedTree_c
	{
		internal OccurrenceList_c occurrences;
		internal Core.IntList_c parents;
		internal Core.StringList_c names;
		internal Core.InheritableBoolList_c visibles;
		internal Material.MaterialList_c materials;
		internal Core.IntList_c transformIndices;
		internal Geom.Matrix4List_c transformMatrices;
		internal Core.StringPairListList_c customProperties;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class PropertyValueList {
		public PropertyValue[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public PropertyValueList(PropertyValue[] tab) { list = tab; }
		public static implicit operator PropertyValue[](PropertyValueList o) { return o.list; }
		public PropertyValue this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public PropertyValueList(int size) { list = new PropertyValue[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PropertyValueList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public struct MetadataDefinition {
		public MetadataDefinition(PropertyValueList value) { this._base = value; }
		public static implicit operator PropertyValueList(MetadataDefinition v) { return v._base; }
		public static implicit operator MetadataDefinition(PropertyValueList v) { return new MetadataDefinition(v); }
		public PropertyValueList _base;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MetadataDefinitionList {
		public MetadataDefinition[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MetadataDefinitionList(MetadataDefinition[] tab) { list = tab; }
		public static implicit operator MetadataDefinition[](MetadataDefinitionList o) { return o.list; }
		public MetadataDefinition this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MetadataDefinitionList(int size) { list = new MetadataDefinition[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MetadataDefinitionList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MetadataList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MetadataList() {}
		public MetadataList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](MetadataList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MetadataList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MetadataList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

namespace Algo {

	public enum SelectionLevel
	{
		Parts = 0,
		Patches = 1,
		Polygons = 2,
	}

	public enum SmartHiddenType
	{
		All = 0,
		OnlyOuter = 1,
		OnlyInners = 2,
	}

	public enum UVGenerationMode
	{
		NoUV = 0,
		FastUV = 1,
		UniformUV = 2,
	}

}

namespace IO {

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class Format
	{
		public Format() {}
		public Format(Format o) {
			this.name = o.name;
			this.extensions = o.extensions;
		}
		public System.String name;
		public Core.StringList extensions;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Format_c
	{
		internal IntPtr name;
		internal Core.StringList_c extensions;
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class FormatList {
		public Format[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public FormatList(Format[] tab) { list = tab; }
		public static implicit operator Format[](FormatList o) { return o.list; }
		public Format this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public FormatList(int size) { list = new Format[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct FormatList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

#endregion

	public static partial class NativeInterface {

#if !PXZ_CUSTOM_DLL_PATH
	#if PXZ_OS_LINUX
		private const string PiXYZImportSDK_dll = "libPiXYZImportSDK";
		private const string memcpy_dll = "libc.so.6";
	#elif PXZ_OS_WIN32
		private const string PiXYZImportSDK_dll = "PiXYZImportSDK";
		private const string memcpy_dll = "msvcrt.dll";
	#else
		private const string PiXYZImportSDK_dll = "PiXYZImportSDK_undefined_platform";
		private const string memcpy_dll = "msvcrt.dll_undefined_platform";
	#endif

#endif

		#region Conversion

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void CAD_OrientedDomain_init(ref CAD.OrientedDomain_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void CAD_OrientedDomain_free(ref CAD.OrientedDomain_c str);

	private static CAD.OrientedDomain ConvertValue(CAD.OrientedDomain_c s) {
		CAD.OrientedDomain ss = new CAD.OrientedDomain();
		ss.domain = (System.UInt32)s.domain;
		ss.orientation = ConvertValue(s.orientation);
		return ss;
	}

	private static CAD.OrientedDomain_c ConvertValue(CAD.OrientedDomain s) {
		CAD.OrientedDomain_c ss = new CAD.OrientedDomain_c();
		CAD_OrientedDomain_init(ref ss);
		ss.domain = (System.UInt32)s.domain;
		ss.orientation = ConvertValue(s.orientation);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_OrientedDomainList_init(ref CAD.OrientedDomainList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_OrientedDomainList_free(ref CAD.OrientedDomainList_c list);

		private static CAD.OrientedDomainList ConvertValue(CAD.OrientedDomainList_c s) {
			CAD.OrientedDomainList list = new CAD.OrientedDomainList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<CAD.OrientedDomain>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.OrientedDomainList_c ConvertValue(CAD.OrientedDomainList s) {
			CAD.OrientedDomainList_c list =  new CAD.OrientedDomainList_c();
			CAD_OrientedDomainList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				CAD.OrientedDomain_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(CAD.OrientedDomain_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_ClosedShellList_init(ref CAD.ClosedShellList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_ClosedShellList_free(ref CAD.ClosedShellList_c list);

		private static CAD.ClosedShellList ConvertValue(CAD.ClosedShellList_c s) {
			CAD.ClosedShellList list = new CAD.ClosedShellList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.ClosedShellList_c ConvertValue(CAD.ClosedShellList s) {
			CAD.ClosedShellList_c list =  new CAD.ClosedShellList_c();
			CAD_ClosedShellList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_DomainList_init(ref CAD.DomainList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_DomainList_free(ref CAD.DomainList_c list);

		private static CAD.DomainList ConvertValue(CAD.DomainList_c s) {
			CAD.DomainList list = new CAD.DomainList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.DomainList_c ConvertValue(CAD.DomainList s) {
			CAD.DomainList_c list =  new CAD.DomainList_c();
			CAD_DomainList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_FaceList_init(ref CAD.FaceList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_FaceList_free(ref CAD.FaceList_c list);

		private static CAD.FaceList ConvertValue(CAD.FaceList_c s) {
			CAD.FaceList list = new CAD.FaceList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.FaceList_c ConvertValue(CAD.FaceList s) {
			CAD.FaceList_c list =  new CAD.FaceList_c();
			CAD_FaceList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_EdgeList_init(ref CAD.EdgeList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_EdgeList_free(ref CAD.EdgeList_c list);

		private static CAD.EdgeList ConvertValue(CAD.EdgeList_c s) {
			CAD.EdgeList list = new CAD.EdgeList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.EdgeList_c ConvertValue(CAD.EdgeList s) {
			CAD.EdgeList_c list =  new CAD.EdgeList_c();
			CAD_EdgeList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_EdgeListList_init(ref CAD.EdgeListList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_EdgeListList_free(ref CAD.EdgeListList_c list);

		private static CAD.EdgeListList ConvertValue(CAD.EdgeListList_c s) {
			CAD.EdgeListList list = new CAD.EdgeListList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(CAD.EdgeList_c)));
				list.list[i] = ConvertValue((CAD.EdgeList_c)Marshal.PtrToStructure(p, typeof(CAD.EdgeList_c)));
			}
			return list;
		}

		private static CAD.EdgeListList_c ConvertValue(CAD.EdgeListList s) {
			CAD.EdgeListList_c list =  new CAD.EdgeListList_c();
			CAD_EdgeListList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				CAD.EdgeList_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(CAD.EdgeList_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_CoEdgeList_init(ref CAD.CoEdgeList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_CoEdgeList_free(ref CAD.CoEdgeList_c list);

		private static CAD.CoEdgeList ConvertValue(CAD.CoEdgeList_c s) {
			CAD.CoEdgeList list = new CAD.CoEdgeList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.CoEdgeList_c ConvertValue(CAD.CoEdgeList s) {
			CAD.CoEdgeList_c list =  new CAD.CoEdgeList_c();
			CAD_CoEdgeList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void CAD_Bounds1D_init(ref CAD.Bounds1D_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void CAD_Bounds1D_free(ref CAD.Bounds1D_c str);

	private static CAD.Bounds1D ConvertValue(CAD.Bounds1D_c s) {
		CAD.Bounds1D ss = new CAD.Bounds1D();
		ss.min = (System.Double)s.min;
		ss.max = (System.Double)s.max;
		return ss;
	}

	private static CAD.Bounds1D_c ConvertValue(CAD.Bounds1D s) {
		CAD.Bounds1D_c ss = new CAD.Bounds1D_c();
		CAD_Bounds1D_init(ref ss);
		ss.min = (System.Double)s.min;
		ss.max = (System.Double)s.max;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_BodyList_init(ref CAD.BodyList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_BodyList_free(ref CAD.BodyList_c list);

		private static CAD.BodyList ConvertValue(CAD.BodyList_c s) {
			CAD.BodyList list = new CAD.BodyList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.BodyList_c ConvertValue(CAD.BodyList s) {
			CAD.BodyList_c list =  new CAD.BodyList_c();
			CAD_BodyList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void CAD_Bounds2D_init(ref CAD.Bounds2D_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void CAD_Bounds2D_free(ref CAD.Bounds2D_c str);

	private static CAD.Bounds2D ConvertValue(CAD.Bounds2D_c s) {
		CAD.Bounds2D ss = new CAD.Bounds2D();
		ss.u = ConvertValue(s.u);
		ss.v = ConvertValue(s.v);
		return ss;
	}

	private static CAD.Bounds2D_c ConvertValue(CAD.Bounds2D s) {
		CAD.Bounds2D_c ss = new CAD.Bounds2D_c();
		CAD_Bounds2D_init(ref ss);
		ss.u = ConvertValue(s.u);
		ss.v = ConvertValue(s.v);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_LoopList_init(ref CAD.LoopList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_LoopList_free(ref CAD.LoopList_c list);

		private static CAD.LoopList ConvertValue(CAD.LoopList_c s) {
			CAD.LoopList list = new CAD.LoopList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.LoopList_c ConvertValue(CAD.LoopList s) {
			CAD.LoopList_c list =  new CAD.LoopList_c();
			CAD_LoopList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_LimitedCurveList_init(ref CAD.LimitedCurveList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_LimitedCurveList_free(ref CAD.LimitedCurveList_c list);

		private static CAD.LimitedCurveList ConvertValue(CAD.LimitedCurveList_c s) {
			CAD.LimitedCurveList list = new CAD.LimitedCurveList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.LimitedCurveList_c ConvertValue(CAD.LimitedCurveList s) {
			CAD.LimitedCurveList_c list =  new CAD.LimitedCurveList_c();
			CAD_LimitedCurveList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_ModelList_init(ref CAD.ModelList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void CAD_ModelList_free(ref CAD.ModelList_c list);

		private static CAD.ModelList ConvertValue(CAD.ModelList_c s) {
			CAD.ModelList list = new CAD.ModelList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static CAD.ModelList_c ConvertValue(CAD.ModelList s) {
			CAD.ModelList_c list =  new CAD.ModelList_c();
			CAD_ModelList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_Date_init(ref Core.Date_c str);
	[DllImport(PiXYZImportSDK_dll)]
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

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_WebLicenseInfo_init(ref Core.WebLicenseInfo_c str);
	[DllImport(PiXYZImportSDK_dll)]
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

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_WebLicenseInfoList_init(ref Core.WebLicenseInfoList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
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

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_StringList_init(ref Core.StringList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
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

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_LicenseInfos_init(ref Core.LicenseInfos_c str);
	[DllImport(PiXYZImportSDK_dll)]
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

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_ColorAlpha_init(ref Core.ColorAlpha_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_ColorAlpha_free(ref Core.ColorAlpha_c str);

	private static Core.ColorAlpha ConvertValue(Core.ColorAlpha_c s) {
		Core.ColorAlpha ss = new Core.ColorAlpha();
		ss.r = (System.Double)s.r;
		ss.g = (System.Double)s.g;
		ss.b = (System.Double)s.b;
		ss.a = (System.Double)s.a;
		return ss;
	}

	private static Core.ColorAlpha_c ConvertValue(Core.ColorAlpha s) {
		Core.ColorAlpha_c ss = new Core.ColorAlpha_c();
		Core_ColorAlpha_init(ref ss);
		ss.r = (System.Double)s.r;
		ss.g = (System.Double)s.g;
		ss.b = (System.Double)s.b;
		ss.a = (System.Double)s.a;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_ByteList_init(ref Core.ByteList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_ByteList_free(ref Core.ByteList_c list);

		private static Core.ByteList ConvertValue(Core.ByteList_c s) {
			Core.ByteList list = new Core.ByteList((int)s.size);
			if (s.size > 0)
				Marshal.Copy(s.ptr, list.list, 0, (int)s.size);
			return list;
		}

		private static Core.ByteList_c ConvertValue(Core.ByteList s) {
			Core.ByteList_c list =  new Core.ByteList_c();
			Core_ByteList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size > 0)
				Marshal.Copy(s.list, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_Color_init(ref Core.Color_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_Color_free(ref Core.Color_c str);

	private static Core.Color ConvertValue(Core.Color_c s) {
		Core.Color ss = new Core.Color();
		ss.r = (System.Double)s.r;
		ss.g = (System.Double)s.g;
		ss.b = (System.Double)s.b;
		return ss;
	}

	private static Core.Color_c ConvertValue(Core.Color s) {
		Core.Color_c ss = new Core.Color_c();
		Core_Color_init(ref ss);
		ss.r = (System.Double)s.r;
		ss.g = (System.Double)s.g;
		ss.b = (System.Double)s.b;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_DoubleList_init(ref Core.DoubleList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_DoubleList_free(ref Core.DoubleList_c list);

		private static Core.DoubleList ConvertValue(Core.DoubleList_c s) {
			Core.DoubleList list = new Core.DoubleList((int)s.size);
			if (s.size > 0)
				Marshal.Copy(s.ptr, list.list, 0, (int)s.size);
			return list;
		}

		private static Core.DoubleList_c ConvertValue(Core.DoubleList s) {
			Core.DoubleList_c list =  new Core.DoubleList_c();
			Core_DoubleList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size > 0)
				Marshal.Copy(s.list, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_DoubleListList_init(ref Core.DoubleListList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_DoubleListList_free(ref Core.DoubleListList_c list);

		private static Core.DoubleListList ConvertValue(Core.DoubleListList_c s) {
			Core.DoubleListList list = new Core.DoubleListList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.DoubleList_c)));
				list.list[i] = ConvertValue((Core.DoubleList_c)Marshal.PtrToStructure(p, typeof(Core.DoubleList_c)));
			}
			return list;
		}

		private static Core.DoubleListList_c ConvertValue(Core.DoubleListList s) {
			Core.DoubleListList_c list =  new Core.DoubleListList_c();
			Core_DoubleListList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Core.DoubleList_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.DoubleList_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_IntList_init(ref Core.IntList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_IntList_free(ref Core.IntList_c list);

		private static Core.IntList ConvertValue(Core.IntList_c s) {
			Core.IntList list = new Core.IntList((int)s.size);
			if (s.size > 0)
				Marshal.Copy(s.ptr, list.list, 0, (int)s.size);
			return list;
		}

		private static Core.IntList_c ConvertValue(Core.IntList s) {
			Core.IntList_c list =  new Core.IntList_c();
			Core_IntList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size > 0)
				Marshal.Copy(s.list, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_BoolList_init(ref Core.BoolList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_BoolList_free(ref Core.BoolList_c list);

		private static Core.BoolList ConvertValue(Core.BoolList_c s) {
			Core.BoolList list = new Core.BoolList((int)s.size);
			if (s.size==0) return list;
			int[] tab = new int[s.size];
			Marshal.Copy(s.ptr, tab, 0, (int)s.size);
				list.list = CopyMemory<System.Boolean>(s.ptr, (int)s.size);
			return list;
		}

		private static Core.BoolList_c ConvertValue(Core.BoolList s) {
			Core.BoolList_c list =  new Core.BoolList_c();
			Core_BoolList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = ConvertValue(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_StringPair_init(ref Core.StringPair_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Core_StringPair_free(ref Core.StringPair_c str);

	private static Core.StringPair ConvertValue(Core.StringPair_c s) {
		Core.StringPair ss = new Core.StringPair();
		ss.key = ConvertValue(s.key);
		ss.value = ConvertValue(s.value);
		return ss;
	}

	private static Core.StringPair_c ConvertValue(Core.StringPair s) {
		Core.StringPair_c ss = new Core.StringPair_c();
		Core_StringPair_init(ref ss);
		ss.key = ConvertValue(s.key);
		ss.value = ConvertValue(s.value);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_StringPairList_init(ref Core.StringPairList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_StringPairList_free(ref Core.StringPairList_c list);

		private static Core.StringPairList ConvertValue(Core.StringPairList_c s) {
			Core.StringPairList list = new Core.StringPairList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.StringPair_c)));
				list.list[i] = ConvertValue((Core.StringPair_c)Marshal.PtrToStructure(p, typeof(Core.StringPair_c)));
			}
			return list;
		}

		private static Core.StringPairList_c ConvertValue(Core.StringPairList s) {
			Core.StringPairList_c list =  new Core.StringPairList_c();
			Core_StringPairList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Core.StringPair_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.StringPair_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_StringPairListList_init(ref Core.StringPairListList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_StringPairListList_free(ref Core.StringPairListList_c list);

		private static Core.StringPairListList ConvertValue(Core.StringPairListList_c s) {
			Core.StringPairListList list = new Core.StringPairListList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.StringPairList_c)));
				list.list[i] = ConvertValue((Core.StringPairList_c)Marshal.PtrToStructure(p, typeof(Core.StringPairList_c)));
			}
			return list;
		}

		private static Core.StringPairListList_c ConvertValue(Core.StringPairListList s) {
			Core.StringPairListList_c list =  new Core.StringPairListList_c();
			Core_StringPairListList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Core.StringPairList_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.StringPairList_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_InheritableBoolList_init(ref Core.InheritableBoolList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_InheritableBoolList_free(ref Core.InheritableBoolList_c list);

		private static Core.InheritableBoolList ConvertValue(Core.InheritableBoolList_c s) {
			Core.InheritableBoolList list = new Core.InheritableBoolList((int)s.size);
			if (s.size==0) return list;
			int[] tab = new int[s.size];
			Marshal.Copy(s.ptr, tab, 0, (int)s.size);
			for (int i = 0; i < (int)s.size; ++i) {
				list.list[i] = (Core.InheritableBool)tab[i];
			}
			return list;
		}

		private static Core.InheritableBoolList_c ConvertValue(Core.InheritableBoolList s) {
			Core.InheritableBoolList_c list =  new Core.InheritableBoolList_c();
			Core_InheritableBoolList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)s.list[i];
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_ColorAlphaList_init(ref Core.ColorAlphaList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Core_ColorAlphaList_free(ref Core.ColorAlphaList_c list);

		private static Core.ColorAlphaList ConvertValue(Core.ColorAlphaList_c s) {
			Core.ColorAlphaList list = new Core.ColorAlphaList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Core.ColorAlpha>(s.ptr, (int)s.size);
			return list;
		}

		private static Core.ColorAlphaList_c ConvertValue(Core.ColorAlphaList s) {
			Core.ColorAlphaList_c list =  new Core.ColorAlphaList_c();
			Core_ColorAlphaList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Core.ColorAlpha_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Core.ColorAlpha_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Array4_init(ref Geom.Array4_c arr, UInt64 size);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Array4_free(ref Geom.Array4_c arr);

	private static Geom.Array4 ConvertValue(Geom.Array4_c arr) {
		Geom.Array4 ss = new Geom.Array4();
		System.Double[] tab = new System.Double[4];
		Marshal.Copy(arr.tab, tab, 0, 4);
		for (int i = 0; i < 4; ++i)
			ss.tab[i] = tab[i];
		return ss;
	}

	private static Geom.Array4_c ConvertValue(Geom.Array4 s) {
		Geom.Array4_c list =  new Geom.Array4_c();
		Geom_Array4_init(ref list, (System.UInt64)4);
		var tab = new System.Double[4];
		for (int i=0; i < 4; ++i)
			tab[i] = s.tab[i];
		Marshal.Copy(tab, 0, list.tab, 4);
		return list;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Point2_init(ref Geom.Point2_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Point2_free(ref Geom.Point2_c str);

	private static Geom.Point2 ConvertValue(Geom.Point2_c s) {
		Geom.Point2 ss = new Geom.Point2();
		ss.x = (System.Double)s.x;
		ss.y = (System.Double)s.y;
		return ss;
	}

	private static Geom.Point2_c ConvertValue(Geom.Point2 s) {
		Geom.Point2_c ss = new Geom.Point2_c();
		Geom_Point2_init(ref ss);
		ss.x = (System.Double)s.x;
		ss.y = (System.Double)s.y;
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Point3_init(ref Geom.Point3_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Point3_free(ref Geom.Point3_c str);

	private static Geom.Point3 ConvertValue(Geom.Point3_c s) {
		Geom.Point3 ss = new Geom.Point3();
		ss.x = (System.Double)s.x;
		ss.y = (System.Double)s.y;
		ss.z = (System.Double)s.z;
		return ss;
	}

	private static Geom.Point3_c ConvertValue(Geom.Point3 s) {
		Geom.Point3_c ss = new Geom.Point3_c();
		Geom_Point3_init(ref ss);
		ss.x = (System.Double)s.x;
		ss.y = (System.Double)s.y;
		ss.z = (System.Double)s.z;
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Curvatures_init(ref Geom.Curvatures_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Curvatures_free(ref Geom.Curvatures_c str);

	private static Geom.Curvatures ConvertValue(Geom.Curvatures_c s) {
		Geom.Curvatures ss = new Geom.Curvatures();
		ss.k1 = (System.Double)s.k1;
		ss.k2 = (System.Double)s.k2;
		ss.v1 = ConvertValue(s.v1);
		ss.v2 = ConvertValue(s.v2);
		return ss;
	}

	private static Geom.Curvatures_c ConvertValue(Geom.Curvatures s) {
		Geom.Curvatures_c ss = new Geom.Curvatures_c();
		Geom_Curvatures_init(ref ss);
		ss.k1 = (System.Double)s.k1;
		ss.k2 = (System.Double)s.k2;
		ss.v1 = ConvertValue(s.v1);
		ss.v2 = ConvertValue(s.v2);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point3List_init(ref Geom.Point3List_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point3List_free(ref Geom.Point3List_c list);

		private static Geom.Point3List ConvertValue(Geom.Point3List_c s) {
			Geom.Point3List list = new Geom.Point3List((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Geom.Point3>(s.ptr, (int)s.size);
			return list;
		}

		private static Geom.Point3List_c ConvertValue(Geom.Point3List s) {
			Geom.Point3List_c list =  new Geom.Point3List_c();
			Geom_Point3List_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Point3_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point3_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point3ListList_init(ref Geom.Point3ListList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point3ListList_free(ref Geom.Point3ListList_c list);

		private static Geom.Point3ListList ConvertValue(Geom.Point3ListList_c s) {
			Geom.Point3ListList list = new Geom.Point3ListList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point3List_c)));
				list.list[i] = ConvertValue((Geom.Point3List_c)Marshal.PtrToStructure(p, typeof(Geom.Point3List_c)));
			}
			return list;
		}

		private static Geom.Point3ListList_c ConvertValue(Geom.Point3ListList s) {
			Geom.Point3ListList_c list =  new Geom.Point3ListList_c();
			Geom_Point3ListList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Point3List_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point3List_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point2List_init(ref Geom.Point2List_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point2List_free(ref Geom.Point2List_c list);

		private static Geom.Point2List ConvertValue(Geom.Point2List_c s) {
			Geom.Point2List list = new Geom.Point2List((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Geom.Point2>(s.ptr, (int)s.size);
			return list;
		}

		private static Geom.Point2List_c ConvertValue(Geom.Point2List s) {
			Geom.Point2List_c list =  new Geom.Point2List_c();
			Geom_Point2List_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Point2_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point2_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point2ListList_init(ref Geom.Point2ListList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Point2ListList_free(ref Geom.Point2ListList_c list);

		private static Geom.Point2ListList ConvertValue(Geom.Point2ListList_c s) {
			Geom.Point2ListList list = new Geom.Point2ListList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point2List_c)));
				list.list[i] = ConvertValue((Geom.Point2List_c)Marshal.PtrToStructure(p, typeof(Geom.Point2List_c)));
			}
			return list;
		}

		private static Geom.Point2ListList_c ConvertValue(Geom.Point2ListList s) {
			Geom.Point2ListList_c list =  new Geom.Point2ListList_c();
			Geom_Point2ListList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Point2List_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point2List_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Matrix4_init(ref Geom.Matrix4_c arr, UInt64 size);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Matrix4_free(ref Geom.Matrix4_c arr);

	private static Geom.Matrix4 ConvertValue(Geom.Matrix4_c arr) {
		Geom.Matrix4 ss = new Geom.Matrix4();
		for (int i = 0; i < 4; ++i) {
			IntPtr p = new IntPtr(arr.tab.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Array4_c)));
			ss.tab[i] = ConvertValue((Geom.Array4_c)Marshal.PtrToStructure(p, typeof(Geom.Array4_c)));
		}
		return ss;
	}

	private static Geom.Matrix4_c ConvertValue(Geom.Matrix4 s) {
		Geom.Matrix4_c list =  new Geom.Matrix4_c();
		Geom_Matrix4_init(ref list, (System.UInt64)4);
		for (int i = 0; i < 4; ++i) {
			Geom.Array4_c elt = ConvertValue(s.tab[i]);
			IntPtr p = new IntPtr(list.tab.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Array4_c)));
			Marshal.StructureToPtr(elt, p, true);
		}
		return list;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_AABB_init(ref Geom.AABB_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_AABB_free(ref Geom.AABB_c str);

	private static Geom.AABB ConvertValue(Geom.AABB_c s) {
		Geom.AABB ss = new Geom.AABB();
		ss.low = ConvertValue(s.low);
		ss.high = ConvertValue(s.high);
		return ss;
	}

	private static Geom.AABB_c ConvertValue(Geom.AABB s) {
		Geom.AABB_c ss = new Geom.AABB_c();
		Geom_AABB_init(ref ss);
		ss.low = ConvertValue(s.low);
		ss.high = ConvertValue(s.high);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Matrix4List_init(ref Geom.Matrix4List_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Matrix4List_free(ref Geom.Matrix4List_c list);

		private static Geom.Matrix4List ConvertValue(Geom.Matrix4List_c s) {
			Geom.Matrix4List list = new Geom.Matrix4List((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Matrix4_c)));
				list.list[i] = ConvertValue((Geom.Matrix4_c)Marshal.PtrToStructure(p, typeof(Geom.Matrix4_c)));
			}
			return list;
		}

		private static Geom.Matrix4List_c ConvertValue(Geom.Matrix4List s) {
			Geom.Matrix4List_c list =  new Geom.Matrix4List_c();
			Geom_Matrix4List_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Matrix4_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Matrix4_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Vector4List_init(ref Geom.Vector4List_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Vector4List_free(ref Geom.Vector4List_c list);

		private static Geom.Vector4List ConvertValue(Geom.Vector4List_c s) {
			Geom.Vector4List list = new Geom.Vector4List((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Geom.Vector4>(s.ptr, (int)s.size);
			return list;
		}

		private static Geom.Vector4List_c ConvertValue(Geom.Vector4List s) {
			Geom.Vector4List_c list =  new Geom.Vector4List_c();
			Geom_Vector4List_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Point4_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point4_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Vector4I_init(ref Geom.Vector4I_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Vector4I_free(ref Geom.Vector4I_c str);

	private static Geom.Vector4I ConvertValue(Geom.Vector4I_c s) {
		Geom.Vector4I ss = new Geom.Vector4I();
		ss.x = (System.Int32)s.x;
		ss.y = (System.Int32)s.y;
		ss.z = (System.Int32)s.z;
		ss.w = (System.Int32)s.w;
		return ss;
	}

	private static Geom.Vector4I_c ConvertValue(Geom.Vector4I s) {
		Geom.Vector4I_c ss = new Geom.Vector4I_c();
		Geom_Vector4I_init(ref ss);
		ss.x = (Int32)s.x;
		ss.y = (Int32)s.y;
		ss.z = (Int32)s.z;
		ss.w = (Int32)s.w;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Vector4IList_init(ref Geom.Vector4IList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Vector4IList_free(ref Geom.Vector4IList_c list);

		private static Geom.Vector4IList ConvertValue(Geom.Vector4IList_c s) {
			Geom.Vector4IList list = new Geom.Vector4IList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Geom.Vector4I>(s.ptr, (int)s.size);
			return list;
		}

		private static Geom.Vector4IList_c ConvertValue(Geom.Vector4IList s) {
			Geom.Vector4IList_c list =  new Geom.Vector4IList_c();
			Geom_Vector4IList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Vector4I_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Vector4I_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Point4_init(ref Geom.Point4_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Geom_Point4_free(ref Geom.Point4_c str);

	private static Geom.Point4 ConvertValue(Geom.Point4_c s) {
		Geom.Point4 ss = new Geom.Point4();
		ss.x = (System.Double)s.x;
		ss.y = (System.Double)s.y;
		ss.z = (System.Double)s.z;
		ss.w = (System.Double)s.w;
		return ss;
	}

	private static Geom.Point4_c ConvertValue(Geom.Point4 s) {
		Geom.Point4_c ss = new Geom.Point4_c();
		Geom_Point4_init(ref ss);
		ss.x = (System.Double)s.x;
		ss.y = (System.Double)s.y;
		ss.z = (System.Double)s.z;
		ss.w = (System.Double)s.w;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Vector3List_init(ref Geom.Vector3List_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_Vector3List_free(ref Geom.Vector3List_c list);

		private static Geom.Vector3List ConvertValue(Geom.Vector3List_c s) {
			Geom.Vector3List list = new Geom.Vector3List((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Geom.Vector3>(s.ptr, (int)s.size);
			return list;
		}

		private static Geom.Vector3List_c ConvertValue(Geom.Vector3List s) {
			Geom.Vector3List_c list =  new Geom.Vector3List_c();
			Geom_Vector3List_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Point3_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Point3_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_CurvaturesList_init(ref Geom.CurvaturesList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Geom_CurvaturesList_free(ref Geom.CurvaturesList_c list);

		private static Geom.CurvaturesList ConvertValue(Geom.CurvaturesList_c s) {
			Geom.CurvaturesList list = new Geom.CurvaturesList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Curvatures_c)));
				list.list[i] = ConvertValue((Geom.Curvatures_c)Marshal.PtrToStructure(p, typeof(Geom.Curvatures_c)));
			}
			return list;
		}

		private static Geom.CurvaturesList_c ConvertValue(Geom.CurvaturesList s) {
			Geom.CurvaturesList_c list =  new Geom.CurvaturesList_c();
			Geom_CurvaturesList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Geom.Curvatures_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Geom.Curvatures_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void IO_Format_init(ref IO.Format_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void IO_Format_free(ref IO.Format_c str);

	private static IO.Format ConvertValue(IO.Format_c s) {
		IO.Format ss = new IO.Format();
		ss.name = ConvertValue(s.name);
		ss.extensions = ConvertValue(s.extensions);
		return ss;
	}

	private static IO.Format_c ConvertValue(IO.Format s) {
		IO.Format_c ss = new IO.Format_c();
		IO_Format_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.extensions = ConvertValue(s.extensions);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void IO_FormatList_init(ref IO.FormatList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void IO_FormatList_free(ref IO.FormatList_c list);

		private static IO.FormatList ConvertValue(IO.FormatList_c s) {
			IO.FormatList list = new IO.FormatList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(IO.Format_c)));
				list.list[i] = ConvertValue((IO.Format_c)Marshal.PtrToStructure(p, typeof(IO.Format_c)));
			}
			return list;
		}

		private static IO.FormatList_c ConvertValue(IO.FormatList s) {
			IO.FormatList_c list =  new IO.FormatList_c();
			IO_FormatList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				IO.Format_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(IO.Format_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_Texture_init(ref Material.Texture_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_Texture_free(ref Material.Texture_c str);

	private static Material.Texture ConvertValue(Material.Texture_c s) {
		Material.Texture ss = new Material.Texture();
		ss.image = (System.UInt32)s.image;
		ss.channel = (System.Int32)s.channel;
		ss.offset = ConvertValue(s.offset);
		ss.tilling = ConvertValue(s.tilling);
		return ss;
	}

	private static Material.Texture_c ConvertValue(Material.Texture s) {
		Material.Texture_c ss = new Material.Texture_c();
		Material_Texture_init(ref ss);
		ss.image = (System.UInt32)s.image;
		ss.channel = (Int32)s.channel;
		ss.offset = ConvertValue(s.offset);
		ss.tilling = ConvertValue(s.tilling);
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_CoeffOrTexture_init(ref Material.CoeffOrTexture_c sel);

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_CoeffOrTexture_free(ref Material.CoeffOrTexture_c sel);

	private static Material.CoeffOrTexture ConvertValue(Material.CoeffOrTexture_c s) {
		Material.CoeffOrTexture ss = new Material.CoeffOrTexture();
		ss._type = (Material.CoeffOrTexture.Type)s._type;
		switch(ss._type) {
			case Material.CoeffOrTexture.Type.UNKNOWN: break;
			case Material.CoeffOrTexture.Type.COEFF: ss.coeff = s.coeff; break;
			case Material.CoeffOrTexture.Type.TEXTURE: ss.texture = ConvertValue(s.texture); break;
		}
		return ss;
	}

	private static Material.CoeffOrTexture_c ConvertValue(Material.CoeffOrTexture s) {
		Material.CoeffOrTexture_c ss = new Material.CoeffOrTexture_c();
		Material_CoeffOrTexture_init(ref ss);
		ss._type = (int)s._type;
		switch (ss._type) {
			case 0: break;
			case 1: ss.coeff = (System.Double)s.coeff; break;
			case 2: ss.texture = ConvertValue(s.texture); break;
		}
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_ColorOrTexture_init(ref Material.ColorOrTexture_c sel);

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_ColorOrTexture_free(ref Material.ColorOrTexture_c sel);

	private static Material.ColorOrTexture ConvertValue(Material.ColorOrTexture_c s) {
		Material.ColorOrTexture ss = new Material.ColorOrTexture();
		ss._type = (Material.ColorOrTexture.Type)s._type;
		switch(ss._type) {
			case Material.ColorOrTexture.Type.UNKNOWN: break;
			case Material.ColorOrTexture.Type.COLOR: ss.color = ConvertValue(s.color); break;
			case Material.ColorOrTexture.Type.TEXTURE: ss.texture = ConvertValue(s.texture); break;
		}
		return ss;
	}

	private static Material.ColorOrTexture_c ConvertValue(Material.ColorOrTexture s) {
		Material.ColorOrTexture_c ss = new Material.ColorOrTexture_c();
		Material_ColorOrTexture_init(ref ss);
		ss._type = (int)s._type;
		switch (ss._type) {
			case 0: break;
			case 1: ss.color = ConvertValue(s.color); break;
			case 2: ss.texture = ConvertValue(s.texture); break;
		}
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_StandardMaterialInfos_init(ref Material.StandardMaterialInfos_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_StandardMaterialInfos_free(ref Material.StandardMaterialInfos_c str);

	private static Material.StandardMaterialInfos ConvertValue(Material.StandardMaterialInfos_c s) {
		Material.StandardMaterialInfos ss = new Material.StandardMaterialInfos();
		ss.name = ConvertValue(s.name);
		ss.diffuse = ConvertValue(s.diffuse);
		ss.specular = ConvertValue(s.specular);
		ss.ambient = ConvertValue(s.ambient);
		ss.emissive = ConvertValue(s.emissive);
		ss.shininess = (System.Double)s.shininess;
		ss.transparency = (System.Double)s.transparency;
		return ss;
	}

	private static Material.StandardMaterialInfos_c ConvertValue(Material.StandardMaterialInfos s) {
		Material.StandardMaterialInfos_c ss = new Material.StandardMaterialInfos_c();
		Material_StandardMaterialInfos_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.diffuse = ConvertValue(s.diffuse);
		ss.specular = ConvertValue(s.specular);
		ss.ambient = ConvertValue(s.ambient);
		ss.emissive = ConvertValue(s.emissive);
		ss.shininess = (System.Double)s.shininess;
		ss.transparency = (System.Double)s.transparency;
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_ColorMaterialInfos_init(ref Material.ColorMaterialInfos_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_ColorMaterialInfos_free(ref Material.ColorMaterialInfos_c str);

	private static Material.ColorMaterialInfos ConvertValue(Material.ColorMaterialInfos_c s) {
		Material.ColorMaterialInfos ss = new Material.ColorMaterialInfos();
		ss.name = ConvertValue(s.name);
		ss.color = ConvertValue(s.color);
		return ss;
	}

	private static Material.ColorMaterialInfos_c ConvertValue(Material.ColorMaterialInfos s) {
		Material.ColorMaterialInfos_c ss = new Material.ColorMaterialInfos_c();
		Material_ColorMaterialInfos_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.color = ConvertValue(s.color);
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_ImageDefinition_init(ref Material.ImageDefinition_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_ImageDefinition_free(ref Material.ImageDefinition_c str);

	private static Material.ImageDefinition ConvertValue(Material.ImageDefinition_c s) {
		Material.ImageDefinition ss = new Material.ImageDefinition();
		ss.id = (System.UInt32)s.id;
		ss.name = ConvertValue(s.name);
		ss.width = (System.Int32)s.width;
		ss.height = (System.Int32)s.height;
		ss.bitsPerComponent = (System.Int32)s.bitsPerComponent;
		ss.componentsCount = (System.Int32)s.componentsCount;
		ss.data = ConvertValue(s.data);
		return ss;
	}

	private static Material.ImageDefinition_c ConvertValue(Material.ImageDefinition s) {
		Material.ImageDefinition_c ss = new Material.ImageDefinition_c();
		Material_ImageDefinition_init(ref ss);
		ss.id = (System.UInt32)s.id;
		ss.name = ConvertValue(s.name);
		ss.width = (Int32)s.width;
		ss.height = (Int32)s.height;
		ss.bitsPerComponent = (Int32)s.bitsPerComponent;
		ss.componentsCount = (Int32)s.componentsCount;
		ss.data = ConvertValue(s.data);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_ImageDefinitionList_init(ref Material.ImageDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_ImageDefinitionList_free(ref Material.ImageDefinitionList_c list);

		private static Material.ImageDefinitionList ConvertValue(Material.ImageDefinitionList_c s) {
			Material.ImageDefinitionList list = new Material.ImageDefinitionList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Material.ImageDefinition_c)));
				list.list[i] = ConvertValue((Material.ImageDefinition_c)Marshal.PtrToStructure(p, typeof(Material.ImageDefinition_c)));
			}
			return list;
		}

		private static Material.ImageDefinitionList_c ConvertValue(Material.ImageDefinitionList s) {
			Material.ImageDefinitionList_c list =  new Material.ImageDefinitionList_c();
			Material_ImageDefinitionList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Material.ImageDefinition_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Material.ImageDefinition_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_MaterialList_init(ref Material.MaterialList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_MaterialList_free(ref Material.MaterialList_c list);

		private static Material.MaterialList ConvertValue(Material.MaterialList_c s) {
			Material.MaterialList list = new Material.MaterialList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Material.MaterialList_c ConvertValue(Material.MaterialList s) {
			Material.MaterialList_c list =  new Material.MaterialList_c();
			Material_MaterialList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_ImageList_init(ref Material.ImageList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_ImageList_free(ref Material.ImageList_c list);

		private static Material.ImageList ConvertValue(Material.ImageList_c s) {
			Material.ImageList list = new Material.ImageList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Material.ImageList_c ConvertValue(Material.ImageList s) {
			Material.ImageList_c list =  new Material.ImageList_c();
			Material_ImageList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_PBRMaterialInfos_init(ref Material.PBRMaterialInfos_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_PBRMaterialInfos_free(ref Material.PBRMaterialInfos_c str);

	private static Material.PBRMaterialInfos ConvertValue(Material.PBRMaterialInfos_c s) {
		Material.PBRMaterialInfos ss = new Material.PBRMaterialInfos();
		ss.name = ConvertValue(s.name);
		ss.albedo = ConvertValue(s.albedo);
		ss.normal = ConvertValue(s.normal);
		ss.metallic = ConvertValue(s.metallic);
		ss.roughness = ConvertValue(s.roughness);
		ss.ao = ConvertValue(s.ao);
		ss.opacity = ConvertValue(s.opacity);
		return ss;
	}

	private static Material.PBRMaterialInfos_c ConvertValue(Material.PBRMaterialInfos s) {
		Material.PBRMaterialInfos_c ss = new Material.PBRMaterialInfos_c();
		Material_PBRMaterialInfos_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.albedo = ConvertValue(s.albedo);
		ss.normal = ConvertValue(s.normal);
		ss.metallic = ConvertValue(s.metallic);
		ss.roughness = ConvertValue(s.roughness);
		ss.ao = ConvertValue(s.ao);
		ss.opacity = ConvertValue(s.opacity);
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_UnlitTextureMaterialInfos_init(ref Material.UnlitTextureMaterialInfos_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_UnlitTextureMaterialInfos_free(ref Material.UnlitTextureMaterialInfos_c str);

	private static Material.UnlitTextureMaterialInfos ConvertValue(Material.UnlitTextureMaterialInfos_c s) {
		Material.UnlitTextureMaterialInfos ss = new Material.UnlitTextureMaterialInfos();
		ss.name = ConvertValue(s.name);
		ss.texture = ConvertValue(s.texture);
		return ss;
	}

	private static Material.UnlitTextureMaterialInfos_c ConvertValue(Material.UnlitTextureMaterialInfos s) {
		Material.UnlitTextureMaterialInfos_c ss = new Material.UnlitTextureMaterialInfos_c();
		Material_UnlitTextureMaterialInfos_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.texture = ConvertValue(s.texture);
		return ss;
	}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_MaterialDefinition_init(ref Material.MaterialDefinition_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Material_MaterialDefinition_free(ref Material.MaterialDefinition_c str);

	private static Material.MaterialDefinition ConvertValue(Material.MaterialDefinition_c s) {
		Material.MaterialDefinition ss = new Material.MaterialDefinition();
		ss.name = ConvertValue(s.name);
		ss.id = (System.UInt32)s.id;
		ss.albedo = ConvertValue(s.albedo);
		ss.normal = ConvertValue(s.normal);
		ss.metallic = ConvertValue(s.metallic);
		ss.roughness = ConvertValue(s.roughness);
		ss.ao = ConvertValue(s.ao);
		ss.opacity = ConvertValue(s.opacity);
		return ss;
	}

	private static Material.MaterialDefinition_c ConvertValue(Material.MaterialDefinition s) {
		Material.MaterialDefinition_c ss = new Material.MaterialDefinition_c();
		Material_MaterialDefinition_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.id = (System.UInt32)s.id;
		ss.albedo = ConvertValue(s.albedo);
		ss.normal = ConvertValue(s.normal);
		ss.metallic = ConvertValue(s.metallic);
		ss.roughness = ConvertValue(s.roughness);
		ss.ao = ConvertValue(s.ao);
		ss.opacity = ConvertValue(s.opacity);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_MaterialDefinitionList_init(ref Material.MaterialDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Material_MaterialDefinitionList_free(ref Material.MaterialDefinitionList_c list);

		private static Material.MaterialDefinitionList ConvertValue(Material.MaterialDefinitionList_c s) {
			Material.MaterialDefinitionList list = new Material.MaterialDefinitionList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Material.MaterialDefinition_c)));
				list.list[i] = ConvertValue((Material.MaterialDefinition_c)Marshal.PtrToStructure(p, typeof(Material.MaterialDefinition_c)));
			}
			return list;
		}

		private static Material.MaterialDefinitionList_c ConvertValue(Material.MaterialDefinitionList s) {
			Material.MaterialDefinitionList_c list =  new Material.MaterialDefinitionList_c();
			Material_MaterialDefinitionList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Material.MaterialDefinition_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Material.MaterialDefinition_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_MeshList_init(ref Polygonal.MeshList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_MeshList_free(ref Polygonal.MeshList_c list);

		private static Polygonal.MeshList ConvertValue(Polygonal.MeshList_c s) {
			Polygonal.MeshList list = new Polygonal.MeshList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Polygonal.MeshList_c ConvertValue(Polygonal.MeshList s) {
			Polygonal.MeshList_c list =  new Polygonal.MeshList_c();
			Polygonal_MeshList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_JointList_init(ref Polygonal.JointList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_JointList_free(ref Polygonal.JointList_c list);

		private static Polygonal.JointList ConvertValue(Polygonal.JointList_c s) {
			Polygonal.JointList list = new Polygonal.JointList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Polygonal.JointList_c ConvertValue(Polygonal.JointList s) {
			Polygonal.JointList_c list =  new Polygonal.JointList_c();
			Polygonal_JointList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Polygonal_DressedPoly_init(ref Polygonal.DressedPoly_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Polygonal_DressedPoly_free(ref Polygonal.DressedPoly_c str);

	private static Polygonal.DressedPoly ConvertValue(Polygonal.DressedPoly_c s) {
		Polygonal.DressedPoly ss = new Polygonal.DressedPoly();
		ss.material = (System.UInt32)s.material;
		ss.firstTri = (System.Int32)s.firstTri;
		ss.triCount = (System.Int32)s.triCount;
		ss.firstQuad = (System.Int32)s.firstQuad;
		ss.quadCount = (System.Int32)s.quadCount;
		ss.externalId = (System.UInt32)s.externalId;
		return ss;
	}

	private static Polygonal.DressedPoly_c ConvertValue(Polygonal.DressedPoly s) {
		Polygonal.DressedPoly_c ss = new Polygonal.DressedPoly_c();
		Polygonal_DressedPoly_init(ref ss);
		ss.material = (System.UInt32)s.material;
		ss.firstTri = (Int32)s.firstTri;
		ss.triCount = (Int32)s.triCount;
		ss.firstQuad = (Int32)s.firstQuad;
		ss.quadCount = (Int32)s.quadCount;
		ss.externalId = (System.UInt32)s.externalId;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_DressedPolyList_init(ref Polygonal.DressedPolyList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_DressedPolyList_free(ref Polygonal.DressedPolyList_c list);

		private static Polygonal.DressedPolyList ConvertValue(Polygonal.DressedPolyList_c s) {
			Polygonal.DressedPolyList list = new Polygonal.DressedPolyList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<Polygonal.DressedPoly>(s.ptr, (int)s.size);
			return list;
		}

		private static Polygonal.DressedPolyList_c ConvertValue(Polygonal.DressedPolyList s) {
			Polygonal.DressedPolyList_c list =  new Polygonal.DressedPolyList_c();
			Polygonal_DressedPolyList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Polygonal.DressedPoly_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Polygonal.DressedPoly_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Polygonal_StylizedLine_init(ref Polygonal.StylizedLine_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Polygonal_StylizedLine_free(ref Polygonal.StylizedLine_c str);

	private static Polygonal.StylizedLine ConvertValue(Polygonal.StylizedLine_c s) {
		Polygonal.StylizedLine ss = new Polygonal.StylizedLine();
		ss.lines = ConvertValue(s.lines);
		ss.width = (System.Double)s.width;
		ss.type = (Polygonal.StyleType)s.type;
		ss.pattern = (System.Int32)s.pattern;
		ss.color = ConvertValue(s.color);
		ss.externalId = (System.UInt32)s.externalId;
		return ss;
	}

	private static Polygonal.StylizedLine_c ConvertValue(Polygonal.StylizedLine s) {
		Polygonal.StylizedLine_c ss = new Polygonal.StylizedLine_c();
		Polygonal_StylizedLine_init(ref ss);
		ss.lines = ConvertValue(s.lines);
		ss.width = (System.Double)s.width;
		ss.type = (Int32)s.type;
		ss.pattern = (Int32)s.pattern;
		ss.color = ConvertValue(s.color);
		ss.externalId = (System.UInt32)s.externalId;
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_StylizedLineList_init(ref Polygonal.StylizedLineList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_StylizedLineList_free(ref Polygonal.StylizedLineList_c list);

		private static Polygonal.StylizedLineList ConvertValue(Polygonal.StylizedLineList_c s) {
			Polygonal.StylizedLineList list = new Polygonal.StylizedLineList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Polygonal.StylizedLine_c)));
				list.list[i] = ConvertValue((Polygonal.StylizedLine_c)Marshal.PtrToStructure(p, typeof(Polygonal.StylizedLine_c)));
			}
			return list;
		}

		private static Polygonal.StylizedLineList_c ConvertValue(Polygonal.StylizedLineList s) {
			Polygonal.StylizedLineList_c list =  new Polygonal.StylizedLineList_c();
			Polygonal_StylizedLineList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Polygonal.StylizedLine_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Polygonal.StylizedLine_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Polygonal_MeshDefinition_init(ref Polygonal.MeshDefinition_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Polygonal_MeshDefinition_free(ref Polygonal.MeshDefinition_c str);

	private static Polygonal.MeshDefinition ConvertValue(Polygonal.MeshDefinition_c s) {
		Polygonal.MeshDefinition ss = new Polygonal.MeshDefinition();
		ss.id = (System.UInt32)s.id;
		ss.vertices = ConvertValue(s.vertices);
		ss.normals = ConvertValue(s.normals);
		ss.tangents = ConvertValue(s.tangents);
		ss.uvChannels = ConvertValue(s.uvChannels);
		ss.uvs = ConvertValue(s.uvs);
		ss.vertexColors = ConvertValue(s.vertexColors);
		ss.curvatures = ConvertValue(s.curvatures);
		ss.triangles = ConvertValue(s.triangles);
		ss.quadrangles = ConvertValue(s.quadrangles);
		ss.vertexMerged = ConvertValue(s.vertexMerged);
		ss.dressedPolys = ConvertValue(s.dressedPolys);
		ss.linesVertices = ConvertValue(s.linesVertices);
		ss.lines = ConvertValue(s.lines);
		ss.points = ConvertValue(s.points);
		ss.pointsColors = ConvertValue(s.pointsColors);
		ss.joints = ConvertValue(s.joints);
		ss.inverseBindMatrices = ConvertValue(s.inverseBindMatrices);
		ss.jointWeights = ConvertValue(s.jointWeights);
		ss.jointIndices = ConvertValue(s.jointIndices);
		return ss;
	}

	private static Polygonal.MeshDefinition_c ConvertValue(Polygonal.MeshDefinition s) {
		Polygonal.MeshDefinition_c ss = new Polygonal.MeshDefinition_c();
		Polygonal_MeshDefinition_init(ref ss);
		ss.id = (System.UInt32)s.id;
		ss.vertices = ConvertValue(s.vertices);
		ss.normals = ConvertValue(s.normals);
		ss.tangents = ConvertValue(s.tangents);
		ss.uvChannels = ConvertValue(s.uvChannels);
		ss.uvs = ConvertValue(s.uvs);
		ss.vertexColors = ConvertValue(s.vertexColors);
		ss.curvatures = ConvertValue(s.curvatures);
		ss.triangles = ConvertValue(s.triangles);
		ss.quadrangles = ConvertValue(s.quadrangles);
		ss.vertexMerged = ConvertValue(s.vertexMerged);
		ss.dressedPolys = ConvertValue(s.dressedPolys);
		ss.linesVertices = ConvertValue(s.linesVertices);
		ss.lines = ConvertValue(s.lines);
		ss.points = ConvertValue(s.points);
		ss.pointsColors = ConvertValue(s.pointsColors);
		ss.joints = ConvertValue(s.joints);
		ss.inverseBindMatrices = ConvertValue(s.inverseBindMatrices);
		ss.jointWeights = ConvertValue(s.jointWeights);
		ss.jointIndices = ConvertValue(s.jointIndices);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_MeshDefinitionList_init(ref Polygonal.MeshDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Polygonal_MeshDefinitionList_free(ref Polygonal.MeshDefinitionList_c list);

		private static Polygonal.MeshDefinitionList ConvertValue(Polygonal.MeshDefinitionList_c s) {
			Polygonal.MeshDefinitionList list = new Polygonal.MeshDefinitionList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Polygonal.MeshDefinition_c)));
				list.list[i] = ConvertValue((Polygonal.MeshDefinition_c)Marshal.PtrToStructure(p, typeof(Polygonal.MeshDefinition_c)));
			}
			return list;
		}

		private static Polygonal.MeshDefinitionList_c ConvertValue(Polygonal.MeshDefinitionList s) {
			Polygonal.MeshDefinitionList_c list =  new Polygonal.MeshDefinitionList_c();
			Polygonal_MeshDefinitionList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Polygonal.MeshDefinition_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Polygonal.MeshDefinition_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_OccurrenceList_init(ref Scene.OccurrenceList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_OccurrenceList_free(ref Scene.OccurrenceList_c list);

		private static Scene.OccurrenceList ConvertValue(Scene.OccurrenceList_c s) {
			Scene.OccurrenceList list = new Scene.OccurrenceList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Scene.OccurrenceList_c ConvertValue(Scene.OccurrenceList s) {
			Scene.OccurrenceList_c list =  new Scene.OccurrenceList_c();
			Scene_OccurrenceList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Scene_PropertyValue_init(ref Scene.PropertyValue_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Scene_PropertyValue_free(ref Scene.PropertyValue_c str);

	private static Scene.PropertyValue ConvertValue(Scene.PropertyValue_c s) {
		Scene.PropertyValue ss = new Scene.PropertyValue();
		ss.name = ConvertValue(s.name);
		ss.value = ConvertValue(s.value);
		return ss;
	}

	private static Scene.PropertyValue_c ConvertValue(Scene.PropertyValue s) {
		Scene.PropertyValue_c ss = new Scene.PropertyValue_c();
		Scene_PropertyValue_init(ref ss);
		ss.name = ConvertValue(s.name);
		ss.value = ConvertValue(s.value);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_ComponentList_init(ref Scene.ComponentList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_ComponentList_free(ref Scene.ComponentList_c list);

		private static Scene.ComponentList ConvertValue(Scene.ComponentList_c s) {
			Scene.ComponentList list = new Scene.ComponentList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Scene.ComponentList_c ConvertValue(Scene.ComponentList s) {
			Scene.ComponentList_c list =  new Scene.ComponentList_c();
			Scene_ComponentList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_PartList_init(ref Scene.PartList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_PartList_free(ref Scene.PartList_c list);

		private static Scene.PartList ConvertValue(Scene.PartList_c s) {
			Scene.PartList list = new Scene.PartList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Scene.PartList_c ConvertValue(Scene.PartList s) {
			Scene.PartList_c list =  new Scene.PartList_c();
			Scene_PartList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Scene_PackedTree_init(ref Scene.PackedTree_c str);
	[DllImport(PiXYZImportSDK_dll)]
	private static extern void Scene_PackedTree_free(ref Scene.PackedTree_c str);

	private static Scene.PackedTree ConvertValue(Scene.PackedTree_c s) {
		Scene.PackedTree ss = new Scene.PackedTree();
		ss.occurrences = ConvertValue(s.occurrences);
		ss.parents = ConvertValue(s.parents);
		ss.names = ConvertValue(s.names);
		ss.visibles = ConvertValue(s.visibles);
		ss.materials = ConvertValue(s.materials);
		ss.transformIndices = ConvertValue(s.transformIndices);
		ss.transformMatrices = ConvertValue(s.transformMatrices);
		ss.customProperties = ConvertValue(s.customProperties);
		return ss;
	}

	private static Scene.PackedTree_c ConvertValue(Scene.PackedTree s) {
		Scene.PackedTree_c ss = new Scene.PackedTree_c();
		Scene_PackedTree_init(ref ss);
		ss.occurrences = ConvertValue(s.occurrences);
		ss.parents = ConvertValue(s.parents);
		ss.names = ConvertValue(s.names);
		ss.visibles = ConvertValue(s.visibles);
		ss.materials = ConvertValue(s.materials);
		ss.transformIndices = ConvertValue(s.transformIndices);
		ss.transformMatrices = ConvertValue(s.transformMatrices);
		ss.customProperties = ConvertValue(s.customProperties);
		return ss;
	}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_PropertyValueList_init(ref Scene.PropertyValueList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_PropertyValueList_free(ref Scene.PropertyValueList_c list);

		private static Scene.PropertyValueList ConvertValue(Scene.PropertyValueList_c s) {
			Scene.PropertyValueList list = new Scene.PropertyValueList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Scene.PropertyValue_c)));
				list.list[i] = ConvertValue((Scene.PropertyValue_c)Marshal.PtrToStructure(p, typeof(Scene.PropertyValue_c)));
			}
			return list;
		}

		private static Scene.PropertyValueList_c ConvertValue(Scene.PropertyValueList s) {
			Scene.PropertyValueList_c list =  new Scene.PropertyValueList_c();
			Scene_PropertyValueList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Scene.PropertyValue_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Scene.PropertyValue_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_MetadataDefinitionList_init(ref Scene.MetadataDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_MetadataDefinitionList_free(ref Scene.MetadataDefinitionList_c list);

		private static Scene.MetadataDefinitionList ConvertValue(Scene.MetadataDefinitionList_c s) {
			Scene.MetadataDefinitionList list = new Scene.MetadataDefinitionList((int)s.size);
			if (s.size==0) return list;
			for (int i = 0; i < (int)s.size; ++i) {
				IntPtr p = new IntPtr(s.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Scene.PropertyValueList_c)));
				list.list[i] = ConvertValue((Scene.PropertyValueList_c)Marshal.PtrToStructure(p, typeof(Scene.PropertyValueList_c)));
			}
			return list;
		}

		private static Scene.MetadataDefinitionList_c ConvertValue(Scene.MetadataDefinitionList s) {
			Scene.MetadataDefinitionList_c list =  new Scene.MetadataDefinitionList_c();
			Scene_MetadataDefinitionList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			for(int i = 0; i < (int)list.size; ++i) {
				Scene.PropertyValueList_c elt = ConvertValue(s.list[i]);
				IntPtr p = new IntPtr(list.ptr.ToInt64() + i * Marshal.SizeOf(typeof(Scene.PropertyValueList_c)));
				Marshal.StructureToPtr(elt, p, true);
			}
			return list;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_MetadataList_init(ref Scene.MetadataList_c list, UInt64 size);
		[DllImport(PiXYZImportSDK_dll)]
		private static extern void Scene_MetadataList_free(ref Scene.MetadataList_c list);

		private static Scene.MetadataList ConvertValue(Scene.MetadataList_c s) {
			Scene.MetadataList list = new Scene.MetadataList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Scene.MetadataList_c ConvertValue(Scene.MetadataList s) {
			Scene.MetadataList_c list =  new Scene.MetadataList_c();
			Scene_MetadataList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
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

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_getLastError();

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

		#region Algo

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_generateLOD(Scene.OccurrenceList_c occurrenceList, Core.IntList_c qualityList);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="occurrenceList"></param>
		/// <param name="qualityList"></param>
		public static void GenerateLOD(Scene.OccurrenceList occurrenceList, Core.IntList qualityList) {
			var occurrenceList_c = ConvertValue(occurrenceList);
			var qualityList_c = ConvertValue(qualityList);
			ImportSDK_generateLOD(occurrenceList_c, qualityList_c);
			Scene_OccurrenceList_free(ref occurrenceList_c);
			Core_IntList_free(ref qualityList_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_combineMeshesByMaterials(Scene.OccurrenceList_c occurrences, Int32 mergeNoMaterials, Int32 mergeHiddenPartsMode);
		/// <summary>
		/// Explode and (re)merge a set of mesh parts by visible materials
		/// </summary>
		/// <param name="occurrences">Occurrences of the parts to merge</param>
		/// <param name="mergeNoMaterials">If true, merge all parts with no active material together, else do not merge them</param>
		/// <param name="mergeHiddenPartsMode">Hidden parts handling mode, Destroy them, make visible or merge separately</param>
		public static Scene.OccurrenceList CombineMeshesByMaterials(Scene.OccurrenceList occurrences, Core.Bool mergeNoMaterials, Scene.MergeHiddenPartsMode mergeHiddenPartsMode) {
			var occurrences_c = ConvertValue(occurrences);
			var ret = ImportSDK_combineMeshesByMaterials(occurrences_c, mergeNoMaterials ? 1 : 0, (int)mergeHiddenPartsMode);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_createFreeEdgesFromPatches(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Create free edges from patch borders
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		public static void CreateFreeEdgesFromPatches(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_createFreeEdgesFromPatches(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_createInstancesBySimilarity(Scene.OccurrenceList_c occurrences, System.Double dimensionsSimilarity, System.Double polycountSimilarity, Int32 ignoreSymmetry, Int32 keepExistingPrototypes, Int32 createNewOccurrencesForPrototypes);
		/// <summary>
		/// Create instances when there are similar parts. This can be used to repair instances or to simplify a model that has similar parts that could be instanciated instead to reduce the number of unique meshes (reduces drawcalls, GPU memory usage and file size). Using 1.0 (100%) in all similarity criterias is non destructive. Using lower values will help finding more similar parts, even if their polycount or dimensions varies a bit.
		/// </summary>
		/// <param name="occurrences">Occurrence for which we want to find similar parts and create instances using prototypes.</param>
		/// <param name="dimensionsSimilarity">The percentage of similarity on dimensions. A value of 1.0 (100%) will find parts that have exactly the same dimensions. A lower value will increase the likelyhood to find similar parts, at the cost of precision.</param>
		/// <param name="polycountSimilarity">The percentage of similarity on polycount. A value of 1.0 (100%) will find parts that have exactly the same polycount. A lower value will increase the likelyhood to find similar parts, at the cost of precision.</param>
		/// <param name="ignoreSymmetry">If True, symmetries will be ignored, otherwise negative scaling will be applied in the occurrence transformation.</param>
		/// <param name="keepExistingPrototypes">If True, existing prototypes will be kept. Otherwise, the selection will be singularized and instanced will be created from scratch.</param>
		/// <param name="createNewOccurrencesForPrototypes">If True, a new occurrence will be created for each prototype. Those occurrences won't appear in the hierarchy, and so deleting one of the part in the scene has no risks of singularizing. If set to False, an arbitrary occurrence will be used as the prototype for other similar occurrences, which is less safe but will result in less occurrences.</param>
		public static void CreateInstancesBySimilarity(Scene.OccurrenceList occurrences, Core.Double dimensionsSimilarity, Core.Double polycountSimilarity, Core.Bool ignoreSymmetry, Core.Bool keepExistingPrototypes, Core.Bool createNewOccurrencesForPrototypes) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_createInstancesBySimilarity(occurrences_c, dimensionsSimilarity, polycountSimilarity, ignoreSymmetry ? 1 : 0, keepExistingPrototypes ? 1 : 0, createNewOccurrencesForPrototypes ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_createNormals(Scene.OccurrenceList_c occurrences, System.Double sharpEdge, Int32 overriding, Int32 useAreaWeighting);
		/// <summary>
		/// Create normal attributes on tessellations
		/// </summary>
		/// <param name="occurrences">Occurrences of components to create attributes</param>
		/// <param name="sharpEdge">Edges with an angle between their polygons greater than sharpEdge will be considered sharp (default use the Pixyz sharpAngle parameter)</param>
		/// <param name="overriding">If true, override existing normals, else only create normals on meshes without normals</param>
		/// <param name="useAreaWeighting">If true, normal computation will be weighted using polygon areas</param>
		public static void CreateNormals(Scene.OccurrenceList occurrences, Core.Double sharpEdge, Core.Bool overriding, Core.Bool useAreaWeighting) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_createNormals(occurrences_c, sharpEdge, overriding ? 1 : 0, useAreaWeighting ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_createTangents(Scene.OccurrenceList_c occurrences, System.Double sharpEdge, Int32 uvChannel, Int32 overriding);
		/// <summary>
		/// Create tangent attributes on tessellations
		/// </summary>
		/// <param name="occurrences">Occurrences of components to create attributes</param>
		/// <param name="sharpEdge">Edges with an angle between their polygons greater than sharpEdge will be considered sharp (default use the Pixyz sharpAngle parameter)</param>
		/// <param name="uvChannel">UV channel to use for the tangents creation</param>
		/// <param name="overriding">If true, override existing tangents, else only create tangents on meshes without tangents</param>
		public static void CreateTangents(Scene.OccurrenceList occurrences, Core.Double sharpEdge, Core.Int uvChannel, Core.Bool overriding) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_createTangents(occurrences_c, sharpEdge, uvChannel, overriding ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_decimate(Scene.OccurrenceList_c occurrences, System.Double surfacicTolerance, System.Double lineicTolerance, System.Double normalTolerance, System.Double texCoordTolerance, Int32 releaseConstraintOnSmallArea);
		/// <summary>
		/// reduce the polygon count by removing some vertices
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		/// <param name="surfacicTolerance">Maximum distance between surfacic vertices and resulting simplified surfaces</param>
		/// <param name="lineicTolerance">Maximum distance between lineic vertices and resulting simplified lines</param>
		/// <param name="normalTolerance">Maximum angle between original normals and those interpolated on the simplified surface</param>
		/// <param name="texCoordTolerance">Maximum distance (in UV space) between original texcoords and those interpolated on the simplified surface</param>
		/// <param name="releaseConstraintOnSmallArea">If True, release constraint of normal and/or texcoord tolerance on small areas (according to surfacicTolerance)</param>
		public static void Decimate(Scene.OccurrenceList occurrences, Core.Double surfacicTolerance, Core.Double lineicTolerance, Core.Double normalTolerance, Core.Double texCoordTolerance, Core.Bool releaseConstraintOnSmallArea) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_decimate(occurrences_c, surfacicTolerance, lineicTolerance, normalTolerance, texCoordTolerance, releaseConstraintOnSmallArea ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_decimatePointClouds(Scene.OccurrenceList_c occurrences, System.Double tolerance);
		/// <summary>
		/// decimate Point Cloud Occurences according to tolerance 
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="tolerance">Avarage distance between points</param>
		public static void DecimatePointClouds(Scene.OccurrenceList occurrences, Core.Double tolerance) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_decimatePointClouds(occurrences_c, tolerance);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_deleteFreeVertices(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Delete all free vertices of the mesh of given parts
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		public static void DeleteFreeVertices(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_deleteFreeVertices(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_deleteLines(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Delete all free line of the mesh of given parts
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		public static void DeleteLines(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_deleteLines(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_deletePatches(Scene.OccurrenceList_c occurrences, Int32 keepOnePatchByMaterial);
		/// <summary>
		/// Delete patches attributes on tessellations
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		/// <param name="keepOnePatchByMaterial">If set, one patch by material will be kept, else all patches will be deleted and materials on patches will be lost</param>
		public static void DeletePatches(Scene.OccurrenceList occurrences, Core.Bool keepOnePatchByMaterial) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_deletePatches(occurrences_c, keepOnePatchByMaterial ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_explodeConnectedMeshes(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Explode connected set of polygons to parts
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		public static void ExplodeConnectedMeshes(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_explodeConnectedMeshes(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_explodePartByMaterials(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Explode all parts by material
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		public static void ExplodePartByMaterials(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_explodePartByMaterials(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_explodePatches(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Explode all parts by patch
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		public static void ExplodePatches(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_explodePatches(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_explodeVertexCount(Scene.OccurrenceList_c occurrences, Int32 maxVertexCount, Int32 maxTriangleCount, Int32 countMergedVerticesOnce);
		/// <summary>
		/// Explode parts to respect a maximum vertex count
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="maxVertexCount">The maximum number of vertices by part</param>
		/// <param name="maxTriangleCount">The maximum number of triangles by part (quadrangles count twice)</param>
		/// <param name="countMergedVerticesOnce">If true, one vertex used in several triangles with different normals will be counted once (for Unity must be False)</param>
		public static void ExplodeVertexCount(Scene.OccurrenceList occurrences, Core.Int maxVertexCount, Core.Int maxTriangleCount, Core.Bool countMergedVerticesOnce) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_explodeVertexCount(occurrences_c, maxVertexCount, maxTriangleCount, countMergedVerticesOnce ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.BoolList_c ImportSDK_hiddenRemoval(Scene.OccurrenceList_c occurrences, Int32 level, Int32 resolution, Int32 sphereCount, System.Double fovX, Int32 considerTransparentOpaque, Int32 adjacencyDepth);
		/// <summary>
		/// Delete parts, patches or polygons not viewed from a sphere around the scene
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		/// <param name="level">Level of parts to remove : Parts, Patches or Polygons</param>
		/// <param name="resolution">Resolution of the visibility viewer</param>
		/// <param name="sphereCount">Segmentation of the sphere sphereCount x sphereCount</param>
		/// <param name="fovX">Horizontal field of view (in degree)</param>
		/// <param name="considerTransparentOpaque">If True, Parts, Patches or Polygons with a transparent appearance are considered as opaque</param>
		/// <param name="adjacencyDepth">Mark neighbors polygons as visible</param>
		public static Core.BoolList HiddenRemoval(Scene.OccurrenceList occurrences, Algo.SelectionLevel level, Core.Int resolution, Core.Int sphereCount, Core.Double fovX, Core.Bool considerTransparentOpaque, Core.Int adjacencyDepth) {
			var occurrences_c = ConvertValue(occurrences);
			var ret = ImportSDK_hiddenRemoval(occurrences_c, (int)level, resolution, sphereCount, fovX, considerTransparentOpaque ? 1 : 0, adjacencyDepth);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_BoolList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_removeHoles(Scene.OccurrenceList_c occurrences, Int32 throughHoles, Int32 blindHoles, Int32 surfacicHoles, System.Double maxDiameter, System.UInt32 fillWithMaterial);
		/// <summary>
		/// Remove some features from tessellations
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		/// <param name="throughHoles">Remove through holes</param>
		/// <param name="blindHoles">Remove blind holes</param>
		/// <param name="surfacicHoles">Remove surfacic holes</param>
		/// <param name="maxDiameter">Maximum diameter of the holes to be removed (-1=no max diameter)</param>
		/// <param name="fillWithMaterial">If set, the given material will be used to fill the holes</param>
		public static void RemoveHoles(Scene.OccurrenceList occurrences, Core.Bool throughHoles, Core.Bool blindHoles, Core.Bool surfacicHoles, Core.Double maxDiameter, Core.Ident fillWithMaterial) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_removeHoles(occurrences_c, throughHoles ? 1 : 0, blindHoles ? 1 : 0, surfacicHoles ? 1 : 0, maxDiameter, fillWithMaterial);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_repairCAD(Scene.OccurrenceList_c occurrences, System.Double tolerance, Int32 orient);
		/// <summary>
		/// Repair CAD shapes, assemble faces, remove duplicated faces, optimize loops and repair topology
		/// </summary>
		/// <param name="occurrences">Occurrences of components to clean</param>
		/// <param name="tolerance">Tolerance</param>
		/// <param name="orient">If true reorient the model</param>
		public static void RepairCAD(Scene.OccurrenceList occurrences, Core.Double tolerance, Core.Bool orient) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_repairCAD(occurrences_c, tolerance, orient ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_repairMesh(Scene.OccurrenceList_c occurrences, System.Double tolerance, Int32 crackNonManifold, Int32 orient);
		/// <summary>
		/// Launch the repair process to repair a disconnected or not clean tessellation
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		/// <param name="tolerance">Connection tolerance</param>
		/// <param name="crackNonManifold">At the end of the repair process, crack resulting non-manifold edges</param>
		/// <param name="orient">If true reorient the model</param>
		public static void RepairMesh(Scene.OccurrenceList occurrences, Core.Double tolerance, Core.Bool crackNonManifold, Core.Bool orient) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_repairMesh(occurrences_c, tolerance, crackNonManifold ? 1 : 0, orient ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.BoolList_c ImportSDK_smartHiddenRemoval(Scene.OccurrenceList_c occurrences, Int32 level, System.Double voxelSize, System.Double minimumCavityVolume, Int32 resolution, Int32 mode, Int32 considerTransparentOpaque, Int32 adjacencyDepth);
		/// <summary>
		/// Delete parts, patches or polygons not viewed from a set of camera automatically generated
		/// </summary>
		/// <param name="occurrences">Occurrences of components to process</param>
		/// <param name="level">Level of parts to remove : Parts, Patches or Polygons</param>
		/// <param name="voxelSize">Size of the voxels in mm (smaller it is, more viewpoints there are)</param>
		/// <param name="minimumCavityVolume">Minimum volume of a cavity in cubic meter (smaller it is, more viewpoints there are)</param>
		/// <param name="resolution">Resolution of the visibility viewer</param>
		/// <param name="mode">Select where to place camera (all cavities, only outer or only inner cavities)</param>
		/// <param name="considerTransparentOpaque">If True, Parts, Patches or Polygons with a transparent appearance are considered as opaque</param>
		/// <param name="adjacencyDepth">Mark neighbors polygons as visible</param>
		public static Core.BoolList SmartHiddenRemoval(Scene.OccurrenceList occurrences, Algo.SelectionLevel level, Core.Double voxelSize, Core.Double minimumCavityVolume, Core.Int resolution, Algo.SmartHiddenType mode, Core.Bool considerTransparentOpaque, Core.Int adjacencyDepth) {
			var occurrences_c = ConvertValue(occurrences);
			var ret = ImportSDK_smartHiddenRemoval(occurrences_c, (int)level, voxelSize, minimumCavityVolume, resolution, (int)mode, considerTransparentOpaque ? 1 : 0, adjacencyDepth);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_BoolList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_tessellate(Scene.OccurrenceList_c occurrences, System.Double maxSag, System.Double maxLength, System.Double maxAngle, Int32 createNormals, Int32 uvMode, Int32 uvChannel, System.Double uvPadding, Int32 createTangents, Int32 createFreeEdges, Int32 keepBRepShape, Int32 overrideExistingTessellation);
		/// <summary>
		/// Create a tessellated representation from a CAD representation for each given part
		/// </summary>
		/// <param name="occurrences">Occurrences of components to tessellate</param>
		/// <param name="maxSag">Maximum distance between the geometry and the tessellation</param>
		/// <param name="maxLength">Maximum length of elements</param>
		/// <param name="maxAngle">Maximum angle between normals of two adjacent elements</param>
		/// <param name="createNormals">If true, normals will be generated</param>
		/// <param name="uvMode">Select the texture coordinates generation mode</param>
		/// <param name="uvChannel">The UV channel of the generated texture coordinates (if any)</param>
		/// <param name="uvPadding">The UV padding between UV island in UV coordinate space (between 0-1). This parameter is handled as an heuristic so it might not be respected</param>
		/// <param name="createTangents">If true, tangents will be generated</param>
		/// <param name="createFreeEdges">If true, free edges will be created for each patch borders</param>
		/// <param name="keepBRepShape">If true, BRep shapes will be kept for Back to Brep or Retessellate</param>
		/// <param name="overrideExistingTessellation">If true, already tessellated parts will be re-tessellated</param>
		public static void Tessellate(Scene.OccurrenceList occurrences, Core.Double maxSag, Core.Double maxLength, Core.Double maxAngle, Core.Bool createNormals, Algo.UVGenerationMode uvMode, Core.Int uvChannel, Core.Double uvPadding, Core.Bool createTangents, Core.Bool createFreeEdges, Core.Bool keepBRepShape, Core.Bool overrideExistingTessellation) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_tessellate(occurrences_c, maxSag, maxLength, maxAngle, createNormals ? 1 : 0, (int)uvMode, uvChannel, uvPadding, createTangents ? 1 : 0, createFreeEdges ? 1 : 0, keepBRepShape ? 1 : 0, overrideExistingTessellation ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_voxelizePointClouds(Scene.OccurrenceList_c occurrences, System.Double voxelSize);
		/// <summary>
		/// Explode point clouds to voxels
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="voxelSize">Size of voxels</param>
		public static void VoxelizePointClouds(Scene.OccurrenceList occurrences, Core.Double voxelSize) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_voxelizePointClouds(occurrences_c, voxelSize);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region BRep

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Curvatures_c ImportSDK_evalCurvatureOnSurface(System.UInt32 surface, Geom.Point2_c parameter);
		/// <summary>
		/// evaluate main curvatures on a surface
		/// </summary>
		/// <param name="surface">The surface</param>
		/// <param name="parameter">Parameter to evaluate</param>
		public static Geom.Curvatures EvalCurvatureOnSurface(Core.Ident surface, Geom.Point2 parameter) {
			var parameter_c = ConvertValue(parameter);
			var ret = ImportSDK_evalCurvatureOnSurface(surface, parameter_c);
			Geom_Point2_free(ref parameter_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Curvatures_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct evalOnCurveReturn_c
		{
			internal Geom.Point3_c d0;
			internal Geom.Point3_c du;
			internal Geom.Point3_c d2u;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern evalOnCurveReturn_c ImportSDK_evalOnCurve(System.UInt32 curve, System.Double parameter, Int32 derivation);
		/// <summary>
		/// evaluate a point and derivatives on a curve
		/// </summary>
		/// <param name="curve">The curve</param>
		/// <param name="parameter">Parameter to evaluate</param>
		/// <param name="derivation">Derivation level (0,1,2)</param>
		public struct evalOnCurveReturn
		{
			public Geom.Point3 d0;
			public Geom.Point3 du;
			public Geom.Point3 d2u;
		}

		public static evalOnCurveReturn EvalOnCurve(Core.Ident curve, Core.Double parameter, Core.Int derivation) {
			var ret = ImportSDK_evalOnCurve(curve, parameter, derivation);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			evalOnCurveReturn retStruct = new evalOnCurveReturn();
			retStruct.d0 = ConvertValue(ret.d0);
			Geom_Point3_free(ref ret.d0);
			retStruct.du = ConvertValue(ret.du);
			Geom_Point3_free(ref ret.du);
			retStruct.d2u = ConvertValue(ret.d2u);
			Geom_Point3_free(ref ret.d2u);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct evalOnSurfaceReturn_c
		{
			internal Geom.Point3_c d0;
			internal Geom.Point3_c du;
			internal Geom.Point3_c dv;
			internal Geom.Point3_c d2u;
			internal Geom.Point3_c d2v;
			internal Geom.Point3_c duv;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern evalOnSurfaceReturn_c ImportSDK_evalOnSurface(System.UInt32 surface, Geom.Point2_c parameter, Int32 derivation);
		/// <summary>
		/// evaluate a point and derivatives on a surface
		/// </summary>
		/// <param name="surface">The surface</param>
		/// <param name="parameter">Parameter to evaluate</param>
		/// <param name="derivation">Derivation level (0,1,2)</param>
		public struct evalOnSurfaceReturn
		{
			public Geom.Point3 d0;
			public Geom.Point3 du;
			public Geom.Point3 dv;
			public Geom.Point3 d2u;
			public Geom.Point3 d2v;
			public Geom.Point3 duv;
		}

		public static evalOnSurfaceReturn EvalOnSurface(Core.Ident surface, Geom.Point2 parameter, Core.Int derivation) {
			var parameter_c = ConvertValue(parameter);
			var ret = ImportSDK_evalOnSurface(surface, parameter_c, derivation);
			Geom_Point2_free(ref parameter_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			evalOnSurfaceReturn retStruct = new evalOnSurfaceReturn();
			retStruct.d0 = ConvertValue(ret.d0);
			Geom_Point3_free(ref ret.d0);
			retStruct.du = ConvertValue(ret.du);
			Geom_Point3_free(ref ret.du);
			retStruct.dv = ConvertValue(ret.dv);
			Geom_Point3_free(ref ret.dv);
			retStruct.d2u = ConvertValue(ret.d2u);
			Geom_Point3_free(ref ret.d2u);
			retStruct.d2v = ConvertValue(ret.d2v);
			Geom_Point3_free(ref ret.d2v);
			retStruct.duv = ConvertValue(ret.duv);
			Geom_Point3_free(ref ret.duv);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.FaceList_c ImportSDK_getAllModelFaces(System.UInt32 model);
		/// <summary>
		/// Get all the face of a model recursively
		/// </summary>
		/// <param name="model">Model</param>
		public static CAD.FaceList GetAllModelFaces(Core.Ident model) {
			var ret = ImportSDK_getAllModelFaces(model);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_FaceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.ClosedShellList_c ImportSDK_getBodyClosedShells(System.UInt32 body);
		/// <summary>
		/// get all closedShells contain in the body
		/// </summary>
		/// <param name="body">The body</param>
		public static CAD.ClosedShellList GetBodyClosedShells(Core.Ident body) {
			var ret = ImportSDK_getBodyClosedShells(body);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_ClosedShellList_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getBoundedCurveDefinitionReturn_c
		{
			internal System.UInt32 curve;
			internal CAD.Bounds1D_c bounds;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getBoundedCurveDefinitionReturn_c ImportSDK_getBoundedCurveDefinition(System.UInt32 boundedCurve);
		/// <summary>
		/// get all parameters contained in the boundedCurve
		/// </summary>
		/// <param name="boundedCurve">The boundedCurve</param>
		public struct getBoundedCurveDefinitionReturn
		{
			public System.UInt32 curve;
			public CAD.Bounds1D bounds;
		}

		public static getBoundedCurveDefinitionReturn GetBoundedCurveDefinition(Core.Ident boundedCurve) {
			var ret = ImportSDK_getBoundedCurveDefinition(boundedCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getBoundedCurveDefinitionReturn retStruct = new getBoundedCurveDefinitionReturn();
			retStruct.curve = (Core.Ident)ret.curve;
			retStruct.bounds = ConvertValue(ret.bounds);
			CAD_Bounds1D_free(ref ret.bounds);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getCircleCurveDefinitionReturn_c
		{
			internal System.Double radius;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getCircleCurveDefinitionReturn_c ImportSDK_getCircleCurveDefinition(System.UInt32 circleCurve);
		/// <summary>
		/// get all parameters contained in the circleCurve
		/// </summary>
		/// <param name="circleCurve">The circleCurve</param>
		public struct getCircleCurveDefinitionReturn
		{
			public System.Double radius;
			public Geom.Matrix4 matrix;
		}

		public static getCircleCurveDefinitionReturn GetCircleCurveDefinition(Core.Ident circleCurve) {
			var ret = ImportSDK_getCircleCurveDefinition(circleCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getCircleCurveDefinitionReturn retStruct = new getCircleCurveDefinitionReturn();
			retStruct.radius = (Core.Double)ret.radius;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.OrientedDomainList_c ImportSDK_getClosedShellOrientedDomains(System.UInt32 closedShell);
		/// <summary>
		/// get all orienteDomains contain in the closedShell
		/// </summary>
		/// <param name="closedShell">The closedShell</param>
		public static CAD.OrientedDomainList GetClosedShellOrientedDomains(Core.Ident closedShell) {
			var ret = ImportSDK_getClosedShellOrientedDomains(closedShell);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_OrientedDomainList_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getCoEdgeDefinitionReturn_c
		{
			internal System.UInt32 edge;
			internal Int32 edgeOrientation;
			internal System.UInt32 loop;
			internal System.UInt32 surface;
			internal System.UInt32 parametricCurve;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getCoEdgeDefinitionReturn_c ImportSDK_getCoEdgeDefinition(System.UInt32 coEdge);
		/// <summary>
		/// get all parameters contained in the coEdge
		/// </summary>
		/// <param name="coEdge">The coEdge</param>
		public struct getCoEdgeDefinitionReturn
		{
			public System.UInt32 edge;
			public System.Boolean edgeOrientation;
			public System.UInt32 loop;
			public System.UInt32 surface;
			public System.UInt32 parametricCurve;
		}

		public static getCoEdgeDefinitionReturn GetCoEdgeDefinition(Core.Ident coEdge) {
			var ret = ImportSDK_getCoEdgeDefinition(coEdge);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getCoEdgeDefinitionReturn retStruct = new getCoEdgeDefinitionReturn();
			retStruct.edge = (Core.Ident)ret.edge;
			retStruct.edgeOrientation = ConvertValue(ret.edgeOrientation);
			retStruct.loop = (Core.Ident)ret.loop;
			retStruct.surface = (Core.Ident)ret.surface;
			retStruct.parametricCurve = (Core.Ident)ret.parametricCurve;
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getCompositeCurveDefinitionReturn_c
		{
			internal CAD.LimitedCurveList_c curves;
			internal Core.DoubleList_c parameters;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getCompositeCurveDefinitionReturn_c ImportSDK_getCompositeCurveDefinition(System.UInt32 compositeCurve);
		/// <summary>
		/// get all parameters contained in the compositeCurve
		/// </summary>
		/// <param name="compositeCurve">The compositeCurve</param>
		public struct getCompositeCurveDefinitionReturn
		{
			public CAD.LimitedCurveList curves;
			public Core.DoubleList parameters;
		}

		public static getCompositeCurveDefinitionReturn GetCompositeCurveDefinition(Core.Ident compositeCurve) {
			var ret = ImportSDK_getCompositeCurveDefinition(compositeCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getCompositeCurveDefinitionReturn retStruct = new getCompositeCurveDefinitionReturn();
			retStruct.curves = ConvertValue(ret.curves);
			CAD_LimitedCurveList_free(ref ret.curves);
			retStruct.parameters = ConvertValue(ret.parameters);
			Core_DoubleList_free(ref ret.parameters);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getConeSurfaceDefinitionReturn_c
		{
			internal System.Double radius;
			internal System.Double semiAngle;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getConeSurfaceDefinitionReturn_c ImportSDK_getConeSurfaceDefinition(System.UInt32 coneSurface);
		/// <summary>
		/// get all parameters contained in the coneSurface
		/// </summary>
		/// <param name="coneSurface">The coneSurface</param>
		public struct getConeSurfaceDefinitionReturn
		{
			public System.Double radius;
			public System.Double semiAngle;
			public Geom.Matrix4 matrix;
		}

		public static getConeSurfaceDefinitionReturn GetConeSurfaceDefinition(Core.Ident coneSurface) {
			var ret = ImportSDK_getConeSurfaceDefinition(coneSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getConeSurfaceDefinitionReturn retStruct = new getConeSurfaceDefinitionReturn();
			retStruct.radius = (Core.Double)ret.radius;
			retStruct.semiAngle = (Core.Double)ret.semiAngle;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getCurveExtrusionSurfaceDefinitionReturn_c
		{
			internal System.UInt32 generatrixCurve;
			internal System.UInt32 directrixCruve;
			internal System.UInt32 surfaceReference;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getCurveExtrusionSurfaceDefinitionReturn_c ImportSDK_getCurveExtrusionSurfaceDefinition(System.UInt32 curveExtrusionSurface);
		/// <summary>
		/// get all parameters contained in the curveExtrusionSurface
		/// </summary>
		/// <param name="curveExtrusionSurface">The curveExtrusionSurface</param>
		public struct getCurveExtrusionSurfaceDefinitionReturn
		{
			public System.UInt32 generatrixCurve;
			public System.UInt32 directrixCruve;
			public System.UInt32 surfaceReference;
		}

		public static getCurveExtrusionSurfaceDefinitionReturn GetCurveExtrusionSurfaceDefinition(Core.Ident curveExtrusionSurface) {
			var ret = ImportSDK_getCurveExtrusionSurfaceDefinition(curveExtrusionSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getCurveExtrusionSurfaceDefinitionReturn retStruct = new getCurveExtrusionSurfaceDefinitionReturn();
			retStruct.generatrixCurve = (Core.Ident)ret.generatrixCurve;
			retStruct.directrixCruve = (Core.Ident)ret.directrixCruve;
			retStruct.surfaceReference = (Core.Ident)ret.surfaceReference;
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.Bounds1D_c ImportSDK_getCurveLimits(System.UInt32 curve);
		/// <summary>
		/// get the parametric space limits of a curve
		/// </summary>
		/// <param name="curve">The curve</param>
		public static CAD.Bounds1D GetCurveLimits(Core.Ident curve) {
			var ret = ImportSDK_getCurveLimits(curve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_Bounds1D_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getCylinderSurfaceDefinitionReturn_c
		{
			internal System.Double radius;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getCylinderSurfaceDefinitionReturn_c ImportSDK_getCylinderSurfaceDefinition(System.UInt32 cylinderSurface);
		/// <summary>
		/// get all parameters contained in the cylinderSurface
		/// </summary>
		/// <param name="cylinderSurface">The cylinderSurface</param>
		public struct getCylinderSurfaceDefinitionReturn
		{
			public System.Double radius;
			public Geom.Matrix4 matrix;
		}

		public static getCylinderSurfaceDefinitionReturn GetCylinderSurfaceDefinition(Core.Ident cylinderSurface) {
			var ret = ImportSDK_getCylinderSurfaceDefinition(cylinderSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getCylinderSurfaceDefinitionReturn retStruct = new getCylinderSurfaceDefinitionReturn();
			retStruct.radius = (Core.Double)ret.radius;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getEdgeDefinitionReturn_c
		{
			internal System.UInt32 vertex1;
			internal System.UInt32 vertex2;
			internal System.UInt32 curve;
			internal CAD.Bounds1D_c bounds;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getEdgeDefinitionReturn_c ImportSDK_getEdgeDefinition(System.UInt32 edge);
		/// <summary>
		/// get all parameters contained in the edge
		/// </summary>
		/// <param name="edge">The edge</param>
		public struct getEdgeDefinitionReturn
		{
			public System.UInt32 vertex1;
			public System.UInt32 vertex2;
			public System.UInt32 curve;
			public CAD.Bounds1D bounds;
		}

		public static getEdgeDefinitionReturn GetEdgeDefinition(Core.Ident edge) {
			var ret = ImportSDK_getEdgeDefinition(edge);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getEdgeDefinitionReturn retStruct = new getEdgeDefinitionReturn();
			retStruct.vertex1 = (Core.Ident)ret.vertex1;
			retStruct.vertex2 = (Core.Ident)ret.vertex2;
			retStruct.curve = (Core.Ident)ret.curve;
			retStruct.bounds = ConvertValue(ret.bounds);
			CAD_Bounds1D_free(ref ret.bounds);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getEllipseCurveDefinitionReturn_c
		{
			internal System.Double radius1;
			internal System.Double radius2;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getEllipseCurveDefinitionReturn_c ImportSDK_getEllipseCurveDefinition(System.UInt32 ellipseCurve);
		/// <summary>
		/// get all parameters contained in the ellipseCurve
		/// </summary>
		/// <param name="ellipseCurve">The ellipseCurve</param>
		public struct getEllipseCurveDefinitionReturn
		{
			public System.Double radius1;
			public System.Double radius2;
			public Geom.Matrix4 matrix;
		}

		public static getEllipseCurveDefinitionReturn GetEllipseCurveDefinition(Core.Ident ellipseCurve) {
			var ret = ImportSDK_getEllipseCurveDefinition(ellipseCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getEllipseCurveDefinitionReturn retStruct = new getEllipseCurveDefinitionReturn();
			retStruct.radius1 = (Core.Double)ret.radius1;
			retStruct.radius2 = (Core.Double)ret.radius2;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getEllipticConeSurfaceDefinitionReturn_c
		{
			internal System.Double radius1;
			internal System.Double radius2;
			internal System.Double semiAngle;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getEllipticConeSurfaceDefinitionReturn_c ImportSDK_getEllipticConeSurfaceDefinition(System.UInt32 ellipticConeSurface);
		/// <summary>
		/// get all parameters contained in the ellipticConeSurface
		/// </summary>
		/// <param name="ellipticConeSurface">The EllipticConeSurface</param>
		public struct getEllipticConeSurfaceDefinitionReturn
		{
			public System.Double radius1;
			public System.Double radius2;
			public System.Double semiAngle;
			public Geom.Matrix4 matrix;
		}

		public static getEllipticConeSurfaceDefinitionReturn GetEllipticConeSurfaceDefinition(Core.Ident ellipticConeSurface) {
			var ret = ImportSDK_getEllipticConeSurfaceDefinition(ellipticConeSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getEllipticConeSurfaceDefinitionReturn retStruct = new getEllipticConeSurfaceDefinitionReturn();
			retStruct.radius1 = (Core.Double)ret.radius1;
			retStruct.radius2 = (Core.Double)ret.radius2;
			retStruct.semiAngle = (Core.Double)ret.semiAngle;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getFaceDefinitionReturn_c
		{
			internal System.UInt32 surface;
			internal CAD.LoopList_c loops;
			internal Int32 orientation;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getFaceDefinitionReturn_c ImportSDK_getFaceDefinition(System.UInt32 face);
		/// <summary>
		/// get all parameters contain in the face
		/// </summary>
		/// <param name="face">The face</param>
		public struct getFaceDefinitionReturn
		{
			public System.UInt32 surface;
			public CAD.LoopList loops;
			public System.Boolean orientation;
		}

		public static getFaceDefinitionReturn GetFaceDefinition(Core.Ident face) {
			var ret = ImportSDK_getFaceDefinition(face);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getFaceDefinitionReturn retStruct = new getFaceDefinitionReturn();
			retStruct.surface = (Core.Ident)ret.surface;
			retStruct.loops = ConvertValue(ret.loops);
			CAD_LoopList_free(ref ret.loops);
			retStruct.orientation = ConvertValue(ret.orientation);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_getFaceMaterial(System.UInt32 face);
		/// <summary>
		/// get the material on a face
		/// </summary>
		/// <param name="face">The face</param>
		public static Core.Ident GetFaceMaterial(Core.Ident face) {
			var ret = ImportSDK_getFaceMaterial(face);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Point2ListList_c ImportSDK_getFaceParametricBoundaries(System.UInt32 face);
		/// <summary>
		/// get parametric definition of each face loop
		/// </summary>
		/// <param name="face">The face</param>
		public static Geom.Point2ListList GetFaceParametricBoundaries(Core.Ident face) {
			var ret = ImportSDK_getFaceParametricBoundaries(face);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Point2ListList_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getHelixCurveDefinitionReturn_c
		{
			internal System.Double radius;
			internal Geom.Matrix4_c matrix;
			internal Int32 trigonometricOrientation;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getHelixCurveDefinitionReturn_c ImportSDK_getHelixCurveDefinition(System.UInt32 helixCurve);
		/// <summary>
		/// get all parameters contained in the helixCurve
		/// </summary>
		/// <param name="helixCurve">The helixCurve</param>
		public struct getHelixCurveDefinitionReturn
		{
			public System.Double radius;
			public Geom.Matrix4 matrix;
			public System.Boolean trigonometricOrientation;
		}

		public static getHelixCurveDefinitionReturn GetHelixCurveDefinition(Core.Ident helixCurve) {
			var ret = ImportSDK_getHelixCurveDefinition(helixCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getHelixCurveDefinitionReturn retStruct = new getHelixCurveDefinitionReturn();
			retStruct.radius = (Core.Double)ret.radius;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			retStruct.trigonometricOrientation = ConvertValue(ret.trigonometricOrientation);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getHermiteCurveDefinitionReturn_c
		{
			internal Geom.Point3_c firstPoint;
			internal Geom.Point3_c secondPoint;
			internal Geom.Point3_c firstTangent;
			internal Geom.Point3_c secondTangent;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getHermiteCurveDefinitionReturn_c ImportSDK_getHermiteCurveDefinition(System.UInt32 hermiteCurve);
		/// <summary>
		/// get all parameters contained in the hermiteCurve
		/// </summary>
		/// <param name="hermiteCurve">The HermiteCurve</param>
		public struct getHermiteCurveDefinitionReturn
		{
			public Geom.Point3 firstPoint;
			public Geom.Point3 secondPoint;
			public Geom.Point3 firstTangent;
			public Geom.Point3 secondTangent;
		}

		public static getHermiteCurveDefinitionReturn GetHermiteCurveDefinition(Core.Ident hermiteCurve) {
			var ret = ImportSDK_getHermiteCurveDefinition(hermiteCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getHermiteCurveDefinitionReturn retStruct = new getHermiteCurveDefinitionReturn();
			retStruct.firstPoint = ConvertValue(ret.firstPoint);
			Geom_Point3_free(ref ret.firstPoint);
			retStruct.secondPoint = ConvertValue(ret.secondPoint);
			Geom_Point3_free(ref ret.secondPoint);
			retStruct.firstTangent = ConvertValue(ret.firstTangent);
			Geom_Point3_free(ref ret.firstTangent);
			retStruct.secondTangent = ConvertValue(ret.secondTangent);
			Geom_Point3_free(ref ret.secondTangent);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getHyperbolaCurveDefinitionReturn_c
		{
			internal System.Double radius1;
			internal System.Double radius2;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getHyperbolaCurveDefinitionReturn_c ImportSDK_getHyperbolaCurveDefinition(System.UInt32 hyperbolaCurve);
		/// <summary>
		/// get all parameters contained in the hyperbolaCurve
		/// </summary>
		/// <param name="hyperbolaCurve">The hyperbolaCurve</param>
		public struct getHyperbolaCurveDefinitionReturn
		{
			public System.Double radius1;
			public System.Double radius2;
			public Geom.Matrix4 matrix;
		}

		public static getHyperbolaCurveDefinitionReturn GetHyperbolaCurveDefinition(Core.Ident hyperbolaCurve) {
			var ret = ImportSDK_getHyperbolaCurveDefinition(hyperbolaCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getHyperbolaCurveDefinitionReturn retStruct = new getHyperbolaCurveDefinitionReturn();
			retStruct.radius1 = (Core.Double)ret.radius1;
			retStruct.radius2 = (Core.Double)ret.radius2;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getLineCurveDefinitionReturn_c
		{
			internal Geom.Point3_c origin;
			internal Geom.Point3_c direction;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getLineCurveDefinitionReturn_c ImportSDK_getLineCurveDefinition(System.UInt32 lineCurve);
		/// <summary>
		/// get all parameters contain in the lineCurve
		/// </summary>
		/// <param name="lineCurve">The lineCurve</param>
		public struct getLineCurveDefinitionReturn
		{
			public Geom.Point3 origin;
			public Geom.Point3 direction;
		}

		public static getLineCurveDefinitionReturn GetLineCurveDefinition(Core.Ident lineCurve) {
			var ret = ImportSDK_getLineCurveDefinition(lineCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getLineCurveDefinitionReturn retStruct = new getLineCurveDefinitionReturn();
			retStruct.origin = ConvertValue(ret.origin);
			Geom_Point3_free(ref ret.origin);
			retStruct.direction = ConvertValue(ret.direction);
			Geom_Point3_free(ref ret.direction);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.CoEdgeList_c ImportSDK_getLoopCoEdges(System.UInt32 loop);
		/// <summary>
		/// get all coEdges contain in the loop
		/// </summary>
		/// <param name="loop">The loop</param>
		public static CAD.CoEdgeList GetLoopCoEdges(Core.Ident loop) {
			var ret = ImportSDK_getLoopCoEdges(loop);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_CoEdgeList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.BodyList_c ImportSDK_getModelBodies(System.UInt32 model);
		/// <summary>
		/// Get the list of bodies contained in a model
		/// </summary>
		/// <param name="model">Model</param>
		public static CAD.BodyList GetModelBodies(Core.Ident model) {
			var ret = ImportSDK_getModelBodies(model);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_BodyList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.EdgeListList_c ImportSDK_getModelBoundaries(System.UInt32 model);
		/// <summary>
		/// Get boundary edges of a model grouped by cycles
		/// </summary>
		/// <param name="model">Model</param>
		public static CAD.EdgeListList GetModelBoundaries(Core.Ident model) {
			var ret = ImportSDK_getModelBoundaries(model);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_EdgeListList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.DomainList_c ImportSDK_getModelDomains(System.UInt32 model);
		/// <summary>
		/// Get the list of domains (Face or OpenShell) contained in a model
		/// </summary>
		/// <param name="model">Model</param>
		public static CAD.DomainList GetModelDomains(Core.Ident model) {
			var ret = ImportSDK_getModelDomains(model);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_DomainList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.EdgeList_c ImportSDK_getModelEdges(System.UInt32 model);
		/// <summary>
		/// Get the list of free edges contained in a model
		/// </summary>
		/// <param name="model">Model</param>
		public static CAD.EdgeList GetModelEdges(Core.Ident model) {
			var ret = ImportSDK_getModelEdges(model);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_EdgeList_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getNURBSCurveDefinitionReturn_c
		{
			internal Int32 degree;
			internal Core.DoubleList_c knots;
			internal Geom.Point3List_c poles;
			internal Core.DoubleList_c weights;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getNURBSCurveDefinitionReturn_c ImportSDK_getNURBSCurveDefinition(System.UInt32 nurbsCurve);
		/// <summary>
		/// get all parameters contained in the nurbsCurve
		/// </summary>
		/// <param name="nurbsCurve">The nurbsCurve</param>
		public struct getNURBSCurveDefinitionReturn
		{
			public System.Int32 degree;
			public Core.DoubleList knots;
			public Geom.Point3List poles;
			public Core.DoubleList weights;
		}

		public static getNURBSCurveDefinitionReturn GetNURBSCurveDefinition(Core.Ident nurbsCurve) {
			var ret = ImportSDK_getNURBSCurveDefinition(nurbsCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getNURBSCurveDefinitionReturn retStruct = new getNURBSCurveDefinitionReturn();
			retStruct.degree = (Core.Int)ret.degree;
			retStruct.knots = ConvertValue(ret.knots);
			Core_DoubleList_free(ref ret.knots);
			retStruct.poles = ConvertValue(ret.poles);
			Geom_Point3List_free(ref ret.poles);
			retStruct.weights = ConvertValue(ret.weights);
			Core_DoubleList_free(ref ret.weights);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getNURBSSurfaceDefinitionReturn_c
		{
			internal Int32 degreeU;
			internal Int32 degreeV;
			internal Core.DoubleList_c knotsU;
			internal Core.DoubleList_c knotsV;
			internal Geom.Point3ListList_c poles;
			internal Core.DoubleListList_c weights;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getNURBSSurfaceDefinitionReturn_c ImportSDK_getNURBSSurfaceDefinition(System.UInt32 nurbsSurface);
		/// <summary>
		/// get all parameters contained in the nurbsSurface
		/// </summary>
		/// <param name="nurbsSurface">The nurbsSurface</param>
		public struct getNURBSSurfaceDefinitionReturn
		{
			public System.Int32 degreeU;
			public System.Int32 degreeV;
			public Core.DoubleList knotsU;
			public Core.DoubleList knotsV;
			public Geom.Point3ListList poles;
			public Core.DoubleListList weights;
		}

		public static getNURBSSurfaceDefinitionReturn GetNURBSSurfaceDefinition(Core.Ident nurbsSurface) {
			var ret = ImportSDK_getNURBSSurfaceDefinition(nurbsSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getNURBSSurfaceDefinitionReturn retStruct = new getNURBSSurfaceDefinitionReturn();
			retStruct.degreeU = (Core.Int)ret.degreeU;
			retStruct.degreeV = (Core.Int)ret.degreeV;
			retStruct.knotsU = ConvertValue(ret.knotsU);
			Core_DoubleList_free(ref ret.knotsU);
			retStruct.knotsV = ConvertValue(ret.knotsV);
			Core_DoubleList_free(ref ret.knotsV);
			retStruct.poles = ConvertValue(ret.poles);
			Geom_Point3ListList_free(ref ret.poles);
			retStruct.weights = ConvertValue(ret.weights);
			Core_DoubleListList_free(ref ret.weights);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getOffsetSurfaceDefinitionReturn_c
		{
			internal System.UInt32 baseSurface;
			internal System.Double distance;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getOffsetSurfaceDefinitionReturn_c ImportSDK_getOffsetSurfaceDefinition(System.UInt32 offsetSurface);
		/// <summary>
		/// get all parameters contained in the offsetSurface
		/// </summary>
		/// <param name="offsetSurface">The offsetSurface</param>
		public struct getOffsetSurfaceDefinitionReturn
		{
			public System.UInt32 baseSurface;
			public System.Double distance;
		}

		public static getOffsetSurfaceDefinitionReturn GetOffsetSurfaceDefinition(Core.Ident offsetSurface) {
			var ret = ImportSDK_getOffsetSurfaceDefinition(offsetSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getOffsetSurfaceDefinitionReturn retStruct = new getOffsetSurfaceDefinitionReturn();
			retStruct.baseSurface = (Core.Ident)ret.baseSurface;
			retStruct.distance = (Core.Double)ret.distance;
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.OrientedDomainList_c ImportSDK_getOpenShellOrientedDomains(System.UInt32 openShell);
		/// <summary>
		/// get all orienteDomains contain in the openShell
		/// </summary>
		/// <param name="openShell">The openShell</param>
		public static CAD.OrientedDomainList GetOpenShellOrientedDomains(Core.Ident openShell) {
			var ret = ImportSDK_getOpenShellOrientedDomains(openShell);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_OrientedDomainList_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getParabolaCurveDefinitionReturn_c
		{
			internal System.Double focalLength;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getParabolaCurveDefinitionReturn_c ImportSDK_getParabolaCurveDefinition(System.UInt32 parabolaCurve);
		/// <summary>
		/// get all parameters contained in the parabolaCurve
		/// </summary>
		/// <param name="parabolaCurve">The parabolaCurve</param>
		public struct getParabolaCurveDefinitionReturn
		{
			public System.Double focalLength;
			public Geom.Matrix4 matrix;
		}

		public static getParabolaCurveDefinitionReturn GetParabolaCurveDefinition(Core.Ident parabolaCurve) {
			var ret = ImportSDK_getParabolaCurveDefinition(parabolaCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getParabolaCurveDefinitionReturn retStruct = new getParabolaCurveDefinitionReturn();
			retStruct.focalLength = (Core.Double)ret.focalLength;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Matrix4_c ImportSDK_getPlaneSurfaceDefinition(System.UInt32 planeSurface);
		/// <summary>
		/// get all parameters contained in the planeSurface
		/// </summary>
		/// <param name="planeSurface">The planeSurface</param>
		public static Geom.Matrix4 GetPlaneSurfaceDefinition(Core.Ident planeSurface) {
			var ret = ImportSDK_getPlaneSurfaceDefinition(planeSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Matrix4_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getPolylineCurveDefinitionReturn_c
		{
			internal Geom.Point3List_c points;
			internal Core.DoubleList_c parameters;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getPolylineCurveDefinitionReturn_c ImportSDK_getPolylineCurveDefinition(System.UInt32 polylineCurve);
		/// <summary>
		/// get all parameters contained in the polylinCurve
		/// </summary>
		/// <param name="polylineCurve">The polylineCurve</param>
		public struct getPolylineCurveDefinitionReturn
		{
			public Geom.Point3List points;
			public Core.DoubleList parameters;
		}

		public static getPolylineCurveDefinitionReturn GetPolylineCurveDefinition(Core.Ident polylineCurve) {
			var ret = ImportSDK_getPolylineCurveDefinition(polylineCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getPolylineCurveDefinitionReturn retStruct = new getPolylineCurveDefinitionReturn();
			retStruct.points = ConvertValue(ret.points);
			Geom_Point3List_free(ref ret.points);
			retStruct.parameters = ConvertValue(ret.parameters);
			Core_DoubleList_free(ref ret.parameters);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getRevolutionSurfaceDefinitionReturn_c
		{
			internal System.UInt32 generatricCurve;
			internal Geom.Point3_c axisOrigin;
			internal Geom.Point3_c axisDirection;
			internal System.Double startAngle;
			internal System.Double endAngle;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getRevolutionSurfaceDefinitionReturn_c ImportSDK_getRevolutionSurfaceDefinition(System.UInt32 revolutionSurface);
		/// <summary>
		/// get all parameters contained in the revolutionSurface
		/// </summary>
		/// <param name="revolutionSurface">The revolutionSurface</param>
		public struct getRevolutionSurfaceDefinitionReturn
		{
			public System.UInt32 generatricCurve;
			public Geom.Point3 axisOrigin;
			public Geom.Point3 axisDirection;
			public System.Double startAngle;
			public System.Double endAngle;
		}

		public static getRevolutionSurfaceDefinitionReturn GetRevolutionSurfaceDefinition(Core.Ident revolutionSurface) {
			var ret = ImportSDK_getRevolutionSurfaceDefinition(revolutionSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getRevolutionSurfaceDefinitionReturn retStruct = new getRevolutionSurfaceDefinitionReturn();
			retStruct.generatricCurve = (Core.Ident)ret.generatricCurve;
			retStruct.axisOrigin = ConvertValue(ret.axisOrigin);
			Geom_Point3_free(ref ret.axisOrigin);
			retStruct.axisDirection = ConvertValue(ret.axisDirection);
			Geom_Point3_free(ref ret.axisDirection);
			retStruct.startAngle = (Core.Double)ret.startAngle;
			retStruct.endAngle = (Core.Double)ret.endAngle;
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getRuledSurfaceDefinitionReturn_c
		{
			internal System.UInt32 firstCurve;
			internal System.UInt32 secondCurve;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getRuledSurfaceDefinitionReturn_c ImportSDK_getRuledSurfaceDefinition(System.UInt32 ruledSurface);
		/// <summary>
		/// get all parameters contained in the ruledSurface
		/// </summary>
		/// <param name="ruledSurface">The ruledSurface</param>
		public struct getRuledSurfaceDefinitionReturn
		{
			public System.UInt32 firstCurve;
			public System.UInt32 secondCurve;
		}

		public static getRuledSurfaceDefinitionReturn GetRuledSurfaceDefinition(Core.Ident ruledSurface) {
			var ret = ImportSDK_getRuledSurfaceDefinition(ruledSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getRuledSurfaceDefinitionReturn retStruct = new getRuledSurfaceDefinitionReturn();
			retStruct.firstCurve = (Core.Ident)ret.firstCurve;
			retStruct.secondCurve = (Core.Ident)ret.secondCurve;
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getSegmentCurveDefinitionReturn_c
		{
			internal Geom.Point3_c startPoint;
			internal Geom.Point3_c endPoint;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getSegmentCurveDefinitionReturn_c ImportSDK_getSegmentCurveDefinition(System.UInt32 segmentCurve);
		/// <summary>
		/// get all parameters contained in the segmentCurve
		/// </summary>
		/// <param name="segmentCurve">The segmentCurve</param>
		public struct getSegmentCurveDefinitionReturn
		{
			public Geom.Point3 startPoint;
			public Geom.Point3 endPoint;
		}

		public static getSegmentCurveDefinitionReturn GetSegmentCurveDefinition(Core.Ident segmentCurve) {
			var ret = ImportSDK_getSegmentCurveDefinition(segmentCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getSegmentCurveDefinitionReturn retStruct = new getSegmentCurveDefinitionReturn();
			retStruct.startPoint = ConvertValue(ret.startPoint);
			Geom_Point3_free(ref ret.startPoint);
			retStruct.endPoint = ConvertValue(ret.endPoint);
			Geom_Point3_free(ref ret.endPoint);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getSphereSurfaceDefinitionReturn_c
		{
			internal System.Double radius;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getSphereSurfaceDefinitionReturn_c ImportSDK_getSphereSurfaceDefinition(System.UInt32 sphereSurface);
		/// <summary>
		/// get all parameters contained in the sphereSurface
		/// </summary>
		/// <param name="sphereSurface">The sphereSurface</param>
		public struct getSphereSurfaceDefinitionReturn
		{
			public System.Double radius;
			public Geom.Matrix4 matrix;
		}

		public static getSphereSurfaceDefinitionReturn GetSphereSurfaceDefinition(Core.Ident sphereSurface) {
			var ret = ImportSDK_getSphereSurfaceDefinition(sphereSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getSphereSurfaceDefinitionReturn retStruct = new getSphereSurfaceDefinitionReturn();
			retStruct.radius = (Core.Double)ret.radius;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.Bounds2D_c ImportSDK_getSurfaceLimits(System.UInt32 surface);
		/// <summary>
		/// get the parametric space limits of a surface
		/// </summary>
		/// <param name="surface">The surface</param>
		public static CAD.Bounds2D GetSurfaceLimits(Core.Ident surface) {
			var ret = ImportSDK_getSurfaceLimits(surface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_Bounds2D_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getSurfacicCurveDefinitionReturn_c
		{
			internal System.UInt32 surface;
			internal System.UInt32 curve2D;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getSurfacicCurveDefinitionReturn_c ImportSDK_getSurfacicCurveDefinition(System.UInt32 surfacicCurve);
		/// <summary>
		/// get all parameters contained in the surfacicCurve
		/// </summary>
		/// <param name="surfacicCurve">The surfacicCurve</param>
		public struct getSurfacicCurveDefinitionReturn
		{
			public System.UInt32 surface;
			public System.UInt32 curve2D;
		}

		public static getSurfacicCurveDefinitionReturn GetSurfacicCurveDefinition(Core.Ident surfacicCurve) {
			var ret = ImportSDK_getSurfacicCurveDefinition(surfacicCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getSurfacicCurveDefinitionReturn retStruct = new getSurfacicCurveDefinitionReturn();
			retStruct.surface = (Core.Ident)ret.surface;
			retStruct.curve2D = (Core.Ident)ret.curve2D;
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getTabulatedCylinderSurfaceDefinitionReturn_c
		{
			internal System.UInt32 directrixCurve;
			internal Geom.Point3_c generatrixLine;
			internal CAD.Bounds1D_c range;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getTabulatedCylinderSurfaceDefinitionReturn_c ImportSDK_getTabulatedCylinderSurfaceDefinition(System.UInt32 tabulatedCylinderSurface);
		/// <summary>
		/// get all parameters contained in the TabulatedCylinderSurface
		/// </summary>
		/// <param name="tabulatedCylinderSurface">The tabulatedCylinderSurface</param>
		public struct getTabulatedCylinderSurfaceDefinitionReturn
		{
			public System.UInt32 directrixCurve;
			public Geom.Point3 generatrixLine;
			public CAD.Bounds1D range;
		}

		public static getTabulatedCylinderSurfaceDefinitionReturn GetTabulatedCylinderSurfaceDefinition(Core.Ident tabulatedCylinderSurface) {
			var ret = ImportSDK_getTabulatedCylinderSurfaceDefinition(tabulatedCylinderSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getTabulatedCylinderSurfaceDefinitionReturn retStruct = new getTabulatedCylinderSurfaceDefinitionReturn();
			retStruct.directrixCurve = (Core.Ident)ret.directrixCurve;
			retStruct.generatrixLine = ConvertValue(ret.generatrixLine);
			Geom_Point3_free(ref ret.generatrixLine);
			retStruct.range = ConvertValue(ret.range);
			CAD_Bounds1D_free(ref ret.range);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getTorusSurfaceDefinitionReturn_c
		{
			internal System.Double majorRadius;
			internal System.Double minorRadius;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getTorusSurfaceDefinitionReturn_c ImportSDK_getTorusSurfaceDefinition(System.UInt32 torusSurface);
		/// <summary>
		/// get all parameters contained in the torusSurface
		/// </summary>
		/// <param name="torusSurface">The torusSurface</param>
		public struct getTorusSurfaceDefinitionReturn
		{
			public System.Double majorRadius;
			public System.Double minorRadius;
			public Geom.Matrix4 matrix;
		}

		public static getTorusSurfaceDefinitionReturn GetTorusSurfaceDefinition(Core.Ident torusSurface) {
			var ret = ImportSDK_getTorusSurfaceDefinition(torusSurface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getTorusSurfaceDefinitionReturn retStruct = new getTorusSurfaceDefinitionReturn();
			retStruct.majorRadius = (Core.Double)ret.majorRadius;
			retStruct.minorRadius = (Core.Double)ret.minorRadius;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getTransformedCurveDefinitionReturn_c
		{
			internal System.UInt32 curve;
			internal Geom.Matrix4_c matrix;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getTransformedCurveDefinitionReturn_c ImportSDK_getTransformedCurveDefinition(System.UInt32 transformedCurve);
		/// <summary>
		/// get all parameters contained in the transformedCurve
		/// </summary>
		/// <param name="transformedCurve">The transformedCurve</param>
		public struct getTransformedCurveDefinitionReturn
		{
			public System.UInt32 curve;
			public Geom.Matrix4 matrix;
		}

		public static getTransformedCurveDefinitionReturn GetTransformedCurveDefinition(Core.Ident transformedCurve) {
			var ret = ImportSDK_getTransformedCurveDefinition(transformedCurve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getTransformedCurveDefinitionReturn retStruct = new getTransformedCurveDefinitionReturn();
			retStruct.curve = (Core.Ident)ret.curve;
			retStruct.matrix = ConvertValue(ret.matrix);
			Geom_Matrix4_free(ref ret.matrix);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Point3_c ImportSDK_getVertexPosition(System.UInt32 vertex);
		/// <summary>
		/// get the position of the vertex
		/// </summary>
		/// <param name="vertex">The vertex</param>
		public static Geom.Point3 GetVertexPosition(Core.Ident vertex) {
			var ret = ImportSDK_getVertexPosition(vertex);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Point3_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_invertCurve(System.UInt32 curve, System.Double precision);
		/// <summary>
		/// Invert a curve parametricaly
		/// </summary>
		/// <param name="curve">The curve to invert</param>
		/// <param name="precision">The precision used to invert the curve</param>
		public static Core.Ident InvertCurve(Core.Ident curve, Core.Double precision) {
			var ret = ImportSDK_invertCurve(curve, precision);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_isCurveClosed(System.UInt32 curve);
		/// <summary>
		/// if the curve is closed, return true, return false otherwise
		/// </summary>
		/// <param name="curve">The curve</param>
		public static Core.Bool IsCurveClosed(Core.Ident curve) {
			var ret = ImportSDK_isCurveClosed(curve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct isCurvePeriodicReturn_c
		{
			internal Int32 periodic;
			internal System.Double period;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern isCurvePeriodicReturn_c ImportSDK_isCurvePeriodic(System.UInt32 curve);
		/// <summary>
		/// if the curve is periodic return true, return false otherwise
		/// </summary>
		/// <param name="curve">The curve</param>
		public struct isCurvePeriodicReturn
		{
			public System.Boolean periodic;
			public System.Double period;
		}

		public static isCurvePeriodicReturn IsCurvePeriodic(Core.Ident curve) {
			var ret = ImportSDK_isCurvePeriodic(curve);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			isCurvePeriodicReturn retStruct = new isCurvePeriodicReturn();
			retStruct.periodic = ConvertValue(ret.periodic);
			retStruct.period = (Core.Double)ret.period;
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct isSurfaceClosedReturn_c
		{
			internal Int32 closedU;
			internal Int32 closedV;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern isSurfaceClosedReturn_c ImportSDK_isSurfaceClosed(System.UInt32 surface);
		/// <summary>
		/// return if the surface is closed on U or on V
		/// </summary>
		/// <param name="surface">The surface</param>
		public struct isSurfaceClosedReturn
		{
			public System.Boolean closedU;
			public System.Boolean closedV;
		}

		public static isSurfaceClosedReturn IsSurfaceClosed(Core.Ident surface) {
			var ret = ImportSDK_isSurfaceClosed(surface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			isSurfaceClosedReturn retStruct = new isSurfaceClosedReturn();
			retStruct.closedU = ConvertValue(ret.closedU);
			retStruct.closedV = ConvertValue(ret.closedV);
			return retStruct;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct isSurfacePeriodicReturn_c
		{
			internal Int32 periodicU;
			internal Int32 periodicV;
			internal System.Double periodU;
			internal System.Double periodV;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern isSurfacePeriodicReturn_c ImportSDK_isSurfacePeriodic(System.UInt32 surface);
		/// <summary>
		/// return if the surface is periodic on U or on V
		/// </summary>
		/// <param name="surface">The surface</param>
		public struct isSurfacePeriodicReturn
		{
			public System.Boolean periodicU;
			public System.Boolean periodicV;
			public System.Double periodU;
			public System.Double periodV;
		}

		public static isSurfacePeriodicReturn IsSurfacePeriodic(Core.Ident surface) {
			var ret = ImportSDK_isSurfacePeriodic(surface);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			isSurfacePeriodicReturn retStruct = new isSurfacePeriodicReturn();
			retStruct.periodicU = ConvertValue(ret.periodicU);
			retStruct.periodicV = ConvertValue(ret.periodicV);
			retStruct.periodU = (Core.Double)ret.periodU;
			retStruct.periodV = (Core.Double)ret.periodV;
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.Double ImportSDK_projectOnCurve(System.UInt32 curve, Geom.Point3_c point, System.Double precision);
		/// <summary>
		/// project a point to a curve
		/// </summary>
		/// <param name="curve">The curve</param>
		/// <param name="point">The point to project</param>
		/// <param name="precision">Projection precision</param>
		public static Core.Double ProjectOnCurve(Core.Ident curve, Geom.Point3 point, Core.Double precision) {
			var point_c = ConvertValue(point);
			var ret = ImportSDK_projectOnCurve(curve, point_c, precision);
			Geom_Point3_free(ref point_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Double)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Point2_c ImportSDK_projectOnSurface(System.UInt32 surface, Geom.Point3_c point, System.Double precision);
		/// <summary>
		/// project a point to a surface
		/// </summary>
		/// <param name="surface">The surface</param>
		/// <param name="point">The point to project</param>
		/// <param name="precision">Projection precision</param>
		public static Geom.Point2 ProjectOnSurface(Core.Ident surface, Geom.Point3 point, Core.Double precision) {
			var point_c = ConvertValue(point);
			var ret = ImportSDK_projectOnSurface(surface, point_c, precision);
			Geom_Point3_free(ref point_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Point2_free(ref ret);
			return convRet;
		}

		#endregion

		#region Core

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_SetPixyzMainThread();
		/// <summary>
		/// Use this function if you need to call functions (import, decimate, repair, etc.) on a different thread than the one that initialized pixyz
		/// </summary>
		public static void SetPixyzMainThread() {
			ImportSDK_SetPixyzMainThread();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_initialize(string productKey, string validationKey, string license, string subProcessName);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="productKey"></param>
		/// <param name="validationKey"></param>
		/// <param name="license">License file as string</param>
		/// <param name="subProcessName">Name of subProcess</param>
		public static void Initialize(Core.String productKey, Core.String validationKey, Core.String license, Core.String subProcessName) {
			ImportSDK_initialize(productKey, validationKey, license, subProcessName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_configureInterfaceLogger(Int32 enableFunction, Int32 enableParameters, Int32 enableExecutionTime);
		/// <summary>
		/// Set new configuration for the Interface Logger
		/// </summary>
		/// <param name="enableFunction">If true, the called function names will be print</param>
		/// <param name="enableParameters">If true, the called function parameters will be print (only if enableFunction=true too)</param>
		/// <param name="enableExecutionTime">If true, the called functions execution times will be print</param>
		public static void ConfigureInterfaceLogger(Core.Bool enableFunction, Core.Bool enableParameters, Core.Bool enableExecutionTime) {
			ImportSDK_configureInterfaceLogger(enableFunction ? 1 : 0, enableParameters ? 1 : 0, enableExecutionTime ? 1 : 0);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_getEntityTypeString(System.UInt32 entity);
		/// <summary>
		/// returns the type name of the entity
		/// </summary>
		/// <param name="entity">The wanted entity</param>
		public static Core.String GetEntityTypeString(Core.Ident entity) {
			var ret = ImportSDK_getEntityTypeString(entity);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_getInstallationDirectory();
		/// <summary>
		/// get the Pixyz installation directory
		/// </summary>
		public static Core.String GetInstallationDirectory() {
			var ret = ImportSDK_getInstallationDirectory();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_loadNoScript(string fileName);
		/// <summary>
		/// Load no script
		/// </summary>
		/// <param name="fileName">Path to load the file</param>
		public static void LoadNoScript(Core.String fileName) {
			ImportSDK_loadNoScript(fileName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_pushAnalytic(string name, string data);
		/// <summary>
		/// push custom analytic event (Only for authorized products)
		/// </summary>
		/// <param name="name">Analytic event name</param>
		/// <param name="data">Analytic event data</param>
		public static void PushAnalytic(Core.String name, Core.String data) {
			ImportSDK_pushAnalytic(name, data);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region IO

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IO.FormatList_c ImportSDK_getExportFormats();
		/// <summary>
		/// Give all the format name and their extensions that can be exported in Pixyz
		/// </summary>
		public static IO.FormatList GetExportFormats() {
			var ret = ImportSDK_getExportFormats();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			IO_FormatList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IO.FormatList_c ImportSDK_getImportFormats();
		/// <summary>
		/// Give all the format name and their extensions that can be imported in Pixyz
		/// </summary>
		public static IO.FormatList GetImportFormats() {
			var ret = ImportSDK_getImportFormats();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			IO_FormatList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_importScene(string fileName, System.UInt32 root);
		/// <summary>
		/// Import a file
		/// </summary>
		/// <param name="fileName">Path of the file to import</param>
		/// <param name="root">Identifier of the destination occurrence</param>
		public static Core.Ident ImportScene(Core.String fileName, Core.Ident root) {
			var ret = ImportSDK_importScene(fileName, root);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_importSceneNoScript(string fileName, System.UInt32 root);
		/// <summary>
		/// Import a file
		/// </summary>
		/// <param name="fileName">Path of the file to import</param>
		/// <param name="root">Identifier of the destination occurrence</param>
		public static Core.Ident ImportSceneNoScript(Core.String fileName, Core.Ident root) {
			var ret = ImportSDK_importSceneNoScript(fileName, root);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_load(string fileName);
		/// <summary>
		/// Load no script
		/// </summary>
		/// <param name="fileName">Path to load the file</param>
		public static void Load(Core.String fileName) {
			ImportSDK_load(fileName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_resetSession();
		/// <summary>
		/// Clear all the current session (all unsaved work will be lost)
		/// </summary>
		public static void ResetSession() {
			ImportSDK_resetSession();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_save(string fileName);
		/// <summary>
		/// Save no script
		/// </summary>
		/// <param name="fileName">Path to save the file</param>
		public static void Save(Core.String fileName) {
			ImportSDK_save(fileName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region Licenses

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_addWantedToken(string tokenName);
		/// <summary>
		/// Add a license token to the list of wanted optional tokens
		/// </summary>
		/// <param name="tokenName">Wanted token</param>
		public static void AddWantedToken(Core.String tokenName) {
			ImportSDK_addWantedToken(tokenName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_checkLicense();
		/// <summary>
		/// check the current license
		/// </summary>
		public static Core.Bool CheckLicense() {
			var ret = ImportSDK_checkLicense();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_configureLicenseServer(string address, System.UInt16 port, Int32 flexLM);
		/// <summary>
		/// Configure the license server to use to get floating licenses
		/// </summary>
		/// <param name="address">Server address</param>
		/// <param name="port">Server port</param>
		/// <param name="flexLM">Enable FlexLM license server</param>
		public static void ConfigureLicenseServer(Core.String address, Core.UShort port, Core.Bool flexLM) {
			ImportSDK_configureLicenseServer(address, port, flexLM ? 1 : 0);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_generateActivationCode(string filePath);
		/// <summary>
		/// Create an activation code to generate an offline license
		/// </summary>
		/// <param name="filePath">Path to write the activation code</param>
		public static void GenerateActivationCode(Core.String filePath) {
			ImportSDK_generateActivationCode(filePath);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_generateDeactivationCode(string filePath);
		/// <summary>
		/// Create an deactivation code to release the license from this machine
		/// </summary>
		/// <param name="filePath">Path to write the deactivation code</param>
		public static void GenerateDeactivationCode(Core.String filePath) {
			ImportSDK_generateDeactivationCode(filePath);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.LicenseInfos_c ImportSDK_getCurrentLicenseInfos();
		/// <summary>
		/// get informations on current installed license
		/// </summary>
		public static Core.LicenseInfos GetCurrentLicenseInfos() {
			var ret = ImportSDK_getCurrentLicenseInfos();
			System.String err = ConvertValue(ImportSDK_getLastError());
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

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getLicenseServerReturn_c ImportSDK_getLicenseServer();
		/// <summary>
		/// Get current license server
		/// </summary>
		public struct getLicenseServerReturn
		{
			public System.String serverHost;
			public System.UInt16 serverPort;
			public System.Boolean useFlexLM;
		}

		public static getLicenseServerReturn GetLicenseServer() {
			var ret = ImportSDK_getLicenseServer();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getLicenseServerReturn retStruct = new getLicenseServerReturn();
			retStruct.serverHost = ConvertValue(ret.serverHost);
			retStruct.serverPort = (Core.UShort)ret.serverPort;
			retStruct.useFlexLM = ConvertValue(ret.useFlexLM);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.StringList_c ImportSDK_getTokens(Int32 onlyMandatory);
		/// <summary>
		/// Get the list of license tokens for this product
		/// </summary>
		/// <param name="onlyMandatory">If True, optional tokens will not be returned</param>
		public static Core.StringList GetTokens(Core.Bool onlyMandatory) {
			var ret = ImportSDK_getTokens(onlyMandatory ? 1 : 0);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_StringList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_installLicense(string licensePath);
		/// <summary>
		/// install a new license
		/// </summary>
		/// <param name="licensePath">Path of the license file</param>
		public static void InstallLicense(Core.String licensePath) {
			ImportSDK_installLicense(licensePath);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_isFloatingLicense();
		/// <summary>
		/// Tells if license is floating
		/// </summary>
		public static Core.Bool IsFloatingLicense() {
			var ret = ImportSDK_isFloatingLicense();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_isTokenValid(string tokenName);
		/// <summary>
		/// Returns True if a token is owned by the product
		/// </summary>
		/// <param name="tokenName">Token name</param>
		public static Core.Bool IsTokenValid(Core.String tokenName) {
			var ret = ImportSDK_isTokenValid(tokenName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_releaseToken(string tokenName);
		/// <summary>
		/// Release an optional license token
		/// </summary>
		/// <param name="tokenName">Token name</param>
		public static void ReleaseToken(Core.String tokenName) {
			ImportSDK_releaseToken(tokenName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_releaseWebLicense(string login, string password, System.UInt32 id);
		/// <summary>
		/// release License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		/// <param name="id">WEB license id</param>
		public static void ReleaseWebLicense(Core.String login, Core.String password, Core.Ident id) {
			ImportSDK_releaseWebLicense(login, password, id);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_removeWantedToken(string tokenName);
		/// <summary>
		/// remove a license token from the list of wanted optional tokens
		/// </summary>
		/// <param name="tokenName">Unwanted token</param>
		public static void RemoveWantedToken(Core.String tokenName) {
			ImportSDK_removeWantedToken(tokenName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_requestWebLicense(string login, string password, System.UInt32 id);
		/// <summary>
		/// request License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		/// <param name="id">WEB license id</param>
		public static void RequestWebLicense(Core.String login, Core.String password, Core.Ident id) {
			ImportSDK_requestWebLicense(login, password, id);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.WebLicenseInfoList_c ImportSDK_retrieveAvailableLicenses(string login, string password);
		/// <summary>
		/// Retrieves License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		public static Core.WebLicenseInfoList RetrieveAvailableLicenses(Core.String login, Core.String password) {
			var ret = ImportSDK_retrieveAvailableLicenses(login, password);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_WebLicenseInfoList_free(ref ret);
			return convRet;
		}

		#endregion

		#region Product structure access

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.AABB_c ImportSDK_getAABB(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Returns the axis aligned bounding box of a list of scene paths
		/// </summary>
		/// <param name="occurrences">List of occurrences to retrieve the AABB</param>
		public static Geom.AABB GetAABB(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			var ret = ImportSDK_getAABB(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_AABB_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.PackedTree_c ImportSDK_getCompleteTree(System.UInt32 root, Int32 visibilityMode);
		/// <summary>
		/// Returns a packed version of the whole scene tree
		/// </summary>
		/// <param name="root">Specify the root of the returned scene</param>
		/// <param name="visibilityMode">The visibility mode</param>
		public static Scene.PackedTree GetCompleteTree(Core.Ident root, Scene.VisibilityMode visibilityMode) {
			var ret = ImportSDK_getCompleteTree(root, (int)visibilityMode);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_PackedTree_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.ComponentList_c ImportSDK_getComponentByOccurrence(Scene.OccurrenceList_c occurrences, Int32 componentType, Int32 followPrototypes);
		/// <summary>
		/// Returns one component of the specified type by occurrence if it exists
		/// </summary>
		/// <param name="occurrences">The occurrences list</param>
		/// <param name="componentType">Type of the component</param>
		/// <param name="followPrototypes">If true and if the component is not set on the occurrence, try to find it on its prototyping chain</param>
		public static Scene.ComponentList GetComponentByOccurrence(Scene.OccurrenceList occurrences, Scene.ComponentType componentType, Core.Bool followPrototypes) {
			var occurrences_c = ConvertValue(occurrences);
			var ret = ImportSDK_getComponentByOccurrence(occurrences_c, (int)componentType, followPrototypes ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_ComponentList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Matrix4_c ImportSDK_getGlobalMatrix(System.UInt32 occurrence);
		/// <summary>
		/// Returns the global matrix on an occurrence
		/// </summary>
		/// <param name="occurrence">Occurrence to get the global matrix</param>
		public static Geom.Matrix4 GetGlobalMatrix(Core.Ident occurrence) {
			var ret = ImportSDK_getGlobalMatrix(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Matrix4_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Matrix4_c ImportSDK_getLocalMatrix(System.UInt32 occurrence);
		/// <summary>
		/// Returns the local matrix on an occurrence
		/// </summary>
		/// <param name="occurrence">Node to get the local matrix</param>
		public static Geom.Matrix4 GetLocalMatrix(Core.Ident occurrence) {
			var ret = ImportSDK_getLocalMatrix(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Matrix4_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Polygonal.MeshDefinitionList_c ImportSDK_getMeshDefinitions(Polygonal.MeshList_c meshes);
		/// <summary>
		/// Returns the definition
		/// </summary>
		/// <param name="meshes">The meshes to get definitions</param>
		public static Polygonal.MeshDefinitionList GetMeshDefinitions(Polygonal.MeshList meshes) {
			var meshes_c = ConvertValue(meshes);
			var ret = ImportSDK_getMeshDefinitions(meshes_c);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_MeshDefinitionList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.MetadataDefinitionList_c ImportSDK_getMetadatasDefinitions(Scene.MetadataList_c metadatas);
		/// <summary>
		/// Returns definition of Metadata components
		/// </summary>
		/// <param name="metadatas">List of metadata component to retrieve definition</param>
		public static Scene.MetadataDefinitionList GetMetadatasDefinitions(Scene.MetadataList metadatas) {
			var metadatas_c = ConvertValue(metadatas);
			var ret = ImportSDK_getMetadatasDefinitions(metadatas_c);
			Scene_MetadataList_free(ref metadatas_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_MetadataDefinitionList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_getNodeName(System.UInt32 occurrence);
		/// <summary>
		/// Returns the name of an occurrence
		/// </summary>
		/// <param name="occurrence">The occurrence to get the name</param>
		public static Core.String GetNodeName(Core.Ident occurrence) {
			var ret = ImportSDK_getNodeName(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_getOccurrenceActiveMaterial(System.UInt32 occurrence);
		/// <summary>
		/// Returns the active material on a given occurrence
		/// </summary>
		/// <param name="occurrence">Occurrence to get the active material</param>
		public static Core.Ident GetOccurrenceActiveMaterial(Core.Ident occurrence) {
			var ret = ImportSDK_getOccurrenceActiveMaterial(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_getOccurrenceChildren(System.UInt32 occurrence);
		/// <summary>
		/// Get the children of an occurrence
		/// </summary>
		/// <param name="occurrence">The occurrence</param>
		public static Scene.OccurrenceList GetOccurrenceChildren(Core.Ident occurrence) {
			var ret = ImportSDK_getOccurrenceChildren(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_getParent(System.UInt32 occurrence);
		/// <summary>
		/// Get the parent of an occurrence
		/// </summary>
		/// <param name="occurrence">The occurrence</param>
		public static Core.Ident GetParent(Core.Ident occurrence) {
			var ret = ImportSDK_getParent(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_getPartActiveShape(System.UInt32 part);
		/// <summary>
		/// Returns the active shape of a part
		/// </summary>
		/// <param name="part">The part</param>
		public static Core.Ident GetPartActiveShape(Core.Ident part) {
			var ret = ImportSDK_getPartActiveShape(part);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Polygonal.MeshList_c ImportSDK_getPartsMeshes(Scene.PartList_c parts);
		/// <summary>
		/// Return the meshes of the TesselatedShape for each given parts if any
		/// </summary>
		/// <param name="parts">The list of part component</param>
		public static Polygonal.MeshList GetPartsMeshes(Scene.PartList parts) {
			var parts_c = ConvertValue(parts);
			var ret = ImportSDK_getPartsMeshes(parts_c);
			Scene_PartList_free(ref parts_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_MeshList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern CAD.ModelList_c ImportSDK_getPartsModels(Scene.PartList_c parts);
		/// <summary>
		/// Return the models of the BRepShape for each given parts if any
		/// </summary>
		/// <param name="parts">The list of part component</param>
		public static CAD.ModelList GetPartsModels(Scene.PartList parts) {
			var parts_c = ConvertValue(parts);
			var ret = ImportSDK_getPartsModels(parts_c);
			Scene_PartList_free(ref parts_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			CAD_ModelList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Geom.Matrix4List_c ImportSDK_getPartsTransforms(Scene.PartList_c parts);
		/// <summary>
		/// Returns the transform matrix of each given parts
		/// </summary>
		/// <param name="parts">The parts to retrieve transform</param>
		public static Geom.Matrix4List GetPartsTransforms(Scene.PartList parts) {
			var parts_c = ConvertValue(parts);
			var ret = ImportSDK_getPartsTransforms(parts_c);
			Scene_PartList_free(ref parts_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_Matrix4List_free(ref ret);
			return convRet;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct getPartsTransformsIndexedReturn_c
		{
			internal Core.IntList_c indices;
			internal Geom.Matrix4List_c transforms;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern getPartsTransformsIndexedReturn_c ImportSDK_getPartsTransformsIndexed(Scene.PartList_c parts);
		/// <summary>
		/// Returns the transform matrix of each given parts (indexed mode)
		/// </summary>
		/// <param name="parts">The parts to retrieve transform</param>
		public struct getPartsTransformsIndexedReturn
		{
			public Core.IntList indices;
			public Geom.Matrix4List transforms;
		}

		public static getPartsTransformsIndexedReturn GetPartsTransformsIndexed(Scene.PartList parts) {
			var parts_c = ConvertValue(parts);
			var ret = ImportSDK_getPartsTransformsIndexed(parts_c);
			Scene_PartList_free(ref parts_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getPartsTransformsIndexedReturn retStruct = new getPartsTransformsIndexedReturn();
			retStruct.indices = ConvertValue(ret.indices);
			Core_IntList_free(ref ret.indices);
			retStruct.transforms = ConvertValue(ret.transforms);
			Geom_Matrix4List_free(ref ret.transforms);
			return retStruct;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_getRootOccurrence();
		/// <summary>
		/// Get the root occurrence of the product structure
		/// </summary>
		public static Core.Ident GetRootOccurrence() {
			var ret = ImportSDK_getRootOccurrence();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		#endregion

		#region Properties

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_addCustomProperty(System.UInt32 entity, string name, string value);
		/// <summary>
		/// Add a custom property to an entity that support custom properties
		/// </summary>
		/// <param name="entity">An entity that support custom properties</param>
		/// <param name="name">Name of the custom property</param>
		/// <param name="value">Value of the custom property</param>
		public static void AddCustomProperty(Core.Ident entity, Core.String name, Core.String value) {
			ImportSDK_addCustomProperty(entity, name, value);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_getModuleProperty(string module, string propertyName);
		/// <summary>
		/// Returns the value of a module property
		/// </summary>
		/// <param name="module">Name of the module</param>
		/// <param name="propertyName">The property name</param>
		public static Core.String GetModuleProperty(Core.String module, Core.String propertyName) {
			var ret = ImportSDK_getModuleProperty(module, propertyName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_getProperty(System.UInt32 entity, string propertyName);
		/// <summary>
		/// Get a property value as String on an entity (error if the property does not exist on the entity)
		/// </summary>
		/// <param name="entity">The entity</param>
		/// <param name="propertyName">The property name</param>
		public static Core.String GetProperty(Core.Ident entity, Core.String propertyName) {
			var ret = ImportSDK_getProperty(entity, propertyName);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.StringList_c ImportSDK_listModuleProperties(string module);
		/// <summary>
		/// Returns all the properties in the given module
		/// </summary>
		/// <param name="module">Name of the module</param>
		public static Core.StringList ListModuleProperties(Core.String module) {
			var ret = ImportSDK_listModuleProperties(module);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_StringList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Core.StringList_c ImportSDK_listProperties(System.UInt32 entity);
		/// <summary>
		/// Returns the name of the properties available on an entity
		/// </summary>
		/// <param name="entity">Entity to list</param>
		public static Core.StringList ListProperties(Core.Ident entity) {
			var ret = ImportSDK_listProperties(entity);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_StringList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_setModuleProperty(string module, string propertyName, string propertyValue);
		/// <summary>
		/// Set the value of a module property
		/// </summary>
		/// <param name="module">Name of the module</param>
		/// <param name="propertyName">The property name</param>
		/// <param name="propertyValue">The property value</param>
		public static Core.String SetModuleProperty(Core.String module, Core.String propertyName, Core.String propertyValue) {
			var ret = ImportSDK_setModuleProperty(module, propertyName, propertyValue);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern IntPtr ImportSDK_setProperty(System.UInt32 entity, string propertyName, string propertyValue);
		/// <summary>
		/// Set a property value on an entity
		/// </summary>
		/// <param name="entity">The entity</param>
		/// <param name="propertyName">The property name</param>
		/// <param name="propertyValue">The property value</param>
		public static Core.String SetProperty(Core.Ident entity, Core.String propertyName, Core.String propertyValue) {
			var ret = ImportSDK_setProperty(entity, propertyName, propertyValue);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		#endregion

		#region Scene optimization

		[DllImport(PiXYZImportSDK_dll)]
		private static extern System.UInt32 ImportSDK_compress(System.UInt32 occurrence);
		/// <summary>
		/// Compress a sub-tree by removing occurrence containing only one Child or empty, and by removing useless instances (see removeUselessInstances)
		/// </summary>
		/// <param name="occurrence">Root occurrence for the process</param>
		public static Core.Ident Compress(Core.Ident occurrence) {
			var ret = ImportSDK_compress(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_deleteEmptyOccurrences(System.UInt32 root);
		/// <summary>
		/// Delete all empty assemblies
		/// </summary>
		/// <param name="root">Root occurrence for the process</param>
		public static void DeleteEmptyOccurrences(Core.Ident root) {
			ImportSDK_deleteEmptyOccurrences(root);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_findByActiveMaterial(System.UInt32 material, Scene.OccurrenceList_c roots);
		/// <summary>
		/// Find all part occurrence with a given material as active material (i.e. as seen in the rendering)
		/// </summary>
		/// <param name="material">A material</param>
		/// <param name="roots">If specified, restrict the search from the given roots</param>
		public static Scene.OccurrenceList FindByActiveMaterial(Core.Ident material, Scene.OccurrenceList roots) {
			var roots_c = ConvertValue(roots);
			var ret = ImportSDK_findByActiveMaterial(material, roots_c);
			Scene_OccurrenceList_free(ref roots_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_findByProperty(string property, string regex, Scene.OccurrenceList_c roots);
		/// <summary>
		/// Returns all occurrences which a property value matches the given regular expression (ECMAScript)
		/// </summary>
		/// <param name="property">Property name</param>
		/// <param name="regex">Regular expression (ECMAScript)</param>
		/// <param name="roots">If specified, restrict the search from the given roots</param>
		public static Scene.OccurrenceList FindByProperty(Core.String property, Core.String regex, Scene.OccurrenceList roots) {
			var roots_c = ConvertValue(roots);
			var ret = ImportSDK_findByProperty(property, regex, roots_c);
			Scene_OccurrenceList_free(ref roots_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_getComponentType(System.UInt32 component);
		/// <summary>
		/// Get the type of a component
		/// </summary>
		/// <param name="component">The component</param>
		public static Scene.ComponentType GetComponentType(Core.Ident component) {
			var ret = ImportSDK_getComponentType(component);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Scene.ComponentType)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_getPartOccurrences(System.UInt32 from);
		/// <summary>
		/// Recursively get all the occurrences containing a part component
		/// </summary>
		/// <param name="from">Source occurrence of the recursion</param>
		public static Scene.OccurrenceList GetPartOccurrences(Core.Ident from) {
			var ret = ImportSDK_getPartOccurrences(from);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.ComponentList_c ImportSDK_listComponents(System.UInt32 occurrence, Int32 followPrototypes);
		/// <summary>
		/// List all components on an occurrence
		/// </summary>
		/// <param name="occurrence">The occurrence to list the components</param>
		/// <param name="followPrototypes">If true list also components owned by the prototype</param>
		public static Scene.ComponentList ListComponents(Core.Ident occurrence, Core.Bool followPrototypes) {
			var ret = ImportSDK_listComponents(occurrence, followPrototypes ? 1 : 0);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_ComponentList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_makeInstanceUnique(Scene.OccurrenceList_c occurrences);
		/// <summary>
		/// Singularize all instances on the sub-tree of an occurrence
		/// </summary>
		/// <param name="occurrences">Root occurrence for the process</param>
		public static void MakeInstanceUnique(Scene.OccurrenceList occurrences) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_makeInstanceUnique(occurrences_c);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_mergeByTreeLevel(Scene.OccurrenceList_c partOccurrences, Int32 maxLevel, Int32 mergeHiddenPartsMode);
		/// <summary>
		/// Merge all parts over maxLevel level
		/// </summary>
		/// <param name="partOccurrences">Occurrence of the parts to merge</param>
		/// <param name="maxLevel">Maximum tree level</param>
		/// <param name="mergeHiddenPartsMode">Hidden parts handling mode, Destroy them, make visible or merge separately</param>
		public static void MergeByTreeLevel(Scene.OccurrenceList partOccurrences, Core.Int maxLevel, Scene.MergeHiddenPartsMode mergeHiddenPartsMode) {
			var partOccurrences_c = ConvertValue(partOccurrences);
			ImportSDK_mergeByTreeLevel(partOccurrences_c, maxLevel, (int)mergeHiddenPartsMode);
			Scene_OccurrenceList_free(ref partOccurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_mergeFinalLevel(Scene.OccurrenceList_c roots, Int32 mergeHiddenPartsMode, Int32 CollapseToParent);
		/// <summary>
		/// Merge final level (occurrences with only occurrence with part component as children)
		/// </summary>
		/// <param name="roots">Roots occurrences for the process (will not be removed)</param>
		/// <param name="mergeHiddenPartsMode">Hidden parts handling mode, Destroy them, make visible or merge separately</param>
		/// <param name="CollapseToParent">If true, final level unique merged part will replace it's parent</param>
		public static void MergeFinalLevel(Scene.OccurrenceList roots, Scene.MergeHiddenPartsMode mergeHiddenPartsMode, Core.Bool CollapseToParent) {
			var roots_c = ConvertValue(roots);
			ImportSDK_mergeFinalLevel(roots_c, (int)mergeHiddenPartsMode, CollapseToParent ? 1 : 0);
			Scene_OccurrenceList_free(ref roots_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_mergeMaterials();
		/// <summary>
		/// Merge all equivalent materials (i.e. with same appearance)
		/// </summary>
		public static Core.Int MergeMaterials() {
			var ret = ImportSDK_mergeMaterials();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Int)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_mergeParts(Scene.OccurrenceList_c partOccurrences, Int32 mergeHiddenPartsMode);
		/// <summary>
		/// Merge a set of parts
		/// </summary>
		/// <param name="partOccurrences">Occurrence of the parts to merge</param>
		/// <param name="mergeHiddenPartsMode">Hidden parts handling mode, Destroy them, make visible or merge separately</param>
		public static Scene.OccurrenceList MergeParts(Scene.OccurrenceList partOccurrences, Scene.MergeHiddenPartsMode mergeHiddenPartsMode) {
			var partOccurrences_c = ConvertValue(partOccurrences);
			var ret = ImportSDK_mergeParts(partOccurrences_c, (int)mergeHiddenPartsMode);
			Scene_OccurrenceList_free(ref partOccurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_mergePartsByMaterials(Scene.OccurrenceList_c partOccurrences, Int32 mergeNoMaterials, Int32 mergeHiddenPartsMode);
		/// <summary>
		/// Merge a set of parts by materials
		/// </summary>
		/// <param name="partOccurrences">Occurrence of the parts to merge</param>
		/// <param name="mergeNoMaterials">If true, merge all parts with no active material together, else do not merge them</param>
		/// <param name="mergeHiddenPartsMode">Hidden parts handling mode, Destroy them, make visible or merge separately</param>
		public static Scene.OccurrenceList MergePartsByMaterials(Scene.OccurrenceList partOccurrences, Core.Bool mergeNoMaterials, Scene.MergeHiddenPartsMode mergeHiddenPartsMode) {
			var partOccurrences_c = ConvertValue(partOccurrences);
			var ret = ImportSDK_mergePartsByMaterials(partOccurrences_c, mergeNoMaterials ? 1 : 0, (int)mergeHiddenPartsMode);
			Scene_OccurrenceList_free(ref partOccurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_rake(System.UInt32 occurrence, Int32 keepInstances);
		/// <summary>
		/// Set the same parent to all descending parts (all parts will be singularized)
		/// </summary>
		/// <param name="occurrence">Root occurrence for the process</param>
		/// <param name="keepInstances">If false, the part will be singularized</param>
		public static void Rake(Core.Ident occurrence, Core.Bool keepInstances) {
			ImportSDK_rake(occurrence, keepInstances ? 1 : 0);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_removeSymmetryMatrices(System.UInt32 occurrence);
		/// <summary>
		/// Remove symmetry matrices (apply matrices on geometries on nodes under an occurrence with a symmetry matrix
		/// </summary>
		/// <param name="occurrence">Root occurrence for the process</param>
		public static void RemoveSymmetryMatrices(Core.Ident occurrence) {
			ImportSDK_removeSymmetryMatrices(occurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_resetPartTransform(System.UInt32 root);
		/// <summary>
		/// Set all part transformation matrices to identity in a sub-tree, transformation will be applied to the shapes
		/// </summary>
		/// <param name="root">Root occurrence for the process</param>
		public static void ResetPartTransform(Core.Ident root) {
			ImportSDK_resetPartTransform(root);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_resetTransform(System.UInt32 root, Int32 recursive, Int32 keepInstantiation, Int32 keepPartTransform);
		/// <summary>
		/// Set all transformation matrices to identity in a sub-tree.
		/// </summary>
		/// <param name="root">Root occurrence for the process</param>
		/// <param name="recursive">If False, transformation will be applied only on the root and its components</param>
		/// <param name="keepInstantiation">If False, all occurrences will be singularized</param>
		/// <param name="keepPartTransform">If False, transformation will be applied to the shapes (BRepShape points or TessellatedShape vertices)</param>
		public static void ResetTransform(Core.Ident root, Core.Bool recursive, Core.Bool keepInstantiation, Core.Bool keepPartTransform) {
			ImportSDK_resetTransform(root, recursive ? 1 : 0, keepInstantiation ? 1 : 0, keepPartTransform ? 1 : 0);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_transferCADMaterialsOnPartOccurrences(System.UInt32 rootOccurrence);
		/// <summary>
		/// Set all materials on part occurrences
		/// </summary>
		/// <param name="rootOccurrence">Root occurrence</param>
		public static void TransferCADMaterialsOnPartOccurrences(Core.Ident rootOccurrence) {
			ImportSDK_transferCADMaterialsOnPartOccurrences(rootOccurrence);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region UVs

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_automaticUVMapping(Scene.OccurrenceList_c occurrences, Int32 channel, System.Double maxAngleDistorsion, System.Double maxAreaDistorsion, Int32 sharpToSeam, Int32 forbidOverlapping);
		/// <summary>
		/// Generates the texture coordinates and automatically cut
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="channel">The UV channel which will contains the texture coordinates</param>
		/// <param name="maxAngleDistorsion">Maximum angle distorsion |2PI-SumVtxAng|/2PI</param>
		/// <param name="maxAreaDistorsion">Maximum area distorsion before scale to 1. |2DArea-3DArea|/3DArea </param>
		/// <param name="sharpToSeam">If enabled, sharp edges are automatically considered as UV seams</param>
		/// <param name="forbidOverlapping">If enabled, UV cannot overlap</param>
		public static void AutomaticUVMapping(Scene.OccurrenceList occurrences, Core.Int channel, Core.Double maxAngleDistorsion, Core.Double maxAreaDistorsion, Core.Bool sharpToSeam, Core.Bool forbidOverlapping) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_automaticUVMapping(occurrences_c, channel, maxAngleDistorsion, maxAreaDistorsion, sharpToSeam ? 1 : 0, forbidOverlapping ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_bakeUV(System.UInt32 source, System.UInt32 destination, Int32 sourceChannel, Int32 destinationChannel, System.Double tolerance);
		/// <summary>
		/// Bake UV from a mesh to another mesh
		/// </summary>
		/// <param name="source">Occurrence of the source mesh</param>
		/// <param name="destination">Occurrence of the destination mesh</param>
		/// <param name="sourceChannel">Source UV channel to bake</param>
		/// <param name="destinationChannel">Destination UV channel to bake to</param>
		/// <param name="tolerance">Tolerance when point is projected on seam (if the model come from a decimation it is recommanded to use the lineic tolerance here)</param>
		public static void BakeUV(Core.Ident source, Core.Ident destination, Core.Int sourceChannel, Core.Int destinationChannel, Core.Double tolerance) {
			ImportSDK_bakeUV(source, destination, sourceChannel, destinationChannel, tolerance);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_mapUvOnAABB(Scene.OccurrenceList_c occurrences, Int32 useLocalAABB, System.Double uv3dSize, Int32 channel, Int32 overrideExistingUVs);
		/// <summary>
		/// Generate texture coordinates using the projection on object Axis Aligned Bounding Box
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="useLocalAABB">If enabled, uses part own bounding box, else use global one</param>
		/// <param name="uv3dSize">3D size of the UV space [0-1]</param>
		/// <param name="channel">The UV channel which will contains the texture coordinates</param>
		/// <param name="overrideExistingUVs">If True, overide existing UVs on channel</param>
		public static void MapUvOnAABB(Scene.OccurrenceList occurrences, Core.Bool useLocalAABB, Core.Double uv3dSize, Core.Int channel, Core.Bool overrideExistingUVs) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_mapUvOnAABB(occurrences_c, useLocalAABB ? 1 : 0, uv3dSize, channel, overrideExistingUVs ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_mapUvOnCubicAABB(Scene.OccurrenceList_c occurrences, System.Double uv3dSize, Int32 channel, Int32 overrideExistingUVs);
		/// <summary>
		/// Generate texture coordinates using the projection on object AABB, with same scale on each axis
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="uv3dSize">3D size of the UV space [0-1]</param>
		/// <param name="channel">The UV channel which will contains the texture coordinates</param>
		/// <param name="overrideExistingUVs">If True, overide existing UVs on channel</param>
		public static void MapUvOnCubicAABB(Scene.OccurrenceList occurrences, Core.Double uv3dSize, Core.Int channel, Core.Bool overrideExistingUVs) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_mapUvOnCubicAABB(occurrences_c, uv3dSize, channel, overrideExistingUVs ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_mapUvOnCustomAABB(Scene.OccurrenceList_c occurrences, Geom.AABB_c aabb, System.Double uv3dSize, Int32 channel, Int32 overrideExistingUVs);
		/// <summary>
		/// Generate texture coordinates using the projection on custom AABB
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="aabb">Axis aligned bounding box to project on</param>
		/// <param name="uv3dSize">3D size of the UV space [0-1]</param>
		/// <param name="channel">The UV channel which will contains the texture coordinates</param>
		/// <param name="overrideExistingUVs">If True, overide existing UVs on channel</param>
		public static void MapUvOnCustomAABB(Scene.OccurrenceList occurrences, Geom.AABB aabb, Core.Double uv3dSize, Core.Int channel, Core.Bool overrideExistingUVs) {
			var occurrences_c = ConvertValue(occurrences);
			var aabb_c = ConvertValue(aabb);
			ImportSDK_mapUvOnCustomAABB(occurrences_c, aabb_c, uv3dSize, channel, overrideExistingUVs ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			Geom_AABB_free(ref aabb_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_normalizeUV(Scene.OccurrenceList_c occurrences, Int32 sourceUVChannel, Int32 destinationUVChannel, Int32 uniform, Int32 sharedUVSpace, Int32 ignoreNullIslands);
		/// <summary>
		/// Normalize UVs to fit in the [0-1] uv space
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="sourceUVChannel">UV Channel to normalize</param>
		/// <param name="destinationUVChannel">UV channel to store the normalized UV (if -1, sourceUVChannel will be replaced)</param>
		/// <param name="uniform">If true, the scale will be uniform. Else UV can be deformed with a non-uniform scale</param>
		/// <param name="sharedUVSpace">If true, all parts will be processed as if they were merged to avoid overlapping of their UV coordinates</param>
		/// <param name="ignoreNullIslands">If true, islands with null height and width will be ignored and their UV coordinates will be set to [0,0] (Slower if enabled)</param>
		public static void NormalizeUV(Scene.OccurrenceList occurrences, Core.Int sourceUVChannel, Core.Int destinationUVChannel, Core.Bool uniform, Core.Bool sharedUVSpace, Core.Bool ignoreNullIslands) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_normalizeUV(occurrences_c, sourceUVChannel, destinationUVChannel, uniform ? 1 : 0, sharedUVSpace ? 1 : 0, ignoreNullIslands ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Scene.OccurrenceList_c ImportSDK_repackUV(Scene.OccurrenceList_c occurrences, Int32 channel, Int32 shareMap, Int32 resolution, Int32 padding, Int32 uniformRatio, Int32 iterations, Int32 removeOverlaps);
		/// <summary>
		/// Pack existing UV (create atlas)
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="channel">The UV channel to repack</param>
		/// <param name="shareMap">If True, the UV of all given parts will be packed together</param>
		/// <param name="resolution">Resolution wanted for the final map</param>
		/// <param name="padding">Set the padding (in pixels) between UV islands</param>
		/// <param name="uniformRatio">If true, UV of different part will have the same ratio</param>
		/// <param name="iterations">Fitting iterations</param>
		/// <param name="removeOverlaps">Remove overlaps to avoid multiple triangles UVs to share the same pixel</param>
		public static Scene.OccurrenceList RepackUV(Scene.OccurrenceList occurrences, Core.Int channel, Core.Bool shareMap, Core.Int resolution, Core.Int padding, Core.Bool uniformRatio, Core.Int iterations, Core.Bool removeOverlaps) {
			var occurrences_c = ConvertValue(occurrences);
			var ret = ImportSDK_repackUV(occurrences_c, channel, shareMap ? 1 : 0, resolution, padding, uniformRatio ? 1 : 0, iterations, removeOverlaps ? 1 : 0);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Scene_OccurrenceList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_swapUvChannels(Scene.OccurrenceList_c occurrences, Int32 firstChannel, Int32 secondChannel);
		/// <summary>
		/// Swap two UV channels
		/// </summary>
		/// <param name="occurrences">Occurrences of part to process</param>
		/// <param name="firstChannel">First UV Channel to swap</param>
		/// <param name="secondChannel">Second UV Channel to swap</param>
		public static void SwapUvChannels(Scene.OccurrenceList occurrences, Core.Int firstChannel, Core.Int secondChannel) {
			var occurrences_c = ConvertValue(occurrences);
			ImportSDK_swapUvChannels(occurrences_c, firstChannel, secondChannel);
			Scene_OccurrenceList_free(ref occurrences_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region materials

		[DllImport(PiXYZImportSDK_dll)]
		private static extern void ImportSDK_exportImage(System.UInt32 image, string filename);
		/// <summary>
		/// Export an image
		/// </summary>
		/// <param name="image">Identifier of the image to export</param>
		/// <param name="filename">Filename of the file to export</param>
		public static void ExportImage(Core.Ident image, Core.String filename) {
			ImportSDK_exportImage(image, filename);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.ImageList_c ImportSDK_getAllImages();
		/// <summary>
		/// Returns all the images loaded in the current session
		/// </summary>
		public static Material.ImageList GetAllImages() {
			var ret = ImportSDK_getAllImages();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ImageList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.MaterialList_c ImportSDK_getAllMaterials();
		/// <summary>
		/// Retrieve the list of all the materials in the material library
		/// </summary>
		public static Material.MaterialList GetAllMaterials() {
			var ret = ImportSDK_getAllMaterials();
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_MaterialList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.ColorMaterialInfos_c ImportSDK_getColorMaterialInfos(System.UInt32 material);
		/// <summary>
		/// Get color material properties
		/// </summary>
		/// <param name="material">The material to get properties</param>
		public static Material.ColorMaterialInfos GetColorMaterialInfos(Core.Ident material) {
			var ret = ImportSDK_getColorMaterialInfos(material);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ColorMaterialInfos_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.ImageDefinition_c ImportSDK_getImageDefinition(System.UInt32 image);
		/// <summary>
		/// Returns the raw data of an image
		/// </summary>
		/// <param name="image">Image's definition</param>
		public static Material.ImageDefinition GetImageDefinition(Core.Ident image) {
			var ret = ImportSDK_getImageDefinition(image);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ImageDefinition_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.ImageDefinitionList_c ImportSDK_getImageDefinitions(Material.ImageList_c images);
		/// <summary>
		/// Returns the raw data of a set of images
		/// </summary>
		/// <param name="images">The images</param>
		public static Material.ImageDefinitionList GetImageDefinitions(Material.ImageList images) {
			var images_c = ConvertValue(images);
			var ret = ImportSDK_getImageDefinitions(images_c);
			Material_ImageList_free(ref images_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ImageDefinitionList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.MaterialDefinitionList_c ImportSDK_getMaterialDefinitions(Material.MaterialList_c materials);
		/// <summary>
		/// Returns the properties of a set of PBR Materials
		/// </summary>
		/// <param name="materials">The PBR Materials</param>
		public static Material.MaterialDefinitionList GetMaterialDefinitions(Material.MaterialList materials) {
			var materials_c = ConvertValue(materials);
			var ret = ImportSDK_getMaterialDefinitions(materials_c);
			Material_MaterialList_free(ref materials_c);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_MaterialDefinitionList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Int32 ImportSDK_getMaterialPatternType(System.UInt32 material);
		/// <summary>
		/// Returns the MaterialPatternType name of the material
		/// </summary>
		/// <param name="material">The material to find the pattern</param>
		public static Material.MaterialPatternType GetMaterialPatternType(Core.Ident material) {
			var ret = ImportSDK_getMaterialPatternType(material);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Material.MaterialPatternType)ret;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.PBRMaterialInfos_c ImportSDK_getPBRMaterialInfos(System.UInt32 material);
		/// <summary>
		/// Get PBR  material properties
		/// </summary>
		/// <param name="material">The material to get properties</param>
		public static Material.PBRMaterialInfos GetPBRMaterialInfos(Core.Ident material) {
			var ret = ImportSDK_getPBRMaterialInfos(material);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_PBRMaterialInfos_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.StandardMaterialInfos_c ImportSDK_getStandardMaterialInfos(System.UInt32 material);
		/// <summary>
		/// Get standard material properties
		/// </summary>
		/// <param name="material">The material to get properties</param>
		public static Material.StandardMaterialInfos GetStandardMaterialInfos(Core.Ident material) {
			var ret = ImportSDK_getStandardMaterialInfos(material);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_StandardMaterialInfos_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZImportSDK_dll)]
		private static extern Material.UnlitTextureMaterialInfos_c ImportSDK_getUnlitTextureMaterialInfos(System.UInt32 material);
		/// <summary>
		/// Get unlit texture material properties
		/// </summary>
		/// <param name="material">The material to get properties</param>
		public static Material.UnlitTextureMaterialInfos GetUnlitTextureMaterialInfos(Core.Ident material) {
			var ret = ImportSDK_getUnlitTextureMaterialInfos(material);
			System.String err = ConvertValue(ImportSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_UnlitTextureMaterialInfos_free(ref ret);
			return convRet;
		}

		#endregion

	}
}
