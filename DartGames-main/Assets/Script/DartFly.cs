using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public enum Mode { Wait,Ready,ShowResult,WaitScore,PowerGauge,Aim,Throw};

public class DartFly : MonoBehaviour
{
    public Mode currentMode = Mode.PowerGauge;
    public GameObject dartPrefab,cursorPrefab,currentDart,cursor;
    public Slider powerGauge;
    public const int powerGaugeMax=200;
    public const float powerGaugeSpeed = 3f;
    public const int maxMeicyuu =60;
    public const int minMeicyuu = 40;

    public bool clickInside = false;

    public bool powerIsON;
    public bool powerIsUp = true;
    Vector3 targetPos;
    Vector3 startPos = new Vector3(0, 0, -8);
    private Vector3 direction;

    private float cursorShakeSpeed =0.5f;
    private float cursorShakeAmount = 0.3f;

    private float TouchPadXMin;
    private float TouchPadXMax;
    private float TouchPadYMin;
    private float TouchPadYMax;
    private Vector3 handPos,clickPos;


    float power;

    public GameObject hand;
    private static Vector3 handDefaultPos;

    //ズーム効果のカメラと位置
    public GameObject cameraPrefab;
    private Vector3 zoomCameraPos = new Vector3(0.3f, 0, -2.11f);
    public bool waitCameraAnim = false;

    private Coroutine powerGcoroutine;
    // Start is called before the first frame update
    void Start()
    {
        currentMode = Mode.Wait;
        powerGauge.maxValue = 100;
        powerGauge.value = 0;
        handDefaultPos = hand.transform.position;
        
        


    }
    /// <summary>
    /// Create Dart At Position
    /// </summary>
    /// <param name="targetPos"> vector3 target position </param>
    private void CreateDart(Vector3 targetPos)
    {
        currentDart= Instantiate(dartPrefab, startPos, Quaternion.identity);
        SpawnCursor();
    }
    // touch pad area
    private void CalculateTouchPad()
    {
        TouchPadXMax = Screen.width * 9.5f / 10;
        TouchPadXMin = Screen.width * 0.5f / 10;
        TouchPadYMax = Screen.height * 6 / 10;
        TouchPadYMin = Screen.height * 1 / 10;
    }

    //Aim shake program
    private void AimShake()
    {
        CalculateTouchPad();
        targetPos = Input.mousePosition;

        //タッチ制限----------
       if(targetPos.x > TouchPadXMax)
        {
            targetPos.x = TouchPadXMax;
        }else if (targetPos.x < TouchPadXMin)
        {
            targetPos.x = TouchPadXMin;
        }
        if (targetPos.y > TouchPadYMax)
        {
            targetPos.y = TouchPadYMax;
        }
        else if (targetPos.y < TouchPadYMin)
        {
            targetPos.y = TouchPadYMin;
        }
      
        targetPos.z = 10;
        Vector3 cursorPos= Camera.main.ScreenToWorldPoint(targetPos);
        //-----------------------
        //hand.GetComponent<Animation>().Play();
        hand.GetComponent<Animation>().Stop();
       // float xdif = (clickPos.x - targetPos.x) / 100;
       // float ydif = (clickPos.y - targetPos.y) / 100;

        Vector3 newHandPos = new Vector3(cursorPos.x+5, cursorPos.y-10, handPos.z);

        hand.transform.position = newHandPos;
        cursorPos.y += 4;
        cursor.transform.position = cursorPos;
        int randomizer= Random.Range(0, 1);
        float zure = 0f;
        if (randomizer == 0)
        {
            zure = Mathf.Sin(Time.time * cursorShakeSpeed) * cursorShakeAmount;
        }
        else
        {
            zure = Mathf.Cos(Time.time * cursorShakeSpeed) * cursorShakeAmount;
        }
        targetPos = new Vector3(cursor.transform.position.x + zure, cursor.transform.position.y + zure, cursor.transform.position.z);
        cursor.transform.position = targetPos;
    }

    //while aiming spawn target cursor
    private void SpawnCursor()
    {

        // cursor = Instantiate(cursorPrefab, Camera.main.ScreenToWorldPoint(targetPos), Quaternion.identity);
        cursor.transform.position = Camera.main.ScreenToWorldPoint(targetPos);
        cursor.gameObject.SetActive(true);

    }


    //throw dart
    private void ShotDart()
    {
        powerIsON = false;
       
        currentDart.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
        direction = (targetPos - startPos);
        hand.GetComponent<Animation>().Stop();
        hand.GetComponent<Animation>().Play("HandPrepare");

        //test 100% meicyuu
       // currentDart.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(direction * powerGaugeMax);
      // power = powerGaugeMax;
        
        //真ん中の当たり
        if (powerGauge.value >minMeicyuu && powerGauge.value < maxMeicyuu)
        {
            currentDart.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(direction * powerGaugeMax);
            power = powerGaugeMax;
        }
        else if(powerGauge.value >=maxMeicyuu)
        {
            //強すぎる
            //上にずれる
            if (powerGauge.value >= maxMeicyuu + 10)
            {
                direction.y += ((powerGauge.value - maxMeicyuu) / 50);
                //ランダム右左にずれる
                direction.x += (Random.Range(-10f, 10f) /100);
            }
            else
            {
                direction.y += ((powerGauge.value - maxMeicyuu) / 30);
                //ランダム右左にずれる
                direction.x += (Random.Range(-10f, 10f) / 10);
            }

            power = powerGaugeMax;
            currentDart.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(direction * powerGaugeMax);
        }else if(powerGauge.value <= minMeicyuu)
        {
            //弱すぎる時普通に届かない
            power = powerGaugeMax * (powerGauge.value +30)/ 100;
            currentDart.transform.GetChild(0).GetComponent<Rigidbody>().AddForce(direction * power);
        }
        


    }

