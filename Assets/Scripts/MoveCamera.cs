using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
	public float speed = .5f;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetAxis("Horizontal") != 0)
		{
			transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed;
		}
		if (Input.GetAxis("Vertical") != 0)
		{
			transform.position += transform.forward * Input.GetAxis("Vertical") * speed;

		}
	}
}
