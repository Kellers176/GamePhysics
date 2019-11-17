#ifndef TESTDLL_H
#define TESTDLL_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else // !__cplusplus
#endif

TESTDLL_SYMBOL int InitParticle2D();
TESTDLL_SYMBOL int DoParticle2D();
TESTDLL_SYMBOL int TermFoo();

#ifdef __cplusplus
}
#else // !__cplusplus
#endif


#endif // !TESTDLL_H