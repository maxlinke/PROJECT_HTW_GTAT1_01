using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

		[Header("Prefabs")]
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject playerBulletPoolPrefab;

		[Header("Scene References")]
	[SerializeField] GameObject foreground;
	[SerializeField] BoxCollider playArea;
	[SerializeField] GameObject playerSpawn;
	[SerializeField] RestartScreenScript restartScreen;
	[SerializeField] ScoreScreenScript scoreScreen;
	[SerializeField] EnemySpawnerScript enemySpawner;

		[Header("Game Settings")]
	[SerializeField] int playerLives;

	bool gameBeginning;
	BulletPoolScript playerBulletPool;
	PlayerController[] players;

	bool gameOver;
	bool gameOverCompletely;

	void Start(){
		//string[] gamepads = Input.GetJoystickNames();	//TODO do something with these
		//foreach(string s in gamepads){
		//	Debug.Log("> " + s);
		//}
		gameOver = true;
		gameOverCompletely = true;
		gameBeginning = true;
		GameObject bulletPool = Instantiate(playerBulletPoolPrefab) as GameObject;
		playerBulletPool = bulletPool.GetComponent<BulletPoolScript>();
		LoadPlayers();
		for(int i=0; i<players.Length; i++){
			players[i].SetRegularComponentsActive(false);
		}

		restartScreen.gameController = this;
		restartScreen.gameObject.SetActive(true);

		scoreScreen.gameObject.SetActive(false);

		enemySpawner.gameController = this;
		enemySpawner.enabled = false;

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Mouse0)){
			Cursor.lockState = CursorLockMode.Locked;
		}
		if(Input.GetKeyDown(KeyCode.Escape)){
			scoreScreen.SaveHighscore();
			Application.Quit();
		}
		if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.R)){
			PlayerPrefs.DeleteAll();
		}
	}

	public void Score(int value){
		if(!gameOver) scoreScreen.AddToScore(value);		//if i weren't lazy, i'd replace this with a static scoresystem class that does the scoring, instead of everything referencing the gamecontroller
	}

	public void StartGame(){
		for(int i=0; i<players.Length; i++){
			players[i].SetRegularComponentsActive(true);
		}
		enemySpawner.enabled = true;
		gameOver = false;
		gameOverCompletely = false;
	}

	public bool GetGameOverCompletely(){
		return gameOverCompletely;
	}

	public void RestartGame(){
		for(int i=0; i<players.Length; i++){
			players[i].transform.localPosition = playerSpawn.transform.localPosition;
			players[i].LevelResetInit();
			players[i].SetRegularComponentsActive(true);
		}
		scoreScreen.gameObject.SetActive(true);
		scoreScreen.ResetScore();
		enemySpawner.enabled = true;
		gameOver = false;
		gameOverCompletely = false;
	}

	public void GameOver(){
		for(int i=0; i<players.Length; i++){
			players[i].SetRegularComponentsActive(false);
		}
		gameOver = true;
		enemySpawner.enabled = false;
		StartCoroutine(OpenRestartScreenAfterWait());
	}

	IEnumerator OpenRestartScreenAfterWait(){
		yield return new WaitForSeconds(2f);
		restartScreen.gameObject.SetActive(true);
		gameOverCompletely = true;
	}

	void LoadPlayers(){
		int playerCount = PlayerPrefManager.GetInt("game_playercount");
		if(playerCount != 1 && playerCount != 2){
			throw new UnityException("unsupported playercount");
		}
		players = new PlayerController[playerCount];
		for(int i=0; i<players.Length; i++){
			int playerNumber = i+1;
			players[i] = InstantiatePlayer(playerNumber);
			players[i].SetFurtherInitData(playerNumber, playerLives, this, playerBulletPool, playArea);
			players[i].name = "Player " + playerNumber;
		}
		if(playerCount == 2){
			players[0].transform.localPosition += Vector3.left;
			players[1].transform.localPosition += Vector3.right;
		}
	}

	InputType ParseInputType(string name){
		return (InputType)(System.Enum.Parse(typeof(InputType), name));
	}

	PlaneType ParsePlaneType(string name){
		return (PlaneType)(System.Enum.Parse(typeof(PlaneType), name));
	}

	PlayerController InstantiatePlayer(int playerNumber){
		GameObject player = Instantiate(playerPrefab) as GameObject;	//TODO space them apart, if multiple players?
		player.transform.parent = foreground.transform;
		player.transform.localRotation = Quaternion.identity;
		player.transform.localPosition = playerSpawn.transform.localPosition;
		PlayerController pc = player.GetComponent<PlayerController>();
		char numberChar = playerNumber.ToString().ToCharArray()[0];
		string inputKey = "game_p#_input".Replace('#', numberChar);
		string planeKey = "game_p#_plane".Replace('#', numberChar);
		InputType input = ParseInputType(PlayerPrefManager.GetString(inputKey));
		PlaneType plane = ParsePlaneType(PlayerPrefManager.GetString(planeKey));
		pc.Initialize(input, plane);
		return pc;
	}

}
