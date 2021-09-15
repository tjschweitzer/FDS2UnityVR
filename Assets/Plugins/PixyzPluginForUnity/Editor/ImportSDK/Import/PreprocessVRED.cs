using UnityEditor;
using UnityEngine;
using Pixyz.ImportSDK.Native;
using Pixyz.Commons.UI.Editor;
using Pixyz.Commons.Utilities;
using Pixyz.Plugin4Unity.Core;
using System.Collections.Generic;
using System;

namespace Pixyz.ImportSDK {

    public class PreprocessVRED : SubProcess {

        [InitializeOnLoadMethod]
        private static void Register() {

            // Preprocess
            var preprocess = new PreprocessVRED();

            //preprocess.outputDirectory = "Assets/" + Preferences.PrefabFolder;
            preprocess.initialize();
            Importer.AddOrSetPreprocess(".vpb", preprocess);

            // Settings Template
            var template = ImportSettingsTemplate.Default;
            template.useMaterialsInResources.defaultValue = true;
            template.loadMetadata.defaultValue = true;
            template.isZUp.defaultValue = false; // VRED is zup/lefthanded but false for animation merge
            template.isLeftHanded.defaultValue = false; // VRED is zup/lefthanded but false for animation merge
            template.importVariants.defaultValue = true;

            template.importVariants.status = ParameterAvailability.Available;
            template.scaleFactor.status = ParameterAvailability.Available;
            template.repairInstances.status = ParameterAvailability.Available;

            // Don't reorient VRED mesh
            template.repair.defaultValue = false;
            template.orient.defaultValue = false;

            // Hidden parameters
            template.loadMetadata.status = ParameterAvailability.Hidden;
            template.importPatchBorders.status = ParameterAvailability.Hidden;
            template.importLines.status = ParameterAvailability.Hidden;
            template.importPoints.status = ParameterAvailability.Hidden;
            template.isZUp.status = ParameterAvailability.Hidden;
            template.isLeftHanded.status = ParameterAvailability.Hidden;
            template.mergeFinalLevel.status = ParameterAvailability.Hidden;
            template.treeProcess.status = ParameterAvailability.Hidden; 
            template.splitTo16BytesIndex.status = ParameterAvailability.Hidden;
            template.orient.status = ParameterAvailability.Hidden;
            template.singularizeSymmetries.status = ParameterAvailability.Hidden;
            template.mapUV.status = ParameterAvailability.Hidden;
            
            Importer.AddOrSetTemplate(".vpb", template); 
        }

        public bool isIOFieldsVisibleC = true; 
        public bool isIOVisible() => isIOFieldsVisibleC;

        public bool isEnabledC = true; 
        public bool isEnabled() => isEnabledC;

        public bool isVisibleMode(int i) => i < 2;

        [UserParameter(displayName: "Prefer Pixyz mesh", tooltip: "Override original mesh with Pixyz tessellation. Will only work if NURBs data is present in the file. Warning: UVs and Vertex Colors will be lost.")]
        public bool preferPixyzMesh = false;
        [UserParameter(displayName: "Import Variants", tooltip: "Import transform and material variants. Import variant sets.")]
        public bool importVariants = true;
        [UserParameter(displayName: "Import Animations", tooltip: "Import animations. Animation file will be placed in Prefab Save Folder (Edit/Preferences...).")]
        public bool importAnimations = false;
        [UserParameter(displayName: "Smart Merging", tooltip: "Merge model while preserving variant groups and animations.")]
        public bool smartMerging = false;
        [UserParameter(displayName: "Material Mapping Table", tooltip: "Automatically rename materials (including variants) using a .csv material table. Remember to select 'Use Materials in Resources' to automatically assign them from your material library. A third UV size column can be added to automatically map UVs.")]
        [FilterParameter(new string[] { "CSV", "csv" })]
        public FilePath materialMappingTable = default(FilePath);

        public override int id { get { return 582792807;} }
        public override string menuPathRuleEngine { get { return null;} }
        public override string menuPathToolbox { get { return null;} }

        public override string name => "Import Settings (.vpb)";

