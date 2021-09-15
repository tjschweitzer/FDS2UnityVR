#include "ImportSDKInterface.h"

using namespace PiXYZ;
using namespace ImportSDKI;


std::string toString(const double & v)
{
   char buffer[256];
#ifdef WIN32
   _snprintf_s(buffer, sizeof(buffer), "%lf", v);
#else
   snprintf(buffer, sizeof(buffer), "%lf", v);
#endif
   return std::string(buffer);
}

std::string matrixToString(const GeomI::Matrix4& mat)
{
   std::string str = "[";
   for (int i = 0; i < 4; ++i) {
      if (i > 0) str += ", ";
      str += "[";
      for (int j = 0; j < 4; ++j) {
         if (j > 0) str += ", ";
         str += toString(mat[i][j]);
      }
      str += "]";
   }
   str += "]";
   return str;
}

void printMaterial(const PiXYZ::MaterialI::Material& material, int indentLevel)
{
   std::string indent = "";
   for (int i = 0; i < indentLevel; i++) indent += "\t";
   
   printf("%sMaterial %d\n", indent.c_str(), material);

   MaterialI::MaterialPatternType type = ImportSDKInterface::getMaterialPatternType(material);
   
   switch (type)
   {
   case MaterialI::MaterialPatternType::COLOR:
      {
         MaterialI::ColorMaterialInfos infos = ImportSDKInterface::getColorMaterialInfos(material);
         printf("%s\tName : %s\n", indent.c_str(), infos.name.c_str());
         printf("%s\tColor : %f %f %f %f\n", indent.c_str(), infos.color.r, infos.color.g, infos.color.b, infos.color.a);
      }
      break;
   case MaterialI::MaterialPatternType::PBR:
   {
      MaterialI::PBRMaterialInfos infos = ImportSDKInterface::getPBRMaterialInfos(material);
      printf("%s\tName : %s\n", indent.c_str(), infos.name.c_str());
      
      //example with albedo for ColorOrTexture
      if (infos.albedo._type == MaterialI::ColorOrTexture::COLOR) {
         printf("%s\tAlbedo color : %f %f %f\n", indent.c_str(), infos.albedo.color.r, infos.albedo.color.g, infos.albedo.color.b);
      }
      else if(infos.albedo._type == MaterialI::ColorOrTexture::TEXTURE){
         printf("%s\tAlbedo : \n", indent.c_str());
         printf("%s\t\tImage : %d\n", indent.c_str(), infos.albedo.texture.image);
         printf("%s\t\tTiling : %f %f\n", indent.c_str(), infos.albedo.texture.tilling.x, infos.albedo.texture.tilling.y);
         printf("%s\t\tOffset : %f %f\n", indent.c_str(), infos.albedo.texture.offset.x, infos.albedo.texture.offset.y);
         printf("%s\t\tChannel : %d\n", indent.c_str(), infos.albedo.texture.channel);
      }

      //example with metallic for CoeffOrTexture
      if (infos.metallic._type == MaterialI::CoeffOrTexture::COEFF) {
         printf("%s\tMetallic coef : %f\n", indent.c_str(), infos.metallic.coeff);
      }
      else if (infos.metallic._type == MaterialI::ColorOrTexture::TEXTURE) {
         printf("%s\tMetallic : \n", indent.c_str());
         printf("%s\t\tImage : %d\n", indent.c_str(), infos.metallic.texture.image);
         printf("%s\t\tTiling : %f %f\n", indent.c_str(), infos.metallic.texture.tilling.x, infos.metallic.texture.tilling.y);
         printf("%s\t\tOffset : %f %f\n", indent.c_str(), infos.metallic.texture.offset.x, infos.metallic.texture.offset.y);
         printf("%s\t\tChannel : %d\n", indent.c_str(), infos.metallic.texture.channel);
      }

   }
   break;

   case MaterialI::MaterialPatternType::STANDARD:
   {
      MaterialI::StandardMaterialInfos infos = ImportSDKInterface::getStandardMaterialInfos(material);
      printf("%s\tName : %s\n", indent.c_str(), infos.name.c_str());

      //example with diffuse for ColorOrTexture
      if (infos.diffuse._type == MaterialI::ColorOrTexture::COLOR) {
         printf("%s\tDiffuse color : %f %f %f\n", indent.c_str(), infos.diffuse.color.r, infos.diffuse.color.g, infos.diffuse.color.b);
      }
      else if (infos.diffuse._type == MaterialI::ColorOrTexture::TEXTURE) {
         printf("%s\tDiffuse : \n", indent.c_str());
         printf("%s\t\tImage : %d\n", indent.c_str(), infos.diffuse.texture.image);
         printf("%s\t\tTiling : %f %f\n", indent.c_str(), infos.diffuse.texture.tilling.x, infos.diffuse.texture.tilling.y);
         printf("%s\t\tOffset : %f %f\n", indent.c_str(), infos.diffuse.texture.offset.x, infos.diffuse.texture.offset.y);
         printf("%s\t\tChannel : %d\n", indent.c_str(), infos.diffuse.texture.channel);
      }

      //example with metallic for Coeff
      printf("%s\tShininess : %f\n", indent.c_str(), infos.shininess);

   }
   break;
   case MaterialI::MaterialPatternType::UNLIT_TEXTURE:
   {
      MaterialI::UnlitTextureMaterialInfos infos = ImportSDKInterface::getUnlitTextureMaterialInfos(material);
      printf("%s\tName : %s\n", indent.c_str(), infos.name.c_str());
      printf("%s\t\tImage : %d\n", indent.c_str(), infos.texture.image);
      printf("%s\t\tTiling : %f %f\n", indent.c_str(), infos.texture.tilling.x, infos.texture.tilling.y);
      printf("%s\t\tOffset : %f %f\n", indent.c_str(), infos.texture.offset.x, infos.texture.offset.y);
      printf("%s\t\tChannel : %d\n", indent.c_str(), infos.texture.channel);

   }
   break;
   default:
      break;
   }
}

