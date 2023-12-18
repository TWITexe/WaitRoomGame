using System.Timers;
using UnityEngine;
public class CatController : MonoBehaviour
{
    [SerializeField] private GameObject timerObject;
    //[SerializeField] private GameObject eventManager;
    // Скрипт банка сообщений в чате для отслеживания команд
    [SerializeField] private VariableStorage chatMessage;
    // Главный герой
    [SerializeField] private GameObject cat;
    [SerializeField] private float catSpeed;
    // Готовность героя к передвижению к новой точке.
    private bool catReady = true;
    // Готовность к "мяу". p.s.Один "мяу" можно использовать только между действиями.
    private bool meowReady = true;
    // Экземпляр класса для работы со звуками кота))0)
    [SerializeField] private CatVoiceManager catVoiceManager;
    // Аниматор TV
    [SerializeField] private Animator tvAnim;
    // Аниматор кота
    [SerializeField] private Animator catsAnim;                                                      
    // Начальная точка кота при движении
    private Vector3 _defaultStartPos = new Vector3(5.3f, -2f, 0);
    private Vector3 _startPos;                    
    // Конечная точка кота при движении
    private Vector3 _endPos;                                                       
    // Кошачий дом
    [SerializeField] private Animator catsHome;  
    // Прошлая и текущая команды для отслеживания повторений.
    private string lastCommand;
    private string nowCommand = "start";
    // Экземпляр класса таймер.
    private TimerForChatСommands timerForChatСommands;
   

    private void Start()
    {
        timerForChatСommands = timerObject.GetComponent<TimerForChatСommands>();
        _startPos = _defaultStartPos;
    }

    private void Update()
    {
        
        
        Debug.Log("CatReady = " + catReady + "| timerOn = " + timerForChatСommands.TimerOn + "| MeowReady =" + meowReady);
        if (VariableStorage.Message.ToLower().LastIndexOf("!meow") > -1 && !timerForChatСommands.TimerOn && meowReady && catReady)
        {
            nowCommand = VariableStorage.Message.ToLower();
            timerForChatСommands.StartTimer(5);
            catVoiceManager.CatMeows();
            meowReady = false;
        }
        // Если есть "!tv" в чате, проверяет дальше, если нет, не проходится по остальным if'ам
        else if(VariableStorage.Message.ToLower().LastIndexOf("!tv") > -1 && !timerForChatСommands.TimerOn && catReady)               
        {
            nowCommand = VariableStorage.Message.ToLower();
            SwitchTVChannel(nowCommand);
        }
        else if (VariableStorage.Message.ToLower().LastIndexOf("!cat") > -1)
        {
            // Записываем новую команду на движенние кота, чтобы при следующем тике сравнить её с предыдущей
            nowCommand = VariableStorage.Message.ToLower();
            CatCommandsForTowardsTheTarget(nowCommand);
        }

        // Смотрим новую команду из чата на движение кота, запомним её, чтобы постоянно не выполнять действие одной и той же команды.
        ComparisonLatestAndNewCommand();

    }
    // Перемещение кота в стартовую позицию ( начальную, перед всеми действиями передвижения)
    private void ReadyToStart()                     
    {
        Debug.Log("Starting");
        timerForChatСommands.StartTimer(5);
        catReady = false;
        cat.SetActive(true);
        catsHome.SetBool("isHome", false);
        CatLetsGo(new Vector3(5.3f, -2f, 0), catSpeed);
        cat.GetComponent<SpriteRenderer>().flipX = true;
        if (_startPos == _endPos)
        {
            cat.GetComponent<SpriteRenderer>().flipX = false;
            catsAnim.SetBool("isJump", false);
            catReady = true;
            _endPos = new Vector3(0, 0, 0);
        }

    }
    private void CatLetsGo(Vector3 endPos, float speed)
    {

        _startPos = new Vector3(cat.transform.position.x, cat.transform.position.y, 0);
        _endPos = endPos;
        catsAnim.SetBool("isJump", true);
        cat.transform.position = Vector3.MoveTowards(_startPos, _endPos, Time.deltaTime * speed);
    }
    
    private void CatCommandsForTowardsTheTarget(string _nowCommand)
    {
        Debug.Log("Now CatMoveCommand = " + nowCommand);
        //Debug.Log(_startPos + " != " + _endPos);
        switch (_nowCommand)
        {
            case "!cathome":
                Debug.Log("LastCommand = " + lastCommand);
                Debug.Log("nowCommand = " + nowCommand);
                if (_startPos != _endPos && catReady)
                {
                    CatLetsGo(new Vector3(-1.48f, -1.71f, 0), catSpeed);
                    if (_startPos == _endPos)
                    {
                        catsAnim.SetBool("isJump", false);
                        catsHome.SetBool("isHome", true);
                        cat.SetActive(false);
                        
                    }
                }
                else if ( nowCommand != lastCommand || !catReady ) // Если сработала другая команда и кот не готов к ней, то ставим кота в готовность.
                {
                    ReadyToStart();
                }
                break;
                
            case "!catchair":
                Debug.Log("LastCommand = " + lastCommand);
                Debug.Log("nowCommand = " + nowCommand);
                if (_startPos != _endPos && catReady)
                {
                    CatLetsGo(new Vector3(3f, -1.3f, 0), catSpeed);
                    if (_startPos == _endPos)
                    {
                        catsAnim.SetBool("isJump", false);
                        catsAnim.SetBool("isChair", true);
                    }
                }
                else if ( nowCommand != lastCommand || !catReady)
                {
                    ReadyToStart();
                }
                break;
                    
        }
        
    }

    private void SwitchTVChannel(string _nowCommand)
    {
        switch (_nowCommand)
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

    private void RefreshMeow(bool _meowReady)
    {
        if (!_meowReady)
            meowReady = !_meowReady;
    }

    private void ComparisonLatestAndNewCommand()
    {
        // сравнение команд и запись последней в "lastCommand".
        if ((lastCommand != nowCommand) && VariableStorage.Message.ToLower().LastIndexOf("!cat") > -1)
        {
            RefreshMeow(meowReady);
            lastCommand = VariableStorage.Message.ToLower();
            
        }
    }


}
