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

float GetMass()
{
	if (inst)
	{
		int result = inst->GetMass();
		return result;
	}
	return 0;
}

int SetMass(float newMass)
{
	if (inst)
	{
		inst->SetMass(newMass);
		return 1;
	}
	return 0;
}

int AddForce(float newForceX, float newForceY, float newForceZ)
{
	if (inst)
	{
		inst->AddForce(Vector3(newForceX, newForceY, newForceZ));
		return 1;
	}
	return 0;
}

int UpdateAcceleration()
{
	if (inst)
	{
		inst->UpdateAcceleration();
		return 1;
	}
	return 0;
}

int UpdatePositionExplicitEuler(float dt)
{
	if (inst)
	{
		inst->UpdatePositionExplicitEuler(dt);
		return 1;
	}
	return 0;
}

int UpdatePositionKinematic(float dt)
{
	if (inst)
	{
		inst->updatePositionKinematic(dt);
		return 1;
	}
	return 0;
}

int UpdateRotationExplicitEuler(float dt)
{
	if (inst)
	{
		inst->UpdateRotationExplicitEuler(dt);
		return 1;
	}
	return 0;
}

int UpdateRotationKinematic(float dt)
{
	if (inst)
	{
		inst->UpdateRotationKinematic(dt);
		return 1;
	}
	return 0;
}

int TermParticle2D()
{
	if (inst)
	{
		delete inst;
		inst = 0;
		return 1;
	}
	return 0;
}
