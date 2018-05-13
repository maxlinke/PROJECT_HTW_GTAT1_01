using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour {

	[SerializeField] Text scoreText;
	[SerializeField] Text highscoreText;
	int score;

	void Start(){
		score = 0;
		ResetScore();
	}
	
	void Update(){

	}

	public void ResetScore(){
		SaveHighscore();
		score = 0;
		UpdatesScoreDisplay();
		UpdateHighscoreDisplay();
	}

	public void AddToScore(int value){
		score += value;
		UpdatesScoreDisplay();
	}

	void UpdatesScoreDisplay(){
		scoreText.text = GetScoreString(score, 6);
	}

	void UpdateHighscoreDisplay(){
		int highscore = PlayerPrefManager.GetInt("game_highscore");
		highscoreText.text = GetScoreString(highscore, 6);
	}

	public void SaveHighscore(){
		if(score > PlayerPrefManager.GetInt("game_highscore")){
			PlayerPrefManager.SetInt("game_highscore", score);
		}
	}

	string GetScoreString(int value, int digits){
		string output = "";
		for(int i=0; i<digits; i++){
			int digit = (value / intPow(10, i)) % 10;
			output = digit + output;
		}
		return output;
	}

	int intPow(int number, int exponent){
		int output = 1;
		for(int i=0; i<exponent; i++){
			output *= number;
		}
		return output;
	}
}
