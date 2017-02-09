using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControladorDoJogo : MonoBehaviour {

	public class Quadrado
	{
		public int Valor;
		public string Nome;

		public bool ComparaValor(int ValorParaComparacao, int ValorDefault) {
			int ValorTratado = Valor;
			if (Valor == 0) 
				ValorTratado = ValorDefault;
			else
				ValorTratado = Valor;

			return ValorTratado == ValorParaComparacao;
		}
	}

	// Propriedade que indica o jogador da vez (1 ou 2)
	private int JogadorDaVez;

	// Propriedade que tera a imagem vazia
	public Sprite SpriteBranco;
	
	// Propriedade que tera a imagem da Bola
	public Sprite SpriteBola;
	
	// Propriedade que tera a imagem do Xis
	public Sprite SpriteXis;

	// Propriedade que indica que o jogo esta encerrado
	private bool JogoEncerrado;

	// Tabuleiro 3x3
	private Quadrado[,] MatrizTabuleiro;

	// Propriedade que referencia component Text que apresentara o texto do turno
	public Text textoTurno;

	// Propriedade que referencia component Text que apresentara o texto da vitoria
	public Text textoVitoria;

	#region Eventos
	// Use this for initialization
	void Start () {
		JogadorDaVez = 1;
		SetTextoTurno ();
		JogoEncerrado = false;
		MatrizTabuleiro = new Quadrado[3, 3];
		textoVitoria.text = "";

		// Inicializa matriz do jogo
		Quadrado q1 = new Quadrado ();
		q1.Valor = 0;
		q1.Nome = "QuadradoA1";
		MatrizTabuleiro [0, 0] = q1;

		Quadrado q2 = new Quadrado ();
		q2.Valor = 0;
		q2.Nome = "QuadradoA2";
		MatrizTabuleiro [1, 0] = q2;

		Quadrado q3 = new Quadrado ();
		q3.Valor = 0;
		q3.Nome = "QuadradoA3";
		MatrizTabuleiro [2, 0] = q3;

		Quadrado q4 = new Quadrado ();
		q4.Valor = 0;
		q4.Nome = "QuadradoB1";
		MatrizTabuleiro [0, 1] = q4;
		
		Quadrado q5 = new Quadrado ();
		q5.Valor = 0;
		q5.Nome = "QuadradoB2";
		MatrizTabuleiro [1, 1] = q5;
		
		Quadrado q6 = new Quadrado ();
		q6.Valor = 0;
		q6.Nome = "QuadradoB3";
		MatrizTabuleiro [2, 1] = q6;

		Quadrado q7 = new Quadrado ();
		q7.Valor = 0;
		q7.Nome = "QuadradoC1";
		MatrizTabuleiro [0, 2] = q7;
		
		Quadrado q8 = new Quadrado ();
		q8.Valor = 0;
		q8.Nome = "QuadradoC2";
		MatrizTabuleiro [1, 2] = q8;
		
		Quadrado q9 = new Quadrado ();
		q9.Valor = 0;
		q9.Nome = "QuadradoC3";
		MatrizTabuleiro [2, 2] = q9;

		DoDesenhaTabuleiro();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Chamado ao criar a tela
	void OnGUI()
	{
		//makes a GUI button at coordinates 10, 100, and a size of 200x40
		if(GUI.Button(new Rect (10,100,200,40),"Restart"))
		{
			//Loads a level
			Application.LoadLevel(Application.loadedLevel);
      	}
	}
	#endregion

	#region Metodos publicos
	public void DoEfetuaJogada (string nomePeca) {
		// Nao permite jogar depois que o jogo estiver encerrado
		if (JogoEncerrado) {
			return;
		}

		// Pega objeto na posiçao
		Quadrado q1 = GetQuadradoPeloNome (nomePeca);

		// Se quadro esta vazio, entao o movimento e valido
		if (q1 != null && q1.Valor == 0) {
			// Marca jogada
			q1.Valor = JogadorDaVez; 
			// Redesenha tabuleiro
			DoDesenhaTabuleiro();

			// Verifica se jogo acabou
			if (FimJogo()) {
				DoVitoriaJogadorDaVez();
				//DoReiniciaJogo();
			}
			else if (JogoEmpatou()) {
				DoEmpate();
				//DoReiniciaJogo();
			}				

			if (!JogoEncerrado) {
				// Muda turno
				DoMudaTurno();
			}
		}
	}
	#endregion

	#region Metodos privados
	// Faz um refresh completo no tabuleiro atualizando os quadrados de acordo com a matriz
	void DoDesenhaTabuleiro() {
		for (var i=0;i<=2;i++) {
			for (var j=0;j<=2;j++) {
				Quadrado q1 = MatrizTabuleiro [i, j];

				GameObject go = GameObject.Find (q1.Nome);
				SpriteRenderer spriteRenderer = (SpriteRenderer) go.GetComponent (typeof(SpriteRenderer));

				switch (q1.Valor) {
					case 1: {
						spriteRenderer.sprite = SpriteXis;
						break;
					}
					case 2: {
						spriteRenderer.sprite = SpriteBola;
						break;
					}
					default: {
						spriteRenderer.sprite = SpriteBranco;
						break;
					}
				}
			}
		}
	}

	Quadrado GetQuadradoPeloNome(string nomePeca) {
		for (var i=0; i<=2; i++) {
			for (var j=0; j<=2; j++) {
				Quadrado q1 = MatrizTabuleiro [i, j];
				if (q1.Nome.Equals(nomePeca)) {
					return q1;
				}
			}
		}
		return null;
	}

	bool FimJogo() {
		// Verifica se jogador da vez possui uma das combinaçoes vitoriosas
		if (MovimentosDeVitoria (JogadorDaVez)) {
			JogoEncerrado = true;
			return true;
		} else {
			return false;
		}
	}

	bool MovimentosDeVitoria(int Jogador, int JogadorCoalesce = 0) {
		// * * * 
		// 0 0 0
		// 0 0 0
		if (MatrizTabuleiro [0, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [0, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [0, 2].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// * 0 0 
		// 0 * 0
		// 0 0 *
		if (MatrizTabuleiro [0, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [2, 2].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// * 0 0 
		// * 0 0
		// * 0 0
		if (MatrizTabuleiro [0, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [2, 0].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// 0 * 0 
		// 0 * 0
		// 0 * 0
		if (MatrizTabuleiro [1, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 2].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// 0 0 * 
		// 0 * 0
		// * 0 0
		if (MatrizTabuleiro [2, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [0, 2].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// 0 0 * 
		// 0 0 *
		// 0 0 *
		if (MatrizTabuleiro [2, 0].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [2, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [2, 2].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// 0 0 0
		// * * *
		// 0 0 0
		if (MatrizTabuleiro [0, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 1].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [2, 1].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		// 0 0 0
		// 0 0 0
		// * * *
		if (MatrizTabuleiro [0, 2].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [1, 2].ComparaValor(Jogador, JogadorCoalesce) &&
		    MatrizTabuleiro [2, 2].ComparaValor(Jogador, JogadorCoalesce))
			return true;
		return false;
	}


	bool JogoEmpatou() {
		// Enquanto existe uma posiçao que pode dar vitoria totalmente vazia
	    // o jogo pode ser ganho por alguem
		if (MovimentosDeVitoria (JogadorDaVez, JogadorDaVez)) {
			return false;
		} else {
			JogoEncerrado = true;
			return true;
		}
	}

	void SetTextoTurno() {
		if (JogadorDaVez == 1) 
			textoTurno.text = "Vez do Jogador 1";
		else 
			textoTurno.text = "Vez do Jogador 2";
	}

	void DoMudaTurno () {
		if (JogadorDaVez == 1)
			JogadorDaVez = 2;
		else
			JogadorDaVez = 1;
		SetTextoTurno ();
	}

	void DoVitoriaJogadorDaVez() {
		if (JogadorDaVez == 1) 
			textoVitoria.text = "Jogador 1 venceu!";
		else 
			textoVitoria.text = "Jogador 2 venceu!";
	}

	void DoEmpate() {
		textoVitoria.text = "Jogo empatou!";
	}

	void DoReiniciaJogo() {
		//if (GUI.Button (new Rect (Screen.width - ((Screen.width / 60)) - (Screen.width / 20), Screen.height / 60, Screen.width  / 20, Screen.width / 20), "Reiniciar"))
		//if (GUI.Button (new Rect(-300, -100, 100, 200), "Reiniciar"))
		//{
			Application.LoadLevel(Application.loadedLevel);
		//}
	}
	#endregion 

}
