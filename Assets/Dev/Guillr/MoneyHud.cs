using UnityEngine;
using System.Collections;

public class MoneyHud : MonoBehaviour {

	public GUIText Money;
	long lMoney = 0;
	float time = 0;

	void Update()
	{
		time = Time.deltaTime;
		lMoney++;
	}

	void OnGUI()
	{
		Money.text = lMoney.ToString();
	}
}
