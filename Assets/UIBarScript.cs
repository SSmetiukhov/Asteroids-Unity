using UnityEngine;
using UnityEngine.UI;

public class UIBarScript : MonoBehaviour
{
    public Text ScoreUIText;

    public Text BestUIText;

    public Text LivesUIText;
    

    // Update is called once per frame
    void Update()
    {
        ScoreUIText.text = "SCORE: " + GameManagerScript.GetScore();
        BestUIText.text = "BEST SCORE: " + GameManagerScript.GetBest();
        LivesUIText.text = "LIVES: " + GameManagerScript.GetLives();
    }
}
