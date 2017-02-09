using UnityEngine;
using System.Collections;

public class CliqueQuadrado : MonoBehaviour {

	public GameObject Tabuleiro;

	// Facilitador para selecionar o controlador do jogo
	ControladorDoJogo GetControlador() {
		//GameObject go = GameObject.Find ("Tabuleiro");
		//return (ControladorDoJogo) go.GetComponent (typeof(ControladorDoJogo));
		return (ControladorDoJogo) Tabuleiro.GetComponent (typeof(ControladorDoJogo));
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	// Disparado quando clica com mouse no quadro
	// Necessario ter box collider para que funcione
	void OnMouseDown (){
		GetControlador().DoEfetuaJogada (name);
	}
}
