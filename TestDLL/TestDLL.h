#ifndef TESTDLL_H
#define TESTDLL_H

#include "Lib.h"

#ifdef __cplusplus
extern "C"
{
#else // !__cplusplus
#endif

TESTDLL_SYMBOL int InitParticle2D();
TESTDLL_SYMBOL float GetMass();
TESTDLL_SYMBOL int SetMass(float newMass);
TESTDLL_SYMBOL int AddForce(float newForceX, float newForceY, float newForceZ);
TESTDLL_SYMBOL int UpdateAcceleration();
TESTDLL_SYMBOL int UpdatePositionExplicitEuler(float dt);
TESTDLL_SYMBOL int UpdatePositionKinematic(float dt);
TESTDLL_SYMBOL int UpdateRotationExplicitEuler(float dt);
TESTDLL_SYMBOL int UpdateRotationKinematic(float dt);
TESTDLL_SYMBOL int TermParticle2D();

#ifdef __cplusplus
}
#else // !__cplusplus
#endif


#endif // !TESTDLL_H