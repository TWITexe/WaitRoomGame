using UnityEngine;

public class CatController : MonoBehaviour
{
    // [SerializeField] private GameObject messageOnTV;
    // Скрипт банка сообщений в чате
    [SerializeField] private VariableStorage chatMessage;
    // Главный герой
    [SerializeField] private GameObject cat;
    private bool catReady = true;
    // Аниматор TV
    [SerializeField] private Animator tvAnim;
    // Аниматор кота
    [SerializeField] private Animator catsAnim;                                                      
    // Начальная точка кота при движении
    private Vector3 _defaultStartPos = new Vector3(5.3f, -2f, 0);
    private Vector3 _startPos;                    
    // Конечная точка кота при движении
    private Vector3 _endPos = new Vector3(5.3f, -2f, 0);                                                       
    // Кошачий дом
    [SerializeField] private Animator catsHome;                                            

    private void Start()
    {
        _startPos = _defaultStartPos;
    }

    private void Update()
    {
        Debug.Log("message = " + chatMessage.Message);
        Debug.Log("name = " + chatMessage.ChatName);

        // Если есть "!tv" в чате, проверяет дальше, если нет, не проходится по остальным if'ам
        if (chatMessage.Message.ToLower().LastIndexOf("!tv") > -1)               
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
                    if (_startPos != _defaultStartPos && catReady)
                    {
                        ReadyToStart();
                    }
                    else if (_startPos == _defaultStartPos && catReady)
                    {   
                        //Кот не готов к следующим имзенениям, пока не достигнет цели.
                        catReady = false;
                        cat.SetActive(true);
                        CatLetsGo(new Vector3(-1.48f, -1.71f, 0), 2.5f);
                        if (_startPos == _endPos)
                        {
                            catsAnim.SetBool("isJump", false);
                            catsHome.SetBool("isHome", true);
                            cat.SetActive(false);
                            catReady = true;
                        }
                    }

                    break;
                case "!catchair":

                    if (_startPos != _defaultStartPos && catReady)
                    {
                        ReadyToStart();
                    }
                    else if (_startPos == _defaultStartPos && catReady)
                    {   
                        //Кот не готов к следующим имзенениям, пока не достигнет цели.
                        catReady = false;
                        cat.SetActive(true);
                        CatLetsGo(new Vector3(3f, -1.3f, 0), 2.5f);
                        if (_startPos == _endPos)
                        {
                            catsAnim.SetBool("isJump", false);
                            catsAnim.SetBool("isChair", true);
                            catReady = true;
                        }
                    }
                    break;
                    
            }
        }
    }
    private void ReadyToStart()                     // Стартовая позиция перед всеми действиями
    {
        Debug.Log("go ready");
        catsHome.SetBool("isHome", false);
        
        CatLetsGo(new Vector3(5.3f, -2f, 0), 2.0f);
        cat.GetComponent<SpriteRenderer>().flipX = true;
        cat.transform.position = Vector3.MoveTowards (_startPos, _endPos, Time.deltaTime*2.5f);

    }
    private void CatLetsGo(Vector3 endPos, float speed)
    {
        _startPos = new Vector3(cat.transform.position.x, cat.transform.position.y, 0);
        _endPos = endPos;
        catsAnim.SetBool("isJump", true);
        cat.transform.position = Vector3.MoveTowards(_startPos, _endPos, Time.deltaTime * speed);
    }
    
}
