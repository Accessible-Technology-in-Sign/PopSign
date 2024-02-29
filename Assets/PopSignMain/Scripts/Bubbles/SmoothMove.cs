using UnityEngine;
using System.Collections;

public class SmoothMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent.SetParent(GameObject.Find("-Meshes").transform);
    }
}