void printPart(const PiXYZ::SceneI::Part& part, int indentLevel)
{
   std::string indent = "";
   for (int i = 0; i < indentLevel; i++) indent += "\t";

   SceneI::Shape shape = ImportSDKInterface::getPartActiveShape(part);

   //test shape type : TessellatedShape, BRepShape
   CoreI::String type = ImportSDKInterface::getEntityTypeString(shape);

   printf("%s\tType : %s\n", indent.c_str(), type.c_str());

   if (strcmp(type.c_str(), "TessellatedShape") == 0) {
      PolygonalI::Tessellation tess = ImportSDKInterface::getTessellatedShapeTessellation(shape);

      PolygonalI::MeshDefinition def = ImportSDKInterface::getMeshDefinitions(tess);

      printf("%s\t\tNb vertices : %zu\n", indent.c_str(), def.vertices.size());
      printf("%s\t\tNb normals : %zu\n", indent.c_str(), def.normals.size());
      printf("%s\t\tNb tangents : %zu\n", indent.c_str(), def.tangents.size());
      
      printf("%s\t\tNb triangles : %zu\n", indent.c_str(), def.triangles.size());
      printf("%s\t\tNb quadrangles : %zu\n", indent.c_str(), def.quadrangles.size());

      printf("%s\t\tNb uvs : %zu\n", indent.c_str(), def.uvs.size());
      for (unsigned int i = 0; i < def.uvs.size(); ++i) {
         printf("%s\t\t\tChannel %d : nb uvs : %zu\n", indent.c_str(), i, def.uvs[i].size());
      }

      for (unsigned int i = 0; i < def.dressedPolys.size(); ++i) {
         printMaterial(def.dressedPolys[i].material, indentLevel + 2);
      }

   }
}

