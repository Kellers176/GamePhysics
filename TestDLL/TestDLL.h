#ifndef TESTDLL_H
#define TESTDLL_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else // !__cplusplus
#endif

TESTDLL_SYMBOL int InitFoo(int f_new);
TESTDLL_SYMBOL int DoFoo(int bar);
TESTDLL_SYMBOL int TermFoo();

#ifdef __cplusplus
}
#else // !__cplusplus
#endif


#endif // !TESTDLL_H