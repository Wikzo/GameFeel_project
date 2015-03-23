using System.Collections.Generic;
using UnityEngine;
using System.Collections;

// https://www.youtube.com/watch?v=zm9bqSSiIdo&index=22&list=WL

public class DrawLines : MonoBehaviour
{
    public Material mat;
    public int NumberOfLines = 10;

    private List<Vector3> startVertex;
    private List<Vector3> endVertex;
    private Vector3 mousePos;

    private bool draw = false;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            draw = true;

            mousePos = Input.mousePosition;

            for (int i = 0; i < NumberOfLines; i++)
                startVertex[i] = new Vector3(mousePos.x/Screen.width, mousePos.y/Screen.height, 0);
        }
        else
            draw = false;

        if (Input.GetKeyDown(KeyCode.R))
            Populate();

    }

    void Populate()
    {
        startVertex = new List<Vector3>();
        endVertex = new List<Vector3>();
        for (int i = 0; i < NumberOfLines; i++)
        {
            startVertex.Add(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
            endVertex.Add(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
        }
    }

    private int counter = 0;
    void OnPostRender()
    {
        if (!draw)
            return;

        for (int i = 0; i < NumberOfLines; i++)
        {
            if (!mat)
            {
                Debug.LogError("Please Assign a material on the inspector");
                return;
            }
            GL.PushMatrix();
            mat.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.LINES);
            GL.Color(Color.red);

            GL.Vertex(startVertex[i]);
            GL.Vertex(endVertex[i]);
            //GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));
            GL.End();
            GL.PopMatrix();
        }
    }
    void Awake()
    {
        Populate();
    }
}