void printOccurence(const PiXYZ::SceneI::Occurrence& occurence, int indentLevel, bool printLocalMatrix, bool printMaterialInfos, bool printProperties) {

   SceneI::SceneNode node = ImportSDKInterface::getSceneNodeOfOccurrence(occurence);

   CoreI::String name = ImportSDKInterface::getNodeName(node);

   std::string indent = "";
   for (int i = 0; i < indentLevel; i++) indent += "\t";

   printf("%s%s\n", indent.c_str(), name.c_str());

   if (printLocalMatrix) {
      GeomI::Matrix4 mat = ImportSDKInterface::getLocalMatrix(node);
      std::string matString = matrixToString(mat);
      printf("%s\tMatrix : %s\n", indent.c_str(), matString.c_str());
   }

   if (printMaterialInfos) {
      MaterialI::Material material = ImportSDKInterface::getOccurrenceActiveMaterial(occurence);

      if (material > 0) {
         printMaterial(material, indentLevel + 1);
      } 
   }

   if (printProperties) {
      CoreI::StringList l = ImportSDKInterface::listProperties(node);
      for (unsigned int i = 0; i < l.size(); ++i) {
         printf("%s\t%s : %s\n", indent.c_str(), l[i].c_str(), ImportSDKInterface::getProperty(node, l[i]).c_str());
      }
   }
   
   SceneI::SceneNodeType type = ImportSDKInterface::getSceneNodeType(node);

   if (type == SceneI::SceneNodeType::INSTANCE) {
      printf("%s\t%s\n", indent.c_str(), "This node is an instance");

      {
         SceneI::OccurrenceList children = ImportSDKInterface::getOccurrenceChildren(occurence);

         for (unsigned int i = 0; i < children.size(); ++i) {
            printOccurence(children[i], indentLevel + 2, printLocalMatrix, printMaterialInfos, printProperties);
         }
      }

   }
   else {
      //printf("%s\t%s\n", indent.c_str(), "This node is a component");

      SceneI::ComponentType componenetType = ImportSDKInterface::getComponentType(node);

      switch (componenetType)
      {
      case SceneI::ComponentType::ASSEMBLY:
         printf("%s\t%s\n", indent.c_str(), "This node is an assembly");
         {
            SceneI::OccurrenceList children = ImportSDKInterface::getOccurrenceChildren(occurence);

            for (unsigned int i = 0; i < children.size(); ++i) {
               printOccurence(children[i], indentLevel + 2, printLocalMatrix, printMaterialInfos, printProperties);
            }

         }
         break;
      case SceneI::ComponentType::LIGHT:
         printf("%s\t%s\n", indent.c_str(), "This node is a light");
         printf("%s\tColor : %s\n", indent.c_str(), ImportSDKInterface::getProperty(node, "Color").c_str());
         printf("%s\tPower : %s\n", indent.c_str(), ImportSDKInterface::getProperty(node, "Power").c_str());
         printf("%s\tDirection : %s\n", indent.c_str(), ImportSDKInterface::getProperty(node, "Direction").c_str());

         break;
      case SceneI::ComponentType::CAMERA:
         printf("%s\t%s\n", indent.c_str(), "This node is a camera");
         break;
      case SceneI::ComponentType::PART:
         printf("%s\t%s\n", indent.c_str(), "This node is a part");
         printPart(node, indentLevel+1);
         break;
      default:
         break;
      }
   }
}


int main(int argc, char ** argv)
{
   if (argc < 2) {
      fprintf(stderr, "Usage: %s fileToImport\n", argv[0]);
      exit(EXIT_FAILURE);
   }

   try {
      ImportSDKInterface::initialize();

      ImportSDKInterface::importScene(argv[1]);

      printOccurence(ImportSDKInterface::getRootOccurrence(), 1, true, true, false);

   }
   catch (const PiXYZ::CoreI::Exception & e) {
      printf("%s\n", e.getDescription().c_str());
   }

   return 0;
}
