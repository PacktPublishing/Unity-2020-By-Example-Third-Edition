// Copyright (c) 2014 Gold Experience Team
//
// Elementals version: 1.1.1
// Author: Gold Experience TeamDev (http://www.ge-team.com/)
// Support: geteamdev@gmail.com
// Please direct any bugs/comments/suggestions to geteamdev@gmail.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

/*
TERMS OF USE - EASING EQUATIONS#
Open source under the BSD License.
Copyright (c)2001 Robert Penner
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

#region Namespaces

using UnityEngine;
using System.Collections;

#endregion

/**************
* Orbit Camera event handler.
* It should be attached with Main Camera in Volcanic Rock in Demo scene.
**************/

public class OrbitCamera : MonoBehaviour
{
  #region Variables

	// Target to look at
	public Transform TargetLookAt;
 
	// Camera distance variables
	public float Distance = 10.0f;
	public float DistanceMin = 5.0f;
	public float DistanceMax = 15.0f;  
	float startingDistance = 0.0f;
	float desiredDistance = 0.0f;

	// Mouse variables
	float mouseX = 0.0f;
	float mouseY = 0.0f;
	public float X_MouseSensitivity = 5.0f;
	public float Y_MouseSensitivity = 5.0f;
	public float MouseWheelSensitivity = 5.0f;

	// Axis limit variables
	public float Y_MinLimit = 15.0f;
	public float Y_MaxLimit = 70.0f;   
	public float DistanceSmooth = 0.025f;
	float velocityDistance = 0.0f; 
	Vector3 desiredPosition = Vector3.zero;   
	public float X_Smooth = 0.05f;
	public float Y_Smooth = 0.1f;

	// Velocity variables
	float velX = 0.0f;
	float velY = 0.0f;
	float velZ = 0.0f;
	Vector3 position = Vector3.zero;

  #endregion

  // ######################################################################
  // MonoBehaviour Functions
  // ######################################################################

  #region Component Segments

	void Start() 
	{
		//Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
		Distance = Vector3.Distance(TargetLookAt.transform.position, gameObject.transform.position);
		if (Distance > DistanceMax)
		  DistanceMax = Distance;
		startingDistance = Distance;
		Reset();
	}

	// Update is called once per frame
	void Update()
	{
	}

	// LateUpdate is called after all Update functions have been called.
	void LateUpdate()
	{
		if (TargetLookAt == null)
		   return;
 
		HandlePlayerInput();
 
		CalculateDesiredPosition();
 
		UpdatePosition();
	}

  #endregion

  // ######################################################################
  // Player Input Functions
  // ######################################################################

  #region Component Segments

	void HandlePlayerInput()
	{
		// mousewheel deadZone
		float deadZone = 0.01f; 
 
		if (Input.GetMouseButton(0))
		{
		   mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
		   mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
		}
	 
		// this is where the mouseY is limited - Helper script
		mouseY = ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);
 
		// get Mouse Wheel Input
		if (Input.GetAxis("Mouse ScrollWheel") < -deadZone || Input.GetAxis("Mouse ScrollWheel") > deadZone)
		{
		   desiredDistance = Mathf.Clamp(Distance - (Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity), 
													 DistanceMin, DistanceMax);
		}
	}

  #endregion

  // ######################################################################
  // Calculation Functions
  // ######################################################################

  #region Component Segments

	void CalculateDesiredPosition()
	{
		// Evaluate distance
		Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velocityDistance, DistanceSmooth);
 
		// Calculate desired position -> Note : mouse inputs reversed to align to WorldSpace Axis
		desiredPosition = CalculatePosition(mouseY, mouseX, Distance);
	}

	Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
		return TargetLookAt.position + (rotation * direction);
	}

  #endregion

  // ######################################################################
  // Utilities Functions
  // ######################################################################

  #region Component Segments

	// update camera position
	void UpdatePosition()
	{
		float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_Smooth);
		float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_Smooth);
		float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_Smooth);
		position = new Vector3(posX, posY, posZ);
 
		transform.position = position;
 
		transform.LookAt(TargetLookAt);
	}

	// Reset Mouse variables
	void Reset() 
	{
		mouseX = 0;
		mouseY = 0;
		Distance = startingDistance;
		desiredDistance = Distance;
	}

	// Clamps angle between a minimum float and maximum float value
	float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360.0f || angle > 360.0f)
		{
		   if (angle < -360.0f)
			 angle += 360.0f;
		   if (angle > 360.0f)
			 angle -= 360.0f;
		}
 
		return Mathf.Clamp(angle, min, max);
	}

  #endregion
}

