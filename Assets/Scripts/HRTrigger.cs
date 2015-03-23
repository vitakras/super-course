using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class HRTrigger : MonoBehaviour {

	SerialPort sp = new SerialPort("/dev/tty.usbmodem1411", 115200);
	public int threshold = 105;
	public int targetBeat = 100;
	
	public int heartRate = 0;
	private bool isFirst;
	public GameObject prefab;
	Vector3 playerPos;
	public GameObject player;
	Vector3 prefabPos;
	Quaternion rotation;
	int count;
	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 1;
		isFirst = true;
		count = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (sp.IsOpen) 
		{
			try
			{
				//MoveObject(sp.ReadByte());
				heartRate = sp.ReadByte();
				if(isFirst)
				{
					if(heartRate <= threshold)
					{
						isFirst = false;

					}
				}else{
					if((heartRate >= targetBeat)&&count==0)
					{
						playerPos = player.transform.position;
						prefabPos = playerPos;
						prefabPos.x = prefabPos.x + 10;
						prefabPos.z = prefabPos.z + 2;
						rotation = Quaternion.identity;
						rotation.y = 90;
						Instantiate(prefab,prefabPos, Quaternion.identity);
						count++;
					}
				}
			}catch (System.Exception)
			{
				
			}
		}
	
	}
}
