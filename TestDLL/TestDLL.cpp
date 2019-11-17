#include "TestDLL.h"
#include "Foo.h"

#include "Particle2D.h"

Particle2D* inst = 0;

int InitParticle2D()
{
	if (!inst)
	{
		inst = new Particle2D();
		return 1;
	}
	return 0;
}

int DoParticle2D()
{
	if (inst)
	{
		int result = inst->GetMass();
		return result;
	}
	return 0;
}

int TermFoo()
{
	if (inst)
	{
		delete inst;
		inst = 0;
		return 1;
	}
	return 0;
}
