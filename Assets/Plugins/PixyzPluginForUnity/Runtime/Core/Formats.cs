using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Pixyz.Plugin4Unity
{
    /// <summary>
    /// Static class for formats related information.
    /// </summary>
    public static class Formats
    {
        private static readonly Dictionary<string, string> _Formats = new Dictionary<string, string> {
            { "FBX files",                  "fbx" },
            { "IGES files",                 "igs,iges" },
            { "STEP files",                 "stp,step,stepz" },
            { "IFC files",                  "ifc" },
            { "U3D files",                  "u3d" },
            { "CATIA files",                "CATProduct,CATPart,cgr,CATShape" },
            { "SolidWorks files",           "sldasm,sldprt" },
            { "Creo files",                 "prt,asm,neu,xas,xpr,prt.*,asm.*,neu.*,xas.*,xpr.*" },
            { "SolidEdge",                  "asm,par,pwd,psm" },
            { "ACIS SAT files",             "sat,sab" },
            { "VDA-FS files",               "vda" },
            { "Rhino files",                "3dm" },
            { "3dxml files",                "3dxml" },
            { "VRML files",                 "wrl,vrml" },
            { "COLLADA files",              "dae" },
            { "Stereolithography files",    "stl" },
            { "JT files",                   "jt" },
            { "Inventor files",             "ipt,iam,ipj" },
            { "Parasolid files",            "x_t,x_b,p_t,p_b,xmt,xmt_txt,xmt_bin" },
            { "PLMXML files",               "plmxml" },
            { "OBJ files",                  "obj" },
            { "CSB files",                  "csb" },
            { "Alias files",                "wire" },
            { "Pdf files",                  "pdf" },
            { "Prc files",                  "prc" },
            { "3DS files",                  "3ds" },
            { "USD files",                  "usd,usdz,usda,usdc" },
            { "Navisworks files",           "rvm" },
#if !MACOS
            { "Sketchup files",             "skp" },
            { "Revit files",                "rvt,rfa" },
            { "AutoCAD files",              "dwg,dxf" },
#endif
            { "VRED files",                 "vpb" },
            { "Point Cloud",                "e57,pts,ptx,rcp" },
            { "gLTF",                       "gltf,glb" },
            { "Pixyz",                      "pxz" },
            { "Others",                     "model,session" }
        };

        /// <summary>
        /// Returns a dictionary of all supported formats, where key is file type and value is corresponding extensions.
        /// </summary>
        public static Dictionary<string, string> SupportedFormatsGrouped {
            get {
                return _Formats.ToDictionary(pair => pair.Key, pair => pair.Value);
            }
        }

        public static readonly HashSet<string> UnitySupportedFormats = new HashSet<string> { ".fbx", ".skp", ".obj", ".3ds", ".dwg", ".dae", ".dxf", ".pdf" };

        /// <summary>
        /// Returns true if the given file is supported, otherwise, returns false. Format is .*
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileSupported(string file, bool liveSync = false)
        {
            return Regex.IsMatch(file.ToLower(), SupportedFormatsRegex);
        }

        public static bool IsPXZ(string file)
        {
            return Path.GetExtension(file.ToLower()) == ".pxz";
        }

        readonly static char[] SPLT_SLASH = new[] { '/', '\\' };
        readonly static char[] SPLT_DOT = new[] { '.' };
        /// <summary>
        /// Returns of the extension of a given file path.
        /// Format is of type ".xxx" where xxx is lowercase.
        /// This function, unlike Path.GetExtention, handles versionned extensions such as ".prt.2". In such cases, it returns ".prt".
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetExtension2(string file)
        {
            var split = file.Split(SPLT_SLASH);
            var split2 = split[split.Length - 1].Split(SPLT_DOT);
            if (split2.Length == 1)
                return null;
            else {
                int v;
                bool isV = int.TryParse(split2[split2.Length - 1], out v);
                if (isV) {
                    return '.' + split2[split2.Length - 2].ToLower();
                } else {
                    return '.' + split2[split2.Length - 1].ToLower();
                }
            }
        }

        /// <summary>
        /// Returns a collection of all extensions formatted as : . + lowercase extension.
        /// </summary>
        public static string[] SupportedFormatsScriptedImporter {
            get {
                if (_SupportedFormatsScriptedImporter == null) {
                    HashSet<string> extensions = new HashSet<string>();
                    foreach (string subformats in _Formats.Values) {
                        var subformatsList = subformats.Split(',');
                        for (int i = 0; i < subformatsList.Length; i++) {
                            extensions.Add("." + subformatsList[i].ToLower());
                        }
                    }
                    foreach (var unitySupportedFormat in UnitySupportedFormats) {
                        extensions.Remove(unitySupportedFormat);
                    }
                    _SupportedFormatsScriptedImporter = extensions.ToArray();
                }
                return _SupportedFormatsScriptedImporter;
            }
        }
        private static string[] _SupportedFormatsScriptedImporter;

        /// <summary>
        /// Returns a collection of all extensions formatted in a Regex.
        /// </summary>
        public static string SupportedFormatsRegex {
            get {
                if (_SupportedFormatsRegex == null) {
                    StringBuilder strbldr = new StringBuilder();
                    foreach (string subformats in _Formats.Values) {
                        var subformatsList = subformats.Split(',');
                        for (int i = 0; i < subformatsList.Length; i++) {
                            strbldr.Append("|." + subformatsList[i].ToLower());
                        }
                    }
                    strbldr[0] = '(';
                    strbldr.Append(")$");
                    strbldr.Replace(".", @"\.");
                    strbldr.Replace("*", @".*");
                    _SupportedFormatsRegex = strbldr.ToString();
                }
                return _SupportedFormatsRegex;
            }
        }
        private static string _SupportedFormatsRegex;

        /// <summary>
        /// Return an array of all supported formats, specially formatted for FileBrowsers.
        /// </summary>
        public static string[] SupportedFormatsForFileBrowser {
            get {
                if (_SupportedFormatsForFileBrowser == null) {
                    _SupportedFormatsForFileBrowser = new string[(_Formats.Count + 1) * 2];
                    StringBuilder strbldr = new StringBuilder();
                    int i = 1;
                    foreach (KeyValuePair<string, string> pair in _Formats) {
                        _SupportedFormatsForFileBrowser[i * 2] = pair.Key;
                        _SupportedFormatsForFileBrowser[i * 2 + 1] = pair.Value;
                        strbldr.Append(_SupportedFormatsForFileBrowser[i * 2 + 1]);
                        strbldr.Append(",");
                        i++;
                    }
                    _SupportedFormatsForFileBrowser[0] = "All Pixyz files";
                    _SupportedFormatsForFileBrowser[1] = strbldr.ToString();
                }
                return _SupportedFormatsForFileBrowser;
            }
        }
        private static string[] _SupportedFormatsForFileBrowser;
    }
}
