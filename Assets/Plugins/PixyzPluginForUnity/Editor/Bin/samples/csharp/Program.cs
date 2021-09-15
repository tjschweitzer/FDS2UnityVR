using System;
using System.Threading;

using pxz = Pixyz.OptimizeSDK.Native;

namespace TestOptimizeSDK
{
    class Program
    {
        public const int MESH_GRID_SIZE = 10;
        public const int NB_THREADS = 10;

        static pxz.Core.Ident createMaterial(){
            pxz.Material.MaterialDefinition materialDefinition = new pxz.Material.MaterialDefinition();
            materialDefinition.name = "Red";

            pxz.Core.Color red = new pxz.Core.Color(){r=1};
            pxz.Core.Color green = new pxz.Core.Color(){g=1};

            pxz.Material.ColorOrTexture albedo = new pxz.Material.ColorOrTexture(){color=red, _type = pxz.Material.ColorOrTexture.Type.COLOR};

            pxz.Material.CoeffOrTexture ao = new pxz.Material.CoeffOrTexture(){coeff=1.0, _type = pxz.Material.CoeffOrTexture.Type.COEFF};

            pxz.Material.CoeffOrTexture metallic = new pxz.Material.CoeffOrTexture(){coeff=0.5, _type = pxz.Material.CoeffOrTexture.Type.COEFF};

            pxz.Material.CoeffOrTexture roughness = new pxz.Material.CoeffOrTexture(){coeff=0.5, _type = pxz.Material.CoeffOrTexture.Type.COEFF};

            pxz.Material.CoeffOrTexture opacity = new pxz.Material.CoeffOrTexture(){coeff=1.0, _type = pxz.Material.CoeffOrTexture.Type.COEFF};
            
            pxz.Material.ColorOrTexture normal = new pxz.Material.ColorOrTexture(){color = green, _type = pxz.Material.ColorOrTexture.Type.COLOR};

            materialDefinition.albedo = albedo;
            materialDefinition.ao = ao;
            materialDefinition.metallic = metallic;
            materialDefinition.roughness = roughness;
            materialDefinition.opacity = opacity;
            materialDefinition.normal = normal;

            return pxz.NativeInterface.CreateMaterial(materialDefinition);
        }

        static pxz.Polygonal.MeshDefinition createMesh(int count)
        {
            pxz.Core.Ident material = createMaterial();

            pxz.Polygonal.MeshDefinition meshDefinition = new pxz.Polygonal.MeshDefinition();

            meshDefinition.vertices = new pxz.Geom.Point3List(count*count);
            meshDefinition.normals = new pxz.Geom.Vector3List(count*count);
            meshDefinition.quadrangles = new pxz.Core.IntList((count - 1)*(count - 1)*4);
            meshDefinition.triangles = new pxz.Core.IntList(0);
            meshDefinition.dressedPolys = new pxz.Polygonal.DressedPolyList(count - 1);

            for (int i = 0; i < count; ++i) {
                if (i < count-1){
                    meshDefinition.dressedPolys.list[i].material = material;
                    meshDefinition.dressedPolys.list[i].firstTri = 0;
                    meshDefinition.dressedPolys.list[i].triCount = 0;
                    meshDefinition.dressedPolys.list[i].firstQuad = (count - 1)*i;
                    meshDefinition.dressedPolys.list[i].quadCount = (count - 1);
                    meshDefinition.dressedPolys.list[i].externalId = (uint)i+1;
                }
                
                for (int j = 0; j < count; ++j) {
                    meshDefinition.vertices.list[i*count + j].x = (Double)j;
                    meshDefinition.vertices.list[i*count + j].y = 1.0;
                    meshDefinition.vertices.list[i*count + j].z = (Double)i;
                    
                    meshDefinition.normals.list[i*count + j]._base.x = 0.0;
                    meshDefinition.normals.list[i*count + j]._base.y = 1.0;
                    meshDefinition.normals.list[i*count + j]._base.z = 0.0;

                    if (i > 0 && j >0) {
                        meshDefinition.quadrangles[((i - 1)*(count - 1) + (j - 1)) * 4]     = (i - 1) * count + (j - 1);
                        meshDefinition.quadrangles[((i - 1)*(count - 1) + (j - 1)) * 4 + 1] = i       * count + (j - 1);
                        meshDefinition.quadrangles[((i - 1)*(count - 1) + (j - 1)) * 4 + 2] = i       * count + j;
                        meshDefinition.quadrangles[((i - 1)*(count - 1) + (j - 1)) * 4 + 3] = (i - 1) * count + j;            
                    }
                }
            }

            return meshDefinition;
        }

        static void createMeshAndDecimate(int id, int count)
        {
            pxz.NativeInterface.SetPixyzMainThread();
            pxz.Polygonal.MeshDefinition meshDefinition = createMesh(count);

            Console.WriteLine("Old Mesh : \n\t tri "+ meshDefinition.triangles.length + "\n\t quad " + meshDefinition.quadrangles.length);

            pxz.Core.Ident mesh = pxz.NativeInterface.CreateMesh(meshDefinition); 
            pxz.Polygonal.MeshList list = new pxz.Polygonal.MeshList(new uint[]{mesh});
            pxz.NativeInterface.DecimateToQualityVertexRemoval(list, 2.0, -1, -1, -1, new pxz.Geom.Matrix4List(0));
            pxz.Polygonal.MeshDefinition decimatedMesh = pxz.NativeInterface.GetMesh(mesh);

            Console.WriteLine("New Mesh : \n\t tri "+ decimatedMesh.triangles.length + "\n\t quad " + decimatedMesh.quadrangles.length);
        }

        static void Main(string[] args)
        {
            pxz.NativeInterface.Initialize("UnityOptimizeSDK", "2053b013182c0e02f63cbaab1edf1cec393e0a210c4824203f5207224f04391d06c744cca05d0ab7ca", "", "");

            pxz.NativeInterface.ConfigureInterfaceLogger(true, false, true);

            bool mt = true;

            if(mt){
                Thread[] tArray = new Thread[NB_THREADS];
                for(int i=0; i< NB_THREADS; ++i){
                    int count = MESH_GRID_SIZE*(i+1);
                    tArray[i] = new Thread(() => createMeshAndDecimate(i, count));
                    tArray[i].Start();
                }

                for(int i=0; i< NB_THREADS; ++i){
                    tArray[i].Join();
                }

            }else{
                createMeshAndDecimate(0, MESH_GRID_SIZE*10);
            }

            

            //pxz.NativeInterface.SaveAsPXZ("/home/thomas/test.pxz");

            pxz.NativeInterface.Clear();
        }
    }
}
