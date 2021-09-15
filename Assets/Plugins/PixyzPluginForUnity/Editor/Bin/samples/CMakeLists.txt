cmake_minimum_required(VERSION 3.6)

set(PIXYZ_SDK_DIR "NOT-DEFINED" CACHE PATH "Path to the PiXYZ SDK install directory")

if(NOT "${PIXYZ_SDK_DIR}" MATCHES "NOT-DEFINED")

    project(ImportSdkSamples)

    LIST(APPEND CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake)

    find_package(ImportSDK)
    include_directories(${PIXYZ_INCLUDE_PATH})

    link_directories(${PIXYZ_SDK_DIR}/bin)

    add_executable(PiXYZSDKSceneSample SampleScene.cpp)

    target_link_libraries(SceneSample ${PIXYZ_IMPORT_LIBRARY})

    set(Opt WIN32)

else()
   message("Please set PIXYZ_SDK_DIR to continue")
endif()
