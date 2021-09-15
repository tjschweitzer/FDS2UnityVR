using UnityEngine;
using UnityEditor;
using Pixyz.OptimizeSDK.Native;

namespace Pixyz.OptimizeSDK.Editor
{
    public class LicenseManagerWindow : EditorWindow
    {
        [SerializeField]
        private Native.Core.LicenseInfos _licenseInfos = null;

        [SerializeField]
        private bool _licenseStatus = false;

        //[MenuItem("Pixyz/License Manager", false, 120)]
        //public static void Open()
        //{
        //    var window = GetWindow<LicenseManagerWindow>();
        //    window.titleContent = new GUIContent("License Manager");
        //    window.Focus();
        //    window.Repaint();
        //}

        public void OnGUI()
        {
            EditorGUILayout.Space();

            DisplaySeparator("License Infos", false);
            
            DisplayLicenseInfo();

            EditorGUILayout.Space();

            DisplaySeparator("License Actions");

            DisplayLicenseAction();
        }

        public void OnEnable()
        {
            _licenseStatus = CheckLicence();
            try
            {
                RefreshLicenseInfo();
            }
            catch(System.Exception)
            {
                _licenseStatus = false;
            }
        }

        private bool CheckLicence()
        {
            return NativeInterface.CheckLicense();
        }

        private void RefreshLicenseInfo()
        {
            _licenseInfos = NativeInterface.GetCurrentLicenseInfos();
        }

        private void DisplayLicenseAction()
        {
            if(GUILayout.Button("Generate Activation Code"))
            {
                string path = EditorUtility.SaveFilePanel("Save activation code", "", "activation_code", "bin");
                NativeInterface.GenerateActivationCode(path);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Install License"))
            {
                string path = EditorUtility.OpenFilePanelWithFilters("Open license to install", "", new string[] { "FileType", "lic" });
                try
                {
                    //NativeInterface.Initialize("UnityOptimizeSDK", NativeInterface.privateKey, "");
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                }
                NativeInterface.InstallLicense(path);

                if(CheckLicence())
                {
                    RefreshLicenseInfo();
                    _licenseStatus = true;
                }
            }
        }
        private void DisplayLicenseInfo()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Owner :", GUILayout.Width(100.0f));
            if(_licenseInfos != null)
                EditorGUILayout.LabelField(_licenseInfos.customerName);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Company :", GUILayout.Width(100.0f));
            if (_licenseInfos != null)
                EditorGUILayout.LabelField(_licenseInfos.customerCompany);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Email :", GUILayout.Width(100.0f));
            if (_licenseInfos != null)
                EditorGUILayout.LabelField(_licenseInfos.customerEmail);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Generated :", GUILayout.Width(100.0f));
            if (_licenseInfos != null)
                EditorGUILayout.LabelField($"{_licenseInfos.startDate.day}/{_licenseInfos.startDate.month}/{_licenseInfos.startDate.year}");

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Expire :", GUILayout.Width(100.0f));
            if (_licenseInfos != null)
                EditorGUILayout.LabelField($"{_licenseInfos.endDate.day}/{_licenseInfos.endDate.month}/{_licenseInfos.endDate.year} ");

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Status :", GUILayout.Width(100.0f));
            EditorGUILayout.LabelField(_licenseStatus ? "Activated" : "Missing/Not active");

            EditorGUILayout.EndHorizontal();
        }
        private void DisplaySeparator(string separatorName, bool indent = false)
        {
            EditorGUILayout.BeginHorizontal();

            Vector2 labelSize = EditorStyles.boldLabel.CalcSize(new GUIContent(separatorName));
            EditorGUILayout.LabelField(separatorName, EditorStyles.boldLabel, GUILayout.Width(labelSize.x + (indent ? 15.0f : 0.0f)));

            Rect rLast = GUILayoutUtility.GetLastRect();

            EditorGUILayout.BeginVertical();

            GUILayout.Space(15);

            Rect r = GUILayoutUtility.GetRect(EditorGUIUtility.currentViewWidth - (rLast.width + rLast.x), 1.0f);
            EditorGUI.DrawRect(r, Color.grey);

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }
    }
}