        public override void run(Importer importer) 
        {            
            ImportSDK.Native.Core.Ident root = importer.root;

            // Get all external files
            System.String anim_fbx_file = ImportSDK.Native.NativeInterface.GetProperty(root, "PXZ_ANIM_FBX_FILE");
            System.String matVar_csv_file = ImportSDK.Native.NativeInterface.GetProperty(root, "PXZ_MATERIAL_CSV_FILE");
            System.String varSets_xml_file = ImportSDK.Native.NativeInterface.GetProperty(root, "PXZ_VARSETS_XML_FILE");

            // Needs to match tags from PiXYZIO/vpb/VpbFile.cpp
            System.String MATERIAL_SWITCH_TAG = "_PiXYZ_MATERIAL_SWITCH_TAG";
            System.String TRANSFORM_SWITCH_TAG = "_PiXYZ_TRANSFORM_SWITCH_TAG";
            System.String TRANSFORM_VARIANT_TAG = "_PiXYZ_TRANSFORM_VARIANT_TAG";
            System.String ANIMATION_TAG = "_PiXYZ_ANIMATION_TAG";
            System.String PXZ_MAT_POOL = "_PiXYZ_MATERIAL_POOL";

            // The rule engine table has to modify the names of the materials in the material library, the material variant file and in the variant sets file
            if (System.IO.Path.GetExtension(materialMappingTable) == "csv")
            {
                runMaterialRuleEngineFromCSV(materialMappingTable.ToString(), matVar_csv_file);
            }

            // Merge occurrences depending on either they belong to a transform switch and/or a material switch and/or an animation group.
            // (1) merge: all parts except material_variants parts or geometric_variants parts or animated parts
            // (2) merge: material_variants parts except if animated parts or parts of geometric variants (in this case merge them depending on that)
            // (3) merge: geometric_variants parts: except animated parts or material_variants parts in this case don't merge
            // (4) merge: animated parts: except geometric_variants parts or material_variants parts
            Native.Scene.OccurrenceList rootList = new Native.Scene.OccurrenceList(1);
            rootList[0] = root;
            if (smartMerging)
            {
                // Tag transform variants groups
                List<uint> transformVariants=new List<uint>();
                var transformSwitches = NativeInterface.FindByProperty(TRANSFORM_SWITCH_TAG, "", rootList);
                for (int i = 0; i < transformSwitches.length; i++)
                {
                    // Variants are switches children
                    
                    var variants = NativeInterface.GetOccurrenceChildren(transformSwitches[i]);
                    for (int  j = 0; j < variants.length; j++)
                    {
                        transformVariants.Add(variants[j]);
                        // Tag variants for later cleaning
                        NativeInterface.AddCustomProperty(variants[j], TRANSFORM_VARIANT_TAG, "");
                    }
                }

                tagPartsFromOccurrences(transformSwitches, TRANSFORM_VARIANT_TAG, "");

               List<string> properties= new List<string>();
                properties.Add(TRANSFORM_VARIANT_TAG);
                properties.Add(PXZ_MAT_POOL);
                properties.Add(MATERIAL_SWITCH_TAG);

                // Tag animated groups
                if (importAnimations)
                {
                    var animatedOccurrences = NativeInterface.FindByProperty(ANIMATION_TAG, "", rootList);
                    tagPartsFromOccurrences(animatedOccurrences, ANIMATION_TAG, "");
                    properties.Add(ANIMATION_TAG);
                }

                // Merge all parts not having any property to preserve
                var exceptedOccurrences = mergeButProperties(properties);

                for (int i = 0; i < properties.Count; i++)
                {
                    if (properties[i] == PXZ_MAT_POOL)
                    {
                        properties.RemoveAt(i);
                    }
                }

                // Merge all parts having same properties/values
                mergeByProperties(exceptedOccurrences, properties);
            }
        }

        Dictionary<string, string> runMaterialRuleEngineFromCSV(string ruleEnginePath, string variantsFile)
        {
            // Read csv and create a map
            Dictionary<string, string> materialMap = new Dictionary<string, string>();
            Dictionary<string, string> uvMap = new Dictionary<string, string>();

            string[] ruleEngineLines = System.IO.File.ReadAllLines(@ruleEnginePath);
            foreach (string line in ruleEngineLines)
            {
                var temp = line.Split(',');
                if (temp.Length >= 2)
                    materialMap[temp[0]] = temp[1];
                if (temp.Length >= 3)
                    uvMap[temp[0]] = temp[2];
            }

            string[] variantLines = System.IO.File.ReadAllLines(@variantsFile);
            string content = "";
            foreach (string line in variantLines)
            {
                var temp = line.Split(';');
                for(int index=0;index < temp.Length;index++)
                {
                    if(materialMap.ContainsKey(temp[index]))
                    {
                        content += materialMap[temp[index]] + ";";
                    }
                    else
                    {
                        content += temp[index] + ";";
                    }
                    content = content.Substring(0, content.Length - 1) + "\n";
                }
            }
            // Rename materials in variantsFile
            System.IO.File.WriteAllText(@variantsFile, content);

            Native.Material.MaterialList materials = NativeInterface.GetAllMaterials();

            Native.Scene.OccurrenceList rootList= new Native.Scene.OccurrenceList(1);
            rootList[0] = NativeInterface.GetRootOccurrence();

            for (int i = 0; i < materials.length; i++)
            {
                uint material = materials[i];
                string vredMaterialName = NativeInterface.GetProperty(material, "Name");
                if (materialMap.ContainsKey(vredMaterialName))
                {
                    // List occurrences with material and apply UVs
                    if (uvMap.ContainsKey(vredMaterialName))
                    {
                        var parts = NativeInterface.FindByActiveMaterial(material, rootList);
                        try
                        {
                            int uvSize = Int32.Parse(uvMap[vredMaterialName]);
                            NativeInterface.MapUvOnAABB(parts, false, uvSize, 0, true);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Wrong uvSize... " + e.Message);
                        }
                    }
                    NativeInterface.SetProperty(material, "Name", materialMap[vredMaterialName]);
                }
            }

            return materialMap;
        }

