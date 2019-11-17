#ifndef PARTICLE2D_H
#define PARTICLE2D_H

#include "Vector3.h"
#include <math.h>
//Integration and forces

class Particle2D
{
public:
	Particle2D();

	void SetMass(float newMass);
	float GetMass();

	void AddForce(Vector3 newForce);

	void UpdateAcceleration();
	void UpdatePositionExplicitEuler(float dt);
	void updatePositionKinematic(float dt);
	void UpdateRotationExplicitEuler(float dt);
	void UpdateRotationKinematic(float dt);

	void FixedUpdate();

	Vector3 position, velocity, acceleration;
	float rotation, angularVelocity, angularAcceleration;

	Vector3 force;
	float mass, massInv;

};

#endif