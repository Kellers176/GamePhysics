#include "Vector3.h"

Vector3::Vector3()
{
	x = 0;
	y = 0;
	z = 0;
}

Vector3::Vector3(float newx, float newy, float newz)
{
	x = newx;
	y = newy;
	z = newz;
}

Vector3 Vector3::Add(Vector3 other)
{
	Vector3 temp = Vector3(0,0,0);
	temp.x = x + other.x;
	temp.y = y + other.y;
	temp.z = z + other.z;
	return temp;
}

Vector3 Vector3::Multiply(float d)
{
	x *= d;
	y *= d;
	z *= d;
	Vector3 temp = Vector3(x, y, z);
	return  temp;
}

void Vector3::Set(float newX, float newY, float newZ)
{
	x = newX;
	y = newY;
	z = newZ;
}
