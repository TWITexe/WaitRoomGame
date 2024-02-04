using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MoveAnswerText : MonoBehaviour
{
    private bool move;
    private Vector2 randomVector;
    private void Update()
    {
        if(!move) return;
        transform.Translate(randomVector * Time.deltaTime);
    }

    public void StartMotion(string answer)
    {
        transform.localPosition = Vector2.zero;
        Debug.Log("answer = " + answer);
        GetComponent<Text>().text = answer;
        randomVector = new Vector2(Random.Range(-5, 5), Random.Range(1, 1.5f));
        move = true;
    }
}
