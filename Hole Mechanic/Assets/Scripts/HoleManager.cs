using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public Mesh _mesh;
    public List<int> VerticesIndex = new List<int>();

    public List<Vector3> offSet = new List<Vector3>();
    public float standardDistance;
    public float holeSize = 1f;
    public float moveSpeed;
    float x, y;
    Vector3 touch, targetPos;


    [Header("Gravity")]
    [Range(1f, 1000f)] public float power = 1f;
    [Range(-10f, 10f)] public float upOrDown;
    [Range(1f, 10f)] public float forceRange = 1f;

    public ForceMode forceMode;
    public LayerMask layerMask;



    void Start()
    {
        
        _mesh = meshFilter.mesh;
        for (int i = 0; i < _mesh.vertices.Length; i++)
        {
            var distance = Vector3.Distance(transform.position, _mesh.vertices[i]);
            Debug.Log(distance);
            if (distance<=standardDistance)
            {
                Debug.Log("added index "+i);
                VerticesIndex.Add(i);
                offSet.Add(_mesh.vertices[i] - transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveHole();
        Vector3[] vertices = _mesh.vertices;
        for (int i = 0; i < VerticesIndex.Count; i++)
        {
            vertices[VerticesIndex[i]] = transform.position + offSet[i] * holeSize;
        }
        _mesh.vertices = vertices;
        meshFilter.mesh = _mesh;
        meshCollider.sharedMesh = _mesh;
        
    }

    private void FixedUpdate()
    {
        Gravity(transform.position, forceRange, layerMask);
    }
    void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        
        touch = Vector3.Lerp(transform.position, transform.position+new Vector3(x,0,y),moveSpeed*Time.deltaTime);
        targetPos = Vector3.ClampMagnitude(touch, 10);
        transform.position = targetPos;
    }
    private void Gravity(Vector3 gravitySource,float range, LayerMask layerMask)
    {
        Collider[] objs = Physics.OverlapSphere(gravitySource, range, layerMask);
        for (int i = 0; i < objs.Length; i++)
        {
            Rigidbody rbs = objs[i].GetComponent<Rigidbody>();
            Vector3 forceDirection = new Vector3(gravitySource.x, upOrDown, gravitySource.z) - objs[i].transform.position;
            rbs.AddForceAtPosition(power * forceDirection.normalized, gravitySource);
        }
    }
}
