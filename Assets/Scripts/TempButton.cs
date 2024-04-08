using UnityEngine;
using System.Collections;
 
public class Tempbutton: MonoBehaviour {
 
void Update(){
if(Input.GetKeyDown(KeyCode.B)){
Application.LoadLevel("BlackJack");
}
}
}