    //reset hand animation to default sprite
    public void HandResetAnim()
    {
        hand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/hand_1") as Sprite;
        hand.transform.position = handDefaultPos;
        clickInside = false;
    }

    //if dart enter collision zone
    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.tag == "IsTarget" && !collision.gameObject.GetComponent<Dart>().IsOnBoard())
        {
            collision.gameObject.GetComponent<Dart>().IsOnBoard(true);
            collision.gameObject.GetComponentInParent<Rigidbody>().velocity = new Vector3(0,0,0);
            collision.gameObject.GetComponentInParent<Rigidbody>().useGravity = false;
            collision.gameObject.GetComponentInParent<Rigidbody>().isKinematic=true;
            collision.gameObject.GetComponentInChildren<CircleCollider2D>().enabled = true;
        }
    }

    //slow motion dart
    public void ThrowDartFakeSlow()
    {
        GetComponent<GameController>().TouchPad.SetActive(false);
        waitCameraAnim = true;
        hand.GetComponent<Animation>().Stop();
        hand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/hand_2") as Sprite;
        hand.GetComponent<Animation>().Play("HandThrow");
        if (Time.timeScale == 1.0f)
            Time.timeScale = 0.5f;
        currentDart.transform.GetChild(1).transform.GetChild(1).gameObject.SetActive( true);
        GetComponent<AudioSource>().PlayOneShot(GetComponent<SoundController>().seList[3]);
        clickInside = false;
        currentDart.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        currentDart.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = true;
        currentDart.transform.GetChild(1).GetComponent<Rigidbody>().AddForce(direction * power);

    }
    // normal dart throw
    public void ThrowDartFake()
    {
        GetComponent<AudioSource>().PlayOneShot(GetComponent<SoundController>().seList[3]);
        hand.GetComponent<Animation>().Stop();
        hand.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/hand_2") as Sprite;
        hand.GetComponent<Animation>().Play("HandThrow");
        clickInside = false;
        currentDart.transform.GetChild(1).GetChild(2).gameObject.SetActive(true);
        currentDart.transform.GetChild(1).GetComponent<Rigidbody>().useGravity = true;
        currentDart.transform.GetChild(1).GetComponent<Rigidbody>().AddForce(direction * power);
    }

    //click screen
    public void Tap()
    {
        CalculateTouchPad();
        targetPos = Input.mousePosition;
        targetPos.z = 10;
        if (targetPos.x > TouchPadXMax || targetPos.x < TouchPadXMin || targetPos.y > TouchPadYMax || targetPos.y < TouchPadYMin)
        {
          
            clickInside = false;
            handPos = hand.transform.position;
            
            return;
        }
        else
        {
            clickInside = true;
        }

        if (currentMode == Mode.Aim)
        {
          
            Vector3 startPos = Camera.main.ScreenToWorldPoint(targetPos);
            startPos.y += 8;
            CreateDart(startPos);
            currentMode = Mode.PowerGauge;
            ShowGauge();
            powerGcoroutine = StartCoroutine(PowerFill());
        }
        else if (currentMode == Mode.PowerGauge)
        {
          
        }
        else if(currentMode== Mode.Throw)
        {
            //HandResetAnim();
           // currentMode = Mode.Aim;
            
        }
    }
    //show power gauge
    private void ShowGauge()
    {
        powerGauge.value = 0;
        powerIsON = true;

    }


    //power gauge Up down calculation
    IEnumerator PowerFill()
    {
       
        while (powerIsON)
        {
            AimShake();
            powerGauge.value= (Mathf.Sin(Time.time *powerGaugeSpeed) *powerGauge.maxValue/2) +(powerGauge.maxValue / 2);
     
            yield return null;
        }
       
    }

   

    // Update is called once per frame
    void Update()
    {
        if (currentMode == Mode.Aim)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Tap();
            }
           
            if (clickInside)
            {
                if (Input.GetMouseButton(0))
                {
                    AimShake();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    ShotDart();
                    currentMode = Mode.Throw;
                    cursor.SetActive(false);
                   // Destroy(cursor);
                }
            }
         
        }
        else if (currentMode == Mode.PowerGauge)
        {
            if (clickInside)
            {
                if (Input.GetMouseButton(0))
                {
                    AimShake();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    ShotDart();
                    currentMode = Mode.Throw;
                    //Destroy(cursor);
                    cursor.SetActive(false);
                }
            }

        }
        else if (currentMode == Mode.Throw)
        {
            if (Input.GetMouseButtonDown(0))
            {
               // Tap();
            }
        }
        
      
        
    }
}
