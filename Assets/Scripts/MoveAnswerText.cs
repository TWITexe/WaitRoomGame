using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MoveAnswerText : MonoBehaviour
{
    private bool move;
    private Vector2 randomVector;
    private float transparencyText = 1;
    private void Update()
    {
        if(!move) return;
        transform.Translate(randomVector * Time.deltaTime * 0.2f);
        StartCoroutine(TransparencyText());
    }

    public void StartMotion(string answer)
    {
        gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);
        transparencyText = 1;
        transform.localPosition = Vector2.zero;
        Debug.Log("answer = " + answer);
        GetComponent<Text>().text = answer;
        randomVector = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        move = true;
    }

    IEnumerator TransparencyText()
    {
        yield return new WaitForSeconds(1);
        transparencyText -= 0.01f;
        gameObject.GetComponent<Text>().color = new Color(1f, 1f, 1f, transparencyText);
        
    }


}
