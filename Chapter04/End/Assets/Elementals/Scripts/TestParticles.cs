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

/***************
* TestParticles class
* This class handles user key inputs, instantiate and destroy the particle effects.
* 
* User presses Up/Down key to switch Element; Fire, Water, Wind, Earth, Light, Darkness.
* Left/Right key to switch particle in current Element.
**************/

public class TestParticles : MonoBehaviour {
	
	#region Variables
	
		// Elements
		public GameObject[] m_PrefabListFire;
		public GameObject[] m_PrefabListWind;
		public GameObject[] m_PrefabListWater;
		public GameObject[] m_PrefabListEarth;
		public GameObject[] m_PrefabListIce;
		public GameObject[] m_PrefabListThunder;
		public GameObject[] m_PrefabListLight;
		public GameObject[] m_PrefabListDarkness;
		
		// Index of current element
		int m_CurrentElementIndex = -1;
	
		// Index of current particle
		int m_CurrentParticleIndex = -1;	
		
		// Name of current element
		string m_ElementName = "";	
	
		// Name of current particle
		string m_ParticleName = "";
		
		// Current particle list
		GameObject[] m_CurrentElementList = null;
	
		// GameObject of current particle that is showing in the scene
		GameObject m_CurrentParticle = null;
	
	#endregion
	
	// ######################################################################
	// MonoBehaviour Functions
	// ######################################################################

	#region Component Segments
		
		// Use this for initialization
		void Start () {
	
			// Check if there is any particle in prefab list
			if(m_PrefabListFire.Length>0 ||
				m_PrefabListWind.Length>0 ||
				m_PrefabListWater.Length>0 ||
				m_PrefabListEarth.Length>0 ||
				m_PrefabListIce.Length>0 ||
				m_PrefabListThunder.Length>0 ||
				m_PrefabListLight.Length>0 ||
				m_PrefabListDarkness.Length>0)
			{
				// reset indices of element and particle
				m_CurrentElementIndex = 0;
				m_CurrentParticleIndex = 0;
			
				// Show particle
				ShowParticle();
			}
		}
		
		// Update is called once per frame
		void Update () {
			
			// Check if there is any particle in prefab list
			if(m_CurrentElementIndex!=-1 && m_CurrentParticleIndex!=-1)
			{
				// User released Up arrow key
				if(Input.GetKeyUp(KeyCode.UpArrow))
				{
					m_CurrentElementIndex++;
					m_CurrentParticleIndex = 0;
					ShowParticle();
				}
				// User released Down arrow key
				else if(Input.GetKeyUp(KeyCode.DownArrow))
				{
					m_CurrentElementIndex--;
					m_CurrentParticleIndex = 0;
					ShowParticle();
				}
				// User released Left arrow key
				else if(Input.GetKeyUp(KeyCode.LeftArrow))
				{
					m_CurrentParticleIndex--;
					ShowParticle();
				}
				// User released Right arrow key
				else if(Input.GetKeyUp(KeyCode.RightArrow))
				{
					m_CurrentParticleIndex++;
					ShowParticle();
				}
			}
		}
	
		// OnGUI is called for rendering and handling GUI events.
		void OnGUI () {
		
			// Show version number
			GUI.Window(1, new Rect((Screen.width-260), 5, 250, 80), InfoWindow, "Info");

			// Show Help GUI window
			GUI.Window(2, new Rect((Screen.width-260), Screen.height-85, 250, 80), ParticleInformationWindow, "Help");
		}
	
	#endregion Component Segments
	
	// ######################################################################
	// Functions Functions
	// ######################################################################

	#region Functions
		
		// Remove old Particle and do Create new Particle GameObject
		void ShowParticle()
		{
			// Make m_CurrentElementIndex be rounded
			if(m_CurrentElementIndex>7)
			{
				m_CurrentElementIndex = 0;
			}
			else if(m_CurrentElementIndex<0)
			{
				m_CurrentElementIndex = 7;
			}
			
			// Update current m_CurrentElementList and m_ElementName
			if(m_CurrentElementIndex==0)
			{
				m_CurrentElementList = m_PrefabListFire;
				m_ElementName = "FIRE";
			}
			else if(m_CurrentElementIndex==1)
			{
				m_CurrentElementList = m_PrefabListWater;
				m_ElementName = "WATER";
			}
			else if(m_CurrentElementIndex==2)
			{
				m_CurrentElementList = m_PrefabListWind;
				m_ElementName = "WIND";
			}
			else if(m_CurrentElementIndex==3)
			{
				m_CurrentElementList = m_PrefabListEarth;
				m_ElementName = "EARTH";
			}
			else if(m_CurrentElementIndex==4)
			{
				m_CurrentElementList = m_PrefabListThunder;
				m_ElementName = "THUNDER";
			}
			else if(m_CurrentElementIndex==5)
			{
				m_CurrentElementList = m_PrefabListIce;
				m_ElementName = "ICE";
			}
			else if(m_CurrentElementIndex==6)
			{
				m_CurrentElementList = m_PrefabListLight;
				m_ElementName = "LIGHT";
			}
			else if(m_CurrentElementIndex==7)
			{
				m_CurrentElementList = m_PrefabListDarkness;
				m_ElementName = "DARKNESS";
			}
	
			// Make m_CurrentParticleIndex be rounded
			if(m_CurrentParticleIndex>=m_CurrentElementList.Length)
			{
				m_CurrentParticleIndex = 0;
			}
			else if(m_CurrentParticleIndex<0)
			{
				m_CurrentParticleIndex = m_CurrentElementList.Length-1;
			}
	
			// update current m_ParticleName
			m_ParticleName = m_CurrentElementList[m_CurrentParticleIndex].name;
	
			// Remove Old particle
			if(m_CurrentParticle!=null)
			{
				DestroyObject(m_CurrentParticle);
			}
	
			// Create new particle
			m_CurrentParticle = (GameObject) Instantiate(m_CurrentElementList[m_CurrentParticleIndex]);
		}
		
		// Show Help window
		void ParticleInformationWindow(int id)
		{
			GUI.Label(new Rect(12, 25, 280, 20), "Up/Down: "+m_ElementName+" ("+(m_CurrentParticleIndex+1) + "/" + m_CurrentElementList.Length +")");
			GUI.Label(new Rect(12, 50, 280, 20), "Left/Right: "+ m_ParticleName.ToUpper());
		}
	
		// Show Info window
		void InfoWindow(int id)
		{
			GUI.Label(new Rect(15, 25, 240, 20), "Elementals 1.1.1");
			GUI.Label(new Rect(15, 50, 240, 20), "www.ge-team.com/pages");
		}
		
	#endregion {Functions}
}
