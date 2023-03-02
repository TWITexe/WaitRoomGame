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
    // Кот (Главный герой)
    [SerializeField] GameObject Cat;
    Animator catsAnim;                                                      // Аниматор кота
    private Vector3 _startPos;                                              // Начальная точка кота при движении
    private Vector3 _endPos;                                                // Конечная точка кота при движении
    private bool catIsReady, catInHome,catOnChair = false;        
    // Кошачий дом
    [SerializeField] GameObject CatsHome;                                            

    private void Start()
    {
        tvAnim = TV.GetComponent<Animator>();
        catsAnim = Cat.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Debug.Log("message = " + chatMessage.Message);
        Debug.Log("name = " + chatMessage.ChatName);
        if (_endPos == _startPos)
            Cat.GetComponent<SpriteRenderer>().flipX = false;
        
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

        if (chatMessage.Message.ToLower().LastIndexOf("!cat") > -1)
        {
            switch (chatMessage.Message.ToLower())
            {

                case "!cathome":
                    if (!catInHome)
                    {
                        ReadyToStart();
                        _startPos = new Vector3(Cat.transform.position.x, Cat.transform.position.y, 0);
                        _endPos = new Vector3(-1.48f, -1.71f, 0);
                        catsAnim.SetBool("isJump", true);
                        Cat.transform.position = Vector3.MoveTowards(_startPos, _endPos, Time.deltaTime * 2.5f);
                        if (_startPos == _endPos)
                        {
                            catsAnim.SetBool("isJump", false);
                            Cat.SetActive(false);
                            CatsHome.GetComponent<Animator>().SetBool("isHome", true);
                            catInHome = true;
                        }
                    }
                    break;
                case "!catchair":
                    if (!catInHome)
                    {
                        if (!catIsReady)
                        {
                            ReadyToStart();  
                        }
                        else if (catIsReady)
                        {
                            _startPos = new Vector3(Cat.transform.position.x, Cat.transform.position.y, 0);
                            _endPos = new Vector3(3f, -1.3f, 0);
                            catsAnim.SetBool("isJump", true);
                            Cat.transform.position = Vector3.MoveTowards(_startPos, _endPos, Time.deltaTime * 2.5f);
                            if (_startPos == _endPos)
                            {
                                catsAnim.SetBool("isJump", false);
                                Cat.SetActive(false);
                                CatsHome.GetComponent<Animator>().SetBool("isHome", true);
                                catOnChair = true;
                            }
                        }
                    }

                    break;
                    
            }
        }
    }
    void ReadyToStart()             // Стартовая позиция перед всеми действиями
    {
        catInHome = false;
        Debug.Log("go ready");
        Cat.SetActive(true);
        _startPos = new Vector3(Cat.transform.position.x, Cat.transform.position.y, 0);
        _endPos = new Vector3(5.3f, -2f, 0);
        Cat.GetComponent<SpriteRenderer>().flipX = true;
        Cat.transform.position = Vector3.MoveTowards (_startPos, _endPos, Time.deltaTime*2.5f);
        if (_endPos == _startPos)
        {
            catIsReady = true;
        }
    }
    
}
