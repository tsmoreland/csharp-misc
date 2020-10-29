#pragma once


#ifdef NATIVELIBRARY_EXPORTS
#define NATIVELIBRARY_EXPORT __declspec(dllexport)
#else 
#define NATIVELIBRARY_EXPORT __declspec(dllimport)
#endif
