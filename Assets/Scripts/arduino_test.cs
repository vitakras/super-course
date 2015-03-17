using UnityEngine;
using System.Collections;
using System.IO.Ports;
public class arduino_test : MonoBehaviour {

	public float speed;

	private float amountToMove;

	SerialPort sp = new SerialPort("/dev/tty.usbmodem1411", 9600);

	public Light RedLight;
	public Light BlueLight;
	public int Number = 1;

	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 1;

		Number = 1;
		BlueLight.GetComponent<Light>().intensity = 0;
		RedLight.GetComponent<Light>().intensity = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//amountToMove = speed * Time.deltaTime;

		if (sp.IsOpen) 
		{
			try
			{
				//MoveObject(sp.ReadByte());
				print(sp.ReadByte());
				if(sp.ReadByte() == 2)
				{
					flashing();
				}
				else
				{
					BlueLight.GetComponent<Light>().intensity = 0;
					RedLight.GetComponent<Light>().intensity = 0;
				}
			}catch (System.Exception)
			{

			}
		}
	}

	void MoveObject(int Direction)
	{
		/*if (Direction == 1) 
		{
			transform.Translate(Vector3.left * amountToMove, Space.World);
		}
		if (Direction == 2) 
		{
			transform.Translate(Vector3.right * amountToMove, Space.World);
		}*/
	}

	void flashing()
	{
		if (Number == 1)
		{
			BlueLight.GetComponent<Light>().intensity = 0;
			RedLight.GetComponent<Light>().intensity = 1.5f;
			StartCoroutine(waitforred());
		}
		if (Number == 2)
		{
			RedLight.GetComponent<Light>().intensity = 0;
			BlueLight.GetComponent<Light>().intensity = 1.5f;
			StartCoroutine(waitforblue());
		}
	}

	IEnumerator waitforred()
	{
		yield return new WaitForSeconds (0.2f);
		Number = 2;
	}
	IEnumerator waitforblue()
	{
		yield return new WaitForSeconds (0.2f);
		Number = 1;
	}
}
