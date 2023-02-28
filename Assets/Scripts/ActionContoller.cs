using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionContoller : MonoBehaviour
{
    [SerializeField] GameObject messageOnTV;
    [SerializeField] VariableStorage chatMessage;
    // Телевизор
    [SerializeField] GameObject TV;
    Animator tvAnim;                                                        // Аниматор TV
    // Кот
    [SerializeField] GameObject Cat;
    Animator catsAnim;                                                      // Аниматор кота
    private Vector3 _startPos;                                              // Начальная точка кота при движении
    private Vector3 _endPos;                                                // Конечная точка кота при движении
    // Кошачий дом
    [SerializeField] GameObject CatsHome;                                            

    private void Start()
    {
        tvAnim = TV.GetComponent<Animator>();
        catsAnim = Cat.GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log("message = " + chatMessage.Message);
        Debug.Log("name = " + chatMessage.ChatName);
        if (chatMessage.Message.ToLower().IndexOf("!tv") > -1)               // Если есть !tv в команде, проверяет дальше, если нет, не проходится по остальным if'ам
        {
            switch (chatMessage.Message.ToLower())
            {
                case "!tv1":
                    tvAnim.Play("tv_news");
                    break;
                case "!tv2":
                    tvAnim.Play("tv_heart");
                    break;
                case "!tv3":
                    tvAnim.Play("tv_18");
                    break;
                case "!tv4":
                    tvAnim.Play("tv_twit");
                    break;
                case "!tv5":
                    tvAnim.Play("tv_frog");
                    break;
                case "!tvoff":
                    tvAnim.Play("tv_off");
                    break;
            }
        }

        if (chatMessage.Message.ToLower().IndexOf("!cat") > -1)
        {
            switch (chatMessage.Message.ToLower())
            {
                case "!cathome":
                    _startPos = new Vector3(Cat.transform.position.x, Cat.transform.position.y, 0);
                    _endPos = new Vector3(-1.48f, -1.71f, 0);
                    catsAnim.SetBool("isJump", true);
                    Cat.transform.position = Vector3.MoveTowards (_startPos, _endPos, Time.deltaTime*2.5f);
                    if (_startPos == _endPos)
                    {
                        catsAnim.SetBool("isJump", false);
                        Cat.SetActive(false);
                        CatsHome.GetComponent<Animator>().SetBool("isHome", true);
                    }
                    break;
            }
        }
    }
}
