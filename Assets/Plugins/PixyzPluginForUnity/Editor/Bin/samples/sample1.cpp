#ifdef BUILDING_PXZ
#include <PiXYZOptimizeSDK/interface/OptimizeSDKInterface.h>
#else
#include <OptimizeSDKInterface.h>
#endif
#include <iostream>

PXZ_USE_MODULE(OptimizeSDKI)
PXZ_USE_MODULE(CoreI)
PXZ_USE_MODULE(GeomI)
PXZ_USE_MODULE(PolygonalI)
PXZ_USE_MODULE(MaterialI)

#define TEST_COUNT 100
#define MESH_GRID_SIZE 3
//#define WITH_MATERIAL

Mesh createMesh()
{
#ifdef WITH_MATERIAL
   MaterialDefinition materialDefinition;
   materialDefinition.name = "Red";
   materialDefinition.albedo = ColorOrTexture(Color(1, 0, 0));
   materialDefinition.ao = CoeffOrTexture(1.0);
   materialDefinition.metallic = CoeffOrTexture(0.5);
   materialDefinition.roughness = CoeffOrTexture(0.5);
   materialDefinition.opacity = CoeffOrTexture(1.0);
   materialDefinition.normal = ColorOrTexture(Color(0, 1, 0));
   Material material = OptimizeSDKInterface::createMaterial(materialDefinition);
#else
   Material material = 0;
#endif

   MeshDefinition meshDefinition;
   meshDefinition.vertices = Point3List(MESH_GRID_SIZE*MESH_GRID_SIZE);
   meshDefinition.normals = Vector3List(MESH_GRID_SIZE*MESH_GRID_SIZE);
   meshDefinition.quadrangles = IntList((MESH_GRID_SIZE - 1)*(MESH_GRID_SIZE - 1)*4);
   meshDefinition.dressedPolys = DressedPolyList(MESH_GRID_SIZE - 1);
   
   for (int i = 0; i < MESH_GRID_SIZE; ++i) {
      if (i < MESH_GRID_SIZE-1)
         meshDefinition.dressedPolys[i] = DressedPoly(material, 0, 0, (MESH_GRID_SIZE - 1)*i, (MESH_GRID_SIZE - 1), i+1);
      for (int j = 0; j < MESH_GRID_SIZE; ++j) {
         meshDefinition.vertices[i*MESH_GRID_SIZE + j] = Point3((Double)j, 0., (Double)i);
         meshDefinition.normals[i*MESH_GRID_SIZE + j] = Vector3(0., 1., 0.);
         if (i > 0 && j >0) {
            meshDefinition.quadrangles[((i - 1)*(MESH_GRID_SIZE - 1) + (j - 1)) * 4]     = (i - 1) * MESH_GRID_SIZE + (j - 1);
            meshDefinition.quadrangles[((i - 1)*(MESH_GRID_SIZE - 1) + (j - 1)) * 4 + 1] = i       * MESH_GRID_SIZE + (j - 1);
            meshDefinition.quadrangles[((i - 1)*(MESH_GRID_SIZE - 1) + (j - 1)) * 4 + 2] = i       * MESH_GRID_SIZE + j;
            meshDefinition.quadrangles[((i - 1)*(MESH_GRID_SIZE - 1) + (j - 1)) * 4 + 3] = (i - 1) * MESH_GRID_SIZE + j;            
         }
      }
   }
      
   return OptimizeSDKInterface::createMesh(meshDefinition);
}

void decimate(Mesh mesh)
{
   OptimizeSDKInterface::decimateToQualityVertexRemoval(MeshList(1, mesh), 1, -1, -1, -1);
}

void createMeshAndDecimate()
{
   Mesh mesh = createMesh();   
   decimate(mesh);
   MeshDefinition decimatedMesh = OptimizeSDKInterface::getMesh(mesh);
}

void progressCallback(void * userData, PiXYZ::CoreI::Int progress)
{
   printf("%d %%\n", progress);
}

int main()
{
    
   try {
      OptimizeSDKInterface::initialize("UnityOptimizeSDK", "2053b013182c0e02f63cbaab1edf1cec393e0a210c4824203f5207224f04391d06c744cca05d0ab7ca", "");
      std::cout << "Initialization successul" << std::endl;
   }
   catch (Exception & e) {
      //OptimizeSDKInterface::installLicense("pixyz.lic");
      std::cout << e.getDescription().c_str() << std::endl;
      return 1;
   }

   //OptimizeSDKInterface::setModuleProperty("Core", "ThreadCount", "1");
   //Ident progressCbId = OptimizeSDKInterface::addProgressChangedCallback(progressCallback);

   OptimizeSDKInterface::configureInterfaceLogger(false, false, false);

   std::cout << "Start single-threaded tests" << std::endl << std::flush;
   //OptimizeSDKInterface::pushProgression(TEST_COUNT, "single thread");
   for(int i = 0; i < TEST_COUNT; ++i)
      createMeshAndDecimate();
   //OptimizeSDKInterface::popProgression();
   std::cout << "Success" << std::endl << std::flush;
      
   OptimizeSDKInterface::saveAsPXZ("D:/data/test.pxz");
   OptimizeSDKInterface::clear();
   //OptimizeSDKInterface::removeProgressChangedCallback(progressCbId);

   std::cout << "Start multi-threaded tests" << std::endl << std::flush;        
#pragma omp parallel for
   for (int i = 0; i < TEST_COUNT; ++i)
      createMeshAndDecimate();   
   std::cout << "Success" << std::endl << std::flush;

   OptimizeSDKInterface::clear();
   
   return 0;
}
