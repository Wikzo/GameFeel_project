using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Average : MonoBehaviour
{
	
public Transform a,b,c,d;

public GameObject g;

public Vector3 Offset;

void Update()
{
	Vector3 average = a.position+b.position+c.position+d.position;
	average = average / 4;
	g.transform.position = average + Offset;
}
	
}
