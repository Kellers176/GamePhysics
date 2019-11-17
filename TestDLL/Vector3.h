#ifndef VECTOR3_H
#define VECTOR3_H

#include <math.h>

class Vector3
{
public:
	Vector3();
	Vector3(float newx, float newy, float newz);
	Vector3 Add(Vector3 other);
	Vector3 Multiply(float d);
	void Set(float x, float y, float z);

	float x, y, z;
};


#endif
