using UnityEngine;
using System.Collections;

public class HomeBaseController : MonoBehaviour {

	public HomeBase HomeBaseData { get; private set; }

	public void SetHomeBaseData(HomeBase data) {
		this.HomeBaseData = data;
	}

}
