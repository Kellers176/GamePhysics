#include "Particle2D.h"

Particle2D::Particle2D()
{
	position = Vector3(0, 0, 0); 
	velocity = Vector3(0, 0, 0);
	acceleration = Vector3(0,0,0);
	force = Vector3(0, 0, 0);
	rotation = 0;
	angularVelocity = 0;
	angularAcceleration = 0;
	mass = 5;
	massInv = 0;
}

void Particle2D::SetMass(float newMass)
{
	mass = fmax(0.0f, newMass);
	massInv = mass > 0.0f ? 1.0f / mass : 0.0f;
}

float Particle2D::GetMass()
{
	return mass;
}

void Particle2D::AddForce(Vector3 newForce)
{
	force = force.Add(newForce);
}

void Particle2D::UpdateAcceleration()
{
	acceleration = force.Multiply(massInv);

	force.Set(0.0f, 0.0f, 0.0f);
}

void Particle2D::UpdatePositionExplicitEuler(float dt)
{
	// x(t+dt) = x(t) + v(t)dt
		// Euler's method:
		// F(t+dt) = F(t) + f(t)dt
		//                + (dF/dt)dt
	position = position.Add(velocity.Multiply(dt));

	// v(t+dt) = v(t) + a(t)dt
	velocity = velocity.Add(acceleration.Multiply(dt));
	//velocity += acceleration * dt;
}

//ignore this for rn
void Particle2D::updatePositionKinematic(float dt)
{
	// x(t+dt) = x(t) + v(t)dt + 1/2a(t)dt^2
//	position += velocity * dt + (0.5f * acceleration * (dt * dt));
	//position = position.Addvelocity.Multiply(dt)

//	velocity += acceleration * dt;
	velocity = velocity.Add(acceleration.Multiply(dt));
}

void Particle2D::UpdateRotationExplicitEuler(float dt)
{
	rotation += angularVelocity * dt;
	angularVelocity += angularAcceleration * dt;
}

void Particle2D::UpdateRotationKinematic(float dt)
{
	rotation += angularVelocity * dt + (0.5f * angularAcceleration * (dt * dt));
	angularVelocity += angularAcceleration * dt;
}

void Particle2D::FixedUpdate()
{
	//We need a way to calculat the time
//	acceleration.x = -sin(Time.fixedTime);
//	angularAcceleration = -sin(Time.fixedTime);

//	UpdatePositionExplicitEuler(Time.fixedDeltaTime);
	//updatePositionKinematic(Time.fixedDeltaTime);
	//updateRotationEulerExplicit(Time.fixedDeltaTime);
	//updateRotationKinematic(Time.fixedDeltaTime);

//	transform.position = position;
//	transform.Rotate(0, 0, rotation);

}
