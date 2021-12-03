#pragma once

#ifdef MANAGEDLIBRARY_EXPORTS
#define MANAGEDLIBRARY_EXPORT __declspec(dllexport)
#else
#define MANAGEDLIBRARY_EXPORT __declspec(dllimport)
#endif

