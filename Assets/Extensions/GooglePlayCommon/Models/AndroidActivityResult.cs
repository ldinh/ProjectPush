using UnityEngine;
using System.Collections;

public class AndroidActivityResult {

	protected AdroidActivityResultCodes _code;


	public AndroidActivityResult(string codeString) {
		_code = (AdroidActivityResultCodes) System.Convert.ToInt32(codeString);
	}


	public AdroidActivityResultCodes code {
		get {
			return _code;
		}
	}


	public bool IsSucceeded {
		get{
			if(code == AdroidActivityResultCodes.RESULT_OK) {
				return true;
			} else {
				return false;
			}
		}
	}

	public bool IsFailed {
		get{
			return !IsSucceeded;
		}
	}
}
