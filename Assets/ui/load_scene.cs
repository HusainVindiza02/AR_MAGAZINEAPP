using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_scene : MonoBehaviour
{
	public void Levels(int _Levels)
	{
		SceneManager.LoadScene(_Levels);
	}
	
}
