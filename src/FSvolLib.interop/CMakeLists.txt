cmake_minimum_required(VERSION 3.21)
project(FSvolLibInterop LANGUAGES CXX)

if(EMSCRIPTEN)
    add_executable(FSvolLibInterop
        VolumeLibrary.interop.cpp
    )
    set_target_properties(FSvolLibInterop PROPERTIES SUFFIX ".js")
else()
    add_library(FSvolLibInterop SHARED
        VolumeLibrary.interop.cpp
    )
endif()

target_include_directories(FSvolLibInterop
    PUBLIC
        ${CMAKE_CURRENT_SOURCE_DIR}
        ${CMAKE_CURRENT_SOURCE_DIR}/../FSvolLib
)

target_link_libraries(FSvolLibInterop PRIVATE FSvolLib)

if(EMSCRIPTEN)
    target_link_options(FSvolLibInterop PRIVATE
        "SHELL:-s WASM=1"
        "SHELL:-s MODULARIZE=1" # generated JS exports a function that returns a Promise that resolves to the module instance. Prevents polluting the global namespace with FSvolLibInterop.
        "SHELL:-s EXPORT_ES6=1"
        #"SHELL:-s SINGLE_FILE=1" # embeds the .wasm binary directly into the .js file. can significantly increase the size of the .js file.
        "SHELL:-s ENVIRONMENT=node"
        "SHELL:-s EXPORTED_FUNCTIONS=['_malloc','_free']"
        "SHELL:-s EXPORTED_RUNTIME_METHODS=['cwrap','ccall','UTF8ToString','stringToUTF8','lengthBytesUTF8','HEAP8','HEAPU8','HEAP32','HEAPU32','HEAPF64']"
    )
endif()

set_target_properties(FSvolLibInterop PROPERTIES
    CXX_STANDARD 17
    CXX_STANDARD_REQUIRED YES
)

if(CMAKE_BUILD_TYPE STREQUAL "Debug")
    target_compile_definitions(FSvolLibInterop PRIVATE _DEBUG)
else()
    target_compile_definitions(FSvolLibInterop PRIVATE NDEBUG)
endif()
