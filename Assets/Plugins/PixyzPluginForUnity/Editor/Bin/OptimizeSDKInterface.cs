#pragma warning disable CA2101

using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Security;

namespace Pixyz.OptimizeSDK.Native {

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
	public class Double {
		System.Double value { get; set; }
		Double(System.Double v) { value = v; }
		public static implicit operator System.Double(Double self) { return self.value; }
		public static implicit operator Double(System.Double v) { return new Double(v); }
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
	public class Byte {
		System.Byte value { get; set; }
		Byte(System.Byte v) { value = v; }
		public static implicit operator System.Byte(Byte self) { return self.value; }
		public static implicit operator Byte(System.Byte v) { return new Byte(v); }
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
	public class EntityList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public EntityList() {}
		public EntityList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](EntityList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public EntityList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EntityList_c
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
	public class ULong {
		System.UInt64 value { get; set; }
		ULong(System.UInt64 v) { value = v; }
		public static implicit operator System.UInt64(ULong self) { return self.value; }
		public static implicit operator ULong(System.UInt64 v) { return new ULong(v); }
	}
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class ULongList {
		public System.UInt64[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public ULongList() {}
		public ULongList(System.UInt64[] tab) { list = tab; }
		public static implicit operator System.UInt64[](ULongList o) { return o.list; }
		public System.UInt64 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public ULongList(int size) { list = new System.UInt64[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ULongList_c
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

	public enum Verbose
	{
		ERR = 0,
		WARNING = 1,
		INFO = 2,
		SCRIPT = 5,
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
	public class OBB
	{
		public OBB() {}
		public OBB(OBB o) {
			this.corner = o.corner;
			this.xAxis = o.xAxis;
			this.yAxis = o.yAxis;
			this.zAxis = o.zAxis;
		}
		public Point3 corner;
		public Point3 xAxis;
		public Point3 yAxis;
		public Point3 zAxis;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct OBB_c
	{
		internal Point3_c corner;
		internal Point3_c xAxis;
		internal Point3_c yAxis;
		internal Point3_c zAxis;
	}

}

namespace Material {

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

}

namespace Polygonal {

	public enum StyleType
	{
		NORMAL = 0,
		STIPPLE = 1,
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
	public class PlaceholderJointList {
		public System.UInt32[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public PlaceholderJointList() {}
		public PlaceholderJointList(System.UInt32[] tab) { list = tab; }
		public static implicit operator System.UInt32[](PlaceholderJointList o) { return o.list; }
		public System.UInt32 this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public PlaceholderJointList(int size) { list = new System.UInt32[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PlaceholderJointList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
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

namespace Algo {

	public enum SelectionLevel
	{
		Parts = 0,
		Patches = 1,
		Polygons = 2,
	}

	public enum MapType
	{
		Diffuse = 0,
		Normal = 1,
		Opacity = 2,
		Roughness = 3,
		Specular = 4,
		Metallic = 5,
		AO = 6,
		PartId = 7,
		MaterialId = 8,
		AO16 = 9,
		AO32 = 10,
		AO64 = 11,
		AO128 = 12,
		Bent16 = 13,
		Bent32 = 14,
		Bent64 = 15,
		Bent128 = 16,
		UV0 = 17,
		UV1 = 18,
		UV2 = 19,
		Displacement = 20,
		LocalPosition = 21,
		GlobalPosition = 22,
	}

	public enum BakingMethod
	{
		RayOnly = 0,
		ProjOnly = 1,
		RayAndProj = 2,
	}

	public enum ReplaceByBoxType
	{
		Minimum = 0,
		LocallyAligned = 1,
	}

	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class MapTypeList {
		public MapType[] list;
		public int length { get { return (list != null) ? list.Length : 0; } }
		public MapTypeList(MapType[] tab) { list = tab; }
		public static implicit operator MapType[](MapTypeList o) { return o.list; }
		public MapType this[int index] {
			get { return list[index]; }
			set { list[index] = value; }
		}
		public MapTypeList(int size) { list = new MapType[size]; }
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MapTypeList_c
	{
		internal System.UInt64 size;
		internal IntPtr ptr;
	}

}

	public enum CreateOccluder
	{
		Occludee = 0,
		Occluder = 1,
	}

#endregion

	public static partial class NativeInterface {

#if !PXZ_CUSTOM_DLL_PATH
	#if PXZ_OS_LINUX
		private const string PiXYZOptimizeSDK_dll = "libPiXYZOptimizeSDK";
		private const string memcpy_dll = "libc.so.6";
	#elif PXZ_OS_WIN32
		private const string PiXYZOptimizeSDK_dll = "PiXYZOptimizeSDK";
		private const string memcpy_dll = "msvcrt.dll";
	#else
		private const string PiXYZOptimizeSDK_dll = "PiXYZOptimizeSDK_undefined_platform";
		private const string memcpy_dll = "msvcrt.dll_undefined_platform";
	#endif

#endif

		#region Conversion

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Algo_MapTypeList_init(ref Algo.MapTypeList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Algo_MapTypeList_free(ref Algo.MapTypeList_c list);

		private static Algo.MapTypeList ConvertValue(Algo.MapTypeList_c s) {
			Algo.MapTypeList list = new Algo.MapTypeList((int)s.size);
			if (s.size==0) return list;
			int[] tab = new int[s.size];
			Marshal.Copy(s.ptr, tab, 0, (int)s.size);
			for (int i = 0; i < (int)s.size; ++i) {
				list.list[i] = (Algo.MapType)tab[i];
			}
			return list;
		}

		private static Algo.MapTypeList_c ConvertValue(Algo.MapTypeList s) {
			Algo.MapTypeList_c list =  new Algo.MapTypeList_c();
			Algo_MapTypeList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)s.list[i];
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Core_Date_init(ref Core.Date_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_StringList_init(ref Core.StringList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Core_WebLicenseInfo_init(ref Core.WebLicenseInfo_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_WebLicenseInfoList_init(ref Core.WebLicenseInfoList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_EntityList_init(ref Core.EntityList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_EntityList_free(ref Core.EntityList_c list);

		private static Core.EntityList ConvertValue(Core.EntityList_c s) {
			Core.EntityList list = new Core.EntityList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Core.EntityList_c ConvertValue(Core.EntityList s) {
			Core.EntityList_c list =  new Core.EntityList_c();
			Core_EntityList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Core_LicenseInfos_init(ref Core.LicenseInfos_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Core_ColorAlpha_init(ref Core.ColorAlpha_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_ColorAlphaList_init(ref Core.ColorAlphaList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_IntList_init(ref Core.IntList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_ULongList_init(ref Core.ULongList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_ULongList_free(ref Core.ULongList_c list);

		private static Core.ULongList ConvertValue(Core.ULongList_c s) {
			Core.ULongList list = new Core.ULongList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt64>(s.ptr, (int)s.size);
			return list;
		}

		private static Core.ULongList_c ConvertValue(Core.ULongList s) {
			Core.ULongList_c list =  new Core.ULongList_c();
			Core_ULongList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			long[] tab = new long[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (long)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Core_Color_init(ref Core.Color_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_ByteList_init(ref Core.ByteList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Core_BoolList_init(ref Core.BoolList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Array4_init(ref Geom.Array4_c arr, UInt64 size);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Matrix4_init(ref Geom.Matrix4_c arr, UInt64 size);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Point2_init(ref Geom.Point2_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Vector4I_init(ref Geom.Vector4I_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Point3_init(ref Geom.Point3_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Point4_init(ref Geom.Point4_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_Curvatures_init(ref Geom.Curvatures_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Vector3List_init(ref Geom.Vector3List_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Point3List_init(ref Geom.Point3List_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Matrix4List_init(ref Geom.Matrix4List_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Point2List_init(ref Geom.Point2List_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Point2ListList_init(ref Geom.Point2ListList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_CurvaturesList_init(ref Geom.CurvaturesList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Vector4List_init(ref Geom.Vector4List_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Geom_Vector4IList_init(ref Geom.Vector4IList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_AABB_init(ref Geom.AABB_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_OBB_init(ref Geom.OBB_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Geom_OBB_free(ref Geom.OBB_c str);

	private static Geom.OBB ConvertValue(Geom.OBB_c s) {
		Geom.OBB ss = new Geom.OBB();
		ss.corner = ConvertValue(s.corner);
		ss.xAxis = ConvertValue(s.xAxis);
		ss.yAxis = ConvertValue(s.yAxis);
		ss.zAxis = ConvertValue(s.zAxis);
		return ss;
	}

	private static Geom.OBB_c ConvertValue(Geom.OBB s) {
		Geom.OBB_c ss = new Geom.OBB_c();
		Geom_OBB_init(ref ss);
		ss.corner = ConvertValue(s.corner);
		ss.xAxis = ConvertValue(s.xAxis);
		ss.yAxis = ConvertValue(s.yAxis);
		ss.zAxis = ConvertValue(s.zAxis);
		return ss;
	}

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Material_Texture_init(ref Material.Texture_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Material_ColorOrTexture_init(ref Material.ColorOrTexture_c sel);

	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Material_ImageList_init(ref Material.ImageList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Material_CoeffOrTexture_init(ref Material.CoeffOrTexture_c sel);

	[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Material_MaterialDefinition_init(ref Material.MaterialDefinition_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Material_MaterialDefinitionList_init(ref Material.MaterialDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Material_MaterialList_init(ref Material.MaterialList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Material_ImageDefinition_init(ref Material.ImageDefinition_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Material_ImageDefinitionList_init(ref Material.ImageDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Polygonal_StylizedLine_init(ref Polygonal.StylizedLine_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_StylizedLineList_init(ref Polygonal.StylizedLineList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_PlaceholderJointList_init(ref Polygonal.PlaceholderJointList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_PlaceholderJointList_free(ref Polygonal.PlaceholderJointList_c list);

		private static Polygonal.PlaceholderJointList ConvertValue(Polygonal.PlaceholderJointList_c s) {
			Polygonal.PlaceholderJointList list = new Polygonal.PlaceholderJointList((int)s.size);
			if (s.size==0) return list;
				list.list = CopyMemory<System.UInt32>(s.ptr, (int)s.size);
			return list;
		}

		private static Polygonal.PlaceholderJointList_c ConvertValue(Polygonal.PlaceholderJointList s) {
			Polygonal.PlaceholderJointList_c list =  new Polygonal.PlaceholderJointList_c();
			Polygonal_PlaceholderJointList_init(ref list, s == null ? 0 : (System.UInt64)s.length);
			if(list.size == 0) return list;
			int[] tab = new int[list.size];
			for (int i = 0; i < (int)list.size; ++i)
				tab[i] = (int)(s.list[i]);
			Marshal.Copy(tab, 0, list.ptr, (int)list.size);
			return list;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_JointList_init(ref Polygonal.JointList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Polygonal_DressedPoly_init(ref Polygonal.DressedPoly_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_DressedPolyList_init(ref Polygonal.DressedPolyList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_MeshList_init(ref Polygonal.MeshList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

	[DllImport(PiXYZOptimizeSDK_dll)]
	private static extern void Polygonal_MeshDefinition_init(ref Polygonal.MeshDefinition_c str);
	[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void Polygonal_MeshDefinitionList_init(ref Polygonal.MeshDefinitionList_c list, UInt64 size);
		[DllImport(PiXYZOptimizeSDK_dll)]
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_getLastError();

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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_replaceByBox(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices, Int32 boxType);
		/// <summary>
		/// Replace mesh with the specified box type
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices of input meshes</param>
		/// <param name="boxType">Type of the box that will replace the mesh</param>
		public static void ReplaceByBox(Polygonal.MeshList meshes, Geom.Matrix4List matrices, Algo.ReplaceByBoxType boxType) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_replaceByBox(meshes_c, matrices_c, (int)boxType);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region Combine and Baking

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_bakeMaterials(Polygonal.MeshList_c sourceMeshes, System.UInt32 targetMesh, Int32 resolution, Int32 padding, Int32 applyMaterial, Int32 method, Algo.MapTypeList_c maps, Geom.Matrix4List_c sourceMatrices, Geom.Matrix4_c targetMatrix);
		/// <summary>
		/// Bakes materials from a mesh to another mesh (the target). The result is a baked material that can be applied to the target mesh.
		/// </summary>
		/// <param name="sourceMeshes">Source mesh from which materials should be baked</param>
		/// <param name="targetMesh">Target geometry on which the baked material should be applied (the mesh needs not overlapping normalized UVs to bake on textures)</param>
		/// <param name="resolution">Output textures resolution</param>
		/// <param name="padding">Minimal padding (in pixels) between UVs islands</param>
		/// <param name="applyMaterial">Should we apply the generated material to the target mesh </param>
		/// <param name="method">Choose the ray or projection method</param>
		/// <param name="maps">Maps to bake</param>
		/// <param name="sourceMatrices">List of matrices (if empty, identity matrices are used)</param>
		/// <param name="targetMatrix">Target mesh matrix</param>
		public static Core.Ident BakeMaterials(Polygonal.MeshList sourceMeshes, Core.Ident targetMesh, Core.Int resolution, Core.Int padding, Core.Bool applyMaterial, Algo.BakingMethod method, Algo.MapTypeList maps, Geom.Matrix4List sourceMatrices, Geom.Matrix4 targetMatrix) {
			var sourceMeshes_c = ConvertValue(sourceMeshes);
			var maps_c = ConvertValue(maps);
			var sourceMatrices_c = ConvertValue(sourceMatrices);
			var targetMatrix_c = ConvertValue(targetMatrix);
			var ret = OptimizeSDK_bakeMaterials(sourceMeshes_c, targetMesh, resolution, padding, applyMaterial ? 1 : 0, (int)method, maps_c, sourceMatrices_c, targetMatrix_c);
			Polygonal_MeshList_free(ref sourceMeshes_c);
			Algo_MapTypeList_free(ref maps_c);
			Geom_Matrix4List_free(ref sourceMatrices_c);
			Geom_Matrix4_free(ref targetMatrix_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_combineMeshes(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices, Int32 forceUVCreation, Int32 resolution, Int32 padding, Int32 bakingMethod);
		/// <summary>
		/// Combine all given meshes to one mesh with one material (baked)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices (same size as the list of meshes)</param>
		/// <param name="forceUVCreation">If True, new UVs will be generated even if meshes already have uvs</param>
		/// <param name="resolution">Output textures resolution</param>
		/// <param name="padding">Padding in pixels</param>
		/// <param name="bakingMethod">Choose the ray or projection method</param>
		public static Core.Ident CombineMeshes(Polygonal.MeshList meshes, Geom.Matrix4List matrices, Core.Bool forceUVCreation, Core.Int resolution, Core.Int padding, Algo.BakingMethod bakingMethod) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_combineMeshes(meshes_c, matrices_c, forceUVCreation ? 1 : 0, resolution, padding, (int)bakingMethod);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_combineSubMeshesByMaterial(Polygonal.MeshList_c meshes);
		/// <summary>
		/// Merges submeshes (defined by MeshDefinition's dressedPolys) by material
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		public static void CombineSubMeshesByMaterial(Polygonal.MeshList meshes) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_combineSubMeshesByMaterial(meshes_c);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_createBillboard(Polygonal.MeshList_c sourceMeshes, Geom.Matrix4List_c sourceMatrices, Int32 resolution, Int32 XPositive, Int32 XNegative, Int32 YPositive, Int32 YNegative, Int32 ZPositive, Int32 ZNegative, Int32 moveFacesToCenter);
		/// <summary>
		/// Create a billboard (6 planes) from a set of meshes
		/// </summary>
		/// <param name="sourceMeshes">Source mesh from which materials should be baked</param>
		/// <param name="sourceMatrices">List of matrices (if empty, identity matrices are used)</param>
		/// <param name="resolution">Total resolution of the billboard (contains all wanted faces)</param>
		/// <param name="XPositive">Bake face facing X+</param>
		/// <param name="XNegative">Bake face facing X-</param>
		/// <param name="YPositive">Bake face facing Y+</param>
		/// <param name="YNegative">Bake face facing Y-</param>
		/// <param name="ZPositive">Bake face facing Z+</param>
		/// <param name="ZNegative">Bake face facing Z-</param>
		/// <param name="moveFacesToCenter">If true, all face are moved to the center of the AABB of the occurrences, else it will shape an AABB</param>
		public static Core.Ident CreateBillboard(Polygonal.MeshList sourceMeshes, Geom.Matrix4List sourceMatrices, Core.Int resolution, Core.Bool XPositive, Core.Bool XNegative, Core.Bool YPositive, Core.Bool YNegative, Core.Bool ZPositive, Core.Bool ZNegative, Core.Bool moveFacesToCenter) {
			var sourceMeshes_c = ConvertValue(sourceMeshes);
			var sourceMatrices_c = ConvertValue(sourceMatrices);
			var ret = OptimizeSDK_createBillboard(sourceMeshes_c, sourceMatrices_c, resolution, XPositive ? 1 : 0, XNegative ? 1 : 0, YPositive ? 1 : 0, YNegative ? 1 : 0, ZPositive ? 1 : 0, ZNegative ? 1 : 0, moveFacesToCenter ? 1 : 0);
			Polygonal_MeshList_free(ref sourceMeshes_c);
			Geom_Matrix4List_free(ref sourceMatrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		#endregion

		#region Configuration

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_SetPixyzMainThread();
		/// <summary>
		/// Use this function if you need to call optimization functions (decimate, repair, etc.) on a different thread than the one that initialized pixyz
		/// </summary>
		public static void SetPixyzMainThread() {
			OptimizeSDK_SetPixyzMainThread();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_initialize(string productName, string validationKey, string license, string subProcessName);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="productName"></param>
		/// <param name="validationKey"></param>
		/// <param name="license">License file as string</param>
		/// <param name="subProcessName">Name of subProcess</param>
		public static void Initialize(Core.String productName, Core.String validationKey, Core.String license, Core.String subProcessName) {
			OptimizeSDK_initialize(productName, validationKey, license, subProcessName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_addConsoleVerbose(Int32 level);
		/// <summary>
		/// add a console verbose level
		/// </summary>
		/// <param name="level">Verbose level</param>
		public static void AddConsoleVerbose(Core.Verbose level) {
			OptimizeSDK_addConsoleVerbose((int)level);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_configureInterfaceLogger(Int32 enableFunction, Int32 enableParameters, Int32 enableExecutionTime);
		/// <summary>
		/// Set new configuration for the Interface Logger
		/// </summary>
		/// <param name="enableFunction">If true, the called function names will be print</param>
		/// <param name="enableParameters">If true, the called function parameters will be print (only if enableFunction=true too)</param>
		/// <param name="enableExecutionTime">If true, the called functions execution times will be print</param>
		public static void ConfigureInterfaceLogger(Core.Bool enableFunction, Core.Bool enableParameters, Core.Bool enableExecutionTime) {
			OptimizeSDK_configureInterfaceLogger(enableFunction ? 1 : 0, enableParameters ? 1 : 0, enableExecutionTime ? 1 : 0);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_popProgression();
		/// <summary>
		/// Leave current progression level
		/// </summary>
		public static void PopProgression() {
			OptimizeSDK_popProgression();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_pushProgression(Int32 stepCount, string progressName);
		/// <summary>
		/// Create a new progression level
		/// </summary>
		/// <param name="stepCount">Step count</param>
		/// <param name="progressName">Name of the progression step</param>
		public static void PushProgression(Core.Int stepCount, Core.String progressName) {
			OptimizeSDK_pushProgression(stepCount, progressName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_removeConsoleVerbose(Int32 level);
		/// <summary>
		/// remove a console verbose level
		/// </summary>
		/// <param name="level">Verbose level</param>
		public static void RemoveConsoleVerbose(Core.Verbose level) {
			OptimizeSDK_removeConsoleVerbose((int)level);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_setLogFile(string path);
		/// <summary>
		/// set the path of the log file
		/// </summary>
		/// <param name="path">Path of the log file</param>
		public static void SetLogFile(Core.String path) {
			OptimizeSDK_setLogFile(path);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_stepProgression(Int32 stepCount);
		/// <summary>
		/// Add a step to current progression level
		/// </summary>
		/// <param name="stepCount">Step count</param>
		public static void StepProgression(Core.Int stepCount) {
			OptimizeSDK_stepProgression(stepCount);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		private delegate void ProgressChangedDelegate(System.IntPtr userdata, Core.Int progress);
		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.Ident OptimizeSDK_addProgressChangedCallback(ProgressChangedDelegate callback, System.IntPtr userdata);
		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_removeProgressChangedCallback(Core.Ident id);

		private static Core.Ident addProgressChangedCallback(ProgressChangedDelegate callback, System.IntPtr userdata) {
			return OptimizeSDK_addProgressChangedCallback(callback, userdata);
		}

		private static void removeProgressChangedCallback(Core.Ident id) {
			OptimizeSDK_removeProgressChangedCallback(id);
		}

		public static class ProgressChangedTask
		{
			public class TaskResult
			{
				public volatile Core.Int progress;
			}
			internal class TaskExecution
			{
				private volatile CancellationTokenSource _cancelTokenSource = null;
				private volatile SemaphoreSlim _isCompleted = null;
				private volatile TaskResult _taskResult;
				private volatile uint _callbackId;

				private GCHandle _delegatePtr;

				private CancellationToken _cancelToken;
				public TaskExecution(CancellationTokenSource cancelTokenSource = null)
				{
					_isCompleted = new SemaphoreSlim(0, 1);
					_cancelTokenSource = cancelTokenSource;

					if(_cancelTokenSource != null)
						_cancelToken = _cancelTokenSource.Token;
				}
				public TaskResult Run()
				{
					try
					{
						ProgressChangedDelegate callback = Completed;

						_delegatePtr = GCHandle.Alloc(callback, GCHandleType.Pinned);
						_callbackId = addProgressChangedCallback(callback, IntPtr.Zero);

						if(_cancelTokenSource != null)
							_isCompleted.Wait(_cancelToken);
						else
							_isCompleted.Wait();

						_delegatePtr.Free();

						return _taskResult;
					}
					catch(OperationCanceledException)
					{
						removeProgressChangedCallback(_callbackId);
						_delegatePtr.Free();

						return null;
					}
				}
				private void Completed(System.IntPtr userdata, Core.Int progress)
				{
					try
					{
						_taskResult = new TaskResult();
						_taskResult.progress = progress;

						_isCompleted.Release();
						removeProgressChangedCallback(_callbackId);
					}
					catch(System.Exception)
					{
						_isCompleted.Release();
						removeProgressChangedCallback(_callbackId);
					}
				}
			}
			public static Task<TaskResult> WaitProgressChanged(CancellationTokenSource cancelSource = null)
			{
				return Task.Run(new TaskExecution(cancelSource).Run);
			}
		}
		#endregion

		#region Conversion

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_createImage(Material.ImageDefinition_c imageDefinition);
		/// <summary>
		/// Import an image from its raw data
		/// </summary>
		/// <param name="imageDefinition">The image definition</param>
		public static Core.Ident CreateImage(Material.ImageDefinition imageDefinition) {
			var imageDefinition_c = ConvertValue(imageDefinition);
			var ret = OptimizeSDK_createImage(imageDefinition_c);
			Material_ImageDefinition_free(ref imageDefinition_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Material.ImageList_c OptimizeSDK_createImages(Material.ImageDefinitionList_c imageDefinitions);
		/// <summary>
		/// Import images from their raw data
		/// </summary>
		/// <param name="imageDefinitions">The image definitions</param>
		public static Material.ImageList CreateImages(Material.ImageDefinitionList imageDefinitions) {
			var imageDefinitions_c = ConvertValue(imageDefinitions);
			var ret = OptimizeSDK_createImages(imageDefinitions_c);
			Material_ImageDefinitionList_free(ref imageDefinitions_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ImageList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Polygonal.PlaceholderJointList_c OptimizeSDK_createJoints(Core.ULongList_c data, Geom.Matrix4List_c worldMatrices);
		/// <summary>
		/// Create fake joint to store in mesh definitions. Thus we can retrieve stored data from getJointPlaceholders
		/// </summary>
		/// <param name="data">Create as much joints as there are data, each joint store one data</param>
		/// <param name="worldMatrices">World matrix for each joints</param>
		public static Polygonal.PlaceholderJointList CreateJoints(Core.ULongList data, Geom.Matrix4List worldMatrices) {
			var data_c = ConvertValue(data);
			var worldMatrices_c = ConvertValue(worldMatrices);
			var ret = OptimizeSDK_createJoints(data_c, worldMatrices_c);
			Core_ULongList_free(ref data_c);
			Geom_Matrix4List_free(ref worldMatrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_PlaceholderJointList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_createMaterial(Material.MaterialDefinition_c materialDefinition);
		/// <summary>
		/// Create PBR material from a material definition
		/// </summary>
		/// <param name="materialDefinition">The structure containing all the PBR material informations</param>
		public static Core.Ident CreateMaterial(Material.MaterialDefinition materialDefinition) {
			var materialDefinition_c = ConvertValue(materialDefinition);
			var ret = OptimizeSDK_createMaterial(materialDefinition_c);
			Material_MaterialDefinition_free(ref materialDefinition_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Material.MaterialList_c OptimizeSDK_createMaterials(Material.MaterialDefinitionList_c materialDefinitions);
		/// <summary>
		/// Create PBR materials from material definitions
		/// </summary>
		/// <param name="materialDefinitions">Material definitions containing properties for each given material</param>
		public static Material.MaterialList CreateMaterials(Material.MaterialDefinitionList materialDefinitions) {
			var materialDefinitions_c = ConvertValue(materialDefinitions);
			var ret = OptimizeSDK_createMaterials(materialDefinitions_c);
			Material_MaterialDefinitionList_free(ref materialDefinitions_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_MaterialList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_createMesh(Polygonal.MeshDefinition_c meshDefinition);
		/// <summary>
		/// Create a new mesh with the given MeshDefinition
		/// </summary>
		/// <param name="meshDefinition">Mesh definition</param>
		public static Core.Ident CreateMesh(Polygonal.MeshDefinition meshDefinition) {
			var meshDefinition_c = ConvertValue(meshDefinition);
			var ret = OptimizeSDK_createMesh(meshDefinition_c);
			Polygonal_MeshDefinition_free(ref meshDefinition_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Polygonal.MeshList_c OptimizeSDK_createMeshes(Polygonal.MeshDefinitionList_c meshDefinitions);
		/// <summary>
		/// Create new meshes with the given MeshDefinitions
		/// </summary>
		/// <param name="meshDefinitions">The MeshDefinitions</param>
		public static Polygonal.MeshList CreateMeshes(Polygonal.MeshDefinitionList meshDefinitions) {
			var meshDefinitions_c = ConvertValue(meshDefinitions);
			var ret = OptimizeSDK_createMeshes(meshDefinitions_c);
			Polygonal_MeshDefinitionList_free(ref meshDefinitions_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_MeshList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Material.ImageDefinition_c OptimizeSDK_getImage(System.UInt32 image);
		/// <summary>
		/// Returns the raw data of an image
		/// </summary>
		/// <param name="image">Image's definition</param>
		public static Material.ImageDefinition GetImage(Core.Ident image) {
			var ret = OptimizeSDK_getImage(image);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ImageDefinition_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Material.ImageDefinitionList_c OptimizeSDK_getImages(Material.ImageList_c images);
		/// <summary>
		/// Returns the raw data of a set of images
		/// </summary>
		/// <param name="images">The images</param>
		public static Material.ImageDefinitionList GetImages(Material.ImageList images) {
			var images_c = ConvertValue(images);
			var ret = OptimizeSDK_getImages(images_c);
			Material_ImageList_free(ref images_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_ImageDefinitionList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.ULongList_c OptimizeSDK_getJoints(Polygonal.PlaceholderJointList_c joints);
		/// <summary>
		/// Get data stored in joint placeholders
		/// </summary>
		/// <param name="joints">Placeholder joints to get data from</param>
		public static Core.ULongList GetJoints(Polygonal.PlaceholderJointList joints) {
			var joints_c = ConvertValue(joints);
			var ret = OptimizeSDK_getJoints(joints_c);
			Polygonal_PlaceholderJointList_free(ref joints_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_ULongList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Material.MaterialDefinition_c OptimizeSDK_getMaterial(System.UInt32 material);
		/// <summary>
		/// Returns the properties of a PBR Material
		/// </summary>
		/// <param name="material">The PBR Material</param>
		public static Material.MaterialDefinition GetMaterial(Core.Ident material) {
			var ret = OptimizeSDK_getMaterial(material);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_MaterialDefinition_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Material.MaterialDefinitionList_c OptimizeSDK_getMaterials(Material.MaterialList_c materials);
		/// <summary>
		/// Returns the properties of a set of PBR Materials
		/// </summary>
		/// <param name="materials">The PBR Materials</param>
		public static Material.MaterialDefinitionList GetMaterials(Material.MaterialList materials) {
			var materials_c = ConvertValue(materials);
			var ret = OptimizeSDK_getMaterials(materials_c);
			Material_MaterialList_free(ref materials_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Material_MaterialDefinitionList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Polygonal.MeshDefinition_c OptimizeSDK_getMesh(System.UInt32 mesh);
		/// <summary>
		/// Returns the definition
		/// </summary>
		/// <param name="mesh">The mesh to get definition</param>
		public static Polygonal.MeshDefinition GetMesh(Core.Ident mesh) {
			var ret = OptimizeSDK_getMesh(mesh);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_MeshDefinition_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Polygonal.MeshDefinitionList_c OptimizeSDK_getMeshes(Polygonal.MeshList_c meshes);
		/// <summary>
		/// Returns the definition
		/// </summary>
		/// <param name="meshes">The meshes to get definitions</param>
		public static Polygonal.MeshDefinitionList GetMeshes(Polygonal.MeshList meshes) {
			var meshes_c = ConvertValue(meshes);
			var ret = OptimizeSDK_getMeshes(meshes_c);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_MeshDefinitionList_free(ref ret);
			return convRet;
		}

		#endregion

		#region Healing

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_createNormals(Polygonal.MeshList_c meshes, System.Double sharpAngle, Int32 areaWeighting, Int32 overriding);
		/// <summary>
		/// Create tangent attributes on input meshes
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="sharpAngle">Edges with an angle between their polygons greater than sharpEdge will be considered sharp (default use the Pixyz sharpAngle parameter)</param>
		/// <param name="areaWeighting">Weights normals with triangle surface area</param>
		/// <param name="overriding">If true, override existing normals, else only create normals on meshes without normals</param>
		public static void CreateNormals(Polygonal.MeshList meshes, Core.Double sharpAngle, Core.Bool areaWeighting, Core.Bool overriding) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_createNormals(meshes_c, sharpAngle, areaWeighting ? 1 : 0, overriding ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_createTangents(Polygonal.MeshList_c meshes, System.Double sharpAngle, Int32 uvChannel, Int32 overriding);
		/// <summary>
		/// Create tangent attributes on input meshes
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="sharpAngle">Edges with an angle between their polygons greater than sharpEdge will be considered sharp (default use the Pixyz sharpAngle parameter)</param>
		/// <param name="uvChannel">UV channel to use for the tangents creation</param>
		/// <param name="overriding">If true, override existing tangents, else only create tangents on meshes without tangents</param>
		public static void CreateTangents(Polygonal.MeshList meshes, Core.Double sharpAngle, Core.Int uvChannel, Core.Bool overriding) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_createTangents(meshes_c, sharpAngle, uvChannel, overriding ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_fixTJunctions(Polygonal.MeshList_c meshes, System.Double tolerance, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Fix T-Junctions, sew boundary even if the vertices are not coincident
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="tolerance">Connection tolerance</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void FixTJunctions(Polygonal.MeshList meshes, Core.Double tolerance, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_fixTJunctions(meshes_c, tolerance, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_removeDegeneratedPolygons(Polygonal.MeshList_c meshes, System.Double tolerance, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Remove polygons when the smallest height is less than the given tolerance
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="tolerance">Degeneration tolerance</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void RemoveDegeneratedPolygons(Polygonal.MeshList meshes, Core.Double tolerance, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_removeDegeneratedPolygons(meshes_c, tolerance, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_repairMeshes(Polygonal.MeshList_c meshes, System.Double tolerance, Int32 crackNonManifold, Int32 orient, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Repairs disconnected or bad tessellations
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="tolerance">Connection tolerance</param>
		/// <param name="crackNonManifold">At the end of the repair process, crack resulting non-manifold edges</param>
		/// <param name="orient">If true, attempts to reorient the meshes faces correctly</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void RepairMeshes(Polygonal.MeshList meshes, Core.Double tolerance, Core.Bool crackNonManifold, Core.Bool orient, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_repairMeshes(meshes_c, tolerance, crackNonManifold ? 1 : 0, orient ? 1 : 0, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_triangularize(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Triangularize a mesh
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void Triangularize(Polygonal.MeshList meshes, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_triangularize(meshes_c, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_weldVertices(Polygonal.MeshList_c meshes, System.Double tolerance, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Weld near boundary vertices on input meshes
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="tolerance">Connection tolerance</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void WeldVertices(Polygonal.MeshList meshes, Core.Double tolerance, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_weldVertices(meshes_c, tolerance, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region Licenses

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_addWantedToken(string tokenName);
		/// <summary>
		/// Add a license token to the list of wanted optional tokens
		/// </summary>
		/// <param name="tokenName">Wanted token</param>
		public static void AddWantedToken(Core.String tokenName) {
			OptimizeSDK_addWantedToken(tokenName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Int32 OptimizeSDK_checkLicense();
		/// <summary>
		/// check the current license
		/// </summary>
		public static Core.Bool CheckLicense() {
			var ret = OptimizeSDK_checkLicense();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_configureLicenseServer(string address, System.UInt16 port, Int32 flexLM);
		/// <summary>
		/// Configure the license server to use to get floating licenses
		/// </summary>
		/// <param name="address">Server address</param>
		/// <param name="port">Server port</param>
		/// <param name="flexLM">Enable FlexLM license server</param>
		public static void ConfigureLicenseServer(Core.String address, Core.UShort port, Core.Bool flexLM) {
			OptimizeSDK_configureLicenseServer(address, port, flexLM ? 1 : 0);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_generateActivationCode(string filePath);
		/// <summary>
		/// Create an activation code to generate an offline license
		/// </summary>
		/// <param name="filePath">Path to write the activation code</param>
		public static void GenerateActivationCode(Core.String filePath) {
			OptimizeSDK_generateActivationCode(filePath);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_generateDeactivationCode(string filePath);
		/// <summary>
		/// Create an deactivation code to release the license from this machine
		/// </summary>
		/// <param name="filePath">Path to write the deactivation code</param>
		public static void GenerateDeactivationCode(Core.String filePath) {
			OptimizeSDK_generateDeactivationCode(filePath);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.LicenseInfos_c OptimizeSDK_getCurrentLicenseInfos();
		/// <summary>
		/// get informations on current installed license
		/// </summary>
		public static Core.LicenseInfos GetCurrentLicenseInfos() {
			var ret = OptimizeSDK_getCurrentLicenseInfos();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
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

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern getLicenseServerReturn_c OptimizeSDK_getLicenseServer();
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
			var ret = OptimizeSDK_getLicenseServer();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			getLicenseServerReturn retStruct = new getLicenseServerReturn();
			retStruct.serverHost = ConvertValue(ret.serverHost);
			retStruct.serverPort = (Core.UShort)ret.serverPort;
			retStruct.useFlexLM = ConvertValue(ret.useFlexLM);
			return retStruct;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.StringList_c OptimizeSDK_getTokens(Int32 onlyMandatory);
		/// <summary>
		/// Get the list of license tokens for this product
		/// </summary>
		/// <param name="onlyMandatory">If True, optional tokens will not be returned</param>
		public static Core.StringList GetTokens(Core.Bool onlyMandatory) {
			var ret = OptimizeSDK_getTokens(onlyMandatory ? 1 : 0);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_StringList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_installLicense(string licensePath);
		/// <summary>
		/// install a new license
		/// </summary>
		/// <param name="licensePath">Path of the license file</param>
		public static void InstallLicense(Core.String licensePath) {
			OptimizeSDK_installLicense(licensePath);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Int32 OptimizeSDK_isFloatingLicense();
		/// <summary>
		/// Tells if license is floating
		/// </summary>
		public static Core.Bool IsFloatingLicense() {
			var ret = OptimizeSDK_isFloatingLicense();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Int32 OptimizeSDK_isTokenValid(string tokenName);
		/// <summary>
		/// Returns True if a token is owned by the product
		/// </summary>
		/// <param name="tokenName">Token name</param>
		public static Core.Bool IsTokenValid(Core.String tokenName) {
			var ret = OptimizeSDK_isTokenValid(tokenName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_releaseToken(string tokenName);
		/// <summary>
		/// Release an optional license token
		/// </summary>
		/// <param name="tokenName">Token name</param>
		public static void ReleaseToken(Core.String tokenName) {
			OptimizeSDK_releaseToken(tokenName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_releaseWebLicense(string login, string password, System.UInt32 id);
		/// <summary>
		/// release License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		/// <param name="id">WEB license id</param>
		public static void ReleaseWebLicense(Core.String login, Core.String password, Core.Ident id) {
			OptimizeSDK_releaseWebLicense(login, password, id);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_removeWantedToken(string tokenName);
		/// <summary>
		/// remove a license token from the list of wanted optional tokens
		/// </summary>
		/// <param name="tokenName">Unwanted token</param>
		public static void RemoveWantedToken(Core.String tokenName) {
			OptimizeSDK_removeWantedToken(tokenName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_requestWebLicense(string login, string password, System.UInt32 id);
		/// <summary>
		/// request License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		/// <param name="id">WEB license id</param>
		public static void RequestWebLicense(Core.String login, Core.String password, Core.Ident id) {
			OptimizeSDK_requestWebLicense(login, password, id);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.WebLicenseInfoList_c OptimizeSDK_retrieveAvailableLicenses(string login, string password);
		/// <summary>
		/// Retrieves License owned by user WEB account
		/// </summary>
		/// <param name="login">WEB account login</param>
		/// <param name="password">WEB account password</param>
		public static Core.WebLicenseInfoList RetrieveAvailableLicenses(Core.String login, Core.String password) {
			var ret = OptimizeSDK_retrieveAvailableLicenses(login, password);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_WebLicenseInfoList_free(ref ret);
			return convRet;
		}

		#endregion

		#region Optimization

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_decimateToPolycount(Polygonal.MeshList_c meshes, Int32 polycount, Geom.Matrix4List_c matrices, Int32 useVertexWeigths, System.Double vertexWeigthOffset, System.Double vertexWeigthScale, System.Double boundaryWeight, System.Double normalWeight, System.Double uvWeight, System.Double sharpNormalWeight, System.Double uvSeamWeight, Int32 forbidUVFoldovers, Int32 useColorsAsWeights);
		/// <summary>
		/// Reduces the polygon count by choosing wisely vertices to remove until target triangle count is reached
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="polycount">Target triangle count</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		/// <param name="useVertexWeigths">Use vertex importance weights if any. Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="vertexWeigthOffset">Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="vertexWeigthScale">Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="boundaryWeight">Boundaries importance during the decimation (boundary edges are edges connected to one triangle only)</param>
		/// <param name="normalWeight">Normal importance during the decimation</param>
		/// <param name="uvWeight">UV importance during the decimation</param>
		/// <param name="sharpNormalWeight">Importance of sharp edges during the decimation</param>
		/// <param name="uvSeamWeight">Importance of UV seams during the decimation</param>
		/// <param name="forbidUVFoldovers">Forbid UV to fold over during the decimation</param>
		/// <param name="useColorsAsWeights">Use red channel of vertex colors as weigths for the decimation process</param>
		public static void DecimateToPolycount(Polygonal.MeshList meshes, Core.Int polycount, Geom.Matrix4List matrices, Core.Bool useVertexWeigths, Core.Double vertexWeigthOffset, Core.Double vertexWeigthScale, Core.Double boundaryWeight, Core.Double normalWeight, Core.Double uvWeight, Core.Double sharpNormalWeight, Core.Double uvSeamWeight, Core.Bool forbidUVFoldovers, Core.Bool useColorsAsWeights) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_decimateToPolycount(meshes_c, polycount, matrices_c, useVertexWeigths ? 1 : 0, vertexWeigthOffset, vertexWeigthScale, boundaryWeight, normalWeight, uvWeight, sharpNormalWeight, uvSeamWeight, forbidUVFoldovers ? 1 : 0, useColorsAsWeights ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_decimateToQuality(Polygonal.MeshList_c meshes, System.Double errorMax, Geom.Matrix4List_c matrices, Int32 useVertexWeigths, System.Double vertexWeigthOffset, System.Double vertexWeigthScale, System.Double boundaryWeight, System.Double normalWeight, System.Double uvWeight, System.Double sharpNormalWeight, System.Double uvSeamWeight, Int32 forbidUVFoldovers, System.Double normalMaxDeviation, System.Double uvMaxDeviation, System.Double uvSeamMaxDeviation, Int32 useColorsAsWeights);
		/// <summary>
		/// Reduces the polygon count by choosing wisely edges to collapse until target quality is reached (defined by tolerances)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="errorMax">Error max between the simplified mesh and the old one</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		/// <param name="useVertexWeigths">Use vertex importance weights if any. Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="vertexWeigthOffset">Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="vertexWeigthScale">Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="boundaryWeight">Boundaries importance during the decimation (boundary edges are edges connected to one triangle only)</param>
		/// <param name="normalWeight">Normal importance during the decimation</param>
		/// <param name="uvWeight">UV importance during the decimation</param>
		/// <param name="sharpNormalWeight">Importance of sharp edges during the decimation</param>
		/// <param name="uvSeamWeight">Importance of UV seams during the decimation</param>
		/// <param name="forbidUVFoldovers">Forbid UV to fold over during the decimation</param>
		/// <param name="normalMaxDeviation">Constrains the normals deviation on decimated model</param>
		/// <param name="uvMaxDeviation">Constrains the uv deviation on decimated model</param>
		/// <param name="uvSeamMaxDeviation">Constrains the uv seams deviation on decimated model</param>
		/// <param name="useColorsAsWeights">Use red channel of vertex colors as weigths for the decimation process</param>
		public static void DecimateToQuality(Polygonal.MeshList meshes, Core.Double errorMax, Geom.Matrix4List matrices, Core.Bool useVertexWeigths, Core.Double vertexWeigthOffset, Core.Double vertexWeigthScale, Core.Double boundaryWeight, Core.Double normalWeight, Core.Double uvWeight, Core.Double sharpNormalWeight, Core.Double uvSeamWeight, Core.Bool forbidUVFoldovers, Core.Double normalMaxDeviation, Core.Double uvMaxDeviation, Core.Double uvSeamMaxDeviation, Core.Bool useColorsAsWeights) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_decimateToQuality(meshes_c, errorMax, matrices_c, useVertexWeigths ? 1 : 0, vertexWeigthOffset, vertexWeigthScale, boundaryWeight, normalWeight, uvWeight, sharpNormalWeight, uvSeamWeight, forbidUVFoldovers ? 1 : 0, normalMaxDeviation, uvMaxDeviation, uvSeamMaxDeviation, useColorsAsWeights ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_decimateToQualityVertexRemoval(Polygonal.MeshList_c meshes, System.Double surfacicTolerance, System.Double lineicTolerance, System.Double normalTolerance, System.Double uvTolerance, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Reduces the polygon count by choosing wisely vertices to remove until target quality is reached (defined by tolerances)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="surfacicTolerance">Maximum distance between surfacic vertices and resulting simplified surfaces</param>
		/// <param name="lineicTolerance">Maximum distance between lineic vertices and resulting simplified lines</param>
		/// <param name="normalTolerance">Maximum angle between original normals and those interpolated on the simplified surface</param>
		/// <param name="uvTolerance">Maximum distance (in UV space) between original texcoords and those interpolated on the simplified surface</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void DecimateToQualityVertexRemoval(Polygonal.MeshList meshes, Core.Double surfacicTolerance, Core.Double lineicTolerance, Core.Double normalTolerance, Core.Double uvTolerance, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_decimateToQualityVertexRemoval(meshes_c, surfacicTolerance, lineicTolerance, normalTolerance, uvTolerance, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_decimateToRatio(Polygonal.MeshList_c meshes, System.Double ratio, Geom.Matrix4List_c matrices, Int32 useVertexWeigths, System.Double vertexWeigthOffset, System.Double vertexWeigthScale, System.Double boundaryWeight, System.Double normalWeight, System.Double uvWeight, System.Double sharpNormalWeight, System.Double uvSeamWeight, Int32 forbidUVFoldovers, Int32 useColorsAsWeights);
		/// <summary>
		/// Reduces the polygon count by choosing wisely vertices to remove until target triangle count ratio is reached
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="ratio">Target ratio (between 0 (0%, everything is removed) and 1 (100%, nothing is removed)</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		/// <param name="useVertexWeigths">Use vertex importance weights if any. Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="vertexWeigthOffset">Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="vertexWeigthScale">Vertex Weights are computed from color: W = offset + (R-B) * scale</param>
		/// <param name="boundaryWeight">Boundaries importance during the decimation (boundary edges are edges connected to one triangle only)</param>
		/// <param name="normalWeight">Normal importance during the decimation</param>
		/// <param name="uvWeight">UV importance during the decimation</param>
		/// <param name="sharpNormalWeight">Importance of sharp edges during the decimation</param>
		/// <param name="uvSeamWeight">Importance of UV seams during the decimation</param>
		/// <param name="forbidUVFoldovers">Forbid UV to fold over during the decimation</param>
		/// <param name="useColorsAsWeights">Use red channel of vertex colors as weigths for the decimation process</param>
		public static void DecimateToRatio(Polygonal.MeshList meshes, Core.Double ratio, Geom.Matrix4List matrices, Core.Bool useVertexWeigths, Core.Double vertexWeigthOffset, Core.Double vertexWeigthScale, Core.Double boundaryWeight, Core.Double normalWeight, Core.Double uvWeight, Core.Double sharpNormalWeight, Core.Double uvSeamWeight, Core.Bool forbidUVFoldovers, Core.Bool useColorsAsWeights) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_decimateToRatio(meshes_c, ratio, matrices_c, useVertexWeigths ? 1 : 0, vertexWeigthOffset, vertexWeigthScale, boundaryWeight, normalWeight, uvWeight, sharpNormalWeight, uvSeamWeight, forbidUVFoldovers ? 1 : 0, useColorsAsWeights ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_destroy(Polygonal.MeshList_c meshes);
		/// <summary>
		/// Destroys meshes to free memory
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		public static void Destroy(Polygonal.MeshList meshes) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_destroy(meshes_c);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_exportMeshes(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices, string filePath);
		/// <summary>
		/// Export a list of meshes with transform into a file (Pxz format)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices (if empty Identity is used for all meshes)</param>
		/// <param name="filePath">Output file path</param>
		public static void ExportMeshes(Polygonal.MeshList meshes, Geom.Matrix4List matrices, Core.String filePath) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_exportMeshes(meshes_c, matrices_c, filePath);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_removeHoles(Polygonal.MeshList_c meshes, Int32 throughHoles, Int32 blindHoles, Int32 surfacicHoles, System.Double maxDiameter);
		/// <summary>
		/// Removes holes in meshes
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="throughHoles">Remove through holes</param>
		/// <param name="blindHoles">Remove blind holes</param>
		/// <param name="surfacicHoles">Remove surfacic holes</param>
		/// <param name="maxDiameter">Maximum diameter of the holes to be removed (use -1 for no max diameter)</param>
		public static void RemoveHoles(Polygonal.MeshList meshes, Core.Bool throughHoles, Core.Bool blindHoles, Core.Bool surfacicHoles, Core.Double maxDiameter) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_removeHoles(meshes_c, throughHoles ? 1 : 0, blindHoles ? 1 : 0, surfacicHoles ? 1 : 0, maxDiameter);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_clear();
		/// <summary>
		/// Clear all the current session (all unsaved work will be lost)
		/// </summary>
		public static void Clear() {
			OptimizeSDK_clear();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_saveAsPXZ(string fileName);
		/// <summary>
		/// Save no script
		/// </summary>
		/// <param name="fileName">Path to save the file</param>
		public static void SaveAsPXZ(Core.String fileName) {
			OptimizeSDK_saveAsPXZ(fileName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region Pivots

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_movePivotPointToCenter(Polygonal.MeshList_c meshes);
		/// <summary>
		/// Move the pivot point to the center of the mesh
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		public static void MovePivotPointToCenter(Polygonal.MeshList meshes) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_movePivotPointToCenter(meshes_c);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_setPivot(Polygonal.MeshList_c meshes, Geom.Matrix4_c pivot);
		/// <summary>
		/// Set the pivot point to the given matrice
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="pivot">New Pivot</param>
		public static void SetPivot(Polygonal.MeshList meshes, Geom.Matrix4 pivot) {
			var meshes_c = ConvertValue(meshes);
			var pivot_c = ConvertValue(pivot);
			OptimizeSDK_setPivot(meshes_c, pivot_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4_free(ref pivot_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region Remove Hidden

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.BoolList_c OptimizeSDK_removeHidden(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices, Int32 selectionLevel, Int32 considerTransparentOpaque, Int32 adjacencyDepth, Int32 cameraResolution);
		/// <summary>
		/// Delete occluded polygons from a sphere around the given meshes
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices (if empty Identity is used for all meshes)</param>
		/// <param name="selectionLevel">Level of parts to remove : Parts, Patches or Polygons</param>
		/// <param name="considerTransparentOpaque">If true, transparent materials will be considered opaque, meaning that geometry behind transparent materials could be considered as 'unseen' and removed</param>
		/// <param name="adjacencyDepth">Mark the N-Ring of a visible polygon as visible</param>
		/// <param name="cameraResolution">Resolution of tests camera</param>
		public static Core.BoolList RemoveHidden(Polygonal.MeshList meshes, Geom.Matrix4List matrices, Algo.SelectionLevel selectionLevel, Core.Bool considerTransparentOpaque, Core.Int adjacencyDepth, Core.Int cameraResolution) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_removeHidden(meshes_c, matrices_c, (int)selectionLevel, considerTransparentOpaque ? 1 : 0, adjacencyDepth, cameraResolution);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_BoolList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.BoolList_c OptimizeSDK_removeHiddenAdvanced(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices, Int32 selectionLevel, System.Double featureSize, System.Double minimumCavityVolume, Int32 considerTransparentOpaque, Int32 adjacencyDepth, Int32 cameraResolution);
		/// <summary>
		/// Deleted occluded polygons from automatically placed viewpoints
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices (if empty Identity is used for all meshes)</param>
		/// <param name="selectionLevel">Level of parts to remove : Parts, Patches or Polygons</param>
		/// <param name="featureSize">Size of the voxels used to automatically place viewpoints, the smaller it is, more viewpoints are generated. This parameter also acts as a seam for watertight detection of volumes</param>
		/// <param name="minimumCavityVolume">Minimum volume of a cavity in cubic meter (smaller it is, more viewpoints there are)</param>
		/// <param name="considerTransparentOpaque">If true, transparent materials will be considered opaque, meaning that geometry behind transparent materials could be considered as 'unseen' and removed</param>
		/// <param name="adjacencyDepth">Mark the N-Ring of a visible polygon as visible</param>
		/// <param name="cameraResolution">Resolution of tests camera</param>
		public static Core.BoolList RemoveHiddenAdvanced(Polygonal.MeshList meshes, Geom.Matrix4List matrices, Algo.SelectionLevel selectionLevel, Core.Double featureSize, Core.Double minimumCavityVolume, Core.Bool considerTransparentOpaque, Core.Int adjacencyDepth, Core.Int cameraResolution) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_removeHiddenAdvanced(meshes_c, matrices_c, (int)selectionLevel, featureSize, minimumCavityVolume, considerTransparentOpaque ? 1 : 0, adjacencyDepth, cameraResolution);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_BoolList_free(ref ret);
			return convRet;
		}

		#endregion

		#region Retopology

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_createOcclusionMesh(Polygonal.MeshList_c meshes, Int32 type, System.Double featureSize, Int32 gap, Geom.Matrix4List_c matrices, Int32 transfertAnimations);
		/// <summary>
		/// Computes an occluder or an occludee with the occurrences selected
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="type">Type of what we create</param>
		/// <param name="featureSize">Size of voxels</param>
		/// <param name="gap">Dilation iterations on the voxel grid</param>
		/// <param name="matrices">List of matrices (if empty, identity matrices are used)</param>
		/// <param name="transfertAnimations">Transfert joint assignation on the resulting mesh to keep animations</param>
		public static Core.Ident CreateOcclusionMesh(Polygonal.MeshList meshes, CreateOccluder type, Core.Double featureSize, Core.Int gap, Geom.Matrix4List matrices, Core.Bool transfertAnimations) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_createOcclusionMesh(meshes_c, (int)type, featureSize, gap, matrices_c, transfertAnimations ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Polygonal.MeshList_c OptimizeSDK_decomposeConvex(System.UInt32 mesh, Int32 maxCount, Int32 vertexCount, Int32 resolution, System.Double concavity);
		/// <summary>
		/// Explode mesh to approximated convex decomposition
		/// </summary>
		/// <param name="mesh">Input meshes</param>
		/// <param name="maxCount">Maximum number of convex hull to generate</param>
		/// <param name="vertexCount">Maximum number of vertices per convex hull</param>
		/// <param name="resolution">The maximum number of voxels generated during the voxelization stage</param>
		/// <param name="concavity">The maximum concavity</param>
		public static Polygonal.MeshList DecomposeConvex(Core.Ident mesh, Core.Int maxCount, Core.Int vertexCount, Core.Int resolution, Core.Double concavity) {
			var ret = OptimizeSDK_decomposeConvex(mesh, maxCount, vertexCount, resolution, concavity);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Polygonal_MeshList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_remesh(Polygonal.MeshList_c meshes, System.Double featureSize, Int32 isPointCloud, Geom.Matrix4List_c matrices, Int32 transfertAnimations);
		/// <summary>
		/// Remeshes the given meshes using projected marching cube representation skin
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="featureSize">Minimal feature size</param>
		/// <param name="isPointCloud">Is the mesh a point cloud</param>
		/// <param name="matrices">List of matrices (if empty, identity matrices are used)</param>
		/// <param name="transfertAnimations">Transfert joint assignation on the resulting mesh to keep animations</param>
		public static Core.Ident Remesh(Polygonal.MeshList meshes, Core.Double featureSize, Core.Bool isPointCloud, Geom.Matrix4List matrices, Core.Bool transfertAnimations) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_remesh(meshes_c, featureSize, isPointCloud ? 1 : 0, matrices_c, transfertAnimations ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_remeshFieldAligned(Polygonal.MeshList_c meshes, Int32 targetCount, Geom.Matrix4List_c matrices, Int32 fullQuad, System.Double featureSize, Int32 transfertAnimations);
		/// <summary>
		/// Remeshes the given meshes with a mesh aligned on features (this method is driven by a target triangle count and is quad dominant)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="targetCount">Approximate triangle count of the result</param>
		/// <param name="matrices">List of matrices (if empty, identity matrices are used)</param>
		/// <param name="fullQuad"></param>
		/// <param name="featureSize">Minimal feature size</param>
		/// <param name="transfertAnimations">Transfert joint assignation on the resulting mesh to keep animations</param>
		public static Core.Ident RemeshFieldAligned(Polygonal.MeshList meshes, Core.Int targetCount, Geom.Matrix4List matrices, Core.Bool fullQuad, Core.Double featureSize, Core.Bool transfertAnimations) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_remeshFieldAligned(meshes_c, targetCount, matrices_c, fullQuad ? 1 : 0, featureSize, transfertAnimations ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_remeshFieldAlignedToRatio(Polygonal.MeshList_c meshes, System.Double ratio, Geom.Matrix4List_c matrices, Int32 fullQuad, System.Double featureSize, Int32 transfertAnimations);
		/// <summary>
		/// Remeshes the given meshes with a mesh aligned on features (this method is driven by a target ratio and is quad dominant)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="ratio">Target ratio (between 0 and 1)</param>
		/// <param name="matrices">List of matrices (if empty, identity matrices are used)</param>
		/// <param name="fullQuad"></param>
		/// <param name="featureSize">Minimal feature size</param>
		/// <param name="transfertAnimations">Transfert joint assignation on the resulting mesh to keep animations</param>
		public static Core.Ident RemeshFieldAlignedToRatio(Polygonal.MeshList meshes, Core.Double ratio, Geom.Matrix4List matrices, Core.Bool fullQuad, Core.Double featureSize, Core.Bool transfertAnimations) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_remeshFieldAlignedToRatio(meshes_c, ratio, matrices_c, fullQuad ? 1 : 0, featureSize, transfertAnimations ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		#endregion

		#region System

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_restartEventManager();
		/// <summary>
		/// 
		/// </summary>
		public static void RestartEventManager() {
			OptimizeSDK_restartEventManager();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_addCustomProperty(System.UInt32 entity, string name, string value);
		/// <summary>
		/// Add a custom property to an entity that support custom properties
		/// </summary>
		/// <param name="entity">An entity that support custom properties</param>
		/// <param name="name">Name of the custom property</param>
		/// <param name="value">Value of the custom property</param>
		public static void AddCustomProperty(Core.Ident entity, Core.String name, Core.String value) {
			OptimizeSDK_addCustomProperty(entity, name, value);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct checkForUpdatesReturn_c
		{
			internal Int32 newVersionAvailable;
			internal IntPtr newVersion;
			internal IntPtr newVersionLink;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern checkForUpdatesReturn_c OptimizeSDK_checkForUpdates();
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
			var ret = OptimizeSDK_checkForUpdates();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			checkForUpdatesReturn retStruct = new checkForUpdatesReturn();
			retStruct.newVersionAvailable = ConvertValue(ret.newVersionAvailable);
			retStruct.newVersion = ConvertValue(ret.newVersion);
			retStruct.newVersionLink = ConvertValue(ret.newVersionLink);
			return retStruct;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern System.UInt32 OptimizeSDK_cloneEntity(System.UInt32 entity);
		/// <summary>
		/// Clone an entity
		/// </summary>
		/// <param name="entity">The entity to clone</param>
		public static Core.Ident CloneEntity(Core.Ident entity) {
			var ret = OptimizeSDK_cloneEntity(entity);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Ident)ret;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_getModuleProperty(string module, string propertyName);
		/// <summary>
		/// Returns the value of a module property
		/// </summary>
		/// <param name="module">Name of the module</param>
		/// <param name="propertyName">The property name</param>
		public static Core.String GetModuleProperty(Core.String module, Core.String propertyName) {
			var ret = OptimizeSDK_getModuleProperty(module, propertyName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_getPixyzWebsiteURL();
		/// <summary>
		/// get the Pixyz website URL
		/// </summary>
		public static Core.String GetPixyzWebsiteURL() {
			var ret = OptimizeSDK_getPixyzWebsiteURL();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_getProductDocumentationURL();
		/// <summary>
		/// get the product documentation URL
		/// </summary>
		public static Core.String GetProductDocumentationURL() {
			var ret = OptimizeSDK_getProductDocumentationURL();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Core.StringList_c OptimizeSDK_getProperties(Core.EntityList_c entities, string propertyName, string defaultValue);
		/// <summary>
		/// Get the property value on entities (if the property is not set on an entity, defaultValue is returned)
		/// </summary>
		/// <param name="entities">List of entities</param>
		/// <param name="propertyName">The property name</param>
		/// <param name="defaultValue">Default value to return if the property does not exist on an entity</param>
		public static Core.StringList GetProperties(Core.EntityList entities, Core.String propertyName, Core.String defaultValue) {
			var entities_c = ConvertValue(entities);
			var ret = OptimizeSDK_getProperties(entities_c, propertyName, defaultValue);
			Core_EntityList_free(ref entities_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Core_StringList_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_getProperty(System.UInt32 entity, string propertyName);
		/// <summary>
		/// Get a property value as String on an entity (error if the property does not exist on the entity)
		/// </summary>
		/// <param name="entity">The entity</param>
		/// <param name="propertyName">The property name</param>
		public static Core.String GetProperty(Core.Ident entity, Core.String propertyName) {
			var ret = OptimizeSDK_getProperty(entity, propertyName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_getVersion();
		/// <summary>
		/// get the Pixyz product version
		/// </summary>
		public static Core.String GetVersion() {
			var ret = OptimizeSDK_getVersion();
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Int32 OptimizeSDK_hasProperty(System.UInt32 entity, string propertyName);
		/// <summary>
		/// Return true if the property was found on the occurrence, will not throw any exception except if the entity does not exist.
		/// </summary>
		/// <param name="entity">An entity that support properties</param>
		/// <param name="propertyName">Name of the property</param>
		public static Core.Bool HasProperty(Core.Ident entity, Core.String propertyName) {
			var ret = OptimizeSDK_hasProperty(entity, propertyName);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return (Core.Bool)(0 != ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_pushAnalytic(string name, string data);
		/// <summary>
		/// push custom analytic event (Only for authorized products)
		/// </summary>
		/// <param name="name">Analytic event name</param>
		/// <param name="data">Analytic event data</param>
		public static void PushAnalytic(Core.String name, Core.String data) {
			OptimizeSDK_pushAnalytic(name, data);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_setModuleProperty(string module, string propertyName, string propertyValue);
		/// <summary>
		/// Set the value of a module property
		/// </summary>
		/// <param name="module">Name of the module</param>
		/// <param name="propertyName">The property name</param>
		/// <param name="propertyValue">The property value</param>
		public static Core.String SetModuleProperty(Core.String module, Core.String propertyName, Core.String propertyValue) {
			var ret = OptimizeSDK_setModuleProperty(module, propertyName, propertyValue);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern IntPtr OptimizeSDK_setProperty(System.UInt32 entity, string propertyName, string propertyValue);
		/// <summary>
		/// Set a property value on an entity
		/// </summary>
		/// <param name="entity">The entity</param>
		/// <param name="propertyName">The property name</param>
		/// <param name="propertyValue">The property value</param>
		public static Core.String SetProperty(Core.Ident entity, Core.String propertyName, Core.String propertyValue) {
			var ret = OptimizeSDK_setProperty(entity, propertyName, propertyValue);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			return ConvertValue(ret);
		}

		#endregion

		#region UV

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_copyUVs(Polygonal.MeshList_c meshes, Int32 sourceChannel, Int32 destinationChannel);
		/// <summary>
		/// Copy an UV channel to another UV channel
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="sourceChannel">The source UV channel to copy</param>
		/// <param name="destinationChannel">The destination UV channel to copy into</param>
		public static void CopyUVs(Polygonal.MeshList meshes, Core.Int sourceChannel, Core.Int destinationChannel) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_copyUVs(meshes_c, sourceChannel, destinationChannel);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_createProjectedUVs(Polygonal.MeshList_c meshes, Int32 useLocalAABB, System.Double uv3dSize, Int32 channel, Int32 overrideExistingUVs, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Creates UVs using axis aligned bounding box projections
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="useLocalAABB">If enabled, uses part own bounding box, else use global one</param>
		/// <param name="uv3dSize">3D size of the UV space [0-1]</param>
		/// <param name="channel">The UV channel which will contains the texture coordinates</param>
		/// <param name="overrideExistingUVs">If True, override existing UVs on channel</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void CreateProjectedUVs(Polygonal.MeshList meshes, Core.Bool useLocalAABB, Core.Double uv3dSize, Core.Int channel, Core.Bool overrideExistingUVs, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_createProjectedUVs(meshes_c, useLocalAABB ? 1 : 0, uv3dSize, channel, overrideExistingUVs ? 1 : 0, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_normalizeUVs(Polygonal.MeshList_c meshes, Int32 sourceUVChannel, Int32 destinationUVChannel, Int32 uniform, Int32 sharedUVSpace, Int32 ignoreNullIslands);
		/// <summary>
		/// Normalize UVs to fit in the [0-1] uv space
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="sourceUVChannel">UV Channel to normalize</param>
		/// <param name="destinationUVChannel">UV channel to store the normalized UV (if -1, sourceUVChannel will be replaced)</param>
		/// <param name="uniform">If true, the scale will be uniform. Else UV can be deformed with a non-uniform scale</param>
		/// <param name="sharedUVSpace">If true, all parts will be processed as if they were merged to avoid overlapping of their UV coordinates</param>
		/// <param name="ignoreNullIslands">If true, islands with null height and width will be ignored and their UV coordinates will be set to [0,0] (Slower if enabled)</param>
		public static void NormalizeUVs(Polygonal.MeshList meshes, Core.Int sourceUVChannel, Core.Int destinationUVChannel, Core.Bool uniform, Core.Bool sharedUVSpace, Core.Bool ignoreNullIslands) {
			var meshes_c = ConvertValue(meshes);
			OptimizeSDK_normalizeUVs(meshes_c, sourceUVChannel, destinationUVChannel, uniform ? 1 : 0, sharedUVSpace ? 1 : 0, ignoreNullIslands ? 1 : 0);
			Polygonal_MeshList_free(ref meshes_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern void OptimizeSDK_repackUVs(Polygonal.MeshList_c meshes, Int32 channel, Int32 shareMap, Int32 resolution, Int32 padding, Int32 uniformRatio, Int32 iterations, Int32 removeOverlaps, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Pack existing UV (create atlas)
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="channel">The UV channel to repack</param>
		/// <param name="shareMap">If True, the UV of all given parts will be packed together</param>
		/// <param name="resolution">Resolution wanted for the final map</param>
		/// <param name="padding">Set the padding (in pixels) between UV islands</param>
		/// <param name="uniformRatio">If true, UV of different part will have the same ratio</param>
		/// <param name="iterations">Fitting iterations</param>
		/// <param name="removeOverlaps">Remove overlaps to avoid multiple triangles UVs to share the same pixel</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static void RepackUVs(Polygonal.MeshList meshes, Core.Int channel, Core.Bool shareMap, Core.Int resolution, Core.Int padding, Core.Bool uniformRatio, Core.Int iterations, Core.Bool removeOverlaps, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			OptimizeSDK_repackUVs(meshes_c, channel, shareMap ? 1 : 0, resolution, padding, uniformRatio ? 1 : 0, iterations, removeOverlaps ? 1 : 0, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
		}

		#endregion

		#region Utils

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Geom.AABB_c OptimizeSDK_getAABB(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Return the AABB contaning provided meshes instance
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static Geom.AABB GetAABB(Polygonal.MeshList meshes, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_getAABB(meshes_c, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_AABB_free(ref ret);
			return convRet;
		}

		[DllImport(PiXYZOptimizeSDK_dll)]
		private static extern Geom.OBB_c OptimizeSDK_getOBB(Polygonal.MeshList_c meshes, Geom.Matrix4List_c matrices);
		/// <summary>
		/// Return the AABB contaning provided meshes instance
		/// </summary>
		/// <param name="meshes">List of input meshes</param>
		/// <param name="matrices">List of matrices of input meshes (if empty Identity will be used)</param>
		public static Geom.OBB GetOBB(Polygonal.MeshList meshes, Geom.Matrix4List matrices) {
			var meshes_c = ConvertValue(meshes);
			var matrices_c = ConvertValue(matrices);
			var ret = OptimizeSDK_getOBB(meshes_c, matrices_c);
			Polygonal_MeshList_free(ref meshes_c);
			Geom_Matrix4List_free(ref matrices_c);
			System.String err = ConvertValue(OptimizeSDK_getLastError());
			if(!System.String.IsNullOrEmpty(err))
				throw new Exception(err);
			var convRet = ConvertValue(ret);
			Geom_OBB_free(ref ret);
			return convRet;
		}

		#endregion

	}
}
