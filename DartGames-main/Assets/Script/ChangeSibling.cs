using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSibling : MonoBehaviour
{
    public void SetAsLast(GameObject target)
    {
        target.transform.SetSiblingIndex(3);
    }

　　//ハイスコアを表示
    public void LoadHiScore()
    {
        transform.GetChild(0).GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("High301", 0).ToString();
        transform.GetChild(1).GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("High501", 0).ToString();
        transform.GetChild(2).GetComponentInChildren<Text>().text = PlayerPrefs.GetInt("HighC", 0).ToString();
        transform.GetChild(3).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetInt("HighJ1", 0).ToString();
        transform.GetChild(3).GetChild(1).GetComponent<Text>().text = PlayerPrefs.GetInt("HighJ2", 0).ToString();
    }
    private void Start()
    {
        LoadHiScore();
        gameObject.SetActive(false);
    }

}
