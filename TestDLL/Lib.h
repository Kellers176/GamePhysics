#ifndef LIB_H
#define LIB_H

#ifdef TESTDLL_EXPORT
#define TESTDLL_SYMBOL __declspec(dllexport)
#else  // !TESTDLL_EXPORT
#ifdef TESTDLL_IMPORT
#define TESTDLL_SYMBOL __declspec(dllimport)
#else  // !TESTDLL_IMPORT
#define TESTDLL_SYMBOL
#endif // !TESTDLL_IMPORT
#endif // !TESTDLL_EXPORT


#endif // !LIB_H