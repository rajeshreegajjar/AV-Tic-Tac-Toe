﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class hindiGlyph : MonoBehaviour {

	//public Text hindiText;
    public TextMeshProUGUI hindiText;

    // Use this for initialization
    void Start () {
		hindiText.text = UnicodeToKrutidev.UnicodeToKrutiDev(hindiText.text);
	}

}
