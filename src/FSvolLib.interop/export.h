#pragma once

#if defined(_WIN32)
#  define FSVOLLIB_INTEROP_API __declspec(dllexport)
#else
#  define FSVOLLIB_INTEROP_API __attribute__((visibility("default")))
#endif
