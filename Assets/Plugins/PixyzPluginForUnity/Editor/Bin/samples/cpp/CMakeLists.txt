cmake_minimum_required(VERSION 3.0)

set(PIXYZ_SDK_DIR "NOT-DEFINED" CACHE PATH "Path to the PiXYZ SDK install directory")

if(NOT "${PIXYZ_SDK_DIR}" MATCHES "NOT-DEFINED")
    project(TessellateSdkSamples)

    LIST(APPEND CMAKE_MODULE_PATH ${CMAKE_SOURCE_DIR}/cmake)

    find_package(TessellateSDK)
    include_directories(${PIXYZ_INCLUDE_PATH})

    link_directories(${PIXYZ_SDK_DIR}/bin)

    add_executable(
        Sample1
        sample1.cpp
    )

    target_link_libraries(Sample1 ${PIXYZ_TESSELLATE_LIBRARY})

    add_executable(
        Sample2
        sample2.cpp
    )

    target_link_libraries(Sample2 ${PIXYZ_TESSELLATE_LIBRARY})

    add_executable(
        Sample3
        sample3.cpp
    )

    target_link_libraries(Sample3 ${PIXYZ_TESSELLATE_LIBRARY})
else()
    message("Please set PIXYZ_SDK_DIR to continue")
endif()
