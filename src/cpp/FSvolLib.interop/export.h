#pragma once

#if defined(__EMSCRIPTEN__)
#  include <emscripten/emscripten.h>
#  define FSVOLLIB_INTEROP_API EMSCRIPTEN_KEEPALIVE __attribute__((visibility("default")))
#elif defined(_WIN32)
#  define FSVOLLIB_INTEROP_API __declspec(dllexport)
#else
#  define FSVOLLIB_INTEROP_API __attribute__((visibility("default")))
#endif
