#ifdef BUILDING_PXZ
#include <PiXYZOptimizeSDK/interface/OptimizeSDKInterface.h>
#else
#include "interface/OptimizeSDKInterface.h"
#endif
#include <iostream>

PXZ_USE_MODULE(OptimizeSDKI)
PXZ_USE_MODULE(CoreI)
PXZ_USE_MODULE(GeomI)
PXZ_USE_MODULE(PolygonalI)
PXZ_USE_MODULE(MaterialI)

#define TEST_COUNT 100
#define MESH_GRID_SIZE 50
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
   std::cout << "Old mesh : dressed(" << meshDefinition.dressedPolys.size() << 
            "), tris (" << meshDefinition.triangles.size() / 3 << 
            "), quad(" << meshDefinition.quadrangles.size() / 4 << ")" << std::endl;
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

   std::cout << "New mesh : dressed(" << decimatedMesh.dressedPolys.size() << 
            "), tris (" << decimatedMesh.triangles.size() / 3 << 
            "), quad(" << decimatedMesh.quadrangles.size() / 4 << ")" << std::endl;

   OptimizeSDKInterface::destroy(PiXYZ::PolygonalI::MeshList(1, mesh));
   //OptimizeSDKInterface::clear();
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

   OptimizeSDKInterface::addConsoleVerbose(PiXYZ::CoreI::Verbose(PiXYZ::CoreI::Verbose::INFO));

   OptimizeSDKInterface::configureInterfaceLogger(true, false, true);

   std::cout << "Start single-threaded tests" << std::endl << std::flush;


   PiXYZ::PolygonalI::MeshList l(100);
   for (size_t i = 0; i < 100; ++i) {
       l[i] = createMesh();
   }

   OptimizeSDKInterface::decimateToQualityVertexRemoval(l, 0.2, -1, -1, -1);

   OptimizeSDKInterface::destroy(l);


   //OptimizeSDKInterface::pushProgression(TEST_COUNT, "single thread");
   /*for(int i = 0; i < 1000; ++i)
      createMeshAndDecimate();
   //OptimizeSDKInterface::popProgression();
   std::cout << "Success" << std::endl << std::flush;*/

   //OptimizeSDKInterface::saveAsPXZ("test.pxz");

/*
   OptimizeSDKInterface::saveAsPXZ("/home/thomas/test.pxz");
   OptimizeSDKInterface::clear();
   //OptimizeSDKInterface::removeProgressChangedCallback(progressCbId);

   std::cout << "Start multi-threaded tests" << std::endl << std::flush;        
#pragma omp parallel for
   for (int i = 0; i < TEST_COUNT; ++i)
      createMeshAndDecimate();   
   std::cout << "Success" << std::endl << std::flush;
   */
   OptimizeSDKInterface::clear();
   
   return 0;
}