        void tagPartsFromOccurrences(Native.Scene.OccurrenceList occurrences, string prop, string val)
        {
            for (int i = 0; i < occurrences.length; i++)
            {
                var occurrence = occurrences[i];
                var parts = NativeInterface.GetPartOccurrences(occurrence);
                for (int j = 0; j < parts.length; j++)
                {
                    if (val == "")
                    {
                        try
                        {
                            string value = NativeInterface.GetProperty(parts[j], prop) + NativeInterface.GetProperty(occurrence, "Name");
                            NativeInterface.AddCustomProperty(parts[j], prop, value);
                        }
                        catch
                        {
                            NativeInterface.AddCustomProperty(parts[j], prop, NativeInterface.GetProperty(occurrence, "Name"));
                        }
                    }
                    else
                    {
                        NativeInterface.AddCustomProperty(parts[j], prop, val);
                    }

                }
            }
        }

        Native.Scene.OccurrenceList mergeButProperties(List<string> properties)
        {
            Native.Scene.OccurrenceList partOccurrences = NativeInterface.GetPartOccurrences(NativeInterface.GetRootOccurrence());
            List<uint> exceptedOccurrences = new List<uint>(); ;
            foreach(string prop in properties)
            {
                Native.Scene.OccurrenceList occurrences = NativeInterface.FindByProperty(prop, ".*", partOccurrences);
                for (int i = 0; i < occurrences.length; i++)
                {
                    var occurrence = occurrences[i];
                    if(!exceptedOccurrences.Contains(occurrence))
                    {
                        exceptedOccurrences.Add(occurrence);
                    }
                }
            }

            NativeInterface.MergePartsByMaterials(new Native.Scene.OccurrenceList(exceptedOccurrences.ToArray()), true, Native.Scene.MergeHiddenPartsMode.MergeSeparately);

            return new Native.Scene.OccurrenceList(exceptedOccurrences.ToArray());
        }

        void mergeByProperties(Native.Scene.OccurrenceList occurrences, List<string> properties)
        {
            Dictionary<string, List<uint>> sortingMap = new Dictionary<string, List<uint>>();

            foreach (uint occurrence in occurrences.list)
            {
                string code = "";

                var propertiesStr = NativeInterface.ListProperties(occurrence);
                List<string> propertiesList = new List<string>(propertiesStr.list);

                foreach (var prop in properties)
                {
                    if (propertiesList.Contains(prop))
                    {
                        //the part has a property from 'properties': add 'propertyCode' (== index in list) + 'value'  to code
                        //make code a string '1value1-2value2-...-NvalueN': it will be the identifier in sortingDict for all the parts having same properties/values

                        int index = properties.IndexOf(prop);//(int)std::distance(properties.begin(), std::find(properties.begin(), properties.end(), prop));
                        var value = NativeInterface.GetProperty(occurrence, prop);
                        code = code + "-" + index.ToString() + value.ToString() ;
                    }
                }
                sortingMap[code].Add(occurrence);
            }
            foreach (var elem in sortingMap)
            {
                if (elem.Key != "")
                {
                    NativeInterface.MergePartsByMaterials(new Native.Scene.OccurrenceList(elem.Value.ToArray()), true, Native.Scene.MergeHiddenPartsMode.MergeSeparately);
                }
            }
        }

        private string rffs(string path) {
            return path.Replace("\\", "/");
        }

        bool drawn = false;
        bool tmpImportVariants;
        bool tmpPreferPixyzMesh;
        public override void onBeforeDraw(ImportSettings importSettings) {
            EditorPrefs.SetString("Pixyz_VRED_VredPath", Preferences.VREDExecutable);

            if (!drawn) {
                importSettings.useMaterialsInResources = true;
                importSettings.isZUp = false;
                importSettings.isLeftHanded = false;
                drawn = true;

                tmpImportVariants = (ImportSDK.Native.NativeInterface.GetModuleProperty("IO", "LoadVariant")=="True");
                tmpPreferPixyzMesh = !(ImportSDK.Native.NativeInterface.GetModuleProperty("IO", "PreferLoadMesh") == "True");
            }

            if (preferPixyzMesh != tmpPreferPixyzMesh) {
                ImportSDK.Native.NativeInterface.SetModuleProperty("IO", "PreferLoadMesh", preferPixyzMesh ? "False" : "True");                
                tmpPreferPixyzMesh = preferPixyzMesh;
            }

            if (importVariants != tmpImportVariants) {
                ImportSDK.Native.NativeInterface.SetModuleProperty("IO", "LoadVariant", importVariants ? "True" : "False");
                tmpImportVariants = importVariants;
            }
        }
    }
}