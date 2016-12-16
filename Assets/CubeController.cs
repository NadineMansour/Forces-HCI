using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshCollider))]
public class CubeController : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	Transform[] ts;

	//Public parameters
	public float massInput;
	private float extraMass = 0.0f;

	//private attributes
	private Text mass,forceH,forceV,accH,accV;
	private float gravity = 9.8f;
	private List<GameObject> cubes;
	public bool added;

	void Start()
	{
		ts = gameObject.GetComponentsInChildren<Transform>();
		added = false;
		foreach (Transform t in ts)
		{
			if(t.CompareTag("mass")){
				mass = t.gameObject.GetComponent<Text>();
			}
			if(t.CompareTag("forceh")){
				forceH = t.gameObject.GetComponent<Text>();
			}
			if(t.CompareTag("forcev")){
				forceV = t.gameObject.GetComponent<Text>();
			}
			if(t.CompareTag("acch")){
				accH = t.gameObject.GetComponent<Text>();
			}
			if(t.CompareTag("accv")){
				accV = t.gameObject.GetComponent<Text>();
			}
		}
		cubes = new List<GameObject>();
	}

	void Update(){
		Debug.Log(this);
		//Debug.Log(extraMass);
		Debug.Log(cubes.Count);
		mass.text = "Mass: " + massInput;
		forceV.text = "Force-V: " + ((massInput+extraMass)*gravity);
		forceH.text = "Force-H: " + 0;
		accV.text = "Acc-V: " + (gravity);
		accH.text = "Acc-V: " + 0;
	}

	void updateAllCubes(float newMass){
		foreach (GameObject c in cubes){
			c.GetComponent<CubeController>().updateExtraMass(newMass);
		}
	}

	public float getMass(){
		return massInput;
	}
	
	void OnMouseDown()
	{
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag()
	{
		Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) , offset;
		transform.position = curPosition;
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.CompareTag("cube")){
			//Debug.Log("Col");
			if(collision.gameObject.transform.position.y < transform.position.y && !collision.gameObject.GetComponent<CubeController>().added){
				collision.gameObject.GetComponent<CubeController>().updateMass(massInput);
				foreach (GameObject c in collision.gameObject.GetComponent<CubeController>().getCubes())
				{
					cubes.Add(c);
				}
				cubes.Add(collision.gameObject);
				collision.gameObject.GetComponent<CubeController>().added = true;
			}
		}        
    }
	void OnCollisionExit(Collision collision) {
		if(collision.gameObject.CompareTag("cube")){
			if(collision.gameObject.transform.position.y < transform.position.y && collision.gameObject.GetComponent<CubeController>().added){
				collision.gameObject.GetComponent<CubeController>().updateMass(-1*massInput);
				cubes = new List<GameObject>();
				collision.gameObject.GetComponent<CubeController>().added = false;
			}
		}        
    }

	List<GameObject> getCubes(){
		return cubes;
	}

	void updateMass(float newMass){
		extraMass += newMass;
		updateAllCubes(newMass);
	}

	void updateExtraMass(float newMass){
		extraMass += newMass;
	}

}
