using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Pixyz.OptimizeSDK.Editor.MeshProcessing;
using Pixyz.Commons.Extensions.Editor;

namespace Pixyz.OptimizeSDK.Editor.Windows
{
    public class ExampleWindow : EditorWindow
    {
        private MeshData _data = null;
        private List<GameObject> _gos = null;
        private PixyzContext _context = null;

        public static void Start(GameObject[] gos)
        {
            ExampleWindow window = GetWindow<ExampleWindow>();
            window.Init(new List<GameObject>(gos));
        }

        private void Init(List<GameObject> gos)
        {
            _gos = gos;
            _context = new PixyzContext();
            PreProcess();
#pragma warning disable CS4014
            Generation();
#pragma warning restore CS4014
        }

        private void OnDestroy()
        {
            if (_context != null)
                _context.Dispose();
        }

        /// <summary>
        /// Collect all source data (Renderers, meshs, materials)
        /// </summary>
        private void PreProcess()
        {
            _data = new MeshData();

            foreach (GameObject go in _gos)
            {
                if (go.GetMeshData(out Renderer renderer, out Mesh mesh, out Material[] materials))
                {
                    _data.renderers.Add(renderer);
                    _data.meshes.Add(mesh);
                    _data.materials.Add(materials);
                }
            }
        }

        /// <summary>
        /// Start to generate pixyz data structure ans starts optimization process
        /// </summary>
        /// <returns></returns>
        private async Task Generation()
        {
            try
            {
                //Generate pixyz data structure and return id of pixyz meshs created, need to be synchrone for this version but this will be improved to have the pixyz side async
                _context.UnityMeshesToPixyzCreate(_data.renderers, _data.meshes, _data.materials);

                //Optimize the mesh, for this example we use a RepairMesh and a DecimateTarget to 5k triangles, full async
                await Task.Factory.StartNew(Process);

                //Rebuild unity assets from pixyz meshs after they've been optimized
                Completed();
            }
            catch(System.Exception e)
            {
                Debug.LogError($"[OptiError] Error : {e.Message} \n {e.StackTrace}");
            }
        }

        private void Process()
        {
            try
            {
                //Tell pixyz that the main thread has changed
                Native.NativeInterface.SetPixyzMainThread();

                //Clean the mesh of possible issues, no orient checks because game assets are usually well oriented
                Native.NativeInterface.RepairMeshes(_context.pixyzMeshes, 0.1f, true, false, _context.pixyzMatrices);

                //Once the mesh is clean we decimate to the desired triangle count, here 5k (The triangle count is a global count for all meshs pass to the function)
                // So if we have : 
                // meshA = 2k,
                // meshB = 4k,
                // meshC = 15k
                // the decimated result will be as this : finalTriangleCount = meshA.triangleCount + meshB.triangleCount + meshC.triangleCount = 5k
                Native.NativeInterface.DecimateToPolycount(_context.pixyzMeshes, 15000, _context.pixyzMatrices, false, 0.0, 1.0, 100, 100, 100, 100, 100, true, false);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Error] {e.Message} /n {e.StackTrace}");
            }
        }

        /// <summary>
        /// Save generated assets as standalone assets
        /// </summary>
        private void Completed()
        {
            GameObject[] gos = _context.PixyzMeshToUnityObject(_context.pixyzMeshes);

            if (!AssetDatabase.IsValidFolder("Assets/PixyzGeneration"))
            {
                AssetDatabase.CreateFolder("Assets", "PixyzGeneration");
            }

            if (!AssetDatabase.IsValidFolder("Assets/PixyzGeneration/Textures"))
            {
                AssetDatabase.CreateFolder("Assets/PixyzGeneration", "Textures");
            }
            if (!AssetDatabase.IsValidFolder("Assets/PixyzGeneration/Materials"))
            {
                AssetDatabase.CreateFolder("Assets/PixyzGeneration", "Materials");
            }
            if (!AssetDatabase.IsValidFolder("Assets/PixyzGeneration/Meshs"))
            {
                AssetDatabase.CreateFolder("Assets/PixyzGeneration", "Meshs");
            }

            AssetDatabase.StartAssetEditing();

            foreach (Texture2D tex in _context.GeneratedTextures)
            {
                string path = AssetDatabase.GenerateUniqueAssetPath("Assets/PixyzGeneration/Textures/pixyz_texture_generated.asset");

                AssetDatabase.CreateAsset(tex, path);
                //byte[] bytes = ImageConversion.EncodeToPNG(tex);
                //System.IO.File.WriteAllBytes(path +".png", bytes);
            }

            foreach (Material mat in _context.GeneratedMaterials)
            {
                string path = AssetDatabase.GenerateUniqueAssetPath("Assets/PixyzGeneration/Materials/pixyz_material_generated.mat");

                AssetDatabase.CreateAsset(mat, path);
            }

            foreach (Mesh mesh in _context.GeneratedMesh)
            {
                string path = AssetDatabase.GenerateUniqueAssetPath("Assets/PixyzGeneration/Meshs/pixyz_mesh_generated.asset");

                AssetDatabase.CreateAsset(mesh, path);
            }

            AssetDatabase.StopAssetEditing();
            
            //Dispose destroys all assets generated pixyz side, all ids past this point will no longer be valid
            _context.Dispose();
            _context = null;

            Close();
        }
    